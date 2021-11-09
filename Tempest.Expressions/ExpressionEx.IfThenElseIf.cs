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
        public static (Expression Test, Expression Body) MakeIfThen(Expression test, Expression body)
        {
            return (test, body);
        }

        /// <summary>
        /// Generates an if-elseif block
        /// </summary>
        /// <example>
        /// <code>
        /// if(test-1)
        /// {
        ///     body1
        /// }
        /// else if(test-2)
        /// {
        ///     body2
        /// }
        /// else if(test-N)
        /// {
        ///     bodyN
        /// }
        /// else
        /// {
        ///     void
        /// }
        /// </code>
        /// </example>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static Expression IfThen(IEnumerable<(Expression Test, Expression Body)> conditions)
        {
            return IfThenElse(conditions, Constants.Void);
        }

        /// <summary>
        /// Generates an if-elseif block
        /// </summary>
        /// <example>
        /// <code>
        /// if(test-1)
        /// {
        ///     body1
        /// }
        /// else if(test-2)
        /// {
        ///     body2
        /// }
        /// else if(test-N)
        /// {
        ///     bodyN
        /// }
        /// else
        /// {
        ///     default
        /// }
        /// </code>
        /// </example>
        /// <param name="conditions"></param>
        /// <param name="finalElse"></param>
        /// <returns></returns>
        public static Expression IfThenElse(IEnumerable<(Expression Test, Expression Body)> conditions, Expression @default)
        {
            if(conditions == null) throw new ArgumentNullException(nameof(conditions));
            if(@default == null) throw new ArgumentNullException(nameof(@default));

            var conds = conditions as IList<(Expression Test, Expression Body)> ?? conditions.ToList();
            if(conds.Count == 0) throw new ArgumentException("no conditions", nameof(conditions));

            var tail = @default;

            for(int i = conds.Count - 1 ; i >= 0; i--)
            {
                var (test, body) = conds[i];
                tail = Expression.IfThenElse(test, body, tail);
            }

            return tail;
        }
    }
}
