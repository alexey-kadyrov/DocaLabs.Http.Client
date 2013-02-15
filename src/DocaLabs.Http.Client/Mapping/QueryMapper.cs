using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Mapping
{
    /// <summary>
    /// Defines methods to create query string from properties of a class.
    /// </summary>
    public static class QueryMapper
    {
        static ConcurrentDictionary<Type, ParsedType> ParsedTypes { get; set; }

        static QueryMapper()
        {
            ParsedTypes = new ConcurrentDictionary<Type, ParsedType>();
        }

        /// <summary>
        /// Creates a query string from an object.
        /// </summary>
        /// <param name="obj">Object which properties are used to create the query string.</param>
        /// <returns>Correctly encode query string or empty string if there is nothing to map or the obj is null.</returns>
        public static string ToQueryString(object obj)
        {
            if(obj == null)
                return "";

            var customeMapper = obj as ICustomQueryMapper;

            return customeMapper != null
                ? ToQueryString(customeMapper.ToParameterDictionary()) 
                : ToQueryString(ToDictionary(obj));
        }

        static IEnumerable<KeyValuePair<string, IList<string>>> ToDictionary(object obj)
        {
            return ToDictionary(obj, ParsedTypes.GetOrAdd(obj.GetType(), ParseType));
        }

        static string ToQueryString(IEnumerable<KeyValuePair<string, IList<string>>> values)
        {
            return new QueryBuilder().Add(values).ToString();
        }

        static ParsedType ParseType(Type type)
        {
            return ParsedType.ParseType(type);
        }

        static IEnumerable<KeyValuePair<string, IList<string>>> ToDictionary(object obj, ParsedType map)
        {
            var values = new CustomNameValueCollection();

            foreach (var property in map.Properties)
                values.AddRange(property.GetValue(obj));

            return values;
        }
    }
}
