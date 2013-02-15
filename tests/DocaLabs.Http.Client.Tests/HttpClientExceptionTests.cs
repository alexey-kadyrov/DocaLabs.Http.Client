using DocaLabs.Testing.Common.MSpec;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests
{
    [Subject(typeof(HttpClientException))]
    class when_http_client_exception_is_newed_using_default_constructor : ExceptionIsNewedUsingDefaultConstructorContext<HttpClientException>
    {
        Behaves_like<ExceptionIsNewedUsingDefaultConstructorBehaviour> a_standard_exception;
    }

    [Subject(typeof(HttpClientException))]
    class when_http_client_exception_is_newed_using_overload_constructor_with_message : ExceptionIsNewedUsingOverloadConstructorWithMessageContext<HttpClientException>
    {
        Behaves_like<ExceptionIsNewedUsingOverloadConstructorWithMessageBehaviour> a_standard_exception;
    }

    [Subject(typeof(HttpClientException))]
    class when_http_client_exception_is_newed_using_overload_constructor_with_message_and_inner_exception : ExceptionIsNewedUsingOverloadConstructorWithMessageAndInnerExceptionContext<HttpClientException>
    {
        Behaves_like<ExceptionIsNewedUsingOverloadConstructorWithMessageAndInnerExceptionBehaviour> a_standard_exception;
    }

    [Subject(typeof(HttpClientException))]
    class when_http_client_exception_is_serialized : ExceptionIsSerializedContext<HttpClientException>
    {
        Behaves_like<ExceptionIsSerializedBehaviour> a_standard_exception;
    }
}
