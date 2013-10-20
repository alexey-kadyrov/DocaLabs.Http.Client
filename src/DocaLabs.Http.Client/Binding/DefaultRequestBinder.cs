using System.Net;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Default implementation of the IRequestBinder
    /// </summary>
    public class DefaultRequestBinder : IRequestBinder, IAsyncRequestWriter
    {
        readonly DefaultUrlComposer _urlComposer;
        readonly DefaultRequestWriter _requestWriter;
        readonly DefaultHeaderMapper _headerMapper;
        readonly DefaultCredentialsMapper _credentialsMapper;

        /// <summary>
        /// Initializes a new instance of the DefaultRequestBinder class.
        /// </summary>
        public DefaultRequestBinder()
        {
            _urlComposer = new DefaultUrlComposer();
            _requestWriter = new DefaultRequestWriter();
            _headerMapper = new DefaultHeaderMapper();
            _credentialsMapper = new DefaultCredentialsMapper();
        }

        /// <summary>
        /// Returns the input model as its passed without any modification.
        /// </summary>
        public virtual object TransformModel(BindingContext context)
        {
            return context.OriginalModel;
        }

        /// <summary>
        /// Uses DefaultUrlComposer to compose a request's URL using based on an input model and base URL.
        /// </summary>
        public virtual string ComposeUrl(BindingContext context)
        {
            return _urlComposer.Compose(context.HttpClient, context.Model, context.BaseUrl);
        }

        /// <summary>
        /// Uses DefaultRequestWriter to infer the request's methods based on an input model.
        /// </summary>
        public virtual string InferRequestMethod(BindingContext context)
        {
            return _requestWriter.InferRequestMethod(context.HttpClient, context.Model);
        }

        /// <summary>
        /// Uses DefaultHeaderMapper to get headers from an input model.
        /// </summary>
        public virtual WebHeaderCollection GetHeaders(BindingContext context)
        {
            return _headerMapper.Map(context.HttpClient, context.Model);
        }

        /// <summary>
        /// Uses DefaultCredentialsMapper to get credentials from an input model.
        /// </summary>
        public virtual ICredentials GetCredentials(BindingContext context)
        {
            return _credentialsMapper.Map(context.HttpClient, context.Model, context.RequestUrl);
        }

        /// <summary>
        /// Uses DefaultRequestWriter to write data into request's stream.
        /// </summary>
        public virtual void Write(BindingContext context, WebRequest request)
        {
            _requestWriter.Write(context.HttpClient, context.Model, request);
        }

        /// <summary>
        /// Uses DefaultRequestWriter to asynchronously write data into request's stream.
        /// </summary>
        public Task WriteAsync(AsyncBindingContext context, WebRequest request)
        {
            return _requestWriter.WriteAsync(context.HttpClient, context.Model, request, context.CancellationToken);
        }
    }
}
