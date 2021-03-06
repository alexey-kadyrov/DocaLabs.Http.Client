﻿using System;
using System.Threading;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Implements IExecuteStrategy to execute and retry if possible the action.
    /// </summary>
    public class DefaultExecuteStrategy<TInputModel, TOutputModel> : DefaultExecuteStrategyBase, IExecuteStrategy<TInputModel, TOutputModel>
    {
        readonly TimeSpan[] _retryTimeouts;
        readonly ManualResetEvent _sleeper = new ManualResetEvent(false);

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

                    _sleeper.WaitOne(_retryTimeouts[attempt]);

                    ++attempt;
                }
            }
        }
    }
}