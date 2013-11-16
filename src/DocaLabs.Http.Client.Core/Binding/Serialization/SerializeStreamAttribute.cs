using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Serializes a given object into the web request as text.
    /// </summary>
    public class SerializeStreamAttribute : RequestSerializationAttribute
    {
        readonly static IRequestStreamFactory RequestStreamFactory = PlatformAdapter.Resolve<IRequestStreamFactory>();
        readonly static IContentEncoderFactory ContentEncoderFactory = PlatformAdapter.Resolve<IContentEncoderFactory>(false);

        string _contentType;

        /// <summary>
        /// Gets or sets the content encoding, if ContentEncoding blank or null no encoding is done.
        /// The encoder is supplied by ContentEncoderFactory.
        /// </summary>
        public string RequestContentEncoding { get; set; }

        /// <summary>
        /// Gets or sets content type. The default value is 'application/octet-stream'
        /// </summary>
        public string ContentType
        {
            get { return _contentType; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");

                _contentType = value;
            }
        }

        /// <summary>
        /// Initializes an instance of the SerializeAsJsonAttribute class.
        /// The instance will serialize the object itself in the Write method.
        /// </summary>
        public SerializeStreamAttribute()
        {
            ContentType = "application/octet-stream";
        }

        /// <summary>
        /// Serializes a given object into the web request.
        /// What actually will be serialized depends on which constructor was used - if the default then obj itself otherwise the property's value.
        /// </summary>
        public override void Serialize(BindingContext context, WebRequest request, object value)
        {
            if(context == null)
                throw new ArgumentNullException("context");

            if(request == null)
                throw new ArgumentNullException("request");

            if (value == null)
                return;

            var stream = value as Stream;
            if (stream == null)
                throw new ArgumentException(string.Format(Resources.Text.the_value_must_be_of_stream_type, value.GetType()), "value");

            request.ContentType = ContentType;
            
            if(string.IsNullOrWhiteSpace(RequestContentEncoding))
                Write(context, request, stream);
            else
                CompressAndWrite(context, request, stream);
        }

        /// <summary>
        /// Asynchronously serializes a given object into the web request.
        /// What actually will be serialized depends on which constructor was used - if the default then obj itself otherwise the property's value.
        /// </summary>
        public override Task SerializeAsync(AsyncBindingContext context, WebRequest request, object value)
        {
            if(context == null)
                throw new ArgumentNullException("context");

            if (request == null)
                throw new ArgumentNullException("request");

            if (value == null)
                return TaskUtils.CompletedTask();

            var stream = value as Stream;
            if (stream == null)
                throw new ArgumentException(string.Format(Resources.Text.the_value_must_be_of_stream_type, value.GetType()), "value");

            request.ContentType = ContentType;

            return string.IsNullOrWhiteSpace(RequestContentEncoding)
                ? WriteAsync(context, request, stream)
                : CompressAndWriteAsync(context, request, stream);
        }

        static void Write(BindingContext context, WebRequest request, Stream stream)
        {
            using (var requestStream = RequestStreamFactory.Get(context, request))
            {
                stream.CopyTo(requestStream);
            }
        }

        static async Task WriteAsync(AsyncBindingContext context, WebRequest request, Stream stream)
        {
            using (var requestStream = await RequestStreamFactory.GetAsync(context, request))
            {
                context.CancellationToken.ThrowIfCancellationRequested();
                await stream.CopyToAsync(requestStream);
            }
        }

        void CompressAndWrite(BindingContext context, WebRequest request, Stream stream)
        {
            if (ContentEncoderFactory == null)
                throw new PlatformNotSupportedException(Resources.Text.content_encoding_is_not_supported);

            request.Headers[StandardHeaders.ContentEncoding] = RequestContentEncoding;

            using (var requestStream = RequestStreamFactory.Get(context, request))
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            {
                stream.CopyTo(compressionStream);
            }
        }

        async Task CompressAndWriteAsync(AsyncBindingContext context, WebRequest request, Stream stream)
        {
            if (ContentEncoderFactory == null)
                throw new PlatformNotSupportedException(Resources.Text.content_encoding_is_not_supported);

            request.Headers[StandardHeaders.ContentEncoding] = RequestContentEncoding;

            using (var requestStream = await RequestStreamFactory.GetAsync(context, request))
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            {
                context.CancellationToken.ThrowIfCancellationRequested();
                await stream.CopyToAsync(compressionStream);
            }
        }
    }
}