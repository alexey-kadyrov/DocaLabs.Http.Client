DocaLabs.Http.Client
====================

Strong typed HTTP client library. The main goal of the library is to minimize plumbing code to bare minimum.
------------------------------------------------------------------------------------------------------------

In order to use the library in most cases you would need to define:
* An interface for the remote service.
* A class for request data, properties of that class can be mapped into the HTTP query or to the request body.
* A class for response data, in some cases you won't need to define even that, for example if you want to get a string back.

That's it. The implemnatation is unit test friedly becuase the only thing you are working is against an interface.


For example for Google's street view you would need to define someting like:

    public interface IStreetViewService
    {
        Image Fetch(StreetViewRequest request);
    }

    public class StreetViewRequest
    {
        public GeoLocation location { get; private set; }
        public ImageSize size { get; set; }
        public int? heading { get; set; }
        public int fov { get; set; }
        public int pitch { get; set; }
        public bool sensor { get; set; }
    }
	
    public class ImageSize
    {
        public int width { get; set; }
        public int height { get; set; }

        public override string ToString()
        {
            return string.Format("{0}x{1}", width, height);
        }
    }
	
	Then inject using the factory (instead of supplying the URL here you can use the app.config file):
	
	HttpClientFactory.CreateInstance<IStreetViewService>(new Uri("http://maps.googleapis.com/maps/api/streetview"));
	
	and finally the call:
	
	var image = service.Fetch(new StreetViewRequest { location = GeoLocation(53.34462, -6.25958), size = new ImageSize { width = 640, height = 300 }, heading = 152, fov = 90 });