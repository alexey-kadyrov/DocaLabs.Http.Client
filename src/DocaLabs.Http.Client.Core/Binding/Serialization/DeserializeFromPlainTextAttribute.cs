using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Deserializes the response stream as plain string and then converts it to the resulting type.
    /// </summary>
    public class DeserializeFromPlainTextAttribute : ResponseDeserializationAttribute, IResponseDeserializationProvider
    {
        /// <summary>
        /// Gets or sets the type of text encoding to be used for Xml serialization.
        /// The default value is null which forces to infer that from the web response.
        /// </summary>
        public string CharSet { get; set; }

        /// <summary>
        /// Deserializes the response stream as plain string and then converts to the resulting type.
        /// </summary>
        public override object Deserialize(IHttpResponseStream responseStream, Type resultType)
        {
            if (responseStream == null)
                throw new ArgumentNullException("responseStream");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            var encoding = GetEncoding();

            var value = responseStream.AsString(encoding);

            return ToResultType(resultType, value);
        }

        /// <summary>
        /// Asynchronously deserializes the response stream as plain string and then converts to the resulting type.
        /// </summary>
        public override async Task<object> DeserializeAsync(IHttpResponseStream responseStream, Type resultType, CancellationToken cancellationToken)
        {
            if (responseStream == null)
                throw new ArgumentNullException("responseStream");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            cancellationToken.ThrowIfCancellationRequested();

            var encoding = GetEncoding();

            var value = await responseStream.AsStringAsync(encoding);

            return ToResultType(resultType, value);
        }

        /// <summary>
        /// Returns true if the content type is 'text/plain' and the TResult is "simple type", like int, string, Guid, double, etc.
        /// or if the content type is one of 'text/html', 'text/xml', 'application/xml', 'application/json' and the TResult is string.
        /// </summary>
        public bool CanDeserialize(IHttpResponseStream responseStream, Type resultType)
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

        static object ToResultType(Type resultType, string value)
        {
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
    }
}
