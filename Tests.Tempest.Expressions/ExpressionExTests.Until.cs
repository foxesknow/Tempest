using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using Tempest.Expressions;

using NUnit.Framework;

namespace Tests.Tempest.Expressions
{
    public partial class ExpressionExTests
    {
        [Test]
        public void Until()
        {
            /*
             * int i = 0;
             * int count = 0;
             * 
             * until(i == 10)
             * {
             *    ++i;
             *    ++count;
             * }
             */
            var body = ExpressionEx.Let(ExpressionEx.Constants.Int.Int_0, i =>
            {
                return ExpressionEx.Let(ExpressionEx.Constants.Int.Int_0, count =>
                {
                    return Expression.Block
                    (
                        ExpressionEx.Until(Expression.Equal(i, Expression.Constant(10)), (b, c) =>
                        {
                            return Expression.Block
                            (
                                Expression.PreIncrementAssign(i),
                                Expression.PreIncrementAssign(count),
                                Expression.PreIncrementAssign(count)
                            );
                        }),
                        count
                    );
                });
            });

            var lambda = Expression.Lambda<Func<int>>(body);
            var function = lambda.Compile();
            var answer = function();

            Assert.That(answer, Is.EqualTo(20));
        }
    }
}
