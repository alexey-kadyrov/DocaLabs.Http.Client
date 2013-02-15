using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace DocaLabs.Http.Client.ResponseDeserialization
{
    /// <summary>
    /// Defines helper methods to deserialize a web response. All public methods are thread safe.
    /// </summary>
    public static class ResponseParser
    {
        static readonly object Locker;
        static IList<IResponseDeserializationProvider> _providers;

        /// <summary>
        /// Gets or sets the list of deserialization providers.
        /// </summary>
        public static IList<IResponseDeserializationProvider> Providers
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

        static ResponseParser()
        {
            Locker = new object();

            _providers = new List<IResponseDeserializationProvider>
            {
                new JsonResponseDeserializer(),
                new XmlResponseDeserializer(),
                new PlainTextResponseDeserializer(),
                new ImageResponseDeserializer()
            };
        }

        /// <summary>
        /// Gets the web response and tries to deserialize the response.
        ///     1. To get ResponseDeserializationAttribute if defined on TResult class.
        ///     2. Tries to find deserialization provider among the registered.
        /// </summary>
        public static object Parse(WebRequest request, Type resultType)
        {
            if(request == null)
                throw new ArgumentNullException("request");

            using (var response = new HttpResponse(request))
            {
                return TransformResult(response, resultType);
            }
        }

        static object TransformResult(HttpResponse response, Type resultType)
        {
            if(resultType == null)
                throw new ArgumentNullException("resultType");

            var deserializer = resultType.GetCustomAttribute(typeof(ResponseDeserializationAttribute), true);
            if (deserializer != null)
                return ((IResponseDeserialization)deserializer).Deserialize(response, resultType);

            if (resultType == typeof (VoidType))
                return VoidType.Value;

            var provider = FindProvider(response, resultType);
            if (provider != null)
                return provider.Deserialize(response, resultType);

            throw new UnrecoverableHttpClientException(Resources.Text.cannot_figure_out_how_to_deserialize);
        }

        static IResponseDeserialization FindProvider(HttpResponse response, Type resultType)
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
