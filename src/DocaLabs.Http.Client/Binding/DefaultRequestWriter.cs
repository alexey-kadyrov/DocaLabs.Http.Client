using System;
using System.Linq;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.RequestSerialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultRequestWriter
    {
        /// <summary>
        /// Looks for IRequestSerialization on model or client level.
        /// Looks for RequestSerializationAttribute descendants defined on:
        ///     1. input model class level
        ///     2. One of it's properties
        ///     3. HttpClient level
        /// </summary>
        public void Write(object httpClient, object model, WebRequest request)
        {
            if (httpClient == null)
                throw new ArgumentNullException("httpClient");

            var serializer = GetSerializer(httpClient, model);
            if (serializer != null)
                serializer.Serialize(model, request);
            else
                request.SetContentLengthToZeroIfBodyIsRequired();
        }

        public string InferRequestMethod(object httpClient, object model)
        {
            if (httpClient == null)
                throw new ArgumentNullException("httpClient");

            return ShouldWrite(httpClient, model)
                ? WebRequestMethods.Http.Post
                : WebRequestMethods.Http.Get;
        }

        static bool ShouldWrite(object httpClient, object model)
        {
            if (model == null)
                return false;

            var modelType = model.GetType();

            return modelType.GetCustomAttribute<RequestSerializationAttribute>(true) != null
                   || httpClient.GetType().GetCustomAttribute<RequestSerializationAttribute>(true) != null
                   || modelType.GetAllPublicInstanceProperties().Any(x => x.IsRequestStream());
        }

        static IRequestSerialization GetSerializer(object httpClient, object model)
        {
            if (model == null)
                return null;

            var serializer = TryQueryClassLevel(model);
            if (serializer != null)
                return serializer;

            return TryQueryPropertyLevel(model) ?? TryHttpClientClassLevel(httpClient);
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
