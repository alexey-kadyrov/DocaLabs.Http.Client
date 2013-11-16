using System;
using System.Collections.Generic;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the http client endpoint. 
    /// </summary>
    public interface IClientEndpoint
    {
        /// <summary>
        /// Gets or sets the endpoint name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the BaseUrl.
        /// </summary>
        Uri BaseUrl { get; }

        /// <summary>
        /// Gets or sets the request's method, e.g. GET,POST,PUT,DELETE. Default value is empty which means that HttpClient will try to figure out the method.
        /// </summary>
        string Method { get; }

        /// <summary>
        /// Gets or sets the request timeout in milliseconds. Default value is 90 seconds.
        /// </summary>
        int Timeout { get; }

        /// <summary>
        /// Gets or sets the response stream reading timeout in milliseconds. Default value is 300 seconds.
        /// </summary>
        int ReadTimeout { get; }

        /// <summary>
        /// Gets or sets the request stream writing timeout in milliseconds. Default value is 300 seconds.
        /// </summary>
        int WriteTimeout { get; }

        /// <summary>
        /// Get or sets whenever to add 'Accept-Encoding' header automatically depending on what content decoders are defined in ContentDecoderFactory.
        /// The default value is true.
        /// </summary>
        bool AutoSetAcceptEncoding { get; }

        /// <summary>
        /// Gets or sets values indicating the level of authentication and impersonation used for this request.
        /// </summary>
        RequestAuthenticationLevel AuthenticationLevel { get; }

        /// <summary> 
        /// Gets or sets authentication information for the request.
        /// </summary>
        IClientNetworkCredential Credential { get; }

        /// <summary>
        /// Gets the headers collection.
        /// </summary>
        IReadOnlyList<IClientHeader> Headers { get; }

        IReadOnlyList<IClientCertificateReference> ClientCertificates { get; }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        IClientProxy Proxy { get; }
    }
}