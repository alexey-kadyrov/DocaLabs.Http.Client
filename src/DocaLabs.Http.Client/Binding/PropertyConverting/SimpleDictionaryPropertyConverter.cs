using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Converts IDictionary type properties.
    /// </summary>
    public class SimpleDictionaryPropertyConverter : IPropertyConverter 
    {
        readonly PropertyInfo _property;
        readonly IValueConverter _valueConverter;

        SimpleDictionaryPropertyConverter(PropertyInfo property)
        {
            _property = property;

            string name = null, format = null;

            var requestUse = property.GetCustomAttribute<RequestUseAttribute>();
            if (requestUse != null)
            {
                name = requestUse.Name;
                format = requestUse.Format;
            }

            if (name == null)
                name = _property.Name;

            _valueConverter = new SimpleDictionaryValueConverter(name, format);
        }

        /// <summary>
        /// Creates the converter if the specified property type:
        ///     * Is derived from IDictionary
        ///     * Is not an indexer
        /// </summary>
        public static IPropertyConverter TryCreate(PropertyInfo property)
        {
            if(property == null)
                throw new ArgumentNullException("property");

            return CanConvert(property)
                ? new SimpleDictionaryPropertyConverter(property) 
                : null;
        }

        /// <summary>
        /// Converts a property value.
        /// If the instance is null or the value of the property is null then the return collection will be empty.
        /// If the Name is not empty then it will be added to the key from the collection, e.g. key = Name + "." + itemKey.
        /// Otherwise the key from the collection is used.
        /// </summary>
        /// <param name="instance">Instance of the object on which the property is defined.</param>
        /// <param name="processed">Ignored.</param>
        /// <returns>Key-value pairs.</returns>
        public NameValueCollection Convert(object instance, ISet<object> processed)
        {
            return instance == null
                ? new NameValueCollection()
                : _valueConverter.Convert(_property.GetValue(instance));
        }

        static bool CanConvert(PropertyInfo property)
        {
            return !property.IsIndexer() && property.GetGetMethod() != null && SimpleDictionaryValueConverter.CanConvert(property.PropertyType);
        }
    }
}