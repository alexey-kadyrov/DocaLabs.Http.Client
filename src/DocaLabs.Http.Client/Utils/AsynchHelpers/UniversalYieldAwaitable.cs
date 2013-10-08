using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Utils.AsynchHelpers
{
    public class UniversalYieldAwaitable : IUniversalAwaitable
    {
        readonly YieldAwaitable _awaitable;

        public UniversalYieldAwaitable()
        {
            _awaitable = Task.Yield();
        }

        public IUniversalAwaiter GetAwaiter()
        {
            return new UniversalYieldAwaiter(_awaitable.GetAwaiter());
        }

        class UniversalYieldAwaiter : IUniversalAwaiter
        {
            YieldAwaitable.YieldAwaiter _awaiter;

            public UniversalYieldAwaiter(YieldAwaitable.YieldAwaiter awaiter)
            {
                _awaiter = awaiter;
            }

            public void OnCompleted(Action continuation)
            {
                _awaiter.OnCompleted(continuation);
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                _awaiter.UnsafeOnCompleted(continuation);
            }

            public bool IsCompleted { get { return _awaiter.IsCompleted; } }

            public void GetResult()
            {
                _awaiter.GetResult();
            }
        }
    }
}
