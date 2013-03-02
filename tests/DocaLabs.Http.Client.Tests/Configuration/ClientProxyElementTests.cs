using System;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [Subject(typeof(ClientProxyElement))]
    class when_changing_value_on_http_client_proxy_which_is_directly_newed
    {
        static ClientProxyElement element;
        static Uri address;

        Establish context = () =>
        {
            address = new Uri("http://foo.bar/");
            element = new ClientProxyElement();
        };

        Because of =
            () => element.Address = address;

        It should_change_address =
            () => element.Address.ShouldBeTheSameAs(address);
    }
}
