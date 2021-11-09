using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Tempest.Expressions;

using NUnit.Framework;
using System.Collections;

namespace Tests.Tempest.Expressions
{
    public partial class ExpressionExTests
    {
        [Test]
        public void For()
        {
            /*
             * int function(int p)
             * {
             *     for(int i = 0; i < 5; i++)
             *     {
             *         p += 100;
             *     }
             *     
             *     return p;
             * }
             */
            var p = ExpressionEx.Parameter<int>();

            var body = ExpressionEx.Let(Expression.Constant(0), i =>
            {
                return ExpressionEx.For(Expression.LessThan(i, Expression.Constant(5)), Expression.PostIncrementAssign(i), (b, c) =>
                {
                    return Expression.AddAssign(p, Expression.Constant(100));
                });
            });

            var lambda = Expression.Lambda<Func<int, int>>
            (
                Expression.Block
                (
                    body,
                    p
                ),
                p
            );

            var function = lambda.Compile();
            Assert.That(function(0), Is.EqualTo(500));
        }

        [Test]
        public void For_Break()
        {
            /*
             * int function(int p)
             * {
             *     for(int i = 0; i < 5; i++)
             *     {
             *         if(i == 2) break;
             *         p += 100;
             *     }
             *     
             *     return p;
             * }
             */
            var p = ExpressionEx.Parameter<int>();

            var body = ExpressionEx.Let(Expression.Constant(0), i =>
            {
                return ExpressionEx.For(Expression.LessThan(i, Expression.Constant(5)), Expression.PostIncrementAssign(i), (b, c) =>
                {
                    return Expression.IfThenElse
                    (
                        Expression.Equal(i, Expression.Constant(2)),
                        Expression.Break(b),
                        Expression.AddAssign(p, Expression.Constant(100))
                    );
                });
            });

            var lambda = Expression.Lambda<Func<int, int>>
            (
                Expression.Block
                (
                    body,
                    p
                ),
                p
            );

            var function = lambda.Compile();
            Assert.That(function(0), Is.EqualTo(200));
        }

        [Test]
        public void For_Continue()
        {
            /*
             * int function(int p)
             * {
             *     for(int i = 0; i < 5; i++)
             *     {
             *         if(i == 2) continue;
             *         p += 100;
             *     }
             *     
             *     return p;
             * }
             */
            var p = ExpressionEx.Parameter<int>();

            var body = ExpressionEx.Let(Expression.Constant(0), i =>
            {
                return ExpressionEx.For(Expression.LessThan(i, Expression.Constant(5)), Expression.PostIncrementAssign(i), (b, c) =>
                {
                    return Expression.IfThenElse
                    (
                        Expression.Equal(i, Expression.Constant(2)),
                        Expression.Continue(c),
                        Expression.AddAssign(p, Expression.Constant(100))
                    );
                });
            });

            var lambda = Expression.Lambda<Func<int, int>>
            (
                Expression.Block
                (
                    body,
                    p
                ),
                p
            );

            var function = lambda.Compile();
            Assert.That(function(0), Is.EqualTo(400));
        }
    }
}
