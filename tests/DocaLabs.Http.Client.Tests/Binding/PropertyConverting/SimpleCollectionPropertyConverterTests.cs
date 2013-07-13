using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    // ReSharper disable UnusedMember.Local
    // ReSharper disable InconsistentNaming
    // ReSharper disable ValueParameterNotUsed
    // ReSharper disable UnusedParameter.Local

    [Subject(typeof(SimpleCollectionPropertyConverter))]
    class when_trying_to_create_simple_collection_property_converter
    {
        It should_not_create_it_for_property_without_public_getter =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("PropertyNoPublicGetter")).ShouldBeNull();

        It should_not_create_it_for_property_without_getter =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("PropertyNoGetter")).ShouldBeNull();

        It should_create_it_for_property_without_public_setter =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("PropertyNoPublicSetter")).ShouldNotBeNull();

        It should_create_it_for_property_without_setter =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("PropertyNoSetter")).ShouldNotBeNull();

        It should_not_create_it_for_bool =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("BoolProperty")).ShouldBeNull();

        It should_not_create_it_for_char =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("CharProperty")).ShouldBeNull();

        It should_not_create_it_for_byte =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ByteProperty")).ShouldBeNull();

        It should_not_create_it_for_short =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ShortProperty")).ShouldBeNull();

        It should_not_create_it_for_ushort =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("UShortProperty")).ShouldBeNull();

        It should_not_create_it_for_int =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("IntProperty")).ShouldBeNull();

        It should_not_create_it_for_uint =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("UIntProperty")).ShouldBeNull();

        It should_not_create_it_for_long =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("LongProperty")).ShouldBeNull();

        It should_not_create_it_for_ulong =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ULongProperty")).ShouldBeNull();

        It should_not_create_it_for_float =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("FloatProperty")).ShouldBeNull();

        It should_not_create_it_for_double =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("DoubleProperty")).ShouldBeNull();

        It should_not_create_it_for_decimal =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("DecimalProperty")).ShouldBeNull();

        It should_not_create_it_for_enum =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumProperty")).ShouldBeNull();

        It should_not_create_it_for_guid =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("GuidProperty")).ShouldBeNull();

        It should_not_create_it_for_datetime =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("DateTimeProperty")).ShouldBeNull();

        It should_not_create_it_for_datetimeoffset =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("DateTimeOffsetProperty")).ShouldBeNull();

        It should_not_create_it_for_timespan =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("TimeSpanProperty")).ShouldBeNull();

        It should_not_create_it_for_string =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("StringProperty")).ShouldBeNull();

        It should_not_create_it_for_byte_array =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ByteArrayProperty")).ShouldBeNull();

        It should_not_create_it_for_object =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ObjectProperty")).ShouldBeNull();

        It should_not_create_it_for_class =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ClassProperty")).ShouldBeNull();

        It should_not_create_it_for_struct =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("StructProperty")).ShouldBeNull();

        It should_create_it_for_enumerable_of_strings =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableStringProperty")).ShouldNotBeNull();

        It should_create_it_for_list_of_string =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ListStringProperty")).ShouldNotBeNull();

        It should_create_it_for_ilist_of_string =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("IListStringProperty")).ShouldNotBeNull();

        It should_create_it_for_array_of_string =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ArrayStringProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_byte_arrays =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableByteArrayProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_object =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableObjectProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_bool =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableBoolProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_char =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableCharProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_byte =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableByteProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_short =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableShortProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_ushort =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableUShortProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_int =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableIntProperty")).ShouldNotBeNull();

        It should_create_it_for_list_of_int =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ListIntProperty")).ShouldNotBeNull();

        It should_create_it_for_ilist_of_int =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("IListIntProperty")).ShouldNotBeNull();

        It should_create_it_for_array_of_int =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ArrayIntProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_uint =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableUIntProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_long =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableLongProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_ulong =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableULongProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_float =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableFloatProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_double =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableDoubleProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_decimal =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableDecimalProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_enum =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableEnumProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_guid =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableGuidProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_datetime =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableDateTimeProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_datetimeoffset =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableDateTimeOffsetProperty")).ShouldNotBeNull();

        It should_create_it_for_enumerable_of_timespan =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableTimeSpanProperty")).ShouldNotBeNull();

        It should_not_create_it_for_indexer =
            () => SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Item")).ShouldBeNull();

        class TestClass
        {
            public IEnumerable<string> PropertyNoPublicGetter { private get; set; }
            public IEnumerable<string> PropertyNoGetter { set { } }
            public IEnumerable<string> PropertyNoPublicSetter { get; private set; }
            public IEnumerable<string> PropertyNoSetter { get { return null; } }
            public bool BoolProperty { get; set; }
            public char CharProperty { get; set; }
            public byte ByteProperty { get; set; }
            public short ShortProperty { get; set; }
            public ushort UShortProperty { get; set; }
            public int IntProperty { get; set; }
            public uint UIntProperty { get; set; }
            public long LongProperty { get; set; }
            public ulong ULongProperty { get; set; }
            public float FloatProperty { get; set; }
            public double DoubleProperty { get; set; }
            public decimal DecimalProperty { get; set; }
            public TestEnum EnumProperty { get; set; }
            public Guid GuidProperty { get; set; }
            public DateTime DateTimeProperty { get; set; }
            public DateTimeOffset DateTimeOffsetProperty { get; set; }
            public TimeSpan TimeSpanProperty { get; set; }
            public string StringProperty { get; set; }
            public byte[] ByteArrayProperty { get; set; }
            public object ObjectProperty { get; set; }
            public TestClass ClassProperty { get; set; }
            public TestStruct StructProperty { get; set; }
            public IEnumerable<string> EnumerableStringProperty { get; set; }
            public List<string> ListStringProperty { get; set; }
            public IList<string> IListStringProperty { get; set; }
            public string[] ArrayStringProperty { get; set; }
            public IEnumerable<byte[]> EnumerableByteArrayProperty { get; set; }
            public IEnumerable<bool> EnumerableBoolProperty { get; set; }
            public IEnumerable<char> EnumerableCharProperty { get; set; }
            public IEnumerable<byte> EnumerableByteProperty { get; set; }
            public IEnumerable<short> EnumerableShortProperty { get; set; }
            public IEnumerable<ushort> EnumerableUShortProperty { get; set; }
            public IEnumerable<int> EnumerableIntProperty { get; set; }
            public List<int> ListIntProperty { get; set; }
            public IList<int> IListIntProperty { get; set; }
            public int[] ArrayIntProperty { get; set; }
            public IEnumerable<uint> EnumerableUIntProperty { get; set; }
            public IEnumerable<long> EnumerableLongProperty { get; set; }
            public IEnumerable<ulong> EnumerableULongProperty { get; set; }
            public IEnumerable<float> EnumerableFloatProperty { get; set; }
            public IEnumerable<double> EnumerableDoubleProperty { get; set; }
            public IEnumerable<decimal> EnumerableDecimalProperty { get; set; }
            public IEnumerable<TestEnum> EnumerableEnumProperty { get; set; }
            public IEnumerable<Guid> EnumerableGuidProperty { get; set; }
            public IEnumerable<DateTime> EnumerableDateTimeProperty { get; set; }
            public IEnumerable<DateTimeOffset> EnumerableDateTimeOffsetProperty { get; set; }
            public IEnumerable<TimeSpan> EnumerableTimeSpanProperty { get; set; }
            public IEnumerable<object> EnumerableObjectProperty { get; set; }
            public IEnumerable EnumerableProperty { get; set; }
            public IEnumerable<int> this[int index]
            {
                get { return null; }
                set { }
            }
        }

        enum TestEnum
        {
        }

        struct TestStruct
        {
        }
    }

    [Subject(typeof(SimpleCollectionPropertyConverter))]
    class when_trying_to_create_simple_collection_property_converter_for_null_property_info
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => SimpleCollectionPropertyConverter.TryCreate(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_info_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("property");
    }

    [Subject(typeof(SimpleCollectionPropertyConverter))]
    class when_simple_collection_property_converter_is_used_on_null_instance
    {
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));

        Because of =
            () => result = converter.Convert(null, new HashSet<object>());

        It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public IEnumerable<int> Values { get; set; }
        }
    }

    [Subject(typeof(SimpleCollectionPropertyConverter))]
    class when_simple_collection_property_converter_is_used_on_null_property
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass();
            converter = SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public IEnumerable<int> Values { get; set; }
        }
    }

    [Subject(typeof(SimpleCollectionPropertyConverter))]
    class when_simple_collection_property_converter_is_used
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new[] { 27, 42 }
            };

            converter = SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Values").ShouldContainOnly("27", "42");

        class TestClass
        {
            public IEnumerable<int> Values { get; set; }
        }
    }

    [Subject(typeof(SimpleCollectionPropertyConverter))]
    class when_simple_collection_property_converter_is_used_with_null_processed_set
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new[] { 27, 42 }
            };

            converter = SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, null);

        It should_still_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_still_be_able_to_get_value_of_property =
            () => result.GetValues("Values").ShouldContainOnly("27", "42");

        class TestClass
        {
            public IEnumerable<int> Values { get; set; }
        }
    }

    [Subject(typeof(SimpleCollectionPropertyConverter))]
    class when_simple_collection_property_converter_is_used_on_collection_property_where_some_values_are_null
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new[] { null, "Hello", null }
            };

            converter = SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Values").ShouldContainOnly("", "Hello", "");

        class TestClass
        {
            public IEnumerable<string> Values { get; set; }
        }
    }

    [Subject(typeof(SimpleCollectionPropertyConverter))]
    class when_simple_collection_property_converter_is_used_together_with_propert_overrides_attribute_where_name_and_format_are_not_set
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new[] { 27, 42 }
            };

            converter = SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Values").ShouldContainOnly("27", "42");

        class TestClass
        {
            [PropertyOverrides]
            public IEnumerable<int> Values { get; set; }
        }
    }

    [Subject(typeof (SimpleCollectionPropertyConverter))]
    class when_simple_collection_property_converter_is_used_together_with_propert_overrides_attribute_where_name_and_format_are_empty_strings
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new[] {27, 42}
            };

            converter = SimpleCollectionPropertyConverter.TryCreate(typeof (TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Values").ShouldContainOnly("27", "42");

        class TestClass
        {
            [PropertyOverrides(Name = "", Format = "")]
            public IEnumerable<int> Values { get; set; }
        }
    }

    [Subject(typeof(SimpleCollectionPropertyConverter))]
    class when_simple_collection_property_converter_is_used_on_property_which_name_is_redefined
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new[] { 27, 42 }
            };

            converter = SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_key_as_the_redefined_name =
            () => result.AllKeys.ShouldContainOnly("Hello World");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Hello World").ShouldContainOnly("27", "42");

        class TestClass
        {
            [PropertyOverrides(Name = "Hello World")]
            public IEnumerable<int> Values { get; set; }
        }
    }

    [Subject(typeof(SimpleCollectionPropertyConverter))]
    class when_simple_collection_property_converter_is_used_on_property_with_custom_format_applied
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new[] { 27, 42 }
            };

            converter = SimpleCollectionPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_key_as_the_redefined_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Values").ShouldContainOnly("1B", "2A");

        class TestClass
        {
            [PropertyOverrides(Format = "{0:X}")]
            public IEnumerable<int> Values { get; set; }
        }
    }

    // ReSharper restore UnusedParameter.Local
    // ReSharper restore ValueParameterNotUsed
    // ReSharper restore InconsistentNaming
    // ReSharper restore UnusedMember.Local
    // ReSharper restore UnusedAutoPropertyAccessor.Local
}
