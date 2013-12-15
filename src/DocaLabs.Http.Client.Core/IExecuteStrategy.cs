using System;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Defines methods to execute and retry the action.
    /// </summary>
    public interface IExecuteStrategy<TInputModel, TOutputModel>
    {
        /// <summary>
        /// Executes the given action.
        /// </summary>
        TOutputModel Execute(TInputModel model, Func<TInputModel, TOutputModel> action);
    }
}
