using System.Reflection;
using DocaLabs.Conversion;

namespace DocaLabs.Http.Client.Binding.UrlMapping
{
    public class OrderedPropertyConverter
    {
        PropertyInfo Info { get; set; }

        string Format { get; set; }

        public OrderedPropertyConverter(PropertyInfo info)
        {
            Info = info;
        }

        public string ConvertValue(object model)
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