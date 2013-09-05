using System;
using System.Net;
using System.Net.Security;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;
using Machine.Specifications.Annotations;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Binding
{
    [Subject(typeof(RequestExtensions))]
    class when_copying_headers_from_client_endpoint_and_null_model
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
    class when_copying_empty_headers_from_client_endpoint_and_null_model
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
    class when_copying_headers_from_client_endpoint_and_model
    {
        static TestModel model;
        static IRequestBinder request_binder;
        static WebHeaderCollection headers;
        static Mock<WebRequest> web_request;
        static BindingContext binding_context;

        Establish context = () =>
        {
            model = new TestModel
            {
                Headers = new WebHeaderCollection
                {
                    { "model-header", "value2" },
                    { "x-header1", "value-from-model" }
                }
            };

            request_binder = ModelBinders.DefaultRequestBinder;

            headers = new WebHeaderCollection();

            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Setup(x => x.Headers).Returns(headers);

            binding_context = new BindingContext(new TestClient(), model, new ClientEndpointElement(), new Uri("http://foo.bar"))
            {
                Model = model
            };
            binding_context.Configuration.Headers.Add(new ClientHeaderElement { Name = "x-header1", Value = "value1" });
        };

        Because of =
            () => web_request.Object.CopyHeadersFrom(request_binder, binding_context);

        It should_copy_headers_from_configuration_and_model_to_the_web_request =
            () => headers.GetValues("x-header1").ShouldContainOnly("value1", "value-from-model");

        It should_copy_header_from_model_to_the_web_request =
            () => headers.GetValues("model-header").ShouldContainOnly("value2");

        class TestModel
        {
            [PropertyOverrides(Name = "")]
            public WebHeaderCollection Headers { [UsedImplicitly] get; set; }
        }

        class TestClient : HttpClient<string, string>
        {
            public TestClient()
                : base(new Uri("http://foo.bar"))
            {
            }
        }
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_web_proxy_from_client_endpoint
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
    class when_copying_web_proxy_from_client_endpoint_whith_credentials_set
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
    class when_copying_web_proxy_with_null_address_from_client_endpoint
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
    class when_copying_web_proxy_with_null_client_endpoint
    {
        static Mock<WebRequest> web_request;
        static ClientEndpointElement element;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
        };

        Because of =
            () => web_request.Object.CopyWebProxyFrom(null);

        It should_not_copy_proxy_to_the_web_request =
            () => web_request.Object.Proxy.ShouldBeNull();
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_headers_from_client_endpoint_to_null_request_and_null_model
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
    class when_copying_proxy_from_client_endpoint_to_null_request
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
    class when_copying_credentials_from_client_endpoint_and_model
    {
        static TestModel model;
        static Mock<WebRequest> web_request;
        static IRequestBinder request_binder;
        static BindingContext binding_context;

        Establish context = () =>
        {
            model = new TestModel
            {
                Credentials = CredentialCache.DefaultCredentials
            };

            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Object.AuthenticationLevel = AuthenticationLevel.None;

            request_binder = ModelBinders.DefaultRequestBinder;
            binding_context = new BindingContext(null, model, new ClientEndpointElement(), new Uri("http://foo.bar"))
            {
                Model = model
            };
            binding_context.Configuration.Credential.CredentialType = CredentialType.NetworkCredential;
            binding_context.Configuration.Credential.User = "user";
            binding_context.Configuration.Credential.Password = "password";
        };

        Because of =
            () => web_request.Object.CopyCredentialsFrom(request_binder, binding_context);

        It should_copy_credentials_from_model =
            () => web_request.Object.Credentials.ShouldBeTheSameAs(model.Credentials);

        It should_leave_default_authentication_level =
            () => web_request.Object.AuthenticationLevel.ShouldEqual(AuthenticationLevel.None);

        class TestModel
        {
            public ICredentials Credentials { get; set; }
        }
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_credentials_from_and_model_when_client_endpoint_is_null
    {
        static TestModel model;
        static Mock<WebRequest> web_request;
        static IRequestBinder request_binder;
        static BindingContext binding_context;

        Establish context = () =>
        {
            model = new TestModel
            {
                Credentials = CredentialCache.DefaultCredentials
            };

            web_request = new Mock<WebRequest>();
            web_request.SetupAllProperties();
            web_request.Object.AuthenticationLevel = AuthenticationLevel.None;

            request_binder = ModelBinders.DefaultRequestBinder;
            binding_context = new BindingContext(null, model, null, new Uri("http://foo.bar"))
            {
                Model = model
            };
        };

        Because of =
            () => web_request.Object.CopyCredentialsFrom(request_binder, binding_context);

        It should_copy_credentials_from_model =
            () => web_request.Object.Credentials.ShouldBeTheSameAs(model.Credentials);

        It should_leave_default_authentication_level =
            () => web_request.Object.AuthenticationLevel.ShouldEqual(AuthenticationLevel.None);

        class TestModel
        {
            public ICredentials Credentials { get; set; }
        }
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_credentials_from_client_endpoint_which_are_set_to_none_without_authentication_level_and_null_model
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
    class when_copying_credentials_from_client_endpoint_which_are_set_to_default_credentials_without_authentication_level_and_null_model
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
    class when_copying_credentials_from_client_endpoint_which_are_set_to_default_credentials_with_authentication_level_and_null_model
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
    class when_copying_credentials_from_client_endpoint_to_null_request_and_null_model
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
    class when_copying_empty_client_certificates_from_client_endpoint
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
    class when_copying_client_certificates_from_client_endpoint_to_null_request
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

    [Subject(typeof(RequestExtensions))]
    class when_copying_client_certificates_from_null_client_endpoint
    {
        static Mock<HttpWebRequest> web_request;

        Establish context = () =>
            web_request = new Mock<HttpWebRequest> { CallBase = true };

        Because of =
            () => web_request.Object.CopyClientCertificatesFrom(null);

        It should_not_copy_the_certificate =
            () => web_request.Object.ClientCertificates.ShouldBeEmpty();
    }

    [Subject(typeof(RequestExtensions))]
    class when_copying_client_certificates_from_client_endpoint_for_non_http_web_request
    {
        static Mock<WebRequest> web_request;
        static ClientEndpointElement element;
        static Exception exception;

        Establish context = () =>
        {
            web_request = new Mock<WebRequest> { CallBase = true };
            element = new ClientEndpointElement();
        };

        Because of =
            () => exception = Catch.Exception(() => web_request.Object.CopyClientCertificatesFrom(element));

        It should_not_throw_any_exception =
            () => exception.ShouldBeNull();
    }

    [Subject(typeof(RequestExtensions))]
    class when_checking_whenever_request_body_is_required_for_file_web_request_and_post_method
    {
        static FileWebRequest web_file_request;
        static bool is_required;

        Establish context = () =>
        {
            web_file_request = (FileWebRequest)WebRequest.CreateDefault(new Uri("file://my-file.txt"));
            web_file_request.Method = "Post";
        };

        Because of =
            () => is_required = web_file_request.IsBodyRequired();

        It should_return_true =
            () => is_required.ShouldBeTrue();
    }

    [Subject(typeof(RequestExtensions))]
    class when_checking_whenever_request_body_is_required_for_file_web_request_and_put_method
    {
        static FileWebRequest web_file_request;
        static bool is_required;

        Establish context = () =>
        {
            web_file_request = (FileWebRequest)WebRequest.CreateDefault(new Uri("file://my-file.txt"));
            web_file_request.Method = "Put";
        };

        Because of =
            () => is_required = web_file_request.IsBodyRequired();

        It should_return_true =
            () => is_required.ShouldBeTrue();
    }

    [Subject(typeof(RequestExtensions))]
    class when_checking_whenever_request_body_is_required_for_file_web_request_and_get_method
    {
        static FileWebRequest web_file_request;
        static bool is_required;

        Establish context = () =>
        {
            web_file_request = (FileWebRequest)WebRequest.CreateDefault(new Uri("file://my-file.txt"));
            web_file_request.Method = "GET";
        };

        Because of =
            () => is_required = web_file_request.IsBodyRequired();

        It should_return_false =
            () => is_required.ShouldBeFalse();
    }

    [Subject(typeof(RequestExtensions))]
    class when_checking_whenever_request_body_is_required_for_file_web_request_and_head_method
    {
        static FileWebRequest web_file_request;
        static bool is_required;

        Establish context = () =>
        {
            web_file_request = (FileWebRequest)WebRequest.CreateDefault(new Uri("file://my-file.txt"));
            web_file_request.Method = "HEAD";
        };

        Because of =
            () => is_required = web_file_request.IsBodyRequired();

        It should_return_false =
            () => is_required.ShouldBeFalse();
    }

    [Subject(typeof(RequestExtensions))]
    class when_checking_whenever_request_body_is_required_for_http_web_request_and_post_method
    {
        static HttpWebRequest http_web_request;
        static bool is_required;

        Establish context = () =>
        {
            http_web_request = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://foo.bar"));
            http_web_request.Method = "Post";
        };

        Because of =
            () => is_required = http_web_request.IsBodyRequired();

        It should_return_true =
            () => is_required.ShouldBeTrue();
    }

    [Subject(typeof(RequestExtensions))]
    class when_checking_whenever_request_body_is_required_for_http_web_request_and_put_method
    {
        static HttpWebRequest http_web_request;
        static bool is_required;

        Establish context = () =>
        {
            http_web_request = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://foo.bar"));
            http_web_request.Method = "Put";
        };

        Because of =
            () => is_required = http_web_request.IsBodyRequired();

        It should_return_true =
            () => is_required.ShouldBeTrue();
    }

    [Subject(typeof(RequestExtensions))]
    class when_checking_whenever_request_body_is_required_for_http_web_request_and_get_method
    {
        static HttpWebRequest http_web_request;
        static bool is_required;

        Establish context = () =>
        {
            http_web_request = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://foo.bar"));
            http_web_request.Method = "GET";
        };

        Because of =
            () => is_required = http_web_request.IsBodyRequired();

        It should_return_false =
            () => is_required.ShouldBeFalse();
    }

    [Subject(typeof(RequestExtensions))]
    class when_checking_whenever_request_body_is_required_for_http_web_request_and_head_method
    {
        static HttpWebRequest http_web_request;
        static bool is_required;

        Establish context = () =>
        {
            http_web_request = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://foo.bar"));
            http_web_request.Method = "HEAD";
        };

        Because of =
            () => is_required = http_web_request.IsBodyRequired();

        It should_return_false =
            () => is_required.ShouldBeFalse();
    }

    [Subject(typeof(RequestExtensions))]
    class when_checking_whenever_request_body_is_required_for_ftp_web_request_and_stor_method
    {
        static FtpWebRequest ftp_web_request;
        static bool is_required;

        Establish context = () =>
        {
            ftp_web_request = (FtpWebRequest)WebRequest.CreateDefault(new Uri("ftp://foo.bar/file.txt"));
            ftp_web_request.Method = "STOR";
        };

        Because of =
            () => is_required = ftp_web_request.IsBodyRequired();

        It should_return_false =
            () => is_required.ShouldBeFalse();
    }

    [Subject(typeof(RequestExtensions))]
    class when_setting_content_length_to_zero_based_on_whenever_request_body_is_required_for_file_web_request_and_post_method
    {
        static FileWebRequest web_file_request;

        Establish context = () =>
        {
            web_file_request = (FileWebRequest)WebRequest.CreateDefault(new Uri("file://my-file.txt"));
            web_file_request.ContentLength = 99;
            web_file_request.Method = "Post";
        };

        Because of =
            () => web_file_request.SetContentLengthToZeroIfBodyIsRequired();

        It should_set_to_zero =
            () => web_file_request.ContentLength.ShouldEqual(0);
    }

    [Subject(typeof(RequestExtensions))]
    class when_setting_content_length_to_zero_based_on_whenever_request_body_is_required_for_file_web_request_and_put_method
    {
        static FileWebRequest web_file_request;

        Establish context = () =>
        {
            web_file_request = (FileWebRequest)WebRequest.CreateDefault(new Uri("file://my-file.txt"));
            web_file_request.ContentLength = 99;
            web_file_request.Method = "Put";
        };

        Because of =
            () => web_file_request.SetContentLengthToZeroIfBodyIsRequired();

        It should_set_to_zero =
            () => web_file_request.ContentLength.ShouldEqual(0);
    }

    [Subject(typeof(RequestExtensions))]
    class when_setting_content_length_to_zero_based_on_whenever_request_body_is_required_for_file_web_request_and_get_method
    {
        static FileWebRequest web_file_request;

        Establish context = () =>
        {
            web_file_request = (FileWebRequest)WebRequest.CreateDefault(new Uri("file://my-file.txt"));
            web_file_request.ContentLength = 99;
            web_file_request.Method = "GET";
        };

        Because of =
            () => web_file_request.SetContentLengthToZeroIfBodyIsRequired();

        It should_leave_unchanged =
            () => web_file_request.ContentLength.ShouldEqual(99);
    }

    [Subject(typeof(RequestExtensions))]
    class when_setting_content_length_to_zero_based_on_whenever_request_body_is_required_for_file_web_request_and_head_method
    {
        static FileWebRequest web_file_request;

        Establish context = () =>
        {
            web_file_request = (FileWebRequest)WebRequest.CreateDefault(new Uri("file://my-file.txt"));
            web_file_request.ContentLength = 99;
            web_file_request.Method = "HEAD";
        };

        Because of =
            () => web_file_request.SetContentLengthToZeroIfBodyIsRequired();

        It should_leave_unchanged =
            () => web_file_request.ContentLength.ShouldEqual(99);
    }

    [Subject(typeof(RequestExtensions))]
    class when_setting_content_length_to_zero_based_on_whenever_request_body_is_required_for_http_web_request_and_post_method
    {
        static HttpWebRequest http_web_request;

        Establish context = () =>
        {
            http_web_request = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://foo.bar"));
            http_web_request.ContentLength = 99;
            http_web_request.Method = "Post";
        };

        Because of =
            () => http_web_request.SetContentLengthToZeroIfBodyIsRequired();

        It should_set_to_zero =
            () => http_web_request.ContentLength.ShouldEqual(0);
    }

    [Subject(typeof(RequestExtensions))]
    class when_setting_content_length_to_zero_based_on_whenever_request_body_is_required_for_http_web_request_and_put_method
    {
        static HttpWebRequest http_web_request;

        Establish context = () =>
        {
            http_web_request = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://foo.bar"));
            http_web_request.ContentLength = 99;
            http_web_request.Method = "Put";
        };

        Because of =
            () => http_web_request.SetContentLengthToZeroIfBodyIsRequired();

        It should_set_to_zero =
            () => http_web_request.ContentLength.ShouldEqual(0);
    }

    [Subject(typeof(RequestExtensions))]
    class when_setting_content_length_to_zero_based_on_whenever_request_body_is_required_for_http_web_request_and_get_method
    {
        static HttpWebRequest http_web_request;

        Establish context = () =>
        {
            http_web_request = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://foo.bar"));
            http_web_request.ContentLength = 99;
            http_web_request.Method = "GET";
        };

        Because of =
            () => http_web_request.SetContentLengthToZeroIfBodyIsRequired();

        It should_leave_unchanged =
            () => http_web_request.ContentLength.ShouldEqual(99);
    }

    [Subject(typeof(RequestExtensions))]
    class when_setting_content_length_to_zero_based_on_whenever_request_body_is_required_for_http_web_request_and_head_method
    {
        static HttpWebRequest http_web_request;

        Establish context = () =>
        {
            http_web_request = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://foo.bar"));
            http_web_request.ContentLength = 99;
            http_web_request.Method = "HEAD";
        };

        Because of =
            () => http_web_request.SetContentLengthToZeroIfBodyIsRequired();

        It should_leave_unchanged =
            () => http_web_request.ContentLength.ShouldEqual(99);
    }

    [Subject(typeof(RequestExtensions))]
    class when_setting_content_length_to_zero_based_on_whenever_request_body_is_required_for_ftp_web_request_and_stor_method
    {
        static FtpWebRequest ftp_web_request;

        Establish context = () =>
        {
            ftp_web_request = (FtpWebRequest)WebRequest.CreateDefault(new Uri("ftp://foo.bar/file.txt"));
            ftp_web_request.ContentLength = 99;
            ftp_web_request.Method = "STOR";
        };

        Because of =
            () => ftp_web_request.SetContentLengthToZeroIfBodyIsRequired();

        It should_leave_unchanged =
            () => ftp_web_request.ContentLength.ShouldEqual(99);
    }
}
