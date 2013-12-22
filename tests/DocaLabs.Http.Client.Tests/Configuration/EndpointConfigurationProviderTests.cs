using DocaLabs.Http.Client.Configuration;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DocaLabs.Http.Client.Tests.Configuration
{
    [TestClass]
    public class when_checking_current_in_default_configuration
    {
        [TestMethod]
        public void it_should_return_default_endpoint_configuration_provider()
        {
            EndpointConfigurationOverride.Current.ShouldBeOfType<EndpointConfigurationProviderOverride>();
        }
    }

    [TestClass]
    public class when_setting_current_provider
    {
        static IEndpointConfigurationProvider original_provider;
        static IEndpointConfigurationProvider new_provider;
        static Exception exception;

        Cleanup after_each =
            () => EndpointConfiguration.Current = original_provider;

        Establish context = () =>
        {
            original_provider = EndpointConfiguration.Current;
            new_provider = new Mock<IEndpointConfigurationProvider>().Object;
        };

        Because of =
            () => EndpointConfiguration.Current = new_provider;

        It should_change_default_endpoint_configuration_provider =
            () => EndpointConfiguration.Current.ShouldBeTheSameAs(new_provider);
    }

    [TestClass]
    public class when_setting_current_provider_to_null
    {
        static IEndpointConfigurationProvider original_provider;
        static Exception exception;

        Establish context =
            () => original_provider = EndpointConfiguration.Current;

        Because of =
            () => exception = Catch.Exception(() => { EndpointConfiguration.Current = null; });

        It should_not_change_default_endpoint_configuration_provider =
            () => EndpointConfiguration.Current.ShouldBeTheSameAs(original_provider);

        It should_throw_argument_null_exception =
            () => exception.ShouldBeOfType<ArgumentNullException>();

        It should_report_value_argument =
            () => ((ArgumentNullException) exception).ParamName.ShouldEqual("value");
    }
}
