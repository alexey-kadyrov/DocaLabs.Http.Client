using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Default request writer.
    /// </summary>
    public class DefaultRequestWriter
    {
        static readonly IRequestSetup RequestSetup = PlatformAdapter.Resolve<IRequestSetup>();

        /// <summary>
        /// Writes data to the request's body or sets the content length to zero if the model cannot be serialized.
        /// Looks for RequestSerializationAttribute descendants defined on:
        ///     1. input model class level
        ///     2. One of it's properties
        ///     3. HttpClient level
        /// </summary>
        public void Write(BindingContext context, WebRequest request)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var info = GetSerializer(context);
            if (info != null && info.ValueToBeSerialized != null)
                info.Serializer.Serialize(context, request, info.ValueToBeSerialized);
            else
                RequestSetup.SetContentLengthToZeroIfBodyIsRequired(request);
        }

        /// <summary>
        /// Writes data to the request's body or sets the content length to zero if the model cannot be serialized.
        /// Looks for RequestSerializationAttribute descendants defined on:
        ///     1. input model class level
        ///     2. One of it's properties
        ///     3. HttpClient level
        /// </summary>
        public Task WriteAsync(AsyncBindingContext context, WebRequest request)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var info = GetSerializer(context);

            if (info == null || info.ValueToBeSerialized == null)
            {
                RequestSetup.SetContentLengthToZeroIfBodyIsRequired(request);
                return TaskUtils.CompletedTask();
            }

            var asyncSerializer = info.Serializer as IAsyncRequestSerialization;
            return asyncSerializer != null
                ? asyncSerializer.SerializeAsync(context, request, info.ValueToBeSerialized)
                : TaskUtils.RunSynchronously(() => info.Serializer.Serialize(context, request, info.ValueToBeSerialized), context.CancellationToken);
        }

        /// <summary>
        /// Tries to figure out what HTTP verb should be used based on http client and model information.
        /// It checks for RequestSerializationAttribute on the model or client types or on any property of the model.
        /// </summary>
        public string InferRequestMethod(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            return ShouldWrite(context)
                ? "POST"
                : "GET";
        }

        static bool ShouldWrite(BindingContext context)
        {
            return GetSerializer(context) != null;
        }

        static SerializerInfo GetSerializer(BindingContext context)
        {
            var model = context.Model;

            if (model == null)
                return null;

            return TryModelPropertyLevel(model) 
                ?? TryModelClassLevel(model) 
                ?? TryHttpClientClassLevel(context.HttpClient, model)
                ?? TryAsStream(model);
        }

        static SerializerInfo TryModelClassLevel(object model)
        {
            var serializer = model.GetType().GetTypeInfo().GetCustomAttribute<RequestSerializationAttribute>(true);
            return serializer == null 
                ? null 
                : new SerializerInfo(serializer, model);
        }

        static SerializerInfo TryModelPropertyLevel(object model)
        {
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var property in model.GetType().GetRuntimeProperties())
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
            var serializer = client.GetType().GetTypeInfo().GetCustomAttribute<RequestSerializationAttribute>(true);

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
