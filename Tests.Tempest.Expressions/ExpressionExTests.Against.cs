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
        public void Against()
        {
            var ctor = ExpressionEx.GetConstructor(() => new List<int>());
            var add = ExpressionEx.GetMethod((List<int> list) => list.Add(0));

            var body = ExpressionEx.Against
            (
                Expression.New(ctor),
                expr => Expression.Call(expr, add, Expression.Constant(5)),
                expr => Expression.Call(expr, add, Expression.Constant(10)),
                expr => Expression.Call(expr, add, Expression.Constant(15)),
                expr => Expression.Call(expr, add, Expression.Constant(20))
            );

            var lambda = Expression.Lambda<Func<List<int>>>(body);
            var function = lambda.Compile();

            var list = function();
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0], Is.EqualTo(5));
            Assert.That(list[1], Is.EqualTo(10));
            Assert.That(list[2], Is.EqualTo(15));
            Assert.That(list[3], Is.EqualTo(20));
        }
    }
}
