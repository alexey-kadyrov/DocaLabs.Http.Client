﻿using System.Net;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Binding.Serialization
{
    /// <summary>
    /// Defines methods to serialize an object into web request.
    /// </summary>
    public interface IAsyncRequestSerialization
    {
        /// <summary>
        /// When is overridden in derived class it asynchronously serializes a given object into the web request.
        /// </summary>
        Task SerializeAsync(AsyncBindingContext context, WebRequest request, object value);
    }
}