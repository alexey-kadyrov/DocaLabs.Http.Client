using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Serialization;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines helper methods to deserialize a web response. All public methods are thread safe.
    /// </summary>
    public class DefaultResponseReader : IResponseReader
    {
        readonly object Locker;
        IList<IResponseDeserializationProvider> _providers;

        /// <summary>
        /// Gets or sets the list of deserialization providers.
        /// </summary>
        public IList<IResponseDeserializationProvider> Providers
        {
            get
            {
                IList<IResponseDeserializationProvider> providers;

                lock (Locker)
                {
                    providers = _providers;
                }

                return providers.ToList();
            }

            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                var providers = value.ToList();

                lock (Locker)
                {
                    _providers = providers;
                }
            }
        }

        public DefaultResponseReader()
        {
            Locker = new object();

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

            var responseStream = new HttpResponseStream(request);

            try
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

                if (resultType == typeof (string))
                    return responseStream.AsString();

                if (resultType == typeof (byte[]))
                    return responseStream.AsByteArray();

                if (resultType == typeof (Stream))
                {
                    var ret = responseStream;
                    responseStream = null;
                    return ret;
                }

                if (resultType == typeof (VoidType))
                    return VoidType.Value;
            }
            finally
            {
                if (responseStream != null)
                    responseStream.Dispose();
            }

            throw new HttpClientException(Resources.Text.cannot_figure_out_how_to_deserialize);
        }

        IResponseDeserialization FindProvider(HttpResponseStream responseStream, Type resultType)
        {
            IList<IResponseDeserializationProvider> providers;

            lock (Locker)
            {
                providers = _providers;
            }

            return providers.FirstOrDefault(x => x.CanDeserialize(responseStream, resultType));
        }
    }
}
