using System;
using System.Threading;
using DocaLabs.Test.Services._ServiceStackServices;
using DocaLabs.Test.Services._WcfServices;

namespace DocaLabs.Test.Services
{
    class Program
    {
        static void Main(params string [] args)
        {
            using (ServerCertificateInstaller.Install())
            using (new ServiceStackServerHost())
            using (new WcfServerHost<TestService>())
            using (new WcfServerHost<TestServiceWithBasicCredentials>())
            using (new WcfServerHost<TestServiceWithCertificate>())
            {
                Console.WriteLine();
                Console.WriteLine(" -- press any key to quit.");
                Console.WriteLine();

                WaitQuitEvent(args.Length == 1 && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : "DocaLabs.Test.Services.QuitEvent");
            }
        }

        static void WaitQuitEvent(string quitEventName)
        {
            EventWaitHandle quitEvent;
            if (!EventWaitHandle.TryOpenExisting(quitEventName, out quitEvent))
            {
                Console.ReadKey();
                return;
            }

            using (quitEvent)
            {
                while (true)
                {
                    if (quitEvent.WaitOne(TimeSpan.FromMilliseconds(200)))
                        return;

                    if (Console.KeyAvailable)
                        return;
                }
            }
        }
    }
}
