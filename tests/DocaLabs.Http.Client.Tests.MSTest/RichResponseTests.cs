using System;
using System.Net;
using System.Net.Fakes;
using System.Runtime.CompilerServices;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.MSTest
{
    [TestClass]
    public class RichResponseTests
    {
        [TestMethod]
        public void WhenRichResponseIsInitializedForWebResponseWhichDoesNotSupportHeaders()
        {
            using (ShimsContext.Create())
            {
                var shimWebResponse = new ShimWebResponse(new ShimHttpWebResponse
                {
                    SupportsHeadersGet = () => false
                });

                var richResponse = new RichResponse<string>(shimWebResponse, "Hello World!");

                Assert.AreEqual(0, richResponse.StatusCode);
                Assert.IsNull(richResponse.StatusDescription);
                Assert.IsNull(richResponse.ETag);
                Assert.AreEqual(DateTime.MinValue, richResponse.LastModified);
                Assert.AreEqual(0, richResponse.Headers.AllKeys.Length);
                Assert.AreEqual("Hello World!", richResponse.Value);
            }
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException)), MethodImpl(MethodImplOptions.NoOptimization)]
        public void WhenRichResponseIsInitializedForNullResponse()
        {
            // ReSharper disable UnusedVariable
            var richResponse = new RichResponse<string>(null, "Hello World!");
            // ReSharper restore UnusedVariable
        }

        [TestMethod]
        public void WhenRichResponseIsInitializedForWebResponseWhichSupportsHeaders()
        {
            using (ShimsContext.Create())
            {
                var webResponse = new ShimWebResponse(new ShimHttpWebResponse
                {
                    SupportsHeadersGet = () => true,
                    HeadersGet = () => new WebHeaderCollection { { "ETag", "W/\"123\"" } }
                });

                var richResponse = new RichResponse<string>(webResponse, "Hello World!");

                Assert.AreEqual(0, richResponse.StatusCode);
                Assert.IsNull(richResponse.StatusDescription);
                Assert.AreEqual("W/\"123\"", richResponse.ETag);
                Assert.AreEqual(DateTime.MinValue, richResponse.LastModified);

                Assert.AreEqual(1, richResponse.Headers.AllKeys.Length);
                Assert.AreEqual("ETag", richResponse.Headers.AllKeys[0]);
                Assert.AreEqual("W/\"123\"", richResponse.Headers["ETag"]);

                Assert.AreEqual("Hello World!", richResponse.Value);
            }
        }

        [TestMethod]
        public void WhenRichResponseIsInitializedForHttpWebResponse()
        {
            using (ShimsContext.Create())
            {
                var lastModified = DateTime.Now;

                var shimHttpWebResponse = new ShimHttpWebResponse
                {
                    StatusCodeGet = () => HttpStatusCode.Conflict,
                    StatusDescriptionGet = () => "Conflict on the server.",
                    SupportsHeadersGet = () => true,
                    LastModifiedGet = () => lastModified,
                    HeadersGet = () => new WebHeaderCollection { { "ETag", "W/\"123\"" } }
                };

                var richResponse = new RichResponse<string>(shimHttpWebResponse, "Hello World!");

                Assert.AreEqual(409, richResponse.StatusCode);
                Assert.AreEqual("Conflict on the server.", richResponse.StatusDescription);
                Assert.AreEqual("W/\"123\"", richResponse.ETag);
                Assert.AreEqual(lastModified.ToUniversalTime(), richResponse.LastModified);

                Assert.AreEqual(1, richResponse.Headers.AllKeys.Length);
                Assert.AreEqual("ETag", richResponse.Headers.AllKeys[0]);
                Assert.AreEqual("W/\"123\"", richResponse.Headers["ETag"]);

                Assert.AreEqual("Hello World!", richResponse.Value);
            }
        }

        [TestMethod]
        public void WhenRichResponseIsInitializedForFtpWebResponse()
        {
            using (ShimsContext.Create())
            {
                var lastModified = DateTime.Now;

                var shimHttpWebResponse = new ShimFtpWebResponse
                {
                    StatusCodeGet = () => FtpStatusCode.ClosingData,
                    StatusDescriptionGet = () => "Closing data.",
                    SupportsHeadersGet = () => true,
                    LastModifiedGet = () => lastModified,
                    HeadersGet = () => new WebHeaderCollection { { "ETag", "W/\"123\"" } }
                };

                var richResponse = new RichResponse<string>(shimHttpWebResponse, "Hello World!");

                Assert.AreEqual(226, richResponse.StatusCode);
                Assert.AreEqual("Closing data.", richResponse.StatusDescription);
                Assert.AreEqual("W/\"123\"", richResponse.ETag);
                Assert.AreEqual(lastModified.ToUniversalTime(), richResponse.LastModified);

                Assert.AreEqual(1, richResponse.Headers.AllKeys.Length);
                Assert.AreEqual("ETag", richResponse.Headers.AllKeys[0]);
                Assert.AreEqual("W/\"123\"", richResponse.Headers["ETag"]);

                Assert.AreEqual("Hello World!", richResponse.Value);
            }
        }

        [TestMethod]
        public void WhenRichResponseIsInitializedForHttpWebResponseWithoutEtagHeader()
        {
            using (ShimsContext.Create())
            {
                var lastModified = DateTime.Now;

                var httpWebResponse = new ShimHttpWebResponse
                {
                    StatusCodeGet = () => HttpStatusCode.Conflict,
                    StatusDescriptionGet = () => "Conflict on the server.",
                    SupportsHeadersGet = () => true,
                    LastModifiedGet = () => lastModified,
                    HeadersGet = () => new WebHeaderCollection { { "custom-header", "custom-value" } }
                };

                var richResponse = new RichResponse<string>(httpWebResponse, "Hello World!");

                Assert.AreEqual(409, richResponse.StatusCode);
                Assert.AreEqual("Conflict on the server.", richResponse.StatusDescription);
                Assert.IsNull(richResponse.ETag);
                Assert.AreEqual(lastModified.ToUniversalTime(), richResponse.LastModified);

                Assert.AreEqual(1, richResponse.Headers.AllKeys.Length);
                Assert.AreEqual("custom-header", richResponse.Headers.AllKeys[0]);
                Assert.AreEqual("custom-value", richResponse.Headers["custom-header"]);

                Assert.AreEqual("Hello World!", richResponse.Value);
            }
        }

        [TestMethod]
        public void WhenRichResponseIsInitializedForFtpWebResponseWithoutEtagHeader()
        {
            using (ShimsContext.Create())
            {
                var lastModified = DateTime.Now;

                var httpWebResponse = new ShimFtpWebResponse
                {
                    StatusCodeGet = () => FtpStatusCode.ClosingData,
                    StatusDescriptionGet = () => "Closing data.",
                    SupportsHeadersGet = () => true,
                    LastModifiedGet = () => lastModified,
                    HeadersGet = () => new WebHeaderCollection { { "custom-header", "custom-value" } }
                };

                var richResponse = new RichResponse<string>(httpWebResponse, "Hello World!");

                Assert.AreEqual(226, richResponse.StatusCode);
                Assert.AreEqual("Closing data.", richResponse.StatusDescription);
                Assert.IsNull(richResponse.ETag);
                Assert.AreEqual(lastModified.ToUniversalTime(), richResponse.LastModified);

                Assert.AreEqual(1, richResponse.Headers.AllKeys.Length);
                Assert.AreEqual("custom-header", richResponse.Headers.AllKeys[0]);
                Assert.AreEqual("custom-value", richResponse.Headers["custom-header"]);

                Assert.AreEqual("Hello World!", richResponse.Value);
            }
        }

        [TestMethod]
        public void WhenRichResponseIsInitializedWithNullValueForValueType()
        {
            using (ShimsContext.Create())
            {
                var lastModified = DateTime.Now;

                var httpWebResponse = new ShimHttpWebResponse
                {
                    StatusCodeGet = () => HttpStatusCode.Conflict,
                    StatusDescriptionGet = () => "Conflict on the server.",
                    SupportsHeadersGet = () => true,
                    LastModifiedGet = () => lastModified,
                    HeadersGet = () => new WebHeaderCollection { { "custom-header", "custom-value" } }
                };

                var richResponse = new RichResponse<int>(httpWebResponse, null);

                Assert.AreEqual(409, richResponse.StatusCode);
                Assert.AreEqual("Conflict on the server.", richResponse.StatusDescription);
                Assert.IsNull(richResponse.ETag);
                Assert.AreEqual(lastModified.ToUniversalTime(), richResponse.LastModified);

                Assert.AreEqual(1, richResponse.Headers.AllKeys.Length);
                Assert.AreEqual("custom-header", richResponse.Headers.AllKeys[0]);
                Assert.AreEqual("custom-value", richResponse.Headers["custom-header"]);

                Assert.AreEqual(0, richResponse.Value);
            }
        }

        [TestMethod]
        public void WhenComparingHttpStatusCode()
        {
            using (ShimsContext.Create())
            {
                var httpWebResponse = new ShimHttpWebResponse
                {
                    StatusCodeGet = () => HttpStatusCode.Conflict,
                    StatusDescriptionGet = () => "Conflict on the server.",
                    SupportsHeadersGet = () => true,
                    HeadersGet = () => new WebHeaderCollection()
                };

                var richResponse = new RichResponse<string>(httpWebResponse, "Hello World!");

                Assert.IsTrue(richResponse.Is(HttpStatusCode.Conflict));
                Assert.IsFalse(richResponse.Is(HttpStatusCode.ExpectationFailed));
            }
        }

        [TestMethod]
        public void WhenComparingFtpStatusCode()
        {
            using (ShimsContext.Create())
            {
                var httpWebResponse = new ShimFtpWebResponse
                {
                    StatusCodeGet = () => FtpStatusCode.ClosingControl,
                    StatusDescriptionGet = () => "Closing control.",
                    SupportsHeadersGet = () => true,
                    HeadersGet = () => new WebHeaderCollection()
                };

                var richResponse = new RichResponse<string>(httpWebResponse, "Hello World!");

                Assert.IsTrue(richResponse.Is(FtpStatusCode.ClosingControl));
                Assert.IsFalse(richResponse.Is(FtpStatusCode.ClosingData));
            }
        }
    }
}
