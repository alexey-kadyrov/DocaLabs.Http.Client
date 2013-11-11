using System;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Provides global access to the HTTP client model binders for the application.
    /// </summary>
    static public class ModelBinders
    {
        static readonly CustomConcurrentDictionary<Type, IRequestBinder> RequestBinders;
        static readonly CustomConcurrentDictionary<Type, IResponseBinder> ResponseBinders;
        static readonly CustomConcurrentDictionary<Type, IAsyncResponseBinder> AsyncResponseBinders;
        static volatile IRequestBinder _defaultRequestBinder;
        static volatile IResponseBinder _defaultResponseBinder;
        static volatile IAsyncResponseBinder _asyncDefaultResponseBinder;

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
        /// Gets or sets the asynchronous default request binder which is used if there is no output model specific binder set.
        /// By default it's DefaultResponseBinder.
        /// </summary>
        public static IAsyncResponseBinder AsyncDefaultResponseBinder
        {
            get { return _asyncDefaultResponseBinder; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _asyncDefaultResponseBinder = value;
            }
        }

        /// <summary>
        /// Gets or sets the synchronous default request binder which is used if there is no output model specific binder set.
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
            RequestBinders = new CustomConcurrentDictionary<Type, IRequestBinder>();
            ResponseBinders = new CustomConcurrentDictionary<Type, IResponseBinder>();
            AsyncResponseBinders = new CustomConcurrentDictionary<Type, IAsyncResponseBinder>();
            _defaultRequestBinder = new DefaultRequestBinder();
            _defaultResponseBinder = new DefaultResponseBinder();
            _asyncDefaultResponseBinder = (IAsyncResponseBinder)_defaultResponseBinder;
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
        /// Adds the specified item to the synchronous output model binder dictionary.
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
        /// Adds the specified item to the asynchronous output model binder dictionary.
        /// </summary>
        public static void Add(Type type, IAsyncResponseBinder binder)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (binder == null)
                throw new ArgumentNullException("binder");

            AsyncResponseBinders[type] = binder;
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

        /// <summary>
        /// Gets a custom request binder associated with the output model. 
        /// If there is no binders registered for the model then DefaultResponseBinder is returned.
        /// </summary>
        public static IAsyncResponseBinder GetAsyncResponseBinder(Type modelType)
        {
            IAsyncResponseBinder responseBinder;
            return AsyncResponseBinders.TryGetValue(modelType, out responseBinder)
                ? responseBinder
                : AsyncDefaultResponseBinder;
        }
    }
}
