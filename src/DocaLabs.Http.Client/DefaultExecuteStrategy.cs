using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Implements IExecuteStrategy to execute and retry if possible the action.
    /// </summary>
    public class DefaultExecuteStrategy<TInputModel, TOutputModel> : IExecuteStrategy<TInputModel, TOutputModel>
    {
        readonly TimeSpan[] _retryTimeouts;

        /// <summary>
        /// Initializes a new instance of the DefaultExecuteStrategy class with specified retry timeouts.
        /// </summary>
        /// <param name="retryTimeouts">The array specifies the number of retries timeouts between attempts.</param>
        public DefaultExecuteStrategy(params TimeSpan[] retryTimeouts)
        {
            _retryTimeouts = retryTimeouts ?? new TimeSpan[0];
        }

        /// <summary>
        /// Executes the given action.
        /// </summary>
        public TOutputModel Execute(TInputModel model, Func<TInputModel, TOutputModel> action)
        {
            if(action == null)
                throw new ArgumentNullException("action");

            var attempt = 0;

            while (true)
            {
                try
                {
                    return action(model);
                }
                catch (Exception e)
                {
                    if (attempt >= _retryTimeouts.Length || !CanRetry(e))
                    {
                        OnRethrowing(attempt, _retryTimeouts.Length, e);
                        throw;
                    }

                    OnRetrying(attempt, _retryTimeouts.Length, e);

                    Thread.Sleep(_retryTimeouts[attempt]);

                    ++attempt;
                }
            }
        }

        /// <summary>
        /// Returns true if it's practical to retry the action after an exception.
        /// The method returns false if the exception is or derived from: 
        /// ArgumentException, NullReferenceException, NotSupportedException, NotImplementedException, HttpClientException
        /// The method must not throw any exceptions on its own.
        /// </summary>
        protected virtual bool CanRetry(Exception e)
        {
            if(e is ArgumentException)
                return false;

            if(e is NullReferenceException)
                return false;

            if(e is NotSupportedException)
                return false;

            if(e is NotImplementedException)
                return false;

            var webException = e as WebException;
            if (webException != null)
                return CanRetry(webException);

            return !(e is HttpClientException);
        }

        /// <summary>
        /// Checks whenever the WebExcpetion can be retried. It's called from CanRetry(Exception e).
        /// The default implementation returns true only for HttpStatusCode.GatewayTimeout or HttpStatusCode.RequestTimeout or Status is WebExceptionStatus.ConnectFailure.
        /// </summary>
        protected virtual bool CanRetry(WebException e)
        {
            if (e.Status == WebExceptionStatus.ConnectFailure)
                return true;

            var httpResponse = e.Response as HttpWebResponse;
            return httpResponse != null && 
                    (httpResponse.StatusCode == HttpStatusCode.GatewayTimeout || httpResponse.StatusCode == HttpStatusCode.RequestTimeout);
        }

        /// <summary>
        /// Is called each time before retry. The default implementation uses Trace.TraceError.
        /// The method must not throw any exceptions on its own.
        /// </summary>
        protected virtual void OnRetrying(int attempt, int maxRetries, Exception e)
        {
            // ReSharper disable EmptyGeneralCatchClause
            
            try
            {
                Trace.TraceError(string.Format(Resources.Text.will_try_again, attempt, maxRetries, e));
            }
            catch
            {
            }

            // ReSharper restore EmptyGeneralCatchClause
        }

        /// <summary>
        /// Is called when the exception is re-thrown. The default implementation does nothing.
        /// The method must not throw any exceptions on its own.
        /// </summary>
        protected virtual void OnRethrowing(int attempt, int maxRetries, Exception e)
        {
        }
    }
}