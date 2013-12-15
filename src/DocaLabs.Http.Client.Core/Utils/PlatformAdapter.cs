using System;
using System.Collections.Generic;
using System.Reflection;

namespace DocaLabs.Http.Client.Utils
{
    static class PlatformAdapter
    {
        static readonly object Locker = new object();
        static readonly Dictionary<Type, object> Adapters = new Dictionary<Type, object>();

        public static T Resolve<T>(bool isRequired = true) where T : class
        {
            var interfaceType = typeof (T);

            lock (Locker)
            {
                object instance;

                if (!Adapters.TryGetValue(interfaceType, out instance))
                {
                    instance = ResolveAdapter(interfaceType);
                    Adapters.Add(interfaceType, instance);
                }

                if(instance == null && isRequired)
                    throw new PlatformNotSupportedException(string.Format("The type {0} is not supported on this platform.", interfaceType));

                return (T)instance;
            }
        }

        static object ResolveAdapter(Type interfaceType)
        {
            var adapter = FindAdapter(interfaceType);

            return adapter != null
                ? Activator.CreateInstance(adapter)
                : null;
        }

        static Type FindAdapter(Type interfaceType)
        {
            var typeName = MakeAdapterTypeName(interfaceType);

            var thisAssembly = typeof (PlatformAdapter).GetTypeInfo().Assembly;

            return thisAssembly.GetType(typeName + "Override") 
                ?? thisAssembly.GetType(typeName);
        }

        static string MakeAdapterTypeName(Type interfaceType)
        {
            // For example, if we're looking for an implementation of DocaLabs.Http.Client.Binding.IHttpResponseStreamFactory, 
            // then we'll look for DocaLabs.Http.Client.Binding.HttpResponseStreamFactory
            return interfaceType.Namespace + "." + interfaceType.Name.Substring(1);
        }
    }
}
