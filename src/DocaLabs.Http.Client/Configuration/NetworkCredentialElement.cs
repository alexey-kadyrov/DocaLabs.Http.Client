using System;
using System.Configuration;
using System.Net;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the network credentials. 
    /// </summary>
    public class NetworkCredentialElement : ConfigurationElement
    {
        const string CredentialTypeProperty = "credentialType";
        const string UserProperty = "user";
        const string PasswordProperty = "password";
        const string DomainProperty = "domain";

        /// <summary>
        /// Gets or sets the type of credentials. If it's DefaultCredentials or DefaultNetworkCredentials other properties will be ignored.
        /// The default value is CredentialType.None.
        /// </summary>
        [ConfigurationProperty(CredentialTypeProperty, IsRequired = false, DefaultValue = CredentialType.None)]
        public CredentialType CredentialType
        {
            get { return ((CredentialType)base[CredentialTypeProperty]); }
            set { base[CredentialTypeProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the user name if the CredentialType is NetworkCredential.
        /// </summary>
        [ConfigurationProperty(UserProperty, IsRequired = false, DefaultValue = "")]
        public string User
        {
            get { return ((string)base[UserProperty]); }
            set { base[UserProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the password if the CredentialType is NetworkCredential.
        /// </summary>
        [ConfigurationProperty(PasswordProperty, IsRequired = false, DefaultValue = "")]
        public string Password
        {
            get { return ((string)base[PasswordProperty]); }
            set { base[PasswordProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the domain if the CredentialType is NetworkCredential.
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
        public ICredentials GetCredential()
        {
            switch (CredentialType)
            {
                case CredentialType.DefaultCredentials:
                    return CredentialCache.DefaultCredentials;

                case CredentialType.DefaultNetworkCredentials:
                    return CredentialCache.DefaultNetworkCredentials;

                case CredentialType.NetworkCredential:
                    return GetNetworkCredential();

                default:
                    return null;
            }
        }

        ICredentials GetNetworkCredential()
        {
            return new NetworkCredential(
                string.IsNullOrWhiteSpace(User) ? string.Empty : User,
                string.IsNullOrWhiteSpace(Password) ? string.Empty : Password,
                string.IsNullOrWhiteSpace(Domain) ? string.Empty : Domain);
        }
    }
}
