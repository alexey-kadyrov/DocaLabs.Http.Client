using System.Net;
using System.Security.Cryptography.X509Certificates;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Test.Utils;
using DocaLabs.Test.Utils.DotNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Integration.Tests.DotNet.Configuration
{
    /// <summary>
    /// The test installs and then remove the test certificate to Trusted People, verify that the certificate was actually removed after the test
    /// </summary>
    [TestClass]
    public class when_copying_client_certificates_from_http_client_endpoint
    {
        static HttpWebRequest _request;
        static RequestSetupOverride _requestSetup;
        static IClientEndpoint _endpoint;
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

            _request = WebRequest.CreateHttp("http://foo.bar/");

            _requestSetup = new RequestSetupOverride();

            _endpoint = EndpointConfigurationFactory.Current.GetEndpoint("certificatesForRequestSetup");

            BecauseOf();
        }

        static void BecauseOf()
        {
            _requestSetup.CopyClientCertificatesFrom(_request, _endpoint);
        }

        [TestMethod]
        public void it_should_copy_the_certificate()
        {
            _request.ClientCertificates.Count.ShouldEqual(1);
        }
    }
}
