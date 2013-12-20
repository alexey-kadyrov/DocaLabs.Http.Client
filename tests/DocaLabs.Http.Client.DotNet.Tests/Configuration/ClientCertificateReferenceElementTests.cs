using DocaLabs.Http.Client.Configuration;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [TestClass]
    public class when_http_client_certitificate_reference_is_newed
    {
        static ClientCertificateReferenceElement _element;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _element = new ClientCertificateReferenceElement();
        }

        [TestMethod]
        public void it_should_have_store_name_set_to_my()
        {
            _element.StoreName.ShouldEqual(CertificateStoreName.My);
        }

        [TestMethod]
        public void it_should_have_store_location_set_to_local_machine()
        {
            _element.StoreLocation.ShouldEqual(CertificateStoreLocation.LocalMachine);
        }

        [TestMethod]
        public void it_should_have_find_type_set_to_subject_distingushed_name()
        {
            _element.X509FindType.ShouldEqual(CertificateX509FindType.FindBySubjectDistinguishedName);
        }

        [TestMethod]
        public void it_should_have_find_value_set_to_empty_string()
        {
            _element.FindValue.ShouldBeEmpty();
        }
    }

    [TestClass]
    public class when_changing_value_on_http_client_certitificate_reference_which_is_directly_newed
    {
        static ClientCertificateReferenceElement _element;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _element = new ClientCertificateReferenceElement();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _element.StoreName = CertificateStoreName.TrustedPeople;
            _element.StoreLocation = CertificateStoreLocation.CurrentUser;
            _element.X509FindType = CertificateX509FindType.FindByThumbprint;
            _element.FindValue = "some certificate";
        }

        [TestMethod]
        public void it_should_change_store_name()
        {
            _element.StoreName.ShouldEqual(CertificateStoreName.TrustedPeople);
        }

        [TestMethod]
        public void it_should_change_store_location()
        {
            _element.StoreLocation.ShouldEqual(CertificateStoreLocation.CurrentUser);
        }

        [TestMethod]
        public void it_should_change_find_type()
        {
            _element.X509FindType.ShouldEqual(CertificateX509FindType.FindByThumbprint);
        }

        [TestMethod]
        public void it_should_change_find_value()
        {
            _element.FindValue.ShouldEqual("some certificate");
        }
    }

    [TestClass]
    public class when_setting_find_value_on_http_client_certitificate_reference_to_null
    {
        static ClientCertificateReferenceElement _element;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _element = new ClientCertificateReferenceElement();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _element.FindValue = null;
        }

        [TestMethod]
        public void it_should_set_find_value_to_empty_string()
        {
            _element.FindValue.ShouldBeEmpty();
        }
    }
}
