using System;
using System.Diagnostics;
using System.Threading;

namespace DocaLabs.Test.Services.Proxy
{
    public class TestServicesProxy : ITestServicesProxy
    {
        const string QuitEventWaitHandleName = "DocaLabs.Test.Services.QuitEvent";

        static EventWaitHandle _quitEvent;
        static Process _serviceHost;
        static readonly object Locker = new object();

        public void Start()
        {
            lock (Locker)
            {
                if (_quitEvent != null)
                    return;

                Console.WriteLine("Starting Test Services");

                _quitEvent = new EventWaitHandle(false, EventResetMode.ManualReset, QuitEventWaitHandleName);

                _serviceHost = new Process
                {
                    StartInfo =
                    {
                        FileName = @"DocaLabs.Test.Services.exe",
                        Arguments = QuitEventWaitHandleName,
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    }
                };

                _serviceHost.Start();
            }
        }

        public void Stop()
        {
            lock (Locker)
            {
                if (_quitEvent == null)
                    return;

                Console.WriteLine("Stopping Test Services");

                _quitEvent.Set();

                _serviceHost.WaitForExit(5000);

                _quitEvent.Dispose();
                _quitEvent = null;
            }
        }
    }
}
