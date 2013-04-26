using System;
using System.Xml;
using System.Xml.Serialization;

namespace DocaLabs.Http.Client.Binding.ResponseDeserialization
{
    /// <summary>
    /// Deserializes xml object from the web response.
    /// </summary>
    public class DeserializeFromXmlAttribute : ResponseDeserializationAttribute
    {
        /// <summary>
        /// Gets or sets a value that determines the processing of DTDs.
        /// The default value is DtdProcessing.Ignore.
        /// </summary>
        public DtdProcessing DtdProcessing { get; set; }

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
        public override object Deserialize(HttpResponse response, Type resultType)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            // cannot wrap into UnrecoverableHttpClientException as the XmlSerializer reads from the stream  so it may throw network exception

            // stream is disposed by the reader
            using (var reader = XmlReader.Create(response.GetDataStream(), GetXmlReaderSettings()))
            {
                return new XmlSerializer(resultType).Deserialize(reader);
            }
        }

        XmlReaderSettings GetXmlReaderSettings()
        {
            return new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing,
                ValidationType = DtdProcessing == DtdProcessing.Parse ? ValidationType.DTD : ValidationType.None
            };
        }
    }
}
