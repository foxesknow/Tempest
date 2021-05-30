using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Tempest.Expressions;

using NUnit.Framework;

using static Tempest.Expressions.ExpressionEx;

namespace Tests.Tempest.Expressions
{
    public partial class ExpressionExTests
    {
        [Test]
        public void Let_Add()
        {
            var add = Let(Variable<int>("a"), Expression.Constant(10), a =>
            {
                return Let(Variable<int>("b"), Expression.Constant(20), b =>
                {
                    return Expression.Add(a, b);
                });
            });

            var lambda = Expression.Lambda<Func<int>>(add);
            var function = lambda.Compile();

            Assert.That(function(), Is.EqualTo(30));
        }

        [Test]
        public void Let_To_The_Power_4()
        {
            var p = ExpressionEx.Parameter<int>();

            var lambda = Expression.Lambda<Func<int, int>>
            (
                ExpressionEx.Let(ExpressionEx.Parameter<int>(), Expression.Multiply(p, p), i => Expression.Multiply(i, i)),
                p
            );

            var function = lambda.Compile();
            Assert.That(function(10), Is.EqualTo(10 * 10 * 10 * 10));
        }
    }
}
