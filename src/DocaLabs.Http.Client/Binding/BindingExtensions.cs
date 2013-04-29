using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Hints;
using DocaLabs.Http.Client.Binding.RequestSerialization;
using DocaLabs.Http.Client.Binding.Utils;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Binding extensions.
    /// </summary>
    public static class BindingExtensions
    {
        /// <summary>
        /// Returns true if the property is can pass its data to the either query or path part of URL.
        /// </summary>
        public static bool IsImplicitUrlPathOrQuery(this PropertyInfo info)
        {
            return CanPropertyBeUsedInRequest(info) &&
                info.PropertyType.IsSimpleType() &&
                !info.IsExplicitUrlPath() &&
                !info.IsExplicitUrlQuery() &&
                !info.IsHeader() &&
                !info.IsCredentials();
        }

        /// <summary>
        /// Returns true if the property is defined to pass its data to the path part of URL.
        /// </summary>
        public static bool IsExplicitUrlPath(this PropertyInfo info)
        {
            return CanPropertyBeUsedInRequest(info) &&
                info.GetCustomAttribute<InRequestPathAttribute>(true) != null;
        }

        /// <summary>
        /// Returns true if the property is defined to pass its data to the query part of URL.
        /// </summary>
        public static bool IsExplicitUrlQuery(this PropertyInfo info)
        {
            return CanPropertyBeUsedInRequest(info) &&
                info.GetCustomAttribute<InRequestQueryAttribute>(true) != null;
        }

        /// <summary>
        /// Returns true if the property is defined to pass its data to request headers.
        /// </summary>
        public static bool IsHeader(this PropertyInfo info)
        {
            return CanPropertyBeUsedInRequest(info) &&
                ((typeof(WebHeaderCollection).IsAssignableFrom(info.PropertyType) && info.GetCustomAttribute<RequestSerializationAttribute>(true) == null)
                    || info.GetCustomAttribute<InRequestHeaderAttribute>() != null);
        }

        /// <summary>
        /// Returns true if the property is defined to be used as request credentials.
        /// </summary>
        public static bool IsCredentials(this PropertyInfo info)
        {
            return CanPropertyBeUsedInRequest(info) &&
                (typeof(ICredentials).IsAssignableFrom(info.PropertyType) && info.GetCustomAttribute<RequestSerializationAttribute>(true) == null);
        }

        /// <summary>
        /// Returns true if the property can be used in form serialization.
        /// </summary>
        public static bool IsFormProperty(this PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            return  info.GetIndexParameters().Length == 0 && 
                info.GetGetMethod() != null && 
                info.PropertyType.IsSimpleType();
        }

        static bool CanPropertyBeUsedInRequest(PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            return info.GetIndexParameters().Length == 0 &&
                info.GetGetMethod() != null &&
                info.GetCustomAttribute<IgnoreInRequestAttribute>(true) == null;
        }
    }
}
