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
        public void DoWhile()
        {
            var counter = Expression.Variable(typeof(int), "a");
            var init = Expression.Assign(counter, Expression.Constant(0));

            var predicate = Expression.LessThan(counter, Expression.Constant(10));

            var doWhile= ExpressionEx.DoWhile(predicate, (b, c) => 
            {
                return Expression.Block
                (
                    Expression.PreIncrementAssign(counter),
                    Expression.PreIncrementAssign(counter),
                    Expression.PreIncrementAssign(counter)
                );
            });

            var block = Expression.Block
            (
                new ParameterExpression[]{counter},
                init,
                doWhile, 
                counter
            );
            var lambda = Expression.Lambda<Func<int>>(block);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(12));
        }

        [Test]
        public void DoWhile_LoopsOnce()
        {
            var counter = Expression.Variable(typeof(int), "a");
            var init = Expression.Assign(counter, Expression.Constant(0));

            var predicate = Expression.LessThan(counter, Expression.Constant(0));

            var doWhile= ExpressionEx.DoWhile(predicate, (b, c) => 
            {
                return Expression.Block
                (
                    Expression.PreIncrementAssign(counter),
                    Expression.PreIncrementAssign(counter),
                    Expression.PreIncrementAssign(counter)
                );
            });

            var block = Expression.Block
            (
                new ParameterExpression[]{counter},
                init,
                doWhile, 
                counter
            );
            var lambda = Expression.Lambda<Func<int>>(block);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(3));
        }
    }
}
