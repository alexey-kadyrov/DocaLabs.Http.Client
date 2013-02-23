using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [Subject(typeof(HttpClientEndpointSection))]
    class when_loading_from_file_service_where_all_data_is_defined
    {
        static HttpClientEndpointSection section;
        static HttpClientEndpointElement endpoint;

        Establish context =
            () => section = HttpClientEndpointSection.GetDefaultSection();

        Because of =
            () => endpoint = section.Endpoints["service1"];

        It should_load_enpoint_parameters =
            () => endpoint.ShouldMatch(x => x.Name == "service1" && x.BaseUrl.ToString() == "http://foo.bar/" && x.Timeout == 1000 &&
                                        x.AutoSetAcceptEncoding == false && x.AuthenticationLevel.GetValueOrDefault() == AuthenticationLevel.MutualAuthRequired);
        
        It should_load_credentials =
            () => endpoint.Credentials.ShouldMatch(x => x.CredentialsType == CredentialsType.NetworkCredential && x.User == "user1" &&
                                        x.Password == "password1" && x.Domain == "domain1");

        It should_load_headers =
            () => endpoint.Headers.ShouldMatch(x => x.Count == 1 && x["x1"].Value == "v1");

        It should_load_client_certificates =
            () => endpoint.ClientCertificates.ShouldMatch(x => x.Count == 1 && x[0].StoreName == StoreName.My && x[0].StoreLocation == StoreLocation.LocalMachine
                && x[0].X509FindType == X509FindType.FindBySubjectName && x[0].FindValue == "some");

        It should_load_proxy_properties =
            () => endpoint.Proxy.Address.ToString().ShouldEqual("http://contoso.com/");

        It should_load_proxy_credentials =
            () => endpoint.Proxy.Credentials.ShouldMatch(x => x.CredentialsType == CredentialsType.NetworkCredential && x.User == "user2" &&
                                        x.Password == "password2" && x.Domain == "domain2");
    }

    [Subject(typeof(HttpClientEndpointSection))]
    class when_loading_from_file_service_where_only_some_data_is_defined
    {
        static HttpClientEndpointSection section;
        static HttpClientEndpointElement endpoint;

        Establish context =
            () => section = HttpClientEndpointSection.GetDefaultSection();

        Because of =
            () => endpoint = section.Endpoints["service2"];

        It should_have_base_url_set_to_null =
            () => endpoint.BaseUrl.ShouldBeNull();

        It should_have_method_set_to_blank_string =
            () => endpoint.Method.ShouldBeEmpty();

        It should_have_timeout_set_to_configured_value =
            () => endpoint.Timeout.ShouldEqual(2000);

        It should_have_auto_set_accept_encoding =
            () => endpoint.AutoSetAcceptEncoding.ShouldBeTrue();

        It should_have_authentication_level_set_to_null =
            () => endpoint.AuthenticationLevel.ShouldBeNull();

        It should_load_empty_credentials =
            () => endpoint.Credentials.CredentialsType.ShouldEqual(CredentialsType.None);

        It should_load_empty_headers =
            () => endpoint.Headers.ShouldBeEmpty();

        It should_load_empty_client_certificates =
            () => endpoint.ClientCertificates.ShouldBeEmpty();

        It should_load_null_proxy =
            () => endpoint.Proxy.Address.ShouldBeNull();

        It should_load_empty_proxy_credentials =
            () => endpoint.Proxy.Credentials.CredentialsType.ShouldEqual(CredentialsType.None);
    }

    [Subject(typeof(HttpClientEndpointSection))]
    class when_trying_to_get_section_by_null_section_name
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => HttpClientEndpointSection.GetSection(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_section_name_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("sectionName");
    }
}
