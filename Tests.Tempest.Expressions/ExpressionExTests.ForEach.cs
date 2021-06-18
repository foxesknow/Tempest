using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Tempest.Expressions;

using NUnit.Framework;
using System.Collections;

namespace Tests.Tempest.Expressions
{
    public partial class ExpressionExTests
    {
        [Test]
        public void ForEach()
        {
            var parameter = ExpressionEx.Parameter<List<string>>();

            Expression<Action> consoleWriteLine = () => Console.WriteLine((object)null);
            var writeLine = ExpressionEx.GetMethod(consoleWriteLine);

            var body = ExpressionEx.ForEach
            (
                parameter,
                (v, b, c) => Expression.Call(null, writeLine, v)
            );

            var lambda = Expression.Lambda<Action<List<string>>>(body, parameter);
            var action = lambda.Compile();

            var sequence = new List<string>{"Rod", "Jane", "Freddy"};
            action(sequence);
        }

        [Test]
        public void ForEach_Pattern()
        {
            var parameter = ExpressionEx.Parameter<CustomSequence>();

            Expression<Action> consoleWriteLine = () => Console.WriteLine(1);
            var writeLine = ExpressionEx.GetMethod(consoleWriteLine);

            var body = ExpressionEx.ForEach
            (
                parameter,
                (v, b, c) => Expression.Call(null, writeLine, v)
            );

            var lambda = Expression.Lambda<Action<CustomSequence>>(body, parameter);
            var action = lambda.Compile();

            var sequence = new CustomSequence();
            action(sequence);
        }

        [Test]
        public void ForEach_Explicit()
        {
            // GetEnumerator won't be visible on the type so we should use IEnumerable<>
            var parameter = ExpressionEx.Parameter<CustomSequence_Explicit>();

            Expression<Action> consoleWriteLine = () => Console.WriteLine(1);
            var writeLine = ExpressionEx.GetMethod(consoleWriteLine);

            var body = ExpressionEx.ForEach
            (
                parameter,
                (v, b, c) => Expression.Call(null, writeLine, v)
            );

            var lambda = Expression.Lambda<Action<CustomSequence_Explicit>>(body, parameter);
            var action = lambda.Compile();

            var sequence = new CustomSequence_Explicit();
            action(sequence);
        }

        [Test]
        public void ForEach_Disposable()
        {
            // GetEnumerator won't be visible on the type so we should use IEnumerable<>
            var parameter = ExpressionEx.Parameter<CustomSequence_Disposable>();

            Expression<Action> consoleWriteLine = () => Console.WriteLine(1);
            var writeLine = ExpressionEx.GetMethod(consoleWriteLine);

            var body = ExpressionEx.ForEach
            (
                parameter,
                (v, b, c) => Expression.Call(null, writeLine, v)
            );

            var lambda = Expression.Lambda<Action<CustomSequence_Disposable>>(body, parameter);
            var action = lambda.Compile();

            var sequence = new CustomSequence_Disposable();
            action(sequence);
        }

        class CustomSequence
        {
            private readonly List<int> m_Numbers = new(){1, 1, 2, 3, 5};

            public CustomEnumerator<int> GetEnumerator()
            {
                return new CustomEnumerator<int>(m_Numbers);
            }
        }

        class CustomSequence_Disposable
        {
            private readonly List<int> m_Numbers = new(){1, 1, 2, 3, 5};

            public CustomEnumerator_Disposable<int> GetEnumerator()
            {
                return new CustomEnumerator_Disposable<int>(m_Numbers);
            }
        }

        class CustomSequence_Explicit : IEnumerable<int>
        {
            private readonly List<int> m_Numbers = new(){1, 1, 2, 3, 5};

            IEnumerator<int> IEnumerable<int>.GetEnumerator()
            {
                return m_Numbers.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return m_Numbers.GetEnumerator();
            }
        }

        class CustomEnumerator<T>
        {
            private readonly IList<T> m_Sequence;
            private int m_CurrentIndex;

            public CustomEnumerator(IList<T> sequence)
            {
                m_Sequence = sequence;
                m_CurrentIndex = -1;
            }

            public bool MoveNext()
            {
                if(m_CurrentIndex == m_Sequence.Count - 1) return false;

                m_CurrentIndex++;
                return true;
            }

            public T Current
            {
                get{return m_Sequence[m_CurrentIndex];}
            }
        }

        class CustomEnumerator_Disposable<T> : IDisposable
        {
            private readonly IList<T> m_Sequence;
            private int m_CurrentIndex;

            public CustomEnumerator_Disposable(IList<T> sequence)
            {
                m_Sequence = sequence;
                m_CurrentIndex = -1;
            }

            public bool MoveNext()
            {
                if(m_CurrentIndex == m_Sequence.Count - 1) return false;

                m_CurrentIndex++;
                return true;
            }            

            public T Current
            {
                get{return m_Sequence[m_CurrentIndex];}
            }

            public void Dispose()
            {
                Console.WriteLine("Disposed");
            }
        }
    }
}
