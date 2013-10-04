﻿using System;
using System.Net;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines methods to read a model from the http response.
    /// </summary>
    public interface IAsyncResponseBinder
    {
        /// <summary>
        /// Asynchronously reads the response stream and returns an object if there is anything there.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <param name="request">The WebRequest object.</param>
        /// <param name="resultType">Expected type for the return value.</param>
        /// <returns>Return value from the stream or null.</returns>
        Task<object> ReadAsync(AsyncBindingContext context, WebRequest request, Type resultType);
    }
}