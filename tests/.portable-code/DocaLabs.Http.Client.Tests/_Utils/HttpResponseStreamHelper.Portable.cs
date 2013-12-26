using System;
using System.IO;
using System.Threading;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Configuration;

namespace DocaLabs.Http.Client.Tests._Utils
{
    public class HttpResponseStreamHelper : IDisposable
    {
        public MockWebRequest MockRequest { get; private set; }
        public MockWebResponse MockResponse { get; private set; }
        public HttpResponseStream ResponseStream { get; private set; }

        HttpResponseStreamHelper()
        {
        }

        public static HttpResponseStreamHelper Setup(string responseContentType, Stream dataForResponse)
        {
            var context = new BindingContext(new object(), "", new ClientEndpoint(), new Uri("http://foo.bar"), typeof(string), typeof(string));

            var helper = new HttpResponseStreamHelper { MockResponse = new MockWebResponse(dataForResponse, responseContentType) };

            helper.MockRequest = new MockWebRequest(helper.MockResponse);
            helper.ResponseStream = (HttpResponseStream)new HttpResponseStreamFactory().CreateStream(context, helper.MockRequest);

            return helper;
        }

        public static HttpResponseStreamHelper SetupAsync(string responseContentType, Stream dataForResponse)
        {
            var context = new AsyncBindingContext(new object(), "", new ClientEndpoint(), new Uri("http://foo.bar"), typeof(string), typeof(string), CancellationToken.None);

            var helper = new HttpResponseStreamHelper { MockResponse = new MockWebResponse(dataForResponse, responseContentType) };

            helper.MockRequest = new MockWebRequest(helper.MockResponse);

            helper.ResponseStream = (HttpResponseStream)(new HttpResponseStreamFactory().CreateAsyncStream(context, helper.MockRequest).Result);

            return helper;
        }

        public void Dispose()
        {
            if(ResponseStream != null)
                ResponseStream.Dispose();
        }
    }
}