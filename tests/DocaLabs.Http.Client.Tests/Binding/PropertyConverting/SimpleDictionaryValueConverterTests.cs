using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;
using Machine.Specifications.Annotations;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_null_value
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of =
            () => result = converter.Convert(null);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "key27", "27" }, 
           { "key42", "42" }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_dictionary_with_mixed_types_some_of_which_are_not_simple
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "key27", 27 }, 
           { "key42", "42" },
           { 3, new object() },
           { 4, "value4" },
           { new object(), "hello" },
           { new object(), new object() },
           { 6, 66 }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42", "Values.4", "Values.6");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");

        It should_be_able_to_convert_third_value =
            () => result.GetValues("Values.4").ShouldContainOnly("value4");

        It should_be_able_to_convert_forth_value =
            () => result.GetValues("Values.6").ShouldContainOnly("66");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_dictionary_with_mixed_simple_types
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "key27", 27 }, 
           { "key42", "42" },
           { 3, "value3" },
           { 4, 44 }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42", "Values.3", "Values.4");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");

        It should_be_able_to_convert_third_value =
            () => result.GetValues("Values.3").ShouldContainOnly("value3");

        It should_be_able_to_convert_forth_value =
            () => result.GetValues("Values.4").ShouldContainOnly("44");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_generic_dictionary_with_mixed_types_some_of_which_are_not_simple
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Dictionary<object, object>
        {
           { "key27", 27 }, 
           { "key42", "42" },
           { 3, new object() },
           { 4, "value4" },
           { new object(), "hello" },
           { new object(), new object() },
           { 6, 66 }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42", "Values.4", "Values.6");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");

        It should_be_able_to_convert_third_value =
            () => result.GetValues("Values.4").ShouldContainOnly("value4");

        It should_be_able_to_convert_forth_value =
            () => result.GetValues("Values.6").ShouldContainOnly("66");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_generic_fictionary_with_non_simple_keys
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Dictionary<TestClass, int>
        {
           { new TestClass { Value = "1" }, 27 }, 
           { new TestClass { Value = "2" }, 42 }
        });

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public string Value { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_generic_fictionary_with_non_simple_values
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Dictionary<int, TestClass>
        {
           { 11, new TestClass { Value = "1" } }, 
           { 22, new TestClass { Value = "2" } }
        });

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public string Value { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_generic_dictionary
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Dictionary<string, string>
        {
           { "key27", "27" }, 
           { "key42", "42" }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_non_string_generic_dictionary
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Dictionary<int, int>
        {
           { 1, 101 }, 
           { 2, 102 },
           { 3, 103 },
           { 4, 104 }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.1", "Values.2", "Values.3", "Values.4");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.1").ShouldContainOnly("101");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.2").ShouldContainOnly("102");

        It should_be_able_to_convert_third_value =
            () => result.GetValues("Values.3").ShouldContainOnly("103");

        It should_be_able_to_convert_forth_value =
            () => result.GetValues("Values.4").ShouldContainOnly("104");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_with_null_name
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter(null, null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "key27", "27" }, 
           { "key42", "42" }
        });

        It should_be_able_to_get_the_key_using_the_source_key =
            () => result.AllKeys.ShouldContainOnly("key27", "key42");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("key42").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_with_empty_name
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("", null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "key27", "27" }, 
           { "key42", "42" }
        });

        It should_be_able_to_get_the_key_using_the_source_key =
            () => result.AllKeys.ShouldContainOnly("key27", "key42");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("key42").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_collection_value_where_some_values_are_null
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter(null, null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "null1", null }, 
           { "key27", "27" }, 
           { "null2", null }
        });

        It should_be_able_to_get_the_key_using_the_source_key =
            () => result.AllKeys.ShouldContainOnly("null1", "key27", "null2");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("null1").ShouldContainOnly("");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("key27").ShouldContainOnly("27");

        It should_be_able_to_convert_third_value =
            () => result.GetValues("null2").ShouldContainOnly("");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_collection_where_some_keys_are_empty
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter(null, null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "", "27" }, 
           { "key42", "42" }
        });

        It should_be_able_to_get_only_non_empty_keys =
            () => result.AllKeys.ShouldContainOnly("key42");

        It should_be_able_to_convert_value_with_non_empty_key =
            () => result.GetValues("key42").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_with_non_empty_name_and_collection_where_some_keys_are_empty
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "", "27" }, 
           { "key42", "42" }
        });

        It should_be_able_to_get_only_non_empty_keys =
            () => result.AllKeys.ShouldContainOnly("Values.key42");

        It should_be_able_to_convert_value_with_non_empty_key =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");
    }
}
