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
        /// Returns true if the type is primitive, enum or string/decimal/Guid/dateTime/TimeSpan/DateTimeOffset/byte[].
        /// </summary>
        public static bool IsSimpleType(this Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            return (type.IsPrimitive ||
                    type.IsEnum ||
                    type == typeof(string) ||
                    type == typeof(decimal) ||
                    type == typeof(Guid) ||
                    type == typeof(DateTime) ||
                    type == typeof(TimeSpan) ||
                    type == typeof(DateTimeOffset) ||
                    type == typeof(byte[]));
        }

        /// <summary>
        /// Return a default value for the type, equivalent of default(string), default(int), etc.
        /// </summary>
        public static object GetDefaultValue(this Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            return type.IsValueType
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

            var attributes = attribute.AttributeType.GetCustomAttributes(typeof(AttributeUsageAttribute), true);

            return attributes.Length == 0 || attributes.Any(x => ((AttributeUsageAttribute)x).ValidOn.HasFlag(flags));
        }

        /// <summary>
        /// Gets all properties defined on the type and all interfaces that it implements.
        /// </summary>
        public static IList<PropertyInfo> GetAllProperties(this Type type, BindingFlags flags)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            flags |= BindingFlags.FlattenHierarchy;

            var list = type.GetProperties(flags).ToList();

            if (!type.IsInterface)
                return list;

            foreach (var @interface in type.GetInterfaces())
            {
                var properties = @interface.GetProperties(flags);

                list.AddRange(properties);
            }

            return list;
        }

        /// <summary>
        /// Checks whenever the type is enumerable, string or byte[] are not considered enumerable.
        /// </summary>
        public static bool IsEnumerable(this Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            if (type == typeof(string) || type == typeof(byte[]))
                return false;

            return type == typeof(IEnumerable) || type.GetInterfaces().Any(x => x == typeof(IEnumerable));
        }

        /// <summary>
        /// Returns enumerable element type, string or byte[] are not considered enumerable..
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Enumerable element type or null if the type is not enumerable.</returns>
        public static Type GetEnumerableElementType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.IsGenericTypeDefinition || (!type.IsEnumerable()))
                return null;

            if (type.IsArray)
                return type.GetElementType();

            if (type == typeof(IEnumerable))
                return typeof(object);

            if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments()[0];

            var genericEnumerable = type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return genericEnumerable != null
                ? genericEnumerable.GetGenericArguments()[0]
                : typeof(object);
        }
    }
}
