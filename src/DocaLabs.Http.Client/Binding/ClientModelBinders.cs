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
        static readonly ConcurrentDictionary<Type, IUrlPathMapper> UrlPathMappers;
        static readonly ConcurrentDictionary<Type, IUrlQueryMapper> UrlQueryMappers;
        static readonly ConcurrentDictionary<Type, IHeaderMapper> HeaderMappers;
        static readonly ConcurrentDictionary<Type, IRequestStreamWriter> RequestStreamWriters;

        static volatile IMutator _defaultMutator;
        static volatile IUrlPathMapper _defaultUrlPathMapper;
        static volatile IUrlQueryMapper _defaultUrlQueryMapper;
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

        public static IUrlPathMapper DefaultUrlPathMapper
        {
            get { return _defaultUrlPathMapper; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _defaultUrlPathMapper = value;
            }
        }

        public static IUrlQueryMapper DefaultUrlQueryMapper
        {
            get { return _defaultUrlQueryMapper; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                _defaultUrlQueryMapper = value;
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
            UrlPathMappers = new ConcurrentDictionary<Type, IUrlPathMapper>();
            UrlQueryMappers = new ConcurrentDictionary<Type, IUrlQueryMapper>();
            HeaderMappers = new ConcurrentDictionary<Type, IHeaderMapper>();
            RequestStreamWriters = new ConcurrentDictionary<Type, IRequestStreamWriter>();

            _defaultMutator = new DefaultMutator();
            _defaultUrlQueryMapper = new DefaultUrlQueryMapper();
            _defaultHeaderMapper = new DefaultHeaderMapper();
            _defaultRequestStreamWriter = new DefaultRequestStreamWriter();
        }

        /// <summary>
        /// Adds the specified item to the model binder dictionary.
        /// </summary>
        /// <param name="type">The type which binder should be used for.</param>
        /// <param name="binder">The binder must implement at least one of IMutator, IUrlQueryMapper, IHeaderMapper, or IRequestStreamWriter.</param>
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

            if (binder is IUrlPathMapper)
            {
                UrlPathMappers[type] = binder as IUrlPathMapper;
                processed = true;
            }

            if (binder is IUrlQueryMapper)
            {
                UrlQueryMappers[type] = binder as IUrlQueryMapper;
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

        public static IUrlPathMapper GetUrlPathMapper(Type type)
        {
            IUrlPathMapper queryMapper;
            return UrlPathMappers.TryGetValue(type, out queryMapper) ? queryMapper : _defaultUrlPathMapper;
        }

        public static IUrlQueryMapper GetUrlQueryMapper(Type type)
        {
            IUrlQueryMapper queryMapper;
            return UrlQueryMappers.TryGetValue(type, out queryMapper) ? queryMapper : _defaultUrlQueryMapper;
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
