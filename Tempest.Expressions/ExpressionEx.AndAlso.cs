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
        public static Expression AndAlso(params Expression[] expressions)
        {
            return AndAlso((IEnumerable<Expression>)expressions);
        }

        public static Expression AndAlso(IEnumerable<Expression> expressions)
        {
            if(expressions == null) throw new ArgumentNullException(nameof(expressions));

            var exprs = expressions.ToReadOnlyList();
            if(exprs.Count < 2) throw new ArgumentException("need at least 2 expressions", nameof(expressions));

            var acc = exprs[0];

            for(int i = 1; i < exprs.Count; i++)
            {
                acc = Expression.AndAlso(acc, exprs[i]);
            }

            return acc;
        }
    }
}
