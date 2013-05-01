using System;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.ResponseDeserialization
{
    /// <summary>
    /// Deserializes the response stream as plain string and then converts it to the resulting type.
    /// </summary>
    public class PlainTextResponseDeserializer : IResponseDeserializationProvider
    {
        /// <summary>
        /// Deserializes the response stream as plain string and then converts to the resulting type.
        /// </summary>
        public object Deserialize(HttpResponse response, Type resultType)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            var value = response.AsString();

            try
            {
                return string.IsNullOrWhiteSpace(value)
                    ? resultType.GetDefaultValue()
                    : CustomConverter.Current.ChangeType(value, resultType);
            }
            catch (Exception e)
            {
                throw new UnrecoverableHttpClientException(e.Message, e);
            }
        }

        /// <summary>
        /// Returns true if the content type is 'text/plain' and the TResult is "simple type", like int, string, Guid, double, etc.
        /// or if the content type is 'text/html' and the TResult is string.
        /// </summary>
        public bool CanDeserialize(HttpResponse response, Type resultType)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            return (
                        (response.ContentType.Is("text/plain") && resultType.IsSimpleType()) 
                        ||
                        (response.ContentType.Is("text/html") && resultType == typeof(string))
                    );
        }
    }
}
