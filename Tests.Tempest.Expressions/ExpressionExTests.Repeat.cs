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
        public void Repeat_1()
        {
            var counter = Expression.Variable(typeof(int), "a");
            var init = Expression.Assign(counter, Expression.Constant(0));

            var repeat = ExpressionEx.Repeat(5, (b, c) => Expression.PreIncrementAssign(counter));

            var block = Expression.Block
            (
                new ParameterExpression[]{counter},
                init,
                repeat, 
                counter
            );
            var lambda = Expression.Lambda<Func<int>>(block);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(5));
        }

        [Test]
        public void Repeat_2()
        {
            var counter = Expression.Variable(typeof(int), "a");
            var init = Expression.Assign(counter, Expression.Constant(0));

            var repeat = ExpressionEx.Repeat(5, (b, c) => 
            {
                return Expression.Block
                (
                    Expression.PreIncrementAssign(counter),
                    Expression.PreIncrementAssign(counter)
                );
            });

            var block = Expression.Block
            (
                new ParameterExpression[]{counter},
                init,
                repeat, 
                counter
            );
            var lambda = Expression.Lambda<Func<int>>(block);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(10));
        }

        [Test]
        public void Repeat_break()
        {
            var counter = Expression.Variable(typeof(int), "a");
            var init = Expression.Assign(counter, Expression.Constant(0));

            var repeat = ExpressionEx.Repeat(5, (b, c) => 
            {
                return Expression.Block
                (
                    Expression.PreIncrementAssign(counter),
                    Expression.IfThen
                    (
                        Expression.GreaterThanOrEqual(counter, Expression.Constant(3)),
                        Expression.Break(b)
                    )
                );
            });

            var block = Expression.Block
            (
                new ParameterExpression[]{counter},
                init,
                repeat, 
                counter
            );
            var lambda = Expression.Lambda<Func<int>>(block);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(3));
        }

        [Test]
        public void Repeat_continue()
        {
            var counter = Expression.Variable(typeof(int), "a");
            var init = Expression.Assign(counter, Expression.Constant(0));

            var repeat = ExpressionEx.Repeat(5, (b, c) => 
            {
                return Expression.Block
                (
                    Expression.IfThen
                    (
                        Expression.GreaterThanOrEqual(counter, Expression.Constant(2)),
                        Expression.Break(b)
                    ),
                    Expression.PreIncrementAssign(counter)
                );
            });

            var block = Expression.Block
            (
                new ParameterExpression[]{counter},
                init,
                repeat, 
                counter
            );
            var lambda = Expression.Lambda<Func<int>>(block);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(2));
        }
    }
}
