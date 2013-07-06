using System;
using System.Collections;
using System.Collections.Generic;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    // ReSharper disable UnusedMember.Local

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_trying_to_create_simple_dictionary_property_converter
    {
        It should_not_create_it_for_bool =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("BoolProperty")).ShouldBeNull();

        It should_not_create_it_for_char =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("CharProperty")).ShouldBeNull();

        It should_not_create_it_for_byte =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ByteProperty")).ShouldBeNull();

        It should_not_create_it_for_short =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ShortProperty")).ShouldBeNull();

        It should_not_create_it_for_ushort =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("UShortProperty")).ShouldBeNull();

        It should_not_create_it_for_int =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("IntProperty")).ShouldBeNull();

        It should_not_create_it_for_uint =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("UIntProperty")).ShouldBeNull();

        It should_not_create_it_for_long =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("LongProperty")).ShouldBeNull();

        It should_not_create_it_for_ulong =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ULongProperty")).ShouldBeNull();

        It should_not_create_it_for_float =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("FloatProperty")).ShouldBeNull();

        It should_not_create_it_for_double =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("DoubleProperty")).ShouldBeNull();

        It should_not_create_it_for_decimal =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("DecimalProperty")).ShouldBeNull();

        It should_not_create_it_for_enum =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumProperty")).ShouldBeNull();

        It should_not_create_it_for_guid =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("GuidProperty")).ShouldBeNull();

        It should_not_create_it_for_datetime =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("DateTimeProperty")).ShouldBeNull();

        It should_not_create_it_for_datetimeoffset =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("DateTimeOffsetProperty")).ShouldBeNull();

        It should_not_create_it_for_timespan =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("TimeSpanProperty")).ShouldBeNull();

        It should_not_create_it_for_string =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("StringProperty")).ShouldBeNull();

        It should_not_create_it_for_byte_array =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ByteArrayProperty")).ShouldBeNull();

        It should_not_create_it_for_object =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ObjectProperty")).ShouldBeNull();

        It should_not_create_it_for_class =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ClassProperty")).ShouldBeNull();

        It should_not_create_it_for_struct =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("StructProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_strings =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableStringProperty")).ShouldBeNull();

        It should_not_create_it_for_list_of_string =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ListStringProperty")).ShouldBeNull();

        It should_not_create_it_for_ilist_of_string =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("IListStringProperty")).ShouldBeNull();

        It should_not_create_it_for_array_of_string =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ArrayStringProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_byte_arrays =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableByteArrayProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_object =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableObjectProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_bool =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableBoolProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_char =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableCharProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_byte =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableByteProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_short =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableShortProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_ushort =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableUShortProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_int =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableIntProperty")).ShouldBeNull();

        It should_not_create_it_for_list_of_int =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ListIntProperty")).ShouldBeNull();

        It should_not_create_it_for_ilist_of_int =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("IListIntProperty")).ShouldBeNull();

        It should_not_create_it_for_array_of_int =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("ArrayIntProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_uint =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableUIntProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_long =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableLongProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_ulong =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableULongProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_float =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableFloatProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_double =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableDoubleProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_decimal =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableDecimalProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_enum =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableEnumProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_guid =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableGuidProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_datetime =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableDateTimeProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_datetimeoffset =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableDateTimeOffsetProperty")).ShouldBeNull();

        It should_not_create_it_for_enumerable_of_timespan =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumerableTimeSpanProperty")).ShouldBeNull();
         
        It should_not_create_it_for_indexer =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Item")).ShouldBeNull();

        It should_create_it_for_dictionary_interface =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("DictionaryInterface")).ShouldNotBeNull();

        It should_create_it_for_generic_dictionary_interface =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("GenericDictionaryInterface")).ShouldNotBeNull();

        It should_create_it_for_generic_dictionary_sub_interface =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("GenericDictionarySubinterface")).ShouldNotBeNull();

        It should_create_it_for_hashtable =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Hashtable")).ShouldNotBeNull();

        It should_create_it_for_sorted_list =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("SortedList")).ShouldNotBeNull();

        It should_create_it_for_generic_dictionary =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("GenericDictionary")).ShouldNotBeNull();

        It should_create_it_for_dictionary_subsclass =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("DictionarySubsclass")).ShouldNotBeNull();

        It should_create_it_for_generic_dictionary_subclass =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("GenericDictionarySubsclass")).ShouldNotBeNull();

        It should_create_it_for_generic_dictionary_subclass_with_defined_generic_arguments =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("GenericDictionarySubsclass2")).ShouldNotBeNull();

        class TestClass
        {
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

            public IDictionary DictionaryInterface { get; set; }
            public IDictionary<int, int> GenericDictionaryInterface { get; set; }
            public ITestGenericDictionarySubinterface<int, int> GenericDictionarySubinterface { get; set; }
            public Hashtable Hashtable { get; set; }
            public SortedList SortedList { get; set; }
            public Dictionary<int, int> GenericDictionary { get; set; }
            public TestDictionarySubsclass DictionarySubsclass { get; set; }
            public TestGenericDictionarySubsclass GenericDictionarySubsclass { get; set; }
            public TestGenericDictionarySubsclass2<int, int> GenericDictionarySubsclass2 { get; set; }

            public IDictionary this[int index]
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

        interface ITestGenericDictionarySubinterface<TKey, TValue> : IDictionary<TKey, TValue>
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

    // ReSharper restore UnusedMember.Local
}
