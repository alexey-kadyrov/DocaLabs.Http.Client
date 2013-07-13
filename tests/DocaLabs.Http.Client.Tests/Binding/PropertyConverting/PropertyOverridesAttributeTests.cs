using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(PropertyOverridesAttribute))]
    class when_propert_overrides_attribute_is_newed_with_the_default_constructor
    {
        static PropertyOverridesAttribute attribute;

        Because of =
            () => attribute = new PropertyOverridesAttribute();

        It should_set_the_name_to_null =
            () => attribute.Name.ShouldBeNull();

        It should_set_the_format_to_null =
            () => attribute.Format.ShouldBeNull();
    }
}
