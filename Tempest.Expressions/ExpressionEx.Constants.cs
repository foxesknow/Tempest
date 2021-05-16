﻿using System;
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
