using System;
using System.IO;
using System.Net;
using System.Text;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Serializes a given object into the web request as text.
    /// </summary>
    public class SerializeAsTextAttribute : RequestSerializationAttribute
    {
        string _charSet;
        string _contentType;

        /// <summary>
        /// Gets or sets the content encoding, if ContentEncoding blank or null no encoding is done.
        /// The encoder is supplied by ContentEncoderFactory.
        /// </summary>
        public string RequestContentEncoding { get; set; }

        /// <summary>
        /// Gets or sets the type of text encoding to be used for serialization. The default value is UTF-8.
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
        /// Gets or sets content type. The default value is 'text/plain'
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
        /// </summary>
        public SerializeAsTextAttribute()
        {
            CharSet = CharSets.Utf8;
            ContentType = "text/plain";
        }

        /// <summary>
        /// Serializes a given object into the web request.
        /// </summary>
        /// <param name="obj">Object to be serialized.</param>
        /// <param name="request">Web request where to serialize to.</param>
        public override void Serialize(object obj, WebRequest request)
        {
            if(request == null)
                throw new ArgumentNullException("request");

            request.ContentType = string.Format("{0}; charset={1}", ContentType, CharSet);
            
            if(string.IsNullOrWhiteSpace(RequestContentEncoding))
                Write(obj, request);
            else
                CompressAndWrite(obj, request);
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

        void Write(object obj, WebRequest request)
        {
            using (var requestStream = request.GetRequestStream())
            {
                var s = obj as Stream;
                if (s != null)
                {
                    s.CopyTo(requestStream);
                }
                else
                {
                    var data = GetData(obj);
                    requestStream.Write(data, 0, data.Length);
                }
            }
        }

        void CompressAndWrite(object obj, WebRequest request)
        {
            request.Headers.Add(string.Format("content-encoding: {0}", RequestContentEncoding));

            using (var requestStream = request.GetRequestStream())
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            {
                var s = obj as Stream;
                if (s != null)
                {
                    s.CopyTo(compressionStream);
                }
                else
                {
                    using (var dataStream = new MemoryStream(GetData(obj)))
                    {
                        dataStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        byte[] GetData(object obj)
        {
            if(obj == null)
                return new byte[0];

            var encoding = GetEncoding();

            var a = obj as byte[];
            if (a != null)
                return a;

            var s = obj as string;
            if( s != null )
                return encoding.GetBytes(s);

            var type = obj.GetType();
            if (type.IsSimpleType())
                return encoding.GetBytes(CustomConverter.Current.ChangeType<string>(obj));

            throw new HttpClientException(string.Format(Resources.Text.object_must_be_of_string_byte_array_or_stream_type, type));
        }
    }
}