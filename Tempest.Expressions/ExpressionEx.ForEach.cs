using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;

namespace Tempest.Expressions
{
    public partial class ExpressionEx
    {
        /// <summary>
        /// Creates a foreach expression that enumerates over a sequence
        /// </summary>
        /// <example>
        /// <code></code>
        /// </example>
        /// <param name="sequence"></param>
        /// <param name="bodyBuilder"></param>
        /// <returns></returns>
        public static Expression ForEach(Expression sequence, LetLoopBodyBuilder bodyBuilder)
        {
            if(sequence == null) throw new ArgumentNullException(nameof(sequence));
            if(bodyBuilder == null) throw new ArgumentNullException(nameof(bodyBuilder));

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
                    return While(Expression.Call(enumerator, moveNext), (@break, @continue) =>
                    {
                        return Let(Expression.Parameter(itemType), Expression.Property(enumerator, current), let => bodyBuilder(let, @break, @continue));
                    });
                });
            });

            return block;
        }
    }
}
