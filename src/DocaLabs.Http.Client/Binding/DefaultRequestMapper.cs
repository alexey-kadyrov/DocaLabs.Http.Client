using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultRequestMapper : IRequestMapper
    {
        ConcurrentDictionary<Type, PropertyMap> ParsedMaps { get; set; }

        public DefaultRequestMapper()
        {
            ParsedMaps = new ConcurrentDictionary<Type, PropertyMap>();
        }

        public void Map(RequestContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (context.QueryModel == null || context.Request == null)
                return;

            var map = ParsedMaps.GetOrAdd(context.QueryModel.GetType(), x => new PropertyMap(x));

            ApplyHeaders(context, map.Headers);

            ApplyCredentials(context, map.Credentials);
        }

        static void ApplyHeaders(RequestContext context, IEnumerable<PropertyInfo> headerProperties)
        {
            foreach (var headers in headerProperties.Select(x => x.GetValue(context.QueryModel)).OfType<WebHeaderCollection>())
                context.Request.Headers.Add(headers);
        }

        static void ApplyCredentials(RequestContext context, IList<PropertyInfo> credentialProperties)
        {
            switch (credentialProperties.Count)
            {
                case 0:
                    return;

                case 1:
                    context.Request.Credentials = credentialProperties[0].GetValue(context.QueryModel) as ICredentials;
                    return;

                default:
                {
                    var credentials = new CredentialCache();
                    var prefix = new Uri(context.Request.RequestUri.GetLeftPart(UriPartial.Authority));

                    foreach (var property in credentialProperties)
                    {
                        var value = property.GetValue(context.QueryModel) as NetworkCredential;
                        if(value == null)
                            continue;

                        credentials.Add(prefix, property.Name, value);
                    }

                    break;
                }
            }
        }

        class PropertyMap
        {
            public IList<PropertyInfo> Headers { get; private set; }
            public IList<PropertyInfo> Credentials { get; private set; }

            public PropertyMap(Type type)
            {
                Headers = new List<PropertyInfo>();
                Credentials = new List<PropertyInfo>();

                Parse(type);
            }

            void Parse(Type type)
            {
                if(type.IsSimpleType())
                    return;

                foreach (var property in type.GetAllProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if(property.IsHeaderCollection())
                        Headers.Add(property);
                    else if(property.IsCredential())
                        Credentials.Add(property);
                }
            }
        }
    }
}
