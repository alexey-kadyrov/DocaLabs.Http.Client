﻿using System.Net;
using DocaLabs.Http.Client.Configuration;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Configuration
{

    [Subject(typeof(ClientNetworkCredentialElement))]
    class when_getting_credentials_for_none
    {
        static ClientNetworkCredentialElement element;
        static ICredentials credentials;

        Establish context = () => element = new ClientNetworkCredentialElement
        {
            CredentialType = CredentialType.None
        };

        Because of =
            () => credentials = element.GetCredential();

        It should_return_null =
            () => credentials.ShouldBeNull();
    }

    [Subject(typeof(ClientNetworkCredentialElement))]
    class when_getting_credentials_for_default_credentials
    {
        static ClientNetworkCredentialElement element;
        static ICredentials credentials;

        Establish context = () => element = new ClientNetworkCredentialElement
        {
            CredentialType = CredentialType.DefaultCredentials
        };

        Because of =
            () => credentials = element.GetCredential();

        It should_return_default_credentials =
            () => credentials.ShouldBeTheSameAs(CredentialCache.DefaultCredentials);
    }

    [Subject(typeof(ClientNetworkCredentialElement))]
    class when_getting_credentials_for_default_network_credentials
    {
        static ClientNetworkCredentialElement element;
        static ICredentials credentials;

        Establish context = () => element = new ClientNetworkCredentialElement
        {
            CredentialType = CredentialType.DefaultNetworkCredentials
        };

        Because of =
            () => credentials = element.GetCredential();

        It should_return_default_network_credentials =
            () => credentials.ShouldBeTheSameAs(CredentialCache.DefaultNetworkCredentials);
    }

    [Subject(typeof(ClientNetworkCredentialElement))]
    class when_getting_credentials_for_custom_credentials
    {
        static ClientNetworkCredentialElement element;
        static ICredentials credentials;

        Establish context = () => element = new ClientNetworkCredentialElement
        {
            CredentialType = CredentialType.NetworkCredential,
            User = "user1",
            Password = "password1",
            Domain = "domain1"
        };

        Because of =
            () => credentials = element.GetCredential();

        It should_return_network_credential_object =
            () => credentials.ShouldBeOfType<NetworkCredential>();

        It should_return_fully_initialized_network_credential_object =
            () => ((NetworkCredential)credentials).ShouldMatch(x => x.UserName == "user1" && x.Password == "password1" && x.Domain == "domain1");
    }
}