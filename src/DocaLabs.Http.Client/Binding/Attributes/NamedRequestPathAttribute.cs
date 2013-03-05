using DocaLabs.Http.Client.Binding.PropertyConverters;

namespace DocaLabs.Http.Client.Binding.Attributes
{
    /// <summary>
    /// Marks property to be mapped into URL's path part. 
    /// If the name is specified then the property name is used to look for a substitution mask.
    /// The name name can be overridden using the Name.
    /// The Url template can be specified like: http://contoso.com/{propertyName1}{propertyName2}.
    /// </summary>
    public class NamedRequestPathAttribute : RequestPathAttribute, INamedPropertyConverterInfo
    {
        /// <summary>
        /// Gets or sets a name which defines the substitution lookup.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes an instance of the NamedRequestPathAttribute class.
        /// </summary>
        public NamedRequestPathAttribute()
        {
        }

        /// <summary>
        /// Initializes an instance of the NamedRequestPathAttribute class with specified substitution lookup name.
        /// </summary>
        public NamedRequestPathAttribute(string name)
        {
            Name = name;
        }
    }
}
