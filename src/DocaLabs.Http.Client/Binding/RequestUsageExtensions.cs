using System;
using System.IO;
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
                !info.IsHeader(true) &&
                !info.IsCredentials() &&
                !info.IsRequestBody();
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
        /// If checkImplicitConditions is false then it doesn't check for WebHeaderCollection on the property type.
        /// </summary>
        public static bool IsHeader(this PropertyInfo info, bool checkImplicitConditions)
        {
            if (!CanPropertyBeUsedInRequest(info))
                return false;

            var useAttribute = info.GetCustomAttribute<RequestUseAttribute>(true);

            if (useAttribute != null)
                return useAttribute.Targets.HasFlag(RequestUseTargets.RequestHeader);

            if (!checkImplicitConditions)
                return false;

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
        public static bool IsRequestBody(this PropertyInfo info)
        {
            return info.TryGetRequestSerializer() != null;
        }

        /// <summary>
        /// Returns IRequestSerialization if the property can be used to serialize into request stream.
        /// </summary>
        public static IRequestSerialization TryGetRequestSerializer(this PropertyInfo info)
        {
            var serializer = info.GetCustomAttribute<RequestSerializationAttribute>(true);
            if(serializer != null)
                return serializer;

            return typeof(Stream).IsAssignableFrom(info.PropertyType) 
                ? new SerializeStreamAttribute(info) 
                : null;
        }

        /// <summary>
        /// Returns true if the type has RequestSerializationAttribute descendant applied.
        /// </summary>
        public static bool IsSerializableToRequestBody(this Type type)
        {
            return type.GetCustomAttribute<RequestSerializationAttribute>(true) != null || typeof(Stream).IsAssignableFrom(type);
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
