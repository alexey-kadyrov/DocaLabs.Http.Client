namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Defines a type of credentials for NetworkCredentialElement
    /// </summary>
    public enum CredentialType
    {
        /// <summary>
        /// Credential are not defined.
        /// </summary>
        None = 0,

        /// <summary>
        /// The system credentials of the application.
        /// </summary>
        DefaultCredentials = 1,

        /// <summary>
        /// The network credentials of the current security context.
        /// </summary>
        DefaultNetworkCredentials = 2,

        /// <summary>
        /// The credentials that are specified explicitly by user,password and domain
        /// </summary>
        NetworkCredential = 3
    }
}
