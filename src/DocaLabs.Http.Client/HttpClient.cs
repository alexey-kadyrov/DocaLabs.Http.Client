using System;
using System.Net;
using DocaLabs.Http.Client.Binding;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Base class for strong typed support to synchronously calling RESTFull services.
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
    public class HttpClient<TInputModel, TOutputModel> : HttpClientBase
    {
        /// <summary>
        /// Execute strategy for calling the remote endpoint.
        /// </summary>
        protected IExecuteStrategy<TInputModel, TOutputModel> ExecuteStrategy { get; private set; }

        /// <summary>
        /// Initializes a new instance of the HttpClient class.
        /// </summary>
        /// <param name="baseUrl">The URL of the service.</param>
        /// <param name="configurationName">If the configuration name is not null it'll be used to get the endpoint configuration from the configuration file.</param>
        /// <param name="executeStrategy">If the parameter null then the default retry strategy will be used.</param>
        public HttpClient(Uri baseUrl = null, string configurationName = null, IExecuteStrategy<TInputModel, TOutputModel> executeStrategy = null)
            : base(baseUrl, configurationName)
        {
            ExecuteStrategy = executeStrategy ?? GetDefaultExecuteStrategy();
        }

        /// <summary>
        /// Executes a http request. By default all properties with public getters are serialized into the http query part.
        /// The model class may define some properties to be serialized into the http query part and to serialize some property
        /// into the request body.
        /// The input data serialization behavior can be altered by:
        ///   * Using IgnoreInRequestAttribute (on class or property level),
        ///   * Using one of the RequestSerializationAttribute derived classes (on the class or property level) 
        ///   * Implementing ICustomQueryMapper interface for custom mapping to query string
        ///   * Overriding TryMakeQueryString and/or TryWriteRequestData
        /// The output data deserialization behavior can be altered by:
        ///   * Using one of the ResponseDeserializationAttribute derived classes (on the class level)
        ///   * Adding or replacing existing deserialization providers in the DefaultResponseBinder static class
        ///   * Overriding ParseResponse
        /// The remote call is wrapped into the retry strategy.
        /// The execution pipeline is:
        ///     1. Transforms the input model (the default behavior is to return the same input model)
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
            catch (WebException e)
            {
                throw new HttpClientWebException(string.Format(Resources.Text.failed_execute_request, BaseUrl, GetType().FullName), e);
            }
            catch (Exception e)
            {
                throw new HttpClientException(string.Format(Resources.Text.failed_execute_request, BaseUrl, GetType().FullName), e);
            }
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
            return (TOutputModel)ModelBinders.GetResponseBinder(typeof(TOutputModel)).Read(context, request);
        }

        static IExecuteStrategy<TInputModel, TOutputModel> GetDefaultExecuteStrategy()
        {
            return new DefaultExecuteStrategy<TInputModel, TOutputModel>(new[]
            {
                TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5)
            });
        }

        TOutputModel ExecutePipeline(TInputModel model)
        {
            var context = new BindingContext(this, model, Configuration, BaseUrl, GetInputModelType<TInputModel>(model), typeof(TOutputModel));

            var pipeline = InitializeExecutionPipeline<TInputModel>(model, context);

            TryWriteRequestData(pipeline.RequestBinder, context, pipeline.WebRequest);

            return ParseResponse(context, pipeline.WebRequest);
        }
    }
}
