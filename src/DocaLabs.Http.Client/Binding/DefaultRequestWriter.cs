using System;
using System.IO;
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
        public void Write(object client, object model, WebRequest request)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            var serializer = GetSerializer(client, model);
            if (serializer != null)
                serializer.Serialize(model, request);
            else
                request.SetContentLengthToZeroIfBodyIsRequired();
        }

        /// <summary>
        /// Tries to figure out what HTTP verb should be used based on http client and model information.
        /// It checks for RequestSerializationAttribute on the model or client types or on any property of the model.
        /// </summary>
        public string InferRequestMethod(object client, object model)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            return ShouldWrite(client, model)
                ? WebRequestMethods.Http.Post
                : WebRequestMethods.Http.Get;
        }

        static bool ShouldWrite(object client, object model)
        {
            return GetSerializer(client, model) != null;
        }

        static IRequestSerialization GetSerializer(object client, object model)
        {
            if (model == null)
                return null;

            return TryModelPropertyLevel(model) 
                ?? TryModelClassLevel(model) 
                ?? TryHttpClientClassLevel(client)
                ?? TryAsStream(model);
        }

        static IRequestSerialization TryModelClassLevel(object model)
        {
            return model.GetType().GetCustomAttribute<RequestSerializationAttribute>(true);
        }

        static IRequestSerialization TryModelPropertyLevel(object model)
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var property in model.GetType().GetProperties())
            {
                var serializer = property.TryGetRequestSerializer();
                if (serializer != null)
                    return serializer;
            }

            return null;
            // ReSharper restore LoopCanBeConvertedToQuery
        }

        static IRequestSerialization TryHttpClientClassLevel(object client)
        {
            return client.GetType().GetCustomAttribute<RequestSerializationAttribute>(true);
        }

        static IRequestSerialization TryAsStream(object model)
        {
            var stream = model as Stream;
            return stream != null
                ? new SerializeStreamAttribute()
                : null;
        }
    }
}
