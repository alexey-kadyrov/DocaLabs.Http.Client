using System;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.UrlComposing
{
    class OrderedPropertyConverter
    {
        PropertyInfo Info { get; set; }

        string Format { get; set; }

        public OrderedPropertyConverter(PropertyInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            Info = info;

            var attribute = info.GetCustomAttribute<RequestPathAttribute>(true);
            if (attribute != null)
                Format = attribute.Format;
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