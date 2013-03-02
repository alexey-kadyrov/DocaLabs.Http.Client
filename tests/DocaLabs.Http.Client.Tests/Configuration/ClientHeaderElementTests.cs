using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [Subject(typeof(ClientHeaderElement))]
    class when_changing_value_on_client_header_element_which_is_directly_newed
    {
        static ClientHeaderElement element;

        Establish context = () =>
        {
            element = new ClientHeaderElement { Name = "name-x1" };
        };

        Because of =
            () => element.Value = "h42";

        It should_set_name =
            () => element.Name.ShouldEqual("name-x1");

        It should_change_value =
            () => element.Value.ShouldEqual("h42");
    }
}
