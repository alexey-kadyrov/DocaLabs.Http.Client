using System.Collections.Specialized;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    public interface IValueConverter
    {
        NameValueCollection Convert(object value);
    }
}
