using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Tempest.Expressions;

public static partial class ExpressionEx
{
    extension(Expression)
    {
        /// <summary>
        /// Discards the return value of an expression by collapsing it to void
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Expression DiscardResult(Expression expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            if(expression.Type == typeof(void)) return expression;

            return Expression.Block
            (
                expression,
                Expression.Void
            );
        }
    }
}
