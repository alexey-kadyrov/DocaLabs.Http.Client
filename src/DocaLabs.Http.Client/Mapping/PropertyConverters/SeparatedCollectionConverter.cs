using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Mapping.PropertyConverters
{
    /// <summary>
    /// Converter for enumerable properties that serializes into delimited string.
    /// </summary>
    public class SeparatedCollectionConverter : PropertyConverterBase, IConvertProperty
    {
        /// <summary>
        /// String's delimiter. The default value is pipe |.
        /// </summary>
        public char Separator { get; set; }

        /// <summary>
        /// Initializes an instance of the SeparatedCollectionConverter class for a specified property.
        /// </summary>
        public SeparatedCollectionConverter(PropertyInfo info)
            : base(info)
        {
            Separator = '|';
        }

        /// <summary>
        /// Serializes the property value to the delimited string.
        /// </summary>
        /// <param name="obj">Instance of the object which "owns" the property.</param>
        /// <returns>One key-value pair with single string which contains all items.</returns>
        public IEnumerable<KeyValuePair<string, IList<string>>> GetValue(object obj)
        {
            var values = new CustomNameValueCollection();

            if (obj != null)
            {
                var collection = Info.GetValue(obj, null) as IEnumerable;

                if (collection != null)
                {
                    var stringBuilder = new StringBuilder();

                    foreach (var value in collection)
                    {
                        if (stringBuilder.Length > 0)
                            stringBuilder.Append(Separator);

                        stringBuilder.Append(ConvertValue(value));
                    }

                    if (stringBuilder.Length > 0)
                        values.Add(Name, stringBuilder.ToString());
                }
            }

            return values;
        }
    }
}
