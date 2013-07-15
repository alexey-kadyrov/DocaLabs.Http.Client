using System;
using System.Net;
using System.Net.Security;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Binding
{
    [Subject(typeof(RequestExtensions))]
    class when_copying_headers_from_http_client_endpoint_and_null_model
    {
        static Mock<IRequestBinder> request_binder;
        static WebHeaderCollection headers;
        static Mock<WebRequest> web_request;
        static BindingContext binding_context;

        Establish context = () =>
        {
            request_binder = new Mock<IRequestBinder>();

            headers = new WebHeaderCollection();

            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Setup(x => x.Headers).Returns(headers);

            binding_context = new BindingContext(null, null, new ClientEndpointElement(), new Uri("http://foo.bar"));

            binding_context.Configuration.Headers.Add(new ClientHeaderElement { Name = "x-header1", Value = "value11,value12" });
            binding_context.Configuration.Headers.Add(new ClientHeaderElement { Name = "x-header2", Value = "value2" });
        };

        Because of =
            () => web_request.Object.CopyHeadersFrom(request_binder.Object, binding_context);

        It should_copy_first_header_to_the_web_request =
            () => headers["x-header1"].ShouldEqual("value11,value12");

        It should_copy_second_header_to_the_web_request =
            () => headers["x-header2"].ShouldEqual("value2");
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_empty_headers_from_http_client_endpoint_and_null_model
    {
        static Mock<IRequestBinder> request_binder;
        static WebHeaderCollection headers;
        static Mock<WebRequest> web_request;
        static BindingContext binding_context;

        Establish context = () =>
        {
            request_binder = new Mock<IRequestBinder>();

            headers = new WebHeaderCollection();

            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Setup(x => x.Headers).Returns(headers);

            binding_context = new BindingContext(null, null, new ClientEndpointElement(), new Uri("http://foo.bar"));
        };

        Because of =
            () => web_request.Object.CopyHeadersFrom(request_binder.Object, binding_context);

        It should_not_copy_anything =
            () => headers.ShouldBeEmpty();
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_web_proxy_from_http_client_endpoint
    {
        static Mock<WebRequest> web_request;
        static ClientEndpointElement element;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();

            element = new ClientEndpointElement();
            element.Proxy.Address = new Uri("http://foo.bar/");
        };

        Because of =
            () => web_request.Object.CopyWebProxyFrom(element);

        It should_copy_proxy_to_the_web_request =
            () => ((WebProxy)web_request.Object.Proxy).Address.ToString().ShouldEqual("http://foo.bar/");

        It should_not_set_credentials =
            () => ((WebProxy)web_request.Object.Proxy).Credentials.ShouldBeNull();
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_web_proxy_from_http_client_endpoint_whith_credentials_set
    {
        static Mock<WebRequest> web_request;
        static ClientEndpointElement element;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();

            element = new ClientEndpointElement();
            element.Proxy.Address = new Uri("http://foo.bar/");
            element.Proxy.Credential.CredentialType = CredentialType.DefaultCredentials;
        };

        Because of =
            () => web_request.Object.CopyWebProxyFrom(element);

        It should_copy_proxy_to_the_web_request =
            () => ((WebProxy)web_request.Object.Proxy).Address.ToString().ShouldEqual("http://foo.bar/");

        It should_set_credentials =
            () => ((WebProxy)web_request.Object.Proxy).Credentials.ShouldBeTheSameAs(CredentialCache.DefaultCredentials);
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_web_proxy_with_null_address_from_http_client_endpoint
    {
        static Mock<WebRequest> web_request;
        static ClientEndpointElement element;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();

            element = new ClientEndpointElement();
        };

        Because of =
            () => web_request.Object.CopyWebProxyFrom(element);

        It should_not_copy_proxy_to_the_web_request =
            () => web_request.Object.Proxy.ShouldBeNull();
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_headers_from_http_client_endpoint_to_null_request_and_null_model
    {
        static Mock<IRequestBinder> request_binder;
        static BindingContext binding_context;
        static Exception exception;

        Establish context = () =>
        {
            request_binder = new Mock<IRequestBinder>();
            binding_context = new BindingContext(null, null, new ClientEndpointElement(), new Uri("http://foo.bar"));
        };

        Because of =
            () => exception = Catch.Exception(() => ((WebRequest)null).CopyHeadersFrom(request_binder.Object, binding_context));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("request");
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_proxy_from_http_client_endpoint_to_null_request
    {
        static ClientEndpointElement element;
        static Exception exception;

        Establish context =
            () => element = new ClientEndpointElement();

        Because of =
            () => exception = Catch.Exception(() => ((WebRequest)null).CopyWebProxyFrom(element));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("request");
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_credentials_from_http_client_endpoint_which_are_set_to_none_without_authentication_level_and_null_model
    {
        static Mock<WebRequest> web_request;
        static Mock<IRequestBinder> request_binder;
        static BindingContext binding_context;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Object.AuthenticationLevel = AuthenticationLevel.None;

            request_binder = new Mock<IRequestBinder>();
            binding_context = new BindingContext(null, null, new ClientEndpointElement(), new Uri("http://foo.bar"));
        };

        Because of =
            () => web_request.Object.CopyCredentialsFrom(request_binder.Object, binding_context);

        It should_not_copy_credentials =
            () => web_request.Object.Credentials.ShouldBeNull();

        It should_leave_default_authentication_level =
            () => web_request.Object.AuthenticationLevel.ShouldEqual(AuthenticationLevel.None);
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_credentials_from_http_client_endpoint_which_are_set_to_default_credentials_without_authentication_level_and_null_model
    {
        static Mock<WebRequest> web_request;
        static Mock<IRequestBinder> request_binder;
        static BindingContext binding_context;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Object.AuthenticationLevel = AuthenticationLevel.None;

            request_binder = new Mock<IRequestBinder>();
            binding_context = new BindingContext(null, null, new ClientEndpointElement(), new Uri("http://foo.bar"));

            binding_context.Configuration.Credential.CredentialType = CredentialType.DefaultCredentials;
        };

        Because of =
            () => web_request.Object.CopyCredentialsFrom(request_binder.Object, binding_context);

        It should_copy_credentials =
            () => web_request.Object.Credentials.ShouldBeTheSameAs(CredentialCache.DefaultCredentials);

        It should_leave_default_authentication_level =
            () => web_request.Object.AuthenticationLevel.ShouldEqual(AuthenticationLevel.None);
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_credentials_from_http_client_endpoint_which_are_set_to_default_credentials_with_authentication_level_and_null_model
    {
        static Mock<WebRequest> web_request;
        static Mock<IRequestBinder> request_binder;
        static BindingContext binding_context;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Object.AuthenticationLevel = AuthenticationLevel.None;

            request_binder = new Mock<IRequestBinder>();
            binding_context = new BindingContext(null, null, new ClientEndpointElement(), new Uri("http://foo.bar"))
            {
                Configuration = { AuthenticationLevel = AuthenticationLevel.MutualAuthRequired }
            };

            binding_context.Configuration.Credential.CredentialType = CredentialType.DefaultCredentials;
        };

        Because of =
            () => web_request.Object.CopyCredentialsFrom(request_binder.Object, binding_context);

        It should_copy_credentials =
            () => web_request.Object.Credentials.ShouldBeTheSameAs(CredentialCache.DefaultCredentials);

        It should_copy_authentication_level =
            () => web_request.Object.AuthenticationLevel.ShouldEqual(AuthenticationLevel.MutualAuthRequired);
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_credentials_from_http_client_endpoint_to_null_request_and_null_model
    {
        static Mock<IRequestBinder> request_binder;
        static BindingContext binding_context;
        static Exception exception;

        Establish context = () =>
        {
            request_binder = new Mock<IRequestBinder>();
            binding_context = new BindingContext(null, null, new ClientEndpointElement(), new Uri("http://foo.bar"));
        };

        Because of =
            () => exception = Catch.Exception(() => ((WebRequest)null).CopyCredentialsFrom(request_binder.Object, binding_context));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("request");
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_empty_client_certificates_from_http_client_endpoint
    {
        static Mock<HttpWebRequest> web_request;
        static ClientEndpointElement element;


        Establish context = () =>
        {
            web_request = new Mock<HttpWebRequest> { CallBase = true };

            element = new ClientEndpointElement();
        };

        Because of =
            () => web_request.Object.CopyClientCertificatesFrom(element);

        It should_copy_the_certificate =
            () => web_request.Object.ClientCertificates.ShouldBeEmpty();
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_client_certificates_from_http_client_endpoint_to_null_request
    {
        static ClientEndpointElement element;
        static Exception exception;

        Establish context =
            () => element = new ClientEndpointElement();

        Because of =
            () => exception = Catch.Exception(() => ((WebRequest)null).CopyClientCertificatesFrom(element));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_request_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("request");
    }
}
