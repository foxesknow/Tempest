﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
    {
        /// <summary>
        /// Extracts the PropertyInfo from a lamda representing a property access
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty<TOut>(Expression<Func<TOut>> @delegate)
        {
            return DoGetProperty(@delegate);
        }

        /// <summary>
        /// Extracts the PropertyInfo from a lamda representing a property access
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty<TIn, TOut>(Expression<Func<TIn, TOut>> @delegate)
        {
            return DoGetProperty(@delegate);
        }

        private static PropertyInfo DoGetProperty<T>(Expression<T> @delegate) where T : Delegate
        {
            if(@delegate == null) throw new ArgumentNullException(nameof(@delegate));

            if(@delegate is LambdaExpression lambda && lambda.Body is MemberExpression member && member.Member is PropertyInfo property)
            {
                return property;
            }

            throw new ArgumentException("not a property call", nameof(@delegate));
        }
    }
}
