using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace DocaLabs.Http.Client.Integration.Tests._Utils
{
    public static class CertificateUtils
    {
        static public X509Certificate2 Install(X509Certificate2 certificate = null, StoreName storeName = StoreName.TrustedPeople, StoreLocation storeLocation = StoreLocation.CurrentUser)
        {
            var certStore = new X509Store(storeName, storeLocation);

            try
            {
                certStore.Open(OpenFlags.ReadWrite | OpenFlags.OpenExistingOnly);

                if (certificate == null)
                    certificate = new X509Certificate2(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test-certificate.cer"));

                certStore.Add(certificate);

                return certificate;
            }
            finally
            {
                certStore.Close();
            }
        }

        static public void Uninstall(X509Certificate2 certificate, StoreName storeName = StoreName.TrustedPeople, StoreLocation storeLocation = StoreLocation.CurrentUser)
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
    }
}
