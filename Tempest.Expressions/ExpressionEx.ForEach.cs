using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Tempest.Expressions
{
    public partial class ExpressionEx
    {
        /// <summary>
        /// Creates a foreach expression that enumerates over a sequence
        /// </summary>
        /// <example>
        /// <code>
        /// foreach(some-var in sequence)
        /// {
        ///     body
        /// }
        /// </code>
        /// </example>
        /// <param name="sequence"></param>
        /// <param name="bodyBuilder"></param>
        /// <returns></returns>
        public static Expression ForEach(Expression sequence, LetLoopBodyBuilder bodyBuilder)
        {
            if(sequence == null) throw new ArgumentNullException(nameof(sequence));
            if(bodyBuilder == null) throw new ArgumentNullException(nameof(bodyBuilder));

            var whileBuilder = MakeWhileBuilder(null);

            if(TryForEachFromPattern(sequence, bodyBuilder, whileBuilder, out var forEach))
            {
                return forEach;
            }

            return ForEachFromIEnumerable(sequence, bodyBuilder, whileBuilder);
        }

        private static bool TryForEachFromPattern(Expression sequence, LetLoopBodyBuilder bodyBuilder, WhileBuilder whileBuilder, [NotNullWhen(true)] out Expression? forEach)
        {
            /*
             * C# applies a pattern matching approach to foreach.
             * If a type has a GetEnumerator method that returns a type that has the shape:
             * 
             *     bool MoveNext();
             *     T Current{get;}
             *     
             * then it will use this method, rather than querying for the IEnumerable interfaces and using those.
             * The type of Current will decide the type
             */

            forEach = null;

            var flags = BindingFlags.Public | BindingFlags.Instance;

            var getEnumerator = sequence.Type.TryGetMethod("GetEnumerator", flags, Type.EmptyTypes);
            if(getEnumerator == null) return false;

            var enumeratorType = getEnumerator.ReturnType;
            if(enumeratorType == null || enumeratorType == typeof(void)) return false;

            var current = enumeratorType.GetProperty("Current", flags);
            if(current == null) return false;

            var itemType = current.PropertyType;

            var moveNext = enumeratorType.GetMethod("MoveNext", flags, Type.EmptyTypes);
            if(moveNext == null || moveNext.ReturnType != typeof(bool)) return false;

            var block = Let(Expression.Variable(enumeratorType, "enumerator"), Expression.Call(sequence, getEnumerator), enumerator =>
            {
                if(typeof(IDisposable).IsAssignableFrom(enumeratorType))
                {
                    return Using(enumerator, b =>
                    {
                        return whileBuilder(Expression.Call(enumerator, moveNext), (@break, @continue) =>
                        {
                            return Let(Expression.Parameter(itemType), Expression.Property(enumerator, current), let => bodyBuilder(let, @break, @continue));
                        });
                    });
                }
                else
                {
                    return whileBuilder(Expression.Call(enumerator, moveNext), (@break, @continue) =>
                    {
                        return Let(Expression.Parameter(itemType), Expression.Property(enumerator, current), let => bodyBuilder(let, @break, @continue));
                    });
                }
            });

            forEach = block;
            return true;
        }

        private static Expression ForEachFromIEnumerable(Expression sequence, LetLoopBodyBuilder bodyBuilder, WhileBuilder whileBuilder)
        {
            if(sequence.Type.TryGetGenericImplementation(typeof(IEnumerable<>), out var enumerableType) == false)
            {
                throw new ArgumentException("sequence is not enumerable", nameof(sequence));
            }

            var itemType = enumerableType.GetGenericArguments()[0];
            var enumeratorType = typeof(IEnumerator<>).MakeGenericType(itemType);

            var getEnumerator = enumerableType.GetMethod("GetEnumerator")!;
            var current = enumeratorType.GetProperty("Current")!;
            var moveNext = typeof(IEnumerator).GetMethod("MoveNext")!;

            var block = Let(Expression.Variable(enumerableType), sequence, enumerable =>
            {
                return Using(Expression.Call(enumerable, getEnumerator), enumerator =>
                {
                    return whileBuilder(Expression.Call(enumerator, moveNext), (@break, @continue) =>
                    {
                        return Let(Expression.Parameter(itemType), Expression.Property(enumerator, current), let => bodyBuilder(let, @break, @continue));
                    });
                });
            });

            return block;
        }
    }
}
