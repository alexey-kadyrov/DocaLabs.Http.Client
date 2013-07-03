using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_on_null_instance
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleValueConverter("Value", null);

        Because of =
            () => result = converter.Convert(null);

        private It should_return_empty_collection =
            () => result.ShouldBeEmpty();
    }

    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_with_null_format
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context = 
            () => converter = new SimpleValueConverter("Value", null);

        Because of =
            () => result = converter.Convert(42);

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_convert_value =
            () => result.GetValues("Value").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_with_empty_format
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleValueConverter("Value", "");

        Because of =
            () => result = converter.Convert(42);

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_convert_value =
            () => result.GetValues("Value").ShouldContainOnly("42");
    }

    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_with_specified_format
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleValueConverter("Value", "{0:X}");

        Because of =
            () => result = converter.Convert(42);

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("Value");

        It should_be_able_to_convert_value =
            () => result.GetValues("Value").ShouldContainOnly("2A");
    }

    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_with_null_name
    {
        // ReSharper disable AssignNullToNotNullAttribute
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleValueConverter(null, null);

        Because of =
            () => result = converter.Convert(42);

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly(new string[] { null });

        It should_be_able_to_convert_value =
            () => result.GetValues(null).ShouldContainOnly("42");
        // ReSharper restore AssignNullToNotNullAttribute
    }

    [Subject(typeof(SimpleValueConverter))]
    class when_simple_value_converter_is_used_with_empty_name
    {
        static IValueConverter converter;
        static NameValueCollection result;

        Establish context =
            () => converter = new SimpleValueConverter("", null);

        Because of =
            () => result = converter.Convert(42);

        It should_be_able_to_get_the_key_using_specified_name =
            () => result.AllKeys.ShouldContainOnly("");

        It should_be_able_to_convert_value =
            () => result.GetValues("").ShouldContainOnly("42");
    }
}
