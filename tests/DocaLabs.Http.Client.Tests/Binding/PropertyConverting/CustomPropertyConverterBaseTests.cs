using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Testing.Common;
using Machine.Specifications;
using Machine.Specifications.Annotations;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(CustomPropertyConverterBase))]
    class when_custome_property_converter_derived_class_is_instantiated_it_initializes_property_information
    {
        static CustomConverter converter;

        Because of =
            () => converter = new CustomConverter(Reflect<Model>.GetPropertyInfo(x => x.IntValue));

        It should_initialize_property_info_with_appropriate_value =
            () => converter.GetPropertyInfo().ShouldBeTheSameAs(Reflect<Model>.GetPropertyInfo(x => x.IntValue));

        class CustomConverter : CustomPropertyConverterBase
        {
            public CustomConverter(PropertyInfo propertyInfo) : base(propertyInfo)
            {
            }

            public override NameValueCollection Convert(object value)
            {
                return new NameValueCollection();
            }

            public PropertyInfo GetPropertyInfo()
            {
                return PropertyInfo;
            }
        }

        class Model
        {
            [UsedImplicitly]
            public string StringValue { get; set; }
            public int IntValue { get; set; }
        }
    }

    [Subject(typeof(CustomPropertyConverterBase))]
    class when_custome_property_converter_derived_class_is_used_to_convert_property
    {
        static Model model;
        static CustomConverter converter;
        static NameValueCollection result;

        Establish context = () =>
        {
            model = new Model
            {
                StringValue = "Hello World!",
                IntValue = 42
            };
            converter = new CustomConverter(Reflect<Model>.GetPropertyInfo(x => x.IntValue));
        };

        Because of =
            () => result = converter.Convert(model, new HashSet<object>());

        It should_extract_property_value_and_pass_it_to_abstract_convert_method =
            () => result.ShouldContainOnly(new NameValue("value", "42"));

        class CustomConverter : CustomPropertyConverterBase
        {
            public CustomConverter(PropertyInfo propertyInfo)
                : base(propertyInfo)
            {
            }

            public override NameValueCollection Convert(object value)
            {
                return new NameValueCollection { { "value", value.ToString() } };
            }
        }

        class Model
        {
            public string StringValue { get; set; }
            public int IntValue { get; set; }
        }
    }
}
