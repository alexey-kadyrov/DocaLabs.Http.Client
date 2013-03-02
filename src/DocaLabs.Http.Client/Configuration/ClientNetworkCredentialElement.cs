using System.Configuration;

namespace DocaLabs.Http.Client.Configuration
{
    /// <summary>
    /// Represents a configuration element that defines the network credentials. 
    /// </summary>
    public class ClientNetworkCredentialElement : ConfigurationElement, IClientNetworkCredential
    {
        const string CredentialTypeProperty = "credentialType";
        const string UserProperty = "user";
        const string PasswordProperty = "password";
        const string DomainProperty = "domain";

        /// <summary>
        /// Gets or sets the type of credentials. If it's DefaultCredentials or DefaultNetworkCredentials other properties will be ignored.
        /// The default value is CredentialType.None.
        /// </summary>
        public CredentialType CredentialType
        {
            get { return CredentialTypeElement; }
            set { CredentialTypeElement = value; }
        }

        /// <summary>
        /// Gets or sets the user name if the CredentialType is NetworkCredential.
        /// </summary>
        public string User
        {
            get { return UserElement; }
            set { UserElement = value; }
        }

        /// <summary>
        /// Gets or sets the password if the CredentialType is NetworkCredential.
        /// </summary>
        public string Password
        {
            get { return PasswordElement; }
            set { PasswordElement = value; }
        }

        /// <summary>
        /// Gets or sets the domain if the CredentialType is NetworkCredential.
        /// </summary>
        public string Domain
        {
            get { return DomainElement; }
            set { DomainElement = value; }
        }

        [ConfigurationProperty(CredentialTypeProperty, IsRequired = false, DefaultValue = CredentialType.None)]
        CredentialType CredentialTypeElement
        {
            get { return ((CredentialType)base[CredentialTypeProperty]); }
            set { base[CredentialTypeProperty] = value; }
        }

        [ConfigurationProperty(UserProperty, IsRequired = false, DefaultValue = "")]
        string UserElement
        {
            get { return ((string)base[UserProperty]); }
            set { base[UserProperty] = value; }
        }

        [ConfigurationProperty(PasswordProperty, IsRequired = false, DefaultValue = "")]
        string PasswordElement
        {
            get { return ((string)base[PasswordProperty]); }
            set { base[PasswordProperty] = value; }
        }

        [ConfigurationProperty(DomainProperty, IsRequired = false, DefaultValue = "")]
        string DomainElement
        {
            get { return ((string)base[DomainProperty]); }
            set { base[DomainProperty] = value; }
        }
    }
}
