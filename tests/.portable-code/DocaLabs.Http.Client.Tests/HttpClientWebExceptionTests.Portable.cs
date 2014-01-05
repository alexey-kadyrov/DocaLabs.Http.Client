using System;
using System.Net;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests
{
    [TestClass]
    public class when_initializing_http_client_web_exception_with_web_expception_without_response_as_inner_exception
    {
        static HttpClientWebException _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = new HttpClientWebException("Hello World!", new WebException("Failed to connect", WebExceptionStatus.ConnectFailure));
        }

        [TestMethod]
        public void it_should_return_null_response()
        {
            _exception.Response.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_return_empty_content_type()
        {
            _exception.ContentType.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_return_status_from_the_inner_web_exception()
        {
            _exception.Status.ShouldEqual(WebExceptionStatus.ConnectFailure);
        }

        [TestMethod]
        public void it_should_return_minus_one_status_code()
        {
            _exception.StatusCode.ShouldEqual(-1);
        }

        [TestMethod]
        public void it_should_return_empty_status_description()
        {
            _exception.StatusDescription.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_return_empty_etag()
        {
            _exception.ETag.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_return_empty_header_collection()
        {
            _exception.Headers.ShouldBeEmpty();
        }
    }

    [TestClass]
    public class when_initializing_http_client_web_exception_with_null_inner_exception
    {
        static HttpClientWebException _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = new HttpClientWebException("Hello World!", (Exception)null);
        }

        [TestMethod]
        public void it_should_return_null_response()
        {
            _exception.Response.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_return_empty_content_type()
        {
            _exception.ContentType.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_return_status_as_unknown_error()
        {
            _exception.Status.ShouldEqual(WebExceptionStatus.UnknownError);
        }

        [TestMethod]
        public void it_should_return_minus_one_status_code()
        {
            _exception.StatusCode.ShouldEqual(-1);
        }

        [TestMethod]
        public void it_should_return_empty_status_description()
        {
            _exception.StatusDescription.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_return_empty_etag()
        {
            _exception.ETag.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_return_empty_header_collection()
        {
            _exception.Headers.ShouldBeEmpty();
        }
    }

    [TestClass]
    public class when_initializing_http_client_web_exception_with_inner_exception_which_is_not_derived_from_web_exception
    {
        static HttpClientWebException _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = new HttpClientWebException("Hello World!", new ArgumentException());
        }

        [TestMethod]
        public void it_should_return_null_response()
        {
            _exception.Response.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_return_empty_content_type()
        {
            _exception.ContentType.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_return_status_as_unknown_error()
        {
            _exception.Status.ShouldEqual(WebExceptionStatus.UnknownError);
        }

        [TestMethod]
        public void it_should_return_minus_one_status_code()
        {
            _exception.StatusCode.ShouldEqual(-1);
        }

        [TestMethod]
        public void it_should_return_empty_status_description()
        {
            _exception.StatusDescription.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_return_empty_etag()
        {
            _exception.ETag.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_return_empty_header_collection()
        {
            _exception.Headers.ShouldBeEmpty();
        }
    }
}
