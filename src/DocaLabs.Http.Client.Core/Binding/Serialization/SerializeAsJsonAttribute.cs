using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using DocaLabs.Http.Client.Utils.JsonSerialization;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Serializes a given object into the web request in json format using UTF-8 encoding.
    /// </summary>
    public class SerializeAsJsonAttribute : RequestSerializationAttribute
    {
        readonly static IRequestStreamFactory RequestStreamFactory = PlatformAdapter.Resolve<IRequestStreamFactory>();
        readonly static IContentEncoderFactory ContentEncoderFactory = PlatformAdapter.Resolve<IContentEncoderFactory>(false);

        /// <summary>
        /// Gets or sets the content encoding, if ContentEncoding blank or null no encoding is done.
        /// The encoder is supplied by ContentEncoderFactory.
        /// </summary>
        public string RequestContentEncoding { get; set; }

        /// <summary>
        /// Serializes a given object into the web request in json format
        /// </summary>
        public override void Serialize(BindingContext context, WebRequest request, object value)
        {
            if(context == null)
                throw new ArgumentNullException("context");

            if(request == null)
                throw new ArgumentNullException("request");

            if(value == null)
                return;

            request.ContentType = "application/json";
            
            if(string.IsNullOrWhiteSpace(RequestContentEncoding))
                Write(context, request, value);
            else
                CompressAndWrite(context, request, value);
        }

        /// <summary>
        /// Asynchronously Serializes a given object into the web request in json format
        /// </summary>
        public override Task SerializeAsync(AsyncBindingContext context, WebRequest request, object value)
        {
            if(context == null)
                throw new ArgumentNullException();

            if (request == null)
                throw new ArgumentNullException("request");

            if (value == null)
                return TaskUtils.CompletedTask();

            request.ContentType = "application/json";

            return string.IsNullOrWhiteSpace(RequestContentEncoding) 
                ? WriteAsync(context, request, value) 
                : CompressAndWriteAsync(context, request, value);
        }

        static void Write(BindingContext context, WebRequest request, object value)
        {
            using (var requestStream = RequestStreamFactory.Get(context, request))
            {
                var s = value as Stream;
                if (s != null)
                {
                    s.CopyTo(requestStream);
                }
                else
                {
                    var data = GetData(value);
                    requestStream.Write(data, 0, data.Length);
                }
            }
        }

        static async Task WriteAsync(AsyncBindingContext context, WebRequest request, object value)
        {
            using (var requestStream = await RequestStreamFactory.GetAsync(context, request))
            {
                var s = value as Stream;
                if (s != null)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    await s.CopyToAsync(requestStream);
                }
                else
                {
                    var data = GetData(value);
                    await requestStream.WriteAsync(data, 0, data.Length, context.CancellationToken);
                }
            }
        }

        void CompressAndWrite(BindingContext context, WebRequest request, object value)
        {
            if (ContentEncoderFactory == null)
                throw new PlatformNotSupportedException(Resources.Text.content_encoding_is_not_supported);

            request.Headers[StandardHeaders.ContentEncoding] = RequestContentEncoding;

            using (var requestStream = RequestStreamFactory.Get(context, request))
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            {
                var s = value as Stream;
                if (s != null)
                {
                    s.CopyTo(compressionStream);
                }
                else
                {
                    using (var dataStream = new MemoryStream(GetData(value)))
                    {
                        dataStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        async Task CompressAndWriteAsync(AsyncBindingContext context, WebRequest request, object value)
        {
            if (ContentEncoderFactory == null)
                throw new PlatformNotSupportedException(Resources.Text.content_encoding_is_not_supported);

            request.Headers[StandardHeaders.ContentEncoding] = RequestContentEncoding;

            using (var requestStream = await RequestStreamFactory.GetAsync(context, request))
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                var s = value as Stream;
                if (s != null)
                {
                    await s.CopyToAsync(compressionStream);
                }
                else
                {
                    using (var dataStream = new MemoryStream(GetData(value)))
                    {
                        await dataStream.CopyToAsync(compressionStream);
                    }
                }
            }
        }

        static byte[] GetData(object obj)
        {
            var a = obj as byte[];
            if (a != null)
                return a;

            var s = obj as string;
            return s != null
                ? Encoding.UTF8.GetBytes(s)
                : Encoding.UTF8.GetBytes(obj == null ? "" : JsonSerializationProvider.Serializer.Serialize(obj));
        }
    }
}
