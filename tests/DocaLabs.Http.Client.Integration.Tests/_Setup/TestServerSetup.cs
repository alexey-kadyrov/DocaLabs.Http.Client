using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Integration.Tests._Setup
{
    static class TestServerSetup
    {
        static ManualResetEvent ServerReady { get; set; }
        static ManualResetEvent KeepingServerAlive { get; set; }

        public static void Setup()
        {
            ServerReady = new ManualResetEvent(false);
            KeepingServerAlive = new ManualResetEvent(false);

            AppDomain.CurrentDomain.DomainUnload += HandleDomainUnload;

            Console.WriteLine(@"Starting {0}.", typeof(TestService));

            Task.Factory.StartNew(Listener);

            ServerReady.WaitOne();

            Console.WriteLine(@"{0} started.", typeof(TestService));
        }

        static void HandleDomainUnload(object sender, EventArgs e)
        {
            KeepingServerAlive.Set();

            AppDomain.CurrentDomain.DomainUnload -= HandleDomainUnload;
        }

        static void Listener()
        {
            try
            {
                using (var host = new ServiceHost(typeof(TestService)/*, new Uri("http://localhost:5701/TestService")*/))
                {
                    ////Add a service endpoint
                    //host.AddServiceEndpoint(typeof(ITestService), new WSHttpBinding(), "");

                    ////Enable metadata exchange
                    //host.Description.Behaviors.Add(new ServiceMetadataBehavior
                    //{
                    //    HttpGetEnabled = true
                    //});

                    host.Open();

                    ServerReady.Set();
                    KeepingServerAlive.WaitOne();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                ServerReady.Set();
            }
        }
    }
}
