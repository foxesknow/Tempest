using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tempest.TaskTypes.DelayedData
{
    /// <summary>
    /// Represents a value whose construction is delayed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Delayed<T>
    {
        private BoxedValue? m_Value;
        private readonly Func<T>? m_Factory;
        
        private object? m_SyncRoot;
        private TaskCompletionSource<T>? m_Tcs;

        /// <summary>
        /// Creates a delayed value that has the specified value
        /// </summary>
        /// <param name="value"></param>
        public Delayed(T value)
        {
            m_Value = new BoxedValue(value);
        }

        /// <summary>
        /// Creates a delayed value whose value will come from the specified factory
        /// </summary>
        /// <param name="factory"></param>
        public Delayed(Func<T> factory)
        {
            if(factory == null) throw new ArgumentNullException(nameof(factory));

            m_Factory = factory;
        }

        /// <summary>
        /// Returns true if a value is available, otherwise false
        /// </summary>
        public bool HasValue
        {
            get{return m_Value != null;}
        }

        /// <summary>
        /// Returns the delayed value.
        /// If we haven't retrieved the value it will be retrieved on the current thread
        /// </summary>
        public T Value()
        {
            return RealizeValue();
        }

        /// <summary>
        /// Returns a task that will retrieve the delayed value
        /// </summary>
        /// <returns></returns>
        public Task<T> AsTask()
        {
            return RealizeTask();
        }

        private Task<T> RealizeTask()
        {
            if(m_Tcs != null) return m_Tcs.Task;

            var tcs = MakeTcs();
            if(Interlocked.CompareExchange(ref m_Tcs, tcs, null) is var existing && existing != null)
            {
                return existing.Task;

            }

            if(m_Value != null)
            {
                m_Value.Apply(tcs);
            }
            else
            {
                // We'll get the value on a new task so that this method won't
                // block on making the value
                Task.Run(() =>
                {
                    var value = MakeValue();
                    value.Apply(tcs);
                });
            }

            return tcs.Task;
        }

        private T RealizeValue()
        {
            // If we've already got the value everything is easy
            if(m_Value != null) return m_Value.GetValue();

            // We'll have to go get the value
            var value = MakeValue();
            return value.GetValue();
        }

        internal void MakeValueInternal()
        {
            MakeValue();
        }

        private BoxedValue MakeValue()
        {
            lock(GetSyncRoot())
            {
                if(m_Value == null)
                {
                    try
                    {
                        var value = m_Factory!();
                        m_Value = new BoxedValue(value);
                    }
                    catch(Exception e)
                    {
                        m_Value = new BoxedValue(e);
                    }
                }

                return m_Value;
            }
        }

        /// <summary>
        /// Create a sync root on demand to avoid wasting memory
        /// </summary>
        /// <returns></returns>
        private object GetSyncRoot()
        {
            if(m_SyncRoot != null) return m_SyncRoot;

            var syncRoot = new object();
            var previousSyncRoot = Interlocked.CompareExchange(ref m_SyncRoot, syncRoot, null);

            return previousSyncRoot ?? syncRoot;
        }

        public static TaskCompletionSource<T> MakeTcs()
        {
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            return tcs;
        }

        class BoxedValue
        {
            private readonly Exception? m_Exception;
            private readonly T m_Value;

            public BoxedValue(Exception exception)
            {
                m_Exception = exception;
                m_Value = default!;
            }

            public BoxedValue(T value)
            {
                m_Value = value;
            }

            public T GetValue()
            {
                if(m_Exception != null) throw m_Exception;

                return m_Value;
            }

            public Task<T> GetTask()
            {
                if(m_Exception != null)
                {
                    return Task.FromException<T>(m_Exception);
                }

                return Task.FromResult(m_Value);
            }

            public void Apply(TaskCompletionSource<T> tcs)
            {
                if(m_Exception != null)
                {
                    tcs.SetException(m_Exception);
                }
                else
                {
                    tcs.SetResult(m_Value);
                }
            }            
        }
    }

    public static class DelayedExtensions
    {
        public static DelayedAwaiter<T> GetAwaiter<T>(this Delayed<T> delayed)
        {
            return new DelayedAwaiter<T>(delayed);
        }
    }
}
