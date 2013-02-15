using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DocaLabs.Http.Client.Utils
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Returns true if the type is primitive or string/decimal/Guid/dateTime/TimeSpan/DateTimeOffset.
        /// </summary>
        public static bool IsSimpleType(this Type type)
        {
            return (type == typeof(string) ||
                    type.IsPrimitive ||
                    type == typeof(decimal) ||
                    type == typeof(Guid) ||
                    type == typeof(DateTime) ||
                    type == typeof(TimeSpan) ||
                    type == typeof(DateTimeOffset) ||
                    type.IsEnum);
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

        public static bool IsValidOn(this CustomAttributeData data, AttributeTargets flags)
        {
            var attributes = data.AttributeType.GetCustomAttributes(typeof(AttributeUsageAttribute), true);

            return attributes.Length == 0 || attributes.Any(x => ((AttributeUsageAttribute)x).ValidOn.HasFlag(flags));
        }

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

        public static bool Exists(this IEnumerable<PropertyInfo> collection, PropertyInfo property)
        {
            var name = property.Name;
            var propertType = property.PropertyType;
            var parameters = property.GetIndexParameters();

            return collection.Any(
                    existingProperty => existingProperty.Name == name 
                    && existingProperty.PropertyType == propertType 
                    && existingProperty.GetIndexParameters().Compare(parameters));
        }

        static bool Compare(this ICollection<ParameterInfo> left, IList<ParameterInfo> right)
        {
            if (left.Count != right.Count)
                return false;

            return !left.Where((t, i) => t.ParameterType != right[i].ParameterType).Any();
        }
    }
}
