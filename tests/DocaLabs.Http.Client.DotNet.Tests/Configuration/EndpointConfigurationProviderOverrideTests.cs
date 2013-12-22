using System;
using System.IO;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [TestClass]
    public class when_loading_from_app_config_file_service_where_all_data_is_defined 
    {
        static EndpointConfigurationProviderOverride _provider;
        static IClientEndpoint _endpoint;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _provider = new EndpointConfigurationProviderOverride();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _endpoint = _provider.GetEndpoint("service1");
        }

        [TestMethod]
        public void it_should_load_enpoint_parameters()
        {
            _endpoint.ShouldMatch(x => x.Name == "service1" && x.BaseUrl.ToString() == "http://foo.bar/" && x.Timeout == 1000 &&
                                            x.AutoSetAcceptEncoding == false &&
                                            x.AuthenticationLevel == RequestAuthenticationLevel.MutualAuthRequired);
        }

        [TestMethod]
        public void it_should_load_credentials()
        {
            _endpoint.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user1" &&
                                                       x.Password == "password1" && x.Domain == "domain1");
        }

        [TestMethod]
        public void it_should_load_headers()
        {
            _endpoint.Headers.ShouldMatch(x => x.Count == 1 && x[0].Name == "x1" && x[0].Value == "v1");
        }

        [TestMethod]
        public void it_should_load_client_certificates()
        {
            _endpoint.ClientCertificates.ShouldMatch(x => x.Count == 1
                    && x[0].StoreName == CertificateStoreName.My
                    && x[0].StoreLocation == CertificateStoreLocation.LocalMachine
                    && x[0].X509FindType == CertificateX509FindType.FindBySubjectName
                    && x[0].FindValue == "some");
        }

        [TestMethod]
        public void it_should_load_proxy_properties()
        {
            _endpoint.Proxy.Address.ToString().ShouldEqual("http://contoso.com/");
        }

        [TestMethod]
        public void it_should_load_proxy_credentials()
        {
            _endpoint.Proxy.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user2" &&
                                                             x.Password == "password2" && x.Domain == "domain2");
        }
    }

    [TestClass]
    public class when_loading_from_app_config_file_service_where_only_some_data_is_defined
    {
        static EndpointConfigurationProviderOverride _provider;
        static IClientEndpoint _endpoint;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _provider = new EndpointConfigurationProviderOverride();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _endpoint = _provider.GetEndpoint("service2");
        }

        [TestMethod]
        public void it_should_have_base_url_set_to_null()
        {
            _endpoint.BaseUrl.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_have_method_set_to_blank_string()
        {
            _endpoint.Method.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_have_timeout_set_to_configured_value()
        {
            _endpoint.Timeout.ShouldEqual(2000);
        }

        [TestMethod]
        public void it_should_have_auto_set_accept_encoding()
        {
            _endpoint.AutoSetAcceptEncoding.ShouldBeTrue();
        }

        [TestMethod]
        public void it_should_have_authentication_level_set_to_undefined()
        {
            _endpoint.AuthenticationLevel.ShouldEqual(RequestAuthenticationLevel.Undefined);
        }

        [TestMethod]
        public void it_should_load_empty_credentials()
        {
            _endpoint.Credential.CredentialType.ShouldEqual(CredentialType.None);
        }

        [TestMethod]
        public void it_should_load_empty_headers()
        {
            _endpoint.Headers.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_load_empty_client_certificates()
        {
            _endpoint.ClientCertificates.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_load_null_proxy()
        {
            _endpoint.Proxy.Address.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_load_empty_proxy_credentials()
        {
            _endpoint.Proxy.Credential.CredentialType.ShouldEqual(CredentialType.None);
        }
    }

    [TestClass]
    public class when_loading_from_external_config_file_service_where_all_data_is_defined
    {
        static EndpointConfigurationProviderOverride _provider;
        static IClientEndpoint _endpoint;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _provider = new EndpointConfigurationProviderOverride();
            _provider.SetSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExternalEndpointConfiguration.config"));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _endpoint = _provider.GetEndpoint("service11inExternalConfig");
        }

        [TestMethod]
        public void it_should_load_enpoint_parameters()
        {
                _endpoint.ShouldMatch(x => x.Name == "service11inExternalConfig" && x.BaseUrl.ToString() == "http://foo.bar/" 
                        && x.Timeout == 11000 && x.AutoSetAcceptEncoding == false 
                        && x.AuthenticationLevel == RequestAuthenticationLevel.MutualAuthRequired);
        }

        [TestMethod]
        public void it_should_load_credentials()
        {
            _endpoint.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user11" &&
                                                       x.Password == "password11" && x.Domain == "domain11");
        }

        [TestMethod]
        public void it_should_load_headers()
        {
            _endpoint.Headers.ShouldMatch(x => x.Count == 1 && x[0].Name == "x11" && x[0].Value == "v11");
        }

        [TestMethod]
        public void it_should_load_client_certificates()
        {
                _endpoint.ClientCertificates.ShouldMatch(
                    x => x.Count == 1 && x[0].StoreName == CertificateStoreName.My && x[0].StoreLocation == CertificateStoreLocation.LocalMachine
                         && x[0].X509FindType == CertificateX509FindType.FindBySubjectName && x[0].FindValue == "some");
        }

        [TestMethod]
        public void it_should_load_proxy_properties()
        {
            _endpoint.Proxy.Address.ToString().ShouldEqual("http://contoso.com/");
        }

        [TestMethod]
        public void it_should_load_proxy_credentials()
        {
            _endpoint.Proxy.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user22" &&
                                                             x.Password == "password22" && x.Domain == "domain22");
        }
    }

    [TestClass]
    public class when_loading_from_external_config_file_service_where_only_some_data_is_defined
    {
        static EndpointConfigurationProviderOverride _provider;
        static IClientEndpoint _endpoint;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _provider = new EndpointConfigurationProviderOverride();
            _provider.SetSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExternalEndpointConfiguration.config"));

            BecauseOf();
        }

        static void BecauseOf()
        {
            _endpoint = _provider.GetEndpoint("service22inExternalConfig");
        }

        [TestMethod]
        public void it_should_have_base_url_set_to_null()
        {
            _endpoint.BaseUrl.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_have_method_set_to_blank_string()
        {
            _endpoint.Method.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_have_timeout_set_to_configured_value()
        {
            _endpoint.Timeout.ShouldEqual(22000);
        }

        [TestMethod]
        public void it_should_have_auto_set_accept_encoding()
        {
            _endpoint.AutoSetAcceptEncoding.ShouldBeTrue();
        }

        [TestMethod]
        public void it_should_have_authentication_level_set_to_undefined()
        {
            _endpoint.AuthenticationLevel.ShouldEqual(RequestAuthenticationLevel.Undefined);
        }

        [TestMethod]
        public void it_should_load_empty_credentials()
        {
            _endpoint.Credential.CredentialType.ShouldEqual(CredentialType.None);
        }

        [TestMethod]
        public void it_should_load_empty_headers()
        {
            _endpoint.Headers.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_load_empty_client_certificates()
        {
            _endpoint.ClientCertificates.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_load_null_proxy()
        {
            _endpoint.Proxy.Address.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_load_empty_proxy_credentials()
        {
            _endpoint.Proxy.Credential.CredentialType.ShouldEqual(CredentialType.None);
        }
    }

    [TestClass]
    public class when_setting_source_to_null_before_loading_service_where_all_data_is_defined
    {
        static EndpointConfigurationProviderOverride provider;
        static IClientEndpoint endpoint;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            provider = new EndpointConfigurationProviderOverride();
            provider.SetSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExternalEndpointConfiguration.config"));
            provider.SetSource(null);

            BecauseOf();
        }

        static void BecauseOf()
        {
            endpoint = provider.GetEndpoint("service1");
        }

        [TestMethod]
        public void it_should_load_enpoint_parameters()
        {
            endpoint.ShouldMatch(x => x.Name == "service1" && x.BaseUrl.ToString() == "http://foo.bar/" && x.Timeout == 1000 &&
                                            x.AutoSetAcceptEncoding == false &&
                                            x.AuthenticationLevel == RequestAuthenticationLevel.MutualAuthRequired);
        }

        [TestMethod]
        public void it_should_load_credentials()
        {
            endpoint.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user1" &&
                                                       x.Password == "password1" && x.Domain == "domain1");
        }

        [TestMethod]
        public void it_should_load_headers()
        {
            endpoint.Headers.ShouldMatch(x => x.Count == 1 && x[0].Name == "x1" && x[0].Value == "v1");
        }

        [TestMethod]
        public void it_should_load_client_certificates()
        {
            endpoint.ClientCertificates.ShouldMatch(x => x.Count == 1
                    && x[0].StoreName == CertificateStoreName.My
                    && x[0].StoreLocation == CertificateStoreLocation.LocalMachine
                    && x[0].X509FindType == CertificateX509FindType.FindBySubjectName
                    && x[0].FindValue == "some");
        }

        [TestMethod]
        public void it_should_load_proxy_properties()
        {
            endpoint.Proxy.Address.ToString().ShouldEqual("http://contoso.com/");
        }

        [TestMethod]
        public void it_should_load_proxy_credentials()
        {
            endpoint.Proxy.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user2" &&
                                                             x.Password == "password2" && x.Domain == "domain2");
        }
    }

    [TestClass]
    public class when_setting_source_to_null_before_loading_service_where_only_some_data_is_defined
    {
        static EndpointConfigurationProviderOverride _provider;
        protected static IClientEndpoint Endpoint;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _provider = new EndpointConfigurationProviderOverride();
            _provider.SetSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExternalEndpointConfiguration.config"));
            _provider.SetSource(null);

            BecauseOf();
        }

        static void BecauseOf()
        {
            Endpoint = _provider.GetEndpoint("service2");
        }

        [TestMethod]
        public void it_should_have_base_url_set_to_null()
        {
            Endpoint.BaseUrl.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_have_method_set_to_blank_string()
        {
            Endpoint.Method.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_have_timeout_set_to_configured_value()
        {
            Endpoint.Timeout.ShouldEqual(2000);
        }

        [TestMethod]
        public void it_should_have_auto_set_accept_encoding()
        {
            Endpoint.AutoSetAcceptEncoding.ShouldBeTrue();
        }

        [TestMethod]
        public void it_should_have_authentication_level_set_to_undefined()
        {
            Endpoint.AuthenticationLevel.ShouldEqual(RequestAuthenticationLevel.Undefined);
        }

        [TestMethod]
        public void it_should_load_empty_credentials()
        {
            Endpoint.Credential.CredentialType.ShouldEqual(CredentialType.None);
        }

        [TestMethod]
        public void it_should_load_empty_headers()
        {
            Endpoint.Headers.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_load_empty_client_certificates()
        {
            Endpoint.ClientCertificates.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_load_null_proxy()
        {
            Endpoint.Proxy.Address.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_load_empty_proxy_credentials()
        {
            Endpoint.Proxy.Credential.CredentialType.ShouldEqual(CredentialType.None);
        }
    }

    [TestClass]
    public class when_setting_source_to_non_esistent_file
    {
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => new EndpointConfigurationProviderOverride().SetSource("wrong-file-name.config"));
        }

        [TestMethod]
        public void it_should_throw_file_not_found_exception()
        {
            _exception.ShouldBeOfType<FileNotFoundException>();
        }

        [TestMethod]
        public void it_should_report_the_file_name()
        {
            _exception.Message.ShouldContain("wrong-file-name.config");
        }
    }

    [TestClass]
    public class when_setting_source_using_invalid_file_name
    {
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => new EndpointConfigurationProviderOverride().SetSource("invalid:file:name?config"));
        }

        [TestMethod]
        public void it_should_throw_file_not_found_exception()
        {
            _exception.ShouldBeOfType<FileNotFoundException>();
        }

        [TestMethod]
        public void it_should_report_the_file_name()
        {
            _exception.Message.ShouldContain("invalid:file:name?config");
        }
    }

    [TestClass]
    public class when_loading_from_non_default_section
    {
        static EndpointConfigurationProviderOverride _provider;
        protected static IClientEndpoint Endpoint;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _provider = new EndpointConfigurationProviderOverride
            {
                SectionName = "customSectionName"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            Endpoint = _provider.GetEndpoint("service4inCustomSectionName");
        }

        [TestMethod]
        public void it_should_keep_modified_section_name()
        {
            _provider.SectionName.ShouldEqual("customSectionName");
        }

        [TestMethod]
        public void it_should_load_enpoint_parameters()
        {
            Endpoint.ShouldMatch(x => x.Name == "service4inCustomSectionName" && x.BaseUrl.ToString() == "http://foo.bar/" && x.Timeout == 4000 &&
                        x.AutoSetAcceptEncoding == false && x.AuthenticationLevel == RequestAuthenticationLevel.MutualAuthRequired);
        }

        [TestMethod]
        public void it_should_load_credentials()
        {
            Endpoint.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user41" &&
                        x.Password == "password41" && x.Domain == "domain41");
        }

        [TestMethod]
        public void it_should_load_headers()
        {
            Endpoint.Headers.ShouldMatch(x => x.Count == 1 && x[0].Name == "x4" && x[0] .Value == "v4");
        }

        [TestMethod]
        public void it_should_load_client_certificates()
        {
            Endpoint.ClientCertificates.ShouldMatch(x => x.Count == 1 && x[0].StoreName == CertificateStoreName.My 
                && x[0].StoreLocation == CertificateStoreLocation.LocalMachine
                && x[0].X509FindType == CertificateX509FindType.FindBySubjectName && x[0].FindValue == "some");
        }

        [TestMethod]
        public void it_should_load_proxy_properties()
        {
            Endpoint.Proxy.Address.ToString().ShouldEqual("http://contoso.com/");
        }

        [TestMethod]
        public void it_should_load_proxy_credentials()
        {
            Endpoint.Proxy.Credential.ShouldMatch(x => x.CredentialType == CredentialType.NetworkCredential && x.User == "user42" &&
                x.Password == "password42" && x.Domain == "domain42");
        }
    }

    [TestClass]
    public class when_setting_section_name_to_null
    {
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => { new EndpointConfigurationProviderOverride().SectionName = null; });
        }

        [TestMethod]
        public void it_should_throw_argument_null_exception()
        {
            _exception.ShouldBeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void it_should_report_value_argument()
        {
            ((ArgumentNullException) _exception).ParamName.ShouldEqual("value");
        }
    }

    [TestClass]
    public class when_loading_from_non_existent_section
    {
        static EndpointConfigurationProviderOverride _provider;
        protected static IClientEndpoint Endpoint;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _provider = new EndpointConfigurationProviderOverride
            {
                SectionName = "unknownSectionName"
            };

            BecauseOf();
        }

        static void BecauseOf()
        {
            Endpoint = _provider.GetEndpoint("service4inCustomSectionName");
        }

        [TestMethod]
        public void it_should_keep_modified_section_name()
        {
            _provider.SectionName.ShouldEqual("unknownSectionName");
        }

        [TestMethod]
        public void it_should_return_null_endpoint_configuration()
        {
            Endpoint.ShouldBeNull();
        }
    }

    [TestClass]
    public class when_loading_from_non_existent_endpoint_configuration
    {
        static EndpointConfigurationProviderOverride _provider;
        protected static IClientEndpoint Endpoint;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _provider = new EndpointConfigurationProviderOverride();

            BecauseOf();
        }

        static void BecauseOf()
        {
            Endpoint = _provider.GetEndpoint("unknownService");
        }

        [TestMethod]
        public void it_should_return_null_endpoint_configuration()
        {
            Endpoint.ShouldBeNull();
        }
    }
}
