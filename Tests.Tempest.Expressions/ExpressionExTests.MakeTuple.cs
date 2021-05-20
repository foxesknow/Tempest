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
        public void MakeTuple_1()
        {
            var tuple = ExpressionEx.MakeTuple(Expression.Constant(10));
            var lambda = Expression.Lambda<Func<ValueTuple<int>>>(tuple);
            var function = lambda.Compile();

            var t = function();
            Assert.That(t.Item1, Is.EqualTo(10));
        }

        [Test]
        public void MakeTuple_2()
        {
            var tuple = ExpressionEx.MakeTuple
            (
                Expression.Constant(10),
                Expression.Constant(12)
            );
            var lambda = Expression.Lambda<Func<ValueTuple<int, int>>>(tuple);
            var function = lambda.Compile();

            var t = function();
            Assert.That(t.Item1, Is.EqualTo(10));
            Assert.That(t.Item2, Is.EqualTo(12));
        }

        [Test]
        public void MakeTuple_3()
        {
            var tuple = ExpressionEx.MakeTuple
            (
                Expression.Constant(10),
                Expression.Constant(12),
                Expression.Constant(14)
            );
            var lambda = Expression.Lambda<Func<ValueTuple<int, int, int>>>(tuple);
            var function = lambda.Compile();

            var t = function();
            Assert.That(t.Item1, Is.EqualTo(10));
            Assert.That(t.Item2, Is.EqualTo(12));
            Assert.That(t.Item3, Is.EqualTo(14));
        }

        [Test]
        public void MakeTuple_4()
        {
            var tuple = ExpressionEx.MakeTuple
            (
                Expression.Constant(10),
                Expression.Constant(12),
                Expression.Constant(14),
                Expression.Constant(16)
            );
            var lambda = Expression.Lambda<Func<ValueTuple<int, int, int, int>>>(tuple);
            var function = lambda.Compile();

            var t = function();
            Assert.That(t.Item1, Is.EqualTo(10));
            Assert.That(t.Item2, Is.EqualTo(12));
            Assert.That(t.Item3, Is.EqualTo(14));
            Assert.That(t.Item4, Is.EqualTo(16));
        }

        [Test]
        public void MakeTuple_5()
        {
            var tuple = ExpressionEx.MakeTuple
            (
                Expression.Constant(10),
                Expression.Constant(12),
                Expression.Constant(14),
                Expression.Constant(16),
                Expression.Constant(18)
            );
            var lambda = Expression.Lambda<Func<ValueTuple<int, int, int, int, int>>>(tuple);
            var function = lambda.Compile();

            var t = function();
            Assert.That(t.Item1, Is.EqualTo(10));
            Assert.That(t.Item2, Is.EqualTo(12));
            Assert.That(t.Item3, Is.EqualTo(14));
            Assert.That(t.Item4, Is.EqualTo(16));
            Assert.That(t.Item5, Is.EqualTo(18));
        }

        [Test]
        public void MakeTuple_6()
        {
            var tuple = ExpressionEx.MakeTuple
            (
                Expression.Constant(10),
                Expression.Constant(12),
                Expression.Constant(14),
                Expression.Constant(16),
                Expression.Constant(18),
                Expression.Constant(20)
            );
            var lambda = Expression.Lambda<Func<ValueTuple<int, int, int, int, int, int>>>(tuple);
            var function = lambda.Compile();

            var t = function();
            Assert.That(t.Item1, Is.EqualTo(10));
            Assert.That(t.Item2, Is.EqualTo(12));
            Assert.That(t.Item3, Is.EqualTo(14));
            Assert.That(t.Item4, Is.EqualTo(16));
            Assert.That(t.Item5, Is.EqualTo(18));
            Assert.That(t.Item6, Is.EqualTo(20));
        }

        [Test]
        public void MakeTuple_7()
        {
            var tuple = ExpressionEx.MakeTuple
            (
                Expression.Constant(10),
                Expression.Constant(12),
                Expression.Constant(14),
                Expression.Constant(16),
                Expression.Constant(18),
                Expression.Constant(20),
                Expression.Constant(22)
            );
            var lambda = Expression.Lambda<Func<ValueTuple<int, int, int, int, int, int, int>>>(tuple);
            var function = lambda.Compile();

            var t = function();
            Assert.That(t.Item1, Is.EqualTo(10));
            Assert.That(t.Item2, Is.EqualTo(12));
            Assert.That(t.Item3, Is.EqualTo(14));
            Assert.That(t.Item4, Is.EqualTo(16));
            Assert.That(t.Item5, Is.EqualTo(18));
            Assert.That(t.Item6, Is.EqualTo(20));
            Assert.That(t.Item7, Is.EqualTo(22));
        }

        [Test]
        public void MakeTuple_Chain_1()
        {
            var tuple = ExpressionEx.MakeTuple
            (
                Expression.Constant(10),
                Expression.Constant(12),
                Expression.Constant(14),
                Expression.Constant(16),
                Expression.Constant(18),
                Expression.Constant(20),
                Expression.Constant(22),
                Expression.Constant("Rod"),
                Expression.Constant("Jane"),
                Expression.Constant("Freddy")
            );
            var lambda = Expression.Lambda<Func<ValueTuple<int, int, int, int, int, int, int, ValueTuple<string, string, string>>>>(tuple);
            var function = lambda.Compile();

            var t = function();
            Assert.That(t.Item1, Is.EqualTo(10));
            Assert.That(t.Item2, Is.EqualTo(12));
            Assert.That(t.Item3, Is.EqualTo(14));
            Assert.That(t.Item4, Is.EqualTo(16));
            Assert.That(t.Item5, Is.EqualTo(18));
            Assert.That(t.Item6, Is.EqualTo(20));
            Assert.That(t.Item7, Is.EqualTo(22));
            Assert.That(t.Item8, Is.EqualTo("Rod"));
            Assert.That(t.Item9, Is.EqualTo("Jane"));
            Assert.That(t.Item10, Is.EqualTo("Freddy"));
        }

        [Test]
        public void MakeTuple_Chain_2()
        {
            var tuple = ExpressionEx.MakeTuple
            (
                Expression.Constant(10),
                Expression.Constant(12),
                Expression.Constant(14),
                Expression.Constant(16),
                Expression.Constant(18),
                Expression.Constant(20),
                Expression.Constant(22),
                Expression.Constant("Rod"),
                Expression.Constant("Jane"),
                Expression.Constant("Freddy"),
                Expression.Constant("Rita"),
                Expression.Constant("Sue"),
                Expression.Constant("Bob"),
                Expression.Constant("Jack"),
                Expression.Constant("Sawyer"),
                Expression.Constant("Ben")
            );
            var lambda = Expression.Lambda<Func<ValueTuple<int, int, int, int, int, int, int, ValueTuple<string, string, string, string, string, string, string, ValueTuple<string, string>>>>>(tuple);
            var function = lambda.Compile();

            var t = function();
            Assert.That(t.Item1, Is.EqualTo(10));
            Assert.That(t.Item2, Is.EqualTo(12));
            Assert.That(t.Item3, Is.EqualTo(14));
            Assert.That(t.Item4, Is.EqualTo(16));
            Assert.That(t.Item5, Is.EqualTo(18));
            Assert.That(t.Item6, Is.EqualTo(20));
            Assert.That(t.Item7, Is.EqualTo(22));
            Assert.That(t.Item8, Is.EqualTo("Rod"));
            Assert.That(t.Item9, Is.EqualTo("Jane"));
            Assert.That(t.Item10, Is.EqualTo("Freddy"));
            Assert.That(t.Item11, Is.EqualTo("Rita"));
            Assert.That(t.Item12, Is.EqualTo("Sue"));
            Assert.That(t.Item13, Is.EqualTo("Bob"));
            Assert.That(t.Item14, Is.EqualTo("Jack"));
            Assert.That(t.Item15, Is.EqualTo("Sawyer"));
            Assert.That(t.Item16, Is.EqualTo("Ben"));
        }
    }
}
