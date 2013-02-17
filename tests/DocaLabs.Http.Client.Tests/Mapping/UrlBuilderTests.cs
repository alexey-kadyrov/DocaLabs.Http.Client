using System;
using DocaLabs.Http.Client.Mapping;
using DocaLabs.Http.Client.Mapping.Attributes;
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

        class TestClass
        {
            public string Value { get; set; }
        }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Local
}
