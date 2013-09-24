using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Integration.Tests._WcfServices
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

            Task.Factory.StartNew(Listener);

            ServerReady.WaitOne();

            Console.WriteLine(@"{0} started.", typeof(TService));
        }

        public void Dispose()
        {
            StopServer.Set();
            ServerStopped.WaitOne(TimeSpan.FromSeconds(1));
        }

        void Listener()
        {
            ServiceHost host = null;

            try
            {
                ServerStopped.Reset();

                host = new ServiceHost(typeof(TService));

                host.Open();

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
                if(host != null)
                    ((IDisposable)host).Dispose();
            }

            ServerStopped.Set();
        }
    }
}
