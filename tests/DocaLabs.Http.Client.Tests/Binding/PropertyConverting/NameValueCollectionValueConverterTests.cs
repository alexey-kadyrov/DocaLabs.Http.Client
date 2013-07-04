using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_null_value
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("Values");

        Because of =
            () => result = converter.Convert(null);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("Values");

        Because of = () => result = converter.Convert(new NameValueCollection
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

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_collection_with_duplicate_keys
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("Values");

        Because of = () => result = converter.Convert(new NameValueCollection
        {
           { "key27", "27" }, 
           { "key42", "42" },
           { "key27", "2-27" }
        });

        It should_be_able_to_get_the_key_using_specified_name_and_the_source_key =
            () => result.AllKeys.ShouldContainOnly("Values.key27", "Values.key42");

        It should_be_able_to_convert_first_values =
            () => result.GetValues("Values.key27").ShouldContainOnly("27", "2-27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("Values.key42").ShouldContainOnly("42");
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_with_null_name
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter(null);

        Because of = () => result = converter.Convert(new NameValueCollection
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

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_collection_with_duplicate_keys_and_name_is_null
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter(null);

        Because of = () => result = converter.Convert(new NameValueCollection
        {
           { "key27", "27" }, 
           { "key42", "42" },
           { "key27", "2-27" }
        });

        It should_be_able_to_get_the_key_using_the_source_key =
            () => result.AllKeys.ShouldContainOnly("key27", "key42");

        It should_be_able_to_convert_first_values =
            () => result.GetValues("key27").ShouldContainOnly("27", "2-27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("key42").ShouldContainOnly("42");
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_with_empty_name
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("");

        Because of = () => result = converter.Convert(new NameValueCollection
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

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_collection_with_duplicate_keys_and_name_is_empty
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter("");

        Because of = () => result = converter.Convert(new NameValueCollection
        {
           { "key27", "27" }, 
           { "key42", "42" },
           { "key27", "2-27" }
        });

        It should_be_able_to_get_the_key_using_the_source_key =
            () => result.AllKeys.ShouldContainOnly("key27", "key42");

        It should_be_able_to_convert_first_values =
            () => result.GetValues("key27").ShouldContainOnly("27", "2-27");

        It should_be_able_to_convert_second_value =
            () => result.GetValues("key42").ShouldContainOnly("42");
    }

    [Subject(typeof(NameValueCollectionValueConverter))]
    class when_namevalue_collection_value_converter_is_used_on_collection_value_where_some_values_are_null
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new NameValueCollectionValueConverter(null);

        Because of = () => result = converter.Convert(new NameValueCollection
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
}
