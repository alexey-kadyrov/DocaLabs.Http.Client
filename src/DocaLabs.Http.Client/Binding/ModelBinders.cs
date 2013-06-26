using System;
using System.Collections.Concurrent;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Provides global access to the HTTP client model binders for the application.
    /// </summary>
    static public class ModelBinders
    {
        static readonly ConcurrentDictionary<Type, IRequestBinder> RequestBinders;
        static readonly ConcurrentDictionary<Type, IResponseBinder> ResponseBinders;
        static volatile IRequestBinder _defaultRequestBinder;
        static volatile IResponseBinder _defaultResponseBinder;

        /// <summary>
        /// Gets or sets the default request binder which is used if there is no input model specific binder set.
        /// By default it's DefaultRequestBinder.
        /// </summary>
        public static IRequestBinder DefaultRequestBinder
        {
            get { return _defaultRequestBinder; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                _defaultRequestBinder = value;
            }
        }

        /// <summary>
        /// Gets or sets the default request binder which is used if there is no output model specific binder set.
        /// By default it's DefaultResponseBinder.
        /// </summary>
        public static IResponseBinder DefaultResponseBinder
        {
            get { return _defaultResponseBinder; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _defaultResponseBinder = value;
            }
        }

        static ModelBinders()
        {
            RequestBinders = new ConcurrentDictionary<Type, IRequestBinder>();
            ResponseBinders = new ConcurrentDictionary<Type, IResponseBinder>();
            _defaultRequestBinder = new DefaultRequestBinder();
            _defaultResponseBinder = new DefaultResponseBinder();
        }

        /// <summary>
        /// Adds the specified item to the input model binder dictionary.
        /// </summary>
        public static void Add(Type type, IRequestBinder binder)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            if(binder == null)
                throw new ArgumentNullException("binder");

            RequestBinders[type] = binder;
        }

        /// <summary>
        /// Adds the specified item to the output model binder dictionary.
        /// </summary>
        public static void Add(Type type, IResponseBinder binder)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (binder == null)
                throw new ArgumentNullException("binder");

            ResponseBinders[type] = binder;
        }

        /// <summary>
        /// Gets a custom request binder associated with the input model. 
        /// If there is no binders registered for the model then DefaultRequestBinder is returned.
        /// </summary>
        public static IRequestBinder GetRequestBinder(Type modelType)
        {
            IRequestBinder binder;
            return RequestBinders.TryGetValue(modelType, out binder) 
                ? binder 
                : DefaultRequestBinder;
        }

        /// <summary>
        /// Gets a custom request binder associated with the output model. 
        /// If there is no binders registered for the model then DefaultResponseBinder is returned.
        /// </summary>
        public static IResponseBinder GetResponseBinder(Type modelType)
        {
            IResponseBinder responseBinder;
            return ResponseBinders.TryGetValue(modelType, out responseBinder)
                ? responseBinder
                : DefaultResponseBinder;
        }
    }
}
