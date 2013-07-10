using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_on_null_value
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleValueConverter("Value", null);

        Because of =
            () => result = converter.Convert(null);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_with_null_format
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context = 
            () => converter = new SimpleValueConverter("Value", null);

        Because of =
            () => result = converter.Convert(42);

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_convert_value =
            () => result.GetValues("Value").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_on_nullable_variable
    {
        static IValueConverter converter;
        static NameValueCollection result;
        static int? value;

        Establish context = () =>
        {
            converter = new SimpleValueConverter("Value", null);
            value = 42;
        };

        Because of =
            () => result = converter.Convert(value);

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_convert_value =
            () => result.GetValues("Value").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_with_empty_format
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleValueConverter("Value", "");

        Because of =
            () => result = converter.Convert(42);

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_convert_value =
            () => result.GetValues("Value").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_with_specified_format
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleValueConverter("Value", "{0:X}");

        Because of =
            () => result = converter.Convert(42);

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_convert_value =
            () => result.GetValues("Value").ShouldContainOnly("2A");
    }

    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_with_null_name
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => new SimpleValueConverter(null, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_name_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("name");
    }

    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_with_empty_name
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => new SimpleValueConverter("", null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_name_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("name");
    }

    [Subject(typeof(SimpleValueConverter))]
    class when_checking_whenever_simple_value_converter_can_convert_the_type
    {
        It should_return_true_for_bool =
            () => SimpleValueConverter.CanConvert(typeof(bool)).ShouldBeTrue();

        It should_return_true_for_byte =
            () => SimpleValueConverter.CanConvert(typeof(byte)).ShouldBeTrue();

        It should_return_true_for_char =
            () => SimpleValueConverter.CanConvert(typeof(char)).ShouldBeTrue();

        It should_return_true_for_short =
            () => SimpleValueConverter.CanConvert(typeof(short)).ShouldBeTrue();

        It should_return_true_for_ushort =
            () => SimpleValueConverter.CanConvert(typeof(ushort)).ShouldBeTrue();

        It should_return_true_for_int =
            () => SimpleValueConverter.CanConvert(typeof(int)).ShouldBeTrue();

        It should_return_true_for_uint =
            () => SimpleValueConverter.CanConvert(typeof(uint)).ShouldBeTrue();

        It should_return_true_for_long =
            () => SimpleValueConverter.CanConvert(typeof(long)).ShouldBeTrue();

        It should_return_true_for_ulong =
            () => SimpleValueConverter.CanConvert(typeof(ulong)).ShouldBeTrue();

        It should_return_true_for_float =
            () => SimpleValueConverter.CanConvert(typeof(float)).ShouldBeTrue();

        It should_return_true_for_double =
            () => SimpleValueConverter.CanConvert(typeof(double)).ShouldBeTrue();

        It should_return_true_for_decimal =
            () => SimpleValueConverter.CanConvert(typeof(decimal)).ShouldBeTrue();

        It should_return_true_for_guid =
            () => SimpleValueConverter.CanConvert(typeof(Guid)).ShouldBeTrue();

        It should_return_true_for_date_time =
            () => SimpleValueConverter.CanConvert(typeof(DateTime)).ShouldBeTrue();

        It should_return_true_for_date_time_offset =
            () => SimpleValueConverter.CanConvert(typeof(DateTimeOffset)).ShouldBeTrue();

        It should_return_true_for_time_span =
            () => SimpleValueConverter.CanConvert(typeof(TimeSpan)).ShouldBeTrue();

        It should_return_true_for_enum =
            () => SimpleValueConverter.CanConvert(typeof(TestEnum)).ShouldBeTrue();

        It should_return_true_for_string =
            () => SimpleValueConverter.CanConvert(typeof(string)).ShouldBeTrue();

        It should_return_true_for_byte_array =
            () => SimpleValueConverter.CanConvert(typeof(byte[])).ShouldBeTrue();

        It should_return_true_for_nullable_bool =
            () => SimpleValueConverter.CanConvert(typeof(bool?)).ShouldBeTrue();

        It should_return_true_for_nullable_byte =
            () => SimpleValueConverter.CanConvert(typeof(byte?)).ShouldBeTrue();

        It should_return_true_for_nullable_char =
            () => SimpleValueConverter.CanConvert(typeof(char?)).ShouldBeTrue();

        It should_return_true_for_nullable_short =
            () => SimpleValueConverter.CanConvert(typeof(short?)).ShouldBeTrue();

        It should_return_true_for_nullable_ushort =
            () => SimpleValueConverter.CanConvert(typeof(ushort?)).ShouldBeTrue();

        It should_return_true_for_nullable_int =
            () => SimpleValueConverter.CanConvert(typeof(int?)).ShouldBeTrue();

        It should_return_true_for_nullable_uint =
            () => SimpleValueConverter.CanConvert(typeof(uint?)).ShouldBeTrue();

        It should_return_true_for_nullable_long =
            () => SimpleValueConverter.CanConvert(typeof(long?)).ShouldBeTrue();

        It should_return_true_for_nullable_ulong =
            () => SimpleValueConverter.CanConvert(typeof(ulong?)).ShouldBeTrue();

        It should_return_true_for_nullable_float =
            () => SimpleValueConverter.CanConvert(typeof(float?)).ShouldBeTrue();

        It should_return_true_for_nullable_double =
            () => SimpleValueConverter.CanConvert(typeof(double?)).ShouldBeTrue();

        It should_return_true_for_nullable_decimal =
            () => SimpleValueConverter.CanConvert(typeof(decimal?)).ShouldBeTrue();

        It should_return_true_for_nullable_guid =
            () => SimpleValueConverter.CanConvert(typeof(Guid?)).ShouldBeTrue();

        It should_return_true_for_nullable_date_time =
            () => SimpleValueConverter.CanConvert(typeof(DateTime?)).ShouldBeTrue();

        It should_return_true_for_nullable_date_time_offset =
            () => SimpleValueConverter.CanConvert(typeof(DateTimeOffset?)).ShouldBeTrue();

        It should_return_true_for_nullable_time_span =
            () => SimpleValueConverter.CanConvert(typeof(TimeSpan?)).ShouldBeTrue();

        It should_return_true_for_nullable_enum =
            () => SimpleValueConverter.CanConvert(typeof(TestEnum?)).ShouldBeTrue();

        It should_return_false_for_bool_array =
            () => SimpleValueConverter.CanConvert(typeof(bool[])).ShouldBeFalse();

        It should_return_false_for_char_array =
            () => SimpleValueConverter.CanConvert(typeof(char[])).ShouldBeFalse();

        It should_return_false_for_short_array =
            () => SimpleValueConverter.CanConvert(typeof(short[])).ShouldBeFalse();

        It should_return_false_for_ushort_array =
            () => SimpleValueConverter.CanConvert(typeof(ushort[])).ShouldBeFalse();

        It should_return_false_for_int_array =
            () => SimpleValueConverter.CanConvert(typeof(int[])).ShouldBeFalse();

        It should_return_false_for_uint_array =
            () => SimpleValueConverter.CanConvert(typeof(uint[])).ShouldBeFalse();

        It should_return_false_for_long_array =
            () => SimpleValueConverter.CanConvert(typeof(long[])).ShouldBeFalse();

        It should_return_false_for_ulong_array =
            () => SimpleValueConverter.CanConvert(typeof(ulong[])).ShouldBeFalse();

        It should_return_false_for_float_array =
            () => SimpleValueConverter.CanConvert(typeof(float[])).ShouldBeFalse();

        It should_return_false_for_double_array =
            () => SimpleValueConverter.CanConvert(typeof(double[])).ShouldBeFalse();

        It should_return_false_for_decimal_array =
            () => SimpleValueConverter.CanConvert(typeof(decimal[])).ShouldBeFalse();

        It should_return_false_for_guid_array =
            () => SimpleValueConverter.CanConvert(typeof(Guid[])).ShouldBeFalse();

        It should_return_false_for_date_time_array =
            () => SimpleValueConverter.CanConvert(typeof(DateTime[])).ShouldBeFalse();

        It should_return_false_for_date_time_offset_array =
            () => SimpleValueConverter.CanConvert(typeof(DateTimeOffset[])).ShouldBeFalse();

        It should_return_false_for_time_span_array =
            () => SimpleValueConverter.CanConvert(typeof(TimeSpan[])).ShouldBeFalse();

        It should_return_false_for_enum_array =
            () => SimpleValueConverter.CanConvert(typeof(TestEnum[])).ShouldBeFalse();

        It should_return_false_for_string_array =
            () => SimpleValueConverter.CanConvert(typeof(string[])).ShouldBeFalse();

        It should_return_false_for_struct =
            () => SimpleValueConverter.CanConvert(typeof(TestStruct)).ShouldBeFalse();

        It should_return_false_for_nullable_struct =
            () => SimpleValueConverter.CanConvert(typeof(TestStruct?)).ShouldBeFalse();

        It should_return_false_for_class =
            () => SimpleValueConverter.CanConvert(typeof(TestClass)).ShouldBeFalse();

        It should_return_false_for_object =
            () => SimpleValueConverter.CanConvert(typeof(object)).ShouldBeFalse();

        It should_return_false_for_namevaluecollection =
            () => SimpleValueConverter.CanConvert(typeof(NameValueCollection)).ShouldBeFalse();

        It should_return_false_for_idictionary =
            () => SimpleValueConverter.CanConvert(typeof(IDictionary)).ShouldBeFalse();

        It should_return_false_for_generic_idictionary =
            () => SimpleValueConverter.CanConvert(typeof(IDictionary<int, int>)).ShouldBeFalse();

        It should_return_false_for_hastable =
            () => SimpleValueConverter.CanConvert(typeof(Hashtable)).ShouldBeFalse();

        It should_return_false_for_sortedlist =
            () => SimpleValueConverter.CanConvert(typeof(SortedList)).ShouldBeFalse();

        It should_return_false_for_dictionary_subclass =
            () => SimpleValueConverter.CanConvert(typeof(TestDictionarySubsclass)).ShouldBeFalse();

        It should_return_false_for_genric_dictionary_sub_interface_with_defined_generic_arguments =
            () => SimpleValueConverter.CanConvert(typeof(ITestGenericDictionarySubinterface)).ShouldBeFalse();

        It should_return_false_for_genric_dictionary_sub_interface =
            () => SimpleValueConverter.CanConvert(typeof(ITestGenericDictionarySubinterface2<int, int>)).ShouldBeFalse();

        It should_return_false_for_genric_dictionary_subclass_with_defined_generic_arguments =
            () => SimpleValueConverter.CanConvert(typeof(TestGenericDictionarySubsclass)).ShouldBeFalse();

        It should_return_false_for_genric_dictionary_subclass =
            () => SimpleValueConverter.CanConvert(typeof(TestGenericDictionarySubsclass2<int, int>)).ShouldBeFalse();

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
