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
        /// A delegate that creates a while loop
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="bodyBuilder"></param>
        /// <returns></returns>
        private delegate Expression WhileBuilder(Expression predicate, LoopBodyBuilder bodyBuilder);

        /// <summary>
        /// Generates a while loop
        /// </summary>
        /// <example>
        /// <code>
        /// while(predicate)
        /// {
        ///     body
        /// }
        /// </code>
        /// </example>
        /// <param name="predicate"></param>
        /// <param name="bodyBuilder"></param>
        /// <returns></returns>
        public static Expression While(Expression predicate, LoopBodyBuilder bodyBuilder)
        {
            return WhileImpl(predicate, bodyBuilder, null);
        }

        /// <summary>
        /// Returns a function that creates a while loop which may have an optional else part
        /// </summary>
        /// <param name="elseBody"></param>
        /// <returns></returns>
        private static WhileBuilder MakeWhileBuilder(Expression? elseBody)
        {
            if(elseBody is null)
            {
                // Avoid capturing the argument
                return (p, c) => WhileImpl(p, c, null);
            }
            else
            {
                return (p, c) => WhileImpl(p, c, elseBody);
            }
        }

        private static Expression WhileImpl(Expression predicate, LoopBodyBuilder bodyBuilder, Expression? elseBody)
        {
            if(predicate == null) throw new ArgumentNullException(nameof(predicate));
            if(bodyBuilder == null) throw new ArgumentNullException(nameof(bodyBuilder));

            var (@break, @continue) = MakeBreakAndContinueLabels();

            if(elseBody is null)
            {
                var @if = Expression.IfThenElse
                (
                    predicate, 
                    bodyBuilder(@break, @continue),
                    Expression.Break(@break)
                );

                return Expression.Block
                (
                    Expression.Label(@continue),
                    @if,
                    Expression.Goto(@continue),
                    Expression.Label(@break)
                );
            }
            else
            {
                var @else = MakeLabel("else");

                var @if = Expression.IfThenElse
                (
                    predicate, 
                    bodyBuilder(@break, @continue),
                    Expression.Break(@else)
                );

                return Expression.Block
                (
                    Expression.Label(@continue),
                    @if,
                    Expression.Goto(@continue),
                    Expression.Label(@else),
                    @elseBody,                
                    Expression.Label(@break)
                );
            }
        }
    }
}
