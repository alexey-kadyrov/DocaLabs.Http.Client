using System;

namespace DocaLabs.Http.Client.Binding.Hints
{
    /// <summary>
    /// Indicates that a property must be ignored when serializing into a URI's query.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class IgnoreInRequestAttribute : Attribute
    {
    }
}
