using System.Net;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// A generic that can be used to wrap the output model in order to get more information about the operation status.
    /// If you subclass that you must provide constructor with parameters (WebResponse response, object value).
    /// </summary>
    /// <typeparam name="T">Your output model.</typeparam>
    public class RichResponse<T> : RichResponseCore
    {
        /// <summary>
        /// Gets the value that is returned with the response.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the Response{} class by pulling additional information from WebResponse instance.
        /// </summary>
        public RichResponse(WebResponse response, object value)
            : base(response)
        {
            if (value != null)
                Value = (T)value;
        }
    }
}
