using System;
using System.Collections.Concurrent;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;

namespace DocaLabs.Http.Client.Binding.UrlMapping
{
    public class DefaultUrlPathComposer : IUrlPathComposer
    {
        readonly ConcurrentDictionary<Type, PropertyMap> _orderedMaps = new ConcurrentDictionary<Type, PropertyMap>();

        public string Compose(object model, Uri baseUrl)
        {
            var existingPath = GetExistingPath(baseUrl);

            return Ignore(model)
                ? existingPath
                : _orderedMaps.GetOrAdd(model.GetType(), x => new PropertyMap(x)).ComposePath(model, existingPath);
        }

        static string GetExistingPath(Uri baseUrl)
        {
            return baseUrl == null 
                       ? ""
                       : baseUrl.GetComponents(UriComponents.Path, baseUrl.UserEscaped ? UriFormat.UriEscaped : UriFormat.Unescaped);
        }

        static bool Ignore(object model)
        {
            return model == null || model.GetType().GetCustomAttribute<IgnoreInRequestAttribute>(true) != null;
        }

        class PropertyMap
        {
            OrderedPropertyMap OrderedProperties { get; set; }
            NamedPropertyMap NamedProperties { get; set; }

            public PropertyMap(Type type)
            {
                OrderedProperties = new OrderedPropertyMap(type);
                NamedProperties = new NamedPropertyMap(type);
            }

            public string ComposePath(object model, string existingPath)
            {
                existingPath = NamedProperties.Compose(model, existingPath);

                return OrderedProperties.Compose(model, existingPath);
            }
        }
    }
}
