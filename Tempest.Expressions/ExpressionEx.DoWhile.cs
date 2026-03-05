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
        /// Generates a do-while loop
        /// </summary>
        /// <example>
        /// <code>
        /// do
        /// {
        ///     body
        /// }while(predicate)
        /// </code>
        /// </example>
        /// <param name="predicate"></param>
        /// <param name="bodyBuilder"></param>
        /// <returns></returns>
        public static Expression DoWhile(Expression predicate, LoopBodyBuilder bodyBuilder)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            ArgumentNullException.ThrowIfNull(bodyBuilder);

            var (@break, @continue) = MakeBreakAndContinueLabels();

            var body = bodyBuilder(@break, @continue);

            var @if = Expression.IfThenElse
            (
                predicate, 
                Expression.Continue(@continue),
                Expression.Break(@break)
            );

            return Expression.Block
            (
                Expression.Label(@continue),
                body,
                @if,
                Expression.Label(@break)
            );
        }
    }
}
