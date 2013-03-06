using System;
using System.Collections.Concurrent;
using DocaLabs.Http.Client.Binding.UrlMapping;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Provides global access to the HTTP client model binders for the application.
    /// </summary>
    static public class ClientModelBinders
    {
        static readonly ConcurrentDictionary<Type, IMutator> Mutators;
        static readonly ConcurrentDictionary<Type, IUrlPathComposer> UrlPathComposers;
        static readonly ConcurrentDictionary<Type, IUrlQueryComposer> UrlQueryComposers;
        static readonly ConcurrentDictionary<Type, IHeaderMapper> HeaderMappers;
        static readonly ConcurrentDictionary<Type, IRequestStreamWriter> RequestStreamWriters;

        static volatile IMutator _defaultMutator;
        static volatile IUrlPathComposer _defaultUrlPathComposer;
        static volatile IUrlQueryComposer _defaultUrlQueryComposer;
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

        public static IUrlPathComposer DefaultUrlPathComposer
        {
            get { return _defaultUrlPathComposer; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _defaultUrlPathComposer = value;
            }
        }

        public static IUrlQueryComposer DefaultUrlQueryComposer
        {
            get { return _defaultUrlQueryComposer; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                _defaultUrlQueryComposer = value;
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
            UrlPathComposers = new ConcurrentDictionary<Type, IUrlPathComposer>();
            UrlQueryComposers = new ConcurrentDictionary<Type, IUrlQueryComposer>();
            HeaderMappers = new ConcurrentDictionary<Type, IHeaderMapper>();
            RequestStreamWriters = new ConcurrentDictionary<Type, IRequestStreamWriter>();

            _defaultMutator = new DefaultMutator();
            _defaultUrlPathComposer = new DefaultUrlPathComposer();
            _defaultUrlQueryComposer = new DefaultUrlQueryComposer();
            _defaultHeaderMapper = new DefaultHeaderMapper();
            _defaultRequestStreamWriter = new DefaultRequestStreamWriter();
        }

        /// <summary>
        /// Adds the specified item to the model binder dictionary.
        /// </summary>
        /// <param name="type">The type which binder should be used for.</param>
        /// <param name="binder">The binder must implement at least one of IMutator, IUrlQueryComposer, IHeaderMapper, or IRequestStreamWriter.</param>
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

            if (binder is IUrlPathComposer)
            {
                UrlPathComposers[type] = binder as IUrlPathComposer;
                processed = true;
            }

            if (binder is IUrlQueryComposer)
            {
                UrlQueryComposers[type] = binder as IUrlQueryComposer;
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

        public static IUrlPathComposer GetUrlPathComposer(Type type)
        {
            IUrlPathComposer pathComposer;
            return UrlPathComposers.TryGetValue(type, out pathComposer) ? pathComposer : _defaultUrlPathComposer;
        }

        public static IUrlQueryComposer GetUrlQueryComposer(Type type)
        {
            IUrlQueryComposer queryComposer;
            return UrlQueryComposers.TryGetValue(type, out queryComposer) ? queryComposer : _defaultUrlQueryComposer;
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
