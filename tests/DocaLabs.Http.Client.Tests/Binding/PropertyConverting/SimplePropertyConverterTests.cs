using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    // ReSharper disable UnusedMember.Local
    // ReSharper disable ValueParameterNotUsed
    // ReSharper disable UnusedParameter.Local
    // ReSharper disable InconsistentNaming

    [Subject(typeof(SimplePropertyConverter))]
    class when_trying_to_create_simple_property_converter
    {
        It should_not_create_it_for_property_without_public_getter =
            () => SimplePropertyConverter.TryCreate(typeof(TestModel).GetProperty("PropertyNoPublicGetter")).ShouldBeNull();

        It should_not_create_it_for_property_without_getter =
            () => SimplePropertyConverter.TryCreate(typeof(TestModel).GetProperty("PropertyNoGetter")).ShouldBeNull();

        It should_create_it_for_property_without_public_setter =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.PropertyNoPublicSetter)).ShouldNotBeNull();

        It should_create_it_for_property_without_setter =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.PropertyNoSetter)).ShouldNotBeNull();

        It should_create_it_for_bool =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.BoolProperty)).ShouldNotBeNull();

        It should_create_it_for_char =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.CharProperty)).ShouldNotBeNull();

        It should_create_it_for_byte =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.ByteProperty)).ShouldNotBeNull();

        It should_create_it_for_short =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.ShortProperty)).ShouldNotBeNull();

        It should_create_it_for_ushort =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.UShortProperty)).ShouldNotBeNull();

        It should_create_it_for_int =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.IntProperty)).ShouldNotBeNull();

        It should_create_it_for_uint =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.UIntProperty)).ShouldNotBeNull();

        It should_create_it_for_long =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.LongProperty)).ShouldNotBeNull();

        It should_create_it_for_ulong =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.ULongProperty)).ShouldNotBeNull();

        It should_create_it_for_float =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.FloatProperty)).ShouldNotBeNull();

        It should_create_it_for_double =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.DoubleProperty)).ShouldNotBeNull();

        It should_create_it_for_decimal =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.DecimalProperty)).ShouldNotBeNull();

        It should_create_it_for_enum =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumProperty)).ShouldNotBeNull();

        It should_create_it_for_guid =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.GuidProperty)).ShouldNotBeNull();

        It should_create_it_for_datetime =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.DateTimeProperty)).ShouldNotBeNull();

        It should_create_it_for_datetimeoffset =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.DateTimeOffsetProperty)).ShouldNotBeNull();

        It should_create_it_for_timespan =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.TimeSpanProperty)).ShouldNotBeNull();

        It should_create_it_for_string =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.StringProperty)).ShouldNotBeNull();

        It should_create_it_for_byte_array =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.ByteArrayProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_bool =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableBoolProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_char =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableCharProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_byte =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableByteProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_short =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableShortProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_ushort =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableUShortProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_int =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableIntProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_uint =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableUIntProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_long =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableLongProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_ulong =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableULongProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_float =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableFloatProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_double =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableDoubleProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_decimal =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableDecimalProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_enum =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableEnumProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_guid =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableGuidProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_datetime =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableDateTimeProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_datetimeoffset =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableDateTimeOffsetProperty)).ShouldNotBeNull();

        It should_create_it_for_nullable_timespan =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableTimeSpanProperty)).ShouldNotBeNull();

        It should_not_create_it_for_object =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.ObjectProperty)).ShouldBeNull();

        It should_not_create_it_for_class =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.ClassProperty)).ShouldBeNull();

        It should_not_create_it_for_struct =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.StructProperty)).ShouldBeNull();

        It should_not_create_it_for_nullable_struct =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NullableStructProperty)).ShouldBeNull();

        It should_not_create_it_for_indexer =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetIndexerInfo<int>()).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_strings =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableStringProperty)).ShouldBeNull();

        It should_not_create_it_for_list_of_string =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.ListStringProperty)).ShouldBeNull();

        It should_not_create_it_for_ilist_of_string =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.IListStringProperty)).ShouldBeNull();

        It should_not_create_it_for_array_of_string =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.ArrayStringProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_byte_arrays =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableByteArrayProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_object =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableObjectProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_bool =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableBoolProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_char =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableCharProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_byte =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableByteProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_short =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableShortProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_ushort =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableUShortProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_int =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableIntProperty)).ShouldBeNull();

        It should_not_create_it_for_list_of_int =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.ListIntProperty)).ShouldBeNull();

        It should_not_create_it_for_ilist_of_int =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.IListIntProperty)).ShouldBeNull();

        It should_not_create_it_for_array_of_int =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.ArrayIntProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_uint =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableUIntProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_long =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableLongProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_ulong =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableULongProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_float =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableFloatProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_double =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableDoubleProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_decimal =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableDecimalProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_enum =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableEnumProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_guid =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableGuidProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_datetime =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableDateTimeProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_datetimeoffset =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableDateTimeOffsetProperty)).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_timespan =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.EnumerableTimeSpanProperty)).ShouldBeNull();

        It should_not_create_it_for_name_value_collection =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.NameValueCollection)).ShouldBeNull();

        It should_not_create_it_for_dictionary_interface =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.DictionaryInterface)).ShouldBeNull();

        It should_not_create_it_for_generic_dictionary_interface =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.GenericDictionaryInterface)).ShouldBeNull();

        It should_not_create_it_for_generic_dictionary_sub_interface_wirh_defined_generic_arguments =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.GenericDictionarySubinterface)).ShouldBeNull();

        It should_not_create_it_for_generic_dictionary_sub_interface =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.GenericDictionarySubinterface2)).ShouldBeNull();

        It should_not_create_it_for_hashtable =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.Hashtable)).ShouldBeNull();

        It should_not_create_it_for_sorted_list =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.SortedList)).ShouldBeNull();

        It should_not_create_it_for_generic_dictionary =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.GenericDictionary)).ShouldBeNull();

        It should_not_create_it_for_dictionary_subsclass =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.DictionarySubsclass)).ShouldBeNull();

        It should_not_create_it_for_generic_dictionary_subclass =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.GenericDictionarySubsclass)).ShouldBeNull();

        It should_not_create_it_for_generic_dictionary_subclass_with_defined_generic_arguments =
            () => SimplePropertyConverter.TryCreate(Reflect<TestModel>.GetPropertyInfo(m => m.GenericDictionarySubsclass2)).ShouldBeNull();

        class TestModel
        {
            public int PropertyNoPublicGetter { private get; set; }
            public int PropertyNoGetter { set {} }
            public int PropertyNoPublicSetter { get; private set; }
            public int PropertyNoSetter { get { return 0; } }
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
            public bool? NullableBoolProperty { get; set; }
            public char? NullableCharProperty { get; set; }
            public byte? NullableByteProperty { get; set; }
            public short? NullableShortProperty { get; set; }
            public ushort? NullableUShortProperty { get; set; }
            public int? NullableIntProperty { get; set; }
            public uint? NullableUIntProperty { get; set; }
            public long? NullableLongProperty { get; set; }
            public ulong? NullableULongProperty { get; set; }
            public float? NullableFloatProperty { get; set; }
            public double? NullableDoubleProperty { get; set; }
            public decimal? NullableDecimalProperty { get; set; }
            public TestEnum? NullableEnumProperty { get; set; }
            public Guid? NullableGuidProperty { get; set; }
            public DateTime? NullableDateTimeProperty { get; set; }
            public DateTimeOffset? NullableDateTimeOffsetProperty { get; set; }
            public TimeSpan? NullableTimeSpanProperty { get; set; }
            public object ObjectProperty { get; set; }
            public TestModel ClassProperty { get; set; }
            public TestStruct StructProperty { get; set; }
            public TestStruct? NullableStructProperty { get; set; }
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
            public NameValueCollection NameValueCollection { get; set; }
            public IDictionary DictionaryInterface { get; set; }
            public IDictionary<int, int> GenericDictionaryInterface { get; set; }
            public ITestGenericDictionarySubinterface GenericDictionarySubinterface { get; set; }
            public ITestGenericDictionarySubinterface2<int, int> GenericDictionarySubinterface2 { get; set; }
            public Hashtable Hashtable { get; set; }
            public SortedList SortedList { get; set; }
            public Dictionary<int, int> GenericDictionary { get; set; }
            public TestDictionarySubsclass DictionarySubsclass { get; set; }
            public TestGenericDictionarySubsclass GenericDictionarySubsclass { get; set; }
            public TestGenericDictionarySubsclass2<int, int> GenericDictionarySubsclass2 { get; set; }
            public int this[int index]
            {
                get { return 0; }
                set { }
            }
        }

        enum TestEnum
        {
        }

        struct TestStruct
        {
        }

        interface ITestGenericDictionarySubinterface : IDictionary<int, int>
        {
        }

        interface ITestGenericDictionarySubinterface2<TKey, TValue> : IDictionary<TKey, TValue>
        {
        }

        class TestDictionarySubsclass : Hashtable
        {
        }

        class TestGenericDictionarySubsclass : Dictionary<int, int>
        {
        }

        class TestGenericDictionarySubsclass2<TKey, TValue> : Dictionary<TKey, TValue>
        {
        }
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_trying_to_create_simple_property_converter_for_null_property_info
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => SimplePropertyConverter.TryCreate(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_property_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("property");
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_simple_property_converter_is_used_on_null_instance
    {
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));

        Because of =
            () => result = converter.Convert(null, new HashSet<object>());

        It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public int Value { get; set; }
        }
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_simple_property_converter_is_used_on_null_property
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass();

            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_simple_property_converter_is_used
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = 42 };
            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Value").ShouldContainOnly("42");

        class TestClass
        {
            public int Value { get; set; }
        }
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_simple_property_converter_is_used_with_null_processed_set
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = 42 };
            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance, null);

        It should_still_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_still_be_able_to_get_value_of_property =
            () => result.GetValues("Value").ShouldContainOnly("42");

        class TestClass
        {
            public int Value { get; set; }
        }
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_simple_property_converter_is_used_together_with_propert_overrides_attribute_where_name_and_format_are_not_set
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = 42 };
            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Value").ShouldContainOnly("42");

        class TestClass
        {
            [PropertyOverrides]
            public int Value { get; set; }
        }
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_simple_property_converter_is_used_together_with_propert_overrides_attribute_where_name_and_format_are_empty_strings
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = 42 };
            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Value").ShouldContainOnly("42");

        class TestClass
        {
            [PropertyOverrides(Name = "", Format = "")]
            public int Value { get; set; }
        }
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_simple_property_converter_is_used_on_property_which_name_is_redefined
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = 42 };
            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_key_as_the_redefined_name =
            () => result.AllKeys.ShouldContainOnly("Hello World");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Hello World").ShouldContainOnly("42");

        class TestClass
        {
            [PropertyOverrides(Name = "Hello World")]
            public int Value { get; set; }
        }
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_simple_property_converter_is_used_on_property_with_custom_format_applied
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = 42 };
            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_key_as_the_redefined_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Value").ShouldContainOnly("2A");

        class TestClass
        {
            [PropertyOverrides(Format = "{0:X}")]
            public int Value { get; set; }
        }
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_simple_property_converter_is_used_on_property_with_custom_format_applied_and_current_ui_culture
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Cleanup after =
            () => Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        Establish context = () =>
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
            instance = new TestClass { Value = new DateTime(2013, 2, 1) };
            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_key_as_the_redefined_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_get_value_of_property =
            () => result.GetValues("Value").ShouldContainOnly("фев");

        class TestClass
        {
            [PropertyOverrides(Format = "{0:MMM}", FormatCulture = FormatCulture.UseCurrentUI)]
            public DateTime Value { get; set; }
        }
    }

    // ReSharper restore InconsistentNaming
    // ReSharper restore UnusedParameter.Local
    // ReSharper restore ValueParameterNotUsed
    // ReSharper restore UnusedMember.Local
    // ReSharper restore UnusedAutoPropertyAccessor.Local
}
