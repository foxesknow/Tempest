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

        public static Expression Using(Expression @using, Expression body)
        {
            if(@using == null) throw new ArgumentNullException(nameof(@using)); 
            if(body == null) throw new ArgumentNullException(nameof(body)); 

            if(@using.Type.IsValueType)
            {
                return UsingStruct(@using, body);
            }
            else
            {
                return UsingClass(@using, body);
            }
        }

        private static Expression UsingClass(Expression @using, Expression body)
        {
            if(typeof(IDisposable).IsAssignableFrom(@using.Type) == false) throw new ArgumentException("using expression is not disposable", nameof(@using));

            var usingVariable = Variable<IDisposable>("using");
            var usingInit = Expression.Assign(usingVariable, @using);

            var tryFinally = Expression.TryFinally
            (
                Expression.Block
                (
                    body
                ),
                Expression.Block
                (
                    Expression.IfThen
                    (
                        Expression.NotEqual(usingVariable, Constants.Null<IDisposable>()),
                        Expression.Call(usingVariable, s_DisposableDispose)
                    )
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

        private static Expression UsingStruct(Expression @using, Expression body)
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
                    body,
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
                    body,
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
