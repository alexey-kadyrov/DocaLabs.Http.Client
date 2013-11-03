using System;
using System.Collections.Generic;
using System.Threading;

namespace DocaLabs.Http.Client.Utils
{
    public class CustomConcurrentDictionary<TKey, TValue> : ICustomConcurrentDictionary<TKey, TValue>
    {
        readonly Dictionary<TKey, TValue> _data = new Dictionary<TKey, TValue>();
        readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

        public TValue this[TKey key]
        {
            get
            {
                _locker.EnterReadLock();

                try
                {
                    return _data[key];
                }
                finally
                {
                    _locker.ExitReadLock();
                }
            }

            set
            {
                _locker.EnterWriteLock();

                try
                {
                    _data[key] = value;
                }
                finally
                {
                    _locker.ExitWriteLock();
                }
            }
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            _locker.EnterReadLock();

            try
            {
                TValue existingEntry;
                if (_data.TryGetValue(key, out existingEntry))
                    return existingEntry;
            }
            finally
            {
                _locker.ExitReadLock();
            }

            return TryAddInternal(key, valueFactory);
        }

        public bool Remove(TKey key)
        {
            _locker.EnterWriteLock();

            try
            {
                return _data.Remove(key);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            _locker.EnterReadLock();

            try
            {
                return _data.TryGetValue(key, out value);
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        TValue TryAddInternal(TKey key, Func<TKey, TValue> valueFactory)
        {
            _locker.EnterWriteLock();

            try
            {
                TValue existingEntry;
                if (_data.TryGetValue(key, out existingEntry))
                {
                    // another thread already inserted an item, so use that one
                    return existingEntry;
                }

                var newEntry = valueFactory(key);

                _data[key] = newEntry;

                return newEntry;
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }
    }
}
