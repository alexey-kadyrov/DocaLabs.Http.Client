using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_on_null_value
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleCollectionValueConverter("Values", null);

        Because of =
            () => result = converter.Convert(null);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_on_int
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleCollectionValueConverter("Values", null);

        Because of =
            () => result = converter.Convert(42);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_on_dictionary
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleCollectionValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Dictionary<string, string>
        {
           { "key27", "27" }, 
           { "key42", "42" }
        });

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_on_namevaluecollection
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleCollectionValueConverter("Values", null);

        Because of = () => result = converter.Convert(new NameValueCollection
        {
           { "key27", "27" }, 
           { "key42", "42" }
        });


        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_get_all_source_keys_as_values =
            () => result.GetValues("Values").ShouldContainOnly("key27", "key42");
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_with_null_format
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context = 
            () => converter = new SimpleCollectionValueConverter("Values", null);

        Because of =
            () => result = converter.Convert(new[] { 27, 42 });

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_convert_value =
            () => result.GetValues("Values").ShouldContainOnly("27", "42");
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_with_empty_format
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleCollectionValueConverter("Values", "");

        Because of =
            () => result = converter.Convert(new[] { 27, 42 });

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_convert_value =
            () => result.GetValues("Values").ShouldContainOnly("27", "42");
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_with_specified_format
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleCollectionValueConverter("Values", "{0:X}");

        Because of =
            () => result = converter.Convert(new[] { 27, 42 });

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_convert_value =
            () => result.GetValues("Values").ShouldContainOnly("1B", "2A");
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_with_null_name
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => new SimpleCollectionValueConverter(null, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_name_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("name");
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_with_empty_name
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => new SimpleCollectionValueConverter("", null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_name_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("name");
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_on_collection_value_where_some_values_are_null
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context = 
            () => converter = new SimpleCollectionValueConverter("Values", null);

        Because of =
            () => result = converter.Convert(new[] { null, "Hello", null });

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_convert_value =
            () => result.GetValues("Values").ShouldContainOnly("", "Hello", "");
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_checking_whenever_simple_collection_value_converter_can_convert_the_type
    {
        It should_return_false_for_bool =
            () => SimpleCollectionValueConverter.CanConvert(typeof(bool)).ShouldBeFalse();

        It should_return_false_for_byte =
            () => SimpleCollectionValueConverter.CanConvert(typeof(byte)).ShouldBeFalse();

        It should_return_false_for_char =
            () => SimpleCollectionValueConverter.CanConvert(typeof(char)).ShouldBeFalse();

        It should_return_false_for_short =
            () => SimpleCollectionValueConverter.CanConvert(typeof(short)).ShouldBeFalse();

        It should_return_false_for_ushort =
            () => SimpleCollectionValueConverter.CanConvert(typeof(ushort)).ShouldBeFalse();

        It should_return_false_for_int =
            () => SimpleCollectionValueConverter.CanConvert(typeof(int)).ShouldBeFalse();

        It should_return_false_for_uint =
            () => SimpleCollectionValueConverter.CanConvert(typeof(uint)).ShouldBeFalse();

        It should_return_false_for_long =
            () => SimpleCollectionValueConverter.CanConvert(typeof(long)).ShouldBeFalse();

        It should_return_false_for_ulong =
            () => SimpleCollectionValueConverter.CanConvert(typeof(ulong)).ShouldBeFalse();

        It should_return_false_for_float =
            () => SimpleCollectionValueConverter.CanConvert(typeof(float)).ShouldBeFalse();

        It should_return_false_for_double =
            () => SimpleCollectionValueConverter.CanConvert(typeof(double)).ShouldBeFalse();

        It should_return_false_for_decimal =
            () => SimpleCollectionValueConverter.CanConvert(typeof(decimal)).ShouldBeFalse();

        It should_return_false_for_guid =
            () => SimpleCollectionValueConverter.CanConvert(typeof(Guid)).ShouldBeFalse();

        It should_return_false_for_date_time =
            () => SimpleCollectionValueConverter.CanConvert(typeof(DateTime)).ShouldBeFalse();

        It should_return_false_for_date_time_offset =
            () => SimpleCollectionValueConverter.CanConvert(typeof(DateTimeOffset)).ShouldBeFalse();

        It should_return_false_for_time_span =
            () => SimpleCollectionValueConverter.CanConvert(typeof(TimeSpan)).ShouldBeFalse();

        It should_return_false_for_enum =
            () => SimpleCollectionValueConverter.CanConvert(typeof(TestEnum)).ShouldBeFalse();

        It should_return_false_for_string =
            () => SimpleCollectionValueConverter.CanConvert(typeof(string)).ShouldBeFalse();

        It should_return_false_for_byte_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(byte[])).ShouldBeFalse();

        It should_return_false_for_class_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(TestClass[])).ShouldBeFalse();

        It should_return_false_for_class_enumarable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(TestClass[])).ShouldBeFalse();

        It should_return_true_for_enumarable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable)).ShouldBeTrue();

        It should_return_true_for_enumarable_objects =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<object>)).ShouldBeTrue();

        It should_return_true_for_bool_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(bool[])).ShouldBeTrue();

        It should_return_true_for_char_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(char[])).ShouldBeTrue();

        It should_return_true_for_short_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(short[])).ShouldBeTrue();

        It should_return_true_for_ushort_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(ushort[])).ShouldBeTrue();

        It should_return_true_for_int_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(int[])).ShouldBeTrue();

        It should_return_true_for_uint_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(uint[])).ShouldBeTrue();

        It should_return_true_for_long_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(long[])).ShouldBeTrue();

        It should_return_true_for_ulong_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(ulong[])).ShouldBeTrue();

        It should_return_true_for_float_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(float[])).ShouldBeTrue();

        It should_return_true_for_double_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(double[])).ShouldBeTrue();

        It should_return_true_for_decimal_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(decimal[])).ShouldBeTrue();

        It should_return_true_for_guid_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(Guid[])).ShouldBeTrue();

        It should_return_true_for_date_time_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(DateTime[])).ShouldBeTrue();

        It should_return_true_for_date_time_offset_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(DateTimeOffset[])).ShouldBeTrue();

        It should_return_true_for_time_span_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(TimeSpan[])).ShouldBeTrue();

        It should_return_true_for_enum_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(TestEnum[])).ShouldBeTrue();

        It should_return_true_for_string_array =
            () => SimpleCollectionValueConverter.CanConvert(typeof(string[])).ShouldBeTrue();

        It should_return_true_for_bool_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<bool>)).ShouldBeTrue();

        It should_return_true_for_char_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<char>)).ShouldBeTrue();

        It should_return_true_for_short_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<short>)).ShouldBeTrue();

        It should_return_true_for_ushort_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<ushort>)).ShouldBeTrue();

        It should_return_true_for_int_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<int>)).ShouldBeTrue();

        It should_return_true_for_uint_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<uint>)).ShouldBeTrue();

        It should_return_true_for_long_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<long>)).ShouldBeTrue();

        It should_return_true_for_ulong_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<ulong>)).ShouldBeTrue();

        It should_return_true_for_float_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<float>)).ShouldBeTrue();

        It should_return_true_for_double_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<double>)).ShouldBeTrue();

        It should_return_true_for_decimal_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<decimal>)).ShouldBeTrue();

        It should_return_true_for_guid_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<Guid>)).ShouldBeTrue();

        It should_return_true_for_date_time_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<DateTime>)).ShouldBeTrue();

        It should_return_true_for_date_time_offset_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<DateTimeOffset>)).ShouldBeTrue();

        It should_return_true_for_time_span_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<TimeSpan>)).ShouldBeTrue();

        It should_return_true_for_enum_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<TestEnum>)).ShouldBeTrue();

        It should_return_true_for_string_enumerable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IEnumerable<string>)).ShouldBeTrue();

        It should_return_false_for_struct =
            () => SimpleCollectionValueConverter.CanConvert(typeof(TestStruct)).ShouldBeFalse();

        It should_return_false_for_class =
            () => SimpleCollectionValueConverter.CanConvert(typeof(TestClass)).ShouldBeFalse();

        It should_return_false_for_object =
            () => SimpleCollectionValueConverter.CanConvert(typeof(object)).ShouldBeFalse();

        It should_return_true_for_namevaluecollection =
            () => SimpleCollectionValueConverter.CanConvert(typeof(NameValueCollection)).ShouldBeTrue();

        It should_return_true_for_idictionary =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IDictionary)).ShouldBeTrue();

        It should_return_false_for_generic_idictionary =
            () => SimpleCollectionValueConverter.CanConvert(typeof(IDictionary<int, int>)).ShouldBeFalse();

        It should_return_true_for_hastable =
            () => SimpleCollectionValueConverter.CanConvert(typeof(Hashtable)).ShouldBeTrue();

        It should_return_true_for_sortedlist =
            () => SimpleCollectionValueConverter.CanConvert(typeof(SortedList)).ShouldBeTrue();

        It should_return_true_for_dictionary_subclass =
            () => SimpleCollectionValueConverter.CanConvert(typeof(TestDictionarySubsclass)).ShouldBeTrue();

        It should_return_false_for_genric_dictionary_sub_interface_with_defined_generic_arguments =
            () => SimpleCollectionValueConverter.CanConvert(typeof(ITestGenericDictionarySubinterface)).ShouldBeFalse();

        It should_return_false_for_genric_dictionary_sub_interface =
            () => SimpleCollectionValueConverter.CanConvert(typeof(ITestGenericDictionarySubinterface2<int, int>)).ShouldBeFalse();

        It should_return_false_for_genric_dictionary_subclass_with_defined_generic_arguments =
            () => SimpleCollectionValueConverter.CanConvert(typeof(TestGenericDictionarySubsclass)).ShouldBeFalse();

        It should_return_false_for_genric_dictionary_subclass =
            () => SimpleCollectionValueConverter.CanConvert(typeof(TestGenericDictionarySubsclass2<int, int>)).ShouldBeFalse();

        enum TestEnum
        {
        }

        struct TestStruct
        {
        }

        class TestClass
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
