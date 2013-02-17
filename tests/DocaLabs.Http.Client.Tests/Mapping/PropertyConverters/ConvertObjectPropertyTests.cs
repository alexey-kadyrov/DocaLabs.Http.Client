using System;
using System.Linq;
using DocaLabs.Http.Client.Mapping;
using DocaLabs.Http.Client.Mapping.Attributes;
using DocaLabs.Http.Client.Mapping.PropertyConverters;
using DocaLabs.Http.Client.Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Mapping.PropertyConverters
{
    // ReSharper disable UnusedMember.Local
    // ReSharper disable ClassNeverInstantiated.Local
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    // ReSharper disable ValueParameterNotUsed
    // ReSharper disable UnusedParameter.Local

    [Subject(typeof(ConvertObjectProperty))]
    class when_trying_to_create_convert_object_property_for_object_properties
    {
        private It should_not_create_it_for_bool =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("BoolProperty")).ShouldBeNull();

        private It should_not_create_it_for_char =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("CharProperty")).ShouldBeNull();

        private It should_not_create_it_for_byte =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("ByteProperty")).ShouldBeNull();

        private It should_not_create_it_for_short =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("ShortProperty")).ShouldBeNull();

        private It should_not_create_it_for_ushort =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("UShortProperty")).ShouldBeNull();

        private It should_not_create_it_for_int =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("IntProperty")).ShouldBeNull();

        private It should_not_create_it_for_uint =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("UIntProperty")).ShouldBeNull();

        private It should_not_create_it_for_long =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("LongProperty")).ShouldBeNull();

        private It should_not_create_it_for_ulong =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("ULongProperty")).ShouldBeNull();

        private It should_not_create_it_for_float =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("FloatProperty")).ShouldBeNull();

        private It should_not_create_it_for_double =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("DoubleProperty")).ShouldBeNull();

        private It should_not_create_it_for_decimal =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("DecimalProperty")).ShouldBeNull();

        private It should_not_create_it_for_enum =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("EnumProperty")).ShouldBeNull();

        private It should_not_create_it_for_guid =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("GuidProperty")).ShouldBeNull();

        private It should_not_create_it_for_datetime =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("DateTimeProperty")).ShouldBeNull();

        private It should_not_create_it_for_datetimeoffset =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("DateTimeOffsetProperty")).ShouldBeNull();

        private It should_not_create_it_for_timespan =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("TimeSpanProperty")).ShouldBeNull();

        private It should_not_create_it_for_string =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("StringProperty")).ShouldBeNull();

        private It should_not_create_it_for_byte_array =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("ByteArrayProperty")).ShouldBeNull();

        private It should_create_it_for_object =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("ObjectProperty")).ShouldNotBeNull();

        private It should_create_it_for_class =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("ClassProperty")).ShouldNotBeNull();

        private It should_create_it_for_struct =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("StructProperty")).ShouldNotBeNull();

        private It should_not_create_it_for_indexer =
            () => ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("Item")).ShouldBeNull();

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
            public TestClass this[int index]
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

    [Subject(typeof(ConvertObjectProperty))]
    class when_trying_to_create_convert_object_property_for_null_property_info
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ConvertObjectProperty.TryCreate(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_info_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("info");
    }

    [Subject(typeof(ConvertObjectProperty))]
    class when_convert_object_property_is_used_on_null_instance
    {
        static IConvertProperty converter;
        static CustomNameValueCollection result;

        Establish context =
            () => converter = ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("Value"));

        Because of =
            () => result = converter.GetValue(null);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public TestPropertyClass Value { get; set; }
        }

        class TestPropertyClass
        {
            public override string ToString()
            {
                return "42";
            }            
        }
    }

    [Subject(typeof(ConvertObjectProperty))]
    class when_convert_object_property_is_used_on_null_property
    {
        static TestClass instance;
        static IConvertProperty converter;
        static CustomNameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass();

            converter = ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.GetValue(instance);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            // ReSharper disable UnusedMember.Local
            public TestPropertyClass Value { get; set; }
            // ReSharper restore UnusedMember.Local
        }

        class TestPropertyClass
        {
            public override string ToString()
            {
                return "42";
            }
        }
    }

    [Subject(typeof(ConvertObjectProperty))]
    class when_convert_object_property_is_used_on_property_of_type_which_does_not_implement_custom_query_mapper
    {
        static TestClass instance;
        static IConvertProperty converter;
        static CustomNameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = new TestPropertyClass() };
            converter = ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.GetValue(instance);

        It should_be_able_to_get_the_key_as_property_name =
            () => result.First().Key.ShouldEqual("Value");

        It should_be_able_to_get_value_of_property_using_to_string_method =
            () => result.First().Value[0].ShouldEqual("42");

        class TestClass
        {
            public TestPropertyClass Value { get; set; }
        }

        class TestPropertyClass
        {
            public override string ToString()
            {
                return "42";
            }
        }
    }

    [Subject(typeof(ConvertObjectProperty))]
    class when_convert_object_property_is_used_on_property_of_type_which_does_not_implement_custom_query_mapper_together_with_query_parameter_where_name_and_format_are_not_set
    {
        static TestClass instance;
        static IConvertProperty converter;
        static CustomNameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = new TestPropertyClass() };
            converter = ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.GetValue(instance);

        It should_be_able_to_get_the_key_as_property_name =
            () => result.First().Key.ShouldEqual("Value");

        It should_be_able_to_get_value_of_property_using_to_string_method =
            () => result.First().Value[0].ShouldEqual("42");

        class TestClass
        {
            [QueryParameter]
            public TestPropertyClass Value { get; set; }
        }

        class TestPropertyClass
        {
            public override string ToString()
            {
                return "42";
            }
        }
    }

    [Subject(typeof(ConvertObjectProperty))]
    class when_convert_object_property_is_used_on_property_of_type_which_does_not_implement_custom_query_mapper_and_which_name_is_redefined_using_query_parameter_attribute
    {
        static TestClass instance;
        static IConvertProperty converter;
        static CustomNameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = new TestPropertyClass() };
            converter = ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.GetValue(instance);

        It should_be_able_to_get_the_key_as_the_redefined_name =
            () => result.First().Key.ShouldEqual("Hello World");

        It should_be_able_to_get_value_of_property_using_to_string_method =
            () => result.First().Value[0].ShouldEqual("42");

        class TestClass
        {
            [QueryParameter(Name = "Hello World")]
            public TestPropertyClass Value { get; set; }
        }

        class TestPropertyClass
        {
            public override string ToString()
            {
                return "42";
            }
        }
    }

    [Subject(typeof(ConvertObjectProperty))]
    class when_convert_object_property_is_used_on_property_of_type_which_does_not_implement_custom_query_mapper_and_custom_format_applied
    {
        static TestClass instance;
        static IConvertProperty converter;
        static CustomNameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = new TestPropertyClass() };
            converter = ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.GetValue(instance);

        It should_be_able_to_get_the_key_as_property_name =
            () => result.First().Key.ShouldEqual("Value");

        It should_be_able_to_get_value_of_property_using_to_string_method_with_format_ignored =
            () => result.First().Value[0].ShouldEqual("42");

        class TestClass
        {
            [QueryParameter(Format = "X")]
            public TestPropertyClass Value { get; set; }
        }

        class TestPropertyClass
        {
            public override string ToString()
            {
                return "42";
            }
        }
    }

    [Subject(typeof(ConvertObjectProperty))]
    class when_convert_object_property_is_used_on_property_of_type_which_implements_custom_query_mapper
    {
        static TestClass instance;
        static IConvertProperty converter;
        static CustomNameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = new TestPropertyClass() };
            converter = ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.GetValue(instance);

        It should_be_able_to_get_the_key_as_returned_by_custom_mapper =
            () => result.First().Key.ShouldEqual("Hello!");

        It should_be_able_to_get_value_of_property_returned_by_custom_mapper =
            () => result.First().Value[0].ShouldEqual("42");

        class TestClass
        {
            public TestPropertyClass Value { get; set; }
        }

        class TestPropertyClass : ICustomQueryMapper
        {
            public CustomNameValueCollection ToParameterDictionary()
            {
                return new CustomNameValueCollection { { "Hello!", new [] { "42" } } };
            }
        }
    }

    [Subject(typeof(ConvertObjectProperty))]
    class when_convert_object_property_is_used_on_property_of_type_which_implements_custom_query_mapper_together_with_query_parameter_where_name_and_format_are_not_set
    {
        static TestClass instance;
        static IConvertProperty converter;
        static CustomNameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = new TestPropertyClass() };
            converter = ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.GetValue(instance);

        It should_be_able_to_get_the_key_as_returned_by_custom_mapper =
            () => result.First().Key.ShouldEqual("Hello!");

        It should_be_able_to_get_value_of_property_returned_by_custom_mapper =
            () => result.First().Value[0].ShouldEqual("42");

        class TestClass
        {
            [QueryParameter]
            public TestPropertyClass Value { get; set; }
        }

        class TestPropertyClass : ICustomQueryMapper
        {
            public CustomNameValueCollection ToParameterDictionary()
            {
                return new CustomNameValueCollection { { "Hello!", new[] { "42" } } };
            }
        }
    }

    [Subject(typeof(ConvertObjectProperty))]
    class when_convert_object_property_is_used_on_property_of_type_which_implements_custom_query_mapper_and_which_name_is_redefined_using_query_parameter_attribute
    {
        static TestClass instance;
        static IConvertProperty converter;
        static CustomNameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = new TestPropertyClass() };
            converter = ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.GetValue(instance);

        It should_be_able_to_get_the_key_as_returned_by_custom_mapper =
            () => result.First().Key.ShouldEqual("Hello!");

        It should_be_able_to_get_value_of_property_returned_by_custom_mapper =
            () => result.First().Value[0].ShouldEqual("42");

        class TestClass
        {
            [QueryParameter(Name = "Will be ignored!")]
            public TestPropertyClass Value { get; set; }
        }

        class TestPropertyClass : ICustomQueryMapper
        {
            public CustomNameValueCollection ToParameterDictionary()
            {
                return new CustomNameValueCollection { { "Hello!", new[] { "42" } } };
            }
        }
    }

    [Subject(typeof(ConvertObjectProperty))]
    class when_convert_object_property_is_used_on_property_of_type_which_implements_custom_query_mapper_and_custom_format_applied
    {
        static TestClass instance;
        static IConvertProperty converter;
        static CustomNameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass { Value = new TestPropertyClass() };
            converter = ConvertObjectProperty.TryCreate(typeof(TestClass).GetProperty("Value"));
        };

        Because of =
            () => result = converter.GetValue(instance);

        It should_be_able_to_get_the_key_as_returned_by_custom_mapper =
            () => result.First().Key.ShouldEqual("Hello!");

        It should_be_able_to_get_value_of_property_returned_by_custom_mapper =
            () => result.First().Value[0].ShouldEqual("42");

        class TestClass
        {
            [QueryParameter(Format = "X")]
            public TestPropertyClass Value { get; set; }
        }

        class TestPropertyClass : ICustomQueryMapper
        {
            public CustomNameValueCollection ToParameterDictionary()
            {
                return new CustomNameValueCollection { { "Hello!", new[] { "42" } } };
            }
        }
    }

    // ReSharper restore ValueParameterNotUsed
    // ReSharper restore UnusedParameter.Local
    // ReSharper restore UnusedMember.Local
    // ReSharper restore ClassNeverInstantiated.Local
    // ReSharper restore UnusedAutoPropertyAccessor.Local
}
