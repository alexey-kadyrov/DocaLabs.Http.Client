using System.Drawing;
using DocaLabs.Http.Client;
using DocsaLabs.Http.Client.Google.Examples.StreetView;
using DocsaLabs.Http.Client.Google.Examples.Utils;
using Machine.Specifications;

namespace DocsaLabs.Http.Client.Google.Examples
{
    [Subject("Google Places")]
    class HttpClientExamples
    {
        static IStreetViewService service;
        static Image image;

        Establish context =
            () => service = HttpClientFactory.CreateInstance<IStreetViewService>();

        Because of = () =>
        {
            image = service.Fetch(new StreetViewRequest(new GeoLocation(53.34462, -6.25958)) { heading = 152 });

            if(image != null)
                image.Save("Dublin.jpg");
        };

        It should_fetch_the_street_view_image =
            () => image.ShouldNotBeNull();
    }
}
