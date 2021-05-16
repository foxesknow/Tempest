using System;
using System.Linq.Expressions;
using System.Threading;
using System.Reflection;

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

        private static MethodInfo GetMethod(Type type, string name, BindingFlags bindingFlags, params Type[] parameters)
        {
            var method = type.GetMethod(name, bindingFlags, null, parameters, null);
            if(method == null) throw new ArgumentException($"could not find method {name}");

            return method;
        }

        private static Expression ConvertIfNecessary(Expression expression, Type type)
        {
            if(expression.Type == type)
            {
                return expression;
            }

            return Expression.Convert(expression, type);
        }

        public static ParameterExpression Parameter<T>()
        {
            return Expression.Parameter(typeof(T));
        }

        public static ParameterExpression Parameter<T>(string? name)
        {
            return Expression.Parameter(typeof(T), name);
        }

        public static ParameterExpression Variable<T>()
        {
            return Expression.Variable(typeof(T));
        }

        public static ParameterExpression Variable<T>(string? name)
        {
            return Expression.Variable(typeof(T), name);
        }

        public static UnaryExpression Convert<T>(Expression expression)
        {
            return Expression.Convert(expression, typeof(T));
        }

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
