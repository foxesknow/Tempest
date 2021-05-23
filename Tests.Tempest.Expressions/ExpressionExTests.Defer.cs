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
        public void Defer()
        {
            var consoleWriteLine = ExpressionEx.GetMethod(() => Console.WriteLine((string)null));

            var body = ExpressionEx.Defer
            (
                Expression.Call(null, consoleWriteLine, Expression.Constant("Last")),
                Expression.Block
                (
                    Expression.Call(null, consoleWriteLine, Expression.Constant("First")),
                    Expression.Constant(7)
                )
            );

            var lambda = Expression.Lambda<Func<int>>(body);
            var func = lambda.Compile();
            var answer = func();

            Assert.That(answer, Is.EqualTo(7));
        }
    }
}
