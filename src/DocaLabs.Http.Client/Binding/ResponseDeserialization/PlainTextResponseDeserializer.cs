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
        public object Deserialize(HttpResponseStream responseStream, Type resultType)
        {
            if (responseStream == null)
                throw new ArgumentNullException("responseStream");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            var value = responseStream.AsString();

            try
            {
                return string.IsNullOrWhiteSpace(value)
                    ? resultType.GetDefaultValue()
                    : CustomConverter.Current.ChangeType(value, resultType);
            }
            catch (Exception e)
            {
                throw new HttpClientException(e.Message, e);
            }
        }

        /// <summary>
        /// Returns true if the content type is 'text/plain' and the TResult is "simple type", like int, string, Guid, double, etc.
        /// or if the content type is one of 'text/html', 'text/xml', 'application/xml', 'application/json' and the TResult is string.
        /// </summary>
        public bool CanDeserialize(HttpResponseStream responseStream, Type resultType)
        {
            if (responseStream == null)
                throw new ArgumentNullException("responseStream");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            return (
                        (responseStream.ContentType.Is("text/plain") && resultType.IsSimpleType()) 
                        ||
                        (
                            resultType == typeof(string) &&
                            (
                                responseStream.ContentType.Is("text/html") ||
                                responseStream.ContentType.Is("text/xml") ||
                                responseStream.ContentType.Is("application/xml") ||
                                responseStream.ContentType.Is("application/json")
                            )
                        )
                    );
        }
    }
}
