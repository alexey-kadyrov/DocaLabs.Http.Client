using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DocaLabs.Http.Client.Utils
{
    static class PlatformAdapter
    {
        static readonly string[] PlatformNames = { "DotNet", "Phone", "Store"};
        static readonly Lazy<Assembly> PlatformSpecificAssimebly = new Lazy<Assembly>(FindForPlatformSpecificAssembly); 
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

        static Assembly FindForPlatformSpecificAssembly()
        {
            return PlatformNames
                .Select(ProbeForPlatformSpecificAssembly)
                .FirstOrDefault(assembly => assembly != null);
        }

        static Assembly ProbeForPlatformSpecificAssembly(string platformName)
        {
            var assemblyName = new AssemblyName
            {
                Name = "DocaLabs.Http.Client." + platformName
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

        static Type FindAdapter(Type interfaceType)
        {
            var typeName = MakeAdapterTypeName(interfaceType);

            var assembly = PlatformSpecificAssimebly.Value ?? 
                typeof (PlatformAdapter).GetTypeInfo().Assembly;

            return assembly.GetType(typeName + "Override") ?? assembly.GetType(typeName);
        }

        static string MakeAdapterTypeName(Type interfaceType)
        {
            // For example, if we're looking for an implementation of DocaLabs.Http.Client.Binding.IHttpResponseStreamFactory, 
            // then we'll look for DocaLabs.Http.Client.Binding.HttpResponseStreamFactory
            return interfaceType.Namespace + "." + interfaceType.Name.Substring(1);
        }
    }
}
