﻿using System.IO;
using System.Text;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Testing.Common;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using Machine.Specifications.Annotations;
using Moq;
using Newtonsoft.Json;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests
{
    [Subject(typeof(HttpClient<,>))]
    class when_instantiating_http_service_with_null_base_url_and_there_is_no_configuration_which_supplies_it
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => new Client<InModel, string>(null));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It shoould_report_base_url_argument =
            () => ((ArgumentException) exception).ParamName.ShouldEqual("baseUrl");
    }

    [Subject(typeof(HttpClient<,>))]
    class when_executing_http_service_witch_throws_exception
    {
        static Exception original_exception;
        static Mock<IExecuteStrategy<InModel, string>> strategy; 
        static Client<InModel, string> client;
        static Exception exception;

        Establish context = () =>
        {
            original_exception = new Exception();
            strategy = new Mock<IExecuteStrategy<InModel, string>>();
            strategy.Setup(x => x.Execute(Moq.It.IsAny<InModel>(), Moq.It.IsAny<Func<InModel, string>>())).Throws(original_exception);
            client = new Client<InModel, string>(new Uri("http://foo.bar/"), strategy.Object);
        };

        Because of =
            () => exception = Catch.Exception(() => client.Execute(new InModel()));

        It should_throw_http_client_exception =
            () => exception.ShouldBeOfType<HttpClientException>();

        It shoould_wrap_the_original_exception =
            () => exception.InnerException.ShouldBeTheSameAs(original_exception);
    }

    [Subject(typeof(HttpClient<,>))]
    class when_executing_http_service_witch_throws_htt_client_exception
    {
        static HttpClientException original_exception;
        static Mock<IExecuteStrategy<InModel, string>> strategy;
        static Client<InModel, string> client;
        static Exception exception;

        Establish context = () =>
        {
            original_exception = new HttpClientException();
            strategy = new Mock<IExecuteStrategy<InModel, string>>();
            strategy.Setup(x => x.Execute(Moq.It.IsAny<InModel>(), Moq.It.IsAny<Func<InModel, string>>())).Throws(original_exception);
            client = new Client<InModel, string>(new Uri("http://foo.bar/"), strategy.Object);
        };

        Because of =
            () => exception = Catch.Exception(() => client.Execute(new InModel()));

        It should_throw_the_same_http_client_exception =
            () => exception.ShouldBeTheSameAs(original_exception);
    }

    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_for_model_without_any_request_serialization_hints
    {
        static Client<InModel, string> client;
        static InModel model;

        Cleanup after = 
            () => TestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new Client<InModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));
            model = new InModel
            {
                ImplictPathValue = "path1",
                ImpilicitQueryValue = "query1",
                ImpilicitQueryValues = new Dictionary<string, string>
                {
                    { "k1", "v1"}
                },
                ExplicitPathValue = "path2",
                ExplicitQueryValue = "query2",
                ImplicitHeaders = new WebHeaderCollection
                {
                    {"xx-hh-1", "hh1"}
                },
                ExplicitHeader = "hh2",
                ImplicitCredentials = new NetworkCredential()
            };
        };

        Because of =
            () => client.Execute(model);

        It should_create_url_with_impicit_and_explicit_path_values =
            () => client.Request.RequestUri.GetLeftPart(UriPartial.Path).ShouldEqual("http://foo.bar/path1/path2");

        It should_create_url_with_impicit_and_explicit_query_values = () => HttpUtility.ParseQueryString(client.Request.RequestUri.Query).ShouldContainOnly(
                new NameValue("p1", "v1"), 
                new NameValue("ImpilicitQueryValue", "query1"),
                new NameValue("ExplicitQueryValue", "query2"),
                new NameValue("ImpilicitQueryValues.k1", "v1"));

        It should_add_implicit_and_explicit_headers = () => client.Request.Headers.ShouldContainOnly(
                new NameValue("ImplicitHeaders.xx-hh-1", "hh1"),
                new NameValue("ExplicitHeader", "hh2"));

        It should_set_implicit_credentials =
            () => client.Request.Credentials.ShouldBeTheSameAs(model.ImplicitCredentials);

        It should_set_content_length_to_zero =
            () => client.Request.ContentLength.ShouldEqual(0);
    }

    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_with_null_value_for_model_without_any_request_serialization_hints
    {
        static Client<InModel, string> client;

        Cleanup after =
            () => TestSerializerAttribute.SerializedObject = null;

        Establish context = 
            () => client = new Client<InModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));

        Because of =
            () => client.Execute(null);

        It should_not_modify_passed_url =
            () => client.Request.RequestUri.OriginalString.ShouldEqual("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1");

        It should_not_add_any_headers = 
            () => client.Request.Headers.ShouldBeEmpty();

        It should_set_implicit_credentials =
            () => client.Request.Credentials.ShouldBeNull();

        It should_set_content_length_to_zero =
            () => client.Request.ContentLength.ShouldEqual(0);
    }

    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_for_model_with_request_serialization_hints
    {
        static Client<RequestSerializableModel, string> client;
        static RequestSerializableModel model;

        Cleanup after =
            () => TestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new Client<RequestSerializableModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));
            model = new RequestSerializableModel
            {
                ImplictPathValue = "path1",
                ImpilicitQueryValue = "query1",
                ImpilicitQueryValues = new Dictionary<string, string>
                {
                    { "k1", "v1"}
                },
                ExplicitPathValue = "path2",
                ExplicitQueryValue = "query2",
                ImplicitHeaders = new WebHeaderCollection
                {
                    {"xx-hh-1", "hh1"}
                },
                ExplicitHeader = "hh2",
                ImplicitCredentials = new NetworkCredential()
            };
        };

        Because of =
            () => client.Execute(model);

        It should_create_url_only_with_explicit_path_values =
            () => client.Request.RequestUri.GetLeftPart(UriPartial.Path).ShouldEqual("http://foo.bar/%7BImplictPathValue%7D/path2");

        It should_create_url_only_with_explicit_query_values = () => HttpUtility.ParseQueryString(client.Request.RequestUri.Query).ShouldContainOnly(
                new NameValue("p1", "v1"),
                new NameValue("ExplicitQueryValue", "query2"));

        It should_add_only_explicit_headers = () => client.Request.Headers.ShouldContainOnly(
                new NameValue("ExplicitHeader", "hh2"));

        It should_not_set_implicit_credentials =
            () => client.Request.Credentials.ShouldBeNull();

        It should_call_serialization =
            () => TestSerializerAttribute.SerializedObject.ShouldBeTheSameAs(model);

        [TestSerializer]
        class RequestSerializableModel : InModel
        {
        }
    }

    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_with_null_value_for_model_with_request_serialization_hints
    {
        static Client<RequestSerializableModel, string> client;

        Cleanup after =
            () => TestSerializerAttribute.SerializedObject = null;

        Establish context = 
            () => client = new Client<RequestSerializableModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));

        Because of =
            () => client.Execute(null);

        It should_not_modify_passed_url =
            () => client.Request.RequestUri.OriginalString.ShouldEqual("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1");

        It should_not_add_any_headers =
            () => client.Request.Headers.ShouldBeEmpty();

        It should_set_implicit_credentials =
            () => client.Request.Credentials.ShouldBeNull();

        It should_set_content_length_to_zero =
            () => client.Request.ContentLength.ShouldEqual(0);

        [TestSerializer]
        class RequestSerializableModel : InModel
        {
        }
    }

    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_for_client_with_request_serialization_hints
    {
        static ClientWithRequestSerializableModel<InModel, string> client;
        static InModel model;

        Cleanup after =
            () => TestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new ClientWithRequestSerializableModel<InModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));
            model = new InModel
            {
                ImplictPathValue = "path1",
                ImpilicitQueryValue = "query1",
                ImpilicitQueryValues = new Dictionary<string, string>
                {
                    { "k1", "v1"}
                },
                ExplicitPathValue = "path2",
                ExplicitQueryValue = "query2",
                ImplicitHeaders = new WebHeaderCollection
                {
                    {"xx-hh-1", "hh1"}
                },
                ExplicitHeader = "hh2",
                ImplicitCredentials = new NetworkCredential()
            };
        };

        Because of =
            () => client.Execute(model);

        It should_create_url_only_with_explicit_path_values =
            () => client.Request.RequestUri.GetLeftPart(UriPartial.Path).ShouldEqual("http://foo.bar/%7BImplictPathValue%7D/path2");

        It should_create_url_only_with_explicit_query_values = () => HttpUtility.ParseQueryString(client.Request.RequestUri.Query).ShouldContainOnly(
                new NameValue("p1", "v1"),
                new NameValue("ExplicitQueryValue", "query2"));

        It should_add_only_explicit_headers = () => client.Request.Headers.ShouldContainOnly(
                new NameValue("ExplicitHeader", "hh2"));

        It should_not_set_implicit_credentials =
            () => client.Request.Credentials.ShouldBeNull();

        It should_call_serialization =
            () => TestSerializerAttribute.SerializedObject.ShouldBeTheSameAs(model);

        [TestSerializer]
        class ClientWithRequestSerializableModel<TIn, TOut> : Client<TIn, TOut>
        {
            public ClientWithRequestSerializableModel(Uri url) : base(url)
            {
            }
        }
    }

    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_for_dictionary_model_without_any_request_serialization_hints
    {
        static Client<InModel, string> client;
        static InModel model;

        Cleanup after =
            () => TestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new Client<InModel, string>(new Uri("http://foo.bar/{ImplictPathValue1}/{ImplictPathValue2}?p1=v1"));
            model = new InModel
            {
                { "ImplictPathValue1", "path1" },
                { "ImplictPathValue2", "path2" },
                { "qq1", "vv1" },
                { "qq2", "vv2" }
            };
        };

        Because of =
            () => client.Execute(model);

        It should_create_url_with_impicit_path_values =
            () => client.Request.RequestUri.GetLeftPart(UriPartial.Path).ShouldEqual("http://foo.bar/path1/path2");

        It should_create_url_with_impicit_query_values = () => HttpUtility.ParseQueryString(client.Request.RequestUri.Query).ShouldContainOnly(
                new NameValue("p1", "v1"),
                new NameValue("qq1", "vv1"),
                new NameValue("qq2", "vv2"));

        It should_not_add_any_header = 
            () => client.Request.Headers.ShouldBeEmpty();

        It should_not_set_implicit_credentials =
            () => client.Request.Credentials.ShouldBeNull();

        It should_set_content_length_to_zero =
            () => client.Request.ContentLength.ShouldEqual(0);

        class InModel : Dictionary<string, string>
        {
        }
    }

    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_for_dictionary_model_with_request_serialization_hints
    {
        static Client<InModel, string> client;
        static InModel model;

        Cleanup after =
            () => TestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new Client<InModel, string>(new Uri("http://foo.bar/{ImplictPathValue1}/{ImplictPathValue2}?p1=v1"));
            model = new InModel
            {
                { "ImplictPathValue1", "path1" },
                { "ImplictPathValue2", "path2" },
                { "qq1", "vv1" },
                { "qq2", "vv2" }
            };
        };

        Because of =
            () => client.Execute(model);

        It should_not_add_impicit_path_values_to_url =
            () => client.Request.RequestUri.GetLeftPart(UriPartial.Path).ShouldEqual("http://foo.bar/%7BImplictPathValue1%7D/%7BImplictPathValue2%7D");

        It should_not_add_impicit_query_values_to_url = () => HttpUtility.ParseQueryString(client.Request.RequestUri.Query).ShouldContainOnly(
                new NameValue("p1", "v1"));

        It should_not_add_any_header =
            () => client.Request.Headers.ShouldBeEmpty();

        It should_not_set_implicit_credentials =
            () => client.Request.Credentials.ShouldBeNull();

        It should_set_content_length_to_zero =
            () => client.Request.ContentLength.ShouldEqual(0);

        [TestSerializer]
        class InModel : Dictionary<string, string>
        {
        }
    }

    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_for_dictionary_model_with_client_with_request_serialization_hints
    {
        static ClientWithRequestSerializableModel<InModel, string> client;
        static InModel model;

        Cleanup after =
            () => TestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new ClientWithRequestSerializableModel<InModel, string>(new Uri("http://foo.bar/{ImplictPathValue1}/{ImplictPathValue2}?p1=v1"));
            model = new InModel
            {
                { "ImplictPathValue1", "path1" },
                { "ImplictPathValue2", "path2" },
                { "qq1", "vv1" },
                { "qq2", "vv2" }
            };
        };

        Because of =
            () => client.Execute(model);

        It should_not_add_impicit_path_values_to_url =
            () => client.Request.RequestUri.GetLeftPart(UriPartial.Path).ShouldEqual("http://foo.bar/%7BImplictPathValue1%7D/%7BImplictPathValue2%7D");

        It should_not_add_impicit_query_values_to_url = () => HttpUtility.ParseQueryString(client.Request.RequestUri.Query).ShouldContainOnly(
                new NameValue("p1", "v1"));

        It should_not_add_any_header =
            () => client.Request.Headers.ShouldBeEmpty();

        It should_not_set_implicit_credentials =
            () => client.Request.Credentials.ShouldBeNull();

        It should_set_content_length_to_zero =
            () => client.Request.ContentLength.ShouldEqual(0);

        class InModel : Dictionary<string, string>
        {
        }

        [TestSerializer]
        class ClientWithRequestSerializableModel<TIn, TOut> : Client<TIn, TOut>
        {
            public ClientWithRequestSerializableModel(Uri url) : base(url)
            {
            }
        }
    }

    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_for_model_with_mixed_targets_url_body_headers_credentials
    {
        static ClientWithMockedRequest<Model, string> client;
        static Model model;

        Cleanup after =
            () => TestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new ClientWithMockedRequest<Model, string>(new Uri("http://foo.bar/{UrlKey}"));
            model = new Model
            {
                UrlKey = "123",
                QueryKey = "q2",
                Credentials = new NetworkCredential(),
                Headers = new WebHeaderCollection
                {
                    { "x-header", "x-value"}
                },
                Body = new InnerModel
                {
                    Value = "Hello World!"
                }
            };
        };

        Because of =
            () => client.Execute(model);

        It should_create_url_with_all_suitable_values =
            () => client.RequestUrl.ShouldEqual("http://foo.bar/123?QueryKey=q2");

        It should_add_all_suiatble_header_values =
            () => client.Request.Headers.ShouldContainOnly(new NameValue("Headers.x-header", "x-value"));

        It should_set_credentials =
            () => client.Request.Credentials.ShouldBeTheSameAs(model.Credentials);

        It should_set_content_type_to_application_json =
            () => client.Request.ContentType.ShouldBeEqualIgnoringCase("application/json");

        It should_serialize_marked_inner_model =
            () => JsonConvert.DeserializeObject<InnerModel>(Encoding.UTF8.GetString(client.RequestData.ToArray())).Value.ShouldEqual("Hello World!");

        class Model
        {
            public string UrlKey { [UsedImplicitly] get; set; }
            public string QueryKey { [UsedImplicitly] get; set; }
            public WebHeaderCollection Headers { [UsedImplicitly] get; set; }
            public ICredentials Credentials { get; set; }

            [SerializeAsJson]
            public InnerModel Body { [UsedImplicitly] get; set; }
        }

        class InnerModel
        {
            public string Value { get; set; }
        }
    }

    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_for_model_with_stream_and_mixed_targets_url_body_headers_credentials
    {
        static ClientWithMockedRequest<Model, string> client;
        static Model model;

        Cleanup after =
            () => TestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new ClientWithMockedRequest<Model, string>(new Uri("http://foo.bar/{UrlKey}"));
            model = new Model
            {
                UrlKey = "123",
                QueryKey = "q2",
                Credentials = new NetworkCredential(),
                Headers = new WebHeaderCollection
                {
                    { "x-header", "x-value"}
                },
                Body = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"))
            };
        };

        Because of =
            () => client.Execute(model);

        It should_create_url_with_all_suitable_values =
            () => client.RequestUrl.ShouldEqual("http://foo.bar/123?QueryKey=q2");

        It should_add_all_suiatble_header_values =
            () => client.Request.Headers.ShouldContainOnly(new NameValue("Headers.x-header", "x-value"));

        It should_set_credentials =
            () => client.Request.Credentials.ShouldBeTheSameAs(model.Credentials);

        It should_set_content_type_to_application_octet =
            () => client.Request.ContentType.ShouldBeEqualIgnoringCase("application/octet-stream");

        It should_serialize_marked_inner_model =
            () => Encoding.UTF8.GetString(client.RequestData.ToArray()).ShouldEqual("Hello World!");

        class Model
        {
            public string UrlKey { [UsedImplicitly] get; set; }
            public string QueryKey { [UsedImplicitly] get; set; }
            public WebHeaderCollection Headers { [UsedImplicitly] get; set; }
            public ICredentials Credentials { get; set; }
            public Stream Body { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(HttpClient<,>))]
    class when_calling_http_service_for_stream_model
    {
        static ClientWithMockedRequest<Stream, string> client;
        static Stream model;

        Cleanup after =
            () => TestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new ClientWithMockedRequest<Stream, string>(new Uri("http://foo.bar/"));
            model = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"));
        };

        Because of =
            () => client.Execute(model);

        It should_leave_original_url=
            () => client.RequestUrl.ShouldEqual("http://foo.bar/");

        It should_set_content_type_to_application_octet =
            () => client.Request.ContentType.ShouldBeEqualIgnoringCase("application/octet-stream");

        It should_serialize_marked_inner_model =
            () => Encoding.UTF8.GetString(client.RequestData.ToArray()).ShouldEqual("Hello World!");
    }

    public class ClientWithMockedRequest<TIn, TOut> : Client<TIn, TOut>
    {
        public MemoryStream RequestData { get; set; }
        public Mock<HttpWebRequest> RequestMock { get; private set; }
        public string RequestUrl { get; private set; }

        public ClientWithMockedRequest(Uri url)
            : base(url)
        {
            RequestData = new MemoryStream();

            RequestMock = new Mock<HttpWebRequest> { CallBase = true };
            RequestMock.SetupAllProperties();
            RequestMock.Setup(x => x.GetRequestStream()).Returns(RequestData);
            RequestMock.Object.Headers = new WebHeaderCollection();
        }

        public ClientWithMockedRequest(Uri url, IExecuteStrategy<TIn, TOut> strategy) : base(url, strategy)
        {
        }

        protected override WebRequest CreateRequest(string url)
        {
            RequestUrl = url;
            return RequestMock.Object;
        }
    }

    public class Client<TIn, TOut> : HttpClient<TIn, TOut>
    {
        public BindingContext Context { get; private set; }
        public WebRequest Request { get; private set; }

        public Client(Uri url)
            : base(url)
        {
            Method = "POST";
        }

        public Client(Uri url, IExecuteStrategy<TIn, TOut> strategy)
            : base(url, null, strategy)
        {
            Method = "POST";
        }

        protected override bool ShouldSetAcceptEncoding(BindingContext context)
        {
            return false;
        }

        protected override TOut ParseResponse(BindingContext context, WebRequest request)
        {
            Request = request;
            Context = context;
            return default(TOut);
        }
    }

    public class InModel
    {
        public string ImplictPathValue { get; set; }
        public string ImpilicitQueryValue { get; set; }
        public Dictionary<string, string> ImpilicitQueryValues { get; set; }
        [RequestUse(RequestUseTargets.UrlPath)]
        public string ExplicitPathValue { get; set; }
        [RequestUse(RequestUseTargets.UrlQuery)]
        public string ExplicitQueryValue { get; set; }
        public WebHeaderCollection ImplicitHeaders { get; set; }
        [RequestUse(RequestUseTargets.RequestHeader)]
        public string ExplicitHeader { get; set; }
        public ICredentials ImplicitCredentials { get; set; }
    }

    class TestSerializerAttribute : RequestSerializationAttribute
    {
        public static object SerializedObject { get; set; }

        public override void Serialize(object obj, WebRequest request)
        {
            SerializedObject = obj;
            request.ContentLength = 0;
        }
    }
}
