using System;

namespace DocaLabs.Http.Client
{
    /// <summary>
    /// Defines methods to execute and retry the action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IExecuteStrategy<T>
    {
        /// <summary>
        /// Executes the given action.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <returns>The return value of the action.</returns>
        T Execute(Func<T> action);
    }
}
