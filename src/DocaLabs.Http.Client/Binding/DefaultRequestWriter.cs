using System;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Serialization;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Default request writer.
    /// </summary>
    public class DefaultRequestWriter
    {
        /// <summary>
        /// Writes data to the request's body or sets the content length to zero if the model cannot be serialized.
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

        /// <summary>
        /// Tries to figure out what HTTP verb should be used based on http client and model information.
        /// It checks for RequestSerializationAttribute on the model or client types or on any property of the model.
        /// </summary>
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
            return GetSerializer(httpClient, model) != null;
        }

        static IRequestSerialization GetSerializer(object httpClient, object model)
        {
            if (model == null)
                return null;

            return TryModelPropertyLevel(model) 
                ?? TryModelClassLevel(model) 
                ?? TryHttpClientClassLevel(httpClient);
        }

        static IRequestSerialization TryModelClassLevel(object query)
        {
            return query.GetType().GetCustomAttribute<RequestSerializationAttribute>(true);
        }

        static IRequestSerialization TryModelPropertyLevel(object query)
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var property in query.GetType().GetProperties())
            {
                var serializer = property.TryGetRequestSerializer();
                if (serializer != null)
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
