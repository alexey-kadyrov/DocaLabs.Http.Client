using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DocaLabs.Http.Client.Binding.ResponseDeserialization
{
    /// <summary>
    /// Defines helper methods to deserialize a web response. All public methods are thread safe.
    /// </summary>
    public class DefaultResponseReader
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
                new JsonResponseDeserializer(),
                new XmlResponseDeserializer(),
                new PlainTextResponseDeserializer()
            };
        }

        public virtual object Read(HttpResponse response, Type resultType)
        {
            if(resultType == null)
                throw new ArgumentNullException("resultType");

            var deserializer = resultType.GetCustomAttribute<ResponseDeserializationAttribute>(true);
            if (deserializer != null)
                return deserializer.Deserialize(response, resultType);

            var provider = FindProvider(response, resultType);
            if (provider != null)
                return provider.Deserialize(response, resultType);

            if (resultType == typeof(string))
                return response.AsString();

            if (resultType == typeof(byte[]))
                return response.AsByteArray();

            if (resultType == typeof(VoidType))
                return VoidType.Value;

            throw new HttpClientException(Resources.Text.cannot_figure_out_how_to_deserialize);
        }

        IResponseDeserialization FindProvider(HttpResponse response, Type resultType)
        {
            IList<IResponseDeserializationProvider> providers;

            lock (Locker)
            {
                providers = _providers;
            }

            return providers.FirstOrDefault(x => x.CanDeserialize(response, resultType));
        }
    }
}
