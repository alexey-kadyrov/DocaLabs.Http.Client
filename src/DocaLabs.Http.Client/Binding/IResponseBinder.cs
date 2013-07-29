using System;
using System.Net;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines methods to read a model from the http response.
    /// </summary>
    public interface IResponseBinder
    {
        /// <summary>
        /// Reads the response stream and returns an object if there is anything there.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <param name="request">The WebRequest object.</param>
        /// <param name="resultType">Expected type for the return value.</param>
        /// <returns>Return value from the stream or null.</returns>
        object Read(BindingContext context, WebRequest request, Type resultType);
    }
}
