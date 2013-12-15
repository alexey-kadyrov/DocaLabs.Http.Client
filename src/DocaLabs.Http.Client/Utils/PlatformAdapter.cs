using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DocaLabs.Http.Client.Utils
{
    static class PlatformAdapter
    {
        static readonly object Locker = new object();
        static readonly Dictionary<Type, object> Adapters = new Dictionary<Type, object>();

        public static T Resolve<T>(bool isRequired = true, string preferredAssembly = null) where T : class
        {
            var interfaceType = typeof(T);

            lock (Locker)
            {
                object instance;

                if (!Adapters.TryGetValue(interfaceType, out instance))
                {
                    instance = ResolveAdapter(interfaceType, preferredAssembly);
                    Adapters.Add(interfaceType, instance);
                }

                if (instance == null && isRequired)
                    throw new PlatformNotSupportedException(string.Format("The type {0} is not supported on this platform.", interfaceType));

                return (T)instance;
            }
        }

        static object ResolveAdapter(Type interfaceType, string preferredAssembly)
        {
            var adapter = FindAdapter(interfaceType, preferredAssembly);

            return adapter != null
                ? Activator.CreateInstance(adapter)
                : null;
        }

        static Type FindAdapter(Type interfaceType, string preferredAssembly)
        {
            var typeName = MakeAdapterTypeName(interfaceType);

            var thisAssembly = typeof(PlatformAdapter).GetTypeInfo().Assembly;

            var assembly = FindSpecificAssembly(preferredAssembly) ?? thisAssembly;

            return assembly.GetType(typeName + "Override")
                ?? assembly.GetType(typeName)
                ?? thisAssembly.GetType(typeName + "Override")
                ?? thisAssembly.GetType(typeName);
        }

        static string MakeAdapterTypeName(Type interfaceType)
        {
            // For example, if we're looking for an implementation of DocaLabs.Http.Client.Binding.IHttpResponseStreamFactory, 
            // then we'll look for DocaLabs.Http.Client.Binding.HttpResponseStreamFactory
            return interfaceType.Namespace + "." + interfaceType.Name.Substring(1);
        }

        static Assembly FindSpecificAssembly(string preferredAssembly)
        {
            return string.IsNullOrWhiteSpace(preferredAssembly) 
                ? ProbeForSpecificAssembly(preferredAssembly) 
                : null;
        }

        static Assembly ProbeForSpecificAssembly(string preferredAssembly)
        {
            var assemblyName = new AssemblyName
            {
                Name = preferredAssembly
            };

            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (FileNotFoundException)
            {
            }
            catch (Exception) // Probably FileIOException due to not SN assembly
            {
                // Try to load a non-SN version of the assembly
                assemblyName.SetPublicKey(null);
                assemblyName.SetPublicKeyToken(null);

                try
                {
                    return Assembly.Load(assemblyName);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }
    }
}
