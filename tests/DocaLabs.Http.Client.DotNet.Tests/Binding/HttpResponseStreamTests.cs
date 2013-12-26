using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using DocaLabs.Http.Client.Tests._Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DocaLabs.Http.Client.Tests.Binding
{
    [TestClass]
    public class when_using_stream_methods_and_properties_on_http_response_stream
    {
        static Mock<Stream> _dataSource;
        static HttpResponseStreamHelper _helper;
        static byte[] _buffer;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _buffer = new byte[0];
            _dataSource = new Mock<Stream>();
            _helper = HttpResponseStreamHelper.Setup("text/plain; charset=utf-8", _dataSource.Object);

            BecauseOf();
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        static void BecauseOf()
        {
            // ReSharper disable UnusedVariable
#pragma warning disable 168

            var canRead = _helper.ResponseStream.CanRead;
            var canSeek = _helper.ResponseStream.CanSeek;
            var canWrite = _helper.ResponseStream.CanWrite;
            var canTimeout = _helper.ResponseStream.CanTimeout;
            var length = _helper.ResponseStream.Length;

            var position = _helper.ResponseStream.Position;
            _helper.ResponseStream.Position = 111;

            var readTimeout = _helper.ResponseStream.ReadTimeout;
            _helper.ResponseStream.ReadTimeout = 222;

            var writeTimeout = _helper.ResponseStream.WriteTimeout;
            _helper.ResponseStream.WriteTimeout = 333;

            _helper.ResponseStream.Flush();
            _helper.ResponseStream.WriteByte(1);
            _helper.ResponseStream.ReadByte();
            _helper.ResponseStream.Seek(444, SeekOrigin.Current);
            _helper.ResponseStream.SetLength(555);
            _helper.ResponseStream.Read(_buffer, 666, 777);
            _helper.ResponseStream.Write(_buffer, 888, 999);

#pragma warning restore 168
            // ReSharper restore UnusedVariable
        }

        [TestMethod]
        public void it_should_call_can_read_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.CanRead, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_can_seek_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.CanSeek, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_can_write_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.CanWrite, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_can_timeout_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.CanTimeout, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_length_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.Length, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_get_position_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.Position, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_set_position_on_the_undelying_stream()
        {
            _dataSource.VerifySet(x => x.Position = 111, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_get_read_timeout_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.ReadTimeout, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_set_read_timeout_on_the_undelying_stream()
        {
            _dataSource.VerifySet(x => x.ReadTimeout = 222, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_get_write_timeout_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.WriteTimeout, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_set_write_timeout_on_the_undelying_stream()
        {
            _dataSource.VerifySet(x => x.WriteTimeout = 333, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_flush_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.Flush(), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_read_byte_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.ReadByte(), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_write_byte_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.WriteByte(1), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_seek_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.Seek(444, SeekOrigin.Current), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_set_length_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.SetLength(555), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_read_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.Read(_buffer, 666, 777), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_write_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.Write(_buffer, 888, 999), Times.AtLeastOnce());
        }
    }

    [TestClass]
    public class when_using_stream_methods_and_properties_on_asynchronous_http_response_stream
    {
        static Mock<Stream> _dataSource;
        static Mock<IAsyncResult> _asyncResult;
        static MemoryStream _destinationStream;
        static HttpResponseStreamHelper _helper;
        static byte[] _buffer;

        [ClassCleanup]
        public static void Cleanup()
        {
            _helper.Dispose();
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _buffer = new byte[0];
            _dataSource = new Mock<Stream>();
            _asyncResult = new Mock<IAsyncResult>();
            _destinationStream = new MemoryStream();
            _helper = HttpResponseStreamHelper.SetupAsync("text/plain; charset=utf-8", _dataSource.Object);

            BecauseOf();
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        static void BecauseOf()
        {
            // ReSharper disable UnusedVariable
#pragma warning disable 168

            var canRead = _helper.ResponseStream.CanRead;
            var canSeek = _helper.ResponseStream.CanSeek;
            var canWrite = _helper.ResponseStream.CanWrite;
            var canTimeout = _helper.ResponseStream.CanTimeout;
            var length = _helper.ResponseStream.Length;

            var position = _helper.ResponseStream.Position;
            _helper.ResponseStream.Position = 111;

            var readTimeout = _helper.ResponseStream.ReadTimeout;
            _helper.ResponseStream.ReadTimeout = 222;

            var writeTimeout = _helper.ResponseStream.WriteTimeout;
            _helper.ResponseStream.WriteTimeout = 333;

            _helper.ResponseStream.Flush();
            _helper.ResponseStream.WriteByte(1);
            _helper.ResponseStream.ReadByte();
            _helper.ResponseStream.Write(_buffer, 888, 999);
            _helper.ResponseStream.Seek(444, SeekOrigin.Current);
            _helper.ResponseStream.SetLength(555);
            _helper.ResponseStream.Read(_buffer, 666, 777);
            _helper.ResponseStream.Write(_buffer, 888, 999);

            _helper.ResponseStream.FlushAsync(CancellationToken.None);

            _helper.ResponseStream.ReadAsync(_buffer, 666, 777, CancellationToken.None);
            _helper.ResponseStream.BeginRead(_buffer, 666, 777, null, null);
            _helper.ResponseStream.EndRead(_asyncResult.Object);

            _helper.ResponseStream.WriteAsync(_buffer, 888, 999, CancellationToken.None);
            _helper.ResponseStream.BeginWrite(_buffer, 666, 777, null, null);
            _helper.ResponseStream.EndWrite(_asyncResult.Object);

            _helper.ResponseStream.CopyToAsync(_destinationStream, 4096, CancellationToken.None);

#pragma warning restore 168
            // ReSharper restore UnusedVariable
        }

        [TestMethod]
        public void it_should_call_can_read_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.CanRead, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_can_seek_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.CanSeek, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_can_write_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.CanWrite, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_can_timeout_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.CanTimeout, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_length_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.Length, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_get_position_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.Position, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_set_position_on_the_undelying_stream()
        {
            _dataSource.VerifySet(x => x.Position = 111, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_get_read_timeout_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.ReadTimeout, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_set_read_timeout_on_the_undelying_stream()
        {
            _dataSource.VerifySet(x => x.ReadTimeout = 222, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_get_write_timeout_on_the_undelying_stream()
        {
            _dataSource.VerifyGet(x => x.WriteTimeout, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_set_write_timeout_on_the_undelying_stream()
        {
            _dataSource.VerifySet(x => x.WriteTimeout = 333, Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_flush_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.Flush(), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_read_byte_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.ReadByte(), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_write_byte_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.WriteByte(1), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_seek_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.Seek(444, SeekOrigin.Current), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_set_length_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.SetLength(555), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_read_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.Read(_buffer, 666, 777), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_write_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.Write(_buffer, 888, 999), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_asynchronously_flush_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.FlushAsync(CancellationToken.None), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_read_async_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.ReadAsync(_buffer, 666, 777, CancellationToken.None), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_begin_read_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.BeginRead(_buffer, 666, 777, null, null), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_end_read_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.EndRead(_asyncResult.Object), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_write_async_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.WriteAsync(_buffer, 888, 999, CancellationToken.None), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_begin_write_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.BeginWrite(_buffer, 666, 777, null, null), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_end_write_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.EndWrite(_asyncResult.Object), Times.AtLeastOnce());
        }

        [TestMethod]
        public void it_should_call_copy_async_on_the_undelying_stream()
        {
            _dataSource.Verify(x => x.CopyToAsync(_destinationStream, 4096, CancellationToken.None), Times.AtLeastOnce());
        }
    }
}