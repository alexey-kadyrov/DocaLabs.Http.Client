﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines helper methods to deserialize a web response. All public methods are thread safe.
    /// </summary>
    public class DefaultResponseBinder : IResponseBinder
    {
        readonly object _locker;
        readonly ConcurrentDictionary<Type, Type> _responseTypes;
        IList<IResponseDeserializationProvider> _providers;

        /// <summary>
        /// Gets or sets the list of deserialization providers.
        /// The setter and getter are thread safe.
        /// </summary>
        public IList<IResponseDeserializationProvider> Providers
        {
            get
            {
                IList<IResponseDeserializationProvider> providers;

                lock (_locker)
                {
                    providers = _providers;
                }

                return providers.ToList();
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                var providers = value.ToList();

                lock (_locker)
                {
                    _providers = providers;
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the DefaultResponseBinder class with Json, Xml, and plain text deserializers.
        /// </summary>
        public DefaultResponseBinder()
        {
            _locker = new object();
            _responseTypes = new ConcurrentDictionary<Type, Type>();
            _providers = new List<IResponseDeserializationProvider>
            {
                new DeserializeFromJsonAttribute(),
                new DeserializeFromXmlAttribute(),
                new DeserializeFromPlainTextAttribute()
            };
        }

        /// <summary>
        /// Reads the response stream and returns an object if there is anything there.
        /// For deserialization it:
        ///     * Checks for ResponseDeserializationAttribute on the output model type
        ///     * Checks for ResponseDeserializationAttribute on the http client type
        ///     * Checks whenever it can use the registered deserializers
        ///     * If the output model is string return it as the string
        ///     * If the output model is byte array return it as byte array
        ///     * If the output model is Stream or HttpResponseStream returns directly the stream - it'll be the caller responsibility to dispose it.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <param name="request">The WebRequest object.</param>
        /// <param name="resultType">Expected type for the return value.</param>
        /// <returns>Return value from the stream or null.</returns>
        public virtual object Read(BindingContext context, WebRequest request, Type resultType)
        {
            if(context == null)
                throw new ArgumentNullException("context");

            if(resultType == null)
                throw new ArgumentNullException("resultType");

            var responseType = _responseTypes.GetOrAdd(resultType, t => t.TryGetWrappedResponseModelType() ?? t);

            HttpResponseStream stream = null;

            try
            {
                stream = new HttpResponseStream(request);

                var value = ReadStream(context, stream, responseType);

                object richResponse = null;

                if (responseType != resultType)
                    richResponse = Activator.CreateInstance(resultType, stream.Response, value);

                if (value.Equals(stream))
                    stream = null;

                return richResponse ?? value;
            }
            catch (WebException e)
            {
                if (Is3XX(e) && responseType != resultType)
                    return Activator.CreateInstance(resultType, e.Response, null);

                throw;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }

        static bool Is3XX(WebException e)
        {
            var httpResponse = e.Response as HttpWebResponse;
            return (httpResponse != null && ((int)httpResponse.StatusCode) >= 300 && ((int)httpResponse.StatusCode) < 400);
        }

        object ReadStream(BindingContext context, HttpResponseStream responseStream, Type resultType)
        {
            var deserializer = resultType.GetCustomAttribute<ResponseDeserializationAttribute>(true);
            if (deserializer != null)
                return deserializer.Deserialize(responseStream, resultType);

            if (context.HttpClient != null)
            {
                deserializer = context.HttpClient.GetType().GetCustomAttribute<ResponseDeserializationAttribute>(true);
                if (deserializer != null)
                    return deserializer.Deserialize(responseStream, resultType);
            }

            if (resultType == typeof(Stream) || resultType == typeof(HttpResponseStream))
                return responseStream;

            if (resultType == typeof(VoidType))
            {
                SpinStream(responseStream);
                return VoidType.Value;
            }

            var provider = FindProvider(responseStream, resultType);
            if (provider != null)
                return provider.Deserialize(responseStream, resultType);

            if (resultType == typeof(string))
                return responseStream.AsString();

            if (resultType == typeof(byte[]))
                return responseStream.AsByteArray();

            throw new HttpClientException(Resources.Text.cannot_figure_out_how_to_deserialize);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void SpinStream(Stream responseStream)
        {
            var buffer = new byte[1024];
            while (responseStream.Read(buffer, 0, buffer.Length) == buffer.Length)
            {
            }
        }

        IResponseDeserialization FindProvider(HttpResponseStream responseStream, Type resultType)
        {
            IList<IResponseDeserializationProvider> providers;

            lock (_locker)
            {
                providers = _providers;
            }

            return providers.FirstOrDefault(x => x.CanDeserialize(responseStream, resultType));
        }
    }
}
