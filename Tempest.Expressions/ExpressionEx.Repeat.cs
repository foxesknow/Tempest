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
        /// <param name="count">How many times to repeat the body</param>
        /// <param name="bodyBuilder"></param>
        /// <returns></returns>
        public static Expression Repeat(int count, LoopBodyBuilder bodyBuilder)
        {
            if(bodyBuilder == null) throw new ArgumentNullException(nameof(bodyBuilder));

            var (@break, @continue) = MakeBreakAndContinueLabels();
            var repeat = MakeLabel("repeat");

            var counter = Expression.Variable(typeof(int));
            var initializer = Expression.Assign(counter, Expression.Constant(0));

            var ifBody = bodyBuilder(@break, @continue);

            var condition = Expression.LessThan(counter, Expression.Constant(count));
            var body = Expression.IfThenElse(condition, ifBody, Expression.Break(@break));

            var increaseCounter = Expression.PreIncrementAssign(counter);

            return Expression.Block
            (
                new ParameterExpression[]{counter},
                initializer,
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
