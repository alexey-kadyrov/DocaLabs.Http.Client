using System;
using System.Net;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Base class for synchronous and asynchronous http clients.
    /// </summary>
    public abstract class HttpClientBase
    {
        static readonly IRequestSetup RequestSetup = PlatformAdapter.Resolve<IRequestSetup>();

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
        /// Initializes a new instance of the HttpClientBase class.
        /// </summary>
        /// <param name="baseUrl">The URL of the service.</param>
        /// <param name="configurationName">If the configuration name is not null it'll be used to get the endpoint configuration from the configuration file.</param>
        protected HttpClientBase(Uri baseUrl, string configurationName)
        {
            BaseUrl = baseUrl;

            ReadConfiguration(configurationName);

            if (BaseUrl == null)
                throw new ArgumentException(Resources.Text.service_url_is_not_defined, "baseUrl");
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

            RequestSetup.CopyCredentialsFrom(request, binder, context);

            RequestSetup.CopyHeadersFrom(request, binder, context);

            RequestSetup.CopyClientCertificatesFrom(request, Configuration);

            RequestSetup.CopyWebProxyFrom(request, Configuration);
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
        /// Gets the input model type, if the model is null it returns typeof(TInputModel).
        /// </summary>
        protected virtual Type GetInputModelType<TInputModel>(object model)
        {
            return model == null
                ? typeof(TInputModel)
                : model.GetType();
        }

        /// <summary>
        /// Initializes the execution pipeline:
        ///     * Infers the type of the input model
        ///     * Gets the request model binder for the inferred input model type
        ///     * Transforms the model (if the binder does it)
        ///     * Composes URL using provided base URL and the input model properties
        ///     * Creates and initializes the WebRequest instance
        /// </summary>
        protected virtual InitializedPipeline InitializeExecutionPipeline<TInputModel>(object model, BindingContext context)
        {
            var binder = ModelBinders.GetRequestBinder(context.InputModelType);

            context.Model = binder.TransformModel(context);

            var url = ComposeUrl(binder, context);

            var request = CreateRequest(url);

            context.RequestUrl = request.RequestUri;

            InitializeRequest(binder, context, request);

            return new InitializedPipeline(binder, request);
        }

        void ReadConfiguration(string configurationName)
        {
            Configuration = GetConfigurationElement(configurationName);

            if (Configuration.BaseUrl != null)
                BaseUrl = Configuration.BaseUrl;

            if (string.IsNullOrWhiteSpace(Method))
                Method = Configuration.Method;
        }

        IClientEndpoint GetConfigurationElement(string configurationName)
        {
            if (string.IsNullOrWhiteSpace(configurationName))
                configurationName = GetType().FullName;

            return EndpointConfigurationFactory.Current.GetEndpoint(configurationName) ?? new ClientEndpointElement();
        }

        /// <summary>
        /// Holds the result of the InitializeExecutionPipeline call.
        /// </summary>
        protected class InitializedPipeline 
        {
            /// <summary>
            /// Gets the request binder.
            /// </summary>
            public IRequestBinder RequestBinder { get; private set; }
            
            /// <summary>
            /// Gets the instance of the WebRequest class.
            /// </summary>
            public WebRequest WebRequest { get; private set; }

            /// <summary>
            /// Initializes an instance of the InitializedPipeline class with the specified request binder and WebRequest instance.
            /// </summary>
            public InitializedPipeline(IRequestBinder requestBinder, WebRequest webRequest)
            {
                RequestBinder = requestBinder;
                WebRequest = webRequest;
            }
        }
    }
}
