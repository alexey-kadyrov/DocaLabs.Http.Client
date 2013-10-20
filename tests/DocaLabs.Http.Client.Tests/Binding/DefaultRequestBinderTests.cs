using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Tests.Binding._Utils;
using Machine.Specifications;
using Machine.Specifications.Annotations;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Binding
{
    [Subject(typeof(DefaultRequestBinder))]
    class when_default_request_binder_transforms_model
    {
        static DefaultRequestBinder request_binder;
        static object original_model;
        static object transformed_model;

        Establish context = () =>
        {
            original_model = new object();
            request_binder = new DefaultRequestBinder();
        };

        Because of =
            () => transformed_model = request_binder.TransformModel(new BindingContext(null, original_model, null, null));

        It should_return_original_model =
            () => transformed_model.ShouldBeTheSameAs(original_model);
    }

    [Subject(typeof(DefaultRequestBinder))]
    class when_default_request_binder_composes_url
    {
        static DefaultRequestBinder request_binder;
        static Uri base_url;
        static TestModel model;
        static BindingContext binding_context;
        static string url;

        Establish context = () =>
        {
            request_binder = new DefaultRequestBinder();
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");
            model = new TestModel
            {
                PathValue1 = "get this",
                PathValue2 = "another path",
                QueryValue1 = "Hello World!"
            };
            binding_context = new BindingContext(new TestClient(), model, null, base_url)
            {
                Model = model
            };
        };

        Because of =
            () => url = request_binder.ComposeUrl(binding_context);

        It should_add_model_values_to_appropariate_path_and_query_parts =
            () => url.ShouldEqual("http://foo.bar/product/get%20this/red/another%20path?c=en-IE&QueryValue1=Hello+World!");

        class TestModel
        {
            public string PathValue1 { [UsedImplicitly] get; set; }
            public string QueryValue1 { [UsedImplicitly] get; set; }
            public string PathValue2 { [UsedImplicitly] get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultRequestBinder))]
    class when_default_request_binder_is_infering_request_method_for_model_with_serialization_hint
    {
        static DefaultRequestBinder request_binder;
        static BindingContext binding_context;
        static string method;

        Establish context = () =>
        {
            var model = new Model();
            binding_context = new BindingContext(new Client(), model, null, null)
            {
                Model = model
            };
            request_binder = new DefaultRequestBinder();
        };

        Because of =
            () => method = request_binder.InferRequestMethod(binding_context);

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
            [UsedImplicitly]
            public string Value { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestBinder))]
    class when_default_request_binder_is_infering_request_method_for_model_without_serialization_hint
    {
        static DefaultRequestBinder request_binder;
        static BindingContext binding_context;
        static string method;

        Establish context = () =>
        {
            var model = new Model();
            binding_context = new BindingContext(new Client(), model, null, null)
            {
                Model = model
            };
            request_binder = new DefaultRequestBinder();
        };

        Because of =
            () => method = request_binder.InferRequestMethod(binding_context);

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
            [UsedImplicitly]
            public string Value { get; set; }
        }
    }

    [Subject(typeof(DefaultRequestBinder))]
    class when_default_request_binder_gets_headers
    {
        static TestModel model;
        static WebHeaderCollection headers;
        static DefaultRequestBinder request_binder;
        static BindingContext binding_context;

        Establish context = () =>
        {
            model = new TestModel
            {
                MyHeader = "Hello World!",
                JustValue = "Nothing"
            };
            binding_context = new BindingContext(new TestClient(), model, null, null)
            {
                Model = model
            };
            request_binder = new DefaultRequestBinder();
        };

        Because of =
            () => headers = request_binder.GetHeaders(binding_context);

        It should_map_properties_marked_as_header =
            () => headers.AllKeys.ShouldContainOnly("MyHeader");

        It should_convert_values_for_header =
            () => headers.GetValues("MyHeader").ShouldContainOnly("Hello World!");

        class TestModel
        {
            [RequestUse(RequestUseTargets.RequestHeader)]
            public string MyHeader { [UsedImplicitly] get; set; }

            public string JustValue { [UsedImplicitly] get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultRequestBinder))]
    class when_default_request_binder_gets_credentials
    {
        static DefaultRequestBinder request_binder;
        static BindingContext binding_context;
        static Model model;
        static ICredentials original_credentials;
        static ICredentials mapped_credentials;

        Establish context = () =>
        {
            original_credentials = new Mock<ICredentials>().Object;
            model = new Model
            {
                Value = "Hello World!",
                Credentials2 = original_credentials
            };
            binding_context = new BindingContext(new TestClient(), model, null, new Uri("http://contoso.com/"))
            {
                Model = model
            };
            request_binder = new DefaultRequestBinder();
        };

        Because of =
            () => mapped_credentials = request_binder.GetCredentials(binding_context);

        It should_return_credetials_object_from_the_model =
            () => mapped_credentials.ShouldBeTheSameAs(original_credentials);

        class Model
        {
            public string Value { [UsedImplicitly] get; set; }
            [UsedImplicitly]
            public ICredentials Credentials1 { get; set; }
            public ICredentials Credentials2 { [UsedImplicitly] get; set; }
        }

        class TestClient : HttpClient<Model, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultRequestBinder))]
    class when_default_request_binder_writes_to_request_body_model_with_serialization_hint
    {
        static DefaultRequestBinder request_binder;
        static BindingContext binding_context;

        Cleanup after =
            () => TestRequestSerializationAttribute.UsedMarker = "";

        Establish context = () =>
        {
            TestRequestSerializationAttribute.UsedMarker = "";
            var model = new Model();
            binding_context = new BindingContext(new Client(), model, null, null)
            {
                Model = model
            };
            request_binder = new DefaultRequestBinder();
        };

        Because of =
            () => request_binder.Write(binding_context, new Mock<WebRequest>().Object);

        It should_use_model_level_serializer =
            () => TestRequestSerializationAttribute.UsedMarker.ShouldEqual("model");

        [TestRequestSerialization(Marker = "model")]
        class Model
        {
            [UsedImplicitly]
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

    [Subject(typeof(DefaultRequestBinder))]
    class when_default_request_binder_asynchronously_writes_to_request_body_model_with_serialization_hint
    {
        static DefaultRequestBinder request_binder;
        static AsyncBindingContext binding_context;
        static CancellationToken cancellation_token;

        Cleanup after = () =>
        {
            TestRequestSerializationAttribute.UsedAsyncMarker = "";
            TestRequestSerializationAttribute.UsedCancellationToken = CancellationToken.None;
        };

        Establish context = () =>
        {
            TestRequestSerializationAttribute.UsedAsyncMarker = "";
            var model = new Model();
            cancellation_token = new CancellationTokenSource().Token;
            binding_context = new AsyncBindingContext(new Client(), model, null, null, cancellation_token)
            {
                Model = model
            };
            request_binder = new DefaultRequestBinder();
        };

        Because of =
            () => Task.WaitAll(request_binder.WriteAsync(binding_context, new Mock<WebRequest>().Object));

        It should_use_model_level_serializer =
            () => TestRequestSerializationAttribute.UsedAsyncMarker.ShouldEqual("model");

        It should_use_provided_cancellation_token =
            () => TestRequestSerializationAttribute.UsedCancellationToken.ShouldEqual(cancellation_token);

        [TestRequestSerialization(Marker = "model")]
        class Model
        {
            [UsedImplicitly]
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
}
