using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [Subject(typeof(ClientNetworkCredentialElement))]
    class when_netwrok_credentials_are_newed
    {
        static ClientNetworkCredentialElement element;

        Because of =
            () => element = new ClientNetworkCredentialElement();

        It should_have_credentials_type_set_to_none =
            () => element.CredentialType.ShouldEqual(CredentialType.None);

        It should_have_user_set_to_empty_string =
            () => element.User.ShouldBeEmpty();

        It should_have_password_set_to_empty_string =
            () => element.Password.ShouldBeEmpty();

        It should_have_domain_to_empty_string =
            () => element.Domain.ShouldBeEmpty();
    }

    [Subject(typeof(ClientNetworkCredentialElement))]
    class when_changing_value_on_netwrok_credentials_which_is_directly_newed
    {
        static ClientNetworkCredentialElement element;

        Establish context =
            () => element = new ClientNetworkCredentialElement();

        Because of = () =>
        {
            element.CredentialType = CredentialType.DefaultNetworkCredentials;
            element.User = "user1";
            element.Password = "password1";
            element.Domain = "domain1";
        };

        It should_change_credentials_type =
            () => element.CredentialType.ShouldEqual(CredentialType.DefaultNetworkCredentials);

        It should_change_user =
            () => element.User.ShouldBeTheSameAs("user1");

        It should_change_password =
            () => element.Password.ShouldEqual("password1");

        It should_change_domain =
            () => element.Domain.ShouldEqual("domain1");
    }
}
