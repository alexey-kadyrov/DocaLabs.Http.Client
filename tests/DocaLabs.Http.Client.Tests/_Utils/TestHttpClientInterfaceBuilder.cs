using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DocaLabs.Http.Client.Tests._Utils
{
    static class TestHttpClientInterfaceBuilder
    {
        static readonly ModuleBuilder ModuleBuilder;

        static TestHttpClientInterfaceBuilder()
        {
            var assemblyName = typeof(TestHttpClientInterfaceBuilder).Namespace + "__test_interfaces";

            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName(assemblyName),
                AssemblyBuilderAccess.Run);

            ModuleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName);
        }

        public static Type CreateInterface(int suffix, Type queryType, Type resultType)
        {
            var typeBuilder = ModuleBuilder.DefineType(
                "ITestService" + suffix, TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.Public);

            typeBuilder.DefineMethod("CallService", 
                                     MethodAttributes.Public | MethodAttributes.Abstract | MethodAttributes.Virtual, resultType, new[] { queryType });

            return typeBuilder.CreateType();
        }
    }
}