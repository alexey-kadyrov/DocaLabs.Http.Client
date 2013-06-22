using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Generates an implementation of HttpClient for a given interface.
    /// </summary>
    public static class HttpClientFactory
    {
        const string Suffix = "__http_client_impl";
        static readonly ModuleBuilder ModuleBuilder;
        static readonly object Locker;
        static readonly Dictionary<Type, CreatedTypeInfo> Constructors;

        static HttpClientFactory()
        {
            var assemblyName = typeof(HttpClientFactory).Namespace + Suffix;

            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName(assemblyName),
                AssemblyBuilderAccess.Run);

            ModuleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName);
            Constructors = new Dictionary<Type, CreatedTypeInfo>();
            Locker = new object();
        }

        /// <summary>
        /// Creates an instance of a concrete class implementing the TInterface, the class is expected to be derived from HttpClient.
        /// </summary>
        /// <typeparam name="TInterface">
        /// Interface which should be implemented, the interface must implement only one method.
        /// The method's parameter type is not void it will be used as the TQuery generic parameter and the return type if it's not void as the TResult.
        /// The method can have any name. The method's implementation will call to TResult Execute(TQuery query) method of the HttpClient.
        /// </typeparam>
        /// <param name="baseUrl">The base URL of the service.</param>
        /// <param name="configurationName">If the configuration name is used to get the endpoint configuration from the config file, if the parameter is null it will default to the interface's type full name.</param>
        public static TInterface CreateInstance<TInterface>(Uri baseUrl = null, string configurationName = null)
        {
            return (TInterface)CreateInstance(typeof(TInterface), baseUrl, configurationName);
        }

        /// <summary>
        /// Creates an instance of a concrete class implementing the interfaceType, the class is derived from HttpClient.
        /// </summary>
        /// <param name="interfaceType">
        /// Interface which should be implemented, the interface must implement only one method.
        /// The method's parameter type is not void it will be used as the TQuery generic parameter and the return type if it's not void as the TResult.
        /// The method can have any name. The method's implementation will call to TResult Execute(TQuery query) method of the HttpClient.
        /// </param>
        /// <param name="baseUrl">The base URL of the service.</param>
        /// <param name="configurationName">If the configuration name is used to get the endpoint configuration from the config file, if the parameter is null it will default to the interface's type full name.</param>
        public static object CreateInstance(Type interfaceType, Uri baseUrl = null, string configurationName = null)
        {
            return CreateInstance(null, interfaceType, baseUrl, configurationName);
        }

        /// <summary>
        /// Creates an instance of a concrete class implementing the interfaceType, the class is derived from baseType.
        /// </summary>
        /// <typeparam name="TInterface">
        /// Interface which should be implemented, the interface must implement only one method.
        /// The method's parameter type is not void it will be used as the TQuery generic parameter and the return type if it's not void as the TResult.
        /// The method can have any name. The method's implementation will call to TResult Execute(TQuery query) method of the HttpClient.
        /// </typeparam>
        /// <param name="baseType">
        /// The base class which will be used to generate the concrete type implementing the interface.
        /// The class must have non default constructor:
        ///     (Uri baseUrl, string configurationName, Func&lt;Func&lt;TResult>, TResult> retryStrategy) 
        ///     or
        ///     (Uri baseUrl, string configurationName)
        /// </param>
        /// <param name="baseUrl">The base URL of the service.</param>
        /// <param name="configurationName">If the configuration name is used to get the endpoint configuration from the config file, if the parameter is null it will default to the interface's type full name.</param>
        public static TInterface CreateInstance<TInterface>(Type baseType, Uri baseUrl = null, string configurationName = null)
        {
            return (TInterface)CreateInstance(baseType, typeof(TInterface), baseUrl, configurationName);
        }

        /// <summary>
        /// Creates an instance of a concrete class implementing the interfaceType, the class is derived from baseType.
        /// </summary>
        /// <param name="baseType">
        /// The base class which will be used to generate the concrete type implementing the interface.
        /// The class must have non default constructor:
        ///     (Uri baseUrl, string configurationName, Func&lt;Func&lt;TResult>, TResult> retryStrategy) 
        ///     or
        ///     (Uri baseUrl, string configurationName)
        /// </param>
        /// <param name="interfaceType">
        /// Interface which should be implemented, the interface must implement only one method.
        /// The method's parameter type is not void it will be used as the TQuery generic parameter and the return type if it's not void as the TResult.
        /// The method can have any name. The method's implementation will call to TResult Execute(TQuery query) method of the HttpClient.
        /// </param>
        /// <param name="baseUrl">The base URL of the service.</param>
        /// <param name="configurationName">If the configuration name is used to get the endpoint configuration from the config file, if the parameter is null it will default to the interface's type full name.</param>
        public static object CreateInstance(Type baseType, Type interfaceType, Uri baseUrl = null, string configurationName = null)
        {
            var constructor = GetMappedConstructor(baseType, interfaceType);

            if (string.IsNullOrWhiteSpace(configurationName))
                configurationName = interfaceType.FullName;

            return constructor.Invoke(new object[] { baseUrl, configurationName });
        }

        static ConstructorInfo GetMappedConstructor(Type baseType, Type interfaceType)
        {
            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");

            lock (Locker)
            {
                CreatedTypeInfo constructor;

                if (!Constructors.TryGetValue(interfaceType, out constructor))
                {
                    Constructors[interfaceType] = constructor = new CreatedTypeInfo
                    {
                        ConstructorInfo = InitType(baseType, interfaceType),
                        OriginalBaseType = baseType
                    };
                }
                else
                {
                    if(constructor.OriginalBaseType != baseType)
                        throw new ArgumentException(string.Format(Resources.Text.base_type_does_not_match, baseType.FullName, constructor.OriginalBaseType.FullName), "baseType");
                }

                return constructor.ConstructorInfo;
            }
        }

        static ConstructorInfo InitType(Type baseType, Type interfaceType)
        {
            return BuildTypeFrom(baseType, interfaceType).GetConstructor(new[] { typeof(Uri), typeof(string) });
        }

        static Type BuildTypeFrom(Type baseType, Type interfaceType)
        {
            var interfaceInfo = new ClientInterfaceInfo(interfaceType);

            baseType = MakeBaseType(baseType, interfaceInfo);

            var typeBuilder = DefineType(baseType, interfaceType);

            TransferAttributes(typeBuilder, interfaceInfo);

            BuildConstructor(baseType, interfaceInfo, typeBuilder);

            BuildServiceCallMethod(baseType, interfaceInfo, typeBuilder);

            return typeBuilder.CreateType();
        }

        static void TransferAttributes(TypeBuilder typeBuilder, ClientInterfaceInfo interfaceInfo)
        {
            foreach (var builder in interfaceInfo.Attributes.Select(x => new CustomAttributeBuilder(
                                                                        x.Constructor, 
                                                                        x.ConstructorArguments, 
                                                                        x.InitializedProperties, 
                                                                        x.InitializedPropertyValues,
                                                                        x.InitializedFields,
                                                                        x.InitializedFieldsValues)))
            {
                typeBuilder.SetCustomAttribute(builder);
            }
        }

        static Type MakeBaseType(Type baseType, ClientInterfaceInfo interfaceInfo)
        {
            if (baseType == null)
                return typeof (HttpClient<,>).MakeGenericType(interfaceInfo.QueryType, interfaceInfo.ResultType);

            if(!baseType.IsGenericTypeDefinition)
                return baseType;

            if(baseType.GetGenericArguments().Length != 2)
                throw new ArgumentException(string.Format(Resources.Text.if_base_class_generic_it_must_have_two_parameters, baseType.FullName), "baseType");

            return baseType.MakeGenericType(interfaceInfo.QueryType, interfaceInfo.ResultType);
        }

        static TypeBuilder DefineType(Type baseType, Type interfaceType)
        {
            return ModuleBuilder.DefineType(
                string.Format("{0}{1}{2}", interfaceType.FullName, Constructors.Count, Suffix),
                TypeAttributes.Class | TypeAttributes.Public,
                baseType,
                new[] { interfaceType });
        }

        static void BuildConstructor(Type baseType, ClientInterfaceInfo interfaceInfo, TypeBuilder typeBuilder)
        {
            var threeParamCtor = true;

            var baseCtor = GetBaseConstructor(baseType, interfaceInfo, ref threeParamCtor);

            var ctor = DefineConstructor(typeBuilder);

            var ctorGenerator = ctor.GetILGenerator();

            ctorGenerator.Emit(OpCodes.Ldarg_0);
            ctorGenerator.Emit(OpCodes.Ldarg_1);
            ctorGenerator.Emit(OpCodes.Ldarg_2);

            if (threeParamCtor)
                ctorGenerator.Emit(OpCodes.Ldnull);

            ctorGenerator.Emit(OpCodes.Call, baseCtor);
            ctorGenerator.Emit(OpCodes.Ret);
        }

        static ConstructorInfo GetBaseConstructor(Type baseType, ClientInterfaceInfo interfaceInfo, ref bool threeParamCtor)
        {
            var baseCtor = baseType.GetConstructor(
                new[] {typeof (Uri), typeof (string), interfaceInfo.RetryStragtegyType});

            if (baseCtor == null)
            {
                threeParamCtor = false;

                baseCtor = baseType.GetConstructor(new[] {typeof (Uri), typeof (string)});

                if (baseCtor == null)
                    throw new ArgumentException(string.Format(Resources.Text.must_implement_constructor,
                                                baseType.FullName, interfaceInfo.RetryStragtegyType.FullName), "baseType");
            }

            return baseCtor;
        }

        static ConstructorBuilder DefineConstructor(TypeBuilder typeBuilder)
        {
            var ctor = typeBuilder.DefineConstructor(
                MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis,
                new[] {typeof (Uri), typeof (string)});
            return ctor;
        }

        static void BuildServiceCallMethod(Type baseType, ClientInterfaceInfo interfaceInfo, TypeBuilder typeBuilder)
        {
            var baseExecute = baseType.GetMethod("Execute", new[] { interfaceInfo.QueryType });
            if (baseExecute == null)
                throw new ArgumentException(string.Format(Resources.Text.must_have_execute_method, 
                                            baseType.FullName, interfaceInfo.ResultType.FullName, interfaceInfo.QueryType.FullName), "baseType");

            var newExecute = DefineServiceCallMethod(interfaceInfo, typeBuilder);

            var executeGenerator = newExecute.GetILGenerator();

            executeGenerator.Emit(OpCodes.Ldarg_0);

            executeGenerator.Emit(interfaceInfo.QueryType != typeof(VoidType)
                ? OpCodes.Ldarg_1
                : OpCodes.Ldnull);

            executeGenerator.Emit(OpCodes.Call, baseExecute);

            if(interfaceInfo.OriginalResultType == typeof(void))
                executeGenerator.Emit(OpCodes.Pop);

            executeGenerator.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(newExecute, interfaceInfo.ServiceExecuteMethod);
        }

        static MethodBuilder DefineServiceCallMethod(ClientInterfaceInfo interfaceInfo, TypeBuilder typeBuilder)
        {
            return typeBuilder.DefineMethod(
                interfaceInfo.ServiceExecuteMethod.Name,
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.Standard | CallingConventions.HasThis,
                interfaceInfo.ResultType != typeof (VoidType)
                    ? interfaceInfo.ResultType
                    : typeof (void),
                interfaceInfo.QueryType != typeof(VoidType)
                    ? new[] { interfaceInfo.QueryType }
                    : null
            );
        }

        class ClientInterfaceInfo
        {
            public MethodInfo ServiceExecuteMethod { get; private set; }
            public Type QueryType { get; private set; }
            public Type ResultType { get; private set; }
            public Type RetryStragtegyType { get; private set; }
            public Type OriginalResultType { get; private set; }
            public IEnumerable<AttributeInfo> Attributes { get; private set; }

            public ClientInterfaceInfo(Type interfaceType)
            {
                if (!interfaceType.IsInterface)
                    throw new ArgumentException(string.Format(Resources.Text.must_be_interface, interfaceType.FullName), "interfaceType");

                if(interfaceType.IsGenericTypeDefinition)
                    throw new ArgumentException(string.Format(Resources.Text.interface_cannot_be_generic_type_defienition, interfaceType.FullName), "interfaceType");

                var methods = interfaceType.GetMethods();
                if (methods.Length != 1)
                    throw new ArgumentException(string.Format(Resources.Text.must_have_only_one_method, interfaceType.FullName), "interfaceType");

                ServiceExecuteMethod = methods[0];
                ResultType = OriginalResultType = ServiceExecuteMethod.ReturnType;

                if (ResultType == typeof (void))
                    ResultType = typeof (VoidType);

                var parameters = ServiceExecuteMethod.GetParameters();
                if (parameters.Length > 1)
                    throw new ArgumentException(string.Format(Resources.Text.method_must_have_no_more_than_one_argument, ServiceExecuteMethod.Name, interfaceType.FullName), "interfaceType");

                QueryType = parameters.Length == 0 
                    ? typeof(VoidType)
                    : parameters[0].ParameterType;

                // typeof(Func<Func<Result>, Result>)
                RetryStragtegyType = typeof(Func<,>).MakeGenericType(typeof(Func<>).MakeGenericType(ResultType), ResultType);

                Attributes = interfaceType.CustomAttributes
                    .Where(x => x.IsValidOn(AttributeTargets.Class))
                    .Select(x => new AttributeInfo(x));
            }
        }

        class AttributeInfo
        {
            public ConstructorInfo Constructor { get; private set; }
            public object[] ConstructorArguments { get; private set; }
            public PropertyInfo[] InitializedProperties { get; private set; }
            public object[] InitializedPropertyValues { get; private set; }
            public FieldInfo[] InitializedFields { get; private set; }
            public object[] InitializedFieldsValues { get; private set; }

            public AttributeInfo(CustomAttributeData data)
            {
                Constructor = data.Constructor;
                ConstructorArguments = data.ConstructorArguments.Select(x => x.Value).ToArray();

                ParseProperties(data);
            }

            void ParseProperties(CustomAttributeData data)
            {
                if (data.NamedArguments == null)
                    return;

                var properties = new List<PropertyInfo>();
                var propertyValues = new List<object>();

                var fields = new List<FieldInfo>();
                var fieldValues = new List<object>();

                foreach (var namedArg in data.NamedArguments)
                {
                    if (namedArg.IsField)
                    {
                        fields.Add((FieldInfo)namedArg.MemberInfo);
                        fieldValues.Add(namedArg.TypedValue.Value);
                    }
                    else
                    {
                        properties.Add((PropertyInfo) namedArg.MemberInfo);
                        propertyValues.Add(namedArg.TypedValue.Value);
                    }
                }

                InitializedProperties = properties.ToArray();
                InitializedPropertyValues = propertyValues.ToArray();

                InitializedFields = fields.ToArray();
                InitializedFieldsValues = fieldValues.ToArray();
            }
        }
    
        class CreatedTypeInfo
        {
            public ConstructorInfo ConstructorInfo { get; set; }
            public Type OriginalBaseType { get; set; }
        }
    }
}
