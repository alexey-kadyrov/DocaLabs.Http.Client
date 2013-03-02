using System;
using System.Net.Security;

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
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the BaseUrl.
        /// </summary>
        Uri BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the request's method, e.g. GET,POST,PUT,DELETE. Default value is empty which means that HttpClient will try to figure out the method.
        /// </summary>
        string Method { get; set; }

        /// <summary>
        /// Gets or sets the request timeout. Default value is 90 seconds.
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Get or sets whenever to add 'Accept-Encoding' header automatically depending on what content decoders are defined in ContentDecoderFactory.
        /// The default value is true.
        /// </summary>
        bool AutoSetAcceptEncoding { get; set; }

        /// <summary>
        /// Gets or sets values indicating the level of authentication and impersonation used for this request.
        /// </summary>
        AuthenticationLevel? AuthenticationLevel { get; set; }

        /// <summary>
        /// Gets or sets authentication information for the request.
        /// </summary>
        IClientNetworkCredential Credential { get; }

        /// <summary>
        /// Gets the headers collection.
        /// </summary>
        IClientHeaderCollection Headers { get; }

        /// <summary>
        /// Gets the client certificate collection.
        /// </summary>
        IClientCertificateReferenceCollection ClientCertificates { get; }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        IClientProxy Proxy { get; }
    }
}