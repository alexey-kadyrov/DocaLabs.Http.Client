using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converter for enumerable properties that serializes their values into delimited string.
    /// </summary>
    public class SeparatedCollectionConverter : IPropertyConverter
    {
        readonly PropertyInfo _property;
        readonly string _name;
        readonly string _format;

        /// <summary>
        /// String's delimiter.
        /// </summary>
        public char Separator { get; set; }

        SeparatedCollectionConverter(PropertyInfo property, char separator)
        {
            _property = property;
            Separator = separator;

            var requestUse = property.GetCustomAttribute<RequestUseAttribute>();
            if (requestUse != null)
            {
                _name = requestUse.Name;
                _format = requestUse.Format;
            }

            if (string.IsNullOrWhiteSpace(_name))
                _name = _property.Name;
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Implements IEnumerable (but it should not be string or byte[] which are considered simple types)
        ///     * The enumerable element type is simple
        ///     * Is not an indexer
        /// </summary>
        public static IPropertyConverter TryCreate(PropertyInfo property, char separator = '|')
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var type = property.PropertyType;

            return CanConvert(property, type)
                ? new SeparatedCollectionConverter(property, separator)
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// If the value of the property is null (or eventually empty string) then the return collection will be empty.
        /// </summary>
        /// <param name="instance">Instance of the object on which the property is defined.</param>
        /// <param name="processed">Ignored.</param>
        /// <returns>One key-value pair with single string as value which contains all items separated by the provided separator.</returns>
        public NameValueCollection Convert(object instance, ISet<object> processed)
        {
            var values = new NameValueCollection();

            if (instance != null)
            {
                var collection = _property.GetValue(instance) as IEnumerable;
                if (collection != null)
                    TryBuildString(values, collection);
            }

            return values;
        }

        void TryBuildString(NameValueCollection values, IEnumerable collection)
        {
            var stringBuilder = new StringBuilder();

            var first = true;

            foreach (var item in collection)
            {
                if (!first)
                    stringBuilder.Append(Separator);

                stringBuilder.Append(CustomConverter.ChangeToString(_format, item));

                first = false;
            }

            if (stringBuilder.Length > 0)
                values.Add(_name, stringBuilder.ToString());
        }

        static bool CanConvert(PropertyInfo property, Type type)
        {
            return type.IsEnumerable() && type.GetEnumerableElementType().IsSimpleType() && property.GetIndexParameters().Length == 0;
        }
    }
}
