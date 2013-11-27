using System;
using System.Security.Cryptography.X509Certificates;
using DocaLabs.Test.Utils.DotNet;

namespace DocaLabs.Test.Services
{
    public static class ServerCertificateInstaller
    {
        public static IDisposable Install()
        {
            var disposer = new CertificateDisposer();

            try
            {
                disposer.CaRootCertificate = CertificateUtils.Install("MyCA.cer", StoreName.Root, StoreLocation.LocalMachine);
                disposer.LocalhostCertificate = CertificateUtils.Install("localhost.pfx", StoreName.My, StoreLocation.LocalMachine);

                disposer.LocalhostCertificate.BindToPort(5705);

            }
            catch
            {
                disposer.Dispose();
                throw;
            }

            return disposer;
        }

        class CertificateDisposer : IDisposable
        {
            public X509Certificate2 CaRootCertificate { get; set; }
            public X509Certificate2 LocalhostCertificate { get; set; }

            public void Dispose()
            {
                if (LocalhostCertificate != null)
                    LocalhostCertificate.Uninstall(StoreName.My, StoreLocation.LocalMachine);

                if (CaRootCertificate != null)
                    CaRootCertificate.Uninstall(StoreName.Root, StoreLocation.LocalMachine);

                CertificateUtils.UnbindPort(5705);
            }
        }
    }
}
