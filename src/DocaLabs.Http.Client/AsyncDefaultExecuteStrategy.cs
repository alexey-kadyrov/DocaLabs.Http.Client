using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Implements IExecuteStrategy to execute and retry if possible the action.
    /// </summary>
    public class AsyncDefaultExecuteStrategy<TInputModel, TOutputModel> : DefaultExecuteStrategyBase, IExecuteStrategy<TInputModel, Task<TOutputModel>>
    {
        readonly TimeSpan[] _retryTimeouts;

        /// <summary>
        /// Initializes a new instance of the DefaultExecuteStrategy class with specified retry timeouts.
        /// </summary>
        /// <param name="retryTimeouts">The array specifies the number of retries timeouts between attempts.</param>
        public AsyncDefaultExecuteStrategy(params TimeSpan[] retryTimeouts)
        {
            _retryTimeouts = retryTimeouts ?? new TimeSpan[0];
        }

        /// <summary>
        /// Executes the given action.
        /// </summary>
        public Task<TOutputModel> Execute(TInputModel model, Func<TInputModel, Task<TOutputModel>> action)
        {
            return action(model);
            //if(action == null)
            //    throw new ArgumentNullException("action");

            //var attempt = 0;

            //while (true)
            //{
            //    try
            //    {
            //        return await action(model);
            //    }
            //    catch (Exception e)
            //    {
            //        if (attempt >= _retryTimeouts.Length || !CanRetry(e))
            //        {
            //            OnRethrowing(attempt, _retryTimeouts.Length, e);
            //            throw;
            //        }

            //        OnRetrying(attempt, _retryTimeouts.Length, e);

            //        Thread.Sleep(_retryTimeouts[attempt]);

            //        ++attempt;
            //    }
            //}
        }
    }
}