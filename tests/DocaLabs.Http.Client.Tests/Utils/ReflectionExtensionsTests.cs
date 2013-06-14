using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DocaLabs.Http.Client.Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Utils
{
    // ReSharper disable UnusedMember.Local

    [Subject(typeof(ReflectionExtensions), "IsSimpleType")]
    class when_checking_for_simple_type
    {
        It should_return_true_for_bool =
            () => typeof (bool).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_byte =
            () => typeof (byte).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_char =
            () => typeof (char).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_short =
            () => typeof (short).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_ushort =
            () => typeof (ushort).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_int =
            () => typeof (int).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_uint =
            () => typeof (uint).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_long =
            () => typeof (long).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_ulong =
            () => typeof (ulong).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_float =
            () => typeof (float).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_double =
            () => typeof (double).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_decimal =
            () => typeof (decimal).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_guid =
            () => typeof (Guid).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_date_time =
            () => typeof (DateTime).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_date_time_offset =
            () => typeof (DateTimeOffset).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_time_span =
            () => typeof (TimeSpan).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_enum =
            () => typeof (TestEnum).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_string =
            () => typeof (string).IsSimpleType().ShouldBeTrue();

        It should_return_true_for_byte_array =
            () => typeof (byte[]).IsSimpleType().ShouldBeTrue();

        It should_return_false_for_bool_array =
            () => typeof (bool[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_char_array =
            () => typeof (char[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_short_array =
            () => typeof (short[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_ushort_array =
            () => typeof (ushort[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_int_array =
            () => typeof (int[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_uint_array =
            () => typeof (uint[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_long_array =
            () => typeof (long[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_ulong_array =
            () => typeof (ulong[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_float_array =
            () => typeof (float[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_double_array =
            () => typeof (double[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_decimal_array =
            () => typeof (decimal[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_guid_array =
            () => typeof (Guid[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_date_time_array =
            () => typeof (DateTime[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_date_time_offset_array =
            () => typeof (DateTimeOffset[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_time_span_array =
            () => typeof (TimeSpan[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_enum_array =
            () => typeof (TestEnum[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_string_array =
            () => typeof (string[]).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_struct =
            () => typeof (TestStruct).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_class =
            () => typeof (TestClass).IsSimpleType().ShouldBeFalse();

        It should_return_false_for_object =
            () => typeof (object).IsSimpleType().ShouldBeFalse();

        enum TestEnum
        {
        }

        struct TestStruct
        {
        }

        class TestClass
        {
        }
    }

    [Subject(typeof(ReflectionExtensions), "IsSimpleType")]
    class when_checking_for_null_simple_type
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ((Type) null).IsSimpleType());

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("type");
    }

    [Subject(typeof(ReflectionExtensions), "GetDefaultValue")]
    class when_getting_default_value_for_a_type
    {
        It should_return_default_value_for_a_primitive_type =
            () => typeof (int).GetDefaultValue().ShouldEqual(0);

        It should_return_default_value_for_a_guid =
            () => typeof(Guid).GetDefaultValue().ShouldEqual(Guid.Empty);

        It should_return_default_value_for_an_enum =
            () => typeof(TestEnum).GetDefaultValue().ShouldEqual(default(TestEnum));

        It should_return_default_value_for_a_struct =
            () => typeof(TestStruct).GetDefaultValue().ShouldEqual(default(TestStruct));

        It should_return_null_for_object =
            () => typeof (object).GetDefaultValue().ShouldBeNull();

        It should_return_null_for_class =
            () => typeof(TestClass).GetDefaultValue().ShouldBeNull();

        It should_return_null_for_string =
            () => typeof(string).GetDefaultValue().ShouldBeNull();

        It should_return_null_for_byte_array =
            () => typeof(byte[]).GetDefaultValue().ShouldBeNull();

        enum TestEnum
        {
        }

        struct TestStruct
        {
        }

        class TestClass
        {
        }
    }

    [Subject(typeof(ReflectionExtensions), "GetDefaultValue")]
    class when_getting_default_value_for_a_null_type
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ((Type)null).GetDefaultValue());

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("type");
    }

    [Subject(typeof(ReflectionExtensions), "IsValidOn")]
    class when_checking_whenever_attribute_is_valid_on_certain_target
    {
        It should_return_true_for_checking_validity_on_class_for_attribute_that_can_be_defined_on_class =
            () => AttributeDataOf<TestClassOnlyAttribute>().IsValidOn(AttributeTargets.Class).ShouldBeTrue();

        It should_return_true_for_checking_validity_on_class_for_attribute_that_can_be_defined_on_class_and_interface =
            () => AttributeDataOf<TestClassAndInterfaceAttribute>().IsValidOn(AttributeTargets.Class).ShouldBeTrue();

        It should_return_true_for_checking_validity_on_interface_for_attribute_that_can_be_defined_on_class_and_interface =
            () => AttributeDataOf<TestClassAndInterfaceAttribute>().IsValidOn(AttributeTargets.Interface).ShouldBeTrue();

        It should_return_true_for_checking_validity_on_interface_and_class_for_attribute_that_can_be_defined_on_class_and_interface =
            () => AttributeDataOf<TestClassAndInterfaceAttribute>().IsValidOn(AttributeTargets.Class | AttributeTargets.Interface).ShouldBeTrue();

        It should_return_false_for_checking_validity_on_interface_and_class_and_property_for_attribute_that_can_be_defined_on_class_and_interface =
            () => AttributeDataOf<TestClassAndInterfaceAttribute>().IsValidOn(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface).ShouldBeFalse();

        It should_return_false_for_checking_validity_on_property_for_attribute_that_can_be_defined_on_class =
            () => AttributeDataOf<TestClassOnlyAttribute>().IsValidOn(AttributeTargets.Property).ShouldBeFalse();

        It should_return_true_for_checking_validity_on_class_for_attribute_that_does_not_have_explicit_attribute_usage =
            () => AttributeDataOf<AtributeWithoutExplicitUsageAttribute>().IsValidOn(AttributeTargets.Class).ShouldBeTrue();

        It should_return_true_for_checking_validity_on_property_for_attribute_that_does_not_have_explicit_attribute_usage =
            () => AttributeDataOf<AtributeWithoutExplicitUsageAttribute>().IsValidOn(AttributeTargets.All).ShouldBeTrue();

        It should_return_true_for_checking_validity_on_all_for_attribute_that_does_not_have_explicit_attribute_usage =
            () => AttributeDataOf<AtributeWithoutExplicitUsageAttribute>().IsValidOn(AttributeTargets.All).ShouldBeTrue();

        class AtributeWithoutExplicitUsageAttribute : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Class)]
        class TestClassOnlyAttribute : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
        class TestClassAndInterfaceAttribute : Attribute
        {
        }

        [TestClassOnly, TestClassAndInterface, AtributeWithoutExplicitUsage]
        class TestClass
        {
        }

        static CustomAttributeData AttributeDataOf<T>() where T : Attribute
        {
            return typeof (TestClass).CustomAttributes.First(x => x.AttributeType == typeof (T));
        }
    }

    [Subject(typeof(ReflectionExtensions), "IsValidOn")]
    class when_checking_whenever_attribute_is_valid_on_certain_target_for_null_attribute
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ((CustomAttributeData)null).IsValidOn(AttributeTargets.Class));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_attribute_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("attribute");
    }

    [Subject(typeof(ReflectionExtensions), "GetAllPublicInstanceProperties")]
    class when_getting_all_public_instance_properties_on_a_null_type
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ((Type)null).GetAllPublicInstanceProperties());

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("type");
    }

    [Subject(typeof(ReflectionExtensions), "GetAllPublicInstanceProperties")]
    class when_getting_all_public_instance_properties_on_a_type
    {
        It should_return_all_requested_properties_for_a_base_interface =
            () => typeof(IInterface1).GetAllPublicInstanceProperties().Count.ShouldEqual(2);

        It should_return_all_requested_properties_for_a_subclassed_interface =
            () => typeof(IInterface2).GetAllPublicInstanceProperties().Count.ShouldEqual(5);

        It should_return_all_requested_properties_for_a_class =
            () => typeof(TestClass).GetAllPublicInstanceProperties().Count.ShouldEqual(5);


        interface IInterface1
        {
            string Property1 { get; set; }
            int this[int i] { get; set; }
        }

        interface IInterface2 : IInterface1
        {
            string Property2 { get; set; }
            int this[int i, int k] { get; set; }
            int this[string i] { get; set; }
        }

        class BaseClass
        {
            public string BaseProperty1 { get; set; }
            protected string BaseProperty2 { get; set; }
            public string Property { get; set; }
        }

        class TestClass : BaseClass, IInterface1
        {
            public new string Property { get; set; }

            public string Property1 { get; set; }

            public int this[int i]
            {
                get { return 0; }
                set { }
            }

            public string Property22 { get; set; }
            public static string Property33 { get; set; }
            string Property44 { get; set; }
            static string Property55 { get; set; }
        }
    }

    [Subject(typeof(ReflectionExtensions), "IsEnumerable")]
    class when_checking_for_an_enumerable_type
    {
        It should_return_false_for_int =
            () => typeof (int).IsEnumerable().ShouldBeFalse();

        It should_return_false_for_byte =
            () => typeof(byte).IsEnumerable().ShouldBeFalse();

        It should_return_false_for_guid =
            () => typeof(Guid).IsEnumerable().ShouldBeFalse();

        It should_return_false_for_object =
            () => typeof(object).IsEnumerable().ShouldBeFalse();

        It should_return_false_for_string =
            () => typeof(string).IsEnumerable().ShouldBeFalse();

        It should_return_false_for_byte_array =
            () => typeof(byte[]).IsEnumerable().ShouldBeFalse();

        It should_return_true_for_string_array =
            () => typeof(string[]).IsEnumerable().ShouldBeTrue();

        It should_return_true_for_int_array =
            () => typeof(int[]).IsEnumerable().ShouldBeTrue();

        It should_return_true_for_non_generic_list =
            () => typeof(ArrayList).IsEnumerable().ShouldBeTrue();

        It should_return_true_for_generic_list =
            () => typeof(List<int>).IsEnumerable().ShouldBeTrue();

        It should_return_true_for_generic_list_definition =
            () => typeof(List<>).IsEnumerable().ShouldBeTrue();

        It should_return_true_for_non_generic_enumerable_interface =
            () => typeof(IEnumerable).IsEnumerable().ShouldBeTrue();

        It should_return_true_for_generic_enumerable_interface =
            () => typeof(IEnumerable<string>).IsEnumerable().ShouldBeTrue();

        It should_return_true_for_generic_enumerable_interface_definition =
            () => typeof(IEnumerable<>).IsEnumerable().ShouldBeTrue();
    }

    [Subject(typeof(ReflectionExtensions), "IsEnumerable")]
    class when_checking_for_an_enumerable_type_on_null_type
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ((Type)null).IsEnumerable());

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("type");
    }

    [Subject(typeof(ReflectionExtensions), "GetEnumerableElementType")]
    class when_getting_an_enumerable_element_type
    {
        It should_return_null_for_int =
            () => typeof(int).GetEnumerableElementType().ShouldBeNull();

        It should_return_null_for_byte =
            () => typeof(byte).GetEnumerableElementType().ShouldBeNull();

        It should_return_null_for_guid =
            () => typeof(Guid).GetEnumerableElementType().ShouldBeNull();

        It should_return_null_for_object =
            () => typeof(object).GetEnumerableElementType().ShouldBeNull();

        It should_return_null_for_string =
            () => typeof(string).GetEnumerableElementType().ShouldBeNull();

        It should_return_null_for_byte_array =
            () => typeof(byte[]).GetEnumerableElementType().ShouldBeNull();

        It should_return_string_for_string_array =
            () => typeof(string[]).GetEnumerableElementType().ShouldEqual(typeof(string));

        It should_return_int_for_int_array =
            () => typeof(int[]).GetEnumerableElementType().ShouldEqual(typeof(int));

        It should_return_object_for_non_generic_list =
            () => typeof(ArrayList).GetEnumerableElementType().ShouldEqual(typeof(object));

        It should_return_int_for_generic_list_of_int =
            () => typeof(List<int>).GetEnumerableElementType().ShouldEqual(typeof(int));

        It should_return_null_for_generic_list_definition =
            () => typeof(List<>).GetEnumerableElementType().ShouldBeNull();

        It should_return_object_for_non_generic_enumerable_interface =
            () => typeof(IEnumerable).GetEnumerableElementType().ShouldEqual(typeof(object));

        It should_return_string_for_generic_enumerable_interface_of_string =
            () => typeof(IEnumerable<string>).GetEnumerableElementType().ShouldEqual(typeof(string));

        It should_return_null_for_generic_enumerable_interface_definition =
            () => typeof(IEnumerable<>).GetEnumerableElementType().ShouldBeNull();
    }

    [Subject(typeof(ReflectionExtensions), "GetEnumerableElementType")]
    class when_getting_an_enumerable_element_type_on_null_type
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => ((Type)null).GetEnumerableElementType());

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_type_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("type");
    }
        
    // ReSharper restore UnusedMember.Local
}
