using System;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.JsonSerialization;

namespace DocaLabs.Http.Client.Binding.ResponseDeserialization
{
    /// <summary>
    /// Deserializes the response stream content using JSON format.
    /// </summary>
    public class JsonResponseDeserializer : IResponseDeserializationProvider
    {
        /// <summary>
        /// Deserializes the response stream content using JSON format.
        /// The method is using Newtonsoft deserializer with default settings.
        /// If the response stream content is empty then the default(TResult) is returned.
        /// </summary>
        public object Deserialize(HttpResponseStream responseStream, Type resultType)
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
    }
}
