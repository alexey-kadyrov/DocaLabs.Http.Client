using System;
using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Hints;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultHeaderMapper : IHeaderMapper
    {
        readonly ConcurrentDictionary<Type, PropertyMap> _headerPropertyMaps = new ConcurrentDictionary<Type, PropertyMap>();

        public WebHeaderCollection Map(object model)
        {
            return Ignore(model) 
                ? new WebHeaderCollection()
                : GetHeaders(PropertyMapGetOrAddType(model).ConvertModel(model));
        }

        PropertyMap PropertyMapGetOrAddType(object model)
        {
            return _headerPropertyMaps.GetOrAdd(model.GetType(), x => new HeaderPropertyMap(x, PropertyMapGetOrAddType));
        }

        static bool Ignore(object model)
        {
            return model == null || model.GetType().GetCustomAttribute<IgnoreInRequestAttribute>(true) != null;
        }

        static WebHeaderCollection GetHeaders(IDictionaryList<string, string> headers)
        {
            var collection = new WebHeaderCollection();

            foreach (var key in headers.Keys)
            {
                foreach (var value in headers[key])
                {
                    collection.Add(key, value);
                }
            }

            return collection;
        }

        class HeaderPropertyMap : PropertyMap
        {
            public HeaderPropertyMap(Type type, Func<object, PropertyMap> propertyMapGetOrAddType)
                : base(type, propertyMapGetOrAddType)
            {
            }

            protected override bool AcceptProperty(PropertyInfo info)
            {
                return info.IsHeader();
            }

            protected override IPropertyConverterOverrides GetPropertyConverterOverrides(PropertyInfo property)
            {
                return property.GetCustomAttribute<InRequestHeaderAttribute>();
            }
        }
    }
}
