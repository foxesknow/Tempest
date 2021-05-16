using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace Tempest.Expressions
{
    public partial class ExpressionEx
    {
        private static readonly MethodInfo s_DisposableDispose = GetMethod(typeof(IDisposable), "Dispose", BindingFlags.Public | BindingFlags.Instance);

        /// <summary>
        /// Generates a using block
        /// </summary>
        /// <example>
        /// <code>
        /// using(@using)
        /// {
        ///     // body
        /// }
        /// </code>
        /// </example>
        /// <param name="using"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static Expression Using(Expression @using, UsingBuilder bodyBuilder)
        {
            if(@using == null) throw new ArgumentNullException(nameof(@using)); 
            if(bodyBuilder == null) throw new ArgumentNullException(nameof(bodyBuilder)); 

            if(@using.Type.IsValueType)
            {
                return UsingStruct(@using, bodyBuilder);
            }
            else
            {
                return UsingClass(@using, bodyBuilder);
            }
        }

        private static Expression UsingClass(Expression @using, UsingBuilder bodyBuilder)
        {
            if(typeof(IDisposable).IsAssignableFrom(@using.Type) == false) throw new ArgumentException("using expression is not disposable", nameof(@using));

            var usingVariable = Expression.Variable(@using.Type, "using");
            var usingInit = Expression.Assign(usingVariable, @using);

            var tryFinally = Expression.TryFinally
            (
                bodyBuilder(usingVariable),
                Expression.IfThen
                (
                    Expression.NotEqual(usingVariable, Expression.Constant(null, @using.Type)),
                    Expression.Call(Convert<IDisposable>(usingVariable), s_DisposableDispose)
                )
            );

            var block = Expression.Block
            (
                new[]{usingVariable},
                usingInit,
                tryFinally
            );

            return block;
        }

        private static Expression UsingStruct(Expression @using, UsingBuilder bodyBuilder)
        {
            if(typeof(IDisposable).IsAssignableFrom(@using.Type) == false) throw new ArgumentException("using expression is not disposable", nameof(@using));

            var usingVariable = Expression.Variable(@using.Type, "using");
            var usingInit = Expression.Assign(usingVariable, @using);

            Expression? tryFinally = null;

            var directDisposeMethod = @using.Type.GetMethod("Dispose", BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);

            if(directDisposeMethod == null)
            {
                // The Dispose() method is explicitly implemented
                tryFinally = Expression.TryFinally
                (
                    bodyBuilder(usingVariable),
                    Expression.Call
                    (
                        Expression.Convert(usingVariable, typeof(IDisposable)),
                        s_DisposableDispose
                    )
                );
            }
            else
            {
                tryFinally = Expression.TryFinally
                (
                    bodyBuilder(usingVariable),
                    Expression.Call(usingVariable, directDisposeMethod)
                );
            }
            
            var block = Expression.Block
            (
                new[]{usingVariable},
                usingInit,
                tryFinally
            );

            return block;
        }
    }
}
