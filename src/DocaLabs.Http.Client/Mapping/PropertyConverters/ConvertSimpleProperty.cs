using System.Collections.Generic;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Mapping.PropertyConverters
{

    public class ConvertSimpleProperty : PropertyConverterBase, IConvertProperty
    {
        ConvertSimpleProperty(PropertyInfo info)
            : base(info)
        {
        }

        public static IConvertProperty TryParse(PropertyInfo info)
        {
            return info.PropertyType.IsSimpleType()
                ? new ConvertSimpleProperty(info) 
                : null;
        }

        public IEnumerable<KeyValuePair<string, IList<string>>> GetValue(object obj)
        {
            var values = new CustomNameValueCollection();

            if (obj != null)
            {
                var value = Info.GetValue(obj, null);

                if (value != null)
                    values.Add(Name, ConvertValue(value));
            }

            return values;
        }
    }
}
