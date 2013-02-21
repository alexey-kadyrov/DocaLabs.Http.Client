using System;
using System.Xml;
using System.Xml.Serialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.ResponseDeserialization
{
    /// <summary>
    /// Deserializes the response stream content using XML format.
    /// </summary>
    public class XmlResponseDeserializer : IResponseDeserializationProvider
    {
        /// <summary>
        /// Gets or sets a value that determines the processing of DTDs.  The value is static because the class is meant to be used
        /// unobtrusively and is not accessible directly in the http request pipeline per instance base.
        /// The property is not thread safe so it should be set before the first usage.
        /// The default value is DtdProcessing.Ignore.
        /// </summary>
        public static DtdProcessing DtdProcessing { get; set; }

        static XmlResponseDeserializer()
        {
            DtdProcessing = DtdProcessing.Ignore;
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

            return response.ContentType.Is("text/xml") && (!resultType.IsSimpleType());
        }

        static XmlReaderSettings GetXmlReaderSettings()
        {
            return new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing,
                ValidationType = DtdProcessing == DtdProcessing.Parse ? ValidationType.DTD : ValidationType.None
            };
        }
    }
}
