using System.Collections.Specialized;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    public class SimpleValueConverter : IValueConverter
    {
        readonly string _name;
        readonly string _format;

        public SimpleValueConverter(string name, string format)
        {
            _name = name;
            _format = format;
        }

        public NameValueCollection Convert(object value)
        {
            var values = new NameValueCollection();

            if (value != null)
                values.Add(_name, CustomConverter.ChangeToString(_format, value));

            return values;
        }
    }
}
