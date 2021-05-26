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
        public void IfThen()
        {
            var p = ExpressionEx.Parameter<int>("p");

            var @return = Expression.Label(typeof(string));
            var body = Expression.Block
            (
                ExpressionEx.IfThen
                (
                    new[]
                    {
                        ExpressionEx.MakeIfThen(Expression.Equal(p, Expression.Constant(0)), Expression.Return(@return, Expression.Constant("zero"))),
                        ExpressionEx.MakeIfThen(Expression.Equal(p, Expression.Constant(1)), Expression.Return(@return, Expression.Constant("one"))),
                        ExpressionEx.MakeIfThen(Expression.Equal(p, Expression.Constant(2)), Expression.Return(@return, Expression.Constant("two"))),
                        ExpressionEx.MakeIfThen(Expression.Equal(p, Expression.Constant(3)), Expression.Return(@return, Expression.Constant("three"))),
                    }
                ),
                Expression.Label(@return, Expression.Constant("nothing"))
            );

            var lambda = Expression.Lambda<Func<int, string>>(body, p);
            var function = lambda.Compile();

            Assert.That(function(0), Is.EqualTo("zero"));
            Assert.That(function(1), Is.EqualTo("one"));
            Assert.That(function(2), Is.EqualTo("two"));
            Assert.That(function(3), Is.EqualTo("three"));
            Assert.That(function(4), Is.EqualTo("nothing"));
        }

        [Test]
        public void IfThenElse()
        {
            var p = ExpressionEx.Parameter<int>("p");

            var @return = Expression.Label(typeof(string));
            var body = Expression.Block
            (
                ExpressionEx.IfThenElse
                (
                    new[]
                    {
                        ExpressionEx.MakeIfThen(Expression.Equal(p, Expression.Constant(0)), Expression.Return(@return, Expression.Constant("zero"))),
                        ExpressionEx.MakeIfThen(Expression.Equal(p, Expression.Constant(1)), Expression.Return(@return, Expression.Constant("one"))),
                        ExpressionEx.MakeIfThen(Expression.Equal(p, Expression.Constant(2)), Expression.Return(@return, Expression.Constant("two"))),
                        ExpressionEx.MakeIfThen(Expression.Equal(p, Expression.Constant(3)), Expression.Return(@return, Expression.Constant("three"))),
                    },
                    Expression.Return(@return, Expression.Constant("unknown"))
                ),
                Expression.Label(@return, Expression.Constant(null, typeof(string)))
            );

            var lambda = Expression.Lambda<Func<int, string>>(body, p);
            var function = lambda.Compile();

            Assert.That(function(0), Is.EqualTo("zero"));
            Assert.That(function(1), Is.EqualTo("one"));
            Assert.That(function(2), Is.EqualTo("two"));
            Assert.That(function(3), Is.EqualTo("three"));
            Assert.That(function(4), Is.EqualTo("unknown"));
        }
    }
}
