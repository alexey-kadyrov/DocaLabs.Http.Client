using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Serializes a given object into the web request as Url encoded form (the content type is: application/x-www-form-urlencoded).
    /// </summary>
    public class SerializeAsFormAttribute : RequestSerializationAttribute
    {
        readonly static IRequestStreamFactory RequestStreamFactory = PlatformAdapter.Resolve<IRequestStreamFactory>();
        readonly static IContentEncoderFactory ContentEncoderFactory = PlatformAdapter.Resolve<IContentEncoderFactory>(false);
        readonly static PropertyMaps Maps = new PropertyMaps();

        string _charSet;

        /// <summary>
        /// Gets or sets the content encoding, if ContentEncoding blank or null no encoding is done.
        /// The encoder is supplied by ContentEncoderFactory.
        /// </summary>
        public string RequestContentEncoding { get; set; }

        /// <summary>
        /// Gets or sets the type of text encoding to be used for Xml serialization. The default value is UTF-8.
        /// </summary>
        public string CharSet
        {
            get { return _charSet; }
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");

                _charSet = value;
            }
        }

        /// <summary>
        /// Initializes an instance of the SerializeAsFormAttribute class.
        /// </summary>
        public SerializeAsFormAttribute()
        {
            CharSet = CharSets.Utf8;
        }

        /// <summary>
        /// Serializes a given object into the web request as Url encoded form (the content type is: application/x-www-form-urlencoded).
        /// </summary>
        public override void Serialize(object model, WebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var form = ToForm(model);

            var encoding = GetEncoding();

            var data = encoding.GetBytes(form);

            request.ContentType = string.Format("application/x-www-form-urlencoded; charset={0}", CharSet);

            if (string.IsNullOrWhiteSpace(RequestContentEncoding))
                Write(data, request);
            else
                CompressAndWrite(data, request);
        }

        /// <summary>
        /// Asynchronously serializes a given object into the web request as Url encoded form (the content type is: application/x-www-form-urlencoded).
        /// </summary>
        public override Task SerializeAsync(object model, WebRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var form = ToForm(model);

            var encoding = GetEncoding();

            var data = encoding.GetBytes(form);

            request.ContentType = string.Format("application/x-www-form-urlencoded; charset={0}", CharSet);

            return string.IsNullOrWhiteSpace(RequestContentEncoding) 
                ? WriteAsync(data, request, cancellationToken) 
                : CompressAndWriteAsync(data, request, cancellationToken);
        }

        /// <summary>
        /// Returns true if the property can be used in form serialization.
        /// </summary>
        public static bool IsFormProperty(PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            return info.GetIndexParameters().Length == 0 &&
                info.GetMethod != null &&
                info.PropertyType.IsSimpleType();
        }

        static string ToForm(object model)
        {
            if (model == null)
                return "";

            var values = Maps.Convert(model, IsFormProperty);

            return new QueryStringBuilder().Add(values).ToString();
        }

        Encoding GetEncoding()
        {
            try
            {
                return Encoding.GetEncoding(CharSet);
            }
            catch (Exception e)
            {
                throw new HttpClientException(e.Message, e);
            }
        }

        static void Write(byte[] data, WebRequest request)
        {
            using (var stream = RequestStreamFactory.Get(request))
            {
                stream.Write(data, 0, data.Length);
            }
        }

        static async Task WriteAsync(byte[] data, WebRequest request, CancellationToken cancellationToken)
        {
            using (var stream = await RequestStreamFactory.GetAsync(request))
            {
                await stream.WriteAsync(data, 0, data.Length, cancellationToken);
            }
        }

        void CompressAndWrite(byte[] data, WebRequest request)
        {
            if (ContentEncoderFactory == null)
                throw new PlatformNotSupportedException(Resources.Text.content_encoding_is_not_supported);

            request.Headers["content-encoding"] = RequestContentEncoding;

            using (var requestStream = RequestStreamFactory.Get(request))
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            using (var dataStream = new MemoryStream(data))
            {
                dataStream.CopyTo(compressionStream);
            }
        }

        async Task CompressAndWriteAsync(byte[] data, WebRequest request, CancellationToken cancellationToken)
        {
            if (ContentEncoderFactory == null)
                throw new PlatformNotSupportedException(Resources.Text.content_encoding_is_not_supported);

            request.Headers["content-encoding"] = RequestContentEncoding;

            using (var requestStream = await RequestStreamFactory.GetAsync(request))
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            using (var dataStream = new MemoryStream(data))
            {
                cancellationToken.ThrowIfCancellationRequested();
                await dataStream.CopyToAsync(compressionStream);
            }
        }
    }
}
