﻿using System;
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
        /// <param name="instance"></param>
        /// <param name="functions"></param>
        /// <returns></returns>
        public static Expression Against(Expression instance, params Func<Expression, Expression>[] functions)
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
        /// <param name="instance"></param>
        /// <param name="functions"></param>
        /// <returns></returns>
        public static Expression Against(Expression instance, IEnumerable<Func<Expression, Expression>> functions)
        {
            if(instance == null) throw new ArgumentNullException(nameof(instance));
            if(functions == null) throw new ArgumentNullException(nameof(functions));

            var funcs = functions as ICollection<Func<Expression, Expression>> ?? functions.ToList();
            if(funcs.Count == 0) throw new ArgumentException("need at least one conditional access", nameof(functions));

            return Let(Expression.Parameter(instance.Type), instance, target =>
            {
                var list = funcs.Select(f => f(target)).ToList();
                list.Add(target);

                return Expression.Block(list);
            });
        }
    }
}
