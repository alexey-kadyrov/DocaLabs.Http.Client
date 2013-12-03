using System.Security.Cryptography.X509Certificates;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Test.Utils;
using DocaLabs.Test.Utils.DotNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Integration.Tests.Configuration
{
    /// <summary>
    /// The test installs and then remove the test certificate to Trusted People, verify that the certificate was actually removed after the test
    /// </summary>
    [TestClass]
    public class when_finding_certitificate
    {
        static ClientCertificateReferenceElement _element;
        static X509Certificate2 _testCertificate;

        [ClassCleanup]
        public static void Cleanup()
        {
            _testCertificate.Uninstall(StoreName.TrustedPeople, StoreLocation.CurrentUser);
        }

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _testCertificate = CertificateUtils.Install("test-certificate.cer", StoreName.TrustedPeople, StoreLocation.CurrentUser);
            _element = new ClientCertificateReferenceElement();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _element.StoreName = CertificateStoreName.TrustedPeople;
            _element.StoreLocation = CertificateStoreLocation.CurrentUser;
            _element.X509FindType = CertificateX509FindType.FindByThumbprint;
            _element.FindValue = _testCertificate.Thumbprint;
        }

        [TestMethod]
        public void it_should_find_certificate()
        {
            _element.Find().Count.ShouldEqual(1);
        }
    }
}
