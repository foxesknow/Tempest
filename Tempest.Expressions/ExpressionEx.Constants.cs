using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Tempest.Expressions;

public static partial class ExpressionEx
{
    private static readonly Expression s_VoidExpression = Expression.Default(typeof(void));

    private static readonly ConstantExpression s_TrueExpression = Expression.Constant(true, typeof(bool));
            
    private static readonly ConstantExpression s_FalseExpression = Expression.Constant(false, typeof(bool));

    private static readonly ConstantExpression s_ZeroInt32 = Expression.Constant(0, typeof(int));

    private static class NullForFactory<T> where T : class
    {
        public static readonly ConstantExpression Value = Expression.Constant(null, typeof(T));
    }

    extension(Expression)
    {
        /// <summary>
        /// A void expression
        /// </summary>
        public static Expression Void => s_VoidExpression;
        
        /// <summary>
        /// An expression that evaluates to true
        /// </summary>
        public static ConstantExpression True => s_TrueExpression;
        

        /// <summary>
        /// An expression that evaluates to false
        /// </summary>
        public static ConstantExpression False => s_FalseExpression;
        
        /// <summary>
        /// An expression that evalues to int(0)
        /// </summary>
        public static ConstantExpression ZeroInt32 => s_ZeroInt32;

        /// <summary>
        /// Returns the default value for a given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DefaultExpression Default<T>()
        {
            return Expression.Default(typeof(T));
        }   

        /// <summary>
        /// Returns the null value for a given reference type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ConstantExpression Null<T>() where T : class
        {
            return NullForFactory<T>.Value;
        }

        /// <summary>
        /// Returns the null value for a given reference type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ConstantExpression Null(Type type)
        {
            if(type == null) throw new ArgumentNullException(nameof(type));
            if(type.IsValueType) throw new ArgumentException("type is a value type", nameof(type));

            return Expression.Constant(null, type);
        }

             
    }
}
