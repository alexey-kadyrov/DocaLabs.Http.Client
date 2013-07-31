using System;
using System.IO;
using System.Net;
using System.Net.Fakes;
using System.Text;
using DocaLabs.Http.Client.Binding;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DocaLabs.Http.Client.Tests.MSTest
{
    [TestClass]
    public class DefaultResponseBinderTests
    {
        [TestMethod]
        public void WhenReadingToRichResponse()
        {
            using (ShimsContext.Create())
            {
                var stream = new MemoryStream(Encoding.UTF8.GetBytes("<Model><Value>Hello World!</Value></Model>"));

                var shimWebResponse = new ShimWebResponse(new ShimHttpWebResponse
                {
                    SupportsHeadersGet = () => false,
                    ContentTypeGet = () => "text/xml; charset=utf-8",
                    ContentLengthGet = () => stream.Length,
                    GetResponseStream = () => stream
                });

                var mockRequest = new Mock<WebRequest>();
                mockRequest.Setup(x => x.GetResponse()).Returns(shimWebResponse);

                var binder = new DefaultResponseBinder();
                var bindingContext = new BindingContext(new Client(), null, null, null);

                var data = (RichResponse<Model>)binder.Read(bindingContext, mockRequest.Object, typeof(RichResponse<Model>));

                Assert.IsNotNull(data);
                Assert.AreEqual("Hello World!", data.Value.Value);
            }
        }
    }

    class Client : HttpClient<string, Model>
    {
        public Client()
            : base(new Uri("http://foo.bar"))
        {
        }
    }

    public class Model
    {
        public string Value { get; set; }
    }
}
