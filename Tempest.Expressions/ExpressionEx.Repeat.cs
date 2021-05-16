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
        /// Creates an expression where the body will be executed a specific number of times
        /// </summary>
        /// <example>
        /// <code>
        /// repeat(count)
        /// {
        ///     body
        /// }
        /// </code>
        /// </example>
        /// <param name="count">How many times to repeat the body</param>
        /// <param name="bodyBuilder"></param>
        /// <returns></returns>
        public static Expression Repeat(Expression count, LoopBodyBuilder bodyBuilder)
        {
            if(count == null) throw new ArgumentNullException(nameof(count));
            if(bodyBuilder == null) throw new ArgumentNullException(nameof(bodyBuilder));

            var (@break, @continue) = MakeBreakAndContinueLabels();
            var repeat = MakeLabel("repeat");

            // TODO: support none int types here
            var counter = Variable<int>("repeatCount");
            var counterInitializer = Expression.Assign(counter, Constants.Int.Int_0);

            var stop = Variable<int>("stop");
            var stopInitializer = Expression.Assign(stop, count);

            var ifBody = bodyBuilder(@break, @continue);

            var condition = Expression.LessThan(counter, stop);
            var body = Expression.IfThenElse(condition, ifBody, Expression.Break(@break));

            var increaseCounter = Expression.PreIncrementAssign(counter);

            return Expression.Block
            (
                new ParameterExpression[]{counter, stop},
                counterInitializer,
                stopInitializer,
                Expression.Label(repeat),
                body,
                Expression.Label(@continue),
                increaseCounter,
                Expression.Continue(repeat),
                Expression.Label(@break)
            );
        }
    }
}
