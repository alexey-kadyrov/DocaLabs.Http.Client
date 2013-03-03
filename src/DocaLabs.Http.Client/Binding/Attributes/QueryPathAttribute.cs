using System;

namespace DocaLabs.Http.Client.Binding.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class QueryPathAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets an order in which property should be mapped to the query.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets a format string that should be used when converting the property value.
        /// </summary>
        public string Format { get; set; }

        public QueryPathAttribute()
        {
        }

        public QueryPathAttribute(int order)
        {
            Order = order;
        }
    }
}
