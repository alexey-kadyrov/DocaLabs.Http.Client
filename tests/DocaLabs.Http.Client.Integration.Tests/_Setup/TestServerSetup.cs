using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Integration.Tests._Setup
{
    static class TestServerSetup
    {
        static ManualResetEvent ServerReady { get; set; }
        static ManualResetEvent StopServer { get; set; }
        static ManualResetEvent ServerStopped { get; set; }

        public static void Start()
        {
            ServerReady = new ManualResetEvent(false);
            StopServer = new ManualResetEvent(false);
            ServerStopped = new ManualResetEvent(true);

            AppDomain.CurrentDomain.DomainUnload += HandleDomainUnload;

            Console.WriteLine(@"Starting {0}.", typeof(TestService));

            Task.Factory.StartNew(Listener);

            ServerReady.WaitOne();

            Console.WriteLine(@"{0} started.", typeof(TestService));
        }

        public static void Stop()
        {
            StopServer.Set();
            ServerStopped.WaitOne(TimeSpan.FromSeconds(1));
        }

        static void HandleDomainUnload(object sender, EventArgs e)
        {
            Stop();

            AppDomain.CurrentDomain.DomainUnload -= HandleDomainUnload;
        }

        static void Listener()
        {
            try
            {
                ServerStopped.Reset();

                using (var host = new ServiceHost(typeof(TestService)/*, new Uri("http://localhost:5701/TestService")*/))
                {
                    host.Open();
                    ServerReady.Set();
                    StopServer.WaitOne();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                ServerReady.Set();
            }

            ServerStopped.Set();
        }
    }
}
