using System;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [TestClass]
    public class when_changing_values_on_http_client_proxy_which_is_directly_newed
    {
        static ClientProxyElement _element;
        static Uri _address;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _address = new Uri("http://foo.bar/");
            _element = new ClientProxyElement();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _element.Address = _address;
        }

        [TestMethod]
        public void it_should_change_address()
        {
            _element.Address.ShouldBeTheSameAs(_address);
        }
    }

    [TestClass]
    public class when_directly_instantiating_http_client_proxy
    {
        static ClientProxyElement _element;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            BecauseOf();
        }

        static void BecauseOf()
        {
            _element = new ClientProxyElement();
        }

        [TestMethod]
        public void it_should_have_null_address()
        {
            _element.Address.ShouldBeNull();
        }

        [TestMethod]
        public void it_should_have_network_credentials_object_with_empty_values()
        {
            _element.Credential.ShouldMatch(x => x.Domain == "" && x.User == "" && x.Password == "");
        }
    }
}
