using System;
using System.Configuration;
using System.Net;
using System.Net.Security;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [Subject(typeof(HttpClientEndpointElement))]
    class when_http_client_endpoint_is_newed
    {
        static HttpClientEndpointElement element;

        Because of =
            () => element = new HttpClientEndpointElement();

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
            () => element.Credentials.CredentialsType.ShouldEqual(CredentialsType.None);

        It should_have_empty_collection_of_headers =
            () => element.Headers.ShouldBeEmpty();

        It should_have_empty_collection_of_client_certificates =
            () => element.ClientCertificates.ShouldBeEmpty();

        It should_have_proxy_with_null_address =
            () => element.Proxy.Address.ShouldBeNull();
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_changing_value_on_http_client_endpoint_which_is_directly_newed
    {
        static HttpClientEndpointElement element;
        static Uri base_url;

        Establish context = () =>
        {
            element = new HttpClientEndpointElement();
            base_url = new Uri("http://foo.bar/");
        };

        Because of = () =>
        {
            element.Name = "name1";
            element.BaseUrl = base_url;
            element.Method = "PUT";
            element.Timeout = 1;
            element.AutoSetAcceptEncoding = false;
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
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_headers_from_http_client_endpoint
    {
        static WebHeaderCollection headers;
        static Mock<WebRequest> web_request;
        static HttpClientEndpointElement element;

        Establish context = () =>
        {
            headers = new WebHeaderCollection();

            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Setup(x => x.Headers).Returns(headers);

            element = new HttpClientEndpointElement();
            element.Headers.Add(new NameValueConfigurationElement("x-header1", "value11,value12"));
            element.Headers.Add(new NameValueConfigurationElement("x-header2", "value2"));
        };

        Because of =
            () => element.CopyHeadersTo(web_request.Object);

        It should_copy_first_header_to_the_web_request =
            () => headers["x-header1"].ShouldEqual("value11,value12");

        It should_copy_second_header_to_the_web_request =
            () => headers["x-header2"].ShouldEqual("value2");
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_empty_headers_from_http_client_endpoint
    {
        static WebHeaderCollection headers;
        static Mock<WebRequest> web_request;
        static HttpClientEndpointElement element;

        Establish context = () =>
        {
            headers = new WebHeaderCollection();

            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Setup(x => x.Headers).Returns(headers);

            element = new HttpClientEndpointElement();
        };

        Because of =
            () => element.CopyHeadersTo(web_request.Object);

        It should_not_copy_anything =
            () => headers.ShouldBeEmpty();
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_web_proxy_from_http_client_endpoint
    {
        static Mock<WebRequest> web_request;
        static HttpClientEndpointElement element;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();

            element = new HttpClientEndpointElement();
            element.Proxy.Address = new Uri("http://foo.bar/");
        };

        Because of =
            () => element.CopyWebProxyTo(web_request.Object);

        It should_copy_proxy_to_the_web_request =
            () => ((WebProxy)web_request.Object.Proxy).Address.ToString().ShouldEqual("http://foo.bar/");

        It should_not_set_credentials =
            () => ((WebProxy) web_request.Object.Proxy).Credentials.ShouldBeNull();
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_web_proxy_from_http_client_endpoint_whith_credentials_set
    {
        static Mock<WebRequest> web_request;
        static HttpClientEndpointElement element;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();

            element = new HttpClientEndpointElement();
            element.Proxy.Address = new Uri("http://foo.bar/");
            element.Proxy.Credentials.CredentialsType = CredentialsType.DefaultCredentials;
        };

        Because of =
            () => element.CopyWebProxyTo(web_request.Object);

        It should_copy_proxy_to_the_web_request =
            () => ((WebProxy)web_request.Object.Proxy).Address.ToString().ShouldEqual("http://foo.bar/");

        It should_set_credentials =
            () => ((WebProxy)web_request.Object.Proxy).Credentials.ShouldBeTheSameAs(CredentialCache.DefaultCredentials);
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_web_proxy_with_null_address_from_http_client_endpoint
    {
        static Mock<WebRequest> web_request;
        static HttpClientEndpointElement element;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();

            element = new HttpClientEndpointElement();
        };

        Because of =
            () => element.CopyWebProxyTo(web_request.Object);

        It should_not_copy_proxy_to_the_web_request =
            () => web_request.Object.Proxy.ShouldBeNull();
    }
    
    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_empty_client_certificates_from_http_client_endpoint
    {
        static Mock<HttpWebRequest> web_request;
        static HttpClientEndpointElement element;


        Establish context = () =>
        {
            web_request = new Mock<HttpWebRequest> { CallBase = true };

            element = new HttpClientEndpointElement();
        };

        Because of =
            () => element.CopyClientCertificatesTo(web_request.Object);

        It should_copy_the_certificate =
            () => web_request.Object.ClientCertificates.ShouldBeEmpty();
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_headers_from_http_client_endpoint_to_null_request
    {
        static HttpClientEndpointElement element;
        static Exception exception;

        Establish context = 
            () => element = new HttpClientEndpointElement();

        Because of =
            () => exception = Catch.Exception(() => element.CopyHeadersTo(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("request");
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_client_certificates_from_http_client_endpoint_to_null_request
    {
        static HttpClientEndpointElement element;
        static Exception exception;

        Establish context =
            () => element = new HttpClientEndpointElement();

        Because of =
            () => exception = Catch.Exception(() => element.CopyClientCertificatesTo(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("request");
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_proxy_from_http_client_endpoint_to_null_request
    {
        static HttpClientEndpointElement element;
        static Exception exception;

        Establish context =
            () => element = new HttpClientEndpointElement();

        Because of =
            () => exception = Catch.Exception(() => element.CopyWebProxyTo(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("request");
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_credentials_from_http_client_endpoint_which_are_set_to_none_without_authentication_level
    {
        static Mock<WebRequest> web_request;
        static HttpClientEndpointElement element;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Object.AuthenticationLevel = AuthenticationLevel.None;

            element = new HttpClientEndpointElement();
        };

        Because of =
            () => element.CopyCredentialsTo(web_request.Object);

        It should_not_copy_credentials =
            () => web_request.Object.Credentials.ShouldBeNull();

        It should_leave_default_authentication_level =
            () => web_request.Object.AuthenticationLevel.ShouldEqual(AuthenticationLevel.None);
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_credentials_from_http_client_endpoint_which_are_set_to_default_credentials_without_authentication_level
    {
        static Mock<WebRequest> web_request;
        static HttpClientEndpointElement element;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Object.AuthenticationLevel = AuthenticationLevel.None;

            element = new HttpClientEndpointElement
            {
                Credentials =
                {
                    CredentialsType = CredentialsType.DefaultCredentials
                }
            };
        };

        Because of =
            () => element.CopyCredentialsTo(web_request.Object);
         
        It should_copy_credentials =
            () => web_request.Object.Credentials.ShouldBeTheSameAs(CredentialCache.DefaultCredentials);

        It should_leave_default_authentication_level =
            () => web_request.Object.AuthenticationLevel.ShouldEqual(AuthenticationLevel.None);
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_credentials_from_http_client_endpoint_which_are_set_to_default_credentials_with_authentication_level
    {
        static Mock<WebRequest> web_request;
        static HttpClientEndpointElement element;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Object.AuthenticationLevel = AuthenticationLevel.None;

            element = new HttpClientEndpointElement
            {
                AuthenticationLevel = AuthenticationLevel.MutualAuthRequired,
                Credentials =
                {
                    CredentialsType = CredentialsType.DefaultCredentials
                }
            };
        };

        Because of =
            () => element.CopyCredentialsTo(web_request.Object);

        It should_copy_credentials =
            () => web_request.Object.Credentials.ShouldBeTheSameAs(CredentialCache.DefaultCredentials);

        It should_copy_authentication_level =
            () => web_request.Object.AuthenticationLevel.ShouldEqual(AuthenticationLevel.MutualAuthRequired);
    }

    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_credentials_from_http_client_endpoint_to_null_request
    {
        static HttpClientEndpointElement element;
        static Exception exception;

        Establish context =
            () => element = new HttpClientEndpointElement();

        Because of =
            () => exception = Catch.Exception(() => element.CopyCredentialsTo(null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("request");
    }
}
