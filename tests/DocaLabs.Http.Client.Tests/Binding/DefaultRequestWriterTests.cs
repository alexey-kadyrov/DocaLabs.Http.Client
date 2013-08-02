using System;
using System.Net;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Binding.Serialization;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Binding
{
    // ReSharper disable UnusedMember.Local

    [Subject(typeof(DefaultRequestWriter), "InferRequestMethod")]
    class when_trying_to_infer_http_method_for_null_client
    {
        static DefaultRequestWriter writer;
        static Exception exception;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => exception = Catch.Exception(() => writer.InferRequestMethod(null, new Model()));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_http_client_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("httpClient");

        class Model
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "InferRequestMethod")]
    class when_trying_to_infer_http_method_for_null_model
    {
        static DefaultRequestWriter writer;
        static string method;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => method = writer.InferRequestMethod(new Client(), null);

        It should_return_get_method =
            () => method.ShouldBeEqualIgnoringCase("GET");

        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "InferRequestMethod")]
    class when_trying_to_infer_http_method_for_model_without_any_serialization_hints
    {
        static DefaultRequestWriter writer;
        static string method;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => method = writer.InferRequestMethod(new Client(), new Model());

        It should_return_get_method =
            () => method.ShouldBeEqualIgnoringCase("GET");

        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class Model
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "InferRequestMethod")]
    class when_trying_to_infer_http_method_for_model_without_any_serialization_hints_but_with_deserialization
    {
        static DefaultRequestWriter writer;
        static string method;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => method = writer.InferRequestMethod(new Client(), new Model());

        It should_return_get_method =
            () => method.ShouldBeEqualIgnoringCase("GET");

        [TestResponseDeserialization]
        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestResponseDeserialization]
        class Model
        {
            public string Value { get; set; }
        }

        class TestResponseDeserializationAttribute : ResponseDeserializationAttribute
        {
            public override object Deserialize(HttpResponseStream responseStream, Type resultType)
            {
                return new object();
            }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "InferRequestMethod")]
    class when_trying_to_infer_http_method_for_model_without_any_serialization_hints_but_with_request_usage_hints
    {
        static DefaultRequestWriter writer;
        static string method;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => method = writer.InferRequestMethod(new Client(), new Model());

        It should_return_get_method =
            () => method.ShouldBeEqualIgnoringCase("GET");

        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class Model
        {
            [RequestUse(RequestUseTargets.UrlQuery)]
            public string Value1 { get; set; }

            [RequestUse(RequestUseTargets.UrlPath)]
            public string Value2 { get; set; }

            [RequestUse(RequestUseTargets.RequestHeader)]
            public string Value3 { get; set; }

            [RequestUse(RequestUseTargets.Ignore)]
            public string Value4 { get; set; }

            public WebHeaderCollection Headers { get; set; }

            public ICredentials Credentials { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "InferRequestMethod")]
    class when_trying_to_infer_http_method_for_model_without_any_serialization_hints_but_with_request_usage_as_form_in_the_body
    {
        static DefaultRequestWriter writer;
        static string method;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => method = writer.InferRequestMethod(new Client(), new Model());

        It should_return_post_method =
            () => method.ShouldBeEqualIgnoringCase("POST");

        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class Model
        {
            [SerializeAsForm]
            public string Value2 { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "InferRequestMethod")]
    class when_trying_to_infer_http_method_for_model_with_serialization_hint
    {
        static DefaultRequestWriter writer;
        static string method;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => method = writer.InferRequestMethod(new Client(), new Model());

        It should_return_post_method =
            () => method.ShouldBeEqualIgnoringCase("POST");

        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestRequestSerializationAttribute]
        class Model
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "InferRequestMethod")]
    class when_trying_to_infer_http_method_for_client_with_serialization_hint
    {
        static DefaultRequestWriter writer;
        static string method;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => method = writer.InferRequestMethod(new Client(), new Model());

        It should_return_post_method =
            () => method.ShouldBeEqualIgnoringCase("POST");

        [TestRequestSerializationAttribute]
        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class Model
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "Write")]
    class when_trying_to_write_using_null_client
    {
        static DefaultRequestWriter writer;
        static Exception exception;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => exception = Catch.Exception(() => writer.Write(null, new Model(), new Mock<WebRequest>().Object));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_http_client_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("httpClient");

        class Model
        {
            public string Value { get; set; }
        }
    }

    class TestRequestSerializationAttribute : RequestSerializationAttribute
    {
        public override void Serialize(object obj, WebRequest request)
        {
        }
    }

    // ReSharper restore UnusedMember.Local
}
