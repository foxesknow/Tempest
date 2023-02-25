using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
    {
        /// <summary>
        /// Creates a short circuited OR expression
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression OrElse(params Expression[] expressions)
        {
            return OrElse((IEnumerable<Expression>)expressions);
        }

        /// <summary>
        /// Creates a short circuited OR expression
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Expression OrElse(IEnumerable<Expression> expressions)
        {
            if(expressions == null) throw new ArgumentNullException(nameof(expressions));

            var exprs = expressions.ToReadOnlyList();
            if(exprs.Count < 2) throw new ArgumentException("need at least 2 expressions", nameof(expressions));

            var acc = exprs[0];

            for(int i = 1; i < exprs.Count; i++)
            {
                acc = Expression.OrElse(acc, exprs[i]);
            }

            return acc;
        }
    }
}
