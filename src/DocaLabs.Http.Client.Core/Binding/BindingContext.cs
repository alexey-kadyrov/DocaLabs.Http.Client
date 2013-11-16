using System;
using DocaLabs.Http.Client.Configuration;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Context for binding operations.
    /// </summary>
    public class BindingContext
    {
        /// <summary>
        /// Gets the HttpClient which is being called.
        /// </summary>
        public object HttpClient { get; private set; }

        /// <summary>
        /// Gets the HttpClient's endpoint configuration.
        /// </summary>
        public IClientEndpoint Configuration { get; private set; }

        /// <summary>
        /// Gets the base URL for the service endpoint.
        /// </summary>
        public Uri BaseUrl { get; private set; }

        /// <summary>
        /// Gets the input model type. Which is inferred from the original model or if it's null then from the generic parameter TInputModel on the http client.
        /// </summary>
        public Type InputModelType { get; private set; }

        /// <summary>
        /// Gets the output model type.
        /// </summary>
        public Type OutputModelType { get; private set; }

        /// <summary>
        /// Gets the original input model which was passed to the HttpClient.Execute method, 
        /// it could be different from the Model due the transformation.
        /// </summary>
        public object OriginalModel { get; private set; }

        /// <summary>
        /// Gets the current input model.
        /// </summary>
        public object Model { get; internal set; }

        /// <summary>
        /// Gets the base URL for the service endpoint.
        /// </summary>
        public Uri RequestUrl { get; internal set; }

        /// <summary>
        /// Initializes in instance of the BindingContext class.
        /// </summary>
        public BindingContext(object httpClient, object originalModel, IClientEndpoint configuration, Uri baseUrl, Type inputModelType, Type outputModelType)
        {
            if (httpClient == null)
                throw new ArgumentNullException("httpClient");

            if(configuration == null)
                throw new ArgumentNullException("configuration");

            if(baseUrl == null)
                throw new ArgumentNullException("baseUrl");

            if(inputModelType == null)
                throw new ArgumentNullException("inputModelType");

            if(outputModelType == null)
                throw new ArgumentNullException("outputModelType");

            BaseUrl = baseUrl;
            InputModelType = inputModelType;
            OutputModelType = outputModelType;
            Configuration = configuration;
            OriginalModel = originalModel;
            HttpClient = httpClient;
        }
    }
}
