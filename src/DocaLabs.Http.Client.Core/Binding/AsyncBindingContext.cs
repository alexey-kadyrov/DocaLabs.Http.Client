using System;
using System.Threading;
using DocaLabs.Http.Client.Configuration;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Context for asynchronous binding operations.
    /// </summary>
    public class AsyncBindingContext : BindingContext
    {
        /// <summary>
        /// The token to monitor for cancellation requests.
        /// </summary>
        public CancellationToken CancellationToken { get; private set; }

        /// <summary>
        /// Initializes in instance of the AsyncBindingContext class.
        /// </summary>
        public AsyncBindingContext(object httpClient, object originalModel, IClientEndpoint configuration, Uri baseUrl, Type inputModelType, Type outputModelType, CancellationToken cancellationToken) 
            : base(httpClient, originalModel, configuration, baseUrl, inputModelType, outputModelType)
        {
            CancellationToken = cancellationToken;
        }
    }
}
