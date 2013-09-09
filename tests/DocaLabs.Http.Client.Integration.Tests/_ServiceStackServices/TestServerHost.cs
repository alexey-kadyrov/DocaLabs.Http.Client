using System;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.WebHost.Endpoints;

namespace DocaLabs.Http.Client.Integration.Tests._ServiceStackServices
{
    class TestServerHost<TService> : IDisposable
    {
        ManualResetEvent ServerReady { get; set; }
        ManualResetEvent StopServer { get; set; }
        ManualResetEvent ServerStopped { get; set; }

        public TestServerHost()
        {
            ServerReady = new ManualResetEvent(false);
            StopServer = new ManualResetEvent(false);
            ServerStopped = new ManualResetEvent(true);

            Console.WriteLine(@"Starting {0}.", typeof(TService));

            Task.Factory.StartNew(Worker);

            ServerReady.WaitOne();

            Console.WriteLine(@"{0} started.", typeof(TService));
        }

        public void Dispose()
        {
            StopServer.Set();
            ServerStopped.WaitOne(TimeSpan.FromSeconds(1));
        }

        void Worker()
        {
            var host = new AppHost();

            try
            {
                ServerStopped.Reset();

                host.Init();
                host.Start("http://*:1337/");

                ServerReady.Set();

                StopServer.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ServerReady.Set();
            }
            finally
            {
                ((IDisposable)host).Dispose();
            }

            ServerStopped.Set();
        }

        public class AppHost : AppHostHttpListenerBase
        {
            public AppHost() : base("StarterTemplate HttpListener", typeof(TService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                Routes
                    .Add<Hello>("/" + typeof(TService).Name.ToLower())
                    .Add<Hello>("/" + typeof(TService).Name.ToLower() + "/{Name}");
            }
        }
    }
}
