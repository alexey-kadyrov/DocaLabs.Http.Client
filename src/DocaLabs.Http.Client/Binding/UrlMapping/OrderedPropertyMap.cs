using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.UrlMapping
{
    class OrderedPropertyMap
    {
        IList<OrderedPropertyConverter> Converters { get; set; }

        public OrderedPropertyMap(Type type)
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

            var orderedCollection = type.GetAllInstancePublicProperties()
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
}