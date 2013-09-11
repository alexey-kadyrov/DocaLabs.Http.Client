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

    [Subject(typeof(PropertyMaps))]
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
                    ObjectWhichBecomesNameValueCollection = new NameValueCollection
                    {
                        { "ok1", "ov1" }  
                    },
                    ObjectWhichBecomesNameValueCollectionWithEmptyBaseName = new NameValueCollection
                    {
                        { "ok1", "ov1" }  
                    },
                    ObjectWhichBecomesDictionary = new Dictionary<string, string>
                    {
                        { "ok2", "ov2" }  
                    },
                    ObjectWhichBecomesDictionaryWithEmptyBaseName = new Dictionary<string, string>
                    {
                        { "ok2", "ov2" }  
                    },
                    ObjectWhichBecomesSimpleCollection = new List<string>
                    {
                        "ov3", "ov4"  
                    },
                    ObjectWhichBecomesSimpleCollectionWithEmptyBaseName = new List<string>
                    {
                        "ov33", "ov44"  
                    },
                    ObjectWhichBecomesSimpleValue = 87.65,
                    ObjectWhichBecomesSimpleValueWithEmptyBaseName = 87.654,
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
                TestClassWithToStringOverridden = new TestClassWithToStringOverridden
                {
                    Width = 512,
                    Height = 1024
                },
                ObjectWhichIsSetToTestClassWithToStringOverridden = new TestClassWithToStringOverridden
                {
                    Width = 640,
                    Height = 480
                },
                NoGetter = "no-getter",
                Nested2WithEmptyBaseName = new NestedClass2
                {
                    Value42 = "42-1"
                },
                Nested2WithOverridenBaseName = new NestedClass2
                {
                    Value42 = "42-2",
                },
                Nested2WithInheritedBaseName = new NestedClass2
                {
                    Value42 = "42-3"
                }
            };

            instance.SelfClassProperty = instance;
            instance.SelfObjectProperty = instance;
            instance.TestClassWithOtherMakingCircularReference.Class = instance;

            maps = new PropertyMaps();
        };

        Because of =
            () => result = maps.Convert(instance, x => true);

        It should_convert_all_eligible_properties =
            () => result.Count.ShouldEqual(39);

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

        It should_convert_object_on_nested_class_property_set_to_name_value_collection =
            () => result["TestClassWithOtherNested.ObjectWhichBecomesNameValueCollection.ok1"].ShouldEqual("ov1");

        It should_convert_object_on_nested_class_property_set_to_name_value_collection_with_empty_base_name =
            () => result["TestClassWithOtherNested.ok1"].ShouldEqual("ov1");

        It should_convert_object_on_nested_class_property_set_to_dictionary =
            () => result["TestClassWithOtherNested.ObjectWhichBecomesDictionary.ok2"].ShouldEqual("ov2");

        It should_convert_object_on_nested_class_property_set_to_dictionary_with_empty_base_name =
            () => result["TestClassWithOtherNested.ok2"].ShouldEqual("ov2");

        It should_convert_object_on_nested_class_property_set_to_simple_collection =
            () => result.GetValues("TestClassWithOtherNested.ObjectWhichBecomesSimpleCollection").ShouldContainOnly("ov3", "ov4");

        It should_convert_object_on_nested_class_property_set_to_simple_collection_with_empty_base_name =
            () => result.GetValues("TestClassWithOtherNested.ObjectWhichBecomesSimpleCollectionWithEmptyBaseName").ShouldContainOnly("ov33", "ov44");

        It should_convert_object_on_nested_class_property_set_to_simple_value =
            () => result["TestClassWithOtherNested.ObjectWhichBecomesSimpleValue"].ShouldEqual("87.65");

        It should_convert_object_on_nested_class_property_set_to_simple_value_with_empty_base_name =
            () => result["TestClassWithOtherNested.ObjectWhichBecomesSimpleValueWithEmptyBaseName"].ShouldEqual("87.654");

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

        It should_convert_dictionary_with_empty_overriden_name_property =
            () => result.GetValues("k51").ShouldContainOnly("v51");

        It should_convert_class_using_specified_format =
            () => result.GetValues("TestClassWithToStringOverridden").ShouldContainOnly("512x1024");

        It should_convert_object_using_specified_format =
            () => result.GetValues("ObjectWhichIsSetToTestClassWithToStringOverridden").ShouldContainOnly("640x480");

        It should_convert_nested_class_properties_with_empty_base_name_using_just_nested_class_property_names =
            () => result.GetValues("Value42").ShouldContainOnly("42-1");

        It should_convert_nested_class_properties_with_overidden_base_name_using_that_name_and_nested_class_property_names =
            () => result.GetValues("OverridenBaseName.Value42").ShouldContainOnly("42-2");

        It should_convert_nested_class_properties_with_inherited_base_name_using_property_name_and_nested_class_property_names =
            () => result.GetValues("Nested2WithInheritedBaseName.Value42").ShouldContainOnly("42-3");

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
            [PropertyOverrides(Name = "object--property--2")]
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
            [PropertyOverrides(Name = "N1")]
            public NameValueCollection NameValueCollectionWithOverridenName { get; set; }
            [PropertyOverrides(Name = "")]
            public NameValueCollection NameValueCollectionWithEmptyOverridenName { get; set; }
            public Dictionary<string, string> Dictionary { get; set; }
            [PropertyOverrides(Name = "N2")]
            public Dictionary<string, string> DictionaryWithOverridenName { get; set; }
            [PropertyOverrides(Name = "")]
            public Dictionary<string, string> DictionaryWithEmptyOverridenName { get; set; }
            [PropertyOverrides(Format = "{0}")]
            public TestClassWithToStringOverridden TestClassWithToStringOverridden { get; set; }
            [PropertyOverrides(Format = "{0}")]
            public object ObjectWhichIsSetToTestClassWithToStringOverridden { get; set; }
            public string NoGetter { set { } }
            public IEnumerable<int> this[int index]
            {
                get { return null; }
                set { }
            }
            string PrivateProperty { get; set; }
            public static string StaticProperty { get; set; }

            [PropertyOverrides(Name = "")]
            public NestedClass2 Nested2WithEmptyBaseName { get; set; }
            [PropertyOverrides(Name = "OverridenBaseName")]
            public NestedClass2 Nested2WithOverridenBaseName { get; set; }
            public NestedClass2 Nested2WithInheritedBaseName { get; set; }

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
            public object ObjectWhichBecomesNameValueCollection { get; set; }
            [PropertyOverrides(Name = "")]
            public object ObjectWhichBecomesNameValueCollectionWithEmptyBaseName { get; set; }
            public object ObjectWhichBecomesDictionary { get; set; }
            [PropertyOverrides(Name = "")]
            public object ObjectWhichBecomesDictionaryWithEmptyBaseName { get; set; }
            public object ObjectWhichBecomesSimpleCollection { get; set; }
            [PropertyOverrides(Name = "")]
            public object ObjectWhichBecomesSimpleCollectionWithEmptyBaseName { get; set; }
            public object ObjectWhichBecomesSimpleValue { get; set; }
            [PropertyOverrides(Name = "")]
            public object ObjectWhichBecomesSimpleValueWithEmptyBaseName { get; set; }
            public TestClass2 Class2 { get; set; }
        }

        class NestedClass2
        {
            public string Value42 { get; set; }
        }

        class TestClassWithOtherMakingCircularReference
        {
            public int SomeSimpleValue { get; set; }
            public TestClass Class { get; set; }
        }

        class TestClassWithToStringOverridden
        {
            public int Width { get; set; }
            public int Height { get; set; }

            public override string ToString()
            {
                return string.Format("{0}x{1}", Width, Height);
            }
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

    [Subject(typeof(PropertyMaps))]
    class when_converting_namevaluecollection
    {
        static NameValueCollection source;
        static NameValueCollection result;
        static PropertyMaps maps;

        Establish context = () =>
        {
            source = new NameValueCollection
            {
                { "k1", "v1" },
                { "k2", "v2" }
            };
            maps = new PropertyMaps();
        };

        Because of =
            () => result = maps.Convert(source, x => true);

        It should_return_all_source_keys =
            () => result.AllKeys.ShouldContainOnly("k1", "k2");

        It should_return_first_value =
            () => result.GetValues("k1").ShouldContainOnly("v1");

        It should_return_second_value =
            () => result.GetValues("k2").ShouldContainOnly("v2");
    }

    [Subject(typeof(PropertyMaps))]
    class when_converting_namevaluecollection_subclass
    {
        static NameValueCollectionSubclass source;
        static NameValueCollection result;
        static PropertyMaps maps;

        Establish context = () =>
        {
            source = new NameValueCollectionSubclass
            {
                { "k1", "v1" },
                { "k2", "v2" }
            };
            maps = new PropertyMaps();
        };

        Because of =
            () => result = maps.Convert(source, x => true);

        It should_return_all_source_keys =
            () => result.AllKeys.ShouldContainOnly("k1", "k2");

        It should_return_first_value =
            () => result.GetValues("k1").ShouldContainOnly("v1");

        It should_return_second_value =
            () => result.GetValues("k2").ShouldContainOnly("v2");

        class NameValueCollectionSubclass : NameValueCollection
        {
        }
    }

    [Subject(typeof(PropertyMaps))]
    class when_converting_generic_dictionary
    {
        static Dictionary<string, int> source;
        static NameValueCollection result;
        static PropertyMaps maps;

        Establish context = () =>
        {
            source = new Dictionary<string, int>
            {
                { "k1", 1 },
                { "k2", 2 }
            };
            maps = new PropertyMaps();
        };

        Because of =
            () => result = maps.Convert(source, x => true);

        It should_return_all_source_keys =
            () => result.AllKeys.ShouldContainOnly("k1", "k2");

        It should_return_first_value =
            () => result.GetValues("k1").ShouldContainOnly("1");

        It should_return_second_value =
            () => result.GetValues("k2").ShouldContainOnly("2");
    }

    [Subject(typeof(PropertyMaps))]
    class when_converting_generic_dictionary_subclass
    {
        static Dictionary<string, int> source;
        static NameValueCollection result;
        static PropertyMaps maps;

        Establish context = () =>
        {
            source = new Dictionary<string, int>
            {
                { "k1", 1 },
                { "k2", 2 }
            };
            maps = new PropertyMaps();
        };

        Because of =
            () => result = maps.Convert(source, x => true);

        It should_return_all_source_keys =
            () => result.AllKeys.ShouldContainOnly("k1", "k2");

        It should_return_first_value =
            () => result.GetValues("k1").ShouldContainOnly("1");

        It should_return_second_value =
            () => result.GetValues("k2").ShouldContainOnly("2");

        class NameValueCollectionSubclass : NameValueCollection
        {
        }
    }

    [Subject(typeof(PropertyMaps))]
    class when_converting_with_null_accept_property_delegate
    {
        static NameValueCollection source;
        static PropertyMaps maps;
        static Exception exception;

        Establish context = () =>
        {
            source = new NameValueCollection
            {
                { "k1", "v1" },
                { "k2", "v2" }
            };
            maps = new PropertyMaps();
        };

        Because of =
            () => exception = Catch.Exception(() => maps.Convert(source, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_accept_property_check_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("acceptPropertyCheck");
    }

    [Subject(typeof(PropertyMaps))]
    class when_converting_null_instance
    {
        static NameValueCollection result;
        static PropertyMaps maps;

        Establish context =
            () => maps = new PropertyMaps();

        Because of =
            () => result = maps.Convert(null, x => true);

        It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(PropertyMaps))]
    class when_converting_class_using_specified_property_checker
    {
        static PropertyMaps maps;
        static TestClass instance;
        static NameValueCollection result;

        Establish context = () =>
        {
            instance = new TestClass
            {
                Property1 = 1,
                Property2 = 2
            };
            maps = new PropertyMaps();
        };

        Because of =
            () => result = maps.Convert(instance, x => x.Name == "Property1");

        It should_convert_only_accepted_properties =
            () => result.AllKeys.ShouldContainOnly("Property1");

        It should_convert_values_of_accepted_properties =
            () => result.GetValues("Property1").ShouldContainOnly("1");

        class TestClass
        {
            public int Property1 { get; set; }
            public int Property2 { get; set; }
        }
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_to_get_model_converter_for_null
    {
        It should_return_null =
            () => PropertyMaps.TryGetDictionaryModelValueConverter(null).ShouldBeNull();
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_to_get_model_converter_for_namevaluecollection
    {
        It should_return_converter =
            () => PropertyMaps.TryGetDictionaryModelValueConverter(new NameValueCollection()).ShouldNotBeNull();
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_to_get_model_converter_for_namevaluecollection_subclass
    {
        It should_return_converter =
            () => PropertyMaps.TryGetDictionaryModelValueConverter(new NameValueCollectionSubclass()).ShouldNotBeNull();

        class NameValueCollectionSubclass : NameValueCollection
        {
        }
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_to_get_model_converter_for_hashtable
    {
        It should_return_converter =
            () => PropertyMaps.TryGetDictionaryModelValueConverter(new Hashtable()).ShouldNotBeNull();
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_to_get_model_converter_for_generic_dictionary
    {
        It should_return_converter =
            () => PropertyMaps.TryGetDictionaryModelValueConverter(new Dictionary<string, string>()).ShouldNotBeNull();
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_to_get_model_converter_for_generic_dictionary_subclass
    {
        It should_return_converter =
            () => PropertyMaps.TryGetDictionaryModelValueConverter(new DictionarySubclass<string, string>()).ShouldNotBeNull();

        class DictionarySubclass<TKey, TValue> : Dictionary<TKey, TValue>
        {
        }
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_to_get_model_converter_for_generic_dictionary_subclass_with_defined_generic_arguments
    {
        It should_return_converter =
            () => PropertyMaps.TryGetDictionaryModelValueConverter(new DictionarySubclass()).ShouldNotBeNull();

        class DictionarySubclass : Dictionary<int, int>
        {
        }
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_check_whenever_model_is_dictionary_for_null
    {
        It should_return_false =
            () => PropertyMaps.IsDictionaryModel(null).ShouldBeFalse();
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_check_whenever_model_is_dictionary_for_object
    {
        It should_return_false =
            () => PropertyMaps.IsDictionaryModel(typeof(object)).ShouldBeFalse();
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_check_whenever_model_is_dictionary_for_namevaluecollection
    {
        It should_return_true =
            () => PropertyMaps.IsDictionaryModel(typeof(NameValueCollection)).ShouldBeTrue();
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_check_whenever_model_is_dictionary_for_namevaluecollection_subclass
    {
        It should_return_true =
            () => PropertyMaps.IsDictionaryModel(typeof(NameValueCollectionSubclass)).ShouldBeTrue();

        class NameValueCollectionSubclass : NameValueCollection
        {
        }
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_to_check_whenever_model_is_dictionary_for_hashtable
    {
        It should_return_true =
            () => PropertyMaps.IsDictionaryModel(typeof(Hashtable)).ShouldBeTrue();
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_to_check_whenever_model_is_dictionary_for_generic_dictionary
    {
        It should_return_true =
            () => PropertyMaps.IsDictionaryModel(typeof(Dictionary<string, string>)).ShouldBeTrue();
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_to_check_whenever_model_is_dictionary_for_generic_dictionary_subclass
    {
        It should_return_true =
            () => PropertyMaps.IsDictionaryModel(typeof(DictionarySubclass<string, string>)).ShouldBeTrue();

        class DictionarySubclass<TKey, TValue> : Dictionary<TKey, TValue>
        {
        }
    }

    [Subject(typeof(PropertyMaps))]
    class when_trying_to_check_whenever_model_is_dictionary_for_generic_dictionary_subclass_with_defined_generic_arguments
    {
        It should_return_true =
            () => PropertyMaps.IsDictionaryModel(typeof(DictionarySubclass)).ShouldBeTrue();

        class DictionarySubclass : Dictionary<int, int>
        {
        }
    }

    // ReSharper restore MemberCanBePrivate.Local
    // ReSharper restore UnusedAutoPropertyAccessor.Local
    // ReSharper restore InconsistentNaming
    // ReSharper restore UnusedMember.Local
    // ReSharper restore UnusedParameter.Local
    // ReSharper restore ValueParameterNotUsed
}
