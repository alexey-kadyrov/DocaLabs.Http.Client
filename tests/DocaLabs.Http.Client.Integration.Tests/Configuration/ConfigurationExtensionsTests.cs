using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Integration.Tests.Configuration
{
    /// <summary>
    /// The test installs and then remove the test certificate to Trusted People, verify that the certificate was actually removed after the test
    /// </summary>
    [Subject(typeof(ClientCertificateReferenceElement))]
    class when_finding_certitificate
    {
        static ClientCertificateReferenceElement element;
        static X509Certificate2 test_certificate;

        Cleanup after_each =
            () => Delete();

        Establish context = () =>
        {
            Create();
            element = new ClientCertificateReferenceElement();
        };

        Because of = () =>
        {
            element.StoreName = StoreName.TrustedPeople;
            element.StoreLocation = StoreLocation.CurrentUser;
            element.X509FindType = X509FindType.FindByThumbprint;
            element.FindValue = test_certificate.Thumbprint;
        };

        It should_find_certificate =
            () => element.Find().Count.ShouldEqual(1);

        static void Create()
        {
            var certStore = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);

            try
            {
                certStore.Open(OpenFlags.ReadWrite | OpenFlags.OpenExistingOnly);

                test_certificate = new X509Certificate2(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test-certificate.cer"));

                certStore.Add(test_certificate);
            }
            finally
            {
                certStore.Close();
            }
        }

        static void Delete()
        {
            var certStore = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);

            try
            {
                certStore.Open(OpenFlags.ReadWrite | OpenFlags.OpenExistingOnly);

                certStore.Remove(test_certificate);
            }
            finally
            {
                certStore.Close();
            }
        }
    }
}
