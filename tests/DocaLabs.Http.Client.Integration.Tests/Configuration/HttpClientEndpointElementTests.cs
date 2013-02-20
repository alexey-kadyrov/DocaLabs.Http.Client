﻿using System.Net;
using System.Security.Cryptography.X509Certificates;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Integration.Tests.Configuration
{
    /// <summary>
    /// The test installs and then remove the test certificate to Trusted People, verify that the certificate was actually removed after the test
    /// </summary>
    [Subject(typeof(HttpClientEndpointElement))]
    class when_copying_client_certificates_from_http_client_endpoint
    {
        static Mock<HttpWebRequest> web_request;
        static HttpClientEndpointElement element;
        static X509Certificate2 test_certificate;

        Cleanup after_each =
            () => Delete();

        Establish context = () =>
        {
            Create();

            web_request = new Mock<HttpWebRequest> { CallBase = true };

            element = new HttpClientEndpointElement();
            element.ClientCertificates.Add(new HttpClientCertificateReferenceElement
            {
                StoreName = StoreName.TrustedPeople,
                StoreLocation = StoreLocation.CurrentUser,
                X509FindType = X509FindType.FindByThumbprint,
                FindValue = test_certificate.Thumbprint
            });
        };

        Because of =
            () => element.CopyClientCertificatesTo(web_request.Object);

        It should_copy_the_certificate =
            () => web_request.Object.ClientCertificates.Count.ShouldEqual(1);

        static void Create()
        {
            var certStore = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);

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