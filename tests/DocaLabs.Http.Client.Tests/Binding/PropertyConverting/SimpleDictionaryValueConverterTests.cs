using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;
using Machine.Specifications.Annotations;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_null_value
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of =
            () => result = converter.Convert(null);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "key27", "27" }, 
           { "key42", "42" }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_dictionary_with_mixed_types_some_of_which_are_not_simple
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "key27", 27 }, 
           { "key42", "42" },
           { 3, new object() },
           { 4, "value4" },
           { new object(), "hello" },
           { new object(), new object() },
           { 6, 66 }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42", "Values.4", "Values.6");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");

        It should_be_able_to_convert_third_value =
            () => result.GetValues("Values.4").ShouldContainOnly("value4");

        It should_be_able_to_convert_forth_value =
            () => result.GetValues("Values.6").ShouldContainOnly("66");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_dictionary_with_mixed_simple_types
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "key27", 27 }, 
           { "key42", "42" },
           { 3, "value3" },
           { 4, 44 }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42", "Values.3", "Values.4");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");

        It should_be_able_to_convert_third_value =
            () => result.GetValues("Values.3").ShouldContainOnly("value3");

        It should_be_able_to_convert_forth_value =
            () => result.GetValues("Values.4").ShouldContainOnly("44");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_generic_dictionary_with_mixed_types_some_of_which_are_not_simple
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Dictionary<object, object>
        {
           { "key27", 27 }, 
           { "key42", "42" },
           { 3, new object() },
           { 4, "value4" },
           { new object(), "hello" },
           { new object(), new object() },
           { 6, 66 }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42", "Values.4", "Values.6");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");

        It should_be_able_to_convert_third_value =
            () => result.GetValues("Values.4").ShouldContainOnly("value4");

        It should_be_able_to_convert_forth_value =
            () => result.GetValues("Values.6").ShouldContainOnly("66");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_generic_dictionary_with_non_simple_keys
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Dictionary<TestClass, int>
        {
           { new TestClass { Value = "1" }, 27 }, 
           { new TestClass { Value = "2" }, 42 }
        });

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public string Value { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_generic_fictionary_with_non_simple_values
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Dictionary<int, TestClass>
        {
           { 11, new TestClass { Value = "1" } }, 
           { 22, new TestClass { Value = "2" } }
        });

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();

        class TestClass
        {
            public string Value { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_generic_dictionary
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Dictionary<string, string>
        {
           { "key27", "27" }, 
           { "key42", "42" }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_generic_dictionary_which_does_not_implement_idictionary
    {
        static IValueConverter converter;
        static IDictionary<string, string> source;
        static NameValueCollection result;

        Establish context = () =>
        {
            converter = new SimpleDictionaryValueConverter("Values", null);
            source = new Dictionary<string, string>
            {
                { "key27", "27" },
                { "key42", "42" }
            };
        };

        Because of = () => result = converter.Convert(new TestDictionary<string, string>(source));

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");

        class TestDictionary<TKey, TValue> : IDictionary<TKey, TValue>
        {
            readonly IDictionary<TKey, TValue> _wrappedDictionary;

            [UsedImplicitly]
            public TValue this[int index]
            {
                get
                {
                    var i = 0;
                    foreach (var key in Keys)
                    {
                        if (i++ == index)
                            return _wrappedDictionary[key];
                    }

                    throw new ArgumentOutOfRangeException("index");
                }

                set
                {
                    {
                        var i = 0;
                        foreach (var key in Keys)
                        {
                            if (i++ == index)
                            {
                                _wrappedDictionary[key] = value;
                                return;
                            }
                        }

                        throw new ArgumentOutOfRangeException("index");
                    }
                }
            }

            public TestDictionary(IDictionary<TKey, TValue> wrappedDictionary)
            {
                _wrappedDictionary = wrappedDictionary;
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return _wrappedDictionary.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(KeyValuePair<TKey, TValue> item)
            {
                _wrappedDictionary.Add(item);
            }

            public void Clear()
            {
                _wrappedDictionary.Clear();
            }

            public bool Contains(KeyValuePair<TKey, TValue> item)
            {
                return _wrappedDictionary.Contains(item);
            }

            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            {
                _wrappedDictionary.CopyTo(array, arrayIndex);
            }

            public bool Remove(KeyValuePair<TKey, TValue> item)
            {
                return _wrappedDictionary.Remove(item);
            }

            public int Count { get { return _wrappedDictionary.Count; } }
            public bool IsReadOnly { get { return _wrappedDictionary.IsReadOnly; } }

            public bool ContainsKey(TKey key)
            {
                return _wrappedDictionary.ContainsKey(key);
            }

            public void Add(TKey key, TValue value)
            {
                _wrappedDictionary.Add(key, value);
            }

            public bool Remove(TKey key)
            {
                return _wrappedDictionary.Remove(key);
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                return _wrappedDictionary.TryGetValue(key, out value);
            }

            public TValue this[TKey key]
            {
                get { return _wrappedDictionary[key]; }
                set { _wrappedDictionary[key] = value; }
            }

            public ICollection<TKey> Keys { get { return _wrappedDictionary.Keys; } }
            public ICollection<TValue> Values { get { return _wrappedDictionary.Values; } }
        }
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_non_string_generic_dictionary
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Dictionary<int, int>
        {
           { 1, 101 }, 
           { 2, 102 },
           { 3, 103 },
           { 4, 104 }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.1", "Values.2", "Values.3", "Values.4");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("Values.1").ShouldContainOnly("101");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.2").ShouldContainOnly("102");

        It should_be_able_to_convert_third_value =
            () => result.GetValues("Values.3").ShouldContainOnly("103");

        It should_be_able_to_convert_forth_value =
            () => result.GetValues("Values.4").ShouldContainOnly("104");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_with_null_name
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter(null, null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "key27", "27" }, 
           { "key42", "42" }
        });

        It should_be_able_to_get_the_key_using_the_source_key =
            () => result.AllKeys.ShouldContainOnly("key27", "key42");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("key42").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_with_empty_name
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("", null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "key27", "27" }, 
           { "key42", "42" }
        });

        It should_be_able_to_get_the_key_using_the_source_key =
            () => result.AllKeys.ShouldContainOnly("key27", "key42");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("key27").ShouldContainOnly("27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("key42").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_on_collection_value_where_some_values_are_null
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter(null, null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "null1", null }, 
           { "key27", "27" }, 
           { "null2", null }
        });

        It should_be_able_to_get_the_key_using_the_source_key =
            () => result.AllKeys.ShouldContainOnly("null1", "key27", "null2");

        It should_be_able_to_convert_first_value =
            () => result.GetValues("null1").ShouldContainOnly("");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("key27").ShouldContainOnly("27");

        It should_be_able_to_convert_third_value =
            () => result.GetValues("null2").ShouldContainOnly("");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_collection_where_some_keys_are_empty
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter(null, null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "", "27" }, 
           { "key42", "42" }
        });

        It should_be_able_to_get_only_non_empty_keys =
            () => result.AllKeys.ShouldContainOnly("key42");

        It should_be_able_to_convert_value_with_non_empty_key =
            () => result.GetValues("key42").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_simple_dictionary_value_converter_is_used_with_non_empty_name_and_collection_where_some_keys_are_empty
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleDictionaryValueConverter("Values", null);

        Because of = () => result = converter.Convert(new Hashtable
        {
           { "", "27" }, 
           { "key42", "42" }
        });

        It should_be_able_to_get_only_non_empty_keys =
            () => result.AllKeys.ShouldContainOnly("Values.key42");

        It should_be_able_to_convert_value_with_non_empty_key =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleDictionaryValueConverter))]
    class when_checking_whenever_simple_dictionary_value_converter_can_convert_the_type
    {
        It should_return_false_for_bool =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(bool)).ShouldBeFalse();

        It should_return_false_for_byte =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(byte)).ShouldBeFalse();

        It should_return_false_for_char =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(char)).ShouldBeFalse();

        It should_return_false_for_short =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(short)).ShouldBeFalse();

        It should_return_false_for_ushort =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(ushort)).ShouldBeFalse();

        It should_return_false_for_int =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(int)).ShouldBeFalse();

        It should_return_false_for_uint =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(uint)).ShouldBeFalse();

        It should_return_false_for_long =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(long)).ShouldBeFalse();

        It should_return_false_for_ulong =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(ulong)).ShouldBeFalse();

        It should_return_false_for_float =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(float)).ShouldBeFalse();

        It should_return_false_for_double =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(double)).ShouldBeFalse();

        It should_return_false_for_decimal =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(decimal)).ShouldBeFalse();

        It should_return_false_for_guid =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(Guid)).ShouldBeFalse();

        It should_return_false_for_date_time =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(DateTime)).ShouldBeFalse();

        It should_return_false_for_date_time_offset =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(DateTimeOffset)).ShouldBeFalse();

        It should_return_false_for_time_span =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(TimeSpan)).ShouldBeFalse();

        It should_return_false_for_enum =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(TestEnum)).ShouldBeFalse();

        It should_return_false_for_string =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(string)).ShouldBeFalse();

        It should_return_false_for_byte_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(byte[])).ShouldBeFalse();

        It should_return_false_for_bool_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(bool[])).ShouldBeFalse();

        It should_return_false_for_char_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(char[])).ShouldBeFalse();

        It should_return_false_for_short_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(short[])).ShouldBeFalse();

        It should_return_false_for_ushort_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(ushort[])).ShouldBeFalse();

        It should_return_false_for_int_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(int[])).ShouldBeFalse();

        It should_return_false_for_uint_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(uint[])).ShouldBeFalse();

        It should_return_false_for_long_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(long[])).ShouldBeFalse();

        It should_return_false_for_ulong_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(ulong[])).ShouldBeFalse();

        It should_return_false_for_float_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(float[])).ShouldBeFalse();

        It should_return_false_for_double_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(double[])).ShouldBeFalse();

        It should_return_false_for_decimal_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(decimal[])).ShouldBeFalse();

        It should_return_false_for_guid_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(Guid[])).ShouldBeFalse();

        It should_return_false_for_date_time_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(DateTime[])).ShouldBeFalse();

        It should_return_false_for_date_time_offset_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(DateTimeOffset[])).ShouldBeFalse();

        It should_return_false_for_time_span_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(TimeSpan[])).ShouldBeFalse();

        It should_return_false_for_enum_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(TestEnum[])).ShouldBeFalse();

        It should_return_false_for_string_array =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(string[])).ShouldBeFalse();

        It should_return_false_for_struct =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(TestStruct)).ShouldBeFalse();

        It should_return_false_for_class =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(TestClass)).ShouldBeFalse();

        It should_return_false_for_object =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(object)).ShouldBeFalse();

        It should_return_false_for_namevaluecollection =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(NameValueCollection)).ShouldBeFalse();

        It should_return_true_for_idictionary =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(IDictionary)).ShouldBeTrue();

        It should_return_true_for_generic_idictionary =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(IDictionary<int, int>)).ShouldBeTrue();

        It should_return_true_for_hastable =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(Hashtable)).ShouldBeTrue();

        It should_return_true_for_sortedlist =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(SortedList)).ShouldBeTrue();

        It should_return_true_for_dictionary_subclass =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(TestDictionarySubsclass)).ShouldBeTrue();

        It should_return_true_for_genric_dictionary_sub_interface_with_defined_generic_arguments =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(ITestGenericDictionarySubinterface)).ShouldBeTrue();

        It should_return_true_for_genric_dictionary_sub_interface =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(ITestGenericDictionarySubinterface2<int, int>)).ShouldBeTrue();

        It should_return_true_for_genric_dictionary_subclass_with_defined_generic_arguments =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(TestGenericDictionarySubsclass)).ShouldBeTrue();

        It should_return_true_for_genric_dictionary_subclass =
            () => SimpleDictionaryValueConverter.CanConvert(typeof(TestGenericDictionarySubsclass2<int, int>)).ShouldBeTrue();

        enum TestEnum
        {
        }

        struct TestStruct
        {
        }

        class TestClass
        {
        }

        class TestDictionarySubsclass : Hashtable
        {
        }

        interface ITestGenericDictionarySubinterface : IDictionary<int, int>
        {
        }

        interface ITestGenericDictionarySubinterface2<TKey, TValue> : IDictionary<TKey, TValue>
        {
        }

        class TestGenericDictionarySubsclass : Dictionary<int, int>
        {
        }

        class TestGenericDictionarySubsclass2<TKey, TValue> : Dictionary<TKey, TValue>
        {
        }
    }
}
