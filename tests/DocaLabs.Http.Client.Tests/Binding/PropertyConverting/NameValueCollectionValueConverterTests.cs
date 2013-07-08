using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_null_value
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("Values");

        Because of =
            () => result = converter.Convert(null);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("Values");

        Because of = () => result = converter.Convert(new NameValueCollection
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

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_dictionary
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("Values");

        Because of = () => result = converter.Convert(new Dictionary<string, string>
        {
           { "key27", "27" }, 
           { "key42", "42" }
        });

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_list
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("Values");

        Because of = () => result = converter.Convert(new List<string>
        {
           "key27",
           "key42"
        });

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_int
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("Values");

        Because of = 
            () => result = converter.Convert(42);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_collection_with_duplicate_keys
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("Values");

        Because of = () => result = converter.Convert(new NameValueCollection
        {
           { "key27", "27" }, 
           { "key42", "42" },
           { "key27", "2-27" }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42");

        It should_be_able_to_convert_first_values =
            () => result.GetValues("Values.key27").ShouldContainOnly("27", "2-27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_with_null_name
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter(null);

        Because of = () => result = converter.Convert(new NameValueCollection
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

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_collection_with_duplicate_keys_and_name_is_null
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter(null);

        Because of = () => result = converter.Convert(new NameValueCollection
        {
           { "key27", "27" }, 
           { "key42", "42" },
           { "key27", "2-27" }
        });

        It should_be_able_to_get_the_key_using_the_source_key =
            () => result.AllKeys.ShouldContainOnly("key27", "key42");

        It should_be_able_to_convert_first_values =
            () => result.GetValues("key27").ShouldContainOnly("27", "2-27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("key42").ShouldContainOnly("42");
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_with_empty_name
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("");

        Because of = () => result = converter.Convert(new NameValueCollection
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

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_collection_with_duplicate_keys_and_name_is_empty
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("");

        Because of = () => result = converter.Convert(new NameValueCollection
        {
           { "key27", "27" }, 
           { "key42", "42" },
           { "key27", "2-27" }
        });

        It should_be_able_to_get_the_key_using_the_source_key =
            () => result.AllKeys.ShouldContainOnly("key27", "key42");

        It should_be_able_to_convert_first_values =
            () => result.GetValues("key27").ShouldContainOnly("27", "2-27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("key42").ShouldContainOnly("42");
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_collection_value_where_some_values_are_null
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter(null);

        Because of = () => result = converter.Convert(new NameValueCollection
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

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_collection_where_some_keys_are_empty
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter(null);

        Because of = () => result = converter.Convert(new NameValueCollection
        {
           { "", "27" }, 
           { "key42", "42" },
           { "", "2-27" }
        });

        It should_be_able_to_get_only_non_empty_keys =
            () => result.AllKeys.ShouldContainOnly("key42");

        It should_be_able_to_convert_value_with_non_empty_key =
            () => result.GetValues("key42").ShouldContainOnly("42");
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_with_non_empty_name_and_collection_where_some_keys_are_empty
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("Values");

        Because of = () => result = converter.Convert(new NameValueCollection
        {
           { "", "27" }, 
           { "key42", "42" },
           { "", "2-27" }
        });

        It should_be_able_to_get_only_non_empty_keys =
            () => result.AllKeys.ShouldContainOnly("Values.key42");

        It should_be_able_to_convert_value_with_non_empty_key =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_collection_where_some_keys_are_null
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter(null);

        Because of = () => result = converter.Convert(new NameValueCollection
        {
           { null, "27" }, 
           { "key42", "42" },
           { null, "2-27" }
        });

        It should_be_able_to_get_only_non_empty_keys =
            () => result.AllKeys.ShouldContainOnly("key42");

        It should_be_able_to_convert_value_with_non_empty_key =
            () => result.GetValues("key42").ShouldContainOnly("42");
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_with_non_empty_name_and_collection_where_some_keys_are_null
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("Values");

        Because of = () => result = converter.Convert(new NameValueCollection
        {
           { null, "27" }, 
           { "key42", "42" },
           { null, "2-27" }
        });

        It should_be_able_to_get_only_non_empty_keys =
            () => result.AllKeys.ShouldContainOnly("Values.key42");

        It should_be_able_to_convert_value_with_non_empty_key =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_checking_whenever_namevalue_collection_value_converter_can_convert_the_type
    {
        It should_return_false_for_bool =
            () => NameValueCollectionValueConverter.CanConvert(typeof(bool)).ShouldBeFalse();

        It should_return_false_for_byte =
            () => NameValueCollectionValueConverter.CanConvert(typeof(byte)).ShouldBeFalse();

        It should_return_false_for_char =
            () => NameValueCollectionValueConverter.CanConvert(typeof(char)).ShouldBeFalse();

        It should_return_false_for_short =
            () => NameValueCollectionValueConverter.CanConvert(typeof(short)).ShouldBeFalse();

        It should_return_false_for_ushort =
            () => NameValueCollectionValueConverter.CanConvert(typeof(ushort)).ShouldBeFalse();

        It should_return_false_for_int =
            () => NameValueCollectionValueConverter.CanConvert(typeof(int)).ShouldBeFalse();

        It should_return_false_for_uint =
            () => NameValueCollectionValueConverter.CanConvert(typeof(uint)).ShouldBeFalse();

        It should_return_false_for_long =
            () => NameValueCollectionValueConverter.CanConvert(typeof(long)).ShouldBeFalse();

        It should_return_false_for_ulong =
            () => NameValueCollectionValueConverter.CanConvert(typeof(ulong)).ShouldBeFalse();

        It should_return_false_for_float =
            () => NameValueCollectionValueConverter.CanConvert(typeof(float)).ShouldBeFalse();

        It should_return_false_for_double =
            () => NameValueCollectionValueConverter.CanConvert(typeof(double)).ShouldBeFalse();

        It should_return_false_for_decimal =
            () => NameValueCollectionValueConverter.CanConvert(typeof(decimal)).ShouldBeFalse();

        It should_return_false_for_guid =
            () => NameValueCollectionValueConverter.CanConvert(typeof(Guid)).ShouldBeFalse();

        It should_return_false_for_date_time =
            () => NameValueCollectionValueConverter.CanConvert(typeof(DateTime)).ShouldBeFalse();

        It should_return_false_for_date_time_offset =
            () => NameValueCollectionValueConverter.CanConvert(typeof(DateTimeOffset)).ShouldBeFalse();

        It should_return_false_for_time_span =
            () => NameValueCollectionValueConverter.CanConvert(typeof(TimeSpan)).ShouldBeFalse();

        It should_return_false_for_enum =
            () => NameValueCollectionValueConverter.CanConvert(typeof(TestEnum)).ShouldBeFalse();

        It should_return_false_for_string =
            () => NameValueCollectionValueConverter.CanConvert(typeof(string)).ShouldBeFalse();

        It should_return_false_for_byte_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(byte[])).ShouldBeFalse();

        It should_return_false_for_bool_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(bool[])).ShouldBeFalse();

        It should_return_false_for_char_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(char[])).ShouldBeFalse();

        It should_return_false_for_short_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(short[])).ShouldBeFalse();

        It should_return_false_for_ushort_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(ushort[])).ShouldBeFalse();

        It should_return_false_for_int_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(int[])).ShouldBeFalse();

        It should_return_false_for_uint_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(uint[])).ShouldBeFalse();

        It should_return_false_for_long_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(long[])).ShouldBeFalse();

        It should_return_false_for_ulong_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(ulong[])).ShouldBeFalse();

        It should_return_false_for_float_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(float[])).ShouldBeFalse();

        It should_return_false_for_double_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(double[])).ShouldBeFalse();

        It should_return_false_for_decimal_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(decimal[])).ShouldBeFalse();

        It should_return_false_for_guid_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(Guid[])).ShouldBeFalse();

        It should_return_false_for_date_time_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(DateTime[])).ShouldBeFalse();

        It should_return_false_for_date_time_offset_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(DateTimeOffset[])).ShouldBeFalse();

        It should_return_false_for_time_span_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(TimeSpan[])).ShouldBeFalse();

        It should_return_false_for_enum_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(TestEnum[])).ShouldBeFalse();

        It should_return_false_for_string_array =
            () => NameValueCollectionValueConverter.CanConvert(typeof(string[])).ShouldBeFalse();

        It should_return_false_for_struct =
            () => NameValueCollectionValueConverter.CanConvert(typeof(TestStruct)).ShouldBeFalse();

        It should_return_false_for_class =
            () => NameValueCollectionValueConverter.CanConvert(typeof(TestClass)).ShouldBeFalse();

        It should_return_false_for_object =
            () => NameValueCollectionValueConverter.CanConvert(typeof(object)).ShouldBeFalse();

        It should_return_true_for_namevaluecollection =
            () => NameValueCollectionValueConverter.CanConvert(typeof(NameValueCollection)).ShouldBeTrue();

        It should_return_true_for_namevaluecollection_subclass =
            () => NameValueCollectionValueConverter.CanConvert(typeof(NameValueCollectionSubclass)).ShouldBeTrue();

        It should_return_false_for_idictionary =
            () => NameValueCollectionValueConverter.CanConvert(typeof(IDictionary)).ShouldBeFalse();

        It should_return_false_for_generic_idictionary =
            () => NameValueCollectionValueConverter.CanConvert(typeof(IDictionary<int, int>)).ShouldBeFalse();

        It should_return_false_for_hastable =
            () => NameValueCollectionValueConverter.CanConvert(typeof(Hashtable)).ShouldBeFalse();

        It should_return_false_for_sortedlist =
            () => NameValueCollectionValueConverter.CanConvert(typeof(SortedList)).ShouldBeFalse();

        It should_return_false_for_dictionary_subclass =
            () => NameValueCollectionValueConverter.CanConvert(typeof(TestDictionarySubsclass)).ShouldBeFalse();

        It should_return_false_for_genric_dictionary_sub_interface_with_defined_generic_arguments =
            () => NameValueCollectionValueConverter.CanConvert(typeof(ITestGenericDictionarySubinterface)).ShouldBeFalse();

        It should_return_false_for_genric_dictionary_sub_interface =
            () => NameValueCollectionValueConverter.CanConvert(typeof(ITestGenericDictionarySubinterface2<int, int>)).ShouldBeFalse();

        It should_return_false_for_genric_dictionary_subclass_with_defined_generic_arguments =
            () => NameValueCollectionValueConverter.CanConvert(typeof(TestGenericDictionarySubsclass)).ShouldBeFalse();

        It should_return_false_for_genric_dictionary_subclass =
            () => NameValueCollectionValueConverter.CanConvert(typeof(TestGenericDictionarySubsclass2<int, int>)).ShouldBeFalse();

        enum TestEnum
        {
        }

        struct TestStruct
        {
        }

        class TestClass
        {
        }

        class NameValueCollectionSubclass : NameValueCollection
        {
        }

        class TestDictionarySubsclass : Hashtable
        {
        }

        interface ITestGenericDictionarySubinterface : IDictionary<int, int>
        {
        }

        interface ITestGenericDictionarySubinterface2<TKey, TValue> : IDictionary<TKey, TValue>
        {
        }

        class TestGenericDictionarySubsclass : Dictionary<int, int>
        {
        }

        class TestGenericDictionarySubsclass2<TKey, TValue> : Dictionary<TKey, TValue>
        {
        }
    }
}
