using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Property binding extensions to determine how a property should be serialized.
    /// </summary>
    public static class RequestUsageExtensions
    {
        /// <summary>
        /// Returns true if the property is can pass its data to the either query or path part of URL.
        /// </summary>
        public static bool IsImplicitUrlPathOrQuery(this PropertyInfo info)
        {
            return CanPropertyBeUsedInRequest(info) &&
                info.GetCustomAttribute<RequestSerializationAttribute>(true) == null &&
                !info.IsExplicitUrlPath() &&
                !info.IsExplicitUrlQuery() &&
                !info.IsHeader() &&
                !info.IsCredentials() &&
                !info.IsRequestStream();
        }

        /// <summary>
        /// Returns true if the property is defined to pass its data to the path part of URL.
        /// </summary>
        public static bool IsExplicitUrlPath(this PropertyInfo info)
        {
            if (!CanPropertyBeUsedInRequest(info))
                return false;

            var useAttribute = info.GetCustomAttribute<RequestUseAttribute>(true);

            return useAttribute != null && useAttribute.Targets.HasFlag(RequestUseTargets.UrlPath);
        }

        /// <summary>
        /// Returns true if the property is defined to pass its data to the query part of URL.
        /// </summary>
        public static bool IsExplicitUrlQuery(this PropertyInfo info)
        {
            if (!CanPropertyBeUsedInRequest(info))
                return false;

            var useAttribute = info.GetCustomAttribute<RequestUseAttribute>(true);

            return useAttribute != null && useAttribute.Targets.HasFlag(RequestUseTargets.UrlQuery);
        }

        /// <summary>
        /// Returns true if the property is defined to pass its data to request headers.
        /// </summary>
        public static bool IsHeader(this PropertyInfo info)
        {
            if (!CanPropertyBeUsedInRequest(info))
                return false;

            var useAttribute = info.GetCustomAttribute<RequestUseAttribute>(true);

            if (useAttribute != null)
                return useAttribute.Targets.HasFlag(RequestUseTargets.RequestHeader);

            return 
                typeof (WebHeaderCollection).IsAssignableFrom(info.PropertyType) && 
                info.GetCustomAttribute<RequestSerializationAttribute>(true) == null;
        }

        /// <summary>
        /// Returns true if the property is defined to be used as request credentials.
        /// </summary>
        public static bool IsCredentials(this PropertyInfo info)
        {
            return CanPropertyBeUsedInRequest(info) &&
                (typeof(ICredentials).IsAssignableFrom(info.PropertyType) && 
                info.GetCustomAttribute<RequestUseAttribute>(true) == null &&
                info.GetCustomAttribute<RequestSerializationAttribute>(true) == null);
        }

        /// <summary>
        /// Returns true if the property can be used to serialize into request stream.
        /// </summary>
        public static bool IsRequestStream(this PropertyInfo info)
        {
            return info.TryGetRequestSerializer() != null;
        }

        /// <summary>
        /// Returns IRequestSerialization if the property can be used to serialize into request stream.
        /// </summary>
        public static IRequestSerialization TryGetRequestSerializer(this PropertyInfo info)
        {
            return !CanPropertyBeUsedInRequest(info) ? null : info.GetCustomAttribute<RequestSerializationAttribute>(true);
        }

        static bool CanPropertyBeUsedInRequest(PropertyInfo info)
        {
            // We don't do indexers, as in general it's impossible to guess what would be the required index parameters
            if (info.IsIndexer() || info.GetGetMethod() == null)
                return false;

            var useAttribute = info.GetCustomAttribute<RequestUseAttribute>(true);

            return useAttribute == null || (!useAttribute.Targets.HasFlag(RequestUseTargets.Ignore));
        }
    }
}
