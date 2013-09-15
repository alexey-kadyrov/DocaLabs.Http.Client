using System.IO;
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
        static long received_data_length;

        Establish context =
            () => service = HttpClientFactory.CreateInstance<IStreetViewService>();

        Because of = () =>
        {
            using (var image = service.Fetch(new StreetViewRequest(new GeoLocation(53.34462, -6.25958)) { heading = 152 }))
            {
                if (image != null)
                {
                    using (var file = File.OpenWrite("Dublin.jpg"))
                    {
                        image.CopyTo(file);
                        file.Flush();
                        received_data_length = file.Length;
                    }
                }
            }
        };

        It should_fetch_the_street_view_image =
            () => received_data_length.ShouldBeGreaterThan(0);
    }
}
