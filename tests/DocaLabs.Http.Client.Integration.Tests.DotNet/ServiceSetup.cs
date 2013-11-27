using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Integration.Tests.DotNet
{
    [TestClass]
    public class ServiceSetup
    {
        const string QuitEventWaitHAndleName = "DocaLabs.Test.Services.QuitEvent";

        static EventWaitHandle _quitEvent;

        static Process _serviceHost;
        
        [AssemblyInitialize]
        public static void StartServices(TestContext context)
        {
            _quitEvent = new EventWaitHandle(false, EventResetMode.ManualReset, QuitEventWaitHAndleName);

            _serviceHost = new Process
            {
                StartInfo =
                {
                    FileName = @"ServiceHost/DocaLabs.Test.Services.exe",
                    Arguments = QuitEventWaitHAndleName,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            _serviceHost.Start();
        }

        [AssemblyCleanup]
        public static void StopServices()
        {
            _quitEvent.Set();

            _serviceHost.WaitForExit(10000);

            _quitEvent.Dispose();
        }
    }
}
