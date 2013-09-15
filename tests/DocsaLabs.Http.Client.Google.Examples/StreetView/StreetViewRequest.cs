using DocsaLabs.Http.Client.Google.Examples.Utils;

namespace DocsaLabs.Http.Client.Google.Examples.StreetView
{
    public class StreetViewRequest
    {
        // ReSharper disable InconsistentNaming
        public GeoLocation location { get; private set; }
        public ImageSize size { get; set; }
        public int? heading { get; set; }
        public int fov { get; set; }
        public int pitch { get; set; }
        public bool sensor { get; set; }

        public StreetViewRequest(GeoLocation locationArg)
        {
            location = locationArg;
            fov = 90;
            sensor = false;
            size = new ImageSize(640, 300);
        }
        // ReSharper restore InconsistentNaming
    }
}
