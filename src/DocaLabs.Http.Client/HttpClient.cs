using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using DocaLabs.Http.Client.Binding;
using DocaLabs.Http.Client.Binding.RequestSerialization;
using DocaLabs.Http.Client.Configuration;
using DocaLabs.Http.Client.ContentEncoding;
using DocaLabs.Http.Client.ResponseDeserialization;
using DocaLabs.Http.Client.Utils;

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
        /// Retry strategy for calling the remote endpoint.
        /// </summary>
        protected Func<Func<TOutputModel>, TOutputModel> RetryStrategy { get; private set; }

        /// <summary>
        /// Initializes a new instance of the HttpClient.
        /// </summary>
        /// <param name="baseUrl">The URL of the service.</param>
        /// <param name="configurationName">If the configuration name is not null it'll be used to get the endpoint configuration from the config file.</param>
        /// <param name="retryStrategy">If the parameter null then the default retry strategy will be used.</param>
        public HttpClient(Uri baseUrl = null, string configurationName = null, Func<Func<TOutputModel>, TOutputModel> retryStrategy = null)
        {
            BaseUrl = baseUrl;
            RetryStrategy = retryStrategy ?? GetDefaultRetryStrategy();

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
        ///   * Adding or replacing existing deserialization providers in the ResponseParser static class
        ///   * Overriding ParseResponse
        /// The remote call is wrapped into the retry strategy.
        /// </summary>
        /// <param name="model">Input parameters.</param>
        /// <returns>Output data.</returns>
        public TOutputModel Execute(TInputModel model)
        {
            try
            {
                return RetryStrategy(() => ExecutePipeline(model));
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
        /// Executes the actual request execution pipeline.
        ///     1. Builds full URL using the model class by calling UrlBuilder.Compose(BaseUrl, model)
        ///     2. Creates web request (if headers, client certificates, and a proxy are defined in the configuration they will be added to the request)
        ///     3. Writes to the request's body if there is something to write
        ///     4. Gets response from the remote server and parses it
        /// </summary>
        protected virtual TOutputModel ExecutePipeline(object model)
        {
            var url = ComposeUrl(model);

            var request = CreateRequest(url);

            InitializeRequest(model, request);

            TryWriteRequestData(model, request);

            return ParseResponse(model, request);
        }

        /// <summary>
        /// Builds a full URL from the BaseUrl and the model object. The method return string instead of Uri for precise control
        /// which may be required in case of URL signing like for some Google services.
        /// </summary>
        /// <returns></returns>
        protected virtual string ComposeUrl(object model)
        {
            return model == null
                ? BaseUrl.AbsoluteUri 
                : ClientModelBinders.GetUrlComposer(model.GetType()).Compose(model, BaseUrl);
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
        protected virtual void InitializeRequest(object model, WebRequest request)
        {
            request.Timeout = Configuration.Timeout;

            request.Method = GetRequestMethod(model);

            if (Configuration.AutoSetAcceptEncoding && (!typeof(Image).IsAssignableFrom(typeof(TOutputModel))))
                ContentDecoderFactory.AddAcceptEncodings(request);

            request.CopyCredentialsFrom(Configuration, model);

            request.CopyHeadersFrom(Configuration, model);

            request.CopyClientCertificatesFrom(Configuration);

            request.CopyWebProxyFrom(Configuration);
        }

        /// <summary>
        /// Gets the request method (GET,PUT, etc.). If Method is null or blank then tries to figure out what method to use
        /// by checking the model type.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetRequestMethod(object model)
        {
            if (!string.IsNullOrWhiteSpace(Method))
                return Method;

            return ShouldSerializeToStream(model) ? WebRequestMethods.Http.Post : WebRequestMethods.Http.Get;
        }

        bool ShouldSerializeToStream(object model)
        {
            if (model == null)
                return false;

            var modelType = model.GetType();

            return modelType.GetCustomAttribute<RequestSerializationAttribute>(true) != null
                   || GetType().GetCustomAttribute<RequestSerializationAttribute>(true) != null
                   || modelType.GetAllInstancePublicProperties().Any(x => x.GetCustomAttribute<RequestSerializationAttribute>(true) != null);
        }

        /// <summary>
        /// Tries to write data to the request's body by examining the model type.
        /// </summary>
        protected virtual void TryWriteRequestData(object model, WebRequest request)
        {
            if(model != null)
                ClientModelBinders.GetRequestWriter(model.GetType()).Write(model, this, request);
        }

        /// <summary>
        /// Gets the response and parses it. 
        /// </summary>
        protected virtual TOutputModel ParseResponse(object query, WebRequest request)
        {
            return (TOutputModel)ResponseParser.Parse(request, typeof(TOutputModel));
        }

        /// <summary>
        /// Simple retry strategy to call the remote server, if the call fails it will be retried the defines number of times after each time increasing the timeout by stepbackIncrease.
        /// </summary>
        protected TOutputModel DefaultRetryStrategy(Func<TOutputModel> action, int retries, int initialTimeout, int stepbackIncrease)
        {
            var timeout = initialTimeout;

            var attempt = 1;

            while (true)
            {
                try
                {
                    return action();
                }
                catch (ArgumentException)
                {
                    throw;
                }
                catch (NullReferenceException)
                {
                    throw;
                }
                catch (UnrecoverableHttpClientException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    if (retries <= 0)
                        throw;

                    OnLogRetry(++attempt, e);

                    --retries;

                    Thread.Sleep(timeout);

                    timeout += stepbackIncrease;
                }
            }
        }
    
        /// <summary>
        /// Is called each time before retry. The default implementation uses Debug.Write.
        /// </summary>
        protected virtual void OnLogRetry(int attempt, Exception e)
        {
            if(e == null)
                Trace.WriteLine(string.Format(Resources.Text.will_try_again, attempt));
            else
                Trace.WriteLine(string.Format(Resources.Text.will_try_again, attempt) + ": " + e);
        }

        /// <summary>
        /// Gets's the configured default strategy. It has 3 retries with initial timeout of 1 sec and step back of 1 sec.
        /// So the timeouts will be: 1 sec after the initial call, 2 sec after the first retry, 3 sec after the second retry.
        /// </summary>
        protected Func<Func<TOutputModel>, TOutputModel> GetDefaultRetryStrategy()
        {
            return action => DefaultRetryStrategy(action, 3, 1000, 1000);
        }
       
        void ReadConfiguration(string configurationName)
        {
            Configuration = GetConfigurationElement(configurationName);

            if (Configuration.BaseUrl != null)
                BaseUrl = Configuration.BaseUrl;

            if (string.IsNullOrWhiteSpace(Method))
                Method = Configuration.Method;
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
    }
}
