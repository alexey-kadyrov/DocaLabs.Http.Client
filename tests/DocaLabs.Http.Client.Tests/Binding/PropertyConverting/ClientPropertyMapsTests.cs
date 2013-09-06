using System;
using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding.PropertyConverting
{
    [Subject(typeof(ClientPropertyMaps))]
    class when_client_property_maps_converting_with_null_accept_property_delegate
    {
        static NameValueCollection source;
        static ClientPropertyMaps maps;
        static Exception exception;

        Establish context = () =>
        {
            source = new NameValueCollection
            {
                { "k1", "v1" },
                { "k2", "v2" }
            };
            maps = new ClientPropertyMaps();
        };

        Because of =
            () => exception = Catch.Exception(() => maps.Convert(new object(), source, null));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_accept_property_check_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("acceptPropertyCheck");
    }

    [Subject(typeof(ClientPropertyMaps))]
    class when_client_property_maps_converting_with_null_client
    {
        static NameValueCollection source;
        static ClientPropertyMaps maps;
        static Exception exception;

        Establish context = () =>
        {
            source = new NameValueCollection
            {
                { "k1", "v1" },
                { "k2", "v2" }
            };
            maps = new ClientPropertyMaps();
        };

        Because of =
            () => exception = Catch.Exception(() => maps.Convert(null, source, x => true));

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_client_argument =
            () => ((ArgumentNullException)exception).ParamName.ShouldEqual("client");
    }
}
