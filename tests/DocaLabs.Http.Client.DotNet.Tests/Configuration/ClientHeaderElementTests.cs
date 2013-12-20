using DocaLabs.Http.Client.Configuration;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [TestClass]
    class when_changing_value_on_client_header_element_which_is_directly_newed
    {
        static ClientHeaderElement _element;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _element = new ClientHeaderElement { Name = "name-x1" };

            BecauseOf();
        }

        static void BecauseOf()
        {
            _element.Value = "h42";
        }

        [TestMethod]
        public void it_should_set_name()
        {
            _element.Name.ShouldEqual("name-x1");
        }

        [TestMethod]
        public void it_should_change_value()
        {
            _element.Value.ShouldEqual("h42");
        }
    }
}
