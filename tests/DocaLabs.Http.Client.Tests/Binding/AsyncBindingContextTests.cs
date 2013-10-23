using System;
using System.Threading;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Binding
{
    [Subject(typeof(AsyncBindingContext))]
    class when_asynchronous_binding_context_is_instantiated
    {
        static object http_client;
        static Mock<IClientEndpoint> client_endpoint;
        static Uri base_url;
        static CancellationToken cancellation_token;
        static AsyncBindingContext binding_context;

        Establish context = () =>
        {
            http_client = new object();
            client_endpoint = new Mock<IClientEndpoint>();
            base_url = new Uri("http://foo.bar/");
            cancellation_token = new CancellationTokenSource().Token;
        };

        Because of =
            () => binding_context = new AsyncBindingContext(http_client, "Hello World!", client_endpoint.Object, base_url, typeof(string), typeof(decimal), cancellation_token);

        It should_set_http_client_to_the_specified_value =
            () => binding_context.HttpClient.ShouldBeTheSameAs(http_client);

        It should_set_original_model_to_the_specified_value =
            () => binding_context.OriginalModel.ShouldBeTheSameAs("Hello World!");

        It should_set_model_to_null_value =
            () => binding_context.Model.ShouldBeNull();

        It should_set_configuration_to_the_specified_value =
            () => binding_context.Configuration.ShouldBeTheSameAs(client_endpoint.Object);

        It should_set_base_url_to_the_specified_value =
            () => binding_context.BaseUrl.ShouldBeTheSameAs(base_url);

        It should_set_request_url_to_null_value =
            () => binding_context.RequestUrl.ShouldBeNull();

        It should_set_input_model_type_to_the_specified_value =
            () => binding_context.InputModelType.ShouldBeTheSameAs(typeof(string));

        It should_set_output_model_type_to_the_specified_value =
            () => binding_context.OutputModelType.ShouldBeTheSameAs(typeof(decimal));

        // the CancellationToken is value type!
        It should_set_cancellation_token_to_the_specified_value =
            () => binding_context.CancellationToken.ShouldEqual(cancellation_token);
    }
}
