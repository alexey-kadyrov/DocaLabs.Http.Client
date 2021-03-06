﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines helper methods to deserialize a web response. All public methods are thread safe.
    /// </summary>
    public class DefaultResponseBinder : IResponseBinder, IAsyncResponseBinder
    {
        static readonly IHttpResponseStreamFactory StreamFactory = PlatformAdapter.Resolve<IHttpResponseStreamFactory>();
        static readonly IStreamTypeChecker StreamTypeChecker = PlatformAdapter.Resolve<IStreamTypeChecker>();

        readonly CustomConcurrentDictionary<Type, Type> _responseTypes;
        IList<IResponseDeserializationProvider> _providers;

        /// <summary>
        /// Gets or sets the list of deserialization providers.
        /// The setter and getter are thread safe.
        /// </summary>
        public IList<IResponseDeserializationProvider> Providers
        {
            // reference assignment is thread safe

            get
            {
                var providers = _providers;

                return providers.ToList();
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                var providers = value.ToList();

                _providers = providers;
            }
        }

        /// <summary>
        /// Creates a new instance of the DefaultResponseBinder class with Json, Xml, and plain text deserializers.
        /// </summary>
        public DefaultResponseBinder()
        {
            _responseTypes = new CustomConcurrentDictionary<Type, Type>();
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
        /// <returns>Return value from the stream or null.</returns>
        public virtual object Read(BindingContext context, WebRequest request)
        {
            var responseType = GetResponseType(context);

            HttpResponseStreamCore stream = null;

            try
            {
                stream = StreamFactory.CreateStream(context, request);

                if (IsSafe3XX(stream.StatusCode))
                {
                    if (responseType != context.OutputModelType)
                        return Activator.CreateInstance(context.OutputModelType, stream.Response, null);
                    else
                        throw new HttpClientWebException(string.Format(Resources.Text.failed_execute_request, context.BaseUrl, context.HttpClient.GetType()), stream.Response);
                }

                var value = ReadStream(context, stream, responseType);

                object richResponse = null;

                if (responseType != context.OutputModelType)
                    richResponse = Activator.CreateInstance(context.OutputModelType, stream.Response, value);

                if (value.Equals(stream))
                    stream = null;

                return richResponse ?? value;
            }
            catch (WebException e)
            {
                if (IsSafe3XX(e) && responseType != context.OutputModelType)
                    return Activator.CreateInstance(context.OutputModelType, e.Response, null);

                throw;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }

        /// <summary>
        /// Asynchronously reads the response stream and returns an object if there is anything there.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <param name="request">The WebRequest object.</param>
        /// <returns>Return value from the stream or default value of T.</returns>
        public async Task<T> ReadAsync<T>(AsyncBindingContext context, WebRequest request)
        {
            var resultType = typeof(T);

            var responseType = GetResponseType(context);

            HttpResponseStreamCore stream = null;

            try
            {
                stream = await StreamFactory.CreateAsyncStream(context, request);

                if (IsSafe3XX(stream.StatusCode))
                {
                    if (responseType != resultType)
                        return (T) Activator.CreateInstance(resultType, stream.Response, null);
                    else
                        throw new HttpClientWebException(string.Format(Resources.Text.failed_execute_request, context.BaseUrl, context.HttpClient.GetType()), stream.Response);
                }

                var deserializer = GetUserSpecifiedDeserializer(context, responseType);
                if (deserializer == null)
                {
                    if (StreamTypeChecker.IsSupportedStream(responseType))
                    {
                        var retVal = stream;
                        stream = null;
                        return (T)MakeReturnValue(retVal, retVal, responseType, resultType);
                    }

                    if (responseType == typeof(VoidType))
                        return (T)MakeReturnValue(stream, VoidType.Value, responseType, resultType);

                    deserializer = FindProvider(stream, responseType);

                    if (deserializer == null)
                    {
                        if (responseType == typeof(string))
                            return await (Task<T>)(object)stream.AsStringAsync();

                        if (responseType == typeof(byte[]))
                            return await (Task<T>)(object)stream.AsByteArrayAsync(context.CancellationToken);

                        throw new HttpClientException(Resources.Text.cannot_figure_out_how_to_deserialize);
                    }
                }

                var asyncDeserializer = deserializer as IAsyncResponseDeserialization;

                var value = await (asyncDeserializer != null
                    ? asyncDeserializer.DeserializeAsync(stream, responseType, context.CancellationToken)
                    : Task.FromResult(deserializer.Deserialize(stream, responseType)));

                return (T)MakeReturnValue(stream, value, responseType, resultType);
            }
            catch (WebException e)
            {
                if (IsSafe3XX(e) && responseType != resultType)
                    return (T)Activator.CreateInstance(resultType, e.Response, null);

                throw;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }

        static object MakeReturnValue(HttpResponseStreamCore stream, object value, Type responseType, Type resultType)
        {
            object richResponse = null;

            if (responseType != resultType)
                richResponse = Activator.CreateInstance(resultType, stream.Response, value);

            return richResponse ?? value;
        }

        Type GetResponseType(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            return _responseTypes.GetOrAdd(context.OutputModelType, t => TryGetWrappedResponseModelType(t) ?? t);
        }

        static bool IsSafe3XX(WebException e)
        {
            var httpResponse = e.Response as HttpWebResponse;

            return httpResponse != null && IsSafe3XX((int)httpResponse.StatusCode);
        }

        static bool IsSafe3XX(int status)
        {
            return status == 301 || status == 302 || status == 303 || status == 304 || status == 307;
        }

        object ReadStream(BindingContext context, HttpResponseStreamCore responseStream, Type resultType)
        {
            var deserializer = resultType.GetTypeInfo().GetCustomAttribute<ResponseDeserializationAttribute>(true);
            if (deserializer != null)
                return deserializer.Deserialize(responseStream, resultType);

            if (context.HttpClient != null)
            {
                deserializer = context.HttpClient.GetType().GetTypeInfo().GetCustomAttribute<ResponseDeserializationAttribute>(true);
                if (deserializer != null)
                    return deserializer.Deserialize(responseStream, resultType);
            }

            if (StreamTypeChecker.IsSupportedStream(resultType))
                return responseStream;

            if (resultType == typeof(VoidType))
                return VoidType.Value;

            var provider = FindProvider(responseStream, resultType);
            if (provider != null)
                return provider.Deserialize(responseStream, resultType);

            if (resultType == typeof(string))
                return responseStream.AsString();

            if (resultType == typeof(byte[]))
                return responseStream.AsByteArray();

            throw new HttpClientException(Resources.Text.cannot_figure_out_how_to_deserialize);
        }

        static IResponseDeserialization GetUserSpecifiedDeserializer(BindingContext context, Type resultType)
        {
            var deserializer = resultType.GetTypeInfo().GetCustomAttribute<ResponseDeserializationAttribute>(true);
            if (deserializer != null)
                return deserializer;

            return context.HttpClient != null 
                ? context.HttpClient.GetType().GetTypeInfo().GetCustomAttribute<ResponseDeserializationAttribute>(true) 
                : null;
        }

        IResponseDeserialization FindProvider(HttpResponseStreamCore responseStream, Type resultType)
        {
            // reference assignment is thread safe

            var providers = _providers;

            return providers.FirstOrDefault(x => x.CanDeserialize(responseStream, resultType));
        }

        static Type TryGetWrappedResponseModelType(Type type)
        {
            while (true)
            {
                var typeInfo = type.GetTypeInfo();

                if (typeof (RichResponse).GetTypeInfo().IsAssignableFrom(typeInfo))
                {
                    var valueProperty = typeInfo.GetDeclaredProperty("Value");
                    if (valueProperty != null && !valueProperty.IsIndexer())
                        return valueProperty.PropertyType;
                }

                if (typeInfo.BaseType == null || typeInfo.BaseType == typeof (object))
                    return null;

                type = typeInfo.BaseType;
            }
        }
    }
}
