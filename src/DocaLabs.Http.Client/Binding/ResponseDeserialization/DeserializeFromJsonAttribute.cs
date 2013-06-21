using System;
using DocaLabs.Http.Client.Utils.JsonSerialization;

namespace DocaLabs.Http.Client.Binding.ResponseDeserialization
{
    /// <summary>
    /// Deserializes JSON object from the web response.
    /// </summary>
    public class DeserializeFromJsonAttribute : ResponseDeserializationAttribute
    {
        /// <summary>
        /// Deserializes JSON object from the web response.
        /// </summary>
        public override object Deserialize(HttpResponseStream responseStream, Type resultType)
        {
            if (responseStream == null)
                throw new ArgumentNullException("responseStream");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            var s = responseStream.AsString();

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
