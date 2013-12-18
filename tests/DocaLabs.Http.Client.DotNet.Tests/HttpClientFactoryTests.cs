using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Tests._Utils;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests
{
    [TestClass]
    public class when_creating_instance_for_service_without_specifying_base_class
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(new Uri("http://foo.bar/"));
        }

        // it's impossible to verify the call as it will try to execute the "real" pipeline
        [TestMethod]
        public void it_should_be_able_to_create_the_instance()
        {
            _instance.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_use_http_client_as_base_class()
        {
            _instance.ShouldBeOfType<HttpClient<RequestModel, ResultModel>>();
        }

        public interface IService
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_service_without_specifying_base_class
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(new Uri("http://foo.bar/"));
        }

        // it's impossible to verify the call as it will try to execute the "real" pipeline
        [TestMethod]
        public void it_should_be_able_to_create_the_instance()
        {
            _instance.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_use_async_http_client_as_base_class()
        {
            _instance.ShouldBeOfType<AsyncHttpClient<RequestModel, ResultModel>>();
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_with_cancellation_token_for_asynchronous_service_without_specifying_base_class
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(new Uri("http://foo.bar/"));
        }

        // it's impossible to verify the call as it will try to execute the "real" pipeline
        [TestMethod]
        public void it_should_be_able_to_create_the_instance()
        {
            _instance.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_use_async_http_client_as_base_class()
        {
            _instance.ShouldBeOfType<AsyncHttpClient<RequestModel, ResultModel>>();
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_generic_interface
    {
        static IGenericService<RequestModel, ResultModel> _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IGenericService<RequestModel, ResultModel>>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }).Value)).Value.ShouldEqual("Hello!");
        }
        
        public interface IGenericService<in TQuery, out TResult>
        {
            TResult GetResult(TQuery query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_for_generic_interface
    {
        static IGenericService<RequestModel, ResultModel> _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IGenericService<RequestModel, ResultModel>>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }).Result.Value)).Value.ShouldEqual("Hello!");
        }

        public interface IGenericService<in TQuery, TResult>
        {
            Task<TResult> GetResult(TQuery query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_for_generic_interface
    {
        static IGenericService<RequestModel, ResultModel> _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IGenericService<RequestModel, ResultModel>>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }, CancellationToken.None).Result.Value)).Value.ShouldEqual("Hello!");
        }

        public interface IGenericService<in TQuery, TResult>
        {
            Task<TResult> GetResult(TQuery query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_service_with_query_and_result_for_generic_type_difinition_as_base_class
    {
        static IServcie _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecuaeOf();
        }

        static void BecuaeOf()
        {
            _instance = HttpClientFactory.CreateInstance<IServcie>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }).Value)).Value.ShouldEqual("Hello!");
        }

        public interface IServcie
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_service_with_query_and_result_for_generic_type_difinition_as_base_class
    {
        static IServcie _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IServcie>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }).Result.Value)).Value.ShouldEqual("Hello!");
        }

        public interface IServcie
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_service_with_query_and_result_and_cancellation_token_for_generic_type_difinition_as_base_class
    {
        static IServcie _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IServcie>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }, CancellationToken.None).Result.Value)).Value.ShouldEqual("Hello!");
        }

        public interface IServcie
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_service_for_fully_defined_generic_type
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseType<RequestModel, ResultModel>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }).Value)).Value.ShouldEqual("Hello!");
        }

        public interface IService
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_service_for_fully_defined_generic_type
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<RequestModel, ResultModel>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }).Result.Value)).Value.ShouldEqual("Hello!");
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_service_with_cancellation_token_for_fully_defined_generic_type
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecuaseOf();
        }

        static void BecuaseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<RequestModel, ResultModel>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }, CancellationToken.None).Result.Value)).Value.ShouldEqual("Hello!");
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_service_for_non_generic_base_type
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(typeof(NonGenericTestHttpClientBaseType), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }).Value)).Value.ShouldEqual("Hello!");
        }

        public class NonGenericTestHttpClientBaseType : TestHttpClientBaseType<RequestModel, ResultModel>
        {
            public NonGenericTestHttpClientBaseType(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName)
            {
            }
        }

        public interface IService
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_service_for_non_generic_base_type
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(typeof(NonGenericTestAsyncHttpClientBaseType), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }).Result.Value)).Value.ShouldEqual("Hello!");
        }

        public class NonGenericTestAsyncHttpClientBaseType : TestAsyncHttpClientBaseType<RequestModel, ResultModel>
        {
            public NonGenericTestAsyncHttpClientBaseType(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName)
            {
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_service_with_cancellation_token_for_non_generic_base_type
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(typeof(NonGenericTestAsyncHttpClientBaseType), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }, CancellationToken.None).Result.Value)).Value.ShouldEqual("Hello!");
        }

        public class NonGenericTestAsyncHttpClientBaseType : TestAsyncHttpClientBaseType<RequestModel, ResultModel>
        {
            public NonGenericTestAsyncHttpClientBaseType(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName)
            {
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_several_times_for_the_same_interface
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_still_be_able_to_create_instane_and_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }).Value)).Value.ShouldEqual("Hello!");
        }

        public interface IService
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_several_times_for_the_same_asynchronous_interface
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_still_be_able_to_create_instane_and_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }).Result.Value)).Value.ShouldEqual("Hello!");
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_with_cancellation_token_several_times_for_the_same_asynchronous_interface
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_still_be_able_to_create_instane_and_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }, CancellationToken.None).Result.Value)).Value.ShouldEqual("Hello!");
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_service_decorated_with_attributes
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }).Value)).Value.ShouldEqual("Hello!");
        }

        [TestMethod]
        public void it_should_not_transfer_attribute_that_is_not_targeted_for_class_definition()
        {
            _instance.GetType().GetCustomAttribute<InterfaceOnlyAttribute>().ShouldBeNull();
        }

        [TestMethod]
        public void it_should_transfer_attribute_that_is_targeted_for_class_definition_and_has_properties_fields_and_constructor()
        {
            _instance.GetType().GetCustomAttribute<ClassAttributeWithFieldsPropertiesAndConstructorArgsAttribute>().ShouldMatch(x => x.FromConstructorArg == "one" && x.Field == "two" && x.Property == "three");
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
        public class ClassAttributeWithFieldsPropertiesAndConstructorArgsAttribute : Attribute
        {
            public string FromConstructorArg { get; private set; }
            public string Property { get; set; }
            public string Field;

            public ClassAttributeWithFieldsPropertiesAndConstructorArgsAttribute(string value)
            {
                FromConstructorArg = value;
            }
        }

        [AttributeUsage(AttributeTargets.Interface)]
        public class InterfaceOnlyAttribute : Attribute
        {
        }

        [InterfaceOnly, ClassAttributeWithFieldsPropertiesAndConstructorArgs("one", Field = "two", Property = "three")]
        public interface IService
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_service_decorated_with_attributes
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }).Result.Value)).Value.ShouldEqual("Hello!");
        }

        [TestMethod]
        public void it_should_not_transfer_attribute_that_is_not_targeted_for_class_definition()
        {
            _instance.GetType().GetCustomAttribute<InterfaceOnlyAttribute>().ShouldBeNull();
        }

        [TestMethod]
        public void it_should_transfer_attribute_that_is_targeted_for_class_definition_and_has_properties_fields_and_constructor()
        {
            _instance.GetType().GetCustomAttribute<ClassAttributeWithFieldsPropertiesAndConstructorArgsAttribute>().ShouldMatch(x => x.FromConstructorArg == "one" && x.Field == "two" && x.Property == "three");
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
        public class ClassAttributeWithFieldsPropertiesAndConstructorArgsAttribute : Attribute
        {
            public string FromConstructorArg { get; private set; }
            public string Property { get; set; }
            public string Field;

            public ClassAttributeWithFieldsPropertiesAndConstructorArgsAttribute(string value)
            {
                FromConstructorArg = value;
            }
        }

        [AttributeUsage(AttributeTargets.Interface)]
        public class InterfaceOnlyAttribute : Attribute
        {
        }

        [InterfaceOnly, ClassAttributeWithFieldsPropertiesAndConstructorArgs("one", Field = "two", Property = "three")]
        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_with_cancellation_token_for_asynchronous_service_decorated_with_attributes
    {
        static IService _instance;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecvauseOf();
        }

        static void BecvauseOf()
        {
            _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));
        }

        [TestMethod]
        public void it_should_be_able_to_call_the_service()
        {
            ((RequestModel)(_instance.GetResult(new RequestModel { Value = "Hello!" }, CancellationToken.None).Result.Value)).Value.ShouldEqual("Hello!");
        }

        [TestMethod]
        public void it_should_not_transfer_attribute_that_is_not_targeted_for_class_definition()
        {
            _instance.GetType().GetCustomAttribute<InterfaceOnlyAttribute>().ShouldBeNull();
        }

        [TestMethod]
        public void it_should_transfer_attribute_that_is_targeted_for_class_definition_and_has_properties_fields_and_constructor()
        {
            _instance.GetType().GetCustomAttribute<ClassAttributeWithFieldsPropertiesAndConstructorArgsAttribute>().ShouldMatch(x => x.FromConstructorArg == "one" && x.Field == "two" && x.Property == "three");
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
        public class ClassAttributeWithFieldsPropertiesAndConstructorArgsAttribute : Attribute
        {
            public string FromConstructorArg { get; private set; }
            public string Property { get; set; }
            public string Field;

            public ClassAttributeWithFieldsPropertiesAndConstructorArgsAttribute(string value)
            {
                FromConstructorArg = value;
            }
        }

        [AttributeUsage(AttributeTargets.Interface)]
        public class InterfaceOnlyAttribute : Attribute
        {
        }

        [InterfaceOnly, ClassAttributeWithFieldsPropertiesAndConstructorArgs("one", Field = "two", Property = "three")]
        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_service_with_result_only
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service =
            () => _instance.Get().ShouldBeOfType<ResultModel>();
        
        public interface IService
        {
            ResultModel Get();
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_service_with_result_only
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service =
            () => _instance.Get().Result.ShouldBeOfType<ResultModel>();

        public interface IService
        {
            Task<ResultModel> Get();
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_with_cancellation_token_for_asynchronous_service_with_result_only
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service =
            () => _instance.Get(CancellationToken.None).Result.ShouldBeOfType<ResultModel>();

        public interface IService
        {
            Task<ResultModel> Get(CancellationToken cancellationToken);
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_service_with_query_only
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            _instance.Post(new RequestModel());
            _instance.GetType().GetProperty("ExecutionMarker").GetValue(_instance, null).ShouldEqual("Pipeline was executed.");
        };

        public interface IService
        {
            void Post(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_service_with_query_only
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            _instance.Post(new RequestModel()).Wait();
            _instance.GetType().GetProperty("ExecutionMarker").GetValue(_instance, null).ShouldEqual("Async pipeline was executed.");
        };

        public interface IService
        {
            Task Post(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_with_cancellation_token_for_asynchronous_service_with_query_only
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            _instance.Post(new RequestModel(), CancellationToken.None).Wait();
            _instance.GetType().GetProperty("ExecutionMarker").GetValue(_instance, null).ShouldEqual("Async pipeline was executed.");
        };

        public interface IService
        {
            Task Post(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_service_without_query_or_result
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            _instance.Do();
            _instance.GetType().GetProperty("ExecutionMarker").GetValue(_instance, null).ShouldEqual("Pipeline was executed.");
        };

        public interface IService
        {
            void Do();
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_service_without_query_or_result
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            _instance.Do().Wait();
            _instance.GetType().GetProperty("ExecutionMarker").GetValue(_instance, null).ShouldEqual("Async pipeline was executed.");
        };

        public interface IService
        {
            Task Do();
        }
    }

    [TestClass]
    public class when_creating_instance_with_cancellation_token_for_asynchronous_service_without_query_or_result
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            _instance.Do(CancellationToken.None).Wait();
            _instance.GetType().GetProperty("ExecutionMarker").GetValue(_instance, null).ShouldEqual("Async pipeline was executed.");
        };

        public interface IService
        {
            Task Do(CancellationToken cancellationToken);
        }
    }

    [TestClass]
    public class when_creating_instance_for_interface_with_method_with_more_than_one_simple_type_args
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            var value = _instance.GetResult(42, "Hello World!");
            value.Value.GetType().GetProperty("query").GetValue(value.Value).ShouldEqual(42);
            value.Value.GetType().GetProperty("notOk").GetValue(value.Value).ShouldEqual("Hello World!");
            _instance.GetType().GetProperty("ExecutionMarker").GetValue(_instance, null).ShouldEqual("Pipeline was executed.");
        };

        public interface IService
        {
            ResultModel GetResult(int query, string notOk);
        }
        
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_interface_with_method_with_more_than_one_simple_type_args
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            var value = _instance.GetResult(42, "Hello World!").Result;
            value.Value.GetType().GetProperty("query").GetValue(value.Value).ShouldEqual(42);
            value.Value.GetType().GetProperty("notOk").GetValue(value.Value).ShouldEqual("Hello World!");
        };

        public interface IService
        {
            Task<ResultModel> GetResult(int query, string notOk);
        }
        
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_with_cancellation_token_for_asynchronous_interface_with_method_with_more_than_one_simple_type_args
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            var value = _instance.GetResult(42, "Hello World!", CancellationToken.None).Result;
            value.Value.GetType().GetProperty("query").GetValue(value.Value).ShouldEqual(42);
            value.Value.GetType().GetProperty("notOk").GetValue(value.Value).ShouldEqual("Hello World!");
        };

        public interface IService
        {
            Task<ResultModel> GetResult(int query, string notOk, CancellationToken token);
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_interface_with_method_with_one_simple_type_arg
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            var value = _instance.GetResult("Hello World!");
            value.Value.ShouldNotBeOfType(typeof(string));
            value.Value.GetType().GetProperty("key").GetValue(value.Value).ShouldEqual("Hello World!");
        };

        public interface IService
        {
            ResultModel GetResult(string key);
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_interface_with_method_with_one_simple_type_arg
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            var value = _instance.GetResult("Hello World!").Result;
            value.Value.ShouldNotBeOfType(typeof(string));
            value.Value.GetType().GetProperty("key").GetValue(value.Value).ShouldEqual("Hello World!");
        };

        public interface IService
        {
            Task<ResultModel> GetResult(string key);
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_eith_cancellation_token_for_asynchronous_interface_with_method_with_one_simple_type_arg
    {
        static IService _instance;

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            var value = _instance.GetResult("Hello World!", CancellationToken.None).Result;
            value.Value.ShouldNotBeOfType(typeof(string));
            value.Value.GetType().GetProperty("key").GetValue(value.Value).ShouldEqual("Hello World!");
        };

        public interface IService
        {
            Task<ResultModel> GetResult(string key, CancellationToken cancellationToken);
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_interface_with_mixed_more_than_one_args
    {
        static IService _instance;
        static DateTime time;
        static Guid guid;

        Establish context = () =>
        {
            time = DateTime.UtcNow;
            guid = Guid.NewGuid();
        };

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            var value = _instance.GetResult("Hello World!", time, guid, new Data { Value = "v1" }, 123.45M);

            var type = value.Value.GetType();

            //attributes
            type.GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("key").GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("time").GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("guid").GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("data").GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("number").GetCustomAttributes().ShouldBeEmpty();

            type.GetProperty("key").GetValue(value.Value).ShouldEqual("Hello World!");
            type.GetProperty("time").GetValue(value.Value).ShouldEqual(time);
            type.GetProperty("guid").GetValue(value.Value).ShouldEqual(guid);
            type.GetProperty("data").GetValue(value.Value).GetType().GetProperty("Value").GetValue(value.Value.GetType().GetProperty("data").GetValue(value.Value)).ShouldEqual("v1");
            type.GetProperty("number").GetValue(value.Value).ShouldEqual(123.45M);
        };

        public interface IService
        {
            ResultModel GetResult(string key, DateTime time, Guid guid, Data data, decimal number);
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }

        public class Data
        {
            public string Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_interface_with_mixed_more_than_one_args
    {
        static IService _instance;
        static DateTime time;
        static Guid guid;

        Establish context = () =>
        {
            time = DateTime.UtcNow;
            guid = Guid.NewGuid();
        };

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            var value = _instance.GetResult("Hello World!", time, guid, new Data { Value = "v1" }, 123.45M).Result;

            var type = value.Value.GetType();

            //attributes
            type.GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("key").GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("time").GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("guid").GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("data").GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("number").GetCustomAttributes().ShouldBeEmpty();

            type.GetProperty("key").GetValue(value.Value).ShouldEqual("Hello World!");
            type.GetProperty("time").GetValue(value.Value).ShouldEqual(time);
            type.GetProperty("guid").GetValue(value.Value).ShouldEqual(guid);
            type.GetProperty("data").GetValue(value.Value).GetType().GetProperty("Value").GetValue(value.Value.GetType().GetProperty("data").GetValue(value.Value)).ShouldEqual("v1");
            type.GetProperty("number").GetValue(value.Value).ShouldEqual(123.45M);
        };

        public interface IService
        {
            Task<ResultModel> GetResult(string key, DateTime time, Guid guid, Data data, decimal number);
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }

        public class Data
        {
            public string Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_with_cancellation_token_for_asynchronous_interface_with_mixed_more_than_one_args
    {
        static IService _instance;
        static DateTime time;
        static Guid guid;

        Establish context = () =>
        {
            time = DateTime.UtcNow;
            guid = Guid.NewGuid();
        };

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service = () =>
        {
            var value = _instance.GetResult("Hello World!", time, guid, new Data { Value = "v1" }, 123.45M, CancellationToken.None).Result;

            var type = value.Value.GetType();

            //attributes
            type.GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("key").GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("time").GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("guid").GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("data").GetCustomAttributes().ShouldBeEmpty();
            type.GetProperty("number").GetCustomAttributes().ShouldBeEmpty();

            type.GetProperty("key").GetValue(value.Value).ShouldEqual("Hello World!");
            type.GetProperty("time").GetValue(value.Value).ShouldEqual(time);
            type.GetProperty("guid").GetValue(value.Value).ShouldEqual(guid);
            type.GetProperty("data").GetValue(value.Value).GetType().GetProperty("Value").GetValue(value.Value.GetType().GetProperty("data").GetValue(value.Value)).ShouldEqual("v1");
            type.GetProperty("number").GetValue(value.Value).ShouldEqual(123.45M);
        };

        public interface IService
        {
            Task<ResultModel> GetResult(string key, DateTime time, Guid guid, Data data, decimal number, CancellationToken cancellationToken);
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }

        public class Data
        {
            public string Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_interface_with_mixed_more_than_one_args_with_attributes
    {
        static IService _instance;
        static DateTime time;
        static Guid guid;

        Establish context = () =>
        {
            time = DateTime.UtcNow;
            guid = Guid.NewGuid();
        };

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service_and_transfer_attributes = () =>
        {
            var value = _instance.GetResult("Hello World!", time, guid, new Data { Value = "v1" }, 123.45M);

            var type = value.Value.GetType();

            //attributes
            type.GetCustomAttribute<SerializeAsJsonAttribute>().RequestContentEncoding.ShouldEqual("gzip");
            type.GetProperty("key").GetCustomAttribute<PropertyOverridesAttribute>().Name.ShouldEqual("with-key");
            type.GetProperty("time").GetCustomAttribute<RequestUseAttribute>().Targets.ShouldEqual(RequestUseTargets.UrlQuery);
            type.GetProperty("guid").GetCustomAttribute<RequestUseAttribute>().Targets.ShouldEqual(RequestUseTargets.UrlPath);
            type.GetProperty("data").GetCustomAttribute<RequestUseAttribute>().Targets.ShouldEqual(RequestUseTargets.UrlQuery | RequestUseTargets.UrlPath);
            type.GetProperty("number").GetCustomAttribute<RequestUseAttribute>().Targets.ShouldEqual(RequestUseTargets.RequestHeader);

            // values
            type.GetProperty("key").GetValue(value.Value).ShouldEqual("Hello World!");
            type.GetProperty("time").GetValue(value.Value).ShouldEqual(time);
            type.GetProperty("guid").GetValue(value.Value).ShouldEqual(guid);
            type.GetProperty("data").GetValue(value.Value).GetType().GetProperty("Value").GetValue(value.Value.GetType().GetProperty("data").GetValue(value.Value)).ShouldEqual("v1");
            type.GetProperty("number").GetValue(value.Value).ShouldEqual(123.45M);
        };

        public interface IService
        {
            [SerializeAsJson(RequestContentEncoding = "gzip")]
            ResultModel GetResult(
                [PropertyOverrides(Name = "with-key")] string key, 
                [RequestUse(RequestUseTargets.UrlQuery)] DateTime time,
                [RequestUse(RequestUseTargets.UrlPath)] Guid guid,
                [RequestUse(RequestUseTargets.UrlQuery | RequestUseTargets.UrlPath)] Data data,
                [RequestUse(RequestUseTargets.RequestHeader)] decimal number);
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }

        public class Data
        {
            public string Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_asynchronous_interface_with_mixed_more_than_one_args_with_attributes
    {
        static IService _instance;
        static DateTime time;
        static Guid guid;

        Establish context = () =>
        {
            time = DateTime.UtcNow;
            guid = Guid.NewGuid();
        };

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service_and_transfer_attributes = () =>
        {
            var value = _instance.GetResult("Hello World!", time, guid, new Data { Value = "v1" }, 123.45M).Result;

            var type = value.Value.GetType();

            //attributes
            type.GetCustomAttribute<SerializeAsJsonAttribute>().RequestContentEncoding.ShouldEqual("gzip");
            type.GetProperty("key").GetCustomAttribute<PropertyOverridesAttribute>().Name.ShouldEqual("with-key");
            type.GetProperty("time").GetCustomAttribute<RequestUseAttribute>().Targets.ShouldEqual(RequestUseTargets.UrlQuery);
            type.GetProperty("guid").GetCustomAttribute<RequestUseAttribute>().Targets.ShouldEqual(RequestUseTargets.UrlPath);
            type.GetProperty("data").GetCustomAttribute<RequestUseAttribute>().Targets.ShouldEqual(RequestUseTargets.UrlQuery | RequestUseTargets.UrlPath);
            type.GetProperty("number").GetCustomAttribute<RequestUseAttribute>().Targets.ShouldEqual(RequestUseTargets.RequestHeader);

            // values
            type.GetProperty("key").GetValue(value.Value).ShouldEqual("Hello World!");
            type.GetProperty("time").GetValue(value.Value).ShouldEqual(time);
            type.GetProperty("guid").GetValue(value.Value).ShouldEqual(guid);
            type.GetProperty("data").GetValue(value.Value).GetType().GetProperty("Value").GetValue(value.Value.GetType().GetProperty("data").GetValue(value.Value)).ShouldEqual("v1");
            type.GetProperty("number").GetValue(value.Value).ShouldEqual(123.45M);
        };

        public interface IService
        {
            [SerializeAsJson(RequestContentEncoding = "gzip")]
            Task<ResultModel> GetResult(
                [PropertyOverrides(Name = "with-key")] string key,
                [RequestUse(RequestUseTargets.UrlQuery)] DateTime time,
                [RequestUse(RequestUseTargets.UrlPath)] Guid guid,
                [RequestUse(RequestUseTargets.UrlQuery | RequestUseTargets.UrlPath)] Data data,
                [RequestUse(RequestUseTargets.RequestHeader)] decimal number);
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }

        public class Data
        {
            public string Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_with_cancellation_token_for_asynchronous_interface_with_mixed_more_than_one_args_with_attributes
    {
        static IService _instance;
        static DateTime time;
        static Guid guid;

        Establish context = () =>
        {
            time = DateTime.UtcNow;
            guid = Guid.NewGuid();
        };

        Because of =
            () => _instance = HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/"));

        It should_be_able_to_call_the_service_and_transfer_attributes = () =>
        {
            var value = _instance.GetResult("Hello World!", time, guid, new Data { Value = "v1" }, 123.45M, CancellationToken.None).Result;

            var type = value.Value.GetType();

            //attributes
            type.GetCustomAttribute<SerializeAsJsonAttribute>().RequestContentEncoding.ShouldEqual("gzip");
            type.GetProperty("key").GetCustomAttribute<PropertyOverridesAttribute>().Name.ShouldEqual("with-key");
            type.GetProperty("time").GetCustomAttribute<RequestUseAttribute>().Targets.ShouldEqual(RequestUseTargets.UrlQuery);
            type.GetProperty("guid").GetCustomAttribute<RequestUseAttribute>().Targets.ShouldEqual(RequestUseTargets.UrlPath);
            type.GetProperty("data").GetCustomAttribute<RequestUseAttribute>().Targets.ShouldEqual(RequestUseTargets.UrlQuery | RequestUseTargets.UrlPath);
            type.GetProperty("number").GetCustomAttribute<RequestUseAttribute>().Targets.ShouldEqual(RequestUseTargets.RequestHeader);

            // values
            type.GetProperty("key").GetValue(value.Value).ShouldEqual("Hello World!");
            type.GetProperty("time").GetValue(value.Value).ShouldEqual(time);
            type.GetProperty("guid").GetValue(value.Value).ShouldEqual(guid);
            type.GetProperty("data").GetValue(value.Value).GetType().GetProperty("Value").GetValue(value.Value.GetType().GetProperty("data").GetValue(value.Value)).ShouldEqual("v1");
            type.GetProperty("number").GetValue(value.Value).ShouldEqual(123.45M);
        };

        public interface IService
        {
            [SerializeAsJson(RequestContentEncoding = "gzip")]
            Task<ResultModel> GetResult(
                [PropertyOverrides(Name = "with-key")] string key,
                [RequestUse(RequestUseTargets.UrlQuery)] DateTime time,
                [RequestUse(RequestUseTargets.UrlPath)] Guid guid,
                [RequestUse(RequestUseTargets.UrlQuery | RequestUseTargets.UrlPath)] Data data,
                [RequestUse(RequestUseTargets.RequestHeader)] decimal number,
                CancellationToken cancellationToken);
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }

        public class Data
        {
            public string Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_several_times_for_the_same_interface_but_different_base_classes
    {
        static Exception exception;

        Establish context =
            () => HttpClientFactory.CreateInstance<IService>(typeof(NonGenericTestHttpClientBaseType), new Uri("http://foo.bar/"));

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseType<,>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class NonGenericTestHttpClientBaseType : TestHttpClientBaseType<RequestModel, ResultModel>
        {
            public NonGenericTestHttpClientBaseType(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName)
            {
            }
        }

        public interface IService
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_several_times_for_the_same_interface_but_different_base_classes
    {
        static Exception exception;

        Establish context =
            () => HttpClientFactory.CreateInstance<IService>(typeof(NonGenericTestAsyncHttpClientBaseType), new Uri("http://foo.bar/"));

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class NonGenericTestAsyncHttpClientBaseType : TestAsyncHttpClientBaseType<RequestModel, ResultModel>
        {
            public NonGenericTestAsyncHttpClientBaseType(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName)
            {
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_several_times_for_the_same_interface_but_different_base_classes
    {
        static Exception exception;

        Establish context =
            () => HttpClientFactory.CreateInstance<IService>(typeof(NonGenericTestAsyncHttpClientBaseType), new Uri("http://foo.bar/"));

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseType<,>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class NonGenericTestAsyncHttpClientBaseType : TestAsyncHttpClientBaseType<RequestModel, ResultModel>
        {
            public NonGenericTestAsyncHttpClientBaseType(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName)
            {
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_base_type_with_more_than_two_generic_args
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseTypeWithThreeGenericArgs<,,>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestHttpClientBaseTypeWithThreeGenericArgs<TQuery, TResult, TSomeOther> : HttpClient<TQuery, TResult>
        {
            public TestHttpClientBaseTypeWithThreeGenericArgs(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName)
            {
            }

            public TSomeOther SomeMethod()
            {
                return default(TSomeOther);
            }
        }

        public interface IService
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }
        
        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_for_base_type_with_more_than_two_generic_args
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithThreeGenericArgs<,,>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithThreeGenericArgs<TQuery, TResult, TSomeOther> : AsyncHttpClient<TQuery, TResult>
        {
            public TestAsyncHttpClientBaseTypeWithThreeGenericArgs(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName)
            {
            }

            public TSomeOther SomeMethod()
            {
                return default(TSomeOther);
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_for_base_type_with_more_than_two_generic_args
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithThreeGenericArgs<,,>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithThreeGenericArgs<TQuery, TResult, TSomeOther> : AsyncHttpClient<TQuery, TResult>
        {
            public TestAsyncHttpClientBaseTypeWithThreeGenericArgs(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName)
            {
            }

            public TSomeOther SomeMethod()
            {
                return default(TSomeOther);
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_base_type_with_one_generic_arg
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseTypeWithOneGenericArg<>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestHttpClientBaseTypeWithOneGenericArg<TQuery> : HttpClient<TQuery, ResultModel>
        {
            public TestHttpClientBaseTypeWithOneGenericArg(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName)
            {
            }
        }

        public interface IService
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_for_base_type_with_one_generic_arg
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithOneGenericArg<>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithOneGenericArg<TQuery> : AsyncHttpClient<TQuery, ResultModel>
        {
            public TestAsyncHttpClientBaseTypeWithOneGenericArg(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName)
            {
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_for_base_type_with_one_generic_arg
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithOneGenericArg<>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithOneGenericArg<TQuery> : AsyncHttpClient<TQuery, ResultModel>
        {
            public TestAsyncHttpClientBaseTypeWithOneGenericArg(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName)
            {
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_base_type_with_only_default_constructor
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseTypeWithOnlyDefaultConstructor), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestHttpClientBaseTypeWithOnlyDefaultConstructor : HttpClient<RequestModel, ResultModel>
        {
            public TestHttpClientBaseTypeWithOnlyDefaultConstructor()
                : base(new Uri("http://foo.bar/"), "my")
            {
            }
        }

        public interface IService
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_for_base_type_with_only_default_constructor
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithOnlyDefaultConstructor), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithOnlyDefaultConstructor : AsyncHttpClient<RequestModel, ResultModel>
        {
            public TestAsyncHttpClientBaseTypeWithOnlyDefaultConstructor()
                : base(new Uri("http://foo.bar/"), "my")
            {
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_for_base_type_with_only_default_constructor
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithOnlyDefaultConstructor), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithOnlyDefaultConstructor : AsyncHttpClient<RequestModel, ResultModel>
        {
            public TestAsyncHttpClientBaseTypeWithOnlyDefaultConstructor()
                : base(new Uri("http://foo.bar/"), "my")
            {
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_base_type_with_constructor_with_wrong_arguments
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseTypeWithConstructorWithWrongArguments), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestHttpClientBaseTypeWithConstructorWithWrongArguments : HttpClient<RequestModel, ResultModel>
        {
            public TestHttpClientBaseTypeWithConstructorWithWrongArguments(string configurationName)
                : base(new Uri("http://foo.bar/"), configurationName)
            {
            }
        }

        public interface IService
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_for_base_type_with_constructor_with_wrong_arguments
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithConstructorWithWrongArguments), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithConstructorWithWrongArguments : AsyncHttpClient<RequestModel, ResultModel>
        {
            public TestAsyncHttpClientBaseTypeWithConstructorWithWrongArguments(string configurationName)
                : base(new Uri("http://foo.bar/"), configurationName)
            {
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_for_base_type_with_constructor_with_wrong_arguments
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithConstructorWithWrongArguments), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithConstructorWithWrongArguments : AsyncHttpClient<RequestModel, ResultModel>
        {
            public TestAsyncHttpClientBaseTypeWithConstructorWithWrongArguments(string configurationName)
                : base(new Uri("http://foo.bar/"), configurationName)
            {
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_base_type_without_execute_method
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseTypeWithoutExecuteMethod), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestHttpClientBaseTypeWithoutExecuteMethod
        {
            public Uri BaseUrl { get; set; }
            public string ConfigurationName { get; set; }

            public TestHttpClientBaseTypeWithoutExecuteMethod(Uri baseUrl, string configurationName)
            {
                BaseUrl = baseUrl;
                ConfigurationName = configurationName;
            }
        }

        public interface IService
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_for_base_type_without_execute_method
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithoutExecuteMethod), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithoutExecuteMethod
        {
            public Uri BaseUrl { get; set; }
            public string ConfigurationName { get; set; }

            public TestAsyncHttpClientBaseTypeWithoutExecuteMethod(Uri baseUrl, string configurationName)
            {
                BaseUrl = baseUrl;
                ConfigurationName = configurationName;
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_for_base_type_without_execute_method
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithoutExecuteMethod), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithoutExecuteMethod
        {
            public Uri BaseUrl { get; set; }
            public string ConfigurationName { get; set; }

            public TestAsyncHttpClientBaseTypeWithoutExecuteMethod(Uri baseUrl, string configurationName)
            {
                BaseUrl = baseUrl;
                ConfigurationName = configurationName;
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_base_type_with_execute_method_with_wrong_arguments
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestHttpClientBaseTypeWithExecuteMethodWithWrongArguments), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestHttpClientBaseTypeWithExecuteMethodWithWrongArguments
        {
            public Uri BaseUrl { get; set; }
            public string ConfigurationName { get; set; }

            public TestHttpClientBaseTypeWithExecuteMethodWithWrongArguments(Uri baseUrl, string configurationName)
            {
                BaseUrl = baseUrl;
                ConfigurationName = configurationName;
            }

            public ResultModel Execute(string query)
            {
                return null;
            }
        }

        public interface IService
        {
            ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_for_base_type_with_execute_method_with_wrong_arguments
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithExecuteMethodWithWrongArguments), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithExecuteMethodWithWrongArguments
        {
            public Uri BaseUrl { get; set; }
            public string ConfigurationName { get; set; }

            public TestAsyncHttpClientBaseTypeWithExecuteMethodWithWrongArguments(Uri baseUrl, string configurationName)
            {
                BaseUrl = baseUrl;
                ConfigurationName = configurationName;
            }

            public Task<ResultModel> Execute(string query)
            {
                return Task.FromResult(new ResultModel());
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_for_base_type_with_execute_method_with_wrong_arguments
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithExecuteMethodWithWrongArguments), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithExecuteMethodWithWrongArguments
        {
            public Uri BaseUrl { get; set; }
            public string ConfigurationName { get; set; }

            public TestAsyncHttpClientBaseTypeWithExecuteMethodWithWrongArguments(Uri baseUrl, string configurationName)
            {
                BaseUrl = baseUrl;
                ConfigurationName = configurationName;
            }

            public Task<ResultModel> ExecuteAsync(string query, CancellationToken cancellationToken)
            {
                return Task.FromResult(new ResultModel());
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellation);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_for_base_type_with_execute_method_without_cancelaltion_token
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithExecuteMethodWithWrongArguments), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithExecuteMethodWithWrongArguments
        {
            public Uri BaseUrl { get; set; }
            public string ConfigurationName { get; set; }

            public TestAsyncHttpClientBaseTypeWithExecuteMethodWithWrongArguments(Uri baseUrl, string configurationName)
            {
                BaseUrl = baseUrl;
                ConfigurationName = configurationName;
            }

            public Task<ResultModel> ExecuteAsync(RequestModel query)
            {
                return Task.FromResult(new ResultModel());
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellation);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_for_base_type_without_execute_async_method
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IService>(typeof(TestAsyncHttpClientBaseTypeWithExecuteMethodWithWrongArguments), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_base_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("baseType");

        public class TestAsyncHttpClientBaseTypeWithExecuteMethodWithWrongArguments
        {
            public Uri BaseUrl { get; set; }
            public string ConfigurationName { get; set; }

            public TestAsyncHttpClientBaseTypeWithExecuteMethodWithWrongArguments(Uri baseUrl, string configurationName)
            {
                BaseUrl = baseUrl;
                ConfigurationName = configurationName;
            }

            public Task<ResultModel> Execute(RequestModel query, CancellationToken cancellationToken)
            {
                return Task.FromResult(new ResultModel());
            }
        }

        public interface IService
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellation);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_null_interface
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance(null, new Uri("http://foo.bar/")));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_interface_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("interfaceType");
    }

    [TestClass]
    public class when_creating_instance_for_class_instead_of_interface
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<ServiceClassInsteadOfInterface>(new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");

        public abstract class ServiceClassInsteadOfInterface
        {
            public abstract ResultModel GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_for_class_instead_of_interface
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<ServiceClassInsteadOfInterface>(new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");

        public abstract class ServiceClassInsteadOfInterface
        {
            public abstract Task<ResultModel> GetResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_for_class_instead_of_interface
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<ServiceClassInsteadOfInterface>(new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");

        public abstract class ServiceClassInsteadOfInterface
        {
            public abstract Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_interface_with_two_methods
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithTwoMethods>(new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");

        public interface IServiceWithTwoMethods
        {
            ResultModel GetResult(RequestModel query);
            ResultModel GetAnotherResult(RequestModel query);
        }
         
        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_for_interface_with_two_methods
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithTwoMethods>(new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");

        public interface IServiceWithTwoMethods
        {
            Task<ResultModel> GetResult(RequestModel query);
            Task<ResultModel> GetAnotherResult(RequestModel query);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_for_interface_with_two_methods
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithTwoMethods>(new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");

        public interface IServiceWithTwoMethods
        {
            Task<ResultModel> GetResult(RequestModel query, CancellationToken cancellationToken);
            Task<ResultModel> GetAnotherResult(RequestModel query, CancellationToken cancellationToken);
        }

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }
    }

    [TestClass]
    public class when_creating_instance_for_interface_without_any_method
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance<IServiceWithoutAnyMethod>(new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");

        public interface IServiceWithoutAnyMethod
        {
        }
    }

    [TestClass]
    public class when_creating_instance_for_generic_type_definition_of_interface
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance(typeof(IService<,>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");

        public interface IService<in TQuery, out TResult>
        {
            TResult GetResult(TQuery query);
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_for_generic_type_definition_of_interface
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance(typeof(IService<,>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");

        public interface IService<in TQuery, TResult>
        {
            Task<TResult> GetResult(TQuery query);
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instance_with_cancellation_token_for_generic_type_definition_of_interface
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientFactory.CreateInstance(typeof(IService<,>), new Uri("http://foo.bar/")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_interface_type_argument =
            () => ((ArgumentException)exception).ParamName.ShouldEqual("interfaceType");

        public interface IService<in TQuery, TResult>
        {
            Task<TResult> GetResult(TQuery query, CancellationToken cancellationToken);
        }
    }

    [TestClass]
    public class when_creating_instances_concurrently
    {
        static List<Type> interfaces;
        static int count;

        Establish context = () =>
        {
            interfaces = new List<Type>();
            for (var i = 0; i < 1000; i++)
                interfaces.Add(TestHttpClientInterfaceBuilder.CreateInterface(i, typeof(RequestModel), typeof(ResultModel)));
        };

        Because of = () => Parallel.ForEach(interfaces, x =>
        {
            var s = HttpClientFactory.CreateInstance(typeof(TestHttpClientBaseTypeForConcurrentRun<,>), x, new Uri("http://www.foo.bar/"));

            var result = s.GetType().GetMethod("CallService").Invoke(s, new object[] { new RequestModel { Value = "Hello!" } });

            ((ResultModel)result).Value.ShouldEqual("Hello!");

            Interlocked.Increment(ref count);
        });

        It should_create_and_call_all_services =
            () => count.ShouldEqual(interfaces.Count);

        public class RequestModel
        {
            public string Value { get; set; }
        }
        
        public class ResultModel
        {
            public object Value { get; set; }
        }
        
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

        public class TestHttpClientBaseTypeForConcurrentRun<TInputModel, TOutputModel> : HttpClient<TInputModel, TOutputModel>
        {
            public string ExecutionMarker { get; set; }

            public TestHttpClientBaseTypeForConcurrentRun(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName, new TestExecuteStrategy())
            {
                ((TestExecuteStrategy)ExecuteStrategy).Client = this;
            }

            class TestExecuteStrategy : IExecuteStrategy<TInputModel, TOutputModel>
            {
                // ReSharper disable MemberCanBePrivate.Local
                public TestHttpClientBaseTypeForConcurrentRun<TInputModel, TOutputModel> Client { get; set; }
                // ReSharper restore MemberCanBePrivate.Local

                public TOutputModel Execute(TInputModel model, Func<TInputModel, TOutputModel> action)
                {
                    var result = Activator.CreateInstance<TOutputModel>();

                    if (typeof(TInputModel) != typeof(VoidType) && typeof(TOutputModel) != typeof(VoidType))
                        typeof(TOutputModel).GetProperty("Value").SetValue(result, typeof(TInputModel).GetProperty("Value").GetValue(model, null));

                    Client.ExecutionMarker = "Pipeline was executed.";

                    Thread.Sleep(10);

                    return result;
                }
            }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instances_concurrently
    {
        static List<Type> interfaces;
        static int count;

        Establish context = () =>
        {
            interfaces = new List<Type>();
            for (var i = 0; i < 1000; i++)
                interfaces.Add(TestAsyncHttpClientInterfaceBuilder.CreateInterface(i, typeof(RequestModel), typeof(ResultModel)));
        };

        Because of = () => Parallel.ForEach(interfaces, x =>
        {
            var s = HttpClientFactory.CreateInstance(typeof(TestAsyncHttpClientBaseTypeForConcurrentRun<,>), x, new Uri("http://www.foo.bar/"));

            var result = s.GetType().GetMethod("CallService").Invoke(s, new object[] { new RequestModel { Value = "Hello!" } });

            ((Task<ResultModel>)result).Result.Value.ShouldEqual("Hello!");

            Interlocked.Increment(ref count);
        });

        It should_create_and_call_all_services =
            () => count.ShouldEqual(interfaces.Count);

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }

        static class TestAsyncHttpClientInterfaceBuilder
        {
            static readonly ModuleBuilder ModuleBuilder;

            static TestAsyncHttpClientInterfaceBuilder()
            {
                var assemblyName = typeof(TestAsyncHttpClientInterfaceBuilder).Namespace + "__async_test_interfaces";

                var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                    new AssemblyName(assemblyName),
                    AssemblyBuilderAccess.Run);

                ModuleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName);
            }

            public static Type CreateInterface(int suffix, Type queryType, Type resultType)
            {
                var typeBuilder = ModuleBuilder.DefineType("IAsyncTestService" + suffix, 
                    TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.Public);

                typeBuilder.DefineMethod("CallService",
                    MethodAttributes.Public | MethodAttributes.Abstract | MethodAttributes.Virtual, 
                    typeof(Task<>).MakeGenericType(resultType), new[] { queryType });

                return typeBuilder.CreateType();
            }
        }

        public class TestAsyncHttpClientBaseTypeForConcurrentRun<TInputModel, TOutputModel> : AsyncHttpClient<TInputModel, TOutputModel>
        {
            public string ExecutionMarker { get; set; }

            public TestAsyncHttpClientBaseTypeForConcurrentRun(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName, new TestExecuteStrategy())
            {
                ((TestExecuteStrategy)ExecuteStrategy).Client = this;
            }

            class TestExecuteStrategy : IExecuteStrategy<TInputModel, Task<TOutputModel>>
            {
                // ReSharper disable MemberCanBePrivate.Local
                public TestAsyncHttpClientBaseTypeForConcurrentRun<TInputModel, TOutputModel> Client { get; set; }
                // ReSharper restore MemberCanBePrivate.Local

                public Task<TOutputModel> Execute(TInputModel model, Func<TInputModel, Task<TOutputModel>> action)
                {
                    var result = Activator.CreateInstance<TOutputModel>();

                    if (typeof(TInputModel) != typeof(VoidType) && typeof(TOutputModel) != typeof(VoidType))
                        typeof(TOutputModel).GetProperty("Value").SetValue(result, typeof(TInputModel).GetProperty("Value").GetValue(model, null));

                    Client.ExecutionMarker = "Async pipeline was executed.";

                    Thread.Sleep(10);

                    return Task.FromResult(result);
                }
            }
        }
    }

    [TestClass]
    public class when_creating_asynchronous_instances_with_cancellation_token_concurrently
    {
        static List<Type> interfaces;
        static int count;

        Establish context = () =>
        {
            interfaces = new List<Type>();
            for (var i = 0; i < 1000; i++)
                interfaces.Add(TestAsyncHttpClientInterfaceBuilder.CreateInterface(i, typeof(RequestModel), typeof(ResultModel)));
        };

        Because of = () => Parallel.ForEach(interfaces, x =>
        {
            var s = HttpClientFactory.CreateInstance(typeof(TestAsyncHttpClientBaseTypeForConcurrentRun<,>), x, new Uri("http://www.foo.bar/"));

            var result = s.GetType().GetMethod("CallService").Invoke(s, new object[] { new RequestModel { Value = "Hello!" }, CancellationToken.None });

            ((Task<ResultModel>)result).Result.Value.ShouldEqual("Hello!");

            Interlocked.Increment(ref count);
        });

        It should_create_and_call_all_services =
            () => count.ShouldEqual(interfaces.Count);

        public class RequestModel
        {
            public string Value { get; set; }
        }

        public class ResultModel
        {
            public object Value { get; set; }
        }

        static class TestAsyncHttpClientInterfaceBuilder
        {
            static readonly ModuleBuilder ModuleBuilder;

            static TestAsyncHttpClientInterfaceBuilder()
            {
                var assemblyName = typeof(TestAsyncHttpClientInterfaceBuilder).Namespace + "__async_canc_token_test_interfaces";

                var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                    new AssemblyName(assemblyName),
                    AssemblyBuilderAccess.Run);

                ModuleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName);
            }

            public static Type CreateInterface(int suffix, Type queryType, Type resultType)
            {
                var typeBuilder = ModuleBuilder.DefineType("IAsyncTestService" + suffix,
                    TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.Public);

                typeBuilder.DefineMethod("CallService",
                    MethodAttributes.Public | MethodAttributes.Abstract | MethodAttributes.Virtual,
                    typeof(Task<>).MakeGenericType(resultType), new[] { queryType, typeof(CancellationToken) });

                return typeBuilder.CreateType();
            }
        }

        public class TestAsyncHttpClientBaseTypeForConcurrentRun<TInputModel, TOutputModel> : AsyncHttpClient<TInputModel, TOutputModel>
        {
            public string ExecutionMarker { get; set; }

            public TestAsyncHttpClientBaseTypeForConcurrentRun(Uri baseUrl, string configurationName)
                : base(baseUrl, configurationName, new TestExecuteStrategy())
            {
                ((TestExecuteStrategy)ExecuteStrategy).Client = this;
            }

            class TestExecuteStrategy : IExecuteStrategy<TInputModel, Task<TOutputModel>>
            {
                // ReSharper disable MemberCanBePrivate.Local
                public TestAsyncHttpClientBaseTypeForConcurrentRun<TInputModel, TOutputModel> Client { get; set; }
                // ReSharper restore MemberCanBePrivate.Local

                public Task<TOutputModel> Execute(TInputModel model, Func<TInputModel, Task<TOutputModel>> action)
                {
                    var result = Activator.CreateInstance<TOutputModel>();

                    if (typeof(TInputModel) != typeof(VoidType) && typeof(TOutputModel) != typeof(VoidType))
                        typeof(TOutputModel).GetProperty("Value").SetValue(result, typeof(TInputModel).GetProperty("Value").GetValue(model, null));

                    Client.ExecutionMarker = "Async pipeline was executed.";

                    Thread.Sleep(10);

                    return Task.FromResult(result);
                }
            }
        }
    }
}
