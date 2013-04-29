using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using DocaLabs.Http.Client.Binding.Utils;

namespace DocaLabs.Http.Client.Binding.ResponseDeserialization
{
    /// <summary>
    /// Deserializes the response stream content using XML format.
    /// Static methods are thread safe.
    /// </summary>
    public class XmlResponseDeserializer : IResponseDeserializationProvider
    {
        static readonly object Locker;
        static HashSet<string> _supportedTypes;
        static DtdProcessing _dtdProcessing;

        /// <summary>
        /// Gets or sets a value that determines the processing of DTDs.  The value is static because the class is meant to be used
        /// unobtrusively and is not accessible directly in the http request pipeline per instance base.
        /// The property is not thread safe so it should be set before the first usage.
        /// The default value is DtdProcessing.Ignore.
        /// </summary>
        public static DtdProcessing DtdProcessing
        {
            get
            {
                lock (Locker)
                {
                    return _dtdProcessing;
                }
            }
            set
            {
                lock (Locker)
                {
                    _dtdProcessing = value;
                }
            }
        }

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

        static XmlResponseDeserializer()
        {
            Locker = new object();

            _dtdProcessing = DtdProcessing.Ignore;

            _supportedTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "text/xml",
                "application/xml"
            };
        }

        /// <summary>
        /// Deserializes the response stream content using JSON format.
        /// The method is using XmlSerializer with default settings except the DTD processing is set to ignore.
        /// </summary>
        public object Deserialize(HttpResponse response, Type resultType)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            // stream is disposed by the reader
            using (var reader = XmlReader.Create(response.GetDataStream(), GetXmlReaderSettings()))
            {
                return new XmlSerializer(resultType).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns true if the content type is 'text/xml' and the TResult is not "simple type", like int, string, Guid, double, etc.
        /// </summary>
        public bool CanDeserialize(HttpResponse response, Type resultType)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            if (resultType.IsSimpleType())
                return false;

            lock (Locker)
            {
                return _supportedTypes.Contains(response.ContentType.MediaType);
            }
        }

        static XmlReaderSettings GetXmlReaderSettings()
        {
            return new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing,
                ValidationType = DtdProcessing == DtdProcessing.Parse ? ValidationType.DTD : ValidationType.None,
            };
        }
    }
}
