using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Tempest.Expressions;

public static partial class ExpressionEx
{
    extension(Expression)
    {
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
    }
}
