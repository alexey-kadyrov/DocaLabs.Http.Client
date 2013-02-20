using DocaLabs.Testing.Common;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests
{
    [Subject(typeof(UnrecoverableHttpClientException))]
    class when_unrecoverable_http_client_exception_is_newed_using_default_constructor : ExceptionIsNewedUsingDefaultConstructorContext<UnrecoverableHttpClientException>
    {
        Behaves_like<ExceptionIsNewedUsingDefaultConstructorBehaviour> a_standard_exception;
    }

    [Subject(typeof(UnrecoverableHttpClientException))]
    class when_unrecoverable_http_client_exception_is_newed_using_overload_constructor_with_message : ExceptionIsNewedUsingOverloadConstructorWithMessageContext<UnrecoverableHttpClientException>
    {
        Behaves_like<ExceptionIsNewedUsingOverloadConstructorWithMessageBehaviour> a_standard_exception;
    }

    [Subject(typeof(UnrecoverableHttpClientException))]
    class when_unrecoverable_http_client_exception_is_newed_using_overload_constructor_with_message_and_inner_exception : ExceptionIsNewedUsingOverloadConstructorWithMessageAndInnerExceptionContext<UnrecoverableHttpClientException>
    {
        Behaves_like<ExceptionIsNewedUsingOverloadConstructorWithMessageAndInnerExceptionBehaviour> a_standard_exception;
    }

    [Subject(typeof(UnrecoverableHttpClientException))]
    class when_unrecoverable_http_client_exception_is_serialized : ExceptionIsSerializedContext<UnrecoverableHttpClientException>
    {
        Behaves_like<ExceptionIsSerializedBehaviour> a_standard_exception;
    }
}
