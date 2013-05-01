using System;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Marks property to be mapped into URL's path part. 
    /// If the name is specified then the property name is used to look for a substitution mask.
    /// The name name can be overridden using the Name. The mask is case insensitive.
    /// The Url template can be specified like: http://contoso.com/{propertyName1}/{propertyName2}.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class InRequestPathAttribute : Attribute, IPropertyConverterOverrides
    {
        /// <summary>
        /// Gets or sets a format string that should be used when converting the property value.
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// Gets or sets a name which defines the substitution lookup.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes an instance of the InRequestPathAttribute class.
        /// </summary>
        public InRequestPathAttribute()
        {
        }

        /// <summary>
        /// Initializes an instance of the InRequestPathAttribute class with specified substitution lookup name.
        /// </summary>
        public InRequestPathAttribute(string name)
        {
            Name = name;
        }
    }
}
