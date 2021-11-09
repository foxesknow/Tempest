using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
    {
        /// <summary>
        /// Simplifies chainng together conditional access
        /// </summary>
        /// <example>
        /// <code>
        ///     instance.Link1.Link2.LinkN
        /// </code>
        /// </example>
        /// <param name="instance"></param>
        /// <param name="links"></param>
        /// <returns></returns>
        public static Expression Chain(Expression instance, params Func<Expression, Expression>[] links)
        {
            return Chain(instance, (IEnumerable<Func<Expression, Expression>>)links);
        }

        /// <summary>
        /// Simplifies chainng together conditional access
        /// </summary>
        /// <example>
        /// <code>
        ///     instance.Link1.Link2.LinkN
        /// </code>
        /// </example>
        /// <param name="instance"></param>
        /// <param name="links"></param>
        /// <returns></returns>
        public static Expression Chain(Expression instance, IEnumerable<Func<Expression, Expression>> links)
        {
            if(instance == null) throw new ArgumentNullException(nameof(instance));
            if(links == null) throw new ArgumentNullException(nameof(links));

            var functions = links.ToReadOnlyList();
            if(functions.Count == 0) throw new ArgumentException("need at least one conditional access", nameof(links));

            var expression = instance;
            for(int i = 0; i < functions.Count; i++)
            {
                var function = functions[i];
                expression = function(expression);
            }

            return expression;
        }
    }
}
