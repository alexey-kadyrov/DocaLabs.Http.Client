using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;
using Machine.Specifications.Annotations;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    // ReSharper disable UnusedMember.Local
    // ReSharper disable InconsistentNaming
    // ReSharper disable UnusedParameter.Local
    // ReSharper disable ValueParameterNotUsed

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_trying_to_create_simple_dictionary_property_converter
    {
        It should_not_create_it_for_property_without_public_getter =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("PropertyNoPublicGetter")).ShouldBeNull();

        It should_not_create_it_for_property_without_getter =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("PropertyNoGetter")).ShouldBeNull();

        It should_create_it_for_property_without_public_setter =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("PropertyNoPublicSetter")).ShouldNotBeNull();

        It should_create_it_for_property_without_setter =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("PropertyNoSetter")).ShouldNotBeNull();

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

        It should_not_create_it_for_name_value_collection =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("NameValueCollection")).ShouldBeNull();

        It should_create_it_for_dictionary_interface =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("DictionaryInterface")).ShouldNotBeNull();

        It should_create_it_for_generic_dictionary_interface =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("GenericDictionaryInterface")).ShouldNotBeNull();

        It should_create_it_for_generic_dictionary_sub_interface_wirh_defined_generic_arguments =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("GenericDictionarySubinterface")).ShouldNotBeNull();

        It should_create_it_for_generic_dictionary_sub_interface =
            () => SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("GenericDictionarySubinterface2")).ShouldNotBeNull();

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
            public IDictionary<string, string> PropertyNoPublicGetter { private get; set; }
            public IDictionary<string, string> PropertyNoGetter { set { } }
            public IDictionary<string, string> PropertyNoPublicSetter { get; private set; }
            public IDictionary<string, string> PropertyNoSetter { get { return null; } }
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

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_trying_to_create_simple_dictionary_property_converter_for_null_property_info
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => SimpleDictionaryPropertyConverter.TryCreate(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_info_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("property");
    }

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_simple_dictionary_property_converter_is_used_on_null_instance
    {
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));

        Because of =
            () => result = converter.Convert(null, new HashSet<object>());

        It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public IDictionary<string, int> Values { get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_simple_dictionary_property_converter_is_used_on_null_property
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass();
            converter = SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public IDictionary<string, int> Values { get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_simple_dictionary_property_converter_is_used
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new Dictionary<string, int>
                {
                    { "key27", 27 },
                    { "key42", 42 }
                }
            };

            converter = SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_keys =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42");

        It should_be_able_to_get_values_for_the_first_key =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_get_values_for_the_second_key =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");

        class TestClass
        {
            public IDictionary<string, int> Values { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_simple_dictionary_property_converter_is_used_with_null_processed_set
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new Dictionary<string, int>
                {
                    { "key27", 27 },
                    { "key42", 42 }
                }
            };

            converter = SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, null);

        It should_still_be_able_to_get_the_keys =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42");

        It should_still_be_able_to_get_values_for_the_first_key =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_still_be_able_to_get_values_for_the_second_key =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");

        class TestClass
        {
            public IDictionary<string, int> Values { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_simple_dictionary_property_converter_is_used_on_collection_property_where_some_keys_are_empty
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new Dictionary<string, string>
                {
                    { "", "27" },
                    { "key42", "42" }
                }
            };

            converter = SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_non_empty_keys =
            () => result.AllKeys.ShouldContainOnly("Values.key42");

        It should_be_able_to_get_values_of_non_empty_key =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");

        class TestClass
        {
            public IDictionary<string, string> Values { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_simple_dictionary_property_converter_is_used_on_collection_property_where_some_values_are_null
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new Dictionary<string, string>
                {
                    { "key27", null },
                    { "key42", "42" }
                }
            };

            converter = SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_all_keys =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42");

        It should_be_able_to_get_values_of_first_key =
            () => result.GetValues("Values.key27").ShouldContainOnly("");

        It should_be_able_to_get_values_of_second_key =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");

        class TestClass
        {
            public IDictionary<string, string> Values { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_simple_dictionary_property_converter_is_used_together_with_propert_overrides_attribute_where_name_and_format_are_not_set
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new Dictionary<string, int>
                {
                    { "key27", 27 },
                    { "key42", 42 }
                }
            };

            converter = SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_keys =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42");

        It should_be_able_to_get_values_for_the_first_key =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_get_values_for_the_second_key =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");

        class TestClass
        {
            [PropertyOverrides]
            public IDictionary<string, int> Values { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_simple_dictionary_property_converter_is_used_together_with_propert_overrides_attribute_where_name_and_format_are_empty_strings
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new Dictionary<string, int>
                {
                    { "key27", 27 },
                    { "key42", 42 }
                }
            };

            converter = SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_source_keys =
            () => result.AllKeys.ShouldContainOnly("key27", "key42");

        It should_be_able_to_get_values_for_the_first_key =
            () => result.GetValues("key27").ShouldContainOnly("27");

        It should_be_able_to_get_values_for_the_second_key =
            () => result.GetValues("key42").ShouldContainOnly("42");

        class TestClass
        {
            [PropertyOverrides(Name = "", Format = "")]
            public IDictionary<string, int> Values { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_simple_dictionary_property_converter_is_used_on_property_which_name_is_redefined
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new Dictionary<string, int>
                {
                    { "key27", 27 },
                    { "key42", 42 }
                }
            };

            converter = SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_keys_prefixed_by_specified_name =
            () => result.AllKeys.ShouldContainOnly("Hello World.key27", "Hello World.key42");

        It should_be_able_to_get_values_for_the_first_key =
            () => result.GetValues("Hello World.key27").ShouldContainOnly("27");

        It should_be_able_to_get_values_for_the_second_key =
            () => result.GetValues("Hello World.key42").ShouldContainOnly("42");

        class TestClass
        {
            [PropertyOverrides(Name = "Hello World")]
            public IDictionary<string, int> Values { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryPropertyConverter))]
    class when_simple_dictionary_property_converter_is_used_on_property_with_custom_format_applied
    {
        static TestClass instance;
        static IPropertyConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Values = new Dictionary<string, int>
                {
                    { "key27", 27 },
                    { "key42", 42 }
                }
            };

            converter = SimpleDictionaryPropertyConverter.TryCreate(typeof(TestClass).GetProperty("Values"));
        };

        Because of =
            () => result = converter.Convert(instance, new HashSet<object>());

        It should_be_able_to_get_the_source_keys =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42");

        It should_be_able_to_get_values_for_the_first_key_using_specified_format =
            () => result.GetValues("Values.key27").ShouldContainOnly("1B");

        It should_be_able_to_get_values_for_the_second_key_using_specified_format =
            () => result.GetValues("Values.key42").ShouldContainOnly("2A");

        class TestClass
        {
            [PropertyOverrides(Format = "{0:X}")]
            public IDictionary<string, int> Values { [UsedImplicitly] get; set; }
        }
    }

    // ReSharper restore ValueParameterNotUsed
    // ReSharper restore UnusedParameter.Local
    // ReSharper restore InconsistentNaming
    // ReSharper restore UnusedMember.Local
}
