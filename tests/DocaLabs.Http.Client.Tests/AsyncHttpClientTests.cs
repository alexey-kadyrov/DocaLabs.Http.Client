using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Utils;
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
    [Subject(typeof(AsyncHttpClient<,>))]
    class when_instantiating_asynchronous_http_service_with_null_base_url_and_there_is_configuration_which_supplies_it
    {
        static Exception exception;

        Because of =
            () => exception = Catch.Exception(() => new AsyncClient<AsyncInModel, string>(null));

        It should_throw_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It shoould_report_base_url_argument =
            () => ((ArgumentException) exception).ParamName.ShouldEqual("baseUrl");
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_executing_http_service_which_throws_exception
    {
        static Exception original_exception;
        static Mock<IExecuteStrategy<AsyncInModel, Task<string>>> strategy;
        static AsyncClient<AsyncInModel, string> client;
        static Exception exception;
        static string result;

        Establish context = () =>
        {
            original_exception = new Exception();
            strategy = new Mock<IExecuteStrategy<AsyncInModel, Task<string>>>();
            strategy.Setup(x => x.Execute(Moq.It.IsAny<AsyncInModel>(), Moq.It.IsAny<Func<AsyncInModel, Task<string>>>())).Throws(original_exception);
            client = new AsyncClient<AsyncInModel, string>(new Uri("http://foo.bar/"), strategy.Object);
        };

        Because of =
            () => exception = Catch.Exception(() => { result = client.ExecuteAsync(new AsyncInModel()).Result; });

        It should_throw_http_client_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<HttpClientException>();

        It shoould_wrap_the_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].InnerException.ShouldBeTheSameAs(original_exception);
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_executing_http_service_which_throws_htt_client_exception
    {
        static HttpClientException original_exception;
        static Mock<IExecuteStrategy<AsyncInModel, Task<string>>> strategy;
        static AsyncClient<AsyncInModel, string> client;
        static Exception exception;
        static string result;

        Establish context = () =>
        {
            original_exception = new HttpClientException();
            strategy = new Mock<IExecuteStrategy<AsyncInModel, Task<string>>>();
            strategy.Setup(x => x.Execute(Moq.It.IsAny<AsyncInModel>(), Moq.It.IsAny<Func<AsyncInModel, Task<string>>>())).Throws(original_exception);
            client = new AsyncClient<AsyncInModel, string>(new Uri("http://foo.bar/"), strategy.Object);
        };

        Because of =
            () => exception = Catch.Exception(() => { result = client.ExecuteAsync(new AsyncInModel()).Result; });

        It should_throw_the_same_http_client_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeTheSameAs(original_exception);
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_model_without_any_request_serialization_hints
    {
        static AsyncClient<AsyncInModel, string> client;
        static AsyncInModel model;

        Cleanup after = 
            () => AsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new AsyncClient<AsyncInModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));
            model = new AsyncInModel
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
            () => client.ExecuteAsync(model).Wait();

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

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_with_non_cancelled_token
    {
        static AsyncClient<AsyncInModel, string> client;
        static AsyncInModel model;

        Cleanup after =
            () => AsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new AsyncClient<AsyncInModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));
            model = new AsyncInModel
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
            () => client.ExecuteAsync(model, CancellationToken.None).Wait();

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

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_with_cancelled_token
    {
        static AsyncClient<AsyncInModel, string> client;
        static AsyncInModel model;
        static Exception exception;

        Cleanup after =
            () => AsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            AsyncClient<AsyncInModel, string>.ParseResponseExecutionMarker = null;
            client = new AsyncClient<AsyncInModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));
            model = new AsyncInModel
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
            () => exception = Catch.Exception(() => client.ExecuteAsync(model, new CancellationToken(true)).Wait());

        It should_throw_task_cancelled_exception =
            () => ((AggregateException) exception).InnerExceptions[0].ShouldBeOfType<TaskCanceledException>();

        It should_not_complete_execution_pipe_line =
            () => AsyncClient<AsyncInModel, string>.ParseResponseExecutionMarker.ShouldBeNull();
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_with_null_value_for_model_without_any_request_serialization_hints
    {
        static AsyncClient<AsyncInModel, string> client;

        Cleanup after =
            () => AsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = 
            () => client = new AsyncClient<AsyncInModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));

        Because of =
            () => client.ExecuteAsync(null).Wait();

        It should_not_modify_passed_url =
            () => client.Request.RequestUri.OriginalString.ShouldEqual("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1");

        It should_not_add_any_headers =
            () => client.Request.Headers.ShouldBeEmpty();

        It should_set_implicit_credentials =
            () => client.Request.Credentials.ShouldBeNull();

        It should_set_content_length_to_zero =
            () => client.Request.ContentLength.ShouldEqual(0);
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_model_with_request_serialization_hints_using_attribute_which_does_not_support_asynch_serialization
    {
        static AsyncClient<RequestSerializableModel, string> client;
        static RequestSerializableModel model;

        Cleanup after =
            () => NoAsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new AsyncClient<RequestSerializableModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));
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
            () => client.ExecuteAsync(model).Wait();

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
            () => NoAsyncTestSerializerAttribute.SerializedObject.ShouldBeTheSameAs(model);

        [NoAsyncTestSerializer]
        class RequestSerializableModel : AsyncInModel
        {
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_servicewith_null_value_for_model_with_request_serialization_hints_using_attribute_which_does_not_support_asynch_serialization
    {
        static AsyncClient<RequestSerializableModel, string> client;

        Cleanup after =
            () => NoAsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = 
            () => client = new AsyncClient<RequestSerializableModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));

        Because of =
            () => client.ExecuteAsync(null).Wait();

        It should_not_modify_passed_url =
            () => client.Request.RequestUri.OriginalString.ShouldEqual("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1");

        It should_not_add_any_headers =
            () => client.Request.Headers.ShouldBeEmpty();

        It should_set_implicit_credentials =
            () => client.Request.Credentials.ShouldBeNull();

        It should_set_content_length_to_zero =
            () => client.Request.ContentLength.ShouldEqual(0);

        [NoAsyncTestSerializer]
        class RequestSerializableModel : AsyncInModel
        {
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_model_with_request_serialization_hints_using_attribute_which_supports_asynch_serialization
    {
        static AsyncClient<RequestSerializableModel, string> client;
        static RequestSerializableModel model;

        Cleanup after =
            () => AsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new AsyncClient<RequestSerializableModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));
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
            () => client.ExecuteAsync(model).Wait();

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
            () => AsyncTestSerializerAttribute.SerializedObject.ShouldBeTheSameAs(model);

        [AsyncTestSerializer]
        class RequestSerializableModel : AsyncInModel
        {
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_model_with_request_serialization_hints_using_binder_which_does_not_support_asynch_writing
    {
        static AsyncClient<RequestSerializableModel, string> client;
        static RequestSerializableModel model;

        Cleanup after =
            () => AsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            ModelBinders.Add(typeof(RequestSerializableModel), new TestRequestBinder());
            client = new AsyncClient<RequestSerializableModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));
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
            () => client.ExecuteAsync(model).Wait();

        It should_create_url_only_with_explicit_path_values =
            () => client.Request.RequestUri.GetLeftPart(UriPartial.Path).ShouldEqual("http://foo.bar/%7BImplictPathValue%7D/path2");

        It should_create_url_only_with_explicit_query_values = () => HttpUtility.ParseQueryString(client.Request.RequestUri.Query).ShouldContainOnly(
                new NameValue("p1", "v1"),
                new NameValue("ExplicitQueryValue", "query2"));

        It should_add_only_explicit_headers = () => client.Request.Headers.ShouldContainOnly(
                new NameValue("ExplicitHeader", "hh2"));

        It should_not_set_implicit_credentials =
            () => client.Request.Credentials.ShouldBeNull();

        It should_still_call_serialization =
            () => AsyncTestSerializerAttribute.SerializedObject.ShouldBeTheSameAs(model);

        [AsyncTestSerializer]
        class RequestSerializableModel : AsyncInModel
        {
        }

        class AsyncTestSerializerAttribute : RequestSerializationAttribute
        {
            public static object SerializedObject { get; set; }

            public override void Serialize(object obj, WebRequest request)
            {
                SerializedObject = obj;
                request.ContentLength = 0;
            }

            public override Task SerializeAsync(object obj, WebRequest request, CancellationToken cancellationToken)
            {
                SerializedObject = obj;
                request.ContentLength = 0;
                return TaskUtils.CompletedTask();
            }
        }

        class TestRequestBinder : IRequestBinder
        {
            readonly DefaultRequestBinder _binder = new DefaultRequestBinder();

            public object TransformModel(BindingContext context)
            {
                return _binder.TransformModel(context);
            }

            public string ComposeUrl(BindingContext context)
            {
                return _binder.ComposeUrl(context);
            }

            public string InferRequestMethod(BindingContext context)
            {
                return _binder.InferRequestMethod(context);
            }

            public WebHeaderCollection GetHeaders(BindingContext context)
            {
                return _binder.GetHeaders(context);
            }

            public ICredentials GetCredentials(BindingContext context)
            {
                return _binder.GetCredentials(context);
            }

            public void Write(BindingContext context, WebRequest request)
            {
                _binder.Write(context, request);
            }
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_client_with_request_serialization_hints_using_attribute_which_does_not_support_asynch_serialization
    {
        static ClientWithRequestSerializableModel<AsyncInModel, string> client;
        static AsyncInModel model;

        Cleanup after =
            () => NoAsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new ClientWithRequestSerializableModel<AsyncInModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));
            model = new AsyncInModel
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
            () => client.ExecuteAsync(model).Wait();

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
            () => NoAsyncTestSerializerAttribute.SerializedObject.ShouldBeTheSameAs(model);

        [NoAsyncTestSerializer]
        class ClientWithRequestSerializableModel<TIn, TOut> : AsyncClient<TIn, TOut>
        {
            public ClientWithRequestSerializableModel(Uri url)
                : base(url)
            {
            }
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_client_with_request_serialization_hints_using_attribute_which_supports_asynch_serialization
    {
        static ClientWithRequestSerializableModel<AsyncInModel, string> client;
        static AsyncInModel model;

        Cleanup after =
            () => AsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new ClientWithRequestSerializableModel<AsyncInModel, string>(new Uri("http://foo.bar/{ImplictPathValue}/{ExplicitPathValue}?p1=v1"));
            model = new AsyncInModel
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
            () => client.ExecuteAsync(model).Wait();

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
            () => AsyncTestSerializerAttribute.SerializedObject.ShouldBeTheSameAs(model);

        [AsyncTestSerializer]
        class ClientWithRequestSerializableModel<TIn, TOut> : AsyncClient<TIn, TOut>
        {
            public ClientWithRequestSerializableModel(Uri url)
                : base(url)
            {
            }
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_dictionary_model_without_any_request_serialization_hints
    {
        static AsyncClient<AsyncInModel, string> client;
        static AsyncInModel model;

        Establish context = () =>
        {
            client = new AsyncClient<AsyncInModel, string>(new Uri("http://foo.bar/{ImplictPathValue1}/{ImplictPathValue2}?p1=v1"));
            model = new AsyncInModel
            {
                { "ImplictPathValue1", "path1" },
                { "ImplictPathValue2", "path2" },
                { "qq1", "vv1" },
                { "qq2", "vv2" }
            };
        };

        Because of =
            () => client.ExecuteAsync(model).Wait();

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

        class AsyncInModel : Dictionary<string, string>
        {
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_dictionary_model_with_request_serialization_hints_using_attribute_which_does_not_support_asynch_serialization
    {
        static AsyncClient<AsyncInModel, string> client;
        static AsyncInModel model;

        Cleanup after =
            () => NoAsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new AsyncClient<AsyncInModel, string>(new Uri("http://foo.bar/{ImplictPathValue1}/{ImplictPathValue2}?p1=v1"));
            model = new AsyncInModel
            {
                { "ImplictPathValue1", "path1" },
                { "ImplictPathValue2", "path2" },
                { "qq1", "vv1" },
                { "qq2", "vv2" }
            };
        };

        Because of =
            () => client.ExecuteAsync(model).Wait();

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

        [NoAsyncTestSerializer]
        class AsyncInModel : Dictionary<string, string>
        {
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_dictionary_model_with_request_serialization_hints_using_attribute_which_supports_asynch_serialization
    {
        static AsyncClient<AsyncInModel, string> client;
        static AsyncInModel model;

        Cleanup after =
            () => AsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new AsyncClient<AsyncInModel, string>(new Uri("http://foo.bar/{ImplictPathValue1}/{ImplictPathValue2}?p1=v1"));
            model = new AsyncInModel
            {
                { "ImplictPathValue1", "path1" },
                { "ImplictPathValue2", "path2" },
                { "qq1", "vv1" },
                { "qq2", "vv2" }
            };
        };

        Because of =
            () => client.ExecuteAsync(model).Wait();

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

        [AsyncTestSerializer]
        class AsyncInModel : Dictionary<string, string>
        {
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_dictionary_model_with_client_with_request_serialization_hints_using_attribute_which_does_not_support_asynch_serialization
    {
        static ClientWithRequestSerializableModel<AsyncInModel, string> client;
        static AsyncInModel model;

        Cleanup after =
            () => NoAsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new ClientWithRequestSerializableModel<AsyncInModel, string>(new Uri("http://foo.bar/{ImplictPathValue1}/{ImplictPathValue2}?p1=v1"));
            model = new AsyncInModel
            {
                { "ImplictPathValue1", "path1" },
                { "ImplictPathValue2", "path2" },
                { "qq1", "vv1" },
                { "qq2", "vv2" }
            };
        };

        Because of =
            () => client.ExecuteAsync(model).Wait();

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

        class AsyncInModel : Dictionary<string, string>
        {
        }

        [NoAsyncTestSerializer]
        class ClientWithRequestSerializableModel<TIn, TOut> : AsyncClient<TIn, TOut>
        {
            public ClientWithRequestSerializableModel(Uri url)
                : base(url)
            {
            }
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_dictionary_model_with_client_with_request_serialization_hints_using_attribute_which_supports_asynch_serialization
    {
        static ClientWithRequestSerializableModel<AsyncInModel, string> client;
        static AsyncInModel model;

        Cleanup after =
            () => AsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new ClientWithRequestSerializableModel<AsyncInModel, string>(new Uri("http://foo.bar/{ImplictPathValue1}/{ImplictPathValue2}?p1=v1"));
            model = new AsyncInModel
            {
                { "ImplictPathValue1", "path1" },
                { "ImplictPathValue2", "path2" },
                { "qq1", "vv1" },
                { "qq2", "vv2" }
            };
        };

        Because of =
            () => client.ExecuteAsync(model).Wait();

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

        class AsyncInModel : Dictionary<string, string>
        {
        }

        [AsyncTestSerializer]
        class ClientWithRequestSerializableModel<TIn, TOut> : AsyncClient<TIn, TOut>
        {
            public ClientWithRequestSerializableModel(Uri url)
                : base(url)
            {
            }
        }
    }

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_model_with_mixed_targets_url_body_headers_credentials
    {
        static AsyncClientWithMockedRequest<Model, string> client;
        static Model model;

        Cleanup after =
            () => AsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new AsyncClientWithMockedRequest<Model, string>(new Uri("http://foo.bar/{UrlKey}"));
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
            () => client.ExecuteAsync(model).Wait();

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

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_model_with_stream_and_mixed_targets_url_body_headers_credentials
    {
        static AsyncClientWithMockedRequest<Model, string> client;
        static Model model;

        Cleanup after =
            () => AsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new AsyncClientWithMockedRequest<Model, string>(new Uri("http://foo.bar/{UrlKey}"));
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
            () => client.ExecuteAsync(model).Wait();

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

    [Subject(typeof(AsyncHttpClient<,>))]
    class when_asynchronously_calling_http_service_for_stream_model
    {
        static AsyncClientWithMockedRequest<Stream, string> client;
        static Stream model;

        Cleanup after =
            () => AsyncTestSerializerAttribute.SerializedObject = null;

        Establish context = () =>
        {
            client = new AsyncClientWithMockedRequest<Stream, string>(new Uri("http://foo.bar/"));
            model = new MemoryStream(Encoding.UTF8.GetBytes("Hello World!"));
        };

        Because of =
            () => client.ExecuteAsync(model).Wait();

        It should_leave_original_url =
            () => client.RequestUrl.ShouldEqual("http://foo.bar/");

        It should_set_content_type_to_application_octet =
            () => client.Request.ContentType.ShouldBeEqualIgnoringCase("application/octet-stream");

        It should_serialize_marked_inner_model =
            () => Encoding.UTF8.GetString(client.RequestData.ToArray()).ShouldEqual("Hello World!");
    }

    public class AsyncClientWithMockedRequest<TIn, TOut> : AsyncClient<TIn, TOut>
    {
        public MemoryStream RequestData { get; set; }
        public Mock<HttpWebRequest> RequestMock { get; private set; }
        public string RequestUrl { get; private set; }

        public AsyncClientWithMockedRequest(Uri url)
            : base(url)
        {
            RequestData = new MemoryStream();

            RequestMock = new Mock<HttpWebRequest> { CallBase = true };
            RequestMock.SetupAllProperties();
            RequestMock.Setup(x => x.GetRequestStreamAsync()).Returns(Task.FromResult((Stream)RequestData));
            RequestMock.Object.Headers = new WebHeaderCollection();
        }

        public AsyncClientWithMockedRequest(Uri url, IExecuteStrategy<TIn, Task<TOut>> strategy) 
            : base(url, strategy)
        {
        }

        protected override WebRequest CreateRequest(string url)
        {
            RequestUrl = url;
            return RequestMock.Object;
        }
    }

    public class AsyncClient<TIn, TOut> : AsyncHttpClient<TIn, TOut>
    {
        public BindingContext Context { get; private set; }
        public WebRequest Request { get; private set; }

        public static string ParseResponseExecutionMarker { get; set; }

        public AsyncClient(Uri url)
            : base(url)
        {
            Method = "POST";
        }

        public AsyncClient(Uri url, IExecuteStrategy<TIn, Task<TOut>> strategy)
            : base(url, null, strategy)
        {
            Method = "POST";
        }

        protected override bool ShouldSetAcceptEncoding(BindingContext context)
        {
            return false;
        }

        protected override Task<TOut> ParseResponse(AsyncBindingContext context, WebRequest request)
        {
            Request = request;
            Context = context;
            ParseResponseExecutionMarker = "executed";
            return Task.FromResult(default(TOut));
        }
    }

    public class AsyncInModel
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

    class NoAsyncTestSerializerAttribute : RequestSerializationAttribute
    {
        public static object SerializedObject { get; set; }

        public override void Serialize(object obj, WebRequest request)
        {
            SerializedObject = obj;
            request.ContentLength = 0;
        }
    }

    class AsyncTestSerializerAttribute : RequestSerializationAttribute
    {
        public static object SerializedObject { get; set; }

        public override void Serialize(object obj, WebRequest request)
        {
        }

        public override Task SerializeAsync(object obj, WebRequest request, CancellationToken cancellationToken)
        {
            SerializedObject = obj;
            request.ContentLength = 0;
            return TaskUtils.CompletedTask();
        }
    }
}
