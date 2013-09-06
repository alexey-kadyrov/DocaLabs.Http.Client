using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Binding.Serialization;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    // ReSharper disable UnusedMember.Local

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_model_which_does_not_have_processing_hints
    {
        static Uri base_url;
        static TestModel model;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");
            model = new TestModel
            {
                PathValue1 = "get this",
                PathValue2 = "another path",
                QueryValue1 = "Hello World!"
            };
        };

        Because of =
            () => url = composer.Compose(new TestClient(), model, base_url);

        It should_add_model_values_to_appropariate_path_and_query_parts =
            () => url.ShouldEqual("http://foo.bar/product/get%20this/red/another%20path?c=en-IE&QueryValue1=Hello+World!");

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class TestModel
        {
            public string PathValue1 { get; set; }
            public string QueryValue1 { get; set; }
            public string PathValue2 { get; set; }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_model_which_has_processing_hints
    {
        static Uri base_url;
        static TestModel model;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/{path}/red/{PathAndQueryValue}?c=en-IE");
            model = new TestModel
            {
                PathValue1 = "get this",
                PathAndQueryValue = "Hello World!"
            };
        };

        Because of =
            () => url = composer.Compose(new TestClient(),  model, base_url);

        It should_add_model_values_to_appropariate_path_and_query_parts =
            () => url.ShouldEqual("http://foo.bar/product/get%20this/red/Hello%20World!?c=en-IE&PathAndQueryValue=Hello+World!");

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class TestModel
        {
            [PropertyOverrides(Name = "Path")]
            public string PathValue1 { get; set; }
            [RequestUse(RequestUseTargets.UrlPath | RequestUseTargets.UrlQuery)]
            public string PathAndQueryValue { get; set; }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_model_which_has_request_serialization_attribute
    {
        static Uri base_url;
        static TestModel model;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/red/{PathAndQueryValue}?c=en-IE");
            model = new TestModel
            {
                ImplicitValue = "get this",
                PathAndQueryValue = "Hello World!"
            };
        };

        Because of =
            () => url = composer.Compose(new TestClient(), model, base_url);

        It should_add_explict_values_only_to_appropariate_path_and_query_parts =
            () => url.ShouldEqual("http://foo.bar/product/red/Hello%20World!?c=en-IE&PathAndQueryValue=Hello+World!");

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestSerializer]
        class TestModel
        {
            public string ImplicitValue { get; set; }
            [RequestUse(RequestUseTargets.UrlPath | RequestUseTargets.UrlQuery)]
            public string PathAndQueryValue { get; set; }
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_http_client_which_has_request_serialization_attribute
    {
        static Uri base_url;
        static TestModel model;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/red/{PathAndQueryValue}?c=en-IE");
            model = new TestModel
            {
                ImplicitValue = "get this",
                PathAndQueryValue = "Hello World!"
            };
        };

        Because of =
            () => url = composer.Compose(new TestClient(), model, base_url);

        It should_add_explict_values_only_to_appropariate_path_and_query_parts =
            () => url.ShouldEqual("http://foo.bar/product/red/Hello%20World!?c=en-IE&PathAndQueryValue=Hello+World!");

        [TestSerializer]
        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class TestModel
        {
            public string ImplicitValue { get; set; }
            [RequestUse(RequestUseTargets.UrlPath | RequestUseTargets.UrlQuery)]
            public string PathAndQueryValue { get; set; }
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_namevaluecollection_model
    {
        static Uri base_url;
        static NameValueCollection model;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");
            model = new NameValueCollection
            {
                { "PathValue1", "get this" },
                { "PathValue2", "another path" },
                { "QueryValue1", "Hello World!" }
            };
        };

        Because of =
            () => url = composer.Compose(new TestClient(), model, base_url);

        It should_add_model_values_to_appropariate_path_and_query_parts =
            () => url.ShouldEqual("http://foo.bar/product/get%20this/red/another%20path?c=en-IE&QueryValue1=Hello+World!");

        class TestClient : HttpClient<NameValueCollection, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_namevaluecollection_model_and_client_with_request_serialization_attribute
    {
        static Uri base_url;
        static NameValueCollection model;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");
            model = new NameValueCollection
            {
                { "PathValue1", "get this" },
                { "PathValue2", "another path" },
                { "QueryValue1", "Hello World!" }
            };
        };

        Because of =
            () => url = composer.Compose(new TestClient(), model, base_url);

        It should_not_add_model_values =
            () => url.ShouldEqual("http://foo.bar/product/%7BpathValue1%7D/red/%7BpathValue2%7D?c=en-IE");

        [TestSerializer]
        class TestClient : HttpClient<NameValueCollection, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_subclass_of_namevaluecollection_model_with_request_serialization_attribute
    {
        static Uri base_url;
        static TestNameValueCollection model;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");
            model = new TestNameValueCollection
            {
                { "PathValue1", "get this" },
                { "PathValue2", "another path" },
                { "QueryValue1", "Hello World!" }
            };
        };

        Because of =
            () => url = composer.Compose(new TestClient(), model, base_url);

        It should_not_add_model_values =
            () => url.ShouldEqual("http://foo.bar/product/%7BpathValue1%7D/red/%7BpathValue2%7D?c=en-IE");

        class TestClient : HttpClient<TestNameValueCollection, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }

        [TestSerializer]
        class TestNameValueCollection : NameValueCollection
        {
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_dictionary_model
    {
        static Uri base_url;
        static Dictionary<string, string> model;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");
            model = new Dictionary<string, string>
            {
                { "PathValue1", "get this" },
                { "PathValue2", "another path" },
                { "QueryValue1", "Hello World!" }
            };
        };

        Because of =
            () => url = composer.Compose(new TestClient(), model, base_url);

        It should_add_model_values_to_appropariate_path_and_query_parts =
            () => url.ShouldEqual("http://foo.bar/product/get%20this/red/another%20path?c=en-IE&QueryValue1=Hello+World!");

        class TestClient : HttpClient<Dictionary<string, string>, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_generic_dictionary_model_and_client_with_request_serialization_attribute
    {
        static Uri base_url;
        static Dictionary<string, string> model;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");
            model = new Dictionary<string, string>
            {
                { "PathValue1", "get this" },
                { "PathValue2", "another path" },
                { "QueryValue1", "Hello World!" }
            };
        };

        Because of =
            () => url = composer.Compose(new TestClient(), model, base_url);

        It should_not_add_model_values =
            () => url.ShouldEqual("http://foo.bar/product/%7BpathValue1%7D/red/%7BpathValue2%7D?c=en-IE");

        [TestSerializer]
        class TestClient : HttpClient<Dictionary<string, string>, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_subclass_of_generic_dictionary_model_with_request_serialization_attribute
    {
        static Uri base_url;
        static TestDictionary model;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");
            model = new TestDictionary
            {
                { "PathValue1", "get this" },
                { "PathValue2", "another path" },
                { "QueryValue1", "Hello World!" }
            };
        };

        Because of =
            () => url = composer.Compose(new TestClient(), model, base_url);

        It should_not_add_model_values =
            () => url.ShouldEqual("http://foo.bar/product/%7BpathValue1%7D/red/%7BpathValue2%7D?c=en-IE");

        class TestClient : HttpClient<TestDictionary, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }

        [TestSerializer]
        class TestDictionary : Dictionary<string, string>
        {
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_null_model
    {
        static Uri base_url;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");
        };

        Because of =
            () => url = composer.Compose(new TestClient(), null, base_url);

        It should_return_anmodified_url =
            () => url.ShouldEqual("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");

        class TestClient : HttpClient<string, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_with_null_base_url
    {
        static Exception exception;
        static TestModel model;
        static DefaultUrlComposer composer;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            model = new TestModel
            {
                PathValue1 = "get this",
                PathValue2 = "another path",
                QueryValue1 = "Hello World!"
            };
        };

        Because of =
            () => exception = Catch.Exception(() => composer.Compose(new TestClient(), model, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_base_url_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("baseUrl");

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class TestModel
        {
            public string PathValue1 { get; set; }
            public string QueryValue1 { get; set; }
            public string PathValue2 { get; set; }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_with_null_http_client
    {
        static Exception exception;
        static TestModel model;
        static DefaultUrlComposer composer;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            model = new TestModel
            {
                PathValue1 = "get this",
                PathValue2 = "another path",
                QueryValue1 = "Hello World!"
            };
        };

        Because of =
            () => exception = Catch.Exception(() => composer.Compose(null, model, new Uri("http://foo.bar")));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_base_client_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("client");
        class TestModel
        {
            public string PathValue1 { get; set; }
            public string QueryValue1 { get; set; }
            public string PathValue2 { get; set; }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_there_is_a_general_exception_during_composing
    {
        static Uri base_url;
        static TestModel model;
        static DefaultUrlComposer composer;
        static Exception exception;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");
            model = new TestModel
            {
                PathValue1 = "get this",
                QueryValue1 = "Hello World!"
            };
        };

        Because of =
            () => exception = Catch.Exception(() => composer.Compose(new TestClient(), model, base_url));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It should_wrap_the_original_exception =
            () => exception.InnerException.ShouldBeOfType<TargetInvocationException>();

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class TestModel
        {
            public string PathValue1 { get; set; }
            public string QueryValue1 { get; set; }
            public string PathValue2 { get { throw new Exception("It's me!"); } }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_base_url_without_path
    {
        static Uri base_url;
        static TestModel model;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar?c=en-IE");
            model = new TestModel
            {
                QueryValue1 = "Hello World!"
            };
        };

        Because of =
            () => url = composer.Compose(new TestClient(), model, base_url);

        It should_add_model_values_to_appropariate_query_parts =
            () => url.ShouldEqual("http://foo.bar/?c=en-IE&QueryValue1=Hello+World!");

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class TestModel
        {
            public string QueryValue1 { get; set; }
        }
    }

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_base_url_without_existing_query
    {
        static Uri base_url;
        static TestModel model;
        static DefaultUrlComposer composer;
        static string url;

        Establish context = () =>
        {
            composer = new DefaultUrlComposer();
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}");
            model = new TestModel
            {
                PathValue1 = "get this",
                PathValue2 = "another path",
                QueryValue1 = "Hello World!"
            };
        };

        Because of =
            () => url = composer.Compose(new TestClient(), model, base_url);

        It should_add_model_values_to_appropariate_path_and_query_parts =
            () => url.ShouldEqual("http://foo.bar/product/get%20this/red/another%20path?QueryValue1=Hello+World!");

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        class TestModel
        {
            public string PathValue1 { get; set; }
            public string QueryValue1 { get; set; }
            public string PathValue2 { get; set; }
        }
    }

    // ReSharper restore UnusedMember.Local
    // ReSharper restore UnusedAutoPropertyAccessor.Local
}
