using System;
using System.Net;
using System.Net.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.MSTest
{
    [TestClass]
    public class RichResponseTests
    {
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
    }

}
//namespace DocaLabs.Http.Client.Tests
//{
//    [Subject(typeof(RichResponse<>))]
//    class when_rich_response_is_initialized_for_web_response_which_does_not_support_headers
//    {
//        static Mock<WebResponse> web_response;
//        static RichResponse<string> rich_response;

//        Establish context = () =>
//        {
//            web_response = new Mock<WebResponse>();
//            web_response.Setup(x => x.SupportsHeaders).Returns(false);
//        };

//        Because of = 
//            () => rich_response = new RichResponse<string>(web_response.Object, "Hello World!");

//        It should_initialize_status_code_to_zero =
//            () => rich_response.StatusCode.ShouldEqual(0);

//        It should_not_initialize_status_description =
//            () => rich_response.StatusDescription.ShouldBeNull();

//        It should_not_initialize_etag =
//            () => rich_response.ETag.ShouldBeNull();

//        It should_initialize_empty_header_collection =
//            () => rich_response.Headers.ShouldBeEmpty();

//        It should_still_initialize_value =
//            () => rich_response.Value.ShouldEqual("Hello World!");
//    }

//    [Subject(typeof(RichResponse<>))]
//    class when_rich_response_is_initialized_for_null_response
//    {
//        static Exception exception;

//        Because of =
//            () => exception = Catch.Exception(() => new RichResponse<string>(null, "Hello World!"));

//        It should_throw_argument_null_exception =
//            () => exception.ShouldBeOfType<ArgumentNullException>();

//        It should_report_response_argument =
//            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("response");
//    }

//    [Subject(typeof(RichResponse<>))]
//    class when_rich_response_is_initialized_for_web_response_which_supports_headers
//    {
//        static Mock<WebResponse> web_response;
//        static RichResponse<string> rich_response;

//        Establish context = () =>
//        {
//            web_response = new Mock<WebResponse>();
//            web_response.Setup(x => x.SupportsHeaders).Returns(true);
//            web_response.Setup(x => x.Headers).Returns(new WebHeaderCollection { { "ETag", "W/\"123\"" } });
//        };

//        Because of = 
//            () => rich_response = new RichResponse<string>(web_response.Object, "Hello World!");

//        It should_initialize_status_code_to_zero =
//            () => rich_response.StatusCode.ShouldEqual(0);

//        It should_not_initialize_status_description =
//            () => rich_response.StatusDescription.ShouldBeNull();

//        It should_initialize_etag =
//            () => rich_response.ETag.ShouldEqual("W/\"123\"");

//        It should_initialize_all_keys_in_header_collection =
//            () => rich_response.Headers.AllKeys.ShouldContainOnly("ETag");

//        It should_initialize_all_values_in_header_collection =
//            () => rich_response.Headers["ETag"].ShouldContainOnly("W/\"123\"");

//        It should_still_initialize_value =
//            () => rich_response.Value.ShouldEqual("Hello World!");
//    }

//    [Subject(typeof(RichResponse<>))]
//    class when_rich_response_is_initialized_for_http_web_response
//    {
//        static IDisposable shim_context;
//        static ShimHttpWebResponse shim_http_web_response;
//        static RichResponse<string> rich_response;

//        Cleanup after =
//            () => shim_context.Dispose();

//        Establish context = () =>
//        {
//            shim_context = ShimsContext.Create();
//            shim_http_web_response = new ShimHttpWebResponse
//            {
//                StatusCodeGet = () => HttpStatusCode.Conflict,
//                StatusDescriptionGet = () => "Conflict on the server.",
//                SupportsHeadersGet = () => true,
//                LastModifiedGet = () => DateTime.Now
//            };

//            //http_web_response = new Mock<HttpWebResponse>();
//            //http_web_response.Setup(x => x.StatusCode).Returns(HttpStatusCode.Conflict);
//            //http_web_response.Setup(x => x.StatusDescription).Returns("Conflict on the server.");
//            //http_web_response.Setup(x => x.SupportsHeaders).Returns(true);
//            //http_web_response.Setup(x => x.Headers).Returns(new WebHeaderCollection { { "ETag", "W/\"123\"" } });
//        };

//        Because of = 
//            () => rich_response = new RichResponse<string>(shim_http_web_response, "Hello World!");

//        It should_initialize_status_code =
//            () => rich_response.StatusCode.ShouldEqual(409);

//        It should_initialize_status_description =
//            () => rich_response.StatusDescription.ShouldEqual("Conflict on the server.");

//        It should_initialize_etag =
//            () => rich_response.ETag.ShouldEqual("W/\"123\"");

//        It should_initialize_all_keys_in_header_collection =
//            () => rich_response.Headers.AllKeys.ShouldContainOnly("ETag");

//        It should_initialize_all_values_in_header_collection =
//            () => rich_response.Headers["ETag"].ShouldContainOnly("W/\"123\"");

//        It should_initialize_value =
//            () => rich_response.Value.ShouldEqual("Hello World!");
//    }

//    [Subject(typeof(RichResponse<>))]
//    class when_rich_response_is_initialized_for_http_web_response_without_etag_header
//    {
//        static Mock<HttpWebResponse> http_web_response;
//        static RichResponse<string> rich_response;

//        Establish context = () =>
//        {
//            http_web_response = new Mock<HttpWebResponse>();
//            http_web_response.Setup(x => x.StatusCode).Returns(HttpStatusCode.Conflict);
//            http_web_response.Setup(x => x.StatusDescription).Returns("Conflict on the server.");
//            http_web_response.Setup(x => x.SupportsHeaders).Returns(true);
//            http_web_response.Setup(x => x.Headers).Returns(new WebHeaderCollection { { "custom-header", "custom-value" } });
//        };

//        Because of = 
//            () => rich_response = new RichResponse<string>(http_web_response.Object, "Hello World!");

//        It should_initialize_status_code =
//            () => rich_response.StatusCode.ShouldEqual(409);

//        It should_initialize_status_description =
//            () => rich_response.StatusDescription.ShouldEqual("Conflict on the server.");

//        It should_initialize_etag_to_null =
//            () => rich_response.ETag.ShouldBeNull();

//        It should_initialize_all_keys_in_header_collection =
//            () => rich_response.Headers.AllKeys.ShouldContainOnly("custom-header");

//        It should_initialize_all_values_in_header_collection =
//            () => rich_response.Headers["custom-header"].ShouldContainOnly("custom-value");

//        It should_initialize_value =
//            () => rich_response.Value.ShouldEqual("Hello World!");
//    }

//    [Subject(typeof(RichResponse<>))]
//    class when_rich_response_is_initialized_with_null_value_for_value_type
//    {
//        static Mock<HttpWebResponse> http_web_response;
//        static RichResponse<int> rich_response;

//        Establish context = () =>
//        {
//            http_web_response = new Mock<HttpWebResponse>();
//            http_web_response.Setup(x => x.StatusCode).Returns(HttpStatusCode.Conflict);
//            http_web_response.Setup(x => x.StatusDescription).Returns("Conflict on the server.");
//            http_web_response.Setup(x => x.SupportsHeaders).Returns(true);
//            http_web_response.Setup(x => x.Headers).Returns(new WebHeaderCollection { { "ETag", "W/\"123\"" } });
//        };

//        Because of = 
//            () => rich_response = new RichResponse<int>(http_web_response.Object, null);

//        It should_initialize_status_code =
//            () => rich_response.StatusCode.ShouldEqual(409);

//        It should_initialize_status_description =
//            () => rich_response.StatusDescription.ShouldEqual("Conflict on the server.");

//        It should_initialize_etag =
//            () => rich_response.ETag.ShouldEqual("W/\"123\"");

//        It should_initialize_all_keys_in_header_collection =
//            () => rich_response.Headers.AllKeys.ShouldContainOnly("ETag");

//        It should_initialize_all_values_in_header_collection =
//            () => rich_response.Headers["ETag"].ShouldContainOnly("W/\"123\"");

//        It should_set_value_to_deafult =
//            () => rich_response.Value.ShouldEqual(0);
//    }

//    [Subject(typeof(RichResponse<>))]
//    class when_comparing_http_status_code
//    {
//        static Mock<HttpWebResponse> http_web_response;
//        static RichResponse<string> rich_response;

//        Establish context = () =>
//        {
//            http_web_response = new Mock<HttpWebResponse>();
//            http_web_response.Setup(x => x.StatusCode).Returns(HttpStatusCode.Conflict);
//            http_web_response.Setup(x => x.StatusDescription).Returns("Conflict on the server.");
//            http_web_response.Setup(x => x.SupportsHeaders).Returns(true);
//            http_web_response.Setup(x => x.Headers).Returns(new WebHeaderCollection());
//        };

//        Because of =
//            () => rich_response = new RichResponse<string>(http_web_response.Object, "Hello World!");

//        It should_return_true_if_codes_are_equal =
//            () => rich_response.Is(HttpStatusCode.Conflict).ShouldBeTrue();

//        It should_return_false_if_codes_are_not_equal =
//            () => rich_response.Is(HttpStatusCode.ExpectationFailed).ShouldBeFalse();
//    }
//}
