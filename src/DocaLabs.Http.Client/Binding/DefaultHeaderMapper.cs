using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.PropertyConverting;

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultHeaderMapper
    {
        readonly ConcurrentDictionary<Type, TypeConverter> _maps = new ConcurrentDictionary<Type, TypeConverter>();

        public WebHeaderCollection Map(object model)
        {
            return Ignore(model) 
                ? new WebHeaderCollection()
                : GetHeaders(GetMap(model).Convert(model));
        }

        TypeConverter GetMap(object model)
        {
            return _maps.GetOrAdd(model.GetType(), x => new TypeConverter(x, PropertyInfoExtensions.IsHeader));
        }

        static bool Ignore(object model)
        {
            if (model == null)
                return false;

            var useAttribute = model.GetType().GetCustomAttribute<UseAttribute>(true);

            return useAttribute != null && useAttribute.Usage == RequestUsage.Ignore;
        }

        static WebHeaderCollection GetHeaders(NameValueCollection headers)
        {
            return new WebHeaderCollection
            {
                headers
            };
        }
    }
}
