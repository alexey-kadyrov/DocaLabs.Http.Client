using System.Collections.Generic;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Represents a collection of associated keys and collection of values for each key.
    /// </summary>
    public interface IDictionaryList<TKey, TValue> : IEnumerable<KeyValuePair<TKey, IList<TValue>>>
    {
        /// <summary>
        /// Gets the number of elements contained in the dictionary.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets a collection containing the keys of the dictionary.
        /// </summary>
        ICollection<TKey> Keys { get; }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        IList<TValue> this[TKey key] { get; set; }

        /// <summary>
        /// Removes all items from the dictionary.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether the dictionary contains an element with the specified key.
        /// </summary>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Adds an element with the provided key and value to the dictionary.
        /// </summary>
        void Add(TKey key, TValue value);

        /// <summary>
        /// Adds an element with the provided key and value to the dictionary.
        /// </summary>
        void Add(TKey key, IList<TValue> value);

        /// <summary>
        /// Removes the element with the specified key from the dictionary.
        /// </summary>
        bool Remove(TKey key);

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the dictionary contains an element with the specified key; otherwise, false.
        /// </returns>
        bool TryGetValue(TKey key, out IList<TValue> value);

        /// <summary>
        /// Adds the elements of the specified collection to the DictionaryList.
        /// </summary>
        void AddRange(IEnumerable<KeyValuePair<TKey, IList<TValue>>> collection);
    }
}