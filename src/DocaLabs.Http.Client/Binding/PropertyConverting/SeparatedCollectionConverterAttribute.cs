using System;
using System.Reflection;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Defines attribute to get custom converter for enumerable properties that converts into delimited string.
    /// </summary>
    public class SeparatedCollectionConverterAttribute : CustomPropertyConverterAttribute
    {
        /// <summary>
        /// String's delimiter. The default value is pipe |.
        /// </summary>
        public char Separator { get; set; }

        /// <summary>
        /// Initializes an instance of the SeparatedCollectionConverterAttribute class.
        /// </summary>
        public SeparatedCollectionConverterAttribute()
        {
            Separator = '|';
        }

        /// <summary>
        /// Returns a new instance of the SeparatedCollectionConverter class for the property.
        /// </summary>
        public override IPropertyConverter GetConverter(PropertyInfo property)
        {
            var converter = SeparatedCollectionConverter.TryCreate(property, Separator);

            if(converter == null)
                throw new ArgumentException(string.Format(Resources.Text.separated_coll_attr_cannot_be_used_for_0_in_1, property.Name, property.DeclaringType), "property");

            return converter;
        }
    }
}
