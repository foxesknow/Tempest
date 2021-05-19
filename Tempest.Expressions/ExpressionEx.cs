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
    }
}
