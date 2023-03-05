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
        public void GetTupleItem_NotNested()
        {
            var tuple = ExpressionEx.MakeTuple(Expression.Constant(10), 
                                               Expression.Constant(20), 
                                               Expression.Constant(30), 
                                               Expression.Constant(40));

            var lookup = ExpressionEx.GetTupleItem(tuple, 3);
            var lambda = Expression.Lambda<Func<int>>(lookup);
            var function = lambda.Compile();
            var value = function();
            Assert.That(value, Is.EqualTo(30));
        }

        [Test]
        public void GetTupleItem_Nested()
        {
            var tuple = ExpressionEx.MakeTuple(Expression.Constant(10), 
                                               Expression.Constant(20), 
                                               Expression.Constant(30), 
                                               Expression.Constant(40),
                                               Expression.Constant(50),
                                               Expression.Constant(60),
                                               Expression.Constant(70),
                                               Expression.Constant(80),
                                               Expression.Constant(90),
                                               Expression.Constant(100));

            var lookup = ExpressionEx.GetTupleItem(tuple, 9);
            var lambda = Expression.Lambda<Func<int>>(lookup);
            var function = lambda.Compile();
            var value = function();
            Assert.That(value, Is.EqualTo(90));
        }

        [Test]
        public void GetTupleItem_DoubleNested()
        {
            var tuple = ExpressionEx.MakeTuple(Expression.Constant(10), 
                                               Expression.Constant(20), 
                                               Expression.Constant(30), 
                                               Expression.Constant(40),
                                               Expression.Constant(50),
                                               Expression.Constant(60),
                                               Expression.Constant(70),
                                               Expression.Constant(80),
                                               Expression.Constant(90),
                                               Expression.Constant(100),
                                               Expression.Constant(110),
                                               Expression.Constant(120),
                                               Expression.Constant(130),
                                               Expression.Constant(140),
                                               Expression.Constant(150),
                                               Expression.Constant(160),
                                               Expression.Constant(170),
                                               Expression.Constant(180));

            var lookup = ExpressionEx.GetTupleItem(tuple, 18);
            var lambda = Expression.Lambda<Func<int>>(lookup);
            var function = lambda.Compile();
            var value = function();
            Assert.That(value, Is.EqualTo(180));
        }
    }
}
