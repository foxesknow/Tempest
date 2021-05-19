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
        /// Generates a call to a property only in the instance is not null
        /// </summary>
        /// <example>
        /// <code>
        ///     instance?.Property
        /// </code>
        /// </example>
        /// <param name="instance"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Expression ConditionalProperty(Expression instance, PropertyInfo property)
        {
            if(instance == null) throw new ArgumentNullException(nameof(instance));
            if(property == null) throw new ArgumentNullException(nameof(property));

            if(instance.Type.IsNullable() == false) throw new ArgumentNullException("instance is not a nullable type", nameof(instance));
            if(property.CanRead == false) throw new ArgumentException("property not readable", nameof(property));

            var defaultValue = property.PropertyType.IsValueType switch
            {
                true => Expression.Default(typeof(Nullable<>).MakeGenericType(property.PropertyType)),
                false => Expression.Default(property.PropertyType)
            };

            var tempName = MakeTemp("condTarget");

            if(instance.Type.IsValueType)
            {
                return Let(Expression.Parameter(instance.Type, tempName), instance, target =>
                {
                    return Expression.Condition
                    (
                        Expression.Property(target, "HasValue"),
                        ConvertIfNecessary(Expression.Property(Expression.Property(target, "Value"), property), defaultValue.Type),
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
                        Expression.ReferenceNotEqual(target, Expression.Default(instance.Type)),
                        ConvertIfNecessary(Expression.Property(target, property), defaultValue.Type),
                        defaultValue                        
                    );
                });
            }
        }
    }
}
