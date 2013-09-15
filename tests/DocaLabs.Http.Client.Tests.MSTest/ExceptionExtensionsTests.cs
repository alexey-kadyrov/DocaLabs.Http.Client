using System;
using System.Net;
using System.Net.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.MSTest
{
    [TestClass]
    public class ExceptionExtensionsTests
    {
        [TestMethod]
        public void WhenComparingHttpStatusCodeToWebExceptionsWithHttpResponse()
        {
            using (ShimsContext.Create())
            {
                var shimHttpWebResponse = new ShimHttpWebResponse
                {
                    StatusCodeGet = () => HttpStatusCode.NotFound,
                };

                var exception = new WebException("Failed request", null, WebExceptionStatus.ProtocolError, shimHttpWebResponse);

                Assert.IsTrue(exception.Is(HttpStatusCode.NotFound));
                Assert.IsFalse(exception.Is(HttpStatusCode.OK));
            }
        }

        [TestMethod]
        public void WhenComparingHttpStatusCodeToNonWebExceptionsWithWebExceptionInnerExceptionWithHttpResponse()
        {
            using (ShimsContext.Create())
            {
                var shimHttpWebResponse = new ShimHttpWebResponse
                {
                    StatusCodeGet = () => HttpStatusCode.NotFound,
                };

                var innerException = new WebException("Failed request", null, WebExceptionStatus.ProtocolError, shimHttpWebResponse);

                var exception = new Exception("Exception", innerException);

                Assert.IsTrue(exception.Is(HttpStatusCode.NotFound));
                Assert.IsFalse(exception.Is(HttpStatusCode.OK));
            }
        }

        [TestMethod]
        public void WhenComparingHttpStatusCodeToNonWebExceptionsWithWebExceptionInnerExceptionWithHttpResponseInTheChainOfInnerExceptions()
        {
            using (ShimsContext.Create())
            {
                var shimHttpWebResponse = new ShimHttpWebResponse
                {
                    StatusCodeGet = () => HttpStatusCode.NotFound,
                };

                var webException = new WebException("Failed request", null, WebExceptionStatus.ProtocolError, shimHttpWebResponse);

                var exception = new Exception("Exception", new Exception("Exception", new Exception("Exception", webException)));

                Assert.IsTrue(exception.Is(HttpStatusCode.NotFound));
                Assert.IsFalse(exception.Is(HttpStatusCode.OK));
            }
        }
    }
}
