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
            try
            {
                using (ServerCertificateInstaller.Install())
                using (new ServiceStackServerHost())
                using (new WcfServerHost<TestService>())
                using (new WcfServerHost<TestServiceWithBasicCredentials>())
                using (new WcfServerHost<TestServiceWithCertificate>())
                {
                    Console.WriteLine();
                    Console.WriteLine(" -- press enter key to quit.");
                    Console.WriteLine();

                    WaitQuitEvent(args.Length == 1 ? args[0] : null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                if(args.Length == 0)
                    return;

                Console.WriteLine();
                Console.WriteLine(" -- press enter key to quit.");
                Console.WriteLine();

                Console.ReadLine();
            }
        }

        static void WaitQuitEvent(string quitEventName)
        {
            EventWaitHandle quitEvent;
            if (string.IsNullOrWhiteSpace(quitEventName) || !EventWaitHandle.TryOpenExisting(quitEventName, out quitEvent))
            {
                Console.ReadLine();
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
