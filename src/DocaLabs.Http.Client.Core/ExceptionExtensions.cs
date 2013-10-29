using System;
using System.Net;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Exception extensions.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Looks for the WebExpception in the chain of the inner exceptions and compare the StatusCode with the specified value.
        /// </summary>
        public static bool Is(this Exception exception, HttpStatusCode status)
        {
            var webException = TryGetWebException(exception);
            if( webException == null)
                return false;

            var httpResponse = webException.Response as HttpWebResponse;

            return httpResponse != null && httpResponse.StatusCode == status;
        }

        static WebException TryGetWebException(Exception exception)
        {
            while (true)
            {
                if (exception == null)
                    return null;

                var webException = exception as WebException;
                if (webException != null)
                    return webException;

                var aggregateException = exception as AggregateException;
                if (aggregateException != null)
                {
                    foreach (var wrappedException in aggregateException.Flatten().InnerExceptions)
                    {
                        var e = TryGetWebException(wrappedException);
                        if (e != null)
                            return e;
                    }
                }

                exception = exception.InnerException;
            }
        }
    }
}
