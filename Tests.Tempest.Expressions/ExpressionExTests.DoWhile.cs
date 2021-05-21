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
            var body = ExpressionEx.Let(ExpressionEx.Variable<int>("counter"), ExpressionEx.Constants.Int.Int_0, counter =>
            {
                var predicate = Expression.LessThan(counter, Expression.Constant(10));

                return Expression.Block
                (
                    ExpressionEx.DoWhile(predicate, (b, c) => 
                    {
                        return Expression.Block
                        (
                            Expression.PreIncrementAssign(counter),
                            Expression.PreIncrementAssign(counter),
                            Expression.PreIncrementAssign(counter)
                        );
                    }),
                    counter
                );
            });

            var lambda = Expression.Lambda<Func<int>>(body);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(12));
        }

        [Test]
        public void DoWhile_LoopsOnce()
        {
            var body = ExpressionEx.Let(ExpressionEx.Variable<int>("counter"), ExpressionEx.Constants.Int.Int_0, counter =>
            {
                var predicate = Expression.LessThan(counter, Expression.Constant(0));

                return Expression.Block
                (
                    ExpressionEx.DoWhile(predicate, (b, c) => 
                    {
                        return Expression.Block
                        (
                            Expression.PreIncrementAssign(counter),
                            Expression.PreIncrementAssign(counter),
                            Expression.PreIncrementAssign(counter)
                        );
                    }),
                    counter
                );
            });
            
            var lambda = Expression.Lambda<Func<int>>(body);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(3));
        }
    }
}
