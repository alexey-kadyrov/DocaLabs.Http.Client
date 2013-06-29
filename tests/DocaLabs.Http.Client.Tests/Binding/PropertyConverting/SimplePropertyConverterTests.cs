﻿using System;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    // ReSharper disable UnusedMember.Local
    // ReSharper disable ValueParameterNotUsed
    // ReSharper disable UnusedParameter.Local

    [Subject(typeof(SimplePropertyConverter))]
    class when_trying_to_create_convert_simple_property
    {
        private It should_create_it_for_bool =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("BoolProperty")).ShouldNotBeNull();

        private It should_create_it_for_char =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("CharProperty")).ShouldNotBeNull();

        private It should_create_it_for_byte =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("ByteProperty")).ShouldNotBeNull();

        private It should_create_it_for_short =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("ShortProperty")).ShouldNotBeNull();

        private It should_create_it_for_ushort =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("UShortProperty")).ShouldNotBeNull();

        private It should_create_it_for_int =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("IntProperty")).ShouldNotBeNull();

        private It should_create_it_for_uint =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("UIntProperty")).ShouldNotBeNull();

        private It should_create_it_for_long =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("LongProperty")).ShouldNotBeNull();

        private It should_create_it_for_ulong =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("ULongProperty")).ShouldNotBeNull();

        private It should_create_it_for_float =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("FloatProperty")).ShouldNotBeNull();

        private It should_create_it_for_double =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("DoubleProperty")).ShouldNotBeNull();

        private It should_create_it_for_decimal =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("DecimalProperty")).ShouldNotBeNull();

        private It should_create_it_for_enum =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("EnumProperty")).ShouldNotBeNull();

        private It should_create_it_for_guid =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("GuidProperty")).ShouldNotBeNull();

        private It should_create_it_for_datetime =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("DateTimeProperty")).ShouldNotBeNull();

        private It should_create_it_for_datetimeoffset =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("DateTimeOffsetProperty")).ShouldNotBeNull();

        private It should_create_it_for_timespan =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("TimeSpanProperty")).ShouldNotBeNull();

        private It should_create_it_for_string =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("StringProperty")).ShouldNotBeNull();

        private It should_create_it_for_byte_array =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("ByteArrayProperty")).ShouldNotBeNull();

        private It should_not_create_it_for_object =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("ObjectProperty")).ShouldBeNull();

        private It should_not_create_it_for_class =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("ClassProperty")).ShouldBeNull();

        private It should_not_create_it_for_struct =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("StructProperty")).ShouldBeNull();

        private It should_not_create_it_for_indexer =
            () => SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Item")).ShouldBeNull();

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
        static IConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));

        Because of =
            () => result = converter.Convert(null);

        private It should_return_empty_collection =
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
        static IConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass();

            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance);

        private It should_return_empty_collection =
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
        static IConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = 42 };
            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance);

        It should_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_get_value_of_property =
            () => result["Value"].ShouldEqual("42");

        class TestClass
        {
            public int Value { get; set; }
        }
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_simple_property_converter_is_used_together_with_use_attribute_where_name_and_format_are_not_set
    {
        static TestClass instance;
        static IConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = 42 };
            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance);

        It should_be_able_to_get_the_key_as_property_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_get_value_of_property =
            () => result["Value"].ShouldEqual("42");

        class TestClass
        {
            [Use(RequestUsage.InQuery)]
            public int Value { get; set; }
        }
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_simple_property_converter_is_used_on_property_which_name_is_redefined_using_use_attribute
    {
        static TestClass instance;
        static IConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = 42 };
            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance);

        It should_be_able_to_get_the_key_as_the_redefined_name =
            () => result.AllKeys.ShouldContainOnly("Hello World");

        It should_be_able_to_get_value_of_property =
            () => result["Hello World"].ShouldEqual("42");

        class TestClass
        {
            [Use(Name = "Hello World")]
            public int Value { get; set; }
        }
    }

    [Subject(typeof(SimplePropertyConverter))]
    class when_simple_property_converter_is_used_on_property_with_custom_format_applied
    {
        static TestClass instance;
        static IConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = 42 };
            converter = SimplePropertyConverter.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.Convert(instance);

        It should_be_able_to_get_the_key_as_the_redefined_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_get_value_of_property =
            () => result["Value"].ShouldEqual("2A");

        class TestClass
        {
            [Use(Format = "{0:X}")]
            public int Value { get; set; }
        }
    }

    // ReSharper restore UnusedParameter.Local
    // ReSharper restore ValueParameterNotUsed
    // ReSharper restore UnusedMember.Local
    // ReSharper restore UnusedAutoPropertyAccessor.Local
}
