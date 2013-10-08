using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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

            var info = GetSerializer(client, model);
            if (info != null && info.ValueToBeSerialized != null)
                info.Serializer.Serialize(info.ValueToBeSerialized, request);
            else
                request.SetContentLengthToZeroIfBodyIsRequired();
        }

        /// <summary>
        /// Writes data to the request's body or sets the content length to zero if the model cannot be serialized.
        /// Looks for RequestSerializationAttribute descendants defined on:
        ///     1. input model class level
        ///     2. One of it's properties
        ///     3. HttpClient level
        /// </summary>
        public Task WriteAsync(object client, object model, WebRequest request, CancellationToken cancellationToken)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            var info = GetSerializer(client, model);
            if (info == null || info.ValueToBeSerialized == null)
                return Task.Run(() => request.SetContentLengthToZeroIfBodyIsRequired(), cancellationToken);

            var asyncSerializer = info.Serializer as IAsyncRequestSerialization;
            return asyncSerializer != null 
                ? asyncSerializer.SerializeAsync(info.ValueToBeSerialized, request, cancellationToken) 
                : Task.Run(() => info.Serializer.Serialize(info.ValueToBeSerialized, request), cancellationToken);
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

        static SerializerInfo GetSerializer(object client, object model)
        {
            if (model == null)
                return null;

            return TryModelPropertyLevel(model) 
                ?? TryModelClassLevel(model) 
                ?? TryHttpClientClassLevel(client, model)
                ?? TryAsStream(model);
        }

        static SerializerInfo TryModelClassLevel(object model)
        {
            var serializer = model.GetType().GetCustomAttribute<RequestSerializationAttribute>(true);
            return serializer == null 
                ? null 
                : new SerializerInfo(serializer, model);
        }

        static SerializerInfo TryModelPropertyLevel(object model)
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var property in model.GetType().GetProperties())
            {
                var serializer = property.TryGetRequestSerializer();
                if (serializer != null)
                    return new SerializerInfo(serializer, model, property);
            }

            return null;
            // ReSharper restore LoopCanBeConvertedToQuery
        }

        static SerializerInfo TryHttpClientClassLevel(object client, object model)
        {
            var serializer = client.GetType().GetCustomAttribute<RequestSerializationAttribute>(true);

            return serializer == null
                ? null
                : new SerializerInfo(serializer, model);
        }

        static SerializerInfo TryAsStream(object model)
        {
            var stream = model as Stream;
            return stream != null
                ? new SerializerInfo(new SerializeStreamAttribute(), model)
                : null;
        }

        class SerializerInfo
        {
            public IRequestSerialization Serializer { get; private set; }
            public object ValueToBeSerialized { get; private set; }

            public SerializerInfo(IRequestSerialization serializer, object model)
            {
                Serializer = serializer;
                ValueToBeSerialized = model;
            }

            public SerializerInfo(IRequestSerialization serializer, object model, PropertyInfo property)
            {
                Serializer = serializer;
                ValueToBeSerialized = property.GetValue(model);
            }
        }
    }
}
