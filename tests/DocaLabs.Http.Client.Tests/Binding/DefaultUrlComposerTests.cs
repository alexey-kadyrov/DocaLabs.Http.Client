using System;
using DocaLabs.Http.Client.Binding;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Binding
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local

    [Subject(typeof(DefaultUrlComposer))]
    class when_url_composer_is_used_for_model_without_serialization_hints
    {
        static Uri base_url;
        static TestModel model;
        static string url;

        Establish context = () =>
        {
            base_url = new Uri("http://foo.bar/product/{pathValue1}/red/{pathValue2}?c=en-IE");

            model = new TestModel
            {
                PathValue1 = "get this",
                PathValue2 = "another path",
                QueryValue1 = "Hello World!"
            };
        };

        Because of =
            () => url = new DefaultUrlComposer().Compose(model, base_url);

        It should_not_modify_authority_and_path_and_should_add_input_model_part =
            () => url.ShouldEqual("http://foo.bar/product/get%20this/red/another%20path?c=en-IE&QueryValue1=Hello+World!");

        class TestModel
        {
            public string PathValue1 { get; set; }
            public string QueryValue1 { get; set; }
            public string PathValue2 { get; set; }
        }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Local
}
