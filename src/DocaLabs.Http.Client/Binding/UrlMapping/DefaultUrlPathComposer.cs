using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using DocaLabs.Conversion;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverters;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.UrlMapping
{
    public class DefaultUrlPathComposer : IUrlPathComposer
    {
        readonly ConcurrentDictionary<Type, PropertyMap> _orderedMaps = new ConcurrentDictionary<Type, PropertyMap>();

        public string Compose(object model, Uri baseUrl)
        {
            var existingPath = baseUrl == null 
                ? "" 
                : baseUrl.AbsolutePath;

            return Ignore(model)
                ? existingPath
                : _orderedMaps.GetOrAdd(model.GetType(), x => new PropertyMap(x)).ComposePath(model, existingPath);
        }

        static bool Ignore(object model)
        {
            return model == null || model.GetType().GetCustomAttribute<IgnoreInRequestAttribute>(true) != null;
        }

        class PropertyMap
        {
            OrderedProperties OrderedProperties { get; set; }
            NamedProperties NamedProperties { get; set; }

            public PropertyMap(Type type)
            {
                OrderedProperties = new OrderedProperties(type);
                NamedProperties = new NamedProperties(type);
            }

            public string ComposePath(object model, string existingPath)
            {
                existingPath = NamedProperties.Compose(model, existingPath);

                return OrderedProperties.Compose(model, existingPath);
            }
        }

        class OrderedProperties
        {
            IList<OrderedPropertyConverter> Converters { get; set; }

            public OrderedProperties(Type type)
            {
                Converters = Parse(type);
            }

            public string Compose(object model, string existingPath)
            {
                var modelPath = Compose(model);

                if (string.IsNullOrWhiteSpace(existingPath))
                    return modelPath;

                return string.IsNullOrWhiteSpace(modelPath)
                    ? existingPath
                    : ConcatenatePathParts(existingPath, modelPath);
            }

            static IList<OrderedPropertyConverter> Parse(Type type)
            {
                if (type.IsSimpleType())
                    return new List<OrderedPropertyConverter>();

                // thanks http://code.google.com/p/lokad-cloud/ for the idea

                var orderedCollection = type.GetAllProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(f => f.GetCustomAttribute<OrderedRequestPathAttribute>(true) != null)
                    // ordering always respect inheritance
                    .GroupBy(f => f.DeclaringType)
                        .OrderBy(g => g.Key, new InheritanceComparer())
                        .Select(g => g.OrderBy(f => f.GetCustomAttribute<OrderedRequestPathAttribute>(true).Order))
                    .SelectMany(f => f)
                    .ToList();

                return orderedCollection.Count == 0
                    ? new List<OrderedPropertyConverter>()
                    : orderedCollection.Select(ParseProperty).Where(x => x != null).ToList();
            }

            static OrderedPropertyConverter ParseProperty(PropertyInfo info)
            {
                if (info.IsUrlOrderedPath() && info.PropertyType.IsSimpleType())
                    return new OrderedPropertyConverter(info);

                return null;
            }

            string Compose(object model)
            {
                var values = Converters.Select(x => x.ConvertValue(model)).ToList();

                ValidatePathValues(values);

                return HttpUtility.UrlPathEncode(string.Join("/", GetNonEmptyValues(values)));
            }

            static void ValidatePathValues(IList<string> values)
            {
                var isPreviousValueEmpty = false;

                foreach (var value in values)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        isPreviousValueEmpty = true;
                    }
                    else
                    {
                        if (isPreviousValueEmpty)
                            throw new UnrecoverableHttpClientException(string.Format(Resources.Text.path_mapping_is_strictly_positioonal, string.Join(",", values)));
                    }
                }
            }

            static IEnumerable<string> GetNonEmptyValues(IEnumerable<string> values)
            {
                return values.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            }

            static string ConcatenatePathParts(string leftPart, string rightPart)
            {
                return leftPart.EndsWith("/")
                           ? leftPart + rightPart
                           : leftPart + "/" + rightPart;
            }
        }

        class NamedProperties
        {
            public IList<NamedPropertyConverter> Converters { get; private set; }

            public NamedProperties(Type type)
            {
                Converters = Parse(type);
            }

            public string Compose(object model, string existingPath)
            {
                if (string.IsNullOrWhiteSpace(existingPath))
                    return "";

                foreach (var converter in Converters)
                {
                    var value = converter.ConvertValue(model);

                    existingPath = existingPath.Replace(
                        "{" + converter.Name + "}", string.IsNullOrWhiteSpace(value) ? "" : HttpUtility.UrlPathEncode(value));
                }

                return existingPath;
            }

            static IList<NamedPropertyConverter> Parse(Type type)
            {
                if (type.IsSimpleType())
                    return new List<NamedPropertyConverter>();

                return type.GetAllProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(ParseProperty)
                    .Where(x => x != null)
                    .ToList();
            }

            static NamedPropertyConverter ParseProperty(PropertyInfo info)
            {
                if (info.IsUrlOrderedPath() && info.PropertyType.IsSimpleType())
                    return new NamedPropertyConverter(info);

                return null;
            }
        }
        
        class OrderedPropertyConverter
        {
            PropertyInfo Info { get; set; }

            string Format { get; set; }

            public OrderedPropertyConverter(PropertyInfo info)
            {
                Info = info;
            }

            public string ConvertValue(object model)
            {
                if (model == null)
                    return string.Empty;

                var value = Info.GetValue(model);
                if(value == null)
                    return string.Empty;

                return string.IsNullOrWhiteSpace(Format)
                           ? CustomConverter.Current.ChangeType<string>(value)
                           : string.Format("{0:" + Format + "}", value);
            }
        }

        class NamedPropertyConverter : PropertyConverterBase<NamedRequestPathAttribute>
        {
            public NamedPropertyConverter(PropertyInfo info) 
                : base(info)
            {
            }
        }

        class InheritanceComparer : IComparer<Type>
        {
            public int Compare(Type x, Type y)
            {
                return ReferenceEquals(x, y) ? 0 : (x.IsSubclassOf(y) ? 1 : -1);
            }
        }
    }
}
