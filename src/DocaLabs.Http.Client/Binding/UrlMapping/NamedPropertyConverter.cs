using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverters;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.UrlMapping
{
    class NamedPropertyConverter : PropertyConverterBase<NamedRequestPathAttribute>
    {
        public NamedPropertyConverter(PropertyInfo info)
            : base(info)
        {
        }

        public string GetValue(object model)
        {
            if (model == null)
                return string.Empty;

            var value = Info.GetValue(model);
            if (value == null)
                return string.Empty;

            return string.IsNullOrWhiteSpace(Format)
                       ? CustomConverter.Current.ChangeType<string>(value)
                       : string.Format("{0:" + Format + "}", value);
        }
    }
}