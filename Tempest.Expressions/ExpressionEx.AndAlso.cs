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
        /// Generates a short circuited AND expression
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression AndAlso(params Expression[] expressions)
        {
            return AndAlso((IEnumerable<Expression>)expressions);
        }

        /// <summary>
        /// Generates a short circuited AND expression
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Expression AndAlso(IEnumerable<Expression> expressions)
        {
            if(expressions == null) throw new ArgumentNullException(nameof(expressions));

            var exprs = expressions.ToReadOnlyList();
            if(exprs.Count < 2) throw new ArgumentException("need at least 2 expressions", nameof(expressions));

            var acc = exprs[0];

            for(int i = 1; i < exprs.Count; i++)
            {
                acc = Expression.AndAlso(acc, exprs[i]);
            }

            return acc;
        }
    }
}
