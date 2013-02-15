using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Machine.Specifications;
using Moq;
using Newtonsoft.Json;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class request_serialization_test_context
    {
        static MemoryStream request_data;
        protected static Mock<WebRequest> mock_web_request;

        Establish context = () =>
        {
            request_data = new MemoryStream();

            mock_web_request = new Mock<WebRequest>();
            mock_web_request.SetupAllProperties();
            mock_web_request.Setup(x => x.GetRequestStream()).Returns(request_data);
            mock_web_request.Object.Headers = new WebHeaderCollection();
        };

        Cleanup after_each =
            () => request_data.Dispose();

        protected static string GetDecodedRequestData()
        {
            // at this stage the request_data is already disposed
            using (var requestStream = new MemoryStream(request_data.ToArray()))
            using (var dataStream = new MemoryStream())
            using (var decomressionStream = new GZipStream(requestStream, CompressionMode.Decompress))
            {
                decomressionStream.CopyTo(dataStream);
                return Encoding.UTF8.GetString(dataStream.ToArray());
            }
        }

        protected static string GetRequestData(Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetString(request_data.ToArray());
        }

        protected static XDocument GetRequestDataAsXDocument()
        {
            return XDocument.Load(new MemoryStream(request_data.ToArray()));
        }

        protected static XDocument GetDecodedRequestDataAsXDocument()
        {
            // at this stage the request_data is already disposed
            using (var requestStream = new MemoryStream(request_data.ToArray()))
            using (var dataStream = new MemoryStream())
            using (var decomressionStream = new GZipStream(requestStream, CompressionMode.Decompress))
            {
                decomressionStream.CopyTo(dataStream);
                dataStream.Seek(0, SeekOrigin.Begin);
                return XDocument.Load(dataStream);
            }
        }

        protected static T ParseRequestDataAsJson<T>(Encoding encoding = null)
        {
            return JsonConvert.DeserializeObject<T>(GetRequestData(encoding));
        }

        protected static T ParseDecodedRequestDataAsJson<T>()
        {
            return JsonConvert.DeserializeObject<T>(GetDecodedRequestData());
        }

        protected static T ParseRequestDataAsXml<T>()
        {
            using (var stream = new MemoryStream(request_data.ToArray()))
            using (var reader = XmlReader.Create(stream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore }))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            }
        }

        protected static T ParseDecodedRequestDataAsXml<T>()
        {
            // at this stage the request_data is already disposed
            using (var requestStream = new MemoryStream(request_data.ToArray()))
            using (var decomressionStream = new GZipStream(requestStream, CompressionMode.Decompress))
            {
                var dataStream = new MemoryStream();
                decomressionStream.CopyTo(dataStream);
                dataStream.Seek(0, SeekOrigin.Begin);

                using (var reader = XmlReader.Create(dataStream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore }))
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                }
            }
        }
    }
}
