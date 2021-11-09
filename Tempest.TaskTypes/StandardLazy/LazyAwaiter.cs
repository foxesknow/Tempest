using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.TaskTypes.StandardLazy
{
    public struct LazyAwaiter<T> : INotifyCompletion
    {
        private readonly Lazy<T> m_Lazy;

        public LazyAwaiter(Lazy<T> lazy)
        {
            m_Lazy = lazy;
        }

        public T GetResult()
        {
            return m_Lazy.Value;
        }

        public bool IsCompleted
        {
            get{return true;}
        }

        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }
    }

    public static class LazyExtensions
    {
        public static LazyAwaiter<T> GetAwaiter<T>(this Lazy<T> lazy)
        {
            return new LazyAwaiter<T>(lazy);
        }
    }
}
