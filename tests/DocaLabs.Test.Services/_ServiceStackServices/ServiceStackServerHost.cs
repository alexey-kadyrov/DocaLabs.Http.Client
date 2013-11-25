using System;
using System.Threading;
using System.Threading.Tasks;
using Funq;
using ServiceStack.WebHost.Endpoints;

namespace DocaLabs.Test.Services._ServiceStackServices
{
    class ServiceStackServerHost : IDisposable
    {
        ManualResetEvent ServerReady { get; set; }
        ManualResetEvent StopServer { get; set; }
        ManualResetEvent ServerStopped { get; set; }

        public ServiceStackServerHost()
        {
            ServerReady = new ManualResetEvent(false);
            StopServer = new ManualResetEvent(false);
            ServerStopped = new ManualResetEvent(true);

            Console.WriteLine(@"Starting ServiceStackServerHost.");

            Task.Factory.StartNew(Worker);

            ServerReady.WaitOne();

            Console.WriteLine(@"ServiceStackServerHost started.");
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
            public AppHost() 
                : base("StarterTemplate HttpListener", typeof (AppHost).Assembly)
            {
                SetConfig(new EndpointHostConfig { DebugMode = true });
            }

            public override void Configure(Container container)
            {
                Routes
                    .Add<GetUser>("/v1/users/{id}", "GET")
                    .Add<GetUser>("/v2/users/{id}", "GET")
                    .Add<UpdateUserRequest>("/v2/users", "PUT")
                    .Add<AddUserRequest>("/v2/users", "POST")
                    .Add<AddUserAndReturnDataRequest>("/v2/users-and-return-data", "POST")
                    .Add<DeleteUserRequest>("/v2/users/{id}", "DELETE");
            }
        }
    }
}
