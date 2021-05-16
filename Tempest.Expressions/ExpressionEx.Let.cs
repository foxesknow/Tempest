using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
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
        /// <param name="letBuilder"></param>
        /// <returns></returns>
        public static Expression Let(ParameterExpression name, Expression expression, LetBuilder bodyBuilder)
        {
            if(name == null) throw new ArgumentNullException(nameof(name));
            if(expression == null) throw new ArgumentNullException(nameof(expression));
            if(bodyBuilder == null) throw new ArgumentNullException(nameof(bodyBuilder));

            var block = Expression.Block
            (
                new[]{name},
                Expression.Assign(name, expression),
                bodyBuilder(name)
            );

            return block;
        }
    }
}
