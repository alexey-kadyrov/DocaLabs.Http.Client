using System;
using System.Linq;
using System.Net;
using DocaLabs.Http.Client.Binding;
using Machine.Specifications;
using Machine.Specifications.Annotations;
using Moq;
using It = Machine.Specifications.It;

namespace DocaLabs.Http.Client.Tests.Binding
{
    [Subject(typeof(DefaultCredentialsMapper))]
    class when_default_credentials_mapper_is_mapping_null_model
    {
        static DefaultCredentialsMapper mapper;
        static ICredentials mapped_credentials;

        Establish context =
            () => mapper = new DefaultCredentialsMapper();

        Because of =
            () => mapped_credentials = mapper.Map(null, new Uri("http://contoso.com/"));

        It should_return_null =
            () => mapped_credentials.ShouldBeNull();
    }

    [Subject(typeof(DefaultCredentialsMapper))]
    class when_default_credentials_mapper_is_mapping_model_with_all_credetials_property_set_to_null
    {
        static DefaultCredentialsMapper mapper;
        static Model model;
        static ICredentials mapped_credentials;

        Establish context = () =>
        {
            model = new Model
            {
                Value = "Hello World!"
            };
            mapper = new DefaultCredentialsMapper();
        };

        Because of =
            () => mapped_credentials = mapper.Map(model, new Uri("http://contoso.com/"));

        It should_return_null =
            () => mapped_credentials.ShouldBeNull();

        class Model
        {
            public string Value { [UsedImplicitly] get; set; }
            [UsedImplicitly]
            public ICredentials Credentials1 { get; set; }
            [UsedImplicitly] 
            public ICredentials Credentials2 { get; set; }
        }
    }

    [Subject(typeof(DefaultCredentialsMapper))]
    class when_default_credentials_mapper_is_mapping_model_with_only_one_non_null_credetials_property
    {
        static DefaultCredentialsMapper mapper;
        static Model model;
        static ICredentials original_credentials;
        static ICredentials mapped_credentials;

        Establish context = () =>
        {
            original_credentials = new Mock<ICredentials>().Object;
            model = new Model
            {
                Value = "Hello World!",
                Credentials2 = original_credentials
            };
            mapper = new DefaultCredentialsMapper();
        };

        Because of =
            () => mapped_credentials = mapper.Map(model, new Uri("http://contoso.com/"));

        It should_return_credetials_object_from_the_model =
            () => mapped_credentials.ShouldBeTheSameAs(original_credentials);

        class Model
        {
            public string Value { [UsedImplicitly] get; set; }
            [UsedImplicitly]
            public ICredentials Credentials1 { get; set; }
            public ICredentials Credentials2 { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(DefaultCredentialsMapper))]
    class when_default_credentials_mapper_is_mapping_model_with_one_credetials_and_another_network_credential_property
    {
        static DefaultCredentialsMapper mapper;
        static Model model;
        static ICredentials original_credentials_1;
        static ICredentials original_credentials_2;
        static Exception exception;

        Establish context = () =>
        {
            original_credentials_1 = new Mock<ICredentials>().Object;
            original_credentials_2 = new NetworkCredential("user", "password");
            model = new Model
            {
                Value = "Hello World!",
                Credentials1 = original_credentials_1,
                Credentials2 = original_credentials_2
            };
            mapper = new DefaultCredentialsMapper();
        };

        Because of =
            () => exception = Catch.Exception(() => mapper.Map(model, new Uri("http://contoso.com/")));

        It should_throw_invalid_argument_exception =
            () => exception.ShouldBeOfType<ArgumentException>();

        It should_report_credential_parameter =
            () => ((ArgumentException) exception).ParamName.ShouldEqual("credential");

        class Model
        {
            public string Value { [UsedImplicitly] get; set; }
            [UsedImplicitly]
            public ICredentials Credentials1 { get; set; }
            public ICredentials Credentials2 { [UsedImplicitly] get; set; }
        }
    }

    [Subject(typeof(DefaultCredentialsMapper))]
    class when_default_credentials_mapper_is_mapping_model_with_two_non_null_network_credetial_property
    {
        static DefaultCredentialsMapper mapper;
        static Model model;
        static Uri url;
        static ICredentials original_credentials_1;
        static ICredentials original_credentials_2;
        static ICredentials mapped_credentials;

        Establish context = () =>
        {
            url = new Uri("http://contoso.com/");
            original_credentials_1 = new NetworkCredential();
            original_credentials_2 = new NetworkCredential();
            model = new Model
            {
                Value = "Hello World!",
                Credentials1 = original_credentials_1,
                Credentials2 = original_credentials_2
            };
            mapper = new DefaultCredentialsMapper();
        };

        Because of =
            () => mapped_credentials = mapper.Map(model, url);

        It should_return_credential_cache_object =
            () => mapped_credentials.ShouldBeOfType<CredentialCache>();

        It should_return_all_original_credetials =
            () => ((CredentialCache)mapped_credentials).Cast<ICredentials>().Count().ShouldEqual(2);

        It should_return_first_credetial_object_from_the_model =
            () => ((CredentialCache)mapped_credentials).GetCredential(new Uri(url.GetLeftPart(UriPartial.Authority)), "Credentials1").ShouldBeTheSameAs(original_credentials_1);

        It should_return_second_credetial_object_from_the_model =
            () => ((CredentialCache)mapped_credentials).GetCredential(new Uri(url.GetLeftPart(UriPartial.Authority)), "Credentials2").ShouldBeTheSameAs(original_credentials_2);

        class Model
        {
            public string Value { [UsedImplicitly] get; set; }
            [UsedImplicitly]
            public ICredentials Credentials1 { [UsedImplicitly] get; set; }
            public ICredentials Credentials2 { [UsedImplicitly] get; set; }
        }
    }
}
