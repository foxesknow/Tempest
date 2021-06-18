using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Tempest.Expressions;

using NUnit.Framework;

namespace Tests.Tempest.Expressions
{
    public partial class ExpressionExTests
    {
        [Test]
        public void WhileElse_AlwaysFalse()
        {
            /*
             * while(false)
             * {
             *   p = 99;
             * }
             * else
             * {
             *   p = 100;
             * }
             * 
             * 
             */
            var block = ExpressionEx.Let(ExpressionEx.Parameter<int>(), ExpressionEx.Constants.Int.Int_0, p =>
            {
                return Expression.Block
                (
                    ExpressionEx.WhileElse
                    (
                        ExpressionEx.Constants.Bool.False,
                        (b, c) => Expression.Assign(p, Expression.Constant(99)),
                        Expression.Assign(p, Expression.Constant(100))
                    ),
                    p
                );
            });

            var lambda = Expression.Lambda<Func<int>>(block);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(100));
        }

        [Test]
        public void WhileElse_Break_In_Body()
        {
            /*
             * while(true)
             * {
             *   p = 99;
             *   break;
             * }
             * else
             * {
             *   p = 100;
             * }
             * 
             * 
             */
            var block = ExpressionEx.Let(ExpressionEx.Parameter<int>(), ExpressionEx.Constants.Int.Int_0, p =>
            {
                return Expression.Block
                (
                    ExpressionEx.WhileElse
                    (
                        ExpressionEx.Constants.Bool.True,
                        (b, c) => Expression.Block
                        (
                            Expression.Assign(p, Expression.Constant(99)),
                            Expression.Break(b)
                        ),
                        Expression.Assign(p, Expression.Constant(100))
                    ),
                    p
                );
            });

            var lambda = Expression.Lambda<Func<int>>(block);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(99));
        }

        [Test]
        public void WhileElse()
        {
            /*
             * int a  = 0;
             * while(a < 10)
             * {
             *   a++;
             *   a++;
             *   a++;
             * }
             * else
             * {
             *   a = a + 10;
             * }
             * 
             * return a;
             * 
             */

            var counter = Expression.Variable(typeof(int), "a");
            var init = Expression.Assign(counter, Expression.Constant(0));

            var predicate = Expression.LessThan(counter, Expression.Constant(10));

            var whileElse = ExpressionEx.WhileElse
            (
                predicate, 
                (b, c) => 
                {
                    return Expression.Block
                    (
                        Expression.PreIncrementAssign(counter),
                        Expression.PreIncrementAssign(counter),
                        Expression.PreIncrementAssign(counter)
                    );
                },
                Expression.AddAssign(counter, Expression.Constant(10))
            );

            var block = Expression.Block
            (
                new ParameterExpression[]{counter},
                init,
                whileElse,
                counter
            );
            var lambda = Expression.Lambda<Func<int>>(block);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(22));
        }
    }
}
