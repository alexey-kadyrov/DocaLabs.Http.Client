namespace DocaLabs.Http.Client.Binding.Attributes
{
    /// <summary>
    /// Marks property to be mapped into URL's path part. The value is appended to the path according it's order.
    /// </summary>
    public class OrderedRequestPathAttribute : RequestPathAttribute
    {
        /// <summary>
        /// Gets or sets an order in which property should be mapped to the query.
        /// </summary>
        public int Order { get; set; }

        public OrderedRequestPathAttribute()
        {
        }

        public OrderedRequestPathAttribute(int order)
        {
            Order = order;
        }
    }
}
