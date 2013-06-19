using System;
using System.Collections.Concurrent;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Provides global access to the HTTP client model binders for the application.
    /// </summary>
    static public class ModelBinders
    {
        static readonly ConcurrentDictionary<Type, IModelBinder> Binders;
        static readonly ConcurrentDictionary<Type, IResponseReader> Readers;
        static volatile IModelBinder _defaultModelBinder;
        static volatile IResponseReader _defaultResponseReader;

        public static IModelBinder DefaultModelBinder
        {
            get { return _defaultModelBinder; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                _defaultModelBinder = value;
            }
        }

        static ModelBinders()
        {
            Binders = new ConcurrentDictionary<Type, IModelBinder>();
            _defaultModelBinder = new DefaultModelBinder();
            _defaultResponseReader = new DefaultResponseReader();
        }

        /// <summary>
        /// Adds the specified item to the model reader dictionary.
        /// </summary>
        public static void Add(Type type, IModelBinder binder)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            if(binder == null)
                throw new ArgumentNullException("binder");

            Binders[type] = binder;
        }

        /// <summary>
        /// Adds the specified item to the model reader dictionary.
        /// </summary>
        public static void Add(Type type, IResponseReader reader)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (reader == null)
                throw new ArgumentNullException("reader");

            Readers[type] = reader;
        }

        public static IModelBinder GetBinder(Type type)
        {
            IModelBinder binder;
            return Binders.TryGetValue(type, out binder) 
                ? binder 
                : _defaultModelBinder;
        }

        public static IResponseReader GetReader(Type type)
        {
            IResponseReader responseReader;
            return Readers.TryGetValue(type, out responseReader)
                ? responseReader
                : _defaultResponseReader;
        }
    }
}
