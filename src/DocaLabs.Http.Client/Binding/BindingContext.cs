﻿using System;
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
        /// Gets the original input model which was passed to the HttpClient.Execute method, 
        /// it could be different from the Model due the transformation.
        /// </summary>
        public object OriginalModel { get; private set; }

        /// <summary>
        /// Gets the current input model.
        /// </summary>
        public object Model { get; internal set; }

        /// <summary>
        /// Gets the HttpClient's endpoint configuration.
        /// </summary>
        public IClientEndpoint Configuration { get; private set; }

        /// <summary>
        /// Gets the base URL fro the service endpoint.
        /// </summary>
        public Uri BaseUrl { get; private set; }

        /// <summary>
        /// Initializes in instance of the BindingContext class.
        /// </summary>
        public BindingContext(object httpClient, object originalModel, IClientEndpoint configuration, Uri baseUrl)
        {
            BaseUrl = baseUrl;
            Configuration = configuration;
            OriginalModel = originalModel;
            HttpClient = httpClient;
        }
    }
}
