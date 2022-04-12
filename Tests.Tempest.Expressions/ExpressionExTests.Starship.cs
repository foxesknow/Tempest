using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Tempest.Expressions;

using NUnit.Framework;

namespace Tests.Tempest.Expressions
{
    public partial class ExpressionExTests
    {
        [Test]
        public void Starship_ValueTypes()
        {
            var lhs = Expression.Parameter(typeof(int));
            var rhs = Expression.Parameter(typeof(int));
            var body = ExpressionEx.Starship(lhs, rhs);

            var lambda = Expression.Lambda<Func<int, int, int>>(body, lhs, rhs);
            var function = lambda.Compile();

            Assert.That(function(1, 1), Is.EqualTo(0));
            Assert.That(function(1, 0), Is.GreaterThan(0));
            Assert.That(function(0, 1), Is.LessThan(0));
        }

        [Test]
        public void Starship_ReferenceTypes()
        {
            var lhs = Expression.Parameter(typeof(string));
            var rhs = Expression.Parameter(typeof(string));
            var body = ExpressionEx.Starship(lhs, rhs);

            var lambda = Expression.Lambda<Func<string, string, int>>(body, lhs, rhs);
            var function = lambda.Compile();

            Assert.That(function("hello", "hello"), Is.EqualTo(0));
            Assert.That(function("hello", "blink"), Is.GreaterThan(0));
            Assert.That(function("hello", "trend"), Is.LessThan(0));
        }
    }
}
