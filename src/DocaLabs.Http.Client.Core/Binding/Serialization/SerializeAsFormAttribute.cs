using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
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
        public override void Serialize(BindingContext context, WebRequest request, object value)
        {
            if(context == null)
                throw new ArgumentNullException("context");

            if (request == null)
                throw new ArgumentNullException("request");

            if(value == null)
                return;

            var form = ToForm(value);

            var encoding = GetEncoding();

            var data = encoding.GetBytes(form);

            request.ContentType = string.Format("application/x-www-form-urlencoded; charset={0}", CharSet);

            if (string.IsNullOrWhiteSpace(RequestContentEncoding))
                Write(context, request, data);
            else
                CompressAndWrite(context, request, data);
        }

        /// <summary>
        /// Asynchronously serializes a given object into the web request as Url encoded form (the content type is: application/x-www-form-urlencoded).
        /// </summary>
        public override Task SerializeAsync(AsyncBindingContext context, WebRequest request, object value)
        {
            if(context == null)
                throw new ArgumentNullException("context");

            if (request == null)
                throw new ArgumentNullException("request");

            if (value == null)
                return TaskUtils.CompletedTask();

            var form = ToForm(value);

            var encoding = GetEncoding();

            var data = encoding.GetBytes(form);

            request.ContentType = string.Format("application/x-www-form-urlencoded; charset={0}", CharSet);

            return string.IsNullOrWhiteSpace(RequestContentEncoding)
                ? WriteAsync(context, request, data)
                : CompressAndWriteAsync(context, request, data);
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

        static void Write(BindingContext context, WebRequest request, byte[] data)
        {
            using (var stream = RequestStreamFactory.Get(context, request))
            {
                stream.Write(data, 0, data.Length);
            }
        }

        static async Task WriteAsync(AsyncBindingContext context, WebRequest request, byte[] data)
        {
            using (var stream = await RequestStreamFactory.GetAsync(context, request))
            {
                await stream.WriteAsync(data, 0, data.Length, context.CancellationToken);
            }
        }

        void CompressAndWrite(BindingContext context, WebRequest request, byte[] data)
        {
            if (ContentEncoderFactory == null)
                throw new PlatformNotSupportedException(Resources.Text.content_encoding_is_not_supported);

            request.Headers[StandardHeaders.ContentEncoding] = RequestContentEncoding;

            using (var requestStream = RequestStreamFactory.Get(context, request))
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            using (var dataStream = new MemoryStream(data))
            {
                dataStream.CopyTo(compressionStream);
            }
        }

        async Task CompressAndWriteAsync(AsyncBindingContext context, WebRequest request, byte[] data)
        {
            if (ContentEncoderFactory == null)
                throw new PlatformNotSupportedException(Resources.Text.content_encoding_is_not_supported);

            request.Headers[StandardHeaders.ContentEncoding] = RequestContentEncoding;

            using (var requestStream = await RequestStreamFactory.GetAsync(context, request))
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            using (var dataStream = new MemoryStream(data))
            {
                context.CancellationToken.ThrowIfCancellationRequested();
                await dataStream.CopyToAsync(compressionStream);
            }
        }
    }
}
