using System;
using System.IO;
using System.Net;
using DocaLabs.Http.Client.Utils.ContentEncoding;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Serializes a given object into the web request as text.
    /// </summary>
    public class SerializeStreamAttribute : RequestSerializationAttribute
    {
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
        /// <param name="obj">Object to be serialized.</param>
        /// <param name="request">Web request where to serialize to.</param>
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

        static void Write(Stream stream, WebRequest request)
        {
            using (var requestStream = request.GetRequestStream())
            {
                stream.CopyTo(requestStream);
            }
        }

        void CompressAndWrite(Stream stream, WebRequest request)
        {
            request.Headers.Add(string.Format("content-encoding: {0}", RequestContentEncoding));

            using (var requestStream = request.GetRequestStream())
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            {
                stream.CopyTo(compressionStream);
            }
        }
    }
}