using System;
using System.Text;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.JsonSerialization;

namespace DocaLabs.Http.Client.Binding.ResponseDeserialization
{
    /// <summary>
    /// Deserializes the response stream content using JSON format.
    /// </summary>
    public class DeserializeFromJsonAttribute : ResponseDeserializationAttribute, IResponseDeserializationProvider
    {
        /// <summary>
        /// Gets or sets the type of text encoding to be used for Xml serialization.
        /// The default value is null which forces to infer that from the web response.
        /// </summary>
        public string CharSet { get; set; }

        /// <summary>
        /// Deserializes the response stream content using JSON format.
        /// The method is using Newtonsoft deserializer with default settings.
        /// If the response stream content is empty then the default(TResult) is returned.
        /// </summary>
        public override object Deserialize(HttpResponseStream responseStream, Type resultType)
        {
            if (responseStream == null)
                throw new ArgumentNullException("responseStream");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            var encoding = GetEncoding();

            var s = responseStream.AsString(encoding);

            return ToResultType(resultType, s);
        }

        /// <summary>
        /// Returns true if the content type is 'application/json' and the TResult is not "simple type", like int, string, Guid, double, etc.
        /// </summary>
        public bool CanDeserialize(HttpResponseStream responseStream, Type resultType)
        {
            if (responseStream == null)
                throw new ArgumentNullException("responseStream");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            return responseStream.ContentType.Is("application/json") && (!resultType.IsSimpleType());
        }

        Encoding GetEncoding()
        {
            try
            {
                return string.IsNullOrWhiteSpace(CharSet)
                    ? null
                    : Encoding.GetEncoding(CharSet);
            }
            catch (Exception e)
            {
                throw new HttpClientException(e.Message, e);
            }
        }

        static object ToResultType(Type resultType, string s)
        {
            try
            {
                return string.IsNullOrWhiteSpace(s)
                    ? null
                    : JsonSerializationProvider.Deserializer.Deserialize(s, resultType);
            }
            catch (Exception e)
            {
                throw new HttpClientException(e.Message, e);
            }
        }
    }
}
