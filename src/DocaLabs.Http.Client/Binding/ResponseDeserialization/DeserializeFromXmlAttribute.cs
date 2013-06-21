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
