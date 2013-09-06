using System;
using System.Net;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Http.Client.Utils.ContentEncoding;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Base class for strong typed support for RESTFull service clients.
    /// The concept is that each service endpoint (the service Url) and each protocol method (such as GET, PUT, POST) is considered to be a separate service. 
    /// For testability it's advisable to define an interface for each service definition and you can use HttpClientFactory.CreateInstance
    /// to create instance of a concrete class implementing the interface without manually defining it.
    /// 
    /// public interface IGoldenUserService
    /// {
    ///     User GetGoldenUser(GetUserQuery model);
    /// }
    /// 
    /// var userService = HttpClientFactory.CreateInstance&lt;IGoldenUserService>(); // the base URL must be defined in the app.config 
    ///     or
    /// var userService = HttpClientFactory.CreateInstance&lt;IGoldenUserService>("http://foo.com/");
    /// 
    /// var user = userService.GetGoldenUser(new GetUserQuery(userId));
    /// 
    /// </summary>
    /// <typeparam name="TInputModel">Type which will be used as input parameters that can be serialized into query string or the request stream.</typeparam>
    /// <typeparam name="TOutputModel">Type which will be used as output data that will be deserialized from the response stream.</typeparam>
    public class HttpClient<TInputModel, TOutputModel>
    {
        /// <summary>
        /// Gets a service Url
        /// </summary>
        protected Uri BaseUrl { get; private set; }

        /// <summary>
        /// Gets a protocol method to be used in the request.
        /// If the property returns null or blank string the client will try to deduce the method from the model type using the next rule:
        /// if there is RequestSerializationAttribute defined either on the TQuery class or one of its properties or on the HttpClient's subclass then the method will be POST
        /// otherwise it'll use GET. The default value is null.
        /// </summary>
        protected string Method { get; set; }

        /// <summary>
        /// Gets the service configuration. If it's not defined then the default values will be used.
        /// </summary>
        protected IClientEndpoint Configuration { get; private set; }

        /// <summary>
        /// Execute strategy for calling the remote endpoint.
        /// </summary>
        protected IExecuteStrategy<TInputModel, TOutputModel> ExecuteStrategy { get; private set; }

        /// <summary>
        /// Initializes a new instance of the HttpClient.
        /// </summary>
        /// <param name="baseUrl">The URL of the service.</param>
        /// <param name="configurationName">If the configuration name is not null it'll be used to get the endpoint configuration from the config file.</param>
        /// <param name="executeStrategy">If the parameter null then the default retry strategy will be used.</param>
        public HttpClient(Uri baseUrl = null, string configurationName = null, IExecuteStrategy<TInputModel, TOutputModel> executeStrategy = null)
        {
            BaseUrl = baseUrl;

            ExecuteStrategy = executeStrategy ?? GetDefaultExecuteStrategy();

            ReadConfiguration(configurationName);

            if(BaseUrl == null)
                throw new ArgumentException(Resources.Text.service_url_is_not_defined, "baseUrl");
        }

        /// <summary>
        /// Executes a http request. By default all properties with public getters are serialized into the http query part.
        /// The model class may define some properties to be serialized into the http query part and to serialize some property
        /// into the request body.
        /// The input data serialization behaviour can be altered by:
        ///   * Using IgnoreInRequestAttribute (on class or property level),
        ///   * Using one of the RequestSerializationAttribute derived classes (on the class or property level) 
        ///   * Implementing ICustomQueryMapper interface for custom mapping to query string
        ///   * Overriding TryMakeQueryString and/or TryWriteRequestData
        /// The output data deserialization behaviour can be altered by:
        ///   * Using one of the ResponseDeserializationAttribute derived classes (on the class level)
        ///   * Adding or replacing existing deserialization providers in the DefaultResponseBinder static class
        ///   * Overriding ParseResponse
        /// The remote call is wrapped into the retry strategy.
        /// The execution pipeline is:
        ///     1. Transforms the input model (the default behaviour is to return the same input model)
        ///     1. Builds full URL using the model class by calling UrlBuilder.Compose(BaseUrl, model)
        ///     2. Creates web request (if headers, client certificates, and a proxy are defined in the configuration they will be added to the request)
        ///     3. Writes to the request's body if there is something to write
        ///     4. Gets response from the remote server and parses it
        /// </summary>
        /// <param name="model">Input parameters.</param>
        /// <returns>Output data.</returns>
        public TOutputModel Execute(TInputModel model)
        {
            try
            {
                return ExecuteStrategy.Execute(model, ExecutePipeline);
            }
            catch (HttpClientException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new HttpClientException(string.Format(Resources.Text.failed_execute_request, BaseUrl, GetType().FullName), e);
            }
        }

        /// <summary>
        /// Gets the input model type, if the model is null it returns typeof(TInputModel).
        /// </summary>
        protected virtual Type GetInputModelType(object model)
        {
            return model == null
                ? typeof(TInputModel)
                : model.GetType();
        }

        /// <summary>
        /// Builds a full URL from the BaseUrl and the model object. The method return string instead of Uri for precise control
        /// which may be required in case of URL signing like for some Google services.
        /// </summary>
        /// <returns></returns>
        protected virtual string ComposeUrl(IRequestBinder binder, BindingContext context)
        {
            return binder.ComposeUrl(context);
        }

        /// <summary>
        /// Creates the request.
        /// </summary>
        protected virtual WebRequest CreateRequest(string url)
        {
            return WebRequest.Create(url);
        }

        /// <summary>
        /// Initializes the request. If headers, client certificates, and a proxy are defined in the configuration they will be added to the request
        /// </summary>
        protected virtual void InitializeRequest(IRequestBinder binder, BindingContext context, WebRequest request)
        {
            request.Timeout = Configuration.Timeout;

            request.Method = GetRequestMethod(binder, context);

            if (ShouldSetAcceptEncoding(context))
                ContentDecoderFactory.AddAcceptEncodings(request);

            request.CopyCredentialsFrom(binder, context);

            request.CopyHeadersFrom(binder, context);

            request.CopyClientCertificatesFrom(Configuration);

            request.CopyWebProxyFrom(Configuration);
        }

        /// <summary>
        /// Gets the request method (GET,PUT, etc.). If Method is null or blank then tries to figure out what method to use
        /// by checking the model type.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetRequestMethod(IRequestBinder binder, BindingContext context)
        {
            return string.IsNullOrWhiteSpace(Method) 
                ? binder.InferRequestMethod(context) 
                : Method;
        }

        /// <summary>
        /// If the method returns true the ContentDecoderFactory.AddAcceptEncodings is called to set the Accept-Encoding header. 
        /// The default implementation returns Configuration.AutoSetAcceptEncoding value;
        /// </summary>
        protected virtual bool ShouldSetAcceptEncoding(BindingContext context)
        {
            return Configuration.AutoSetAcceptEncoding;
        }

        /// <summary>
        /// Tries to write data to the request's body by examining the model type.
        /// </summary>
        protected virtual void TryWriteRequestData(IRequestBinder binder, BindingContext context, WebRequest request)
        {
            binder.Write(context, request);
        }

        /// <summary>
        /// Gets the response and parses it. 
        /// </summary>
        protected virtual TOutputModel ParseResponse(BindingContext context, WebRequest request)
        {
            return (TOutputModel)ModelBinders.GetResponseBinder(typeof(TOutputModel)).Read(context, request, typeof(TOutputModel));
        }

        /// <summary>
        /// Gets's the configured default strategy. The timeouts are: 
        ///     1 second between the initial call and the first retry, 
        ///     2 seconds between the first and second retries, 
        ///     5 seconds between the second and third retries.
        /// </summary>
        protected IExecuteStrategy<TInputModel, TOutputModel> GetDefaultExecuteStrategy()
        {
            return new DefaultExecuteStrategy<TInputModel, TOutputModel>(new[]
            {
                TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5)
            });
        }

        /// <summary>
        /// Returns endpoint configuration. If configuration is not found in the app.config (or web.config) then the default will be used.
        /// </summary>
        /// <param name="configurationName">The parameter that was passed to the constructor. It it's empty string then that full class name will be used.</param>
        /// <returns></returns>
        protected virtual IClientEndpoint GetConfigurationElement(string configurationName)
        {
            if (string.IsNullOrWhiteSpace(configurationName))
                configurationName = GetType().FullName;

            return EndpointConfiguration.Current.GetEndpoint(configurationName) ?? new ClientEndpointElement();
        }

        void ReadConfiguration(string configurationName)
        {
            Configuration = GetConfigurationElement(configurationName);

            if (Configuration.BaseUrl != null)
                BaseUrl = Configuration.BaseUrl;

            if (string.IsNullOrWhiteSpace(Method))
                Method = Configuration.Method;
        }

        TOutputModel ExecutePipeline(TInputModel model)
        {
            var context = new BindingContext(this, model, Configuration, BaseUrl);

            var inputModelType = GetInputModelType(model);

            var binder = ModelBinders.GetRequestBinder(inputModelType);

            context.Model = binder.TransformModel(context);

            var url = ComposeUrl(binder, context);

            var request = CreateRequest(url);

            context.RequestUrl = request.RequestUri;

            InitializeRequest(binder, context, request);

            TryWriteRequestData(binder, context, request);

            return ParseResponse(context, request);
        }
    }
}
