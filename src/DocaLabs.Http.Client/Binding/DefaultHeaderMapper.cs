using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultHeaderMapper : IHeaderMapper
    {
        ConcurrentDictionary<Type, PropertyMap> ParsedMaps { get; set; }

        public DefaultHeaderMapper()
        {
            ParsedMaps = new ConcurrentDictionary<Type, PropertyMap>();
        }

        public WebHeaderCollection Map(object model)
        {
            return model == null ? 
                new WebHeaderCollection() 
                : GetHeaders(model, ParsedMaps.GetOrAdd(model.GetType(), x => new PropertyMap(x)));
        }

        static WebHeaderCollection GetHeaders(object model, PropertyMap map)
        {
            var collection = new WebHeaderCollection();

            foreach (var headers in map.Headers.Select(x => x.GetValue(model)).OfType<WebHeaderCollection>())
                collection.Add(headers);

            return collection;
        }

        class PropertyMap
        {
            public IList<PropertyInfo> Headers { get; private set; }

            public PropertyMap(Type type)
            {
                Headers = Parse(type);
            }

            static IList<PropertyInfo> Parse(Type type)
            {
                return type.IsSimpleType()
                           ? new List<PropertyInfo>()
                           : type.GetAllProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(x => x.IsHeaderCollection())
                                 .ToList();
            }
        }
    }
}
