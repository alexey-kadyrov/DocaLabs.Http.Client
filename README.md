DocaLabs.Http.Client
====================

_Strong typed HTTP client library. The main goal of the library is to minimize plumbing code to bare minimum._


The nuget package can be found at: https://nuget.org/packages/DocaLabs.Http.Client/


In order to use the library in most cases you would need to define:
* A class for response data (output model), in some cases you won't need to define even that, for example if you want to get a string back.
* An interface for the remote service.
* (optionally as if you are using the client factory when the method parameters will be used to extend the request's URL) A class for request data (input model), properties of that class can be mapped into the URL path, query or to the request body.


That's it. The implementation is unit test friendly because the only thing you need to worry is interface and input/output models.


For example for Google's street view you would need to define something like:

    public interface IStreetViewService
    {
        Stream Fetch(StreetViewRequest request);
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
	
    public class ImageSize : ICustomValueConverter
    {
        public int width { get; set; }
        public int height { get; set; }

        public NameValueCollection ConvertProperties()
        {
            return new NameValueCollection { { "", string.Format("{0}x{1}", Width, Height) } };
        }
    }
	
Then inject using the factory (instead of supplying the URL here you can use the app.config file):
	
	HttpClientFactory.CreateInstance<IStreetViewService>(new Uri("http://maps.googleapis.com/maps/api/streetview"));
	
and finally the call:
	
    using (var imageStream = _service.Fetch(new StreetViewRequest(new GeoLocation(53.34462, -6.25958)) { heading = 152 }))
    {
		// ...
    }
	

Or for the example user service:

	public class User
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
	}

	public interface IGetUserService
	{
		User Get(Guid id);
	}

	var user = _service.Get(userId);

The configuration for the service could be like this:

	<configSections>
		<section name="httpClientEndpoints" type="DocaLabs.Http.Client.Configuration.HttpClientEndpointSection, DocaLabs.Http.Client" />
	</configSections>

	<httpClientEndpoints>
		<endpoint name="getUserService" baseUrl="http://localhost:1337/v2/users/{id}"/>
	</httpClientEndpoints>

Then the factory call is:

	HttpClientFactory.CreateInstance<IGetUserService>("getUserService");

If you want to get additional information about the response you can wrap you output model into the RichRespose<> generic class which will contain such things as etag, status code, status description, headers.
	
	public interface IGetUserService
	{
		RichResponse<User> Get(Guid id);
	}

More details
------------
HttpClient is the class which maintains the request pipeline and utilizes WebRequest/WebResponse classes from the .Net. One notable difference in behaviour is that when you wrap your output model into the RichResponse<> and the service returns one of the 3XX instead of throwing the exception (the default behaviour of the HttpWebResponse class) it will return the rich response with the value set to null (or rather the default value of the output model).

By default all public properties of the input model are mapped to the URL's path if it's name matches the pattern {property-name} or to URL's query part otherwise using a property name as a parameter name. If the property is marked by any attribute derived from RequestSerializationAttribute (SerializeAsJsonAttribute, SerializeAsXmlAttribute, SerializeAsFormAttribute, SerializeAsTextAttribute, or your custom attribute) then the value of that property will be serialized into the request body. If the whole model or the interface are marked with such property then whole model will be serialized into the request.

There are a lot of extension points that can be used to alter the framework behaviour. The simplest and most likely to be used is mark properties with the PropertyOverridesAttribute (in order to alter the name or force using string.Format) or RequestUseAttribute (in order additionally specify that the value should be used as a headers value or explicitly in query or path). If the property is of WebHeaderCollection then its values will be added to the request headers. Another ways of extending is to change the binding by registering custom binders or subclass HttpClient class and override its members.

By default the request method is determined based on the input model (if it doesn't have anything to serialize into a request body then it's GET otherwise it's POST) but you can define it explicitly in the application configuration file.

The response is processed smartly - it checks whenever the content MIME type is 'application/json', 'application/xml', 'text/xml' and tries to use appropriate deserialization. However you can supply your own deserialization using subclasses of ResponseDeserializationAttribute defined either on the result model type or the service interface or provide your own implementations of IResponseDeserializationProvider.

When using the factory the service interface must have only one method. If it uses one simple (int, string, double, Guid, etc.) argument or several arguments then the factory will generate a model with properties corresponding to the arguments. It may have the empty argument list or return void.

The alternative way to use the framework is to use the HttpClient<,> class directly. However in this case if you need to use void then must use use supplied VoidType.

The service behaviour can be configured using an application configuration file where you can specify baseURL (instead of on CreateInstance) timeout, headers, authentication, client certificates, proxy.


<a href='http://www.pledgie.com/campaigns/19326'><img alt='Click here to lend your support to: DocaLabs.Http.Client and make a donation at www.pledgie.com !' src='http://www.pledgie.com/campaigns/19326.png?skin_name=chrome' border='0' /></a>

v2.0.0.1 Release Notes
----------------------
There are a lot of changes and enhancements. Some major improvements:
* Ability to map a model properties into the URL's path, e.g. if the RUL is specified like http://foo.com/accounts/{id} and the model as a property named id then the value of that property will be mapped into the path in place of {id}.
* Ability to specify that some properties must be added as headers, a property must be either marked by the RequstUse(RequestUseTargets.RequestHeader) attribute or be of WebHeaderCollection type.
* Ability to supply credentials in the model, a property should be of ICredentials type
* Special treatment of Stream as input or output models.
* HttpClientFactory will be able to generate plumbing to service call methods that have more than one argument - it'll generate a model class with properties for each argument.
* Binding is made more flexible, it's possible to do custom binding per model type for each step in the pipeline - mapping properties to the URL, headers, credentials, writing to request body.
* Ability to get some information about the response - headers, etag, status code

v1.0.1.0 Release Notes
----------------------
Fixed #10 When service call method in interface has void return type the factory generates invalid IL code
