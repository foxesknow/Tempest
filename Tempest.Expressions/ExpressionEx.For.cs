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
    public static partial class ExpressionEx
    {
        /// <summary>
        /// Generates a for loop.
        /// Note, iteration is explicity left to the caller
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="iteration"></param>
        /// <param name="bodyBuilder"></param>
        /// <returns></returns>
        public static Expression For(Expression predicate, Expression iteration, LoopBodyBuilder bodyBuilder)
        {
            if(predicate == null) throw new ArgumentNullException(nameof(predicate));
            if(iteration == null) throw new ArgumentNullException(nameof(iteration));
            if(bodyBuilder == null) throw new ArgumentNullException(nameof(bodyBuilder));

            var repeat = MakeLabel("repeat");
            var (@break, @continue) = MakeBreakAndContinueLabels();
            var body = bodyBuilder(@break, @continue);

            var ifBody = Expression.IfThenElse(predicate, body, Expression.Break(@break));

            return Expression.Block
            (
                Expression.Label(repeat),
                ifBody,
                Expression.Label(@continue),
                iteration,
                Expression.Goto(repeat),
                Expression.Label(@break)
            );
        }
    }
}
