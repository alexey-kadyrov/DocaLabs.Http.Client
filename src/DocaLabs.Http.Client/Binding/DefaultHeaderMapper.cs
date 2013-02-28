using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Mapping.PropertyConverters;
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

            foreach (var items in map.Headers)
            {
                var property = items as PropertyInfo;
                if (property != null)
                {
                    var headers = property.GetValue(model) as WebHeaderCollection;
                    if (headers != null)
                        collection.Add(headers);
                }
                else
                {
                    var converter = items as IConvertProperty;
                    if (converter != null)
                    {
                        var values = converter.GetValue(model);
                        if (values != null)
                        {
                            foreach (var key in values.Keys)
                            {
                                foreach (var value in values[key])
                                {
                                    collection.Add(key, value);
                                }
                            }
                        }
                    }
                }
            }

            return collection;
        }

        class PropertyMap
        {
            public IList<object> Headers { get; private set; }

            public PropertyMap(Type type)
            {
                Headers = Parse(type);
            }

            static IList<object> Parse(Type type)
            {
                return type.IsSimpleType()
                           ? new List<object>()
                           : type.GetAllProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(x => x.IsHeaderCollection())
                                 .Cast<object>()
                                 .ToList();
            }
        }
    }
}
