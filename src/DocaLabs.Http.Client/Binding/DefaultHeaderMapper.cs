using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Hints;
using DocaLabs.Http.Client.Binding.PropertyConverting;

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

        static WebHeaderCollection GetHeaders(NameValueCollection headers)
        {
            return new WebHeaderCollection
            {
                headers
            };
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
