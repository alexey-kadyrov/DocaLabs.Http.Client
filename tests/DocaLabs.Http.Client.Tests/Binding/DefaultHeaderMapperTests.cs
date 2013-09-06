using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Binding.Serialization;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    // ReSharper disable UnusedMember.Local

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_one_property_marked_by_as_request_header
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            MyHeader = "Hello World!",
            JustValue = "Nothing"
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_map_properties_marked_as_header =
            () => headers.AllKeys.ShouldContainOnly("MyHeader");

        It should_convert_values_for_header =
            () => headers.GetValues("MyHeader").ShouldContainOnly("Hello World!");

        class TestModel
        {
            [RequestUse(RequestUseTargets.RequestHeader)]
            public string MyHeader { get; set; }

            public string JustValue { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_several_properties_marked_as_request_header
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
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_map_properties_marked_as_header =
            () => headers.AllKeys.ShouldContainOnly("MyHeader", "AnotherMyHeader");

        It should_convert_values_for_first_header =
            () => headers.GetValues("MyHeader").ShouldContainOnly("Hello World!");

        It should_convert_values_for_second_header =
            () => headers.GetValues("AnotherMyHeader").ShouldContainOnly("header-x");

        class TestModel
        {
            [RequestUse(RequestUseTargets.RequestHeader)]
            public string MyHeader { get; set; }

            public string JustValue { get; set; }

            [RequestUse(RequestUseTargets.RequestHeader)]
            public string AnotherMyHeader { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_and_client_with_request_serialization_attribute
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            MyHeader = "Hello World!",
            JustValue = "Nothing",
            AnotherMyHeader = "header-x",
            Headers = new WebHeaderCollection
            {
                {"header-11", "value-11"}
            }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_map_only_properties_explicitly_marked_as_header =
            () => headers.AllKeys.ShouldContainOnly("MyHeader", "AnotherMyHeader");

        It should_convert_values_for_first_header =
            () => headers.GetValues("MyHeader").ShouldContainOnly("Hello World!");

        It should_convert_values_for_second_header =
            () => headers.GetValues("AnotherMyHeader").ShouldContainOnly("header-x");

        class TestModel
        {
            [RequestUse(RequestUseTargets.RequestHeader)]
            public string MyHeader { get; set; }

            public string JustValue { get; set; }

            [RequestUse(RequestUseTargets.RequestHeader)]
            public string AnotherMyHeader { get; set; }

            public WebHeaderCollection Headers { get; set; }
        }

        [TestSerializer]
        class TestClient : HttpClient<TestModel, string>
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

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_with_request_serialization_attribute
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            MyHeader = "Hello World!",
            JustValue = "Nothing",
            AnotherMyHeader = "header-x",
            Headers = new WebHeaderCollection
            {
                {"header-11", "value-11"}
            }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_map_only_properties_explicitly_marked_as_header =
            () => headers.AllKeys.ShouldContainOnly("MyHeader", "AnotherMyHeader");

        It should_convert_values_for_first_header =
            () => headers.GetValues("MyHeader").ShouldContainOnly("Hello World!");

        It should_convert_values_for_second_header =
            () => headers.GetValues("AnotherMyHeader").ShouldContainOnly("header-x");

        [TestSerializer]
        class TestModel
        {
            [RequestUse(RequestUseTargets.RequestHeader)]
            public string MyHeader { get; set; }

            public string JustValue { get; set; }

            [RequestUse(RequestUseTargets.RequestHeader)]
            public string AnotherMyHeader { get; set; }

            public WebHeaderCollection Headers { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
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

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_namevaluecollection
    {
        static NameValueCollection model;
        static WebHeaderCollection headers;

        Establish context = () => model = new NameValueCollection
        {
            { "MyHeader", "Hello World!" },
            { "AnotherMyHeader", "header-x" }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_not_map =
            () => headers.ShouldBeEmpty();

        class TestClient : HttpClient<NameValueCollection, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_hashtable
    {
        static Hashtable model;
        static WebHeaderCollection headers;

        Establish context = () => model = new Hashtable
        {
            { "MyHeader", "Hello World!" },
            { "AnotherMyHeader", "header-x" }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_not_map =
            () => headers.ShouldBeEmpty();

        class TestClient : HttpClient<Hashtable, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_hashtable_and_client_with_request_serialization_attribute
    {
        static Hashtable model;
        static WebHeaderCollection headers;

        Establish context = () => model = new Hashtable
        {
            { "MyHeader", "Hello World!" },
            { "AnotherMyHeader", "header-x" }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_not_map =
            () => headers.ShouldBeEmpty();

        [TestSerializer]
        class TestClient : HttpClient<Hashtable, string>
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

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_subclass_of_hashtable_with_request_serialization_attribute
    {
        static TestHashTable model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestHashTable
        {
            { "MyHeader", "Hello World!" },
            { "AnotherMyHeader", "header-x" }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_not_map =
            () => headers.ShouldBeEmpty();

        class TestClient : HttpClient<TestHashTable, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestSerializer]
        class TestHashTable : Hashtable
        {
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_namevaluecollection_and_client_with_request_serialization_attribute
    {
        static NameValueCollection model;
        static WebHeaderCollection headers;

        Establish context = () => model = new NameValueCollection
        {
            { "MyHeader", "Hello World!" },
            { "AnotherMyHeader", "header-x" }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_not_map =
            () => headers.ShouldBeEmpty();

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

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_subclass_of_namevaluecollection_with_request_serialization_attribute
    {
        static TestNameValueCollection model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestNameValueCollection
        {
            { "MyHeader", "Hello World!" },
            { "AnotherMyHeader", "header-x" }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_not_map =
            () => headers.ShouldBeEmpty();

        class TestClient : HttpClient<TestNameValueCollection, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestSerializer]
        class TestNameValueCollection : NameValueCollection
        {
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_generic_dictionary_and_client_with_request_serialization_attribute
    {
        static Dictionary<string, string> model;
        static WebHeaderCollection headers;

        Establish context = () => model = new Dictionary<string, string>
        {
            { "MyHeader", "Hello World!" },
            { "AnotherMyHeader", "header-x" }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_not_map =
            () => headers.ShouldBeEmpty();

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

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_subclass_of_generic_dictionary_with_request_serialization_attribute
    {
        static TestDictionary model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestDictionary
        {
            { "MyHeader", "Hello World!" },
            { "AnotherMyHeader", "header-x" }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_not_map =
            () => headers.ShouldBeEmpty();

        class TestClient : HttpClient<TestDictionary, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }

        [TestSerializer]
        class TestDictionary : Dictionary<string, string>
        {
        }

        class TestSerializerAttribute : RequestSerializationAttribute
        {
            public override void Serialize(object obj, WebRequest request)
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_generic_dictionary
    {
        static Dictionary<string, string> model;
        static WebHeaderCollection headers;

        Establish context = () => model = new Dictionary<string, string>
        {
            { "MyHeader", "Hello World!" },
            { "AnotherMyHeader", "header-x" }
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_not_map =
            () => headers.ShouldBeEmpty();

        class TestClient : HttpClient<Dictionary<string, string>, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
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
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_map_all_keys_of_that_property =
            () => headers.AllKeys.ShouldContainOnly("MyHeaders.MyHeader", "MyHeaders.AnotherMyHeader");

        It should_convert_values_for_first_header =
            () => headers.GetValues("MyHeaders.MyHeader").ShouldContainOnly("Hello World!");

        It should_convert_values_for_second_header =
            () => headers.GetValues("MyHeaders.AnotherMyHeader").ShouldContainOnly("header-x");

        class TestModel
        {
            public WebHeaderCollection MyHeaders { get; set; }
            public string JustValue { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_one_property_of_web_header_collection_type_with_empty_overridden_name
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
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_map_all_keys_of_that_property =
            () => headers.AllKeys.ShouldContainOnly("MyHeader", "AnotherMyHeader");

        It should_convert_values_for_first_header =
            () => headers.GetValues("MyHeader").ShouldContainOnly("Hello World!");

        It should_convert_values_for_second_header =
            () => headers.GetValues("AnotherMyHeader").ShouldContainOnly("header-x");

        class TestModel
        {
            [PropertyOverrides(Name = "")]
            public WebHeaderCollection MyHeaders { get; set; }
            public string JustValue { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_properties_marked_as_request_header_some_properties_of_web_header_collection
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
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);


        It should_map_all_keys_of_that_property =
            () => headers.AllKeys.ShouldContainOnly("MyHeader", "AnotherMyHeader");

        It should_convert_values_for_first_header =
            () => headers.GetValues("MyHeader").ShouldContainOnly("Hello World!", "Wow");

        It should_convert_values_for_second_header =
            () => headers.GetValues("AnotherMyHeader").ShouldContainOnly("header-x");

        class TestModel
        {
            [RequestUse(RequestUseTargets.RequestHeader)]
            public string MyHeader { get; set; }

            public string JustValue { get; set; }

            [PropertyOverrides(Name = "")]
            public WebHeaderCollection AnotherMyHeaders { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_a_property_marked_as_request_header_which_defines_header_name_expcitly
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            MyHeader = "Hello World!",
            JustValue = "Nothing"
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_map_properties_marked_as_header_using_explicit_header_name =
            () => headers.AllKeys.ShouldContainOnly("xx-header-xx");

        It should_convert_values_for_header =
            () => headers.GetValues("xx-header-xx").ShouldContainOnly("Hello World!");

        class TestModel
        {
            [RequestUse(RequestUseTargets.RequestHeader, Name = "xx-header-xx")]
            public string MyHeader { get; set; }

            public string JustValue { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_a_property_marked_as_request_header_which_defines_header_name_and_format_expcitly
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            MyHeader = 2,
            JustValue = "Nothing"
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_map_properties_marked_as_header_using_explicit_header_name =
            () => headers.AllKeys.ShouldContainOnly("xx-header-xx");

        It should_convert_values_for_header_using_specified_format =
            () => headers.GetValues("xx-header-xx").ShouldContainOnly("0002");

        class TestModel
        {
            [RequestUse(RequestUseTargets.RequestHeader, Name = "xx-header-xx", Format = "{0:0000}")]
            public int MyHeader { get; set; }
            public string JustValue { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_does_not_have_any_property_marked_as_request_header_or_of_type_web_header_request
    {
        static TestModel model;
        static WebHeaderCollection headers;

        Establish context = () => model = new TestModel
        {
            Value1 = "Hello World!",
            Value2 = "Nothing"
        };

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_not_map_any_properties =
            () => headers.ShouldBeEmpty();

        class TestModel
        {
            public string Value1 { get; set; }
            public string Value2 { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_null_model
    {
        static WebHeaderCollection headers;

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), null);

        It should_not_map_any_properties =
            () => headers.ShouldBeEmpty();

        class TestClient : HttpClient<string, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_null_client
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => new DefaultHeaderMapper().Map(null, new Model()));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_client_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("client");

        class Model
        {
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_of_simple_type
    {
        static WebHeaderCollection headers;

        Because of =
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), 42);

        It should_not_map_any_properties =
            () => headers.ShouldBeEmpty();

        class TestClient : HttpClient<int, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(DefaultHeaderMapper))]
    class when_default_header_mapper_used_for_model_that_has_some_suitable_properties_as_null
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
            () => headers = new DefaultHeaderMapper().Map(new TestClient(), model);

        It should_map_properties_marked_as_header =
            () => headers.AllKeys.ShouldContainOnly("MyHeader2", "AnotherMyHeaders2.xx-xx");

        It should_convert_values_for_first_header =
            () => headers.GetValues("MyHeader2").ShouldContainOnly("Hello World!");

        It should_convert_values_for_second_header =
            () => headers.GetValues("AnotherMyHeaders2.xx-xx").ShouldContainOnly("header-x");

        class TestModel
        {
            [RequestUse(RequestUseTargets.RequestHeader)]
            public string MyHeader { get; set; }

            [RequestUse(RequestUseTargets.RequestHeader)]
            public string MyHeader2 { get; set; }

            public string JustValue { get; set; }

            public WebHeaderCollection AnotherMyHeaders { get; set; }

            public WebHeaderCollection AnotherMyHeaders2 { get; set; }
        }

        class TestClient : HttpClient<TestModel, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    // ReSharper restore UnusedMember.Local
    // ReSharper restore UnusedAutoPropertyAccessor.Local
}
