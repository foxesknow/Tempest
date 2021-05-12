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
        public void While()
        {
            var counter = Expression.Variable(typeof(int), "a");
            var init = Expression.Assign(counter, Expression.Constant(0));

            var predicate = Expression.LessThan(counter, Expression.Constant(10));

            var @while = ExpressionEx.While(predicate, (b, c) => 
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
                @while, 
                counter
            );
            var lambda = Expression.Lambda<Func<int>>(block);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(12));
        }
    }
}
