using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultCredentialsMapper : ICredentialsMapper
    {
        ConcurrentDictionary<Type, PropertyMap> PropertyMaps { get; set; }

        public ICredentials Map(object model, Uri url)
        {
            if (model == null)
                return null;

            return GetCredentials(model, url, PropertyMaps.GetOrAdd(model.GetType(), x => new PropertyMap(x)));
        }

        static ICredentials GetCredentials(object model, Uri url, PropertyMap map)
        {
            switch (map.Credentials.Count)
            {
                case 0:
                    return null;

                case 1:
                    return map.Credentials[0].GetValue(model) as ICredentials;

                default:
                    return GetAsCredentialCache(model, url, map);
            }
        }

        static ICredentials GetAsCredentialCache(object model, Uri url, PropertyMap map)
        {
            if (url == null)
                return null;

            var credentials = new CredentialCache();
            var prefix = new Uri(url.GetLeftPart(UriPartial.Authority));

            foreach (var property in map.Credentials)
            {
                var value = property.GetValue(model) as NetworkCredential;
                if (value == null)
                    continue;

                credentials.Add(prefix, property.Name, value);
            }

            return credentials;
        }

        class PropertyMap
        {
            public IList<PropertyInfo> Credentials { get; private set; }

            public PropertyMap(Type type)
            {
                Credentials = Parse(type);
            }

            IList<PropertyInfo> Parse(Type type)
            {
                return type.IsSimpleType() 
                    ? new List<PropertyInfo>()
                    : type.GetAllProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(x => x.IsCredentials())
                        .ToList();
            }
        }
    }
}