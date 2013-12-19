using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DocaLabs.Http.Client.Tests.Utils
{
    [TestClass]
    public class when_getting_stream_for_synchronous_request
    {
        static RequestStreamFactory _factory;
        static BindingContext _context;
        static Mock<WebRequest> _request;
        static Mock<Stream> _stream;
        static Stream _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _context = new BindingContext(new object(), new object(), new ClientEndpoint { Timeout = 1111, ReadTimeout = 2222, WriteTimeout = 3333 },
                new Uri("http://foo.bar/"), typeof(string), typeof(string));

            _stream = new Mock<Stream>();
            _stream.SetupAllProperties();
            _stream.SetupGet(x => x.CanTimeout).Returns(true);

            _request = new Mock<WebRequest>();
            _request.SetupAllProperties();
            _request.Setup(x => x.GetRequestStream()).Returns(_stream.Object);

            _factory = new RequestStreamFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _factory.Get(_context, _request.Object);
        }

        [TestMethod]
        public void it_should_return_stream()
        {
            _result.ShouldBeTheSameAs(_stream.Object);
        }

        [TestMethod]
        public void it_should_set_write_timeout()
        {
            _result.WriteTimeout.ShouldEqual(3333);
        }
    }

    [TestClass]
    public class when_getting_stream_for_asynchronous_request
    {
        static RequestStreamFactory _factory;
        static AsyncBindingContext _context;
        static Mock<WebRequest> _request;
        static Mock<Stream> _stream;
        static Stream _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _context = new AsyncBindingContext(new object(), new object(), new ClientEndpoint { Timeout = 1111, ReadTimeout = 2222, WriteTimeout = 3333 },
                new Uri("http://foo.bar/"), typeof(string), typeof(string), CancellationToken.None);

            _stream = new Mock<Stream>();
            _stream.SetupAllProperties();
            _stream.SetupGet(x => x.CanTimeout).Returns(true);

            _request = new Mock<WebRequest>();
            _request.SetupAllProperties();
            _request.Setup(x => x.GetRequestStreamAsync()).Returns(Task.FromResult(_stream.Object));

            _factory = new RequestStreamFactory();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _factory.GetAsync(_context, _request.Object).Result;
        }

        [TestMethod]
        public void it_should_return_stream()
        {
            _result.ShouldBeTheSameAs(_stream.Object);
        }

        [TestMethod]
        public void it_should_set_write_timeout()
        {
            _result.WriteTimeout.ShouldEqual(3333);
        }
    }
}
