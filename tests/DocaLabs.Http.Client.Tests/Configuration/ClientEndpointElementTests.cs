using System;
using System.Net.Security;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [Subject(typeof(ClientEndpointElement))]
    class when_http_client_endpoint_is_newed
    {
        static ClientEndpointElement element;

        Because of =
            () => element = new ClientEndpointElement();

        It should_have_name_set_to_empty_string =
            () => element.Name.ShouldBeEmpty();

        It should_have_base_url_set_to_null =
            () => element.BaseUrl.ShouldBeNull();

        It should_have_method_set_to_blank_string =
            () => element.Method.ShouldBeEmpty();

        It should_have_timeout_set_to_90_seconds =
            () => element.Timeout.ShouldEqual(90000);

        It should_have_auto_set_accept_encoding =
            () => element.AutoSetAcceptEncoding.ShouldBeTrue();

        It should_have_authentication_level_set_to_null =
            () => element.AuthenticationLevel.ShouldBeNull();

        It should_have_credentials_set_to_none =
            () => element.Credential.CredentialType.ShouldEqual(CredentialType.None);

        It should_have_empty_collection_of_headers =
            () => element.Headers.ShouldBeEmpty();

        It should_have_empty_collection_of_client_certificates =
            () => element.ClientCertificates.ShouldBeEmpty();

        It should_have_proxy_with_null_address =
            () => element.Proxy.Address.ShouldBeNull();
    }

    [Subject(typeof(ClientEndpointElement))]
    class when_changing_value_on_http_client_endpoint_which_is_directly_newed
    {
        static ClientEndpointElement element;
        static Uri base_url;

        Establish context = () =>
        {
            element = new ClientEndpointElement();
            base_url = new Uri("http://foo.bar/");
        };

        Because of = () =>
        {
            element.Name = "name1";
            element.BaseUrl = base_url;
            element.Method = "PUT";
            element.Timeout = 1;
            element.AutoSetAcceptEncoding = false;
            element.AuthenticationLevel = AuthenticationLevel.MutualAuthRequired;
        };

        It should_change_name =
            () => element.Name.ShouldEqual("name1");

        It should_change_method =
            () => element.Method.ShouldEqual("PUT");

        It should_change_base_url =
            () => element.BaseUrl.ShouldBeTheSameAs(base_url);

        It should_change_timeout =
            () => element.Timeout.ShouldEqual(1);

        It should_change_auto_set_accept_encoding =
            () => element.AutoSetAcceptEncoding.ShouldBeFalse();

        It should_change_authentication_level =
            () => element.AuthenticationLevel.ShouldEqual(AuthenticationLevel.MutualAuthRequired);
    }
}
