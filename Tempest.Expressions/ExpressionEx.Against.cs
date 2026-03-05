using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;

namespace Tempest.Expressions;

/// <summary>
/// Useful expression methods
/// </summary>
public static partial class ExpressionEx
{
    extension(Expression instance)
    {
        /// <summary>
        /// Applies a series of expression to an instance, returning the instance
        /// </summary>
        /// <example>
        /// <code>
        /// T Against(T t)
        /// {
        ///     t.Method(...);
        ///     t.Method(...);
        ///     t.Method(...);
        ///     
        ///     return t;
        /// }
        /// </code>
        /// </example>
        /// <param name="functions"></param>
        /// <returns></returns>
        public Expression Against(params Func<Expression, Expression>[] functions)
        {
            return Against(instance, (IEnumerable<Func<Expression, Expression>>)functions);
        }

        /// <summary>
        /// Applies a series of expression to an instance, returning the instance
        /// </summary>
        /// <example>
        /// <code>
        /// T Against(T t)
        /// {
        ///     t.Method(...);
        ///     t.Method(...);
        ///     t.Method(...);
        ///     
        ///     return t;
        /// }
        /// </code>
        /// </example>
        /// <param name="functions"></param>
        /// <returns></returns>
        public Expression Against(IEnumerable<Func<Expression, Expression>> functions)
        {
            ArgumentNullException.ThrowIfNull(functions);

            var funcs = functions.ToReadOnlyList();
            if(funcs.Count == 0) throw new ArgumentException("need at least one conditional access", nameof(functions));

            return Let(instance, target =>
            {
                var list = funcs.Select(f => f(target)).ToList();
                list.Add(target);

                return Expression.Block(list);
            });
        }
    }
}
