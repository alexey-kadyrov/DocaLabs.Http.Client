using System;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.UrlMapping;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.UrlMapping
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_for_url_without_query_and_type_without_serialization_hints
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
            () => url = UrlBuilder.CreateUrl(model, client, base_url);

        It should_not_modify_authority_and_path_and_should_add_query_part =
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
    class when_url_builder_is_used_for_url_with_some_query_query_part_and_type_without_serialization_hints
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
            () => url = UrlBuilder.CreateUrl(model, client, base_url);

        It should_not_modify_authority_and_path_and_should_keep_existing_query_part_at_left_and_add_new_query_part =
            () => url.ToString().ShouldEqual("http://foo.bar/product/red?keepMe=Yes&Value=Hello+World!");

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
    class when_url_builder_is_used_for_url_with_for_type_with_serialization_hints_for_path_and_query
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
            () => url = UrlBuilder.CreateUrl(model, client, base_url);

        It should_not_modify_authority_and_path_and_should_keep_existing_query_part_at_left_and_add_new_query_part =
            () => url.ToString().ShouldEqual("http://foo.bar/catalog/red/42?keepMe=Yes&Value=Hello+World!");

        class TestModel
        {
            [QueryPath(1)]
            public string ProductCategory { get; set; }

            [QueryPath(2)]
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
    class when_url_builder_is_used_for_url_with_user_and_password_and_for_type_without_serialization_hints
    {
        static Uri base_url;
        static TestClient client;
        static TestModel model;
        static Uri url;

        Establish context = () =>
        {
            base_url = new Uri("http://user1:password1@foo.bar/product/red/");

            client = new TestClient(base_url);

            model = new TestModel { Value = "Hello World!" };
        };

        Because of =
            () => url = UrlBuilder.CreateUrl(model, client, base_url);

        It should_not_modify_user_password_authority_and_path_and_should_add_query_part =
            () => url.ToString().ShouldEqual("http://user1:password1@foo.bar/product/red/?Value=Hello+World!");

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

    //[Subject(typeof(UrlBuilder))]
    //class when_url_builder_is_used_with_query_instance_marked_as_ignore
    //{
    //    static TestClass instance;
    //    static Uri url;

    //    Establish context = () => instance = new TestClass
    //    {
    //        Value = "Hello"
    //    };

    //    Because of =
    //        () => url = UrlBuilder.CreateUrl(new Uri("http://foo.bar/"), instance);

    //    It should_not_add_query_to_the_url =
    //        () => url.ToString().ShouldEqual("http://foo.bar/");

    //    [QueryIgnore]
    //    class TestClass
    //    {
    //        public string Value { get; set; }
    //    }
    //}

    //[Subject(typeof(UrlBuilder))]
    //class when_url_builder_is_used_on_null_instance
    //{
    //    static Uri url;

    //    Because of =
    //        () => url = UrlBuilder.CreateUrl(new Uri("http://foo.bar/"), null);

    //    It should_not_modify_the_url =
    //        () => url.ToString().ShouldEqual("http://foo.bar/");
    //}

    //[Subject(typeof(UrlBuilder))]
    //class when_exception_is_thrown_during_creating_url_by_url_builder
    //{
    //    static Exception exception;
    //    static Exception original_exception;

    //    Establish context =
    //        () => original_exception = new Exception();

    //    Because of =
    //        () => exception = Catch.Exception(() => UrlBuilder.CreateUrl(new Uri("http://foo.bar/"), new TestClass()));

    //    It should_rethrow_unrecoverable_http_client_exception =
    //        () => exception.ShouldBeOfType<UnrecoverableHttpClientException>();

    //    It should_wrap_the_original_exception =
    //        () => exception.InnerException.ShouldBeTheSameAs(original_exception);

    //    class TestClass : ICustomQueryMapper
    //    {
    //        public CustomNameValueCollection ToParameterDictionary()
    //        {
    //            throw original_exception;
    //        }
    //    }
    //}

    // ReSharper restore UnusedAutoPropertyAccessor.Local
}
