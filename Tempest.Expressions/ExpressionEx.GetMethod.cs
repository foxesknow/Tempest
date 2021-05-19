using System;
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
        /// Extracts the MethodInfo from a lambda representing a method call
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static MethodInfo GetMethod<TOut>(Expression<Func<TOut>> @delegate)
        {
            return DoGetMethod(@delegate);
        }

        /// <summary>
        /// Extracts the MethodInfo from a lambda representing a method call
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static MethodInfo GetMethod<TIn, TOut>(Expression<Func<TIn, TOut>> @delegate)
        {
            return DoGetMethod(@delegate);
        }

        /// <summary>
        /// Extracts the MethodInfo from a lambda representing a method call
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static MethodInfo GetMethod<TOut>(Expression<Action<TOut>> @delegate)
        {
            return DoGetMethod(@delegate);
        }

        /// <summary>
        /// Extracts the MethodInfo from a lambda representing a method call
        /// </summary>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static MethodInfo GetMethod(Expression<Action> @delegate)
        {
            return DoGetMethod(@delegate);
        }

        private static MethodInfo DoGetMethod<T>(Expression<T> @delegate) where T : Delegate
        {
            if(@delegate == null) throw new ArgumentNullException(nameof(@delegate));

            if(@delegate is LambdaExpression lambda && lambda.Body is MethodCallExpression methodCall)
            {
                return methodCall.Method;
            }

            throw new ArgumentException("not a method call", nameof(@delegate));
        }
    }
}
