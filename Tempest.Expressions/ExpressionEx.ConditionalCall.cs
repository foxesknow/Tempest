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
        /// Generates a call to a method only if the instance is not null
        /// </summary>
        /// <example>
        /// <code>
        ///     instance?.Method(p1, p2, ...)
        /// </code>
        /// </example>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Expression ConditionalCall(Expression instance, MethodInfo method, params Expression[]? arguments)
        {
            if(instance == null) throw new ArgumentNullException(nameof(instance));
            if(method == null) throw new ArgumentNullException(nameof(method));

            if(instance.Type.IsNullable() == false) throw new ArgumentException("instance is not a nullable type", nameof(instance));

            // void is a value type, so be careful...
            var returnType = method.ReturnType;
            var defaultValue = returnType.IsValueType switch
            {
                true => (returnType == typeof(void) ? Constants.Void : Expression.Default(typeof(Nullable<>).MakeGenericType(returnType))),
                false => Expression.Default(method.ReturnType)
            };
    
            var tempName = MakeTemp("condTarget");

            if(instance.Type.IsValueType)
            {
                return Let(Expression.Parameter(instance.Type, tempName), instance, target =>
                {
                    return Expression.Condition
                    (
                        Expression.Property(target, "HasValue"),
                        ConvertIfNecessary(Expression.Call(Expression.Property(target, "Value"), method, arguments), defaultValue.Type),
                        defaultValue
                    );
                });
            }
            else
            {
                return Let(Expression.Parameter(instance.Type, tempName), instance, target =>
                {
                    // NOTE: Don't use "NotEqual" as it will use any operator!= overloads, which we don't want
                    return Expression.Condition
                    (
                        Expression.ReferenceNotEqual(target, Constants.Null(instance.Type)),
                        ConvertIfNecessary(Expression.Call(target, method, arguments), defaultValue.Type),
                        defaultValue                        
                    );
                });
            }
        }
    }
}
