using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    // ReSharper disable UnusedMember.Local
    // ReSharper disable ValueParameterNotUsed
    // ReSharper disable UnusedParameter.Local
    // ReSharper disable InconsistentNaming
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    // ReSharper disable MemberCanBePrivate.Local

    [Subject(typeof(PropertyMap))]
    class when_converting_class
    {
        static PropertyMaps maps;
        static TestClass instance;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                IntProperty = 1,
                NullableIntProperty = 42,
                EnumProperty = TestEnum.Large,
                GuidProperty = Guid.NewGuid(),
                StringProperty = "Hello World!",
                ByteArrayProperty = new byte[] { 1, 2, 3, 4 },
                ObjectProperty = new object(),
                ObjectProperty2 = new TestClass2 { StringValue = "Again..., Hello World!" },
                TestClassWithOtherNested = new TestClassWithOtherNested
                {
                    SomeSimpleValue = 44,
                    Class2 = new TestClass2 { StringValue = "Some String" }
                },
                TestClassWithOtherMakingCircularReference = new TestClassWithOtherMakingCircularReference
                {
                    SomeSimpleValue = 57
                },
                StructProperty = new TestStruct { Country = "IE", Price = 12.34M },
                EnumerableStringProperty = new[] { "string1", "string2" },
                EnumerableStringAsSeparatedCollection = new[] { "string12", "string22" },
                ListIntProperty = new List<int> { 75, 68 },
                IListIntProperty = new List<int> { 23, 84 },
                ArrayIntProperty = new[] { 11, 22 },
                EnumerableObjectProperty = new object[] { 77, "string-77", 45, 67M },
                EnumerableProperty = new object[] { "s42", "s43", 45 },
                NameValueCollection = new NameValueCollection
                {
                    {"k1", "v1"}
                },
                NameValueCollectionWithOverridenName = new NameValueCollection
                {
                    {"k11", "v11"}
                },
                NameValueCollectionWithEmptyOverridenName = new NameValueCollection
                {
                    {"k21", "v21"}
                },
                Dictionary = new Dictionary<string, string>
                {
                    {"k31", "v31"}
                },
                DictionaryWithOverridenName = new Dictionary<string, string>
                {
                    {"k41", "v41"}
                },
                DictionaryWithEmptyOverridenName = new Dictionary<string, string>
                {
                    {"k51", "v51"}
                },
                NoGetter = "no-getter",
            };

            instance.SelfClassProperty = instance;
            instance.SelfObjectProperty = instance;
            instance.TestClassWithOtherMakingCircularReference.Class = instance;

            maps = new PropertyMaps(x => true);
        };

        Because of =
            () => result = maps.Parse(instance).Convert(instance);

        It should_convert_all_eligible_properties =
            () => result.Count.ShouldEqual(26);

        It should_not_convert_nullable_int_property_set_to_null =
            () => result["NullableNullIntProperty"].ShouldBeNull();

        It should_not_convert_string_property_set_to_null =
            () => result["NullStringProperty"].ShouldBeNull();

        It should_not_convert_object_property_set_to_null =
            () => result["NullObjectProperty"].ShouldBeNull();

        It should_not_convert_object_property_set_to_plain_object =
            () => result["ObjectProperty"].ShouldBeNull();

        It should_not_convert_object_property_set_to_self =
            () => result["SelfObjectProperty"].ShouldBeNull();

        It should_not_convert_self_property_set_to_self =
            () => result["SelfClassProperty"].ShouldBeNull();

        It should_not_convert_list_property_set_to_null =
            () => result["NullListIntProperty"].ShouldBeNull();

        It should_not_convert_ilist_property_set_to_null =
            () => result["NullIListIntProperty"].ShouldBeNull();

        It should_not_convert_array_property_set_to_null =
            () => result["NullArrayIntProperty"].ShouldBeNull();

        It should_not_convert_property_without_getter =
            () => result["NoGetter"].ShouldBeNull();

        It should_not_convert_indexer_property =
            () => result["Item"].ShouldBeNull();

        It should_not_convert_private_property =
            () => result["PrivateProperty"].ShouldBeNull();

        It should_not_convert_static_property =
            () => result["StaticProperty"].ShouldBeNull();

        It should_convert_int_property =
            () => result["IntProperty"].ShouldEqual("1");

        It should_convert_nullable_int_property_which_is_not_null =
            () => result["NullableIntProperty"].ShouldEqual("42");

        It should_convert_enum_property =
            () => result["EnumProperty"].ShouldEqual("Large");

        It should_convert_guid_property =
            () => Guid.Parse(result["GuidProperty"]).ShouldEqual(instance.GuidProperty);

        It should_convert_int_property_with_default_value =
            () => result["IntPropertyWithDefaultValue"].ShouldEqual("0");

        It should_convert_string_property =
            () => result["StringProperty"].ShouldEqual("Hello World!");

        It should_convert_byte_array_property =
            () => result["ByteArrayProperty"].ShouldEqual(Convert.ToBase64String(new byte[] {1, 2, 3, 4}));

        It should_convert_string_property_of_object_property_set_to_class_honoring_name_override =
            () => result["object--property--2.StringValue"].ShouldEqual("Again..., Hello World!");

        It should_convert_int_property_of_object_property_set_to_class =
            () => result["TestClassWithOtherNested.SomeSimpleValue"].ShouldEqual("44");

        It should_convert_string_property_of_nested_object_of_object_property_set_to_class =
            () => result["TestClassWithOtherNested.Class2.StringValue"].ShouldEqual("Some String");

        It should_convert_int_property_of_class_which_has_circular_reference =
            () => result["TestClassWithOtherMakingCircularReference.SomeSimpleValue"].ShouldEqual("57");

        It should_convert_country_field_of_struct_property =
            () => result["StructProperty.Country"].ShouldEqual("IE");

        It should_convert_price_field_of_struct_property =
            () => result["StructProperty.Price"].ShouldEqual("12.34");

        It should_convert_enumerable_string_property =
            () => result.GetValues("EnumerableStringProperty").ShouldContainOnly("string1", "string2");

        It should_convert_enumerable_string_as_separated_string_property =
            () => result.GetValues("EnumerableStringAsSeparatedCollection").ShouldContainOnly("string12|string22");

        It should_convert_int_list_property =
            () => result.GetValues("ListIntProperty").ShouldContainOnly("75", "68");

        It should_convert_int_ilist_property =
            () => result.GetValues("IListIntProperty").ShouldContainOnly("23", "84");

        It should_convert_int_array_property =
            () => result.GetValues("ArrayIntProperty").ShouldContainOnly("11", "22");

        It should_convert_generic_enumerable_of_objects_property =
            () => result.GetValues("EnumerableObjectProperty").ShouldContainOnly("77", "string-77", "45", "67");

        It should_convert_enumerable_of_objects_property_honoring_separated_collection_converter_attribute =
            () => result.GetValues("EnumerableProperty").ShouldContainOnly("s42,s43,45");

        It should_convert_namevaluecollection_property =
            () => result.GetValues("NameValueCollection.k1").ShouldContainOnly("v1");

        It should_convert_namevaluecollection_with_overriden_name_property =
            () => result.GetValues("N1.k11").ShouldContainOnly("v11");

        It should_convert_namevaluecollection_with_empty_overriden_name_property =
            () => result.GetValues("k21").ShouldContainOnly("v21");

        It should_convert_dictionary_property =
            () => result.GetValues("Dictionary.k31").ShouldContainOnly("v31");

        It should_convert_dictionary_with_overriden_name_property =
            () => result.GetValues("N2.k41").ShouldContainOnly("v41");

        It should_convert_ditionary_with_empty_overriden_name_property =
            () => result.GetValues("k51").ShouldContainOnly("v51");

        class TestClass
        {
            public int IntProperty { get; set; }
            public int? NullableIntProperty { get; set; }
            public int? NullableNullIntProperty { get; set; }
            public TestEnum EnumProperty { get; set; }
            public Guid GuidProperty { get; set; }
            public int IntPropertyWithDefaultValue { get; set; }
            public string StringProperty { get; set; }
            public string NullStringProperty { get; set; }
            public byte[] ByteArrayProperty { get; set; }
            public object NullObjectProperty { get; set; }
            public object ObjectProperty { get; set; }
            [RequestUse(Name = "object--property--2")]
            public object ObjectProperty2 { get; set; }
            public object SelfObjectProperty { get; set; }
            public TestClass SelfClassProperty { get; set; }
            public TestClassWithOtherNested TestClassWithOtherNested { get; set; }
            public TestClassWithOtherMakingCircularReference TestClassWithOtherMakingCircularReference { get; set; }
            public TestStruct StructProperty { get; set; }
            public IEnumerable<string> EnumerableStringProperty { get; set; }
            [SeparatedCollectionConverter]
            public IEnumerable<string> EnumerableStringAsSeparatedCollection { get; set; }
            public List<int> ListIntProperty { get; set; }
            public List<int> NullListIntProperty { get; set; }
            public IList<int> IListIntProperty { get; set; }
            public IList<int> NullIListIntProperty { get; set; }
            public int[] ArrayIntProperty { get; set; }
            public int[] NullArrayIntProperty { get; set; }
            public IEnumerable<object> EnumerableObjectProperty { get; set; }
            [SeparatedCollectionConverter(Separator = ',')]
            public IEnumerable EnumerableProperty { get; set; }
            public NameValueCollection NameValueCollection { get; set; }
            [RequestUse(Name = "N1")]
            public NameValueCollection NameValueCollectionWithOverridenName { get; set; }
            [RequestUse(Name = "")]
            public NameValueCollection NameValueCollectionWithEmptyOverridenName { get; set; }
            public Dictionary<string, string> Dictionary { get; set; }
            [RequestUse(Name = "N2")]
            public Dictionary<string, string> DictionaryWithOverridenName { get; set; }
            [RequestUse(Name = "")]
            public Dictionary<string, string> DictionaryWithEmptyOverridenName { get; set; }
            public string NoGetter { set { } }
            public IEnumerable<int> this[int index]
            {
                get { return null; }
                set { }
            }
            string PrivateProperty { get; set; }
            public static string StaticProperty { get; set; }

            public TestClass()
            {
                PrivateProperty = "private-value";
                StaticProperty = "always-the-same";
            }
        }

        class TestClass2
        {
            public string StringValue { get; set; }
        }

        class TestClassWithOtherNested
        {
            public int SomeSimpleValue { get; set; }
            public TestClass2 Class2 { get; set; }
        }

        class TestClassWithOtherMakingCircularReference
        {
            public int SomeSimpleValue { get; set; }
            public TestClass Class { get; set; }
        }

        enum TestEnum
        {
            Small = 0,
            Large = 1
        }

        struct TestStruct
        {
            public string Country { get; set; }
            public decimal Price { get; set; }
        }
    }

    [Subject(typeof(PropertyMap))]
    class when_converting_namevaluecollection
    {
        static NameValueCollection source;
        static PropertyMaps maps;

    }

    // ReSharper restore MemberCanBePrivate.Local
    // ReSharper restore UnusedAutoPropertyAccessor.Local
    // ReSharper restore InconsistentNaming
    // ReSharper restore UnusedMember.Local
    // ReSharper restore UnusedParameter.Local
    // ReSharper restore ValueParameterNotUsed
}
