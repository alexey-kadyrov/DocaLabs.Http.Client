using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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

        public virtual object Read(BindingContext context, WebRequest request, Type resultType)
        {
            if(resultType == null)
                throw new ArgumentNullException("resultType");

            var responseType = _responseTypes.GetOrAdd(resultType, t => t.TryGetWrappedResponseModelType() ?? t);

            var responseStream = new HttpResponseStream(request);

            try
            {
                var value = ReadStream(context, responseStream, responseType);

                object responseWrapper = null;

                if (responseType != resultType)
                    responseWrapper = InitializeResponseWrapper(responseType, responseStream, value);

                if (value.Equals(responseStream))
                    responseStream = null;

                return responseWrapper ?? value;
            }
            finally
            {
                if (responseStream != null)
                    responseStream.Dispose();
            }
        }

        object ReadStream(BindingContext context, HttpResponseStream responseStream, Type resultType)
        {
            var deserializer = resultType.GetCustomAttribute<ResponseDeserializationAttribute>(true);
            if (deserializer != null)
                return deserializer.Deserialize(responseStream, resultType);

            deserializer = context.HttpClient.GetType().GetCustomAttribute<ResponseDeserializationAttribute>(true);
            if (deserializer != null)
                return deserializer.Deserialize(responseStream, resultType);

            var provider = FindProvider(responseStream, resultType);
            if (provider != null)
                return provider.Deserialize(responseStream, resultType);

            if (resultType == typeof(string))
                return responseStream.AsString();

            if (resultType == typeof(byte[]))
                return responseStream.AsByteArray();

            if (resultType == typeof(Stream))
                return responseStream;

            if (resultType == typeof(VoidType))
                return VoidType.Value;

            throw new HttpClientException(Resources.Text.cannot_figure_out_how_to_deserialize);
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

        static object InitializeResponseWrapper(Type responseType, HttpResponseStream stream, object value)
        {
            var status = stream.GetResponseStatus();
            return Activator.CreateInstance(responseType, status.StatusCode, status.StatusDescription, value);
        }
    }
}
