using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Tests._Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests
{
    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_service_without_specifying_base_class
    {
        static IServiceWithQueryAndResult instance;

        Because of =
            () => instance = HttpClientFactory.CreateInstance<IServiceWithQueryAndResult>(new Uri("http://foo.bar/"));

        // it's impossible to verify the call as it will try to execute the "real" pipeline
        It should_be_able_to_create_the_instance =
            () => instance.ShouldNotBeNull();
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_generic_interface
    {
        static IGenericService<TestsQuery, TestResultValue> instance;

        Because of =
            () => instance = HttpClientFactory.CreateInstance<IGenericService<TestsQuery, TestResultValue>>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service =
            () => ((TestsQuery)(instance.GetResult(new TestsQuery { Value = "Hello!" }).Value)).Value.ShouldEqual("Hello!");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_service_with_query_and_result_for_generic_type_difinition_as_base_class
    {
        static IServiceWithQueryAndResult2 instance;

        Because of =
            () => instance = HttpClientFactory.CreateInstance<IServiceWithQueryAndResult2>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service =
            () => ((TestsQuery)(instance.GetResult(new TestsQuery { Value = "Hello!" }).Value)).Value.ShouldEqual("Hello!");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_service_for_fully_defined_generic_type
    {
        static IServiceWithQueryAndResult3 instance;

        Because of =
            () => instance = HttpClientFactory.CreateInstance<IServiceWithQueryAndResult3>(typeof(TestHttpClientBaseType<TestsQuery, TestResultValue>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service =
            () => ((TestsQuery)(instance.GetResult(new TestsQuery { Value = "Hello!" }).Value)).Value.ShouldEqual("Hello!");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_service_for_non_generic_base_type
    {
        static IServiceWithQueryAndResult5 instance;

        Because of =
            () => instance = HttpClientFactory.CreateInstance<IServiceWithQueryAndResult5>(typeof(NonGenericTestHttpClientBaseType), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service =
            () => ((TestsQuery)(instance.GetResult(new TestsQuery { Value = "Hello!" }).Value)).Value.ShouldEqual("Hello!");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_several_times_for_the_same_interface
    {
        static IServiceWithQueryAndResult4 instance;

        Establish context =
            () => HttpClientFactory.CreateInstance<IServiceWithQueryAndResult4>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        Because of =
            () => instance = HttpClientFactory.CreateInstance<IServiceWithQueryAndResult4>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_still_be_able_to_create_instane_and_call_the_service =
            () => ((TestsQuery)(instance.GetResult(new TestsQuery { Value = "Hello!" }).Value)).Value.ShouldEqual("Hello!");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_service_decorated_with_attributes
    {
        static IDecoratedService instance;

        Because of =
            () => instance = HttpClientFactory.CreateInstance<IDecoratedService>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service =
            () => ((TestsQuery)(instance.GetResult(new TestsQuery { Value = "Hello!" }).Value)).Value.ShouldEqual("Hello!");

        It should_not_transfer_attribute_that_is_not_defined_for_class =
            () => instance.GetType().GetCustomAttribute<InterfaceOnlyAttribute>().ShouldBeNull();

        It should_transfer_attribute_that_is_defined_for_class_and_has_properties_fields_and_constructor =
            () => instance.GetType().GetCustomAttribute<ClassAttributeWithFieldsPropertiesAndConstructorArgsAttribute>()
                          .ShouldMatch(x => x.FromConstructorArg == "one" && x.Field == "two" && x.Property == "three");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_service_with_result_only
    {
        static IServiceWithResultOnly instance;

        Because of =
            () => instance = HttpClientFactory.CreateInstance<IServiceWithResultOnly>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service =
            () => instance.Get().ShouldBeOfType<TestResultValue>();
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_service_with_query_only
    {
        static IServiceWithQueryOnly instance;

        Because of =
            () => instance = HttpClientFactory.CreateInstance<IServiceWithQueryOnly>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            instance.Post(new TestsQuery());
            instance.GetType().GetProperty("ExecutionMarker").GetValue(instance, null).ShouldEqual("Pipeline was executed.");
        };
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_service_without_query_or_result
    {
        static IServiceWithoutQueryOrResult instance;

        Because of =
            () => instance = HttpClientFactory.CreateInstance<IServiceWithoutQueryOrResult>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            instance.Do();
            instance.GetType().GetProperty("ExecutionMarker").GetValue(instance, null).ShouldEqual("Pipeline was executed.");
        };
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_interface_with_method_with_more_than_one_simple_type_args
    {
        static IServiceWithMethodWithMoreThanOneArg instance;

        Because of =
            () => instance = HttpClientFactory.CreateInstance<IServiceWithMethodWithMoreThanOneArg>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            var value = instance.GetResult(42, "Hello World!");
            value.Value.GetType().GetProperty("query").GetValue(value.Value).ShouldEqual(42);
            value.Value.GetType().GetProperty("notOk").GetValue(value.Value).ShouldEqual("Hello World!");
            instance.GetType().GetProperty("ExecutionMarker").GetValue(instance, null).ShouldEqual("Pipeline was executed.");
        };
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_several_times_for_the_same_interface_but_different_base_classes
    {
        static Exception exception;

        Establish context =
            () => HttpClientFactory.CreateInstance<IServiceWithQueryAndResult6>(typeof(NonGenericTestHttpClientBaseType), new Uri("http://foo.bar/"));

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithQueryAndResult6>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_base_type_with_more_than_two_generic_args
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithQueryAndResult7>(typeof(TestHttpClientBaseTypeWithThreeGenericArgs<,,>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_base_type_with_one_generic_arg
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithQueryAndResult8>(typeof(TestHttpClientBaseTypeWithOneGenericArg<>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_base_type_with_only_default_constructor
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithQueryAndResult9>(typeof(TestHttpClientBaseTypeWithOnlyDefaultConstructor), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_base_type_with_constructor_with_wrong_arguments
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithQueryAndResult10>(typeof(TestHttpClientBaseTypeWithConstructorWithWrongArguments), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_base_type_without_execute_method
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithQueryAndResult11>(typeof(TestHttpClientBaseTypeWithoutExecuteMethod), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_base_type_with_execute_method_with_wrong_arguments
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithQueryAndResult12>(typeof(TestHttpClientBaseTypeWithExecuteMethodWithWrongArguments), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_null_interface
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance(null, new Uri("http://foo.bar/")));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_interface_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("interfaceType");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_class_instead_of_interface
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<ServiceClassInsteadOfInterface>(new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_interface_with_two_methods
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithTwoMethods>(new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_interface_without_any_method
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithoutAnyMethod>(new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instance_for_generic_type_definition_of_interface
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance(typeof(IGenericService2<,>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");
    }

    [Subject(typeof(HttpClientFactory))]
    class when_creating_instances_concurrently
    {
        static List<Type> interfaces;
        static int count;

        Establish context = () =>
        {
            interfaces = new List<Type>();
            for (var i = 0; i < 1000; i++)
                interfaces.Add(TestHttpClientInterfaceBuilder.CreateInterface(i, typeof(TestsQuery), typeof(TestResultValue)));
        };

        Because of = () => Parallel.ForEach(interfaces, x =>
        {
            var s = HttpClientFactory.CreateInstance(typeof(TestHttpClientBaseTypeForConcurrentRun<,>), x, new Uri("http://www.foo.bar/"));

            var result = s.GetType().GetMethod("CallService").Invoke(s, new object[] { new TestsQuery { Value = "Hello!" } });

            ((TestResultValue)result).Value.ShouldEqual("Hello!");

            Interlocked.Increment(ref count);
        });

        It should_create_and_call_all_services =
            () => count.ShouldEqual(interfaces.Count);
    }
}
