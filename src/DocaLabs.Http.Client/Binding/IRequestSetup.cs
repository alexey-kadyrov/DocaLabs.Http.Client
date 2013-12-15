using System.Net;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Configuration;

namespace DocaLabs.Http.Client.Binding
{
    public interface IRequestSetup
    {
        /// <summary>
        /// Sets the ContentLength to zero if the method is POST or PUT and the request is HttpWebRequest or FileWebRequest.
        /// </summary>
        void SetContentLengthToZeroIfBodyIsRequired(WebRequest request, BindingContext context);

        /// <summary>
        /// Sets the ContentLength to zero if the method is POST or PUT and the request is HttpWebRequest or FileWebRequest.
        /// </summary>
        Task SetContentLengthToZeroIfBodyIsRequiredAsync(WebRequest request, AsyncBindingContext context);

        /// <summary>
        /// Returns true if the method is POST or PUT and the request is HttpWebRequest or FileWebRequest.
        /// </summary>
        bool IsBodyRequired(WebRequest request);

        /// <summary>
        /// If headers are defined in the endpoint configuration then the methods adds them to the request.
        /// </summary>
        void CopyHeadersFrom(WebRequest request, IRequestBinder binder, BindingContext context);

        /// <summary>
        /// If client certificates are defined in the endpoint configuration then the methods adds them to the request.
        /// </summary>
        void CopyClientCertificatesFrom(WebRequest request, IClientEndpoint endpoint);

        /// <summary>
        /// If the AuthenticationLevel and Credential are defined then the method copies them into the request.
        /// </summary>
        void CopyCredentialsFrom(WebRequest request, IRequestBinder binder, BindingContext context);

        /// <summary>
        /// If web proxy are defined in the endpoint configuration then the methods adds it to the request.
        /// </summary>
        void CopyWebProxyFrom(WebRequest request, IClientEndpoint endpoint);
    }
}