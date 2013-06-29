using System;
using System.Collections.Generic;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;
using Machine.Specifications.Annotations;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(SeparatedCollectionConverterAttribute))]
    class when_separated_collection_converter_attribute_is_newed
    {
        static SeparatedCollectionConverterAttribute attribute;

        Because of =
            () => attribute = new SeparatedCollectionConverterAttribute();

        It should_return_pipe_as_item_separator =
            () => attribute.Separator.ShouldEqual('|');
    }

    [Subject(typeof(SeparatedCollectionConverterAttribute))]
    class when_get_converter_of_separated_collection_converter_attribute_is_used
    {
        static SeparatedCollectionConverterAttribute attribute;
        static IConverter converter;

        Establish context = 
            () => attribute = new SeparatedCollectionConverterAttribute();

        Because of =
            () => converter = attribute.GetConverter(typeof(TestClass).GetProperty("Countries"));

        It should_return_separated_collection_converter =
            () => converter.ShouldBeOfType<SeparatedCollectionConverter>();

        It should_copy_separator_from_the_attribute =
            () => ((SeparatedCollectionConverter)converter).Separator.ShouldEqual(attribute.Separator);

        class TestClass
        {
            [SeparatedCollectionConverter(Separator = ';'), UsedImplicitly]
            public IEnumerable<string> Countries { get; set; }
        }
    }

    [Subject(typeof(SeparatedCollectionConverterAttribute))]
    class when_get_converter_of_separated_collection_converter_attribute_is_used_on_non_emumerable_property
    {
        static SeparatedCollectionConverterAttribute attribute;
        static Exception exception;

        Establish context =
            () => attribute = new SeparatedCollectionConverterAttribute();

        Because of =
            () => exception = Catch.Exception(() => attribute.GetConverter(typeof(TestClass).GetProperty("Value")));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_property_argument =
            () => ((ArgumentException) exception).ParamName.ShouldEqual("property");

        class TestClass
        {
            [SeparatedCollectionConverter, UsedImplicitly]
            public int Value { get; set; }
        }
    }
}
