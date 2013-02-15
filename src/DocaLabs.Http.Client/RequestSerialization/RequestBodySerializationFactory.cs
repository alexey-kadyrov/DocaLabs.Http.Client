using System;
using System.Reflection;

namespace DocaLabs.Http.Client.RequestSerialization
{
    /// <summary>
    /// Defines methods to get IRequestSerialization for an http client and query. All public methods are thread safe.
    /// </summary>
    public static class RequestBodySerializationFactory
    {
        /// <summary>
        /// Gets IRequestSerialization for an http client and query.
        /// Looks for RequestSerializationAttribute descendants defined on:
        ///     1. Query class level
        ///     2. One of it's properties
        ///     3. HttpClient level
        /// </summary>
        public static IRequestSerialization GetSerializer(object httpClient, object query)
        {
            if(httpClient == null)
                throw new ArgumentNullException("httpClient");

            if (query != null)
            {
                var serializer = TryQueryClassLevel(query);
                if (serializer != null)
                    return serializer;

                serializer = TryQueryPropertyLevel(query);
                if (serializer != null)
                    return serializer;
            }

            return TryHttpClientClassLevel(httpClient);
        }

        static IRequestSerialization TryQueryClassLevel(object query)
        {
            var serializer = query.GetType().GetCustomAttribute(typeof(RequestSerializationAttribute), true);

            return serializer == null 
                ? null 
                : serializer as IRequestSerialization;
        }

        static IRequestSerialization TryQueryPropertyLevel(object query)
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var property in query.GetType().GetProperties())
            {
                var serializer = property.GetCustomAttribute(typeof(RequestSerializationAttribute), true);
                if (serializer == null)
                    continue;

                return serializer as IRequestSerialization;
            }

            return null;
            // ReSharper restore LoopCanBeConvertedToQuery
        }

        static IRequestSerialization TryHttpClientClassLevel(object httpClient)
        {
            var serializer = httpClient.GetType().GetCustomAttribute(typeof(RequestSerializationAttribute), true);

            return serializer == null 
                ? null 
                : serializer as IRequestSerialization;
        }
    }
}
