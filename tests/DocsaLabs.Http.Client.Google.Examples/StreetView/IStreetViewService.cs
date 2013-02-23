using System.Drawing;

namespace DocsaLabs.Http.Client.Google.Examples.StreetView
{
    public interface IStreetViewService
    {
        Image Fetch(StreetViewRequest request);
    }
}
