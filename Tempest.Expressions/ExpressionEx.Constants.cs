using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
    {
        public static class Constants
        {
            public static class Bool
            {
                public static readonly Expression True = Expression.Constant(true, typeof(bool));
                public static readonly Expression False = Expression.Constant(false, typeof(bool));
            }

            public static class Int
            {
                public static readonly Expression Int_0 = Expression.Constant(0, typeof(int));
            }

            /// <summary>
            /// Returns the null value for a given reference type
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static ConstantExpression Null<T>() where T : class
            {
                return DefaultForFactory<T>.Value;
            }

            /// <summary>
            /// Returns the default value for a given type
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static ConstantExpression DefaultFor<T>()
            {
                return DefaultForFactory<T>.Value;
            }

            static class DefaultForFactory<T>
            {
                public static readonly ConstantExpression Value = Expression.Constant(default(T), typeof(T));
            }
        }
    }
}
