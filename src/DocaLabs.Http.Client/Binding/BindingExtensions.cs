using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.RequestSerialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public static class BindingExtensions
    {
        public static bool IsExplicitUrlPath(this PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            return info.GetIndexParameters().Length == 0 &&
                    info.GetGetMethod() != null &&
                    (!info.IsHeader()) &&
                    (!info.IsCredentials()) &&
                    info.GetCustomAttribute<RequestPathAttribute>(true) != null &&
                    info.GetCustomAttribute<IgnoreInRequestAttribute>(true) == null &&
                    info.GetCustomAttribute<RequestSerializationAttribute>(true) == null;
        }

        public static bool IsExplicitUrlQuery(this PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            return info.GetIndexParameters().Length == 0 &&
                    info.GetGetMethod() != null &&
                    (!info.IsHeader()) &&
                    (!info.IsCredentials()) &&
                    info.GetCustomAttribute<RequestQueryAttribute>(true) != null &&
                    info.GetCustomAttribute<IgnoreInRequestAttribute>(true) == null &&
                    info.GetCustomAttribute<RequestSerializationAttribute>(true) == null;
        }

        public static bool IsImplicitUrlPathOrQuery(this PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            return info.GetIndexParameters().Length == 0 &&
                    info.GetGetMethod() != null &&
                    (!info.IsHeader()) &&
                    (!info.IsCredentials()) &&
                    info.GetCustomAttributes(typeof(RequestPathAttribute), true).Length == 0 &&
                    info.GetCustomAttribute<RequestQueryAttribute>(true) == null &&
                    info.GetCustomAttribute<IgnoreInRequestAttribute>(true) == null &&
                    info.GetCustomAttribute<RequestSerializationAttribute>(true) == null;
        }

        public static bool IsHeader(this PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            return info.GetIndexParameters().Length == 0 &&
                    info.GetGetMethod() != null &&
                    info.GetCustomAttribute<IgnoreInRequestAttribute>(true) == null &&
                    info.GetCustomAttribute<RequestSerializationAttribute>(true) == null &&
                    (typeof(WebHeaderCollection).IsAssignableFrom(info.PropertyType) || info.GetCustomAttribute<RequestHeaderAttribute>() != null);
        }

        public static bool IsCredentials(this PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            return info.GetIndexParameters().Length == 0 &&
                    info.GetGetMethod() != null &&
                    info.GetCustomAttribute<IgnoreInRequestAttribute>(true) == null &&
                    info.GetCustomAttribute<RequestSerializationAttribute>(true) == null &&
                    typeof(ICredentials).IsAssignableFrom(info.PropertyType);
        }

        public static bool IsFormProperty(this PropertyInfo info)
        {
            return info.GetIndexParameters().Length == 0 && info.GetGetMethod() != null && info.PropertyType.IsSimpleType();
        }

    }
}
