using System;
using System.Linq.Expressions;
using System.Threading;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
    {
        private static long s_BreakAndContinue;
        private static long s_Label;

        private static (LabelTarget Break, LabelTarget Continue) MakeBreakAndContinueLabels()
        {
            var id = Interlocked.Increment(ref s_BreakAndContinue);

            return
            (
                Expression.Label($"break-{id}"),
                Expression.Label($"continue-{id}")
            );
        }

        private static LabelTarget MakeLabel(string name)
        {
            var id = Interlocked.Increment(ref s_Label);
            return Expression.Label($"{name}-{id}");
        }
    }
}
