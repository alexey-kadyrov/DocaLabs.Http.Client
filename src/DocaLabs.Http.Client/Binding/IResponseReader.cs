using System;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines methods to read a model from the http response.
    /// </summary>
    public interface IResponseReader
    {
        /// <summary>
        /// Reads the response string and returns an object if there is anything there.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <param name="response">The response.</param>
        /// <param name="resultType">Expected type for the return value.</param>
        /// <returns>Return value from the stream or null.</returns>
        object Read(BindingContext context, HttpResponse response, Type resultType);
    }
}
