DocaLabs.Http.Client
====================

Strong typed HTTP client library. The main goal of the library is to minimize plumbing code to bare minimum.
------------------------------------------------------------------------------------------------------------

In order to use the library in most cases you would need to define:
* An interface for the remote service.
* A class for request data (query model), properties of that class can be mapped into the HTTP query or to the request body.
* A class for response data (result model), in some cases you won't need to define even that, for example if you want to get a string back.

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
	

More details
------------
By default all public properties of a query model are mapped to URL's query part using a property name as a parameter name. Properties can be ignore by marling them with QueryIgnoreAttribute and the parameter name can be changed by using QueryParameterAttribute.
The behaviour can be altered by using attributes. If you want your entire query model to be serialised into request body you can apply any descendants of RequestSerializationAttribute to the model type or the service interface. However if you are not using HttpClientFactory to instantiate but isntead prefer to use HttpClient<,> directly you will need to create subclass an apply the attribute on that subclass (it's only in case if you do not want to drop[ the attribut in your query model type).
You can also mix by mapping some properties to a URL's query part and one of the properties to a request's body, you wiil need mark the property with QueryIgnoreAttribute and any descendants of RequestSerializationAttribute which you want to serialisr to request body.
The request can be compressed using deflate or gzip, for details see descendants of RequestSerializationAttribute.

The response is processed smartly - it checks whenever the conttent MIME type is 'applicatiion/json', 'application/xml', 'text/xml' and tryes to use appropriate deserialization. However you can still supply your own deserialization using subclasses of ResponseDeserializationAttribute defined either on the result model type or the service interface.