using System;
using System.IO;
using System.Net;
using System.Threading;
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
        public override void Serialize(object obj, WebRequest request)
        {
            if(request == null)
                throw new ArgumentNullException("request");

            request.ContentType = ContentType;
            
            if (obj == null)
                return;

            var stream = obj as Stream;
            if (stream == null)
                throw new ArgumentException(string.Format(Resources.Text.the_value_must_be_of_stream_type, obj.GetType()), "obj");
            
            if(string.IsNullOrWhiteSpace(RequestContentEncoding))
                Write(stream, request);
            else
                CompressAndWrite(stream, request);
        }

        /// <summary>
        /// Asynchronously serializes a given object into the web request.
        /// What actually will be serialized depends on which constructor was used - if the default then obj itself otherwise the property's value.
        /// </summary>
        public override Task SerializeAsync(object obj, WebRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            request.ContentType = ContentType;

            if (obj == null)
                return TaskUtils.CompletedTask();

            var stream = obj as Stream;
            if (stream == null)
                throw new ArgumentException(string.Format(Resources.Text.the_value_must_be_of_stream_type, obj.GetType()), "obj");

            return string.IsNullOrWhiteSpace(RequestContentEncoding) 
                ? WriteAsync(stream, request, cancellationToken) 
                : CompressAndWriteAsync(stream, request, cancellationToken);
        }

        static void Write(Stream stream, WebRequest request)
        {
            using (var requestStream = RequestStreamFactory.Get(request))
            {
                stream.CopyTo(requestStream);
            }
        }

        static async Task WriteAsync(Stream stream, WebRequest request, CancellationToken cancellationToken)
        {
            using (var requestStream = await RequestStreamFactory.GetAsync(request))
            {
                cancellationToken.ThrowIfCancellationRequested();
                await stream.CopyToAsync(requestStream);
            }
        }

        void CompressAndWrite(Stream stream, WebRequest request)
        {
            if (ContentEncoderFactory == null)
                throw new PlatformNotSupportedException(Resources.Text.content_encoding_is_not_supported);

            request.Headers["content-encoding"] = RequestContentEncoding;

            using (var requestStream = RequestStreamFactory.Get(request))
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            {
                stream.CopyTo(compressionStream);
            }
        }

        async Task CompressAndWriteAsync(Stream stream, WebRequest request, CancellationToken cancellationToken)
        {
            if (ContentEncoderFactory == null)
                throw new PlatformNotSupportedException(Resources.Text.content_encoding_is_not_supported);

            request.Headers["content-encoding"] = RequestContentEncoding;

            using (var requestStream = await RequestStreamFactory.GetAsync(request))
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            {
                cancellationToken.ThrowIfCancellationRequested();
                await stream.CopyToAsync(compressionStream);
            }
        }
    }
}