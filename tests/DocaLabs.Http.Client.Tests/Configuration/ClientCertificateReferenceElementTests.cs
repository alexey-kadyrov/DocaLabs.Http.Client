using System.Security.Cryptography.X509Certificates;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [Subject(typeof(ClientCertificateReferenceElement))]
    class when_http_client_certitificate_reference_is_newed
    {
        static ClientCertificateReferenceElement element;

        Because of =
            () => element = new ClientCertificateReferenceElement();

        It should_have_store_name_set_to_my =
            () => element.StoreName.ShouldEqual(StoreName.My);

        It should_have_store_location_set_to_local_machine =
            () => element.StoreLocation.ShouldEqual(StoreLocation.LocalMachine);

        It should_have_find_type_set_to_subject_distingushed_name =
            () => element.X509FindType.ShouldEqual(X509FindType.FindBySubjectDistinguishedName);

        It should_have_find_value_set_to_empty_string =
            () => element.FindValue.ShouldBeEmpty();
    }

    [Subject(typeof(ClientCertificateReferenceElement))]
    class when_changing_value_on_http_client_certitificate_reference_which_is_directly_newed
    {
        static ClientCertificateReferenceElement element;

        Establish context =
            () => element = new ClientCertificateReferenceElement();

        Because of = () =>
        {
            element.StoreName = StoreName.TrustedPeople;
            element.StoreLocation = StoreLocation.CurrentUser;
            element.X509FindType = X509FindType.FindByThumbprint;
            element.FindValue = "some certificate";
        };

        It should_change_store_name =
            () => element.StoreName.ShouldEqual(StoreName.TrustedPeople);

        It should_change_store_location =
            () => element.StoreLocation.ShouldEqual(StoreLocation.CurrentUser);

        It should_change_find_type =
            () => element.X509FindType.ShouldEqual(X509FindType.FindByThumbprint);

        It should_change_find_value =
            () => element.FindValue.ShouldEqual("some certificate");
    }

    [Subject(typeof(ClientCertificateReferenceElement))]
    class when_setting_find_value_on_http_client_certitificate_reference_to_null
    {
        static ClientCertificateReferenceElement element;

        Establish context =
            () => element = new ClientCertificateReferenceElement();

        Because of = 
            () => element.FindValue = null;

        It should_set_find_value_to_empty_string =
            () => element.FindValue.ShouldBeEmpty();
    }
}
