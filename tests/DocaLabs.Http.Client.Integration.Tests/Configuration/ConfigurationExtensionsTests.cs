using System.Security.Cryptography.X509Certificates;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Http.Client.Integration.Tests._Utils;
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
            () => CertificateUtils.Uninstall(test_certificate);

        Establish context = () =>
        {
            test_certificate = CertificateUtils.Install();
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
    }
}
