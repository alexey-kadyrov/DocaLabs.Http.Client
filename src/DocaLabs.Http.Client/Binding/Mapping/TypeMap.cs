using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverters;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.Mapping
{
    /// <summary>
    /// Defines class that contains information about properties that can be mapped.
    /// </summary>
    public class TypeMap
    {
        /// <summary>
        /// Gets parsed properties.
        /// </summary>
        public IList<IConvertProperty> Properties { get; private set; }

        /// <summary>
        /// Initializes a new instance of the TypeMap class for the specified type.
        /// </summary>
        public TypeMap(Type type)
        {
            Properties = Parse(type);
        }

        static IList<IConvertProperty> Parse(Type type)
        {
            return type.IsSimpleType()
                ? new List<IConvertProperty>()
                : type.GetAllProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(ParseProperty)
                    .Where(x => x != null)
                    .ToList();
        }

        static IConvertProperty ParseProperty(PropertyInfo info)
        {
            if (Ignore(info))
                return null;

            return TryGetCustomPropertyParser(info)
                ?? ConvertCollectionProperty.TryCreate(info)
                ?? ConvertSimpleProperty.TryCreate(info)
                ?? ConvertObjectProperty.TryCreate(info);
        }

        static bool Ignore(PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            return  info.GetIndexParameters().Length > 0 ||
                    info.GetGetMethod() == null ||
                    info.GetCustomAttribute<QueryIgnoreAttribute>(true) != null;
        }

        static IConvertProperty TryGetCustomPropertyParser(PropertyInfo info)
        {
            var attribute = info.GetCustomAttribute<QueryPropertyConverterAttribute>(true);

            return attribute != null
                ? attribute.GetConverter(info)
                : null;
        }
    }
}
