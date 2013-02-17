using System;

namespace DocaLabs.Http.Client.Mapping.Attributes
{
    /// <summary>
    /// Indicates that a property must be ignored when serializing into a URI's query.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class QueryIgnoreAttribute : Attribute
    {
    }
}
