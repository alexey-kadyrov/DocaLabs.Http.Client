//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using DocaLabs.Http.Client.Binding.PropertyConverting;
//using Machine.Specifications;

//namespace DocaLabs.Http.Client.Tests.Mapping.Attributes
//{
//    [Subject(typeof(SeparatedCollectionConverterAttribute))]
//    class when_separated_collection_converter_attribute_is_newed
//    {
//        static SeparatedCollectionConverterAttribute attribute;

//        Because of =
//            () => attribute = new SeparatedCollectionConverterAttribute();

//        It should_return_pipe_as_item_separator =
//            () => attribute.Separator.ShouldEqual('|');
//    }

//    [Subject(typeof(SeparatedCollectionConverterAttribute))]
//    class when_separated_collection_converter_attribute_is_used
//    {
//        static PropertyInfo property_info;
//        static SeparatedCollectionConverterAttribute attribute;
//        static TestClass instance;
//        static IPropertyConverter converter;

//        Establish context = () =>
//        {
//            property_info = typeof (TestClass).GetProperty("Countries");
//            attribute = property_info.GetCustomAttribute<SeparatedCollectionConverterAttribute>();
//            instance = new TestClass
//            {
//                Countries = new[] { "IE", "UK" }
//            };
//        };

//        Because of =
//            () => converter = attribute.GetConverter(property_info);

//        It should_return_separated_collection_converter =
//            () => converter.ShouldBeOfType<SeparatedCollectionConverter>();

//        It should_copy_separator_from_the_attribute =
//            () => ((SeparatedCollectionConverter) converter).Separator.ShouldEqual(attribute.Separator);

//        It should_be_able_to_get_the_key_as_property_name =
//            () => converter.Convert(instance).First().Key.ShouldEqual("Countries");

//        It should_be_able_to_get_value_of_property =
//            () => converter.Convert(instance).First().Value[0].ShouldEqual("IE;UK");

//        class TestClass
//        {
//            // ReSharper disable UnusedAutoPropertyAccessor.Local
//            [SeparatedCollectionConverter(Separator = ';')]
//            public IEnumerable<string> Countries { get; set; }
//            // ReSharper restore UnusedAutoPropertyAccessor.Local
//        }
//    }
//}
