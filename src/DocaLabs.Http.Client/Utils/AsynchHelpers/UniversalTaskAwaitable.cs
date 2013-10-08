using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Utils.AsynchHelpers
{
    public class UniversalTaskAwaitable : IUniversalAwaitable
    {
        readonly Task _awaitable;

        public UniversalTaskAwaitable(Task awaitable)
        {
            _awaitable = awaitable;
        }

        public IUniversalAwaiter GetAwaiter()
        {
            return new UniversalTaskAwaiter(_awaitable.GetAwaiter());
        }

        class UniversalTaskAwaiter : IUniversalAwaiter
        {
            TaskAwaiter _awaiter;

            public UniversalTaskAwaiter(TaskAwaiter awaiter)
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

    public class UniversalTaskAwaitable<T> : IUniversalAwaitable<T>
    {
        readonly Task<T> _awaitable;

        public UniversalTaskAwaitable(Task<T> awaitable)
        {
            _awaitable = awaitable;
        }

        public IUniversalAwaiter<T> GetAwaiter()
        {
            return new UniversalTaskAwaiter<T>(_awaitable.GetAwaiter());
        }

        class UniversalTaskAwaiter<T> : IUniversalAwaiter<T>
        {
            TaskAwaiter<T> _awaiter;

            public UniversalTaskAwaiter(TaskAwaiter<T> awaiter)
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

            public T GetResult()
            {
                return _awaiter.GetResult();
            }
        }
    }
}