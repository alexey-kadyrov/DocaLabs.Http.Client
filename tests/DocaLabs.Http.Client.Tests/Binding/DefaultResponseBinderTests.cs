using System;
using System.IO;
using System.Net;
using System.Text;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.Serialization;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Binding
{
    [Subject(typeof(DefaultResponseBinder))]
    class when_default_response_binder_is_instantiated
    {
        static DefaultResponseBinder binder;

        Because of =
            () => binder = new DefaultResponseBinder();

        It should_instantiate_three_deserializers =
            () => binder.Providers.Count.ShouldEqual(3);

        It should_instantiate_json_deserializer =
            () => binder.Providers.ShouldContain(x => x is DeserializeFromJsonAttribute);

        It should_instantiate_xml_deserializer =
            () => binder.Providers.ShouldContain(x => x is DeserializeFromXmlAttribute);

        It should_instantiate_plain_text_deserializer_as_the_last_in_the_list =
            () => binder.Providers[2].ShouldBeOfType<DeserializeFromPlainTextAttribute>();
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_with_null_context
    {
        static DefaultResponseBinder binder;
        static Exception exception;

        Establish context =
            () => binder = new DefaultResponseBinder();

        Because of =
            () => exception = Catch.Exception(() => binder.Read(null, new Mock<WebRequest>().Object, typeof (string)));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_context_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("context");
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_null_result_type
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static Exception exception;

        Establish context = () =>
        {
            binding_context = new BindingContext(new HttpClient<string, string>(new Uri("http://foo.bar")), null, null, null);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => exception = Catch.Exception(() => binder.Read(binding_context, new Mock<WebRequest>().Object, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_result_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("resultType");
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_type_with_response_deserialization_attribute : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static Model data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = (Model)binder.Read(binding_context, mock_request.Object, typeof(Model));

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestResponseDeserialization(ReturnType = typeof(Model))]
        class Model
        {
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_type_and_client_with_response_deserialization_attribute : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static Model data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = (Model)binder.Read(binding_context, mock_request.Object, typeof(Model));

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        [TestResponseDeserialization(ReturnType = typeof(object))]
        class Client : HttpClient<string, Model>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestResponseDeserialization(ReturnType = typeof(Model))]
        class Model
        {
        }
    }

    [Subject(typeof(DefaultResponseBinder))]
    class when_reading_for_client_with_response_deserialization_attribute : default_response_binder_test_context
    {
        static DefaultResponseBinder binder;
        static BindingContext binding_context;
        static int data;

        Establish context = () =>
        {
            Setup("text/plain; charset=utf-8", new MemoryStream(Encoding.UTF8.GetBytes("Hello World!")));
            binding_context = new BindingContext(new Client(), null, null, null);
            binder = new DefaultResponseBinder();
        };

        Because of =
            () => data = (int)binder.Read(binding_context, mock_request.Object, typeof(int));

        It should_return_expected_data_using_attribute_on_the_model =
            () => data.ShouldNotBeNull();

        [TestResponseDeserialization(ReturnType = typeof(int))]
        class Client : HttpClient<string, int>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class Model
        {
        }
    }

    class default_response_binder_test_context
    {
        protected static Mock<WebRequest> mock_request;
        protected static Mock<WebResponse> mock_response;

        protected static void Setup(string contentType, Stream stream)
        {
            mock_response = new Mock<WebResponse>();
            mock_response.SetupAllProperties();
            mock_response.Setup(x => x.GetResponseStream()).Returns(stream);
            mock_response.Object.ContentType = contentType;
            mock_response.Object.ContentLength = stream.Length;

            mock_request = new Mock<WebRequest>();
            mock_request.Setup(x => x.GetResponse()).Returns(mock_response.Object);
        }
    }

    class TestResponseDeserializationAttribute : ResponseDeserializationAttribute
    {
        public Type ReturnType { get; set; }

        public override object Deserialize(HttpResponseStream responseStream, Type resultType)
        {
            return Activator.CreateInstance(ReturnType);
        }
    }
}
