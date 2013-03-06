using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverters;

namespace DocaLabs.Http.Client.Binding.UrlMapping
{
    class NamedPropertyConverter : PropertyConverterBase<NamedRequestPathAttribute>
    {
        public NamedPropertyConverter(PropertyInfo info)
            : base(info)
        {
        }
    }
}