using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Generates an implementation of HttpClient for a given interface.
    /// </summary>
    public static class HttpClientFactory
    {
        const string AssemblySuffix = "__assembly_http_client_impl";
        const string ClientSuffix = "__http_client_impl";
        const string ModelSuffix = "__model_impl";

        static readonly ModuleBuilder ModuleBuilder;
        static readonly object Locker;
        static readonly Dictionary<Type, ClientTypeInfo> Constructors;
        static readonly Random Random; // it's used from under Locker so should be safe.

        static long _typeCount;

        static HttpClientFactory()
        {
            var assemblyName = typeof(HttpClientFactory).Namespace + AssemblySuffix;

            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName(assemblyName),
                AssemblyBuilderAccess.Run);

            ModuleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName);
            Constructors = new Dictionary<Type, ClientTypeInfo>();
            Random = new Random();
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
        /// <param name="configurationName">If the configuration name is used to get the endpoint configuration from the configuration file, if the parameter is null it will default to the interface's type full name.</param>
        public static TInterface CreateInstance<TInterface>(string configurationName)
        {
            return (TInterface)CreateInstance(typeof(TInterface), null, configurationName);
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
        /// <param name="configurationName">If the configuration name is used to get the endpoint configuration from the configuration file, if the parameter is null it will default to the interface's type full name.</param>
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
        /// <param name="configurationName">If the configuration name is used to get the endpoint configuration from the configuration file, if the parameter is null it will default to the interface's type full name.</param>
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
        /// <param name="configurationName">If the configuration name is used to get the endpoint configuration from the configuration file, if the parameter is null it will default to the interface's type full name.</param>
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
        /// <param name="configurationName">If the configuration name is used to get the endpoint configuration from the configuration file, if the parameter is null it will default to the interface's type full name.</param>
        public static object CreateInstance(Type baseType, Type interfaceType, Uri baseUrl = null, string configurationName = null)
        {
            var constructor = GetConstructor(baseType, interfaceType);

            if (string.IsNullOrWhiteSpace(configurationName))
                configurationName = interfaceType.FullName;

            return constructor.Invoke(new object[] { baseUrl, configurationName });
        }

        static ConstructorInfo GetConstructor(Type baseType, Type interfaceType)
        {
            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");

            lock (Locker)
            {
                ClientTypeInfo constructor;

                if (!Constructors.TryGetValue(interfaceType, out constructor))
                {
                    Constructors[interfaceType] = constructor = new ClientTypeInfo
                    {
                        ConstructorInfo = BuildTypeFrom(baseType, interfaceType).GetConstructor(new[] { typeof(Uri), typeof(string) }),
                        OriginalBaseType = baseType
                    };
                }
                else
                {
                    if(constructor.OriginalBaseType != baseType)
                        throw new ArgumentException(string.Format(Resources.PlatformText.base_type_does_not_match, baseType.FullName, constructor.OriginalBaseType.FullName), "baseType");
                }

                return constructor.ConstructorInfo;
            }
        }

        static Type BuildTypeFrom(Type baseType, Type interfaceType)
        {
            var interfaceInfo = new ClientInterfaceInfo(interfaceType);

            baseType = interfaceInfo.EnsureBaseType(baseType);

            var typeBuilder = ModuleBuilder.DefineType(
                string.Format("{0}{1}{2}{3}", interfaceType.FullName, Interlocked.Increment(ref _typeCount), Random.Next(), ClientSuffix),
                TypeAttributes.Class | TypeAttributes.Public, baseType, new[] { interfaceType });

            interfaceInfo.TransferCustomAttributes(typeBuilder);

            interfaceInfo.BuildConstructor(baseType, typeBuilder);

            interfaceInfo.BuildServiceCallMethod(baseType, typeBuilder);

            return typeBuilder.CreateType();
        }

        class ClientInterfaceInfo
        {
            readonly ServiceCallMethodInfo _serviceExecuteMethodInfo;
            readonly CustomeAttributes _attributes;

            public ClientInterfaceInfo(Type interfaceType)
            {
                if (!interfaceType.IsInterface)
                    throw new ArgumentException(string.Format(Resources.PlatformText.must_be_interface, interfaceType.FullName), "interfaceType");

                if(interfaceType.IsGenericTypeDefinition)
                    throw new ArgumentException(string.Format(Resources.PlatformText.interface_cannot_be_generic_type_definition, interfaceType.FullName), "interfaceType");

                var methods = interfaceType.GetMethods();
                if (methods.Length != 1)
                    throw new ArgumentException(string.Format(Resources.PlatformText.must_have_only_one_method, interfaceType.FullName), "interfaceType");

                _serviceExecuteMethodInfo = new ServiceCallMethodInfo(methods[0]);

                _attributes = new CustomeAttributes(interfaceType, AttributeTargets.Class);
            }

            public Type EnsureBaseType(Type baseType)
            {
                if (baseType == null)
                    return _serviceExecuteMethodInfo.MakeConcreteGenericType();

                if (!baseType.IsGenericTypeDefinition)
                    return baseType;

                if (baseType.GetGenericArguments().Length != 2)
                    throw new ArgumentException(string.Format(Resources.PlatformText.if_base_class_generic_it_must_have_two_parameters, baseType.FullName), "baseType");

                return _serviceExecuteMethodInfo.MakeConcreteGenericType(baseType);
            }

            public void TransferCustomAttributes(TypeBuilder typeBuilder)
            {
                _attributes.Transfer(typeBuilder);
            }

            public void BuildConstructor(Type baseType, TypeBuilder typeBuilder)
            {
                var threeParamCtor = true;

                var baseCtor = GetBaseConstructor(baseType, ref threeParamCtor);

                var ctor = typeBuilder.DefineConstructor(
                    MethodAttributes.Public, CallingConventions.Standard | CallingConventions.HasThis, new[] { typeof(Uri), typeof(string) });

                var ctorGenerator = ctor.GetILGenerator();

                ctorGenerator.Emit(OpCodes.Ldarg_0);
                ctorGenerator.Emit(OpCodes.Ldarg_1);
                ctorGenerator.Emit(OpCodes.Ldarg_2);

                if (threeParamCtor)
                    ctorGenerator.Emit(OpCodes.Ldnull);

                ctorGenerator.Emit(OpCodes.Call, baseCtor);
                ctorGenerator.Emit(OpCodes.Ret);
            }

            public void BuildServiceCallMethod(Type baseType, TypeBuilder typeBuilder)
            {
                _serviceExecuteMethodInfo.BuildServiceCallMethod(baseType, typeBuilder);
            }

            ConstructorInfo GetBaseConstructor(Type baseType, ref bool threeParamCtor)
            {
                var baseCtor = baseType.GetConstructor(new[] { typeof(Uri), typeof(string), _serviceExecuteMethodInfo.ExecuteStrategyType });

                if (baseCtor == null)
                {
                    threeParamCtor = false;

                    baseCtor = baseType.GetConstructor(new[] { typeof(Uri), typeof(string) });

                    if (baseCtor == null)
                        throw new ArgumentException(string.Format(Resources.PlatformText.must_implement_constructor, baseType.FullName, _serviceExecuteMethodInfo.ExecuteStrategyType.FullName), "baseType");
                }

                return baseCtor;
            }
        }

        class ServiceCallMethodInfo
        {
            readonly MethodInfo _methodInfo;

            ParameterInfo[] _modelParameters;

            bool _hasCancellationToken;

            Type _outputModelType;

            Type _originalOutputModelType;

            bool _isAsyncClient;

            readonly InputModelInfo _inputModelInfo;

            public Type ExecuteStrategyType { get; private set; }

            public ServiceCallMethodInfo(MethodInfo serviceCall)
            {
                _methodInfo = serviceCall;

                InitializeOutputModelType();

                InitializeMethodParameters(serviceCall);

                _inputModelInfo = new InputModelInfo(_modelParameters, _methodInfo);

                InitializeExecuteStrategyType();
            }

            public Type MakeConcreteGenericType()
            {
                return _isAsyncClient
                    ? typeof(AsyncHttpClient<,>).MakeGenericType(_inputModelInfo.ModelType, _outputModelType)
                    : typeof(HttpClient<,>).MakeGenericType(_inputModelInfo.ModelType, _outputModelType);
            }

            public Type MakeConcreteGenericType(Type baseType)
            {
                return baseType.MakeGenericType(_inputModelInfo.ModelType, _outputModelType);
            }

            public void BuildServiceCallMethod(Type baseType, TypeBuilder typeBuilder)
            {
                if(_hasCancellationToken)
                    BuildCancellableServiceCallMethod(baseType, typeBuilder);
                else
                    BuildOrdinaryServiceCallMethod(baseType, typeBuilder);
            }

            void InitializeOutputModelType()
            {
                _outputModelType = _originalOutputModelType = _methodInfo.ReturnType;

                if (_outputModelType == typeof(void))
                {
                    _outputModelType = typeof(VoidType);
                }
                else if (_outputModelType == typeof(Task))
                {
                    _outputModelType = typeof(VoidType);
                    _isAsyncClient = true;
                }
                else if (_outputModelType.IsGenericType)
                {
                    var outputGeneric = _outputModelType.GetGenericTypeDefinition();
                    if (outputGeneric != typeof (Task<>))
                        return;

                    _outputModelType = _outputModelType.GetGenericArguments()[0];
                    _isAsyncClient = true;
                }
            }

            void InitializeMethodParameters(MethodInfo serviceCall)
            {
                var parameters = serviceCall.GetParameters();

                if (!_isAsyncClient)
                {
                    _modelParameters = parameters;
                    return;
                }

                var count = parameters.Length;
                if (count == 0)
                {
                    _modelParameters = new ParameterInfo[0];
                    return;
                }

                if (parameters[count - 1].ParameterType == typeof(CancellationToken))
                {
                    --count;
                    _modelParameters = new ParameterInfo[count];
                    for (var i = 0; i < count; i++)
                        _modelParameters[i] = parameters[i];
                    _hasCancellationToken = true;
                    return;
                }

                _modelParameters = parameters;
            }

            void InitializeExecuteStrategyType()
            {
                if (_isAsyncClient)
                {
                    if (_originalOutputModelType == typeof(Task))
                        ExecuteStrategyType = typeof(IExecuteStrategy<,>).MakeGenericType(_inputModelInfo.ModelType, typeof(Task<VoidType>));
                    else
                        ExecuteStrategyType = typeof(IExecuteStrategy<,>).MakeGenericType(_inputModelInfo.ModelType, _originalOutputModelType);
                }
                else
                {
                    ExecuteStrategyType = typeof(IExecuteStrategy<,>).MakeGenericType(_inputModelInfo.ModelType, _outputModelType);
                }
            }

            void BuildOrdinaryServiceCallMethod(Type baseType, TypeBuilder typeBuilder)
            {
                var baseExecute = baseType.GetMethod(_isAsyncClient ? "ExecuteAsync" : "Execute", new[] { _inputModelInfo.ModelType });
                if (baseExecute == null)
                    throw new ArgumentException(string.Format(Resources.PlatformText.must_have_execute_method,
                                                baseType.FullName, _outputModelType.FullName, _inputModelInfo.ModelType.FullName), "baseType");

                var newExecute = DefineServiceCallMethod(typeBuilder);

                var executeGenerator = newExecute.GetILGenerator();

                _inputModelInfo.EmitLoadModel(executeGenerator);

                executeGenerator.Emit(OpCodes.Call, baseExecute);

                if (_originalOutputModelType == typeof(void))
                    executeGenerator.Emit(OpCodes.Pop);

                executeGenerator.Emit(OpCodes.Ret);

                typeBuilder.DefineMethodOverride(newExecute, _methodInfo);
            }

            void BuildCancellableServiceCallMethod(Type baseType, TypeBuilder typeBuilder)
            {
                var baseExecute = baseType.GetMethod("ExecuteAsync", new[] { _inputModelInfo.ModelType, typeof(CancellationToken) });
                if (baseExecute == null)
                    throw new ArgumentException(string.Format(Resources.PlatformText.must_have_execute_method,
                                                baseType.FullName, _outputModelType.FullName, _inputModelInfo.ModelType.FullName), "baseType");

                var newExecute = DefineServiceCallMethod(typeBuilder);

                var executeGenerator = newExecute.GetILGenerator();

                _inputModelInfo.EmitLoadModel(executeGenerator);

                // load cancellation token
                executeGenerator.Emit(OpCodes.Ldarg, _methodInfo.GetParameters().Length);

                executeGenerator.Emit(OpCodes.Call, baseExecute);

                if (_originalOutputModelType == typeof(void))
                    executeGenerator.Emit(OpCodes.Pop);

                executeGenerator.Emit(OpCodes.Ret);

                typeBuilder.DefineMethodOverride(newExecute, _methodInfo);
            }

            MethodBuilder DefineServiceCallMethod(TypeBuilder typeBuilder)
            {
                return typeBuilder.DefineMethod(
                    _methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                    CallingConventions.Standard | CallingConventions.HasThis,
                    _originalOutputModelType,
                    _methodInfo.GetParameters().Select(x => x.ParameterType).ToArray());
            }
        }

        class InputModelInfo
        {
            bool _isModelAutogenerated;
 
            public Type ModelType { get; private set; }

            public InputModelInfo(IList<ParameterInfo> parameters, MethodBase methodInfo)
            {
                if (parameters.Count > 1)
                {
                    CreateInputModel(parameters, methodInfo);
                }
                else if (parameters.Count == 1 && parameters[0].ParameterType.IsSimpleType())
                {
                    CreateInputModel(parameters, methodInfo);
                }
                else if (parameters.Count == 0)
                {
                    ModelType = typeof (VoidType);
                }
                else
                {
                    ModelType = parameters[0].ParameterType;
                }
            }

            public void EmitLoadModel(ILGenerator executeGenerator)
            {
                if (!_isModelAutogenerated)
                {
                    executeGenerator.Emit(OpCodes.Ldarg_0);

                    if (ModelType != typeof(VoidType))
                        executeGenerator.Emit(OpCodes.Ldarg_1);
                    else
                        executeGenerator.Emit(OpCodes.Ldsfld, Reflect<VoidType>.GetFieldInfo(x => VoidType.Value));
                }
                else
                {
                    EmitInitializeAndLoadModel(executeGenerator);
                }
            }

            void EmitInitializeAndLoadModel(ILGenerator executeGenerator)
            {
                var properties = ModelType.GetProperties();

                var model = executeGenerator.DeclareLocal(ModelType);

                // ReSharper disable AssignNullToNotNullAttribute
                executeGenerator.Emit(OpCodes.Newobj, ModelType.GetConstructor(new Type[0]));
                // ReSharper restore AssignNullToNotNullAttribute
                executeGenerator.Emit(OpCodes.Stloc, model);

                for (var i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];

                    executeGenerator.Emit(OpCodes.Ldloc, model);
                    executeGenerator.Emit(OpCodes.Ldarg, i + 1);
                    executeGenerator.Emit(OpCodes.Callvirt, property.GetSetMethod());
                }

                executeGenerator.Emit(OpCodes.Ldarg_0);
                executeGenerator.Emit(OpCodes.Ldloc, model);
            }

            void CreateInputModel(IEnumerable<ParameterInfo> parameters, MemberInfo methodInfo)
            {
                _isModelAutogenerated = true;

                var methodNameBase = methodInfo.DeclaringType != null
                    ? methodInfo.DeclaringType.FullName + "_" + methodInfo.Name
                    : methodInfo.Name;

                var typeBuilder = ModuleBuilder.DefineType(
                    string.Format("{0}{1}{2}{3}", methodNameBase, Interlocked.Increment(ref _typeCount), Random.Next(), ModelSuffix),
                    TypeAttributes.Class | TypeAttributes.Public);

                foreach (var parameter in parameters)
                    CreateProperty(typeBuilder, parameter);

                new CustomeAttributes(methodInfo, AttributeTargets.Property).Transfer(typeBuilder);

                ModelType = typeBuilder.CreateType();
            }

            static void CreateProperty(TypeBuilder modelBulder, ParameterInfo parameter)
            {
                var propertyName = parameter.Name;
                var propertyType = parameter.ParameterType;

                var fieldBuilder = modelBulder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

                var propertyBuilder = modelBulder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

                const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

                var getterBuilder = modelBulder.DefineMethod("get_" + propertyName, getSetAttr, propertyType, Type.EmptyTypes);
                var getterIL = getterBuilder.GetILGenerator();
                getterIL.Emit(OpCodes.Ldarg_0);
                getterIL.Emit(OpCodes.Ldfld, fieldBuilder);
                getterIL.Emit(OpCodes.Ret);

                var setterBuilder = modelBulder.DefineMethod("set_" + propertyName, getSetAttr, null, new[] { propertyType });
                var settrIL = setterBuilder.GetILGenerator();
                settrIL.Emit(OpCodes.Ldarg_0);
                settrIL.Emit(OpCodes.Ldarg_1);
                settrIL.Emit(OpCodes.Stfld, fieldBuilder);
                settrIL.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getterBuilder);
                propertyBuilder.SetSetMethod(setterBuilder);

                new CustomeAttributes(parameter, AttributeTargets.Property).Transfer(propertyBuilder);
            }
        }

        class CustomAttributeInfo
        {
            public ConstructorInfo Constructor { get; private set; }
            public object[] ConstructorArguments { get; private set; }
            public PropertyInfo[] InitializedProperties { get; private set; }
            public object[] InitializedPropertyValues { get; private set; }
            public FieldInfo[] InitializedFields { get; private set; }
            public object[] InitializedFieldsValues { get; private set; }

            public CustomAttributeInfo(CustomAttributeData data)
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

        class CustomeAttributes
        {
            readonly IEnumerable<CustomAttributeInfo> _attributes;

            public CustomeAttributes(MemberInfo owner, AttributeTargets targets)
            {
                _attributes = owner.CustomAttributes
                    .Where(x => x.IsValidOn(targets))
                    .Select(x => new CustomAttributeInfo(x));
            }

            public CustomeAttributes(ParameterInfo owner, AttributeTargets targets)
            {
                _attributes = owner.CustomAttributes
                    .Where(x => x.IsValidOn(targets))
                    .Select(x => new CustomAttributeInfo(x));
            }

            public void Transfer(PropertyBuilder propertyBuilder)
            {
                foreach (var builder in _attributes.Select(x => new CustomAttributeBuilder(
                                                                            x.Constructor,
                                                                            x.ConstructorArguments,
                                                                            x.InitializedProperties,
                                                                            x.InitializedPropertyValues,
                                                                            x.InitializedFields,
                                                                            x.InitializedFieldsValues)))
                {
                    propertyBuilder.SetCustomAttribute(builder);
                }
            }

            public void Transfer(TypeBuilder typeBuilder)
            {
                foreach (var builder in _attributes.Select(x => new CustomAttributeBuilder(
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
        }

        class ClientTypeInfo
        {
            public ConstructorInfo ConstructorInfo { get; set; }
            public Type OriginalBaseType { get; set; }
        }
    }
}
