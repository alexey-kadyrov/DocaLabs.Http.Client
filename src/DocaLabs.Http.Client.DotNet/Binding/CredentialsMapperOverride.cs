using System;
using System.Collections.Generic;
using System.Net;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Default credentials mapper.
    /// </summary>
    public class CredentialsMapperOverride : CredentialsMapper
    {
        protected override ICredentials GetAsCredentialCache(Uri url, IEnumerable<KeyValuePair<string, ICredentials>> credentials)
        {
            var builder = new CredentialCacheBuilder(url);

            foreach (var credential in credentials)
                builder.AddNetworkCredential(credential.Key, credential.Value);

            return builder.CredentialCache;
        }
    
        class CredentialCacheBuilder
        {
            readonly Uri _prefix;
            public CredentialCache CredentialCache { get; private set; }

            public CredentialCacheBuilder(Uri url)
            {
                CredentialCache = new CredentialCache();
                _prefix = new Uri(url.GetLeftPart(UriPartial.Authority));
            }

            public void AddNetworkCredential(string authenticationType, ICredentials credential)
            {
                var value = credential as NetworkCredential;
                if (value == null)
                    throw new ArgumentException(string.Format(Resources.PlatformText.cannot_mix_network_credential_with_other, credential), "credential");

                CredentialCache.Add(_prefix, authenticationType, value);
            }
        }
    }
}