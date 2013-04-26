using System;
using DocaLabs.Http.Client.JsonSerialization;
using DocaLabs.Http.Client.Utils;

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
        public object Deserialize(HttpResponse response, Type resultType)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            var s = response.AsString();

            try
            {
                return string.IsNullOrWhiteSpace(s)
                    ? null
                    : JsonSerializationProvider.Deserializer.Deserialize(s, resultType);
            }
            catch (Exception e)
            {
                throw new UnrecoverableHttpClientException(e.Message, e);
            }
        }

        /// <summary>
        /// Returns true if the content type is 'application/json' and the TResult is not "simple type", like int, string, Guid, double, etc.
        /// </summary>
        public bool CanDeserialize(HttpResponse response, Type resultType)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            return response.ContentType.Is("application/json") && (!resultType.IsSimpleType());
        }
    }
}
