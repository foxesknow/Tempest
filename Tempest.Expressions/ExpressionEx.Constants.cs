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
        /// <summary>
        /// Pre defined constants
        /// </summary>
        public static class Constants
        {
            /// <summary>
            /// A void expression
            /// </summary>
            public static readonly Expression Void = Expression.Default(typeof(void));

            /// <summary>
            /// Boolean constants
            /// </summary>
            public static class Bool
            {
                /// <summary>
                /// A true constant expression
                /// </summary>
                public static readonly Expression True = Expression.Constant(true, typeof(bool));
                
                /// <summary>
                /// A false constant expression
                /// </summary>
                public static readonly Expression False = Expression.Constant(false, typeof(bool));
            }

            /// <summary>
            /// Useful integer constants
            /// </summary>
            public static class Int
            {
                /// <summary>
                /// Zero as a constant
                /// </summary>
                public static readonly Expression Int_0 = Expression.Constant(0, typeof(int));
            }

            /// <summary>
            /// Returns the zero constant for a type, if applicable
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static ConstantExpression ZeroFor(Type type)
            {
                if(type == null) throw new ArgumentNullException(nameof(type));

                if(type == typeof(byte)) return DefaultFor<byte>();
                if(type == typeof(sbyte)) return DefaultFor<sbyte>();
                if(type == typeof(short)) return DefaultFor<short>();
                if(type == typeof(ushort)) return DefaultFor<ushort>();
                if(type == typeof(int)) return DefaultFor<int>();
                if(type == typeof(uint)) return DefaultFor<uint>();
                if(type == typeof(long)) return DefaultFor<long>();
                if(type == typeof(ulong)) return DefaultFor<ulong>();
                if(type == typeof(float)) return DefaultFor<float>();
                if(type == typeof(double)) return DefaultFor<double>();
                if(type == typeof(char)) return DefaultFor<char>();

                throw new ArgumentException($"zero not available for {type.Name}", nameof(type));
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
