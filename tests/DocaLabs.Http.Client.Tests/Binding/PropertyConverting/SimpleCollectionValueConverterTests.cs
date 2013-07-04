using System;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_on_null_value
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleCollectionValueConverter("Values", null);

        Because of =
            () => result = converter.Convert(null);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_with_null_format
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context = 
            () => converter = new SimpleCollectionValueConverter("Values", null);

        Because of =
            () => result = converter.Convert(new[] { 27, 42 });

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_convert_value =
            () => result.GetValues("Values").ShouldContainOnly("27", "42");
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_with_empty_format
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleCollectionValueConverter("Values", "");

        Because of =
            () => result = converter.Convert(new[] { 27, 42 });

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_convert_value =
            () => result.GetValues("Values").ShouldContainOnly("27", "42");
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_with_specified_format
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleCollectionValueConverter("Values", "{0:X}");

        Because of =
            () => result = converter.Convert(new[] { 27, 42 });

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_convert_value =
            () => result.GetValues("Values").ShouldContainOnly("1B", "2A");
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_with_null_name
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => new SimpleCollectionValueConverter(null, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_name_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("name");
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_with_empty_name
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => new SimpleCollectionValueConverter("", null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_name_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("name");
    }

    [Subject(typeof(SimpleCollectionValueConverter))]
    class when_simple_collection_value_converter_is_used_on_collection_value_where_some_values_are_null
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context = 
            () => converter = new SimpleCollectionValueConverter("Values", null);

        Because of =
            () => result = converter.Convert(new[] { null, "Hello", null });

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Values");

        It should_be_able_to_convert_value =
            () => result.GetValues("Values").ShouldContainOnly("", "Hello", "");
    }
}
