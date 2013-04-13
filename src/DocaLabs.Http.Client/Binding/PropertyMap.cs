using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    abstract class PropertyMap
    {
        public IList<IPropertyConverter> Converters { get; private set; }

        protected PropertyMap(Type type)
        {
            Converters = Parse(type);
        }

        public CustomNameValueCollection ConvertModel(object obj)
        {
            var values = new CustomNameValueCollection();

            foreach (var converter in Converters)
                values.AddRange(converter.Convert(obj));

            return values;
        }

        IList<IPropertyConverter> Parse(Type type)
        {
            return type.IsSimpleType()
                ? new List<IPropertyConverter>()
                : type.GetAllInstancePublicProperties()
                        .Select(ParseProperty)
                        .Where(x => x != null)
                        .ToList();
        }

        IPropertyConverter ParseProperty(PropertyInfo property)
        {
            return AcceptProperty(property) 
                ? property.GetConverter(GetPropertyConverterOverrides(property)) 
                : null;
        }

        protected abstract bool AcceptProperty(PropertyInfo property);

        protected virtual IPropertyConverterOverrides GetPropertyConverterOverrides(PropertyInfo property)
        {
            return null;
        }
    }
}