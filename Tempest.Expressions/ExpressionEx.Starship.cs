using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
    {
        /// <summary>
        /// An implementation of lhs <=> rhs :
        /// 
        /// <!<![CDATA[
        /// The expression evalues to:
        ///     
        ///      0  if lhs == rhs
        ///     <0  if lhs < rhs
        ///     >0  if lhs > rhs
        /// ]]>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Expression Starship(Expression lhs, Expression rhs)
        {
            if(lhs is null) throw new ArgumentNullException(nameof(lhs));
            if(rhs is null) throw new ArgumentNullException(nameof(rhs));

            if(lhs.Type != rhs.Type) throw new ArgumentException("lhs and rhs do not have the same type");

            var type = lhs.Type;
            var comparerType = typeof(Comparer<>).MakeGenericType(type);
            
            var getComparer = comparerType.GetProperty("Default")!;
            var compareMethod = getComparer.PropertyType.GetMethod("Compare")!;
            
            var expression = Let(Expression.Property(null, getComparer), comparer =>
            {
                return Expression.Call(comparer, compareMethod, lhs, rhs);
            });

            return expression;
        }
    }
}
