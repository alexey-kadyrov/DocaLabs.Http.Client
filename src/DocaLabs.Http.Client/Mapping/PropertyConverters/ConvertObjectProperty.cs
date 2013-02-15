using System.Collections.Generic;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Mapping.PropertyConverters
{
    public class ConvertObjectProperty : PropertyConverterBase, IConvertProperty
    {
        ConvertObjectProperty(PropertyInfo info)
            : base(info)
        {
        }

        public static IConvertProperty TryParse(PropertyInfo info)
        {
            return info.PropertyType.IsPrimitive
                ? new ConvertObjectProperty(info)
                : null;
        }

        public IEnumerable<KeyValuePair<string, IList<string>>> GetValue(object obj)
        {
            IEnumerable<KeyValuePair<string, IList<string>>> values = null;

            var value = Info.GetValue(obj, null);

            if (value != null)
            {
                var customeMapper = obj as ICustomQueryMapper;
                if (customeMapper != null)
                    values = customeMapper.ToParameterDictionary();
            }

            return values ?? new CustomNameValueCollection();
        }
    }
}
