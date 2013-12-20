using System;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [TestClass]
    public class when_http_client_endpoint_is_newed
    {
        static ClientEndpointElement _element;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _element = new ClientEndpointElement();
        }

        [TestMethod]
        public void it_should_have_name_set_to_empty_string()
        {
            _element.Name.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_have_base_url_set_to_null()
        {
            _element.BaseUrl.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_have_method_set_to_blank_string()
        {
            _element.Method.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_have_timeout_set_to_90_seconds()
        {
            _element.Timeout.ShouldEqual(90000);
        }

        [TestMethod]
        public void it_should_have_auto_set_accept_encoding()
        {
            _element.AutoSetAcceptEncoding.ShouldBeTrue();
        }

        [TestMethod]
        public void it_should_have_authentication_level_set_to_null()
        {
            _element.AuthenticationLevel.ShouldEqual(RequestAuthenticationLevel.Undefined);
        }

        [TestMethod]
        public void it_should_have_credentials_set_to_none()
        {
            _element.Credential.CredentialType.ShouldEqual(CredentialType.None);
        }

        [TestMethod]
        public void it_should_have_empty_collection_of_headers()
        {
            _element.Headers.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_have_empty_collection_of_client_certificates()
        {
            _element.ClientCertificates.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_have_proxy_with_null_address()
        {
            _element.Proxy.Address.ShouldBeNull();
        }
    }

    [TestClass]
    public class when_changing_value_on_http_client_endpoint_which_is_directly_newed
    {
        static ClientEndpointElement _element;
        static Uri _baseUrl;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        { 
            _element = new ClientEndpointElement();
            _baseUrl = new Uri("http://foo.bar/");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _element.Name = "name1";
            _element.BaseUrl = _baseUrl;
            _element.Method = "PUT";
            _element.Timeout = 1;
            _element.AutoSetAcceptEncoding = false;
            _element.AuthenticationLevel = RequestAuthenticationLevel.MutualAuthRequired;
        }

        [TestMethod]
        public void it_should_change_name()
        {
            _element.Name.ShouldEqual("name1");
        }

        [TestMethod]
        public void it_should_change_method()
        {
            _element.Method.ShouldEqual("PUT");
        }

        [TestMethod]
        public void it_should_change_base_url()
        {
            _element.BaseUrl.ShouldBeTheSameAs(_baseUrl);
        }

        [TestMethod]
        public void it_should_change_timeout()
        {
            _element.Timeout.ShouldEqual(1);
        }

        [TestMethod]
        public void it_should_change_auto_set_accept_encoding()
        {
            _element.AutoSetAcceptEncoding.ShouldBeFalse();
        }

        [TestMethod]
        public void it_should_change_authentication_level()
        {
            _element.AuthenticationLevel.ShouldEqual(RequestAuthenticationLevel.MutualAuthRequired);
        }
    }
}
