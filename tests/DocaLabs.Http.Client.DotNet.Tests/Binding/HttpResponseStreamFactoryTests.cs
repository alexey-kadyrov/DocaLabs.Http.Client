namespace DocaLabs.Http.Client.Tests.Binding
{
    //[TestClass]
    //public class when_http_response_is_newed_with_null_request
    //{
    //    static Exception exception;

    //    Because of =
    //        () => exception = Catch.Exception(() => HttpResponseStream.CreateResponseStream(null));

    //    It should_throw_argument_null_exception =
    //        () => exception.ShouldBeOfType<ArgumentNullException>();

    //    It should_report_that_the_request_argument =
    //        () => ((ArgumentNullException)exception).ParamName.ShouldContain("request");
    //}

    //    [TestClass]
    //    class when_asynchronous_http_response_is_newed_with_null_request
    //    {
    //        static Exception exception;

    //        Because of =
    //            () => exception = Catch.Exception(() => HttpResponseStream.CreateAsyncResponseStream(null, CancellationToken.None).Wait());

    //        It should_throw_argument_null_exception =
    //            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<ArgumentNullException>();

    //        It should_report_that_the_request_argument =
    //            () => ((ArgumentNullException)((AggregateException)exception).InnerExceptions[0]).ParamName.ShouldContain("request");
    //    }

    //    [TestClass]
    //    class when_http_response_is_newed_and_the_stream_is_null
    //    {
    //        static Mock<WebRequest> mock_request;
    //        static Mock<WebResponse> mock_response;
    //        static Exception exception;

    //        Establish context = () =>
    //        {
    //            mock_response = new Mock<WebResponse>();
    //            mock_response.SetupAllProperties();
    //            mock_response.Setup(x => x.GetResponseStream()).Returns((Stream)null);
    //            mock_response.Object.ContentType = "plain/text; charset=utf-8";
    //            mock_response.Object.ContentLength = 0;

    //            mock_request = new Mock<WebRequest>();
    //            mock_request.Setup(x => x.GetResponse()).Returns(mock_response.Object);
    //        };

    //        Because of =
    //            () => exception = Catch.Exception(() => HttpResponseStream.CreateResponseStream(mock_request.Object));

    //        It should_throw_exception =
    //            () => exception.ShouldBeOfType<Exception>();

    //        It should_report_that_the_response_stream_is_null =
    //            () => exception.Message.ShouldContain("Response stream is null");
    //    }

    //    [TestClass]
    //    class when_asynchronous_http_response_is_newed_and_the_stream_is_null
    //    {
    //        static Mock<WebRequest> mock_request;
    //        static Mock<WebResponse> mock_response;
    //        static Exception exception;

    //        Establish context = () =>
    //        {
    //            mock_response = new Mock<WebResponse>();
    //            mock_response.SetupAllProperties();
    //            mock_response.Setup(x => x.GetResponseStream()).Returns((Stream)null);
    //            mock_response.Object.ContentType = "plain/text; charset=utf-8";
    //            mock_response.Object.ContentLength = 0;

    //            mock_request = new Mock<WebRequest>();
    //            mock_request.Setup(x => x.GetResponseAsync()).Returns(Task.FromResult(mock_response.Object));
    //        };

    //        Because of =
    //            () => exception = Catch.Exception(() => HttpResponseStream.CreateAsyncResponseStream(mock_request.Object, CancellationToken.None).Wait());

    //        It should_throw_exception =
    //            () => exception.ShouldBeOfType<Exception>();

    //        It should_report_that_the_response_stream_is_null =
    //            () => ((AggregateException)exception).InnerExceptions[0].Message.ShouldContain("Response stream is null");
    //    }
}
