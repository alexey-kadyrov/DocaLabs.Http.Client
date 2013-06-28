using System;
using System.Collections.Specialized;
using System.Net;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;
using DocaLabs.Testing.Common;

namespace DocaLabs.Http.Client.Tests.Binding
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    // ReSharper disable UnusedMember.Local

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_one_property_marked_by_request_header_attribute
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            MyHeader = "Hello World!",
            JustValue = "Nothing"
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(model);

        It should_map_properties_marked_as_header =
            () => headers.ShouldContainOnly(new NameValueCollection { { "MyHeader", "Hello World!" } });

        class TestModel
        {
            [Use(RequestUsage.InHeader)]
            public string MyHeader { get; set; }

            public string JustValue { get; set; }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_several_properties_marked_by_request_header_attribute
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            MyHeader = "Hello World!",
            JustValue = "Nothing",
            AnotherMyHeader = "header-x"
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(model);

        It should_map_properties_marked_as_header = () => headers.ShouldContainOnly(new NameValueCollection
        {
            { "MyHeader", "Hello World!" },
            { "AnotherMyHeader", "header-x" }
        });

        class TestModel
        {
            [Use(RequestUsage.InHeader)]
            public string MyHeader { get; set; }

            public string JustValue { get; set; }

            [Use(RequestUsage.InHeader)]
            public string AnotherMyHeader { get; set; }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_one_property_of_web_header_collection_type
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            MyHeaders = new WebHeaderCollection
            {
                { "MyHeader", "Hello World!" },
                { "AnotherMyHeader", "header-x" }
            },
            JustValue = "Nothing"
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(model);

        It should_map_properties_marked_as_header = () => headers.ShouldContainOnly(new NameValueCollection
        {
            { "MyHeader", "Hello World!" },
            { "AnotherMyHeader", "header-x" }
        });

        class TestModel
        {
            public WebHeaderCollection MyHeaders { get; set; }

            public string JustValue { get; set; }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_properties_marked_by_request_header_attribute_plus_some_properties_of_web_header_collection
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            MyHeader = "Hello World!",
            JustValue = "Nothing",
            AnotherMyHeaders = new WebHeaderCollection
            {
                { "MyHeader", "Wow" },
                { "AnotherMyHeader", "header-x" }
            }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(model);

        It should_map_properties_marked_as_header = () => headers.ShouldContainOnly(new NameValueCollection
        {
            { "MyHeader", "Hello World!,Wow" },
            { "AnotherMyHeader", "header-x" }
        });

        class TestModel
        {
            [Use(RequestUsage.InHeader)]
            public string MyHeader { get; set; }

            public string JustValue { get; set; }

            public WebHeaderCollection AnotherMyHeaders { get; set; }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_a_property_marked_by_request_header_attribute_which_defines_header_name_expcitly
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            MyHeader = "Hello World!",
            JustValue = "Nothing"
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(model);

        It should_map_properties_marked_as_header_using_explicit_header_name =
            () => headers.ShouldContainOnly(new NameValueCollection { { "xx-header-xx", "Hello World!" } });

        class TestModel
        {
            [Use(RequestUsage.InHeader, Name = "xx-header-xx")]
            public string MyHeader { get; set; }

            public string JustValue { get; set; }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_a_property_marked_by_request_header_attribute_which_defines_header_name_and_format_expcitly
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            MyHeader = 2,
            JustValue = "Nothing"
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(model);

        It should_map_properties_marked_as_header_using_explicit_header_name_and_format =
            () => headers.ShouldContainOnly(new NameValueCollection { { "xx-header-xx", "0002" } });

        class TestModel
        {
            [Use(RequestUsage.InHeader, Name = "xx-header-xx", Format = "0000")]
            public int MyHeader { get; set; }

            public string JustValue { get; set; }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_a_non_simple_property_marked_by_request_header_attribute
    {
        static TestModel model;
        static Exception exception;

        Establish context = () => model = new TestModel
        {
            MyHeader = new InnerTestClass { Value = "Hello World!" },
            JustValue = "Nothing"
        };

        Because of =
            () => exception = Catch.Exception(() => new DefaultHeaderMapper().Map(model));

        It should_thow_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        class TestModel
        {
            [Use(RequestUsage.InHeader)]
            public InnerTestClass MyHeader { get; set; }

            public string JustValue { get; set; }
        }

        class InnerTestClass
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_does_not_have_any_property_marked_by_request_header_attribute_or_of_type_web_header_request
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            Value1 = "Hello World!",
            Value2 = "Nothing"
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(model);

        It should_not_map_any_properties =
            () => headers.ShouldBeEmpty();

        class TestModel
        {
            public string Value1 { get; set; }

            public string Value2 { get; set; }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_null_model
    {
        static WebHeaderCollection headers;

        Because of =
            () => headers = new DefaultHeaderMapper().Map(null);

        It should_not_map_any_properties =
            () => headers.ShouldBeEmpty();
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_of_simple_type
    {
        static WebHeaderCollection headers;

        Because of =
            () => headers = new DefaultHeaderMapper().Map(42);

        It should_not_map_any_properties =
            () => headers.ShouldBeEmpty();
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_some_sitable_properties_as_null
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            MyHeader2 = "Hello World!",
            JustValue = "Nothing",
            AnotherMyHeaders2 = new WebHeaderCollection
            {
                { "xx-xx", "header-x" }
            }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(model);

        It should_map_properties_marked_as_header = () => headers.ShouldContainOnly(new NameValueCollection
        {
            { "MyHeader2", "Hello World!" },
            { "xx-xx", "header-x" }
        });

        class TestModel
        {
            [Use(RequestUsage.InHeader)]
            public string MyHeader { get; set; }

            [Use(RequestUsage.InHeader)]
            public string MyHeader2 { get; set; }

            public string JustValue { get; set; }

            public WebHeaderCollection AnotherMyHeaders { get; set; }

            public WebHeaderCollection AnotherMyHeaders2 { get; set; }
        }
    }

    // ReSharper restore UnusedMember.Local
    // ReSharper restore UnusedAutoPropertyAccessor.Local
}
