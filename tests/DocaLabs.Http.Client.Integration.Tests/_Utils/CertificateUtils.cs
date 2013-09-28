using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace DocaLabs.Http.Client.Integration.Tests._Utils
{
    public static class CertificateUtils
    {
        static public X509Certificate2 Install(string certFile, StoreName storeName, StoreLocation storeLocation)
        {
            var certStore = new X509Store(storeName, storeLocation);

            try
            {
                certStore.Open(OpenFlags.ReadWrite | OpenFlags.OpenExistingOnly);

                var password = new SecureString();
                password.AppendChar('1');

                var certificate = certFile.EndsWith(".pfx", StringComparison.InvariantCultureIgnoreCase)
                    ? new X509Certificate2(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, certFile), password)
                    : new X509Certificate2(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, certFile));

                certStore.Add(certificate);

                return certificate;
            }
            finally
            {
                certStore.Close();
            }
        }

        static public void Uninstall(this X509Certificate2 certificate, StoreName storeName, StoreLocation storeLocation)
        {
            var certStore = new X509Store(storeName, storeLocation);

            try
            {
                certStore.Open(OpenFlags.ReadWrite | OpenFlags.OpenExistingOnly);

                certStore.Remove(certificate);
            }
            finally
            {
                certStore.Close();
            }
        }

        static public void BindToPort(this X509Certificate2 certificate, int port)
        {
            UnbindPort(port);

            var bindPortToCertificate = new Process
            {
                StartInfo =
                {
                    FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "netsh.exe"),
                    Arguments = string.Format("http add sslcert ipport=0.0.0.0:{0} certhash={1} appid={{{2}}}", port, certificate.Thumbprint, Guid.NewGuid()),
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            bindPortToCertificate.Start();

            Console.WriteLine(bindPortToCertificate.StandardOutput.ReadToEnd());

            bindPortToCertificate.WaitForExit();
        }

        static public void UnbindPort(int port)
        {
            var bindPortToCertificate = new Process
            {
                StartInfo =
                {
                    FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "netsh.exe"),
                    Arguments = string.Format("http delete sslcert ipport=0.0.0.0:{0}", port),
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            bindPortToCertificate.Start();

            Console.WriteLine(bindPortToCertificate.StandardOutput.ReadToEnd());

            bindPortToCertificate.WaitForExit();
        }
    }
}
