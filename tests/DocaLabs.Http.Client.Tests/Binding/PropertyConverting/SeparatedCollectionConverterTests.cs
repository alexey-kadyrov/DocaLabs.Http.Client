﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    // ReSharper disable UnusedMember.Local
    // ReSharper disable ValueParameterNotUsed
    // ReSharper disable UnusedParameter.Local
    // ReSharper disable UnusedAutoPropertyAccessor.Local

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_created
    {
        static IConverter converter;

        Because of =
            () => converter = SeparatedCollectionConverter.TryCreate(typeof(TestClass).GetProperty("Countries"));

        It should_be_of_separated_collection_converter_type =
            () => converter.ShouldBeOfType<SeparatedCollectionConverter>();

        It should_return_pipe_as_item_separator =
            () => ((SeparatedCollectionConverter)converter).Separator.ShouldEqual('|');

        class TestClass
        {
            [SeparatedCollectionConverter(Separator = ';')]
            public IEnumerable<string> Countries { get; set; }
        }
    }

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_created_with_null_property_info
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => SeparatedCollectionConverter.TryCreate(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_info_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("property");
    }

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_created_for_indexer
    {
        static IConverter converter;

        Because of =
            () => converter = SeparatedCollectionConverter.TryCreate(typeof(TestClass).GetProperties()[0]);

        It should_return_null =
            () => converter.ShouldBeNull();

        class TestClass
        {
            public IEnumerable<string> this[int index]
            {
                get { return null; }
                set { }
            }
        }
    }

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_created_for_string
    {
        static IConverter converter;

        Because of =
            () => converter = SeparatedCollectionConverter.TryCreate(typeof(TestClass).GetProperty("Value"));

        It should_return_null =
            () => converter.ShouldBeNull();

        class TestClass
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_created_for_byte_array
    {
        static IConverter converter;

        Because of =
            () => converter = SeparatedCollectionConverter.TryCreate(typeof(TestClass).GetProperty("Value"));

        It should_return_null =
            () => converter.ShouldBeNull();

        class TestClass
        {
            public byte[] Value { get; set; }
        }
    }

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_created_for_non_collection_type
    {
        static IConverter converter;

        Because of =
            () => converter = SeparatedCollectionConverter.TryCreate(typeof(TestClass).GetProperty("Value"));

        It should_return_null =
            () => converter.ShouldBeNull();

        class TestClass
        {
            public decimal Value { get; set; }
        }
    }

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_used
    {
        static PropertyInfo property_info;
        static TestClass instance;
        static IConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            property_info = typeof(TestClass).GetProperty("Values");
            instance = new TestClass
            {
                Values = new[] { 27, 42 }
            };

            converter = SeparatedCollectionConverter.TryCreate(property_info, ',');
        };

        Because of =
            () => result = converter.Convert(instance);

        It should_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Values").ShouldContainOnly("27,42");

        class TestClass
        {
            public IEnumerable<int> Values { get; set; }
        }
    }

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_used_on_null_instance
    {
        static PropertyInfo property_info;
        static IConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            property_info = typeof(TestClass).GetProperty("Values");
            converter = SeparatedCollectionConverter.TryCreate(property_info);
        };

        Because of =
            () => result = converter.Convert(null);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public IEnumerable<int> Values { get; set; }
        }
    }

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_used_on_null_property
    {
        static PropertyInfo property_info;
        static TestClass instance;
        static IConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            property_info = typeof(TestClass).GetProperty("Values");
            instance = new TestClass();
            converter = SeparatedCollectionConverter.TryCreate(property_info);
        };

        Because of =
            () => result = converter.Convert(instance);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public IEnumerable<int> Values { get; set; }
        }
    }

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_used_on_collection_property_where_some_values_are_null
    {
        static PropertyInfo property_info;
        static TestClass instance;
        static IConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            property_info = typeof(TestClass).GetProperty("Values");

            instance = new TestClass
            {
                Values = new[] { null, "Hello", null }
            };

            converter = SeparatedCollectionConverter.TryCreate(property_info, ',');
        };

        Because of =
            () => result = converter.Convert(instance);

        It should_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Values").ShouldContainOnly(",Hello,");

        class TestClass
        {
            public IEnumerable<string> Values { get; set; }
        }
    }

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_used_together_with_request_use_attribute_where_name_and_format_are_not_set
    {
        static PropertyInfo property_info;
        static TestClass instance;
        static IConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            property_info = typeof(TestClass).GetProperty("Values");
            instance = new TestClass
            {
                Values = new[] { 27, 42 }
            };

            converter = SeparatedCollectionConverter.TryCreate(property_info, ',');
        };

        Because of =
            () => result = converter.Convert(instance);

        It should_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Values").ShouldContainOnly("27,42");

        class TestClass
        {
            [RequestUse]
            public IEnumerable<int> Values { get; set; }
        }
    }

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_used_on_property_which_name_is_redefined_using_request_use_attribute
    {
        static PropertyInfo property_info;
        static TestClass instance;
        static IConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            property_info = typeof(TestClass).GetProperty("Values");
            instance = new TestClass
            {
                Values = new[] { 27, 42 }
            };

            converter = SeparatedCollectionConverter.TryCreate(property_info);
        };

        Because of =
            () => result = converter.Convert(instance);

        It should_be_able_to_get_the_key_as_the_redefined_name =
            () => result.AllKeys.ShouldContainOnly("Hello World");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Hello World").ShouldContainOnly("27|42");

        class TestClass
        {
            [RequestUse(Name = "Hello World")]
            public IEnumerable<int> Values { get; set; }
        }
    }

    [Subject(typeof(SeparatedCollectionConverter))]
    class when_separated_collection_converter_is_used_on_property_with_custom_format_applied
    {
        static PropertyInfo property_info;
        static TestClass instance;
        static IConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            property_info = typeof(TestClass).GetProperty("Values");
            instance = new TestClass
            {
                Values = new[] { 27, 42 }
            };

            converter = SeparatedCollectionConverter.TryCreate(property_info, ',');
        };

        Because of =
            () => result = converter.Convert(instance);

        It should_be_able_to_get_the_key_as_the_redefined_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Values").ShouldContainOnly("1B,2A");

        class TestClass
        {
            [RequestUse(Format = "{0:X}")]
            public IEnumerable<int> Values { get; set; }
        }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Local
    // ReSharper restore UnusedParameter.Local
    // ReSharper restore ValueParameterNotUsed
    // ReSharper restore UnusedMember.Local
}
