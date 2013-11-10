using System;
using System.Collections;
using System.Collections.Generic;

namespace DocaLabs.Http.Client.Utils
{
    public class CustomKeyValueCollection : ICustomKeyValueCollection
    {
        readonly Dictionary<string, List<string>> _data = new Dictionary<string, List<string>>();

        public IReadOnlyCollection<string> AllKeys { get { return new ReadOlnyKeys(_data.Keys); } }

        public IReadOnlyList<string> GetValues(string key)
        {
            List<string> values;
            return _data.TryGetValue(key, out values) 
                ? new ReadOlnyValues(values) 
                : new ReadOlnyValues();
        }

        public void Add(string key, string value)
        {
            List<string> values;
            if (!_data.TryGetValue(key, out values))
                _data[key] = values = new List<string>();

            values.Add(value);
        }

        public void Add(ICustomKeyValueCollection collection)
        {
            if(collection == null)
                throw new ArgumentNullException("collection");

            foreach (var key in collection)
            {
                List<string> existingValues;
                if (!_data.TryGetValue(key, out existingValues))
                    _data[key] = existingValues = new List<string>();

                existingValues.AddRange(collection.GetValues(key));
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _data.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class ReadOlnyKeys : IReadOnlyCollection<string>
        {
            readonly ICollection<string> _keys;

            public ReadOlnyKeys(ICollection<string> keys)
            {
                _keys = keys;
            }

            public IEnumerator<string> GetEnumerator()
            {
                return _keys.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public int Count { get { return _keys.Count; } }
        }

        class ReadOlnyValues : IReadOnlyList<string>
        {
            readonly IList<string> _values;

            public ReadOlnyValues()
            {
                _values = new List<string>();
            }

            public ReadOlnyValues(IList<string> values)
            {
                _values = values;
            }

            public IEnumerator<string> GetEnumerator()
            {
                return _values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public int Count { get { return _values.Count; } }

            public string this[int index]
            {
                get { return _values[index]; }
            }
        }
    }
}
