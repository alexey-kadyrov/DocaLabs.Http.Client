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
            return info.PropertyType.IsSimpleType()
                ? null
                : new ConvertObjectProperty(info);
        }

        public IEnumerable<KeyValuePair<string, IList<string>>> GetValue(object obj)
        {
            IEnumerable<KeyValuePair<string, IList<string>>> values = null;

            if (obj != null)
            {
                var value = Info.GetValue(obj, null);

                if (value != null)
                {
                    var customeMapper = obj as ICustomQueryMapper;

                    values = customeMapper != null
                                 ? customeMapper.ToParameterDictionary()
                                 : new CustomNameValueCollection {{Name, obj.ToString()}};
                }
            }

            return values ?? new CustomNameValueCollection();
        }
    }
}
