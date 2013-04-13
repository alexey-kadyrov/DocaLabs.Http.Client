using System;
using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultHeaderMapper : IHeaderMapper
    {
        readonly static ConcurrentDictionary<Type, HeaderPropertyMap> HeaderPropertyMaps = new ConcurrentDictionary<Type, HeaderPropertyMap>();

        public WebHeaderCollection Map(object model)
        {
            return Ignore(model) 
                ? new WebHeaderCollection()
                : GetHeaders(HeaderPropertyMaps.GetOrAdd(model.GetType(), x => new HeaderPropertyMap(x)).ConvertModel(model));
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
            public HeaderPropertyMap(Type type)
                : base(type)
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
