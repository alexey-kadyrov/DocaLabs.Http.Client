using System;
using System.Net;
using DocaLabs.Http.Client.Binding.RequestSerialization;
using DocaLabs.Http.Client.Binding.ResponseDeserialization;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Default implementation of the IModelBinder
    /// </summary>
    public class DefaultModelBinder : IModelBinder
    {
        readonly DefaultUrlComposer _urlComposer;
        readonly DefaultRequestWriter _requestWriter;
        readonly DefaultHeaderMapper _headerMapper;
        readonly DefaultCredentialsMapper _credentialsMapper;
        readonly DefaultResponseReader _responseReader;

        /// <summary>
        /// Initializes a new instance of the DefaultModelBinder class.
        /// </summary>
        public DefaultModelBinder()
        {
            _urlComposer = new DefaultUrlComposer();
            _requestWriter = new DefaultRequestWriter();
            _headerMapper = new DefaultHeaderMapper();
            _credentialsMapper = new DefaultCredentialsMapper();
            _responseReader = new DefaultResponseReader();
        }

        /// <summary>
        /// Returns the input model as its passed without any modification.
        /// </summary>
        public virtual object TransformInputModel(BindingContext context)
        {
            return context.OriginalModel;
        }

        /// <summary>
        /// Uses DefaultUrlComposer to compose a request's URL using based on an input model and base URL.
        /// </summary>
        public virtual string ComposeUrl(BindingContext context)
        {
            return _urlComposer.Compose(context.Model, context.BaseUrl);
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
            return _headerMapper.Map(context.Model);
        }

        /// <summary>
        /// Uses DefaultCredentialsMapper to get credentials from an input model.
        /// </summary>
        public virtual ICredentials GetCredentials(BindingContext context, Uri url)
        {
            return _credentialsMapper.Map(context.Model, url);
        }

        /// <summary>
        /// Uses DefaultRequestWriter to write data into request's stream.
        /// </summary>
        public virtual void Write(BindingContext context, WebRequest request)
        {
            _requestWriter.Write(context.HttpClient, context.Model, request);
        }

        /// <summary>
        /// Uses DefaultResponseReader to process response.
        /// </summary>
        public virtual object Read(BindingContext context, HttpResponse response, Type resultType)
        {
            return _responseReader.Read(response, resultType);
        }
    }
}
