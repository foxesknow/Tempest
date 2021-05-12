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
        public static Expression Forever(LoopBodyBuilder bodyBuilder)
        {
            if(bodyBuilder == null) throw new ArgumentNullException(nameof(bodyBuilder));

            var (@break, @continue) = MakeBreakAndContinueLabels();
            var body = bodyBuilder(@break, @continue);
            var loop = Expression.Loop(body, @break);

            return Expression.Block
            (
                Expression.Label(@continue),
                loop
            );
        }
    }
}
