namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Defines property for a configuration element that defines the network credentials. 
    /// </summary>
    public interface IClientNetworkCredential
    {
        /// <summary>
        /// Gets or sets the type of credentials. If it's DefaultCredentials or DefaultNetworkCredentials other properties will be ignored.
        /// </summary>
        CredentialType CredentialType { get; set; }

        /// <summary>
        /// Gets or sets the user name if the CredentialType is NetworkCredential.
        /// </summary>
        string User { get; set; }

        /// <summary>
        /// Gets or sets the password if the CredentialType is NetworkCredential.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the domain if the CredentialType is NetworkCredential.
        /// </summary>
        string Domain { get; set; }
    }
}