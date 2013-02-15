using System;
using System.Collections;
using System.Collections.Generic;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Represents a collection of associated keys and collection of values for each key.
    /// </summary>
    [Serializable]
    public class DictionaryList<TKey, TValue> : IDictionaryList<TKey, TValue>
    {
        Dictionary<TKey, IList<TValue>> Inner { get; set; }

        /// <summary>
        /// Gets the number of elements contained in the dictionary.
        /// </summary>
        public int Count { get { return Inner.Count; } }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        public IList<TValue> this[TKey key]
        {
            get { return Inner[key]; }
            set { Inner[key] = value; }
        }

        /// <summary>
        /// Gets a collection containing the keys of the dictionary.
        /// </summary>
        public ICollection<TKey> Keys { get { return Inner.Keys; } }

        /// <summary>
        /// Initializes a new instance of the DictionaryList class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public DictionaryList()
        {
            Inner = new Dictionary<TKey, IList<TValue>>();
        }

        /// <summary>
        /// Initializes a new instance of the DictionaryList class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the DictionaryList can contain.</param>
        public DictionaryList(int capacity)
        {
            Inner = new Dictionary<TKey, IList<TValue>>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the DictionaryList class that is empty, has the default initial capacity, and uses the specified IEqualityComparer.
        /// </summary>
        /// <param name="comparer">The IEqualityComparer implementation to use when comparing keys, or null to use the default EqualityComparer for the type of the key.</param>
        public DictionaryList(IEqualityComparer<TKey> comparer)
        {
            Inner = new Dictionary<TKey, IList<TValue>>(comparer);
        }

        /// <summary>
        /// Initializes a new instance of the DictionaryList class that is empty, has the specified initial capacity, and uses the specified IEqualityComparer.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the DictionaryList can contain.</param>
        /// <param name="comparer">The IEqualityComparer implementation to use when comparing keys, or null to use the default EqualityComparer for the type of the key.</param>
        public DictionaryList(int capacity, IEqualityComparer<TKey> comparer)
        {
            Inner = new Dictionary<TKey, IList<TValue>>(capacity, comparer);
        }

        /// <summary>
        /// Initializes a new instance of the DictionaryList class that contains elements copied from the specified IDictionary and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionary whose elements are copied to the new DictionaryList.</param>
        public DictionaryList(IDictionary<TKey, IList<TValue>> dictionary)
        {
            Inner = new Dictionary<TKey, IList<TValue>>(dictionary);
        }

        /// <summary>
        /// Initializes a new instance of the DictionaryList class that contains elements copied from the specified IDictionaryList and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The IDictionaryList whose elements are copied to the new DictionaryList.</param>
        public DictionaryList(IDictionaryList<TKey, TValue> dictionary)
            : this(dictionary, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DictionaryList class that contains elements copied from the specified IDictionary&lt;TKey, TValue> and uses the specified IEqualityComparer.
        /// </summary>
        /// <param name="dictionary">The IDictionary whose elements are copied to the new DictionaryList.</param>
        /// <param name="comparer">The IEqualityComparer implementation to use when comparing keys, or null to use the default EqualityComparer for the type of the key.</param>
        public DictionaryList(IDictionary<TKey, IList<TValue>> dictionary, IEqualityComparer<TKey> comparer)
        {
            Inner = new Dictionary<TKey, IList<TValue>>(dictionary, comparer);
        }

        /// <summary>
        /// Initializes a new instance of the DictionaryList class that contains elements copied from the specified IDictionaryList&lt;TKey, TValue> and uses the specified IEqualityComparer.
        /// </summary>
        /// <param name="dictionary">The IDictionaryList whose elements are copied to the new DictionaryList.</param>
        /// <param name="comparer">The IEqualityComparer implementation to use when comparing keys, or null to use the default EqualityComparer for the type of the key.</param>
        public DictionaryList(IDictionaryList<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : this(comparer)
        {
            if(dictionary == null)
                throw new ArgumentNullException("dictionary");

            foreach (var key in dictionary.Keys)
                Inner.Add(key, dictionary[key]);    
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, IList<TValue>>> GetEnumerator()
        {
            return Inner.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Removes all items from the dictionary.
        /// </summary>
        public void Clear()
        {
            Inner.Clear();
        }

        /// <summary>
        /// Determines whether the dictionary contains an element with the specified key.
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            return Inner.ContainsKey(key);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the dictionary.
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            DoAdd(key, value);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the dictionary.
        /// </summary>
        public void Add(TKey key, IList<TValue> value)
        {
            DoAdd(key, value);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the DictionaryList.
        /// </summary>
        public void AddRange(IEnumerable<KeyValuePair<TKey, IList<TValue>>> collection)
        {
            if(collection == null)
                return;

            foreach (var pair in collection)
            {
                Add(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Removes the element with the specified key from the dictionary.
        /// </summary>
        public bool Remove(TKey key)
        {
            return Inner.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the dictionary contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(TKey key, out IList<TValue> value)
        {
            return Inner.TryGetValue(key, out value);
        }

        void DoAdd(TKey key, IEnumerable<TValue> values)
        {
            IList<TValue> existingValue;

            if (!Inner.TryGetValue(key, out existingValue))
            {
                existingValue = new List<TValue>();
                Inner[key] = existingValue;
            }

            if (values != null)
                existingValue.AddRange(values);
        }

        void DoAdd(TKey key, TValue value)
        {
            IList<TValue> existingValue;

            if (!Inner.TryGetValue(key, out existingValue))
            {
                existingValue = new List<TValue>();
                Inner[key] = existingValue;
            }

            existingValue.Add(value);
        }
    }
}
