using System;
using System.IO;
using System.Net;
using System.Text;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using DocaLabs.Http.Client.Utils.JsonSerialization;

namespace DocaLabs.Http.Client.Binding.RequestSerialization
{
    /// <summary>
    /// Serializes a given object into the web request in json format.
    /// </summary>
    public class SerializeAsJsonAttribute : RequestSerializationAttribute
    {
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
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");

                _charSet = value;
            }
        }

        /// <summary>
        /// Initializes an instance of the SerializeAsJsonAttribute class.
        /// </summary>
        public SerializeAsJsonAttribute()
        {
            CharSet = CharSets.Utf8;
        }

        /// <summary>
        /// Serializes a given object into the web request in json format
        /// </summary>
        /// <param name="obj">Object to be serialized.</param>
        /// <param name="request">Web request where to serialize to.</param>
        public override void Serialize(object obj, WebRequest request)
        {
            if(request == null)
                throw new ArgumentNullException("request");

            var encoding = GetEncoding();

            var data = encoding.GetBytes(obj == null ? "" : JsonSerializationProvider.Serializer.Serialize(obj));

            request.ContentType = string.Format("application/json; charset={0}", CharSet);
            
            if(string.IsNullOrWhiteSpace(RequestContentEncoding))
                Write(data, request);
            else
                CompressAndWrite(data, request);
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
