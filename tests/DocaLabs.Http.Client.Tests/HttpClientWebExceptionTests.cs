using System;
using System.Net;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests
{
    [Subject(typeof(HttpClientWebException))]
    class when_initializing_http_client_web_exception_with_web_expception_without_response_as_inner_exception
    {
        static HttpClientWebException exception;

        Because of =
            () => exception = new HttpClientWebException("Hello World!", new WebException("Failed to connect", WebExceptionStatus.ConnectFailure));

        It should_return_null_response =
            () => exception.Response.ShouldBeNull();

        It should_return_empty_content_type =
            () => exception.ContentType.ShouldBeEmpty();

        It should_return_status_from_the_inner_web_exception =
            () => exception.Status.ShouldEqual(WebExceptionStatus.ConnectFailure);

        It should_return_minus_one_status_code =
            () => exception.StatusCode.ShouldEqual(-1);

        It should_return_empty_status_description =
            () => exception.StatusDescription.ShouldBeEmpty();

        It should_return_empty_etag =
            () => exception.ETag.ShouldBeEmpty();

        It should_return_minimum_value_for_last_modified =
            () => exception.LastModified.ShouldEqual(DateTime.MinValue);

        It should_return_empty_header_collection =
            () => exception.Headers.ShouldBeEmpty();
    }

    [Subject(typeof(HttpClientWebException))]
    class when_initializing_http_client_web_exception_with_null_inner_exception
    {
        static HttpClientWebException exception;

        Because of =
            () => exception = new HttpClientWebException("Hello World!", null);

        It should_return_null_response =
            () => exception.Response.ShouldBeNull();

        It should_return_empty_content_type =
            () => exception.ContentType.ShouldBeEmpty();

        It should_return_status_as_unknown_error =
            () => exception.Status.ShouldEqual(WebExceptionStatus.UnknownError);

        It should_return_minus_one_status_code =
            () => exception.StatusCode.ShouldEqual(-1);

        It should_return_empty_status_description =
            () => exception.StatusDescription.ShouldBeEmpty();

        It should_return_empty_etag =
            () => exception.ETag.ShouldBeEmpty();

        It should_return_minimum_value_for_last_modified =
            () => exception.LastModified.ShouldEqual(DateTime.MinValue);

        It should_return_empty_header_collection =
            () => exception.Headers.ShouldBeEmpty();
    }

    [Subject(typeof(HttpClientWebException))]
    class when_initializing_http_client_web_exception_with_inner_exception_which_is_not_derived_from_web_exception
    {
        static HttpClientWebException exception;

        Because of =
            () => exception = new HttpClientWebException("Hello World!", new ArgumentException());

        It should_return_null_response =
            () => exception.Response.ShouldBeNull();

        It should_return_empty_content_type =
            () => exception.ContentType.ShouldBeEmpty();

        It should_return_status_as_unknown_error =
            () => exception.Status.ShouldEqual(WebExceptionStatus.UnknownError);

        It should_return_minus_one_status_code =
            () => exception.StatusCode.ShouldEqual(-1);

        It should_return_empty_status_description =
            () => exception.StatusDescription.ShouldBeEmpty();

        It should_return_empty_etag =
            () => exception.ETag.ShouldBeEmpty();

        It should_return_minimum_value_for_last_modified =
            () => exception.LastModified.ShouldEqual(DateTime.MinValue);

        It should_return_empty_header_collection =
            () => exception.Headers.ShouldBeEmpty();
    }
}
