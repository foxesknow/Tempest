using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

namespace Tempest.Expressions;

public static partial class ExpressionEx
{
    extension(Expression)
    {
        /// <summary>
        /// Defers running an expression until after the body has been evaluates.
        /// This is basically a reversed try/finally block.
        /// </summary>
        /// <example>
        /// <code>
        /// try
        /// {
        ///     body
        /// }
        /// finally
        /// {
        ///     defer
        /// }
        /// </code>
        /// </example>
        /// <param name="defer"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static Expression Defer(Expression defer, Expression body)
        {
            ArgumentNullException.ThrowIfNull(defer);
            ArgumentNullException.ThrowIfNull(body);

            return Expression.TryFinally(body, defer);
        }
    }
}
