using System;

namespace DocaLabs.Http.Client.Utils
{
    public interface ICustomConcurrentDictionary<TKey, TValue>
    {
        TValue this[TKey key] { get; set; }
        TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);
        bool Remove(TKey key);
        bool TryGetValue(TKey key, out TValue value);
    }
}
