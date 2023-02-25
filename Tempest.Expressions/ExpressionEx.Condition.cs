using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
    {
        /// <summary>
        /// Creates an expression tat evaluates a series of tests, executing the body for the first test that evaluates to true.
        /// If not evalutes to true then the default expression is used.
        /// 
        /// This is similar to a switch expression
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Expression Condition(IEnumerable<(Expression Test, Expression Body)> conditions, Expression @default)
        {
            if(conditions == null) throw new ArgumentNullException(nameof(conditions));
            if(@default == null) throw new ArgumentNullException(nameof(@default));

            var conds = conditions.ToReadOnlyList();
            if(conds.Count == 0) throw new ArgumentException("no conditions", nameof(conditions));

            var tail = @default;

            for(int i = conds.Count -1 ; i >= 0; i--)
            {
                var ifTrue = conds[i];
                tail = Expression.Condition(ifTrue.Test, ifTrue.Body, tail);
            }

            return tail;
        }
    }
}
