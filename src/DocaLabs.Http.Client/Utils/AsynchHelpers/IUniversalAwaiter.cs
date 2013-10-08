using System.Runtime.CompilerServices;

namespace DocaLabs.Http.Client.Utils.AsynchHelpers
{
    public interface IUniversalAwaiter : ICriticalNotifyCompletion 
    {
        bool IsCompleted { get; }
        void GetResult();
    }

    public interface IUniversalAwaiter<out T>
    {
        T GetResult();
    }
}