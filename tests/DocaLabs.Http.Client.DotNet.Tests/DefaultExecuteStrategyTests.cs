using System;
using System.Net;
using System.Net.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests
{
    [TestClass]
    public class DefaultExecuteStrategyTests
    {
        [TestMethod]
        public void WhenExecutingActionThrowsWebExceptionWith404()
        {
            using (ShimsContext.Create())
            {
                var lastModified = DateTime.Now;

                var shimHttpWebResponse = new ShimHttpWebResponse
                {
                    StatusCodeGet = () => HttpStatusCode.NotFound,
                    StatusDescriptionGet = () => "Entity not found.",
                    SupportsHeadersGet = () => true,
                    LastModifiedGet = () => lastModified,
                    HeadersGet = () => new WebHeaderCollection { { "Server", "Test" } }
                };

                var targetException = new WebException("Failed request", null, WebExceptionStatus.ProtocolError, shimHttpWebResponse);

                var strategy = new DefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1) });

                Exception exception = null;
                var attempts = 0;

                var started = DateTime.UtcNow;

                try
                {
                    strategy.Execute("Hello World!", x => { ++attempts; throw targetException; });
                }
                catch (Exception e)
                {
                    exception = e;
                }

                var duration = DateTime.UtcNow - started;

                Assert.IsNotNull(exception);

                Assert.AreEqual(1, attempts);
                Assert.IsTrue(duration < TimeSpan.FromSeconds(1));

            }
        }

        [TestMethod]
        public void WhenExecutingActionThrowsWebExceptionWithConnectionFailure()
        {
            using (ShimsContext.Create())
            {
                var lastModified = DateTime.Now;

                var shimHttpWebResponse = new ShimHttpWebResponse
                {
                    StatusCodeGet = () => HttpStatusCode.Conflict,
                    StatusDescriptionGet = () => "Entity not found.",
                    SupportsHeadersGet = () => true,
                    LastModifiedGet = () => lastModified,
                    HeadersGet = () => new WebHeaderCollection { { "Server", "Test" } }
                };

                var targetException = new WebException("Failed request", null, WebExceptionStatus.ConnectFailure, shimHttpWebResponse);

                var strategy = new DefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200) });

                Exception exception = null;
                var attempts = 0;

                var started = DateTime.UtcNow;

                try
                {
                    strategy.Execute("Hello World!", x => { ++attempts; throw targetException; });
                }
                catch (Exception e)
                {
                    exception = e;
                }

                var duration = DateTime.UtcNow - started;

                Assert.IsNotNull(exception);

                Assert.AreEqual(3, attempts);
                Assert.IsTrue(duration > TimeSpan.FromMilliseconds(300) && duration < TimeSpan.FromSeconds(1));
            }
        }

        [TestMethod]
        public void WhenExecutingActionThrowsWebExceptionWithRequestTimeout()
        {
            using (ShimsContext.Create())
            {
                var lastModified = DateTime.Now;

                var shimHttpWebResponse = new ShimHttpWebResponse
                {
                    StatusCodeGet = () => HttpStatusCode.RequestTimeout,
                    StatusDescriptionGet = () => "Entity not found.",
                    SupportsHeadersGet = () => true,
                    LastModifiedGet = () => lastModified,
                    HeadersGet = () => new WebHeaderCollection { { "Server", "Test" } }
                };

                var targetException = new WebException("Failed request", null, WebExceptionStatus.ProtocolError, shimHttpWebResponse);

                var strategy = new DefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200) });

                Exception exception = null;
                var attempts = 0;

                var started = DateTime.UtcNow;

                try
                {
                    strategy.Execute("Hello World!", x => { ++attempts; throw targetException; });
                }
                catch (Exception e)
                {
                    exception = e;
                }

                var duration = DateTime.UtcNow - started;

                Assert.IsNotNull(exception);

                Assert.AreEqual(3, attempts);
                Assert.IsTrue(duration > TimeSpan.FromMilliseconds(300) && duration < TimeSpan.FromSeconds(1));
            }
        }

        [TestMethod]
        public void WhenExecutingActionThrowsWebExceptionWithGatewayTimeout()
        {
            using (ShimsContext.Create())
            {
                var lastModified = DateTime.Now;

                var shimHttpWebResponse = new ShimHttpWebResponse
                {
                    StatusCodeGet = () => HttpStatusCode.GatewayTimeout,
                    StatusDescriptionGet = () => "Entity not found.",
                    SupportsHeadersGet = () => true,
                    LastModifiedGet = () => lastModified,
                    HeadersGet = () => new WebHeaderCollection { { "Server", "Test" } }
                };

                var targetException = new WebException("Failed request", null, WebExceptionStatus.ProtocolError, shimHttpWebResponse);

                var strategy = new DefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200) });

                Exception exception = null;
                var attempts = 0;

                var started = DateTime.UtcNow;

                try
                {
                    strategy.Execute("Hello World!", x => { ++attempts; throw targetException; });
                }
                catch (Exception e)
                {
                    exception = e;
                }

                var duration = DateTime.UtcNow - started;

                Assert.IsNotNull(exception);

                Assert.AreEqual(3, attempts);
                Assert.IsTrue(duration > TimeSpan.FromMilliseconds(300) && duration < TimeSpan.FromSeconds(1));
            }
        }
    }
}
