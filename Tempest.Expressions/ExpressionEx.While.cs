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
        public static Expression While(Expression predicate, LoopBodyBuilder bodyBuilder)
        {
            if(predicate == null) throw new ArgumentNullException(nameof(predicate));
            if(predicate.Type != typeof(bool)) throw new ArgumentException("predicate is not a boolean", nameof(predicate));

            if(bodyBuilder == null) throw new ArgumentNullException(nameof(bodyBuilder));

            var (@break, @continue) = MakeBreakAndContinueLabels();

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
                Expression.Continue(@continue),
                Expression.Label(@break)
            );
        }
    }
}
