using System;
using System.Net;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests
{
    [Subject(typeof(ExceptionExtensions))]
    class when_comparing_http_status_code_to_null_exceptions
    {
        static Exception exception;
        static bool result;

        Because of =
            () => result = exception.Is(HttpStatusCode.NotFound);

        It should_return_false =
            () => result.ShouldBeFalse();
    }

    [Subject(typeof(ExceptionExtensions))]
    class when_comparing_http_status_code_to_non_web_exceptions_without_any_inner_exceptions
    {
        static Exception exception;
        static bool result;

        Establish context =
            () => exception = new Exception();

        Because of =
            () => result = exception.Is(HttpStatusCode.NotFound);

        It should_return_false =
            () => result.ShouldBeFalse();
    }

    [Subject(typeof(ExceptionExtensions))]
    class when_comparing_http_status_code_to_aggregate_exceptions_without_any_inner_exceptions
    {
        static Exception exception;
        static bool result;

        Establish context =
            () => exception = new AggregateException();

        Because of =
            () => result = exception.Is(HttpStatusCode.NotFound);

        It should_return_false =
            () => result.ShouldBeFalse();
    }

    [Subject(typeof(ExceptionExtensions))]
    class when_comparing_http_status_code_to_aggregate_exceptions_without_web_exeption_in_list_of_inner_exceptions
    {
        static Exception exception;
        static bool result;

        Establish context =
            () => exception = new AggregateException(new Exception(), new Exception());

        Because of =
            () => result = exception.Is(HttpStatusCode.NotFound);

        It should_return_false =
            () => result.ShouldBeFalse();
    }

    [Subject(typeof(ExceptionExtensions))]
    class when_comparing_http_status_code_to_hierarchy_of_aggregate_exceptions_without_web_exeption_in_list_of_inner_exceptions
    {
        static Exception exception;
        static bool result;

        Establish context =
            () => exception = new AggregateException(new AggregateException(new Exception()), new Exception());

        Because of =
            () => result = exception.Is(HttpStatusCode.NotFound);

        It should_return_false =
            () => result.ShouldBeFalse();
    }

    [Subject(typeof(ExceptionExtensions))]
    class when_comparing_http_status_code_to_exception_which_has_hierarchy_of_aggregate_exceptions_without_web_exeption_in_list_of_inner_exceptions
    {
        static Exception exception;
        static bool result;

        Establish context =
            () => exception = new Exception("error", new AggregateException(new AggregateException(new Exception()), new Exception()));

        Because of =
            () => result = exception.Is(HttpStatusCode.NotFound);

        It should_return_false =
            () => result.ShouldBeFalse();
    }

    [Subject(typeof(ExceptionExtensions))]
    class when_comparing_http_status_code_to_non_web_exceptions_with_non_web_exception_inner_exception
    {
        static Exception exception;
        static bool result;

        Establish context =
            () => exception = new Exception("Exception", new Exception());

        Because of =
            () => result = exception.Is(HttpStatusCode.NotFound);

        It should_return_false =
            () => result.ShouldBeFalse();
    }

    [Subject(typeof(ExceptionExtensions))]
    class when_comparing_http_status_code_to_non_web_exceptions_with_chain_of_non_web_exception_inner_exception
    {
        static Exception exception;
        static bool result;

        Establish context =
            () => exception = new Exception("Exception", new Exception("Exception", new Exception("Exception", new Exception())));

        Because of =
            () => result = exception.Is(HttpStatusCode.NotFound);

        It should_return_false =
            () => result.ShouldBeFalse();
    }

    [Subject(typeof(ExceptionExtensions))]
    class when_comparing_http_status_code_to_web_exceptions_without_embeded_response
    {
        static Exception exception;
        static bool result;

        Establish context =
            () => exception = new WebException();

        Because of =
            () => result = exception.Is(HttpStatusCode.NotFound);

        It should_return_false =
            () => result.ShouldBeFalse();
    }

    [Subject(typeof(ExceptionExtensions))]
    class when_comparing_http_status_code_to_non_web_exceptions_with_web_exception_inner_exception_without_embeded_response
    {
        static Exception exception;
        static bool result;

        Establish context =
            () => exception = new Exception("Exception", new WebException());

        Because of =
            () => result = exception.Is(HttpStatusCode.NotFound);

        It should_return_false =
            () => result.ShouldBeFalse();
    }

    [Subject(typeof(ExceptionExtensions))]
    class when_comparing_http_status_code_to_web_exceptions_with_non_http_response
    {
        static Exception exception;
        static bool result;

        Establish context =
            () => exception = new WebException("Exception", null, WebExceptionStatus.ConnectFailure, new Mock<WebResponse>().Object);

        Because of =
            () => result = exception.Is(HttpStatusCode.NotFound);

        It should_return_false =
            () => result.ShouldBeFalse();
    }

    [Subject(typeof(ExceptionExtensions))]
    class when_comparing_http_status_code_to_non_web_exceptions_with_web_exception_inner_exception_with_non_http_response
    {
        static Exception exception;
        static bool result;

        Establish context =
            () => exception = new Exception("Exception", new WebException("Exception", null, WebExceptionStatus.ConnectFailure, new Mock<WebResponse>().Object));

        Because of =
            () => result = exception.Is(HttpStatusCode.NotFound);

        It should_return_false =
            () => result.ShouldBeFalse();
    }
}
