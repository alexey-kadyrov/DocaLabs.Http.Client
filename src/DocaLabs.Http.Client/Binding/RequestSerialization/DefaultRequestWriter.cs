using System;
using System.Linq;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.RequestSerialization
{
    public class DefaultRequestWriter : IRequestWriter
    {
        /// <summary>
        /// Looks for IRequestSerialization on model or client level.
        /// Looks for RequestSerializationAttribute descendants defined on:
        ///     1. input model class level
        ///     2. One of it's properties
        ///     3. HttpClient level
        /// </summary>
        public void Write(object model, object client, WebRequest request)
        {
            var serializer = GetSerializer(model, client);
            if (serializer != null)
                serializer.Serialize(model, request);
            else
                request.SetContentLengthToZeroIfBodyIsRequired();
        }

        public bool ShouldWrite(object model)
        {
            if (model == null)
                return false;

            var modelType = model.GetType();

            return modelType.GetCustomAttribute<RequestSerializationAttribute>(true) != null
                   || GetType().GetCustomAttribute<RequestSerializationAttribute>(true) != null
                   || modelType.GetAllInstancePublicProperties().Any(x => x.GetCustomAttribute<RequestSerializationAttribute>(true) != null);
        }

        static IRequestSerialization GetSerializer(object model, object client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            if (model != null)
            {
                var serializer = TryQueryClassLevel(model);
                if (serializer != null)
                    return serializer;

                serializer = TryQueryPropertyLevel(model);
                if (serializer != null)
                    return serializer;
            }

            return TryHttpClientClassLevel(client);
        }

        static IRequestSerialization TryQueryClassLevel(object query)
        {
            return query.GetType().GetCustomAttribute<RequestSerializationAttribute>(true);
        }

        static IRequestSerialization TryQueryPropertyLevel(object query)
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var property in query.GetType().GetProperties())
            {
                var serializer = property.GetCustomAttribute<RequestSerializationAttribute>(true);
                if (serializer == null)
                    continue;

                return serializer;
            }

            return null;
            // ReSharper restore LoopCanBeConvertedToQuery
        }

        static IRequestSerialization TryHttpClientClassLevel(object httpClient)
        {
            return httpClient.GetType().GetCustomAttribute<RequestSerializationAttribute>(true);
        }
    }
}
