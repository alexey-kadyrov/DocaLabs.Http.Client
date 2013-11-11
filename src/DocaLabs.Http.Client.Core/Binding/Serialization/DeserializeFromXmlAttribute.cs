using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Deserializes xml object from the web response.
    /// </summary>
    public class DeserializeFromXmlAttribute : ResponseDeserializationAttribute, IResponseDeserializationProvider
    {
        static readonly object Locker;
        static HashSet<string> _supportedTypes;

        /// <summary>
        /// Gets/sets content MIME/media type which are supported by the deserializer.
        /// By default it's 'text/xml' and 'application/xml'
        /// </summary>
        public static string[] SupportedTypes
        {
            get
            {
                lock (Locker)
                {
                    return _supportedTypes.ToArray();
                }
            }
            set
            {
                lock (Locker)
                {
                    _supportedTypes = new HashSet<string>(value, StringComparer.OrdinalIgnoreCase);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that determines the processing of DTDs.
        /// The default value is DtdProcessing.Ignore.
        /// </summary>
        public DtdProcessing DtdProcessing { get; set; }

        static DeserializeFromXmlAttribute()
        {
            Locker = new object();

            _supportedTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "text/xml",
                "application/xml"
            };
        }

        /// <summary>
        /// Initializes an instance of the DeserializeFromXmlAttribute class.
        /// </summary>
        public DeserializeFromXmlAttribute()
        {
            DtdProcessing = DtdProcessing.Ignore;
        }

        /// <summary>
        /// Deserializes xml object from the web response.
        /// </summary>
        public override object Deserialize(HttpResponseStreamCore responseStream, Type resultType)
        {
            if (responseStream == null)
                throw new ArgumentNullException("responseStream");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            using (var reader = XmlReader.Create(responseStream, GetXmlReaderSettings()))
            {
                return new XmlSerializer(resultType).Deserialize(reader);
            }
        }

        /// <summary>
        /// Asynchronously Deserializes xml object from the web response.
        /// </summary>
        public override async Task<object> DeserializeAsync(HttpResponseStreamCore responseStream, Type resultType, CancellationToken cancellationToken)
        {
            if (responseStream == null)
                throw new ArgumentNullException("responseStream");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            using (var buffer = new MemoryStream(4096))
            {
                // do at least reading asynchronously
                await responseStream.CopyToAsync(buffer, 4096, cancellationToken);

                buffer.Seek(0, SeekOrigin.Begin);

                using (var reader = XmlReader.Create(buffer, GetXmlReaderSettings()))
                {
                    return new XmlSerializer(resultType).Deserialize(reader);
                }
            }
        }

        /// <summary>
        /// Returns true if the content type is 'text/xml' and the TResult is not "simple type", like int, string, Guid, double, etc.
        /// </summary>
        public bool CanDeserialize(HttpResponseStreamCore responseStream, Type resultType)
        {
            if (responseStream == null)
                throw new ArgumentNullException("responseStream");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            if (resultType.IsSimpleType())
                return false;

            lock (Locker)
            {
                return _supportedTypes.Contains(responseStream.ContentType.MediaType);
            }
        }

        XmlReaderSettings GetXmlReaderSettings()
        {
            return new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing,
                CloseInput = false
            };
        }
    }
}
