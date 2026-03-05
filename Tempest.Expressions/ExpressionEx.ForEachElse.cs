using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Tempest.Expressions;

public static partial class ExpressionEx
{
    extension(Expression)
    {
        /// <summary>
        /// Creates a foreach/else expression that enumerates over a sequence
        /// </summary>
        /// <example>
        /// <code>
        /// foreach(some-var in sequence)
        /// {
        ///     body
        /// }
        /// else
        /// {
        ///     // Executed unless the body was exited via break
        /// }
        /// </code>
        /// </example>
        /// <param name="sequence"></param>
        /// <param name="bodyBuilder"></param>
        /// <param name="elseBody"></param>
        /// <returns></returns>
        public static Expression ForEachElse(Expression sequence, LetLoopBodyBuilder bodyBuilder, Expression elseBody)
        {
            ArgumentNullException.ThrowIfNull(sequence);
            ArgumentNullException.ThrowIfNull(bodyBuilder);
            ArgumentNullException.ThrowIfNull(elseBody);

            var whileBuilder = MakeWhileBuilder(elseBody);

            if(TryForEachFromPattern(sequence, bodyBuilder, whileBuilder, out var forEach))
            {
                return forEach;
            }

            return ForEachFromIEnumerable(sequence, bodyBuilder, whileBuilder);
        }
    }
}
