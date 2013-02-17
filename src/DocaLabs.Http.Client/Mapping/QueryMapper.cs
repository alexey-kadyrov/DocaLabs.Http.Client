using System;
using System.Collections.Concurrent;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Mapping
{
    /// <summary>
    /// Defines methods to create query string from properties of a class.
    /// All members are thread safe.
    /// </summary>
    public static class QueryMapper
    {
        static ConcurrentDictionary<Type, TypeMap> ParsedTypeMaps { get; set; }

        static QueryMapper()
        {
            ParsedTypeMaps = new ConcurrentDictionary<Type, TypeMap>();
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

        static CustomNameValueCollection ToDictionary(object obj)
        {
            return ToDictionary(obj, ParsedTypeMaps.GetOrAdd(obj.GetType(), x => new TypeMap(x)));
        }

        static CustomNameValueCollection ToDictionary(object obj, TypeMap map)
        {
            var values = new CustomNameValueCollection();

            foreach (var property in map.Properties)
                values.AddRange(property.GetValue(obj));

            return values;
        }

        static string ToQueryString(CustomNameValueCollection values)
        {
            return new QueryBuilder().Add(values).ToString();
        }
    }
}
