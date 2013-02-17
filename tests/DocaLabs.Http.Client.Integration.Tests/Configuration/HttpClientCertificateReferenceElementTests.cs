using System.Security.Cryptography.X509Certificates;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Integration.Tests.Configuration
{
    [Subject(typeof(HttpClientCertificateReferenceElement))]
    class when_finding_certitificate
    {
        static HttpClientCertificateReferenceElement element;
        static X509Certificate2 test_certificate;

        Cleanup after_each =
            () => Delete();

        Establish context = () =>
        {
            Create();
            element = new HttpClientCertificateReferenceElement();
        };

        Because of = () =>
        {
            element.StoreName = StoreName.My;
            element.StoreLocation = StoreLocation.CurrentUser;
            element.X509FindType = X509FindType.FindByThumbprint;
            element.FindValue = "176480aed0a9f3c790d75b952bc9fddd18a70371";
        };

        It should_find_certificate =
            () => element.Find().ShouldNotBeNull();

        static void Create()
        {
            var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            try
            {
                certStore.Open(OpenFlags.ReadWrite | OpenFlags.OpenExistingOnly);

                test_certificate = new X509Certificate2("test-certificate.cer");

                certStore.Add(test_certificate);
            }
            finally
            {
                certStore.Close();
            }
        }

        static void Delete()
        {
            var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);

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
