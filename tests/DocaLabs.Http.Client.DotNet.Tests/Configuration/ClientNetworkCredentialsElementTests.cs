using DocaLabs.Http.Client.Configuration;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [TestClass]
    class when_netwrok_credentials_are_newed
    {
        static ClientNetworkCredentialElement _element;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _element = new ClientNetworkCredentialElement();
        }

        [TestMethod]
        public void it_should_have_credentials_type_set_to_none()
        {
            _element.CredentialType.ShouldEqual(CredentialType.None);
        }

        [TestMethod]
        public void it_should_have_user_set_to_empty_string()
        {
            _element.User.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_have_password_set_to_empty_string()
        {
            _element.Password.ShouldBeEmpty();
        }

        [TestMethod]
        public void it_should_have_domain_to_empty_string()
        {
            _element.Domain.ShouldBeEmpty();
        }
    }

    [TestClass]
    class when_changing_value_on_netwrok_credentials_which_is_directly_newed
    {
        static ClientNetworkCredentialElement _element;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _element = new ClientNetworkCredentialElement();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _element.CredentialType = CredentialType.DefaultNetworkCredentials;
            _element.User = "user1";
            _element.Password = "password1";
            _element.Domain = "domain1";
        }

        [TestMethod]
        public void it_should_change_credentials_type()
        {
            _element.CredentialType.ShouldEqual(CredentialType.DefaultNetworkCredentials);
        }

        [TestMethod]
        public void it_should_change_user()
        {
            _element.User.ShouldBeTheSameAs("user1");
        }

        [TestMethod]
        public void it_should_change_password()
        {
            _element.Password.ShouldEqual("password1");
        }

        [TestMethod]
        public void it_should_change_domain()
        {
            _element.Domain.ShouldEqual("domain1");
        }
    }
}
