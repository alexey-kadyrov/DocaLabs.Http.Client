using System.Reflection;
using DocaLabs.Http.Client.Binding.Mapping.PropertyConverters;

namespace DocaLabs.Http.Client.Binding.Mapping.Attributes
{
    /// <summary>
    /// Defines converter for enumerable properties that serializes into delimited string.
    /// </summary>
    public class SeparatedCollectionConverterAttribute : QueryPropertyConverterAttribute
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
        /// Returns a new instance of the SeparatedCollectionConverter class.
        /// </summary>
        public override IConvertProperty GetConverter(PropertyInfo info)
        {
            return new SeparatedCollectionConverter(info)
            {
                Separator = Separator
            };
        }
    }
}
