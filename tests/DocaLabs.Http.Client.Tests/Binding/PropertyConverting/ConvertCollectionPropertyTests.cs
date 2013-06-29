//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using DocaLabs.Http.Client.Binding.Attributes;
//using DocaLabs.Http.Client.Binding.PropertyConverters;
//using DocaLabs.Http.Client.Utils;
//using Machine.Specifications;

//namespace DocaLabs.Http.Client.Tests.Mapping.PropertyConverters
//{
//    // ReSharper disable UnusedAutoPropertyAccessor.Local
//    // ReSharper disable UnusedMember.Local
//    // ReSharper disable InconsistentNaming
//    // ReSharper disable ValueParameterNotUsed
//    // ReSharper disable UnusedParameter.Local

//    [Subject(typeof(SimpleCollectionPropertyConverter<>))]
//    class when_trying_to_create_convert_collection_property
//    {
//        private It should_not_create_it_for_bool =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("BoolProperty")).ShouldBeNull();

//        private It should_not_create_it_for_char =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("CharProperty")).ShouldBeNull();

//        private It should_not_create_it_for_byte =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("ByteProperty")).ShouldBeNull();

//        private It should_not_create_it_for_short =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("ShortProperty")).ShouldBeNull();

//        private It should_not_create_it_for_ushort =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("UShortProperty")).ShouldBeNull();

//        private It should_not_create_it_for_int =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("IntProperty")).ShouldBeNull();

//        private It should_not_create_it_for_uint =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("UIntProperty")).ShouldBeNull();

//        private It should_not_create_it_for_long =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("LongProperty")).ShouldBeNull();

//        private It should_not_create_it_for_ulong =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("ULongProperty")).ShouldBeNull();

//        private It should_not_create_it_for_float =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("FloatProperty")).ShouldBeNull();

//        private It should_not_create_it_for_double =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("DoubleProperty")).ShouldBeNull();

//        private It should_not_create_it_for_decimal =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("DecimalProperty")).ShouldBeNull();

//        private It should_not_create_it_for_enum =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumProperty")).ShouldBeNull();

//        private It should_not_create_it_for_guid =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("GuidProperty")).ShouldBeNull();

//        private It should_not_create_it_for_datetime =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("DateTimeProperty")).ShouldBeNull();

//        private It should_not_create_it_for_datetimeoffset =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("DateTimeOffsetProperty")).ShouldBeNull();

//        private It should_not_create_it_for_timespan =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("TimeSpanProperty")).ShouldBeNull();

//        private It should_not_create_it_for_string =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("StringProperty")).ShouldBeNull();

//        private It should_not_create_it_for_byte_array =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("ByteArrayProperty")).ShouldBeNull();

//        private It should_not_create_it_for_object =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("ObjectProperty")).ShouldBeNull();

//        private It should_not_create_it_for_class =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("ClassProperty")).ShouldBeNull();

//        private It should_not_create_it_for_struct =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("StructProperty")).ShouldBeNull();

//        private It should_create_it_for_enumerable_of_strings =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableStringProperty")).ShouldNotBeNull();

//        private It should_create_it_for_list_of_string =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("ListStringProperty")).ShouldNotBeNull();

//        private It should_create_it_for_ilist_of_string =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("IListStringProperty")).ShouldNotBeNull();

//        private It should_create_it_for_array_of_string =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("ArrayStringProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_byte_arrays =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableByteArrayProperty")).ShouldNotBeNull();

//        private It should_not_create_it_for_enumerable_of_object =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableObjectProperty")).ShouldBeNull();

//        private It should_not_create_it_for_enumerable =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableProperty")).ShouldBeNull();

//        private It should_create_it_for_enumerable_of_bool =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableBoolProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_char =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableCharProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_byte =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableByteProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_short =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableShortProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_ushort =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableUShortProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_int =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableIntProperty")).ShouldNotBeNull();

//        private It should_create_it_for_list_of_int =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("ListIntProperty")).ShouldNotBeNull();

//        private It should_create_it_for_ilist_of_int =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("IListIntProperty")).ShouldNotBeNull();

//        private It should_create_it_for_array_of_int =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("ArrayIntProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_uint =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableUIntProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_long =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableLongProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_ulong =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableULongProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_float =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableFloatProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_double =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableDoubleProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_decimal =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableDecimalProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_enum =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableEnumProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_guid =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableGuidProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_datetime =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableDateTimeProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_datetimeoffset =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableDateTimeOffsetProperty")).ShouldNotBeNull();

//        private It should_create_it_for_enumerable_of_timespan =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("EnumerableTimeSpanProperty")).ShouldNotBeNull();

//        private It should_not_create_it_for_indexer =
//            () => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("Item")).ShouldBeNull();

//        class TestClass
//        {
//            public bool BoolProperty { get; set; }
//            public char CharProperty { get; set; }
//            public byte ByteProperty { get; set; }
//            public short ShortProperty { get; set; }
//            public ushort UShortProperty { get; set; }
//            public int IntProperty { get; set; }
//            public uint UIntProperty { get; set; }
//            public long LongProperty { get; set; }
//            public ulong ULongProperty { get; set; }
//            public float FloatProperty { get; set; }
//            public double DoubleProperty { get; set; }
//            public decimal DecimalProperty { get; set; }
//            public TestEnum EnumProperty { get; set; }
//            public Guid GuidProperty { get; set; }
//            public DateTime DateTimeProperty { get; set; }
//            public DateTimeOffset DateTimeOffsetProperty { get; set; }
//            public TimeSpan TimeSpanProperty { get; set; }
//            public string StringProperty { get; set; }
//            public byte[] ByteArrayProperty { get; set; }
//            public object ObjectProperty { get; set; }
//            public TestClass ClassProperty { get; set; }
//            public TestStruct StructProperty { get; set; }
//            public IEnumerable<string> EnumerableStringProperty { get; set; }
//            public List<string> ListStringProperty { get; set; }
//            public IList<string> IListStringProperty { get; set; }
//            public string[] ArrayStringProperty { get; set; }
//            public IEnumerable<byte[]> EnumerableByteArrayProperty { get; set; }
//            public IEnumerable<bool> EnumerableBoolProperty { get; set; }
//            public IEnumerable<char> EnumerableCharProperty { get; set; }
//            public IEnumerable<byte> EnumerableByteProperty { get; set; }
//            public IEnumerable<short> EnumerableShortProperty { get; set; }
//            public IEnumerable<ushort> EnumerableUShortProperty { get; set; }
//            public IEnumerable<int> EnumerableIntProperty { get; set; }
//            public List<int> ListIntProperty { get; set; }
//            public IList<int> IListIntProperty { get; set; }
//            public int[] ArrayIntProperty { get; set; }
//            public IEnumerable<uint> EnumerableUIntProperty { get; set; }
//            public IEnumerable<long> EnumerableLongProperty { get; set; }
//            public IEnumerable<ulong> EnumerableULongProperty { get; set; }
//            public IEnumerable<float> EnumerableFloatProperty { get; set; }
//            public IEnumerable<double> EnumerableDoubleProperty { get; set; }
//            public IEnumerable<decimal> EnumerableDecimalProperty { get; set; }
//            public IEnumerable<TestEnum> EnumerableEnumProperty { get; set; }
//            public IEnumerable<Guid> EnumerableGuidProperty { get; set; }
//            public IEnumerable<DateTime> EnumerableDateTimeProperty { get; set; }
//            public IEnumerable<DateTimeOffset> EnumerableDateTimeOffsetProperty { get; set; }
//            public IEnumerable<TimeSpan> EnumerableTimeSpanProperty { get; set; }
//            public IEnumerable<object> EnumerableObjectProperty { get; set; }
//            public IEnumerable EnumerableProperty { get; set; }
//            public IEnumerable<int> this[int index]
//            {
//                get { return null; }
//                set { }
//            }
//        }

//        enum TestEnum
//        {
//        }

//        struct TestStruct
//        {
//        }
//    }

//    [Subject(typeof(SimpleCollectionPropertyConverter<>))]
//    class when_trying_to_create_convert_collection_property_for_null_property_info
//    {
//        static Exception exception;

//        Because of =
//            () => exception = Catch.Exception(() => SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(null));

//        It should_throw_argument_null_exception =
//            () => exception.ShouldBeOfType<ArgumentNullException>();

//        It should_report_info_argument =
//            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("info");
//    }

//    [Subject(typeof(SimpleCollectionPropertyConverter<>))]
//    class when_convert_collection_property_is_used_on_null_instance
//    {
//        static IConverter converter;
//        static NameValueCollection result;

//        Establish context =
//            () => converter = SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("Values"));

//        Because of =
//            () => result = converter.Convert(null);

//        private It should_return_empty_collection =
//            () => result.ShouldBeEmpty();

//        class TestClass
//        {
//            public IEnumerable<int> Values { get; set; }
//        }
//    }

//    [Subject(typeof(SimpleCollectionPropertyConverter<>))]
//    class when_convert_collection_property_is_used_on_null_property
//    {
//        static TestClass instance;
//        static IConverter converter;
//        static NameValueCollection result;

//        Establish context = () =>
//        {
//            instance = new TestClass();
//            converter = SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("Values"));
//        };

//        Because of =
//            () => result = converter.Convert(instance);

//        private It should_return_empty_collection =
//            () => result.ShouldBeEmpty();

//        class TestClass
//        {
//            public IEnumerable<int> Values { get; set; }
//        }
//    }

//    [Subject(typeof(SimpleCollectionPropertyConverter<>))]
//    class when_convert_collection_property_is_used
//    {
//        static TestClass instance;
//        static IConverter converter;
//        static NameValueCollection result;

//        Establish context = () =>
//        {
//            instance = new TestClass
//            {
//                Values = new[] { 27, 42 }
//            };

//            converter = SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("Values"));
//        };

//        Because of =
//            () => result = converter.Convert(instance);

//        It should_be_able_to_get_the_key_as_property_name =
//            () => result.First().Key.ShouldEqual("Values");

//        It should_be_able_to_get_value_of_property =
//            () => result.First().Value.ShouldContainOnly("27", "42");

//        class TestClass
//        {
//            public IEnumerable<int> Values { get; set; }
//        }
//    }

//    [Subject(typeof(SimpleCollectionPropertyConverter<>))]
//    class when_convert_collection_property_is_used_on_collection_property_where_some_values_are_null
//    {
//        static TestClass instance;
//        static IConverter converter;
//        static NameValueCollection result;

//        Establish context = () =>
//        {
//            instance = new TestClass
//            {
//                Values = new[] { null, "Hello", null }
//            };

//            converter = SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("Values"));
//        };

//        Because of =
//            () => result = converter.Convert(instance);

//        It should_be_able_to_get_the_key_as_property_name =
//            () => result.First().Key.ShouldEqual("Values");

//        It should_be_able_to_get_value_of_property =
//            () => result.First().Value.ShouldContainOnly("", "Hello", "");

//        class TestClass
//        {
//            public IEnumerable<string> Values { get; set; }
//        }
//    }

//    [Subject(typeof(SimpleCollectionPropertyConverter<>))]
//    class when_convert_collection_property_is_used_together_with_query_parameter_where_name_and_format_are_not_set
//    {
//        static TestClass instance;
//        static IConverter converter;
//        static NameValueCollection result;

//        Establish context = () =>
//        {
//            instance = new TestClass
//            {
//                Values = new[] { 27, 42 }
//            };

//            converter = SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("Values"));
//        };

//        Because of =
//            () => result = converter.Convert(instance);

//        It should_be_able_to_get_the_key_as_property_name =
//            () => result.First().Key.ShouldEqual("Values");

//        It should_be_able_to_get_value_of_property =
//            () => result.First().Value.ShouldContainOnly("27", "42");

//        class TestClass
//        {
//            [RequestQuery]
//            public IEnumerable<int> Values { get; set; }
//        }
//    }

//    [Subject(typeof(SimpleCollectionPropertyConverter<>))]
//    class when_convert_collection_property_is_used_on_property_which_name_is_redefined_using_query_parameter_attribute
//    {
//        static TestClass instance;
//        static IConverter converter;
//        static NameValueCollection result;

//        Establish context = () =>
//        {
//            instance = new TestClass
//            {
//                Values = new[] { 27, 42 }
//            };

//            converter = SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("Values"));
//        };

//        Because of =
//            () => result = converter.Convert(instance);

//        It should_be_able_to_get_the_key_as_the_redefined_name =
//            () => result.First().Key.ShouldEqual("Hello World");

//        It should_be_able_to_get_value_of_property =
//            () => result.First().Value.ShouldContainOnly("27", "42");

//        class TestClass
//        {
//            [RequestQuery(Name = "Hello World")]
//            public IEnumerable<int> Values { get; set; }
//        }
//    }

//    [Subject(typeof(SimpleCollectionPropertyConverter<>))]
//    class when_convert_collection_property_is_used_on_property_with_custom_format_applied
//    {
//        static TestClass instance;
//        static IConverter converter;
//        static NameValueCollection result;

//        Establish context = () =>
//        {
//            instance = new TestClass
//            {
//                Values = new[] { 27, 42 }
//            };

//            converter = SimpleCollectionPropertyConverter<InRequestQueryAttribute>.TryCreate(typeof(TestClass).GetProperty("Values"));
//        };

//        Because of =
//            () => result = converter.Convert(instance);

//        It should_be_able_to_get_the_key_as_the_redefined_name =
//            () => result.First().Key.ShouldEqual("Values");

//        It should_be_able_to_get_value_of_property =
//            () => result.First().Value.ShouldContainOnly("1B", "2A");

//        class TestClass
//        {
//            [RequestQuery(Format = "X")]
//            public IEnumerable<int> Values { get; set; }
//        }
//    }

//    // ReSharper restore UnusedParameter.Local
//    // ReSharper restore ValueParameterNotUsed
//    // ReSharper restore InconsistentNaming
//    // ReSharper restore UnusedMember.Local
//    // ReSharper restore UnusedAutoPropertyAccessor.Local
//}
