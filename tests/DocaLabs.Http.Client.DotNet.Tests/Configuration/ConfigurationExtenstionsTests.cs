using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [TestClass]
    public class when_finding_certificate_for_null_reference
    {
        static Exception _exception;

        [ClassInitialize]
        public static void EsatblishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => ((IClientCertificateReference) null).Find());
        }

        [TestMethod]
        public void it_should_throw_argument_null_exception()
        {
            _exception.ShouldBeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void it_should_report_reference_argument()
        {
            ((ArgumentNullException) _exception).ParamName.ShouldEqual("reference");
        }
    }

    [TestClass]
    public class when_converting_certificate_store_location_enum_values
    {
        [TestMethod]
        public void it_should_convert_known_values()
        {
            CertificateStoreLocation.CurrentUser.ToStoreLocation().ShouldEqual(StoreLocation.CurrentUser);
            CertificateStoreLocation.LocalMachine.ToStoreLocation().ShouldEqual(StoreLocation.LocalMachine);
        }

        [TestMethod]
        public void it_should_throw_ivlaid_enum_exception_for_unknown_values()
        {
            Catch.Exception(() => ((CertificateStoreLocation)99999).ToStoreLocation()).ShouldBeOfType<InvalidEnumArgumentException>();
        }
    }

    [TestClass]
    public class when_converting_certificate_store_name_enum_values
    {
        [TestMethod]
        public void it_should_convert_known_values()
        {
            CertificateStoreName.AddressBook.ToStoreName().ShouldEqual(StoreName.AddressBook);
            CertificateStoreName.AuthRoot.ToStoreName().ShouldEqual(StoreName.AuthRoot);
            CertificateStoreName.CertificateAuthority.ToStoreName().ShouldEqual(StoreName.CertificateAuthority);
            CertificateStoreName.Disallowed.ToStoreName().ShouldEqual(StoreName.Disallowed);
            CertificateStoreName.My.ToStoreName().ShouldEqual(StoreName.My);
            CertificateStoreName.Root.ToStoreName().ShouldEqual(StoreName.Root);
            CertificateStoreName.TrustedPeople.ToStoreName().ShouldEqual(StoreName.TrustedPeople);
            CertificateStoreName.TrustedPublisher.ToStoreName().ShouldEqual(StoreName.TrustedPublisher);
        }

        [TestMethod]
        public void it_should_throw_ivlaid_enum_exception_for_unknown_values()
        {
            Catch.Exception(() => ((CertificateStoreName)99999).ToStoreName()).ShouldBeOfType<InvalidEnumArgumentException>();
        }
    }

    [TestClass]
    public class when_converting_certificate_x509_find_type_enum_values
    {
        [TestMethod]
        public void it_should_convert_known_values()
        {
            CertificateX509FindType.FindByApplicationPolicy.ToX509FindType().ShouldEqual(X509FindType.FindByApplicationPolicy);
            CertificateX509FindType.FindByCertificatePolicy.ToX509FindType().ShouldEqual(X509FindType.FindByCertificatePolicy);
            CertificateX509FindType.FindByExtension.ToX509FindType().ShouldEqual(X509FindType.FindByExtension);
            CertificateX509FindType.FindByIssuerDistinguishedName.ToX509FindType().ShouldEqual(X509FindType.FindByIssuerDistinguishedName);
            CertificateX509FindType.FindByIssuerName.ToX509FindType().ShouldEqual(X509FindType.FindByIssuerName);
            CertificateX509FindType.FindByKeyUsage.ToX509FindType().ShouldEqual(X509FindType.FindByKeyUsage);
            CertificateX509FindType.FindBySerialNumber.ToX509FindType().ShouldEqual(X509FindType.FindBySerialNumber);
            CertificateX509FindType.FindBySubjectDistinguishedName.ToX509FindType().ShouldEqual(X509FindType.FindBySubjectDistinguishedName);
            CertificateX509FindType.FindBySubjectKeyIdentifier.ToX509FindType().ShouldEqual(X509FindType.FindBySubjectKeyIdentifier);
            CertificateX509FindType.FindBySubjectName.ToX509FindType().ShouldEqual(X509FindType.FindBySubjectName);
            CertificateX509FindType.FindByTemplateName.ToX509FindType().ShouldEqual(X509FindType.FindByTemplateName);
            CertificateX509FindType.FindByThumbprint.ToX509FindType().ShouldEqual(X509FindType.FindByThumbprint);
            CertificateX509FindType.FindByTimeExpired.ToX509FindType().ShouldEqual(X509FindType.FindByTimeExpired);
            CertificateX509FindType.FindByTimeNotYetValid.ToX509FindType().ShouldEqual(X509FindType.FindByTimeNotYetValid);
            CertificateX509FindType.FindByTimeValid.ToX509FindType().ShouldEqual(X509FindType.FindByTimeValid);
        }

        [TestMethod]
        public void it_should_throw_ivlaid_enum_exception_for_unknown_values()
        {
            Catch.Exception(() => ((CertificateX509FindType)99999).ToX509FindType()).ShouldBeOfType<InvalidEnumArgumentException>();
        }
    }
}
