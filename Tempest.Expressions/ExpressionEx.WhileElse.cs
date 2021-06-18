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
        /// Implements a Python while/else construct
        /// </summary>
        /// <example>
        /// <code>
        /// while(predicate)
        /// {
        ///     // body
        /// }
        /// else
        /// {
        ///     // When predicate is false
        /// }
        /// </code>
        /// </example>
        /// <param name="predicate"></param>
        /// <param name="whileBodyBuilder"></param>
        /// <param name="elseBody"></param>
        /// <returns></returns>
        public static Expression WhileElse(Expression predicate, LoopBodyBuilder whileBodyBuilder, Expression elseBody)
        {
            if(elseBody == null) throw new ArgumentNullException(nameof(elseBody));

            /*
             * This construct has the general form
             * 
             * while(condition)
             * {
             * }
             * else
             * {
             *   // If condition is false
             * }
             * 
             * If you break out of the while body then the else body does not execute
             */

            return WhileImpl(predicate, whileBodyBuilder, elseBody);
        }
    }
}
