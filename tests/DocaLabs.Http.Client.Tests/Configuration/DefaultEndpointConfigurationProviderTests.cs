using System;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [Subject(typeof(DefaultEndpointConfigurationProvider))]
    class when_loading_from_app_config_file_service_where_all_data_is_defined
    {
        static DefaultEndpointConfigurationProvider provider;
        protected static IClientEndpoint endpoint;

        Establish context =
            () => provider = new DefaultEndpointConfigurationProvider();

        Because of =
            () => endpoint = provider.GetEndpoint("service1");

        Behaves_like<SectionFromAppConfigFileWhereAllDataIsDefined> a_section_from_app_config_file_where_all_data_is_defined;
    }

    [Subject(typeof(DefaultEndpointConfigurationProvider))]
    class when_loading_from_app_config_file_service_where_only_some_data_is_defined
    {
        static DefaultEndpointConfigurationProvider provider;
        protected static IClientEndpoint endpoint;

        Establish context =
            () => provider = new DefaultEndpointConfigurationProvider();

        Because of =
            () => endpoint = provider.GetEndpoint("service2");

        Behaves_like<SectionFromAppConfigFileServiceWhereOnlySomeDataIsDefined> a_section_from_app_config_file_service_where_only_some_data_is_defined;
    }

    [Subject(typeof(DefaultEndpointConfigurationProvider))]
    class when_loading_from_external_config_file_service_where_all_data_is_defined
    {
        static DefaultEndpointConfigurationProvider provider;
        protected static IClientEndpoint endpoint;

        Establish context = () =>
        {
            provider = new DefaultEndpointConfigurationProvider();
            provider.SetSource("ExternalEndpointConfiguration.config");
        };

        Because of =
            () => endpoint = provider.GetEndpoint("service11inExternalConfig");

        Behaves_like<SectionFromExternalConfigFileWhereAllDataIsDefined> a_section_from_external_config_file_where_all_data_is_defined;
    }

    [Subject(typeof(DefaultEndpointConfigurationProvider))]
    class when_loading_from_external_config_file_service_where_only_some_data_is_defined
    {
        static DefaultEndpointConfigurationProvider provider;
        protected static IClientEndpoint endpoint;

        Establish context = () =>
        {
            provider = new DefaultEndpointConfigurationProvider();
            provider.SetSource("ExternalEndpointConfiguration.config");
        };

        Because of =
            () => endpoint = provider.GetEndpoint("service22inExternalConfig");

        Behaves_like<SectionFromExternalConfigFileServiceWhereOnlySomeDataIsDefined> a_section_from_external_config_file_service_where_only_some_data_is_defined;
    }

    [Subject(typeof(DefaultEndpointConfigurationProvider))]
    class when_setting_source_to_null_before_loading_service_where_all_data_is_defined
    {
        static DefaultEndpointConfigurationProvider provider;
        protected static IClientEndpoint endpoint;

        Establish context = () =>
        {
            provider = new DefaultEndpointConfigurationProvider();
            provider.SetSource("ExternalEndpointConfiguration.config");
            provider.SetSource(null);
        };

        Because of =
            () => endpoint = provider.GetEndpoint("service1");

        Behaves_like<SectionFromAppConfigFileWhereAllDataIsDefined> a_section_from_app_config_file_where_all_data_is_defined;
    }

    [Subject(typeof(DefaultEndpointConfigurationProvider))]
    class when_setting_source_to_null_before_loading_service_where_only_some_data_is_defined
    {
        static DefaultEndpointConfigurationProvider provider;
        protected static IClientEndpoint endpoint;

        Establish context = () =>
        {
            provider = new DefaultEndpointConfigurationProvider();
            provider.SetSource("ExternalEndpointConfiguration.config");
            provider.SetSource(null);
        };

        Because of =
            () => endpoint = provider.GetEndpoint("service2");

        Behaves_like<SectionFromAppConfigFileServiceWhereOnlySomeDataIsDefined> a_section_from_app_config_file_service_where_only_some_data_is_defined;
    }

    [Subject(typeof(DefaultEndpointConfigurationProvider))]
    class when_setting_source_to_non_esistent_file
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => new DefaultEndpointConfigurationProvider().SetSource("wrong-file-name.config"));

        It should_throw_file_not_found_exception =
            () => exception.ShouldBeOfType<FileNotFoundException>();

        It should_report_the_file_name =
            () => exception.Message.ShouldContain("wrong-file-name.config");
    }

    [Subject(typeof(DefaultEndpointConfigurationProvider))]
    class when_setting_source_using_invalid_file_name
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => new DefaultEndpointConfigurationProvider().SetSource("invalid:file:name?config"));

        It should_throw_file_not_found_exception =
            () => exception.ShouldBeOfType<FileNotFoundException>();

        It should_report_the_file_name =
            () => exception.Message.ShouldContain("invalid:file:name?config");
    }

    [Subject(typeof(DefaultEndpointConfigurationProvider))]
    class when_loading_from_non_default_section
    {
        static DefaultEndpointConfigurationProvider provider;
        protected static IClientEndpoint endpoint;

        Establish context = () => provider = new DefaultEndpointConfigurationProvider
        {
            SectionName = "customSectionName"
        };

        Because of =
            () => endpoint = provider.GetEndpoint("service4inCustomSectionName");

        It should_keep_modified_section_name =
            () => provider.SectionName.ShouldEqual("customSectionName");

        It should_load_enpoint_parameters =
            () => endpoint.ShouldMatch(x => x.Name == "service4inCustomSectionName" && x.BaseUrl.ToString() == "http://foo.bar/" && x.Timeout == 4000 &&
                                        x.AutoSetAcceptEncoding == false && x.AuthenticationLevel.GetValueOrDefault() == AuthenticationLevel.MutualAuthRequired);

        It should_load_credentials =
            () => endpoint.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user41" &&
                                        x.Password == "password41" && x.Domain == "domain41");

        It should_load_headers =
            () => endpoint.Headers.ShouldMatch(x => x.AllKeys.Length == 1 && x["x4"].Value == "v4");

        It should_load_client_certificates =
            () => endpoint.ClientCertificates.ShouldMatch(x => x.AllKeys.Length == 1 && x[0].StoreName == StoreName.My && x[0].StoreLocation == StoreLocation.LocalMachine
                && x[0].X509FindType == X509FindType.FindBySubjectName && x[0].FindValue == "some");

        It should_load_proxy_properties =
            () => endpoint.Proxy.Address.ToString().ShouldEqual("http://contoso.com/");

        It should_load_proxy_credentials =
            () => endpoint.Proxy.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user42" &&
                                        x.Password == "password42" && x.Domain == "domain42");
    }

    [Subject(typeof(DefaultEndpointConfigurationProvider))]
    class when_setting_section_name_to_null
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => { new DefaultEndpointConfigurationProvider().SectionName = null; });

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("value");
    }

    [Subject(typeof(DefaultEndpointConfigurationProvider))]
    class when_loading_from_non_existent_section
    {
        static DefaultEndpointConfigurationProvider provider;
        protected static IClientEndpoint endpoint;

        Establish context = () => provider = new DefaultEndpointConfigurationProvider
        {
            SectionName = "unknownSectionName"
        };

        Because of =
            () => endpoint = provider.GetEndpoint("service4inCustomSectionName");

        It should_keep_modified_section_name =
            () => provider.SectionName.ShouldEqual("unknownSectionName");

        It should_return_null_endpoint_configuration =
            () => endpoint.ShouldBeNull();
    }

    [Subject(typeof(DefaultEndpointConfigurationProvider))]
    class when_loading_from_non_existent_endpoint_configuration
    {
        static DefaultEndpointConfigurationProvider provider;
        protected static IClientEndpoint endpoint;

        Establish context = () => 
            provider = new DefaultEndpointConfigurationProvider();

        Because of =
            () => endpoint = provider.GetEndpoint("unknownService");

        It should_return_null_endpoint_configuration =
            () => endpoint.ShouldBeNull();
    }

    [Behaviors]
    class SectionFromAppConfigFileWhereAllDataIsDefined
    {
        protected static IClientEndpoint endpoint;

        It should_load_enpoint_parameters =
            () => endpoint.ShouldMatch(x => x.Name == "service1" && x.BaseUrl.ToString() == "http://foo.bar/" && x.Timeout == 1000 &&
                                        x.AutoSetAcceptEncoding == false && x.AuthenticationLevel.GetValueOrDefault() == AuthenticationLevel.MutualAuthRequired);

        It should_load_credentials =
            () => endpoint.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user1" &&
                                        x.Password == "password1" && x.Domain == "domain1");

        It should_load_headers =
            () => endpoint.Headers.ShouldMatch(x => x.AllKeys.Length == 1 && x["x1"].Value == "v1");

        It should_load_client_certificates =
            () => endpoint.ClientCertificates.ShouldMatch(x => x.AllKeys.Length == 1 && x[0].StoreName == StoreName.My && x[0].StoreLocation == StoreLocation.LocalMachine
                && x[0].X509FindType == X509FindType.FindBySubjectName && x[0].FindValue == "some");

        It should_load_proxy_properties =
            () => endpoint.Proxy.Address.ToString().ShouldEqual("http://contoso.com/");

        It should_load_proxy_credentials =
            () => endpoint.Proxy.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user2" &&
                                        x.Password == "password2" && x.Domain == "domain2");
    }

    [Behaviors]
    class SectionFromAppConfigFileServiceWhereOnlySomeDataIsDefined
    {
        protected static IClientEndpoint endpoint;

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
            () => endpoint.Credential.CredentialType.ShouldEqual(CredentialType.None);

        It should_load_empty_headers =
            () => endpoint.Headers.ShouldBeEmpty();

        It should_load_empty_client_certificates =
            () => endpoint.ClientCertificates.ShouldBeEmpty();

        It should_load_null_proxy =
            () => endpoint.Proxy.Address.ShouldBeNull();

        It should_load_empty_proxy_credentials =
            () => endpoint.Proxy.Credential.CredentialType.ShouldEqual(CredentialType.None);
    }

    [Behaviors]
    class SectionFromExternalConfigFileWhereAllDataIsDefined
    {
        protected static IClientEndpoint endpoint;

        It should_load_enpoint_parameters =
            () => endpoint.ShouldMatch(x => x.Name == "service11inExternalConfig" && x.BaseUrl.ToString() == "http://foo.bar/" && x.Timeout == 11000 &&
                                        x.AutoSetAcceptEncoding == false && x.AuthenticationLevel.GetValueOrDefault() == AuthenticationLevel.MutualAuthRequired);

        It should_load_credentials =
            () => endpoint.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user11" &&
                                        x.Password == "password11" && x.Domain == "domain11");

        It should_load_headers =
            () => endpoint.Headers.ShouldMatch(x => x.AllKeys.Length == 1 && x["x11"].Value == "v11");

        It should_load_client_certificates =
            () => endpoint.ClientCertificates.ShouldMatch(x => x.AllKeys.Length == 1 && x[0].StoreName == StoreName.My && x[0].StoreLocation == StoreLocation.LocalMachine
                && x[0].X509FindType == X509FindType.FindBySubjectName && x[0].FindValue == "some");

        It should_load_proxy_properties =
            () => endpoint.Proxy.Address.ToString().ShouldEqual("http://contoso.com/");

        It should_load_proxy_credentials =
            () => endpoint.Proxy.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user22" &&
                                        x.Password == "password22" && x.Domain == "domain22");
    }

    [Behaviors]
    class SectionFromExternalConfigFileServiceWhereOnlySomeDataIsDefined
    {
        protected static IClientEndpoint endpoint;

        It should_have_base_url_set_to_null =
            () => endpoint.BaseUrl.ShouldBeNull();

        It should_have_method_set_to_blank_string =
            () => endpoint.Method.ShouldBeEmpty();

        It should_have_timeout_set_to_configured_value =
            () => endpoint.Timeout.ShouldEqual(22000);

        It should_have_auto_set_accept_encoding =
            () => endpoint.AutoSetAcceptEncoding.ShouldBeTrue();

        It should_have_authentication_level_set_to_null =
            () => endpoint.AuthenticationLevel.ShouldBeNull();

        It should_load_empty_credentials =
            () => endpoint.Credential.CredentialType.ShouldEqual(CredentialType.None);

        It should_load_empty_headers =
            () => endpoint.Headers.ShouldBeEmpty();

        It should_load_empty_client_certificates =
            () => endpoint.ClientCertificates.ShouldBeEmpty();

        It should_load_null_proxy =
            () => endpoint.Proxy.Address.ShouldBeNull();

        It should_load_empty_proxy_credentials =
            () => endpoint.Proxy.Credential.CredentialType.ShouldEqual(CredentialType.None);
    }
}
