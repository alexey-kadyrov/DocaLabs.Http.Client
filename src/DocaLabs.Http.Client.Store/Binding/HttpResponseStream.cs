using System.Net;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Wraps around the response stream.
    /// </summary>
    public class HttpResponseStream : HttpResponseStreamCore
    {
        internal HttpResponseStream(WebResponse response)
        {
            Response = response;
        }
    }
}
