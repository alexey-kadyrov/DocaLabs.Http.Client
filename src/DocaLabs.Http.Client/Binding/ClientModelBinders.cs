using System;
using System.Collections.Concurrent;
using DocaLabs.Http.Client.Binding.RequestSerialization;
using DocaLabs.Http.Client.Binding.UrlComposing;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Provides global access to the HTTP client model binders for the application.
    /// </summary>
    static public class ClientModelBinders
    {
        static readonly ConcurrentDictionary<Type, IMutator> Mutators;
        static readonly ConcurrentDictionary<Type, IUrlComposer> UrlComposers;
        static readonly ConcurrentDictionary<Type, IHeaderMapper> HeaderMappers;
        static readonly ConcurrentDictionary<Type, ICredentialsMapper> CredentialsMappers;
        static readonly ConcurrentDictionary<Type, IRequestWriter> RequestWriters;

        static volatile IMutator _defaultMutator;
        static volatile IUrlComposer _defaultUrlComposer;
        static volatile IHeaderMapper _defaultHeaderMapper;
        static volatile ICredentialsMapper _defaultCredentialsMapper;
        static volatile IRequestWriter _defaultRequestWriter;

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

        public static IUrlComposer DefaultUrlComposer
        {
            get { return _defaultUrlComposer; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _defaultUrlComposer = value;
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

        public static ICredentialsMapper DefaultCredentialsMapper
        {
            get { return _defaultCredentialsMapper; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _defaultCredentialsMapper = value;
            }
        }

        public static IRequestWriter DefaultRequestWriter
        {
            get { return _defaultRequestWriter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _defaultRequestWriter = value;
            }
        }

        static ClientModelBinders()
        {
            Mutators = new ConcurrentDictionary<Type, IMutator>();
            UrlComposers = new ConcurrentDictionary<Type, IUrlComposer>();
            HeaderMappers = new ConcurrentDictionary<Type, IHeaderMapper>();
            CredentialsMappers = new ConcurrentDictionary<Type, ICredentialsMapper>();
            RequestWriters = new ConcurrentDictionary<Type, IRequestWriter>();

            _defaultMutator = new DefaultMutator();
            _defaultUrlComposer = new DefaultUrlComposer();
            _defaultHeaderMapper = new DefaultHeaderMapper();
            _defaultCredentialsMapper = new DefaultCredentialsMapper();
            _defaultRequestWriter = new DefaultRequestWriter();
        }

        /// <summary>
        /// Adds the specified item to the model binder dictionary.
        /// </summary>
        /// <param name="type">The type which binder should be used for.</param>
        /// <param name="binder">The binder must implement at least one of IMutator, IUrlQueryComposer, IHeaderMapper, or IRequestWriter.</param>
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

            if (binder is IUrlComposer)
            {
                UrlComposers[type] = binder as IUrlComposer;
                processed = true;
            }

            if (binder is IHeaderMapper)
            {
                HeaderMappers[type] = binder as IHeaderMapper;
                processed = true;
            }

            if (binder is ICredentialsMapper)
            {
                CredentialsMappers[type] = binder as ICredentialsMapper;
                processed = true;
            }

            if (binder is IRequestWriter)
            {
                RequestWriters[type] = binder as IRequestWriter;
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

        public static IUrlComposer GetUrlComposer(Type type)
        {
            IUrlComposer composer;
            return UrlComposers.TryGetValue(type, out composer) ? composer : _defaultUrlComposer;
        }

        public static IHeaderMapper GetHeaderMapper(Type type)
        {
            IHeaderMapper mapper;
            return HeaderMappers.TryGetValue(type, out mapper) ? mapper : _defaultHeaderMapper;
        }

        public static ICredentialsMapper GetCredentialsMapper(Type type)
        {
            ICredentialsMapper mapper;
            return CredentialsMappers.TryGetValue(type, out mapper) ? mapper : _defaultCredentialsMapper;
        }

        public static IRequestWriter GetRequestWriter(Type type)
        {
            IRequestWriter writer;
            return RequestWriters.TryGetValue(type, out writer) ? writer : _defaultRequestWriter;
        }
    }
}
