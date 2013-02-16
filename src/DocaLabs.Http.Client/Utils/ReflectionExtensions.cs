using System;
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
            return type.IsValueType
                ? Activator.CreateInstance(type)
                : null;
        }

        /// <summary>
        /// Checks whenever the attribute can be used on the specified targets.
        /// </summary>
        public static bool IsValidOn(this CustomAttributeData attribute, AttributeTargets flags)
        {
            var attributes = attribute.AttributeType.GetCustomAttributes(typeof(AttributeUsageAttribute), true);

            return attributes.Length == 0 || attributes.Any(x => ((AttributeUsageAttribute)x).ValidOn.HasFlag(flags));
        }

        /// <summary>
        /// Gets all properties defined on the type and all interfaces that it implements.
        /// </summary>
        public static IList<PropertyInfo> GetAllProperties(this Type type, BindingFlags flags)
        {
            flags |= BindingFlags.FlattenHierarchy;

            var list = type.GetProperties(flags).ToList();

            if (!type.IsInterface)
                return list;

            foreach (var @interface in type.GetInterfaces())
            {
                var properties = @interface.GetProperties(flags).Where(x => (!x.IsSpecialName) && (!list.Exists(x)));

                list.AddRange(properties);
            }

            return list;
        }

        static bool Exists(this IEnumerable<PropertyInfo> collection, PropertyInfo property)
        {
            var name = property.Name;
            var propertType = property.PropertyType;
            var parameters = property.GetIndexParameters();

            return collection.Any(x => x.Name == name && x.PropertyType == propertType && x.GetIndexParameters().Compare(parameters));
        }

        static bool Compare(this ICollection<ParameterInfo> left, IList<ParameterInfo> right)
        {
            if (left.Count != right.Count)
                return false;

            return !left.Where((t, i) => t.ParameterType != right[i].ParameterType).Any();
        }
    }
}
