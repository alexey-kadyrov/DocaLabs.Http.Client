using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(RequestUseAttribute))]
    class when_request_use_attribute_is_newed_with_the_default_constructor
    {
        static RequestUseAttribute attribute;

        Because of =
            () => attribute = new RequestUseAttribute();

        It should_set_the_target_to_url_query =
            () => attribute.Targets.ShouldEqual(RequestUseTargets.UrlQuery);

        It should_set_the_name_to_null =
            () => attribute.Name.ShouldBeNull();

        It should_set_the_format_to_null =
            () => attribute.Format.ShouldBeNull();
    }

    [Subject(typeof(RequestUseAttribute))]
    class when_request_use_attribute_is_newed_with_the_overload_constructor_with_targets
    {
        static RequestUseAttribute attribute;

        Because of =
            () => attribute = new RequestUseAttribute(RequestUseTargets.RequestHeader | RequestUseTargets.UrlPath);

        It should_set_the_target_to_the_specified_targets =
            () => attribute.Targets.ShouldEqual(RequestUseTargets.RequestHeader | RequestUseTargets.UrlPath);

        It should_set_the_name_to_null =
            () => attribute.Name.ShouldBeNull();

        It should_set_the_format_to_null =
            () => attribute.Format.ShouldBeNull();
    }

    [Subject(typeof(RequestUseAttribute))]
    class when_request_use_attribute_is_newed_with_the_overload_constructor_with_targets_and_name
    {
        static RequestUseAttribute attribute;

        Because of =
            () => attribute = new RequestUseAttribute(RequestUseTargets.RequestHeader, "new name");

        It should_set_the_target_to_the_specified_targets =
            () => attribute.Targets.ShouldEqual(RequestUseTargets.RequestHeader);

        It should_set_the_name_to_the_specified_value =
            () => attribute.Name.ShouldEqual("new name");

        It should_set_the_format_to_null =
            () => attribute.Format.ShouldBeNull();
    }
}
