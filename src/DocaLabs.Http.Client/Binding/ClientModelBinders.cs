using System;
using System.Collections.Concurrent;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Provides global access to the HTTP client model binders for the application.
    /// </summary>
    static public class ClientModelBinders
    {
        static readonly ConcurrentDictionary<Type, IMutator> Mutators;
        static readonly ConcurrentDictionary<Type, IUrlMapper> UrlMappers;
        static readonly ConcurrentDictionary<Type, IHeaderMapper> HeaderMappers;
        static readonly ConcurrentDictionary<Type, IRequestStreamWriter> RequestStreamWriters;

        static volatile IMutator _defaultMutator;
        static volatile IUrlMapper _defaultUrlMapper;
        static volatile IHeaderMapper _defaultHeaderMapper;
        static volatile IRequestStreamWriter _defaultRequestStreamWriter;

        public static IMutator DefaultMutator
        {
            get { return _defaultMutator; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                _defaultMutator = value;
            }
        }

        public static IUrlMapper DefaultUrlMapper
        {
            get { return _defaultUrlMapper; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                _defaultUrlMapper = value;
            }
        }

        public static IHeaderMapper DefaultHeaderMapper
        {
            get { return _defaultHeaderMapper; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                _defaultHeaderMapper = value;
            }
        }

        public static IRequestStreamWriter DefaultRequestStreamWriter
        {
            get { return _defaultRequestStreamWriter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _defaultRequestStreamWriter = value;
            }
        }

        static ClientModelBinders()
        {
            Mutators = new ConcurrentDictionary<Type, IMutator>();
            UrlMappers = new ConcurrentDictionary<Type, IUrlMapper>();
            HeaderMappers = new ConcurrentDictionary<Type, IHeaderMapper>();
            RequestStreamWriters = new ConcurrentDictionary<Type, IRequestStreamWriter>();

            _defaultMutator = new DefaultMutator();
            _defaultUrlMapper = new DefaultUrlMapper();
            _defaultHeaderMapper = new DefaultHeaderMapper();
            _defaultRequestStreamWriter = new DefaultRequestStreamWriter();
        }

        /// <summary>
        /// Adds the specified item to the model binder dictionary.
        /// </summary>
        /// <param name="type">The type which binder should be used for.</param>
        /// <param name="binder">The binder must implement at least one of IMutator, IUrlMapper, IHeaderMapper, or IRequestStreamWriter.</param>
        public static void Add(Type type, object binder)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            if(binder == null)
                throw new ArgumentNullException("binder");

            var processed = false;

            if (binder is IMutator)
            {
                Mutators[type] = binder as IMutator;
                processed = true;
            }

            if (binder is IUrlMapper)
            {
                UrlMappers[type] = binder as IUrlMapper;
                processed = true;
            }

            if (binder is IHeaderMapper)
            {
                HeaderMappers[type] = binder as IHeaderMapper;
                processed = true;
            }

            if (binder is IRequestStreamWriter)
            {
                RequestStreamWriters[type] = binder as IRequestStreamWriter;
                processed = true;
            }

            if(!processed)
                throw new ArgumentException(string.Format(Resources.Text.binder_must_implement, binder.GetType().FullName), "binder");
        }

        public static IMutator GetMutator(Type type)
        {
            IMutator mutator;
            return Mutators.TryGetValue(type, out mutator) ? mutator : _defaultMutator;
        }

        public static IUrlMapper GetUrlMapper(Type type)
        {
            IUrlMapper mapper;
            return UrlMappers.TryGetValue(type, out mapper) ? mapper : _defaultUrlMapper;
        }

        public static IHeaderMapper GetHeaderMapper(Type type)
        {
            IHeaderMapper mapper;
            return HeaderMappers.TryGetValue(type, out mapper) ? mapper : _defaultHeaderMapper;
        }

        public static IRequestStreamWriter GetRequestStreamWriter(Type type)
        {
            IRequestStreamWriter writer;
            return RequestStreamWriters.TryGetValue(type, out writer) ? writer : _defaultRequestStreamWriter;
        }
    }
}
