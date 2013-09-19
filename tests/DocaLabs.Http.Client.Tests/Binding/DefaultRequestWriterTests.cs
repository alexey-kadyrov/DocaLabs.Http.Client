using System;
using System.IO;
using System.Net;
using System.Text;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Tests.Binding._Utils;
using DocaLabs.Http.Client.Tests._Utils;
using Machine.Specifications;
using Machine.Specifications.Annotations;
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

        It should_report_client_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("client");

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
    class when_trying_to_infer_http_method_for_property_with_serialization_hint
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
            [TestRequestSerialization]
            public string Value { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "InferRequestMethod")]
    class when_trying_to_infer_http_method_for_property_with_serialization_hint_and_client_has_use_in_request_ignore
    {
        static DefaultRequestWriter writer;
        static string method;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => method = writer.InferRequestMethod(new Client(), new Model());

        It should_still_return_post_method =
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
            [TestRequestSerialization, RequestUse(RequestUseTargets.Ignore)]
            public string Value { get; set; }
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

        [TestRequestSerialization]
        class Model
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "InferRequestMethod")]
    class when_trying_to_infer_http_method_for_model_with_serialization_hint_and_client_has_use_in_request_ignore
    {
        static DefaultRequestWriter writer;
        static string method;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => method = writer.InferRequestMethod(new Client(), new Model());

        It should_still_return_post_method =
            () => method.ShouldBeEqualIgnoringCase("POST");

        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestRequestSerialization, RequestUse(RequestUseTargets.Ignore)]
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

        [TestRequestSerialization]
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
    class when_trying_to_infer_http_method_for_client_with_serialization_hint_and_client_has_use_in_request_ignore
    {
        static DefaultRequestWriter writer;
        static string method;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => method = writer.InferRequestMethod(new Client(), new Model());

        It should_still_return_post_method =
            () => method.ShouldBeEqualIgnoringCase("POST");

        [TestRequestSerialization, RequestUse(RequestUseTargets.Ignore)]
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
    class when_trying_to_infer_http_method_for_property_of_stream_type
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
            public Stream Value { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "InferRequestMethod")]
    class when_trying_to_infer_http_method_for_property_of_stream_derived_type
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
            public FileStream Value { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "InferRequestMethod")]
    class when_trying_to_infer_http_method_for_model_of_stream_derived_type
    {
        static DefaultRequestWriter writer;
        static string method;

        Establish context =
            () => writer = new DefaultRequestWriter();

        Because of =
            () => method = writer.InferRequestMethod(new Client(), new MemoryStream());

        It should_return_post_method =
            () => method.ShouldBeEqualIgnoringCase("POST");

        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar"))
            {
            }
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

        It should_report_client_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("client");

        class Model
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "Write")]
    class when_trying_to_write_using_model_with_serialization_attribute_on_property_and_model_and_client_levels
    {
        static DefaultRequestWriter writer;

        Cleanup after =
            () => TestRequestSerializationAttribute.UsedMarker = "";

        Establish context = () =>
        {
            TestRequestSerializationAttribute.UsedMarker = "";
            writer = new DefaultRequestWriter();
        };

        Because of =
            () => writer.Write(new Client(), new Model { Value = "Hello World!"}, new Mock<WebRequest>().Object);

        It should_use_property_level_serializer =
            () => TestRequestSerializationAttribute.UsedMarker.ShouldEqual("property");

        [TestRequestSerialization(Marker = "model")]
        class Model
        {
            [TestRequestSerialization(Marker = "property")]
            public string Value { get; set; }
        }

        [TestRequestSerialization(Marker = "client")]
        class Client : HttpClient<Model, string>
        {
            public Client() : base(new Uri("http://foo.bar/"))
            {
            }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "Write")]
    class when_trying_to_write_using_model_with_serialization_attribute_on_null_property_and_model_and_client_levels
    {
        static HttpWebRequest web_request;
        static DefaultRequestWriter writer;

        Establish context = () =>
        {
            web_request = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://foo.bar"));
            web_request.ContentLength = 99;
            web_request.Method = "POST";
            writer = new DefaultRequestWriter();
        };

        Because of =
            () => writer.Write(new Client(), new Model(), web_request);

        It should_set_content_length_to_zero =
            () => web_request.ContentLength.ShouldEqual(0);

        [TestRequestSerialization(Marker = "model")]
        class Model
        {
            [TestRequestSerialization(Marker = "property")]
            public string Value { get; set; }
        }

        [TestRequestSerialization]
        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "Write")]
    class when_trying_to_write_using_model_with_serialization_attribute_on_model_and_client_levels
    {
        static DefaultRequestWriter writer;

        Cleanup after =
            () => TestRequestSerializationAttribute.UsedMarker = "";

        Establish context = () =>
        {
            TestRequestSerializationAttribute.UsedMarker = "";
            writer = new DefaultRequestWriter();
        };

        Because of =
            () => writer.Write(new Client(), new Model(), new Mock<WebRequest>().Object);

        It should_use_model_level_serializer =
            () => TestRequestSerializationAttribute.UsedMarker.ShouldEqual("model");

        [TestRequestSerialization(Marker = "model")]
        class Model
        {
            public string Value { get; set; }
        }

        [TestRequestSerialization(Marker = "client")]
        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "Write")]
    class when_trying_to_write_using_model_with_serialization_attribute_on_client_levels
    {
        static DefaultRequestWriter writer;

        Cleanup after =
            () => TestRequestSerializationAttribute.UsedMarker = "";

        Establish context = () =>
        {
            TestRequestSerializationAttribute.UsedMarker = "";
            writer = new DefaultRequestWriter();
        };

        Because of =
            () => writer.Write(new Client(), new Model(), new Mock<WebRequest>().Object);

        It should_use_client_level_serializer =
            () => TestRequestSerializationAttribute.UsedMarker.ShouldEqual("client");

        class Model
        {
            public string Value { get; set; }
        }

        [TestRequestSerialization(Marker = "client")]
        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "Write")]
    class when_trying_to_write_using_model_with_serialization_attribute_on_property_level_and_property_has_use_in_request_ignore
    {
        static DefaultRequestWriter writer;

        Cleanup after =
            () => TestRequestSerializationAttribute.UsedMarker = "";

        Establish context = () =>
        {
            TestRequestSerializationAttribute.UsedMarker = "";
            writer = new DefaultRequestWriter();
        };

        Because of =
            () => writer.Write(new Client(), new Model { Value = "Hello World!"}, new Mock<WebRequest>().Object);

        It should_still_use_property_serializer =
            () => TestRequestSerializationAttribute.UsedMarker.ShouldEqual("property");

        class Model
        {
            [TestRequestSerialization(Marker = "property"), RequestUse(RequestUseTargets.Ignore)]
            public string Value { get; set; }
        }

        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "Write")]
    class when_trying_to_write_using_model_with_serialization_attribute_on_model_level_and_model_has_use_in_request_ignore
    {
        static DefaultRequestWriter writer;

        Cleanup after =
            () => TestRequestSerializationAttribute.UsedMarker = "";

        Establish context = () =>
        {
            TestRequestSerializationAttribute.UsedMarker = "";
            writer = new DefaultRequestWriter();
        };

        Because of =
            () => writer.Write(new Client(), new Model(), new Mock<WebRequest>().Object);

        It should_still_use_model_serializer =
            () => TestRequestSerializationAttribute.UsedMarker.ShouldEqual("model");

        [TestRequestSerialization(Marker = "model"), RequestUse(RequestUseTargets.Ignore)]
        class Model
        {
            public string Value { get; set; }
        }

        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "Write")]
    class when_trying_to_write_using_model_with_serialization_attribute_on_client_level_and_client_has_use_in_request_ignore
    {
        static DefaultRequestWriter writer;

        Cleanup after =
            () => TestRequestSerializationAttribute.UsedMarker = "";

        Establish context = () =>
        {
            TestRequestSerializationAttribute.UsedMarker = "";
            writer = new DefaultRequestWriter();
        };

        Because of =
            () => writer.Write(new Client(), new Model(), new Mock<WebRequest>().Object);

        It should_still_use_client_serializer =
            () => TestRequestSerializationAttribute.UsedMarker.ShouldEqual("client");

        class Model
        {
            public string Value { get; set; }
        }

        [TestRequestSerialization(Marker = "client"), RequestUse(RequestUseTargets.Ignore)]
        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "Write")]
    class when_trying_to_write_using_null_model
    {
        static HttpWebRequest web_request; 
        static DefaultRequestWriter writer;

        Establish context = () =>
        {
            web_request = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://foo.bar"));
            web_request.ContentLength = 99;
            web_request.Method = "POST";
            writer = new DefaultRequestWriter();
        };

        Because of =
            () => writer.Write(new Client(), null, web_request);

        It should_set_content_length_to_zero =
            () => web_request.ContentLength.ShouldEqual(0);

        [TestRequestSerialization]
        class Client : HttpClient<Model, string>
        {
            public Client()
                : base(new Uri("http://foo.bar/"))
            {
            }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "Write")]
    class when_trying_to_write_using_model_with_stream_property : request_serialization_test_context
    {
        static DefaultRequestWriter writer;
        static Model model;

        Establish context = () =>
        {
            writer = new DefaultRequestWriter();
            model = new Model
            {
                Id = "123456789",
                Value = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"))
            };
        };

        Because of =
            () => writer.Write(new HttpClient<Model, string>(new Uri("http://foo.bar/")), model, mock_web_request.Object);

        It should_set_request_content_type_as_application_octet =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/octet-stream");

        It should_serialize_the_property_value =
            () => GetRequestData().ShouldEqual("Hello World!");

        class Model
        {
            public string Id { [UsedImplicitly] get; set; }
            public Stream Value { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(DefaultRequestWriter), "Write")]
    class when_trying_to_write_using_stream_model : request_serialization_test_context
    {
        static DefaultRequestWriter writer;
        static Stream model;

        Establish context = () =>
        {
            writer = new DefaultRequestWriter();
            model = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"));
        };

        Because of =
            () => writer.Write(new HttpClient<Model, string>(new Uri("http://foo.bar/")), model, mock_web_request.Object);

        It should_set_request_content_type_as_application_octet =
            () => mock_web_request.Object.ContentType.ShouldBeEqualIgnoringCase("application/octet-stream");

        It should_serialize_the_property_value =
            () => GetRequestData().ShouldEqual("Hello World!");
    }

    // ReSharper restore UnusedMember.Local
}
