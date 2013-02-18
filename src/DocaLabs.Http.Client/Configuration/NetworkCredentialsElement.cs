using System.Configuration;
using System.Net;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the network credentials. 
    /// </summary>
    public class NetworkCredentialsElement : ConfigurationElement
    {
        const string CredentialsTypeProperty = "credentialsType";
        const string UserProperty = "user";
        const string PasswordProperty = "password";
        const string DomainProperty = "domain";

        /// <summary>
        /// Gets or sets the type of credentials. If it's DefaultCredentials or DefaultNetworkCredentials other properties will be ignored.
        /// The default value is CredentialsType.None.
        /// </summary>
        [ConfigurationProperty(CredentialsTypeProperty, IsRequired = false, DefaultValue = CredentialsType.None)]
        public CredentialsType CredentialsType
        {
            get { return ((CredentialsType)base[CredentialsTypeProperty]); }
            set { base[CredentialsTypeProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the user name if the CredentialsType is NetworkCredential.
        /// </summary>
        [ConfigurationProperty(UserProperty, IsRequired = false, DefaultValue = "")]
        public string User
        {
            get { return ((string)base[UserProperty]); }
            set { base[UserProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the password if the CredentialsType is NetworkCredential.
        /// </summary>
        [ConfigurationProperty(PasswordProperty, IsRequired = false, DefaultValue = "")]
        public string Password
        {
            get { return ((string)base[PasswordProperty]); }
            set { base[PasswordProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the domain if the CredentialsType is NetworkCredential.
        /// </summary>
        [ConfigurationProperty(DomainProperty, IsRequired = false, DefaultValue = "")]
        public string Domain
        {
            get { return ((string)base[DomainProperty]); }
            set { base[DomainProperty] = value; }
        }

        /// <summary>
        /// Gets credentials from the description.
        /// </summary>
        /// <returns></returns>
        public ICredentials GetCredentials()
        {
            switch (CredentialsType)
            {
                case CredentialsType.DefaultCredentials:
                    return CredentialCache.DefaultCredentials;

                case CredentialsType.DefaultNetworkCredentials:
                    return CredentialCache.DefaultNetworkCredentials;

                case CredentialsType.NetworkCredential:
                    return new NetworkCredential(
                        string.IsNullOrWhiteSpace(User) ? string.Empty : User,
                        string.IsNullOrWhiteSpace(Password) ? string.Empty : Password,
                        string.IsNullOrWhiteSpace(Domain) ? string.Empty : Domain);

                default:
                    return null;
            }
        }
    }
}
