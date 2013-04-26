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
        static volatile IModelBinder _defaultModelBinder;

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
        }

        /// <summary>
        /// Adds the specified item to the model binder dictionary.
        /// </summary>
        public static void Add(Type type, IModelBinder binder)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            if(binder == null)
                throw new ArgumentNullException("binder");

            Binders[type] = binder;
        }

        public static IModelBinder GetBinder(Type type)
        {
            IModelBinder binder;
            return Binders.TryGetValue(type, out binder) 
                ? binder 
                : _defaultModelBinder;
        }
    }
}
