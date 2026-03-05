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
        /// Generates a let binding ala F#
        /// </summary>
        /// <example>
        /// <code>
        /// let name = expression in body
        /// </code>
        /// </example>
        /// <param name="name"></param>
        /// <param name="expression"></param>
        /// <param name="in"></param>
        /// <returns></returns>
        public static Expression Let(ParameterExpression name, Expression expression, LetBuilder @in)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(expression);
            ArgumentNullException.ThrowIfNull(@in);

            var block = Expression.Block
            (
                new[]{name},
                Expression.Assign(name, expression),
                @in(name)
            );

            return block;
        }

         /// <summary>
        /// Generates a let binding ala F#.
        /// The expression is evaluated and assigned to a variable passed to "in"
        /// </summary>
        /// <example>
        /// <code>
        /// let some-name = expression in body
        /// </code>
        /// </example>
        /// <param name="expression"></param>
        /// <param name="in"></param>
        /// <returns></returns>
        public static Expression Let(Expression expression, LetBuilder @in)
        {
            return Let(Expression.Parameter(expression.Type), expression, @in);
        }
    }
}