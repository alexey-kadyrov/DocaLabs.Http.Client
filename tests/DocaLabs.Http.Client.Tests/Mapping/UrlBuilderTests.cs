using System;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.Mapping;
using DocaLabs.Http.Client.Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Mapping
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used
    {
        static TestClass instance;
        static Uri url;

        Establish context = () => instance = new TestClass
        {
            Value = "Hello"
        };

        Because of =
            () => url = UrlBuilder.CreateUrl(new Uri("http://foo.bar/"), instance);

        It should_add_query_to_the_url =
            () => url.ToString().ShouldEqual("http://foo.bar/?Value=Hello");

        class TestClass
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_with_query_instance_marked_as_ignore
    {
        static TestClass instance;
        static Uri url;

        Establish context = () => instance = new TestClass
        {
            Value = "Hello"
        };

        Because of =
            () => url = UrlBuilder.CreateUrl(new Uri("http://foo.bar/"), instance);

        It should_not_add_query_to_the_url =
            () => url.ToString().ShouldEqual("http://foo.bar/");

        [QueryIgnore]
        class TestClass
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(UrlBuilder))]
    class when_url_builder_is_used_on_null_instance
    {
        static Uri url;

        Because of =
            () => url = UrlBuilder.CreateUrl(new Uri("http://foo.bar/"), null);

        It should_not_modify_the_url =
            () => url.ToString().ShouldEqual("http://foo.bar/");
    }

    [Subject(typeof(UrlBuilder))]
    class when_exception_is_thrown_during_creating_url_by_url_builder
    {
        static Exception exception;
        static Exception original_exception;

        Establish context =
            () => original_exception = new Exception();

        Because of =
            () => exception = Catch.Exception(() => UrlBuilder.CreateUrl(new Uri("http://foo.bar/"), new TestClass()));

        It should_rethrow_unrecoverable_http_client_exception =
            () => exception.ShouldBeOfType<UnrecoverableHttpClientException>();

        It should_wrap_the_original_exception =
            () => exception.InnerException.ShouldBeTheSameAs(original_exception);

        class TestClass : ICustomQueryMapper
        {
            public CustomNameValueCollection ToParameterDictionary()
            {
                throw original_exception;
            }
        }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Local
}
