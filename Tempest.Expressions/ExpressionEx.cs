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

        private static long s_Temp;

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

        private static string MakeTemp(string name)
        {
            var id = Interlocked.Increment(ref s_Temp);
            return $"{name}-{id}";
        }

        private static Expression ConvertIfNecessary(Expression expression, Type type)
        {
            if(expression.Type == type)
            {
                return expression;
            }

            return Expression.Convert(expression, type);
        }

        private static Type GetTupleDefinition(int arity)
        {
            return arity switch
            {
                1 => typeof(ValueTuple<>),
                2 => typeof(ValueTuple<,>),
                3 => typeof(ValueTuple<,,>),
                4 => typeof(ValueTuple<,,,>),
                5 => typeof(ValueTuple<,,,,>),
                6 => typeof(ValueTuple<,,,,,>),
                7 => typeof(ValueTuple<,,,,,,>),
                8 => typeof(ValueTuple<,,,,,,,>),
                _ => throw new ArgumentException($"invalid arity : {arity}", nameof(arity))
            };
        }

        public static bool IsTupleType(Type type)
        {
            if(type.IsGenericType == false) return false;

            var genericType = type.GetGenericTypeDefinition();

            return genericType == typeof(ValueTuple<>) ||
                   genericType == typeof(ValueTuple<,>) ||
                   genericType == typeof(ValueTuple<,,>) ||
                   genericType == typeof(ValueTuple<,,,>) ||
                   genericType == typeof(ValueTuple<,,,,>) ||
                   genericType == typeof(ValueTuple<,,,,,>) ||
                   genericType == typeof(ValueTuple<,,,,,,>) ||
                   genericType == typeof(ValueTuple<,,,,,,,>);
        }

        /// <summary>
        /// Returns a parameter for the spcified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ParameterExpression Parameter<T>()
        {
            return Expression.Parameter(typeof(T));
        }

        /// <summary>
        /// Returns a parameter for the spcified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ParameterExpression Parameter<T>(string? name)
        {
            return Expression.Parameter(typeof(T), name);
        }

        /// <summary>
        /// Returns a variable for the spcified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ParameterExpression Variable<T>()
        {
            return Expression.Variable(typeof(T));
        }

        /// <summary>
        /// Returns a variable for the spcified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ParameterExpression Variable<T>(string? name)
        {
            return Expression.Variable(typeof(T), name);
        }

        /// <summary>
        /// Returns an expression that converts to a specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static UnaryExpression Convert<T>(Expression expression)
        {
            return Expression.Convert(expression, typeof(T));
        }
    }
}
