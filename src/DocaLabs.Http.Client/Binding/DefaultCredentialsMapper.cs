using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Default credentials mapper.
    /// </summary>
    public class DefaultCredentialsMapper
    {
        static readonly ConcurrentDictionary<Type, PropertyMap> PropertyMaps = new ConcurrentDictionary<Type, PropertyMap>();

        /// <summary>
        /// Maps the model to credentials by checking whenever any of its properties returns non null object implementing ICredentials.
        /// If more then one NetworkCredential value is detected then CredentialCache object is returned with all of then cached.
        /// In this case the UriPartial.Authority of the URL is used to add credentials to the CredentialCache and the property name is used as the authentication type.
        /// </summary>
        public ICredentials Map(object client, object model, Uri url)
        {
            if(client == null)
                throw new ArgumentNullException("client");

            return Ignore(client, model) 
                ? null 
                : GetCredentials(model, url, PropertyMaps.GetOrAdd(model.GetType(), x => new PropertyMap(x)));
        }

        static bool Ignore(object client, object model)
        {
            return model == null || model.GetType().IsSerializableToRequestBody() || client.GetType().IsSerializableToRequestBody();
        }

        static ICredentials GetCredentials(object model, Uri url, PropertyMap map)
        {
            var credentials = GetAllCredentialValues(model, map);

            switch (credentials.Count)
            {
                case 0:
                    return null;

                case 1:
                    return credentials.Values.First();

                default:
                    return GetAsCredentialCache(url, credentials);
            }
        }

        static IDictionary<string, ICredentials> GetAllCredentialValues(object model, PropertyMap map)
        {
            var credentials = new Dictionary<string, ICredentials>();

            foreach (var info in map.Credentials)
            {
                var value = info.GetValue(model) as ICredentials;
                if (value != null)
                    credentials[info.Name] = value;
            }

            return credentials;
        }

        static ICredentials GetAsCredentialCache(Uri url, IEnumerable<KeyValuePair<string, ICredentials>> credentials)
        {
            var builder = new CredentialCacheBuilder(url);

            foreach (var credential in credentials)
                builder.AddNetworkCredential(credential.Key, credential.Value);

            return builder.CredentialCache;
        }

        class PropertyMap
        {
            public IList<PropertyInfo> Credentials { get; private set; }

            public PropertyMap(Type type)
            {
                Credentials = Parse(type);
            }

            static IList<PropertyInfo> Parse(Type type)
            {
                return type.IsSimpleType() 
                    ? new List<PropertyInfo>()
                    : type.GetAllPublicInstanceProperties()
                        .Where(x => x.IsCredentials())
                        .ToList();
            }
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
                    throw new ArgumentException(string.Format(Resources.Text.cannot_mix_network_credential_with_other, credential), "credential");

                CredentialCache.Add(_prefix, authenticationType, value);
            }
        }
    }
}