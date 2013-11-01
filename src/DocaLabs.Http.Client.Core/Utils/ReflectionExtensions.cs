using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Reflection extension methods.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Return true if the property is an indexer.
        /// </summary>
        public static bool IsIndexer(this PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            return property.GetIndexParameters().Length > 0;
        }

        /// <summary>
        /// Returns true if the type is primitive, enum or string/decimal/Guid/dateTime/TimeSpan/DateTimeOffset/byte[].
        /// </summary>
        public static bool IsSimpleType(this Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            var typeInfo = type.GetTypeInfo();

            var isSimple = (typeInfo.IsPrimitive ||
                    typeInfo.IsEnum ||
                    type == typeof(string) ||
                    type == typeof(decimal) ||
                    type == typeof(Guid) ||
                    type == typeof(DateTime) ||
                    type == typeof(TimeSpan) ||
                    type == typeof(DateTimeOffset) ||
                    type == typeof(byte[]));

            return isSimple ||
                   (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(type).IsSimpleType());
        }

        /// <summary>
        /// Return a default value for the type, equivalent of default(string), default(int), etc.
        /// </summary>
        public static object GetDefaultValue(this Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            return type.GetTypeInfo().IsValueType
                ? Activator.CreateInstance(type)
                : null;
        }

        /// <summary>
        /// Checks whenever the attribute can be used on the specified targets.
        /// </summary>
        public static bool IsValidOn(this CustomAttributeData attribute, AttributeTargets flags)
        {
            if(attribute == null)
                throw new ArgumentNullException("attribute");

            var attributes = attribute.AttributeType.GetTypeInfo().GetCustomAttributes(typeof(AttributeUsageAttribute), true).ToArray();

            return attributes.Length == 0 || attributes.Any(x => ((AttributeUsageAttribute)x).ValidOn.HasFlag(flags));
        }

        /// <summary>
        /// Gets all public instance properties defined on the type and all interfaces that it implements.
        /// Useful for interfaces as when you define interface inheritance chain the "sub-interface" 
        /// is not derived from them - it "implements" them.
        /// </summary>
        public static IList<PropertyInfo> GetAllPublicInstanceProperties(this Type type)
        {
            //return type.GetAllProperties(BindingFlags.Public | BindingFlags.Instance);
            if (type == null)
                throw new ArgumentNullException("type");

            var typeInfo = type.GetTypeInfo();

            var list = type.GetRuntimeProperties().Where(x => x.IsInstanceAndPublic()).ToList();

            if (!typeInfo.IsInterface)
                return list;

            foreach (var @interface in type.GetTypeInfo().ImplementedInterfaces)
            {
                var properties = @interface.GetRuntimeProperties().Where(x => x.IsInstanceAndPublic()).ToList();

                list.AddRange(properties);
            }

            return list;

        }

        static bool IsInstanceAndPublic(this PropertyInfo property)
        {
            var method = property.GetMethod ?? property.SetMethod;

            return method != null && !method.IsStatic && method.IsPublic;
        }

        /// <summary>
        /// Checks whenever the type is enumerable, note that string or byte[] are not considered enumerable.
        /// </summary>
        public static bool IsEnumerable(this Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            if (type == typeof(string) || type == typeof(byte[]))
                return false;

            return type == typeof(IEnumerable) || type.GetTypeInfo().ImplementedInterfaces.Any(x => x == typeof(IEnumerable));
        }

        /// <summary>
        /// Returns enumerable element type, note that string or byte[] are not considered enumerable.
        /// </summary>
        /// <returns>Enumerable element type or null if the type is not enumerable.</returns>
        public static Type GetEnumerableElementType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsGenericTypeDefinition || (!type.IsEnumerable()))
                return null;

            if (type.IsArray)
                return type.GetElementType();

            if (type == typeof(IEnumerable))
                return typeof(object);

            if (typeInfo.IsInterface && typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GenericTypeArguments[0];

            var genericEnumerable = typeInfo.ImplementedInterfaces.FirstOrDefault(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return genericEnumerable != null
                ? genericEnumerable.GenericTypeArguments[0]
                : typeof(object);
        }

        /// <summary>
        /// Returns the wrapped model type if the type is subclass of Response{} or null if it's not.
        /// </summary>
        public static Type TryGetWrappedResponseModelType(this Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(RichResponse<>))
                return type.GenericTypeArguments[0];

            if (typeInfo.BaseType == null || typeInfo.BaseType == typeof(object))
                return null;

            return TryGetWrappedResponseModelType(typeInfo.BaseType);
        }
    }
}
