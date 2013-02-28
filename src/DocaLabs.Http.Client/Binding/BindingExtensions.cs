using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Mapping.Attributes;
using DocaLabs.Http.Client.RequestSerialization;

namespace DocaLabs.Http.Client.Binding
{
    public static class BindingExtensions
    {
        public static bool CanGoToUrl(this PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            return info.GetIndexParameters().Length == 0 &&
                    info.GetGetMethod() != null &&
                    (!info.IsHeaderCollection()) &&
                    (!info.IsCredentials()) &&
                    info.GetCustomAttribute<QueryIgnoreAttribute>(true) == null &&
                    info.GetCustomAttribute<RequestSerializationAttribute>(true) == null;
        }

        public static bool IsHeaderCollection(this PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            return info.GetIndexParameters().Length == 0 &&
                   info.GetGetMethod() != null &&
                   (typeof (WebHeaderCollection).IsAssignableFrom(info.PropertyType) || info.GetCustomAttribute<QueryHeaderAttribute>() != null);
        }

        public static bool IsCredentials(this PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            return info.GetIndexParameters().Length == 0 &&
                   info.GetGetMethod() != null &&
                   typeof(ICredentials).IsAssignableFrom(info.PropertyType);
        }
    }
}
