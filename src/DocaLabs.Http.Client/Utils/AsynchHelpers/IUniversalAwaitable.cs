namespace DocaLabs.Http.Client.Utils.AsynchHelpers
{
    /// <summary>
    /// Universal awaitable due that the TaskAwaitable and YieldAwaitable don't share any base type.
    /// </summary>
    public interface IUniversalAwaitable
    {
        IUniversalAwaiter GetAwaiter();
    }

    /// <summary>
    /// Universal awaitable due that the Task{T} is not covariant.
    /// </summary>
    public interface IUniversalAwaitable<out T>
    {
        IUniversalAwaiter<T> GetAwaiter();
    }
}