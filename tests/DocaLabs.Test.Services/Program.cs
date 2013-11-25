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
            if(args.Length != 1 || string.IsNullOrWhiteSpace(args[0]))
                throw new ArgumentNullException("args", "There must be one argument with the non blank name of the quit event wait handle");

            using (ServerCertificateInstaller.Install())
            using (new ServiceStackServerHost())
            using (new WcfServerHost<TestService>())
            using (new WcfServerHost<TestServiceWithBasicCredentials>())
            using (new WcfServerHost<TestServiceWithCertificate>())
            {
                WaitQuitEvent(args[0]);
            }
        }

        static void WaitQuitEvent(string quitEventName)
        {
            using (var quitEvent = EventWaitHandle.OpenExisting(quitEventName))
            {
                quitEvent.WaitOne(TimeSpan.FromMinutes(10));
            }
        }
    }
}
