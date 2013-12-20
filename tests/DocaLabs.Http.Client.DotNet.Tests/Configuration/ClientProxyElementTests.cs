using System;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [TestClass]
    public class when_changing_value_on_http_client_proxy_which_is_directly_newed
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
}
