﻿using System;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.UrlMapping;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.UrlMapping
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    // ReSharper disable ClassNeverInstantiated.Local
    // ReSharper disable UnusedMember.Local

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_for_url_without_input_model_and_type_without_serialization_hints
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/product/red/");

            client = new TestClient(base_url);

            model = new TestModel { Value = "Hello World!" };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, client, base_url);

        It should_not_modify_authority_and_path_and_should_add_input_model_part =
            () => url.ToString().ShouldEqual("http://foo.bar/product/red/?Value=Hello+World!");

        class TestModel
        {
            public string Value { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl) : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_for_url_with_some_input_model_part_and_type_without_serialization_hints
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/product/red?keepMe=Yes");

            client = new TestClient(base_url);

            model = new TestModel { Value = "Hello World!" };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, client, base_url);

        It should_not_modify_authority_and_path_and_should_keep_existing_input_model_part_at_left_and_add_new_input_model_part =
            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/product/red?keepMe=Yes&Value=Hello+World!");

        class TestModel
        {
            public string Value { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_for_url_with_for_type_with_serialization_hints_for_ordered_path_and_query
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

            client = new TestClient(base_url);

            model = new TestModel
            {
                ProductCategory = "red category",
                ProductId = 42,
                Value = "Hello World!"
            };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, client, base_url);

        It should_not_modify_authority_and_path_and_should_keep_existing_input_model_part_at_left_and_add_new_input_model_part =
            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/red%20category/42?keepMe=Yes&Value=Hello+World!");

        class TestModel
        {
            [OrderedRequestPath(1)]
            public string ProductCategory { get; set; }

            [OrderedRequestPath(2)]
            public int ProductId { get; set; }

            public string Value { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_for_url_with_for_type_with_serialization_hints_for_named_path_and_query
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/catalog/{productCategory}/{productId}?keepMe=Yes");

            client = new TestClient(base_url);

            model = new TestModel
            {
                ProductCategory = "red category",
                ProductId = 42,
                Value = "Hello World!"
            };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, client, base_url);

        It should_not_modify_authority_and_path_and_should_keep_existing_input_model_part_at_left_and_add_new_input_model_part =
            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/red%20category/42?keepMe=Yes&Value=Hello+World!");

        class TestModel
        {
            [NamedRequestPath]
            public string ProductCategory { get; set; }

            [NamedRequestPath]
            public int ProductId { get; set; }

            public string Value { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_for_url_with_for_type_with_serialization_hints_for_named_and_ordered_path_and_query
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/catalog/{productCategory}/{productId}?keepMe=Yes");

            client = new TestClient(base_url);

            model = new TestModel
            {
                ProductCategory = "red category",
                ViewType = "small",
                ProductId = 42,
                Value = "Hello World!"
            };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, client, base_url);

        It should_not_modify_authority_and_path_and_should_keep_existing_input_model_part_at_left_and_add_new_input_model_part =
            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/red%20category/42/small?keepMe=Yes&Value=Hello+World!");

        class TestModel
        {
            [NamedRequestPath]
            public string ProductCategory { get; set; }

            [OrderedRequestPath(1)]
            public string ViewType { get; set; }

            [NamedRequestPath]
            public int ProductId { get; set; }

            public string Value { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_for_url_with_user_password_existing_input_model_part_and_fragment
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://user1:password1@foo.bar/product/red?keepMe=Yes#keepMeAsWell/andMe?andMeToo");

            client = new TestClient(base_url);

            model = new TestModel { Value = "Hello World!" };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, client, base_url);

        It should_not_modify_user_password_authority_path_existing_qyery_part_and_fragment_and_should_add_new_input_model_part =
            () => url.AbsoluteUri.ShouldEqual("http://user1:password1@foo.bar/product/red?keepMe=Yes&Value=Hello+World!#keepMeAsWell/andMe?andMeToo");

        class TestModel
        {
            public string Value { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_to_create_ordered_path_where_empty_value_follow_non_emtpy
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://user1:password1@foo.bar/product/?keepMe=Yes#keepMeAsWell/andMe?andMeToo");

            client = new TestClient(base_url);

            model = new TestModel
            {
                Value1 = "Hello World",
                Value2 = "",
                Value3 = ""
            };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, client, base_url);

        It should_not_modify_user_password_authority_path_existing_qyery_part_and_fragment_and_should_add_new_input_model_part =
            () => url.AbsoluteUri.ShouldEqual("http://user1:password1@foo.bar/product/Hello%20World?keepMe=Yes#keepMeAsWell/andMe?andMeToo");

        class TestModel
        {
            [OrderedRequestPath(1)]
            public string Value1 { get; set; }

            [OrderedRequestPath(2)]
            public string Value2 { get; set; }

            [OrderedRequestPath(3)]
            public string Value3 { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_to_create_named_path_where_empty_value_follow_non_emtpy
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/catalog/{productCategory}/{productId}?keepMe=Yes");

            client = new TestClient(base_url);

            model = new TestModel
            {
                ProductCategory = "red category",
                Value = "Hello World!"
            };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, client, base_url);

        It should_not_modify_authority_and_path_and_should_keep_existing_input_model_part_at_left_and_add_new_input_model_part =
            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/red%20category/?keepMe=Yes&Value=Hello+World!");

        class TestModel
        {
            [NamedRequestPath]
            public string ProductCategory { get; set; }

            [NamedRequestPath]
            public string ProductId { get; set; }

            public string Value { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_with_model_instance_marked_as_ignore
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

            client = new TestClient(base_url);

            model = new TestModel
            {
                ProductCategory = "red",
                ProductId = 42,
                Value = "Hello World!"
            };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, client, base_url);

        It should_not_modify_url =
            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/?keepMe=Yes");

        [IgnoreInRequest]
        class TestModel
        {
            [OrderedRequestPath(1)]
            public string ProductCategory { get; set; }

            [OrderedRequestPath(2)]
            public int ProductId { get; set; }

            public string Value { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_with_client_instance_marked_as_ignore
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

            client = new TestClient(base_url);

            model = new TestModel
            {
                ProductCategory = "red",
                ProductId = 42,
                Value = "Hello World!"
            };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, client, base_url);

        It should_not_modify_url =
            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/?keepMe=Yes");

        class TestModel
        {
            [OrderedRequestPath(1)]
            public string ProductCategory { get; set; }

            [OrderedRequestPath(2)]
            public int ProductId { get; set; }

            public string Value { get; set; }
        }

        [IgnoreInRequest]
        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_with_null_model_instance
    {
        static Uri base_url;
        static TestClient client;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

            client = new TestClient(base_url);
        };

        Because of =
            () => url = UrlBuilder.Compose(null, client, base_url);

        It should_not_modify_url =
            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/?keepMe=Yes");

        class TestModel
        {
            [OrderedRequestPath(1)]
            public string ProductCategory { get; set; }

            [OrderedRequestPath(2)]
            public int ProductId { get; set; }

            public string Value { get; set; }
        }

        [IgnoreInRequest]
        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_with_null_client_instance
    {
        static Uri base_url;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

            model = new TestModel
            {
                ProductCategory = "red",
                ProductId = 42,
                Value = "Hello World!"
            };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, null, base_url);

        It should_still_modify_url =
            () => url.AbsoluteUri.ShouldEqual("http://foo.bar/catalog/red/42?keepMe=Yes&Value=Hello+World!");

        class TestModel
        {
            [OrderedRequestPath(1)]
            public string ProductCategory { get; set; }

            [OrderedRequestPath(2)]
            public int ProductId { get; set; }

            public string Value { get; set; }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_for_url_for_local_file
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("file:///c:/root");

            client = new TestClient(base_url);

            model = new TestModel
            {
                SubFolder1 = "red",
                SubFolder2 = 42,
                File = "readme.txt"
            };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, client, base_url);

        It should_create_full_url =
            () => url.AbsoluteUri.ShouldEqual("file:///c:/root/red/42/readme.txt");

        class TestModel
        {
            [OrderedRequestPath(1)]
            public string SubFolder1 { get; set; }

            [OrderedRequestPath(2)]
            public int SubFolder2 { get; set; }

            [OrderedRequestPath(2)]
            public string File { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_for_url_for_unc_file
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("file://file-server/root");

            client = new TestClient(base_url);

            model = new TestModel
            {
                SubFolder1 = "red",
                SubFolder2 = 42,
                File = "read me.txt"
            };
        };

        Because of =
            () => url = UrlBuilder.Compose(model, client, base_url);

        It should_create_full_url =
            () => url.AbsoluteUri.ShouldEqual("file://file-server/root/red/42/read%20me.txt");

        class TestModel
        {
            [OrderedRequestPath(1)]
            public string SubFolder1 { get; set; }

            [OrderedRequestPath(2)]
            public int SubFolder2 { get; set; }

            [OrderedRequestPath(2)]
            public string File { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_with_null_base_url
    {
        static TestClient client;
        static TestModel model;
        static Exception exception;

        Establish context = () =>
        {
            client = new TestClient(new Uri("http://contoso.com/"));

            model = new TestModel
            {
                ProductCategory = "red",
                ProductId = 42,
                Value = "Hello World!"
            };
        };

        Because of =
            () => exception = Catch.Exception(() => UrlBuilder.Compose(model, client, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_base_url_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("baseUrl");

        class TestModel
        {
            [OrderedRequestPath(1)]
            public string ProductCategory { get; set; }

            [OrderedRequestPath(2)]
            public int ProductId { get; set; }

            public string Value { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_and_exepction_is_thrown
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Exception exception;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

            client = new TestClient(base_url);

            model = new TestModel();
        };

        Because of =
            () => exception = Catch.Exception(() => UrlBuilder.Compose(model, client, base_url));

        It should_throw_unrecoverable_http_client_exception =
            () => exception.ShouldBeOfType<UnrecoverableHttpClientException>();

        It should_wrap_original_exception =
            () => exception.InnerException.InnerException.Message.ShouldEqual("You got me!");

        class TestModel
        {
            public string Value { get {throw new Exception("You got me!"); } }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_to_create_ordered_path_where_non_empty_value_follow_empty
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Exception exception;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/catalog/?keepMe=Yes");

            client = new TestClient(base_url);

            model = new TestModel
            {
                Value1 = "",
                Value2 = "red",
                Value3 = null
            };
        };

        Because of =
            () => exception = Catch.Exception(() => UrlBuilder.Compose(model, client, base_url));

        It should_throw_unrecoverable_http_client_exception =
            () => exception.ShouldBeOfType<UnrecoverableHttpClientException>();

        It should_not_wrap_original_exception_as_it_is_already_unrecoverable_http_client_exception =
            () => exception.InnerException.ShouldBeNull();

        class TestModel
        {
            [OrderedRequestPath(1)]
            public string Value1 { get; set; }

            [OrderedRequestPath(2)]
            public string Value2 { get; set; }

            [OrderedRequestPath(3)]
            public string Value3 { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient(Uri baseUrl)
                : base(baseUrl)
            {
            }
        }
    }

    // ReSharper restore UnusedMember.Local
    // ReSharper restore ClassNeverInstantiated.Local
    // ReSharper restore UnusedAutoPropertyAccessor.Local
}
