using System;
using System.Net;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines the methods that are required for a model binder.
    /// </summary>
    public interface IModelBinder
    {
        /// <summary>
        /// Transforms an input model to an instance of some other type which will be used in the rest of the request pipeline.
        /// The method is useful for implementing canonical data model pattern.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <returns>A new or the same model, depends on your logic. The method may return null if the baseURL is enough to call the remote endpoint.</returns>
        object TransformInputModel(BindingContext context);

        /// <summary>
        /// Composes a request's URL using an input model and a base URL.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <returns>A URL which of a remote endpoint.</returns>
        string ComposeUrl(BindingContext context);

        /// <summary>
        /// If the method is not explicitly defined in the client's configuration this method will be called 
        /// in order to determine what method w=should be used to call the remote endpoint.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <returns>The methods, e.g. GET, POST, PUT, etc.</returns>
        string InferRequestMethod(BindingContext context);

        /// <summary>
        /// Gets headers that may be defined in an input model.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <returns>The collection of headers, the collection can be empty.</returns>
        WebHeaderCollection GetHeaders(BindingContext context);

        /// <summary>
        /// Gets credentials that may be defined in an input model.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <param name="url">The request's URL.</param>
        /// <returns>Credentials or null if nothing is defined in the model.</returns>
        ICredentials GetCredentials(BindingContext context, Uri url);

        /// <summary>
        /// The method is called to write data to the request's stream. It's expected that the method will set correctly
        /// property related to the data in the request, like content encoding, content length, etc.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <param name="request">The request.</param>
        void Write(BindingContext context, WebRequest request);
    }
}
