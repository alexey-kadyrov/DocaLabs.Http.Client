using System.Net.Mime;
using DocaLabs.Http.Client.Binding.Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Utils
{
    [Subject(typeof(ContentTypeExtensions))]
    class when_checking_media_type_for_null_content_type
    {
        It should_return_false =
            () => ((ContentType) null).Is("text/plain").ShouldBeFalse();
    }

    [Subject(typeof(ContentTypeExtensions))]
    class when_checking_media_type_for_null_media_type
    {
        It should_return_false =
            () => new ContentType().Is(null).ShouldBeFalse();
    }

    [Subject(typeof(ContentTypeExtensions))]
    class when_checking_media_type_for_the_same_media_type
    {
        It should_return_true =
            () => new ContentType("text/plain; charset=utf-8").Is("TEXT/plain").ShouldBeTrue();
    }

    [Subject(typeof(ContentTypeExtensions))]
    class when_checking_media_type_for_different_media_type
    {
        It should_return_false =
            () => new ContentType("text/plain; charset=utf-8").Is("application/json").ShouldBeFalse();
    }
}
