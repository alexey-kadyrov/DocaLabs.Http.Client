using DocsaLabs.Http.Client.Examples.Core.GeoTypes;

namespace DocsaLabs.Http.Client.Examples.Core
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
