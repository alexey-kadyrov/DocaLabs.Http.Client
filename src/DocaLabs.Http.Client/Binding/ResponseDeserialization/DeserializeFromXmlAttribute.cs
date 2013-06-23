using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.ResponseDeserialization
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
        public override object Deserialize(HttpResponseStream responseStream, Type resultType)
        {
            if (responseStream == null)
                throw new ArgumentNullException("responseStream");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            // stream is disposed by the reader
            using (var reader = XmlReader.Create(responseStream, GetXmlReaderSettings()))
            {
                return new XmlSerializer(resultType).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns true if the content type is 'text/xml' and the TResult is not "simple type", like int, string, Guid, double, etc.
        /// </summary>
        public bool CanDeserialize(HttpResponseStream responseStream, Type resultType)
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
                ValidationType = DtdProcessing == DtdProcessing.Parse ? ValidationType.DTD : ValidationType.None,
                CloseInput = false
            };
        }
    }
}
