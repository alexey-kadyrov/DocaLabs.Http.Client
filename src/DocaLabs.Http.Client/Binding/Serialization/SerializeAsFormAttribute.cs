using System;
using System.IO;
using System.Net;
using System.Text;
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
        readonly static PropertyMaps Maps = new PropertyMaps(PropertyInfoExtensions.IsFormProperty);
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
        /// <param name="model">Object to be serialized.</param>
        /// <param name="request">Web request where to serialize to.</param>
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

        static string ToForm(object model)
        {
            if (model == null)
                return "";

            var values = Maps.Convert(model);

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
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }

        void CompressAndWrite(byte[] data, WebRequest request)
        {
            request.Headers.Add(string.Format("content-encoding: {0}", RequestContentEncoding));

            using (var requestStream = request.GetRequestStream())
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            using (var dataStream = new MemoryStream(data))
            {
                dataStream.CopyTo(compressionStream);
            }
        }
    }
}
