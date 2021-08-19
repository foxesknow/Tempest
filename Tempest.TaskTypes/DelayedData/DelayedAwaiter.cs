using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.TaskTypes.DelayedData
{
    /// <summary>
    /// Ensures that a delayed value that comes from a factory
    /// is run on a thread to avoid blocking the caller thread
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct DelayedAwaiter<T> : INotifyCompletion
    {
        private readonly Delayed<T> m_Delayed;

        public DelayedAwaiter(Delayed<T> delayed)
        {
            m_Delayed = delayed;
        }

        public T GetResult()
        {
            return m_Delayed.Value();
        }

        public bool IsCompleted
        {
            get{return m_Delayed.HasValue;}
        }

        public void OnCompleted(Action continuation)
        {
            var delayed = m_Delayed;

            // Just in case the value has arrived since IsCompleted was called...
            if(delayed.HasValue)
            {
                continuation();
            }
            else
            {
                Task.Run(() =>
                {
                    delayed.MakeValueInternal();
                    continuation();
                });
            }
        }
    }
}
