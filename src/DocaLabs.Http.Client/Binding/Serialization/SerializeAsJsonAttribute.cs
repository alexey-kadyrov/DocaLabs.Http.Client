using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils.ContentEncoding;
using DocaLabs.Http.Client.Utils.JsonSerialization;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Serializes a given object into the web request in json format using UTF-8 encoding.
    /// </summary>
    public class SerializeAsJsonAttribute : RequestSerializationAttribute
    {
        /// <summary>
        /// Gets or sets the content encoding, if ContentEncoding blank or null no encoding is done.
        /// The encoder is supplied by ContentEncoderFactory.
        /// </summary>
        public string RequestContentEncoding { get; set; }

        /// <summary>
        /// Serializes a given object into the web request in json format
        /// </summary>
        public override void Serialize(object obj, WebRequest request)
        {
            if(request == null)
                throw new ArgumentNullException("request");

            request.ContentType = "application/json";
            
            if(string.IsNullOrWhiteSpace(RequestContentEncoding))
                Write(obj, request);
            else
                CompressAndWrite(obj, request);
        }

        /// <summary>
        /// Asynchronously Serializes a given object into the web request in json format
        /// </summary>
        public override Task SerializeAsync(object obj, WebRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            request.ContentType = "application/json";

            return string.IsNullOrWhiteSpace(RequestContentEncoding) 
                ? WriteAsync(obj, request, cancellationToken) 
                : CompressAndWriteAsync(obj, request, cancellationToken);
        }

        static void Write(object obj, WebRequest request)
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

        static async Task WriteAsync(object obj, WebRequest request, CancellationToken cancellationToken)
        {
            using (var requestStream = await request.GetRequestStreamAsync())
            {
                var s = obj as Stream;
                if (s != null)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await s.CopyToAsync(requestStream);
                }
                else
                {
                    var data = GetData(obj);
                    await requestStream.WriteAsync(data, 0, data.Length, cancellationToken);
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

        async Task CompressAndWriteAsync(object obj, WebRequest request, CancellationToken cancellationToken)
        {
            request.Headers.Add(string.Format("content-encoding: {0}", RequestContentEncoding));

            using (var requestStream = await request.GetRequestStreamAsync())
            using (var compressionStream = ContentEncoderFactory.Get(RequestContentEncoding).GetCompressionStream(requestStream))
            {
                cancellationToken.ThrowIfCancellationRequested();

                var s = obj as Stream;
                if (s != null)
                {
                    await s.CopyToAsync(compressionStream);
                }
                else
                {
                    using (var dataStream = new MemoryStream(GetData(obj)))
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
