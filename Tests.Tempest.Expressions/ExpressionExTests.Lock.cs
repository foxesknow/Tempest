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
        public void Lock()
        {
            var @lock = Expression.Constant("Hello");
            var body = Expression.Constant("Goodbye");

            var e = ExpressionEx.Lock(@lock, body);
            var lambda = Expression.Lambda<Func<string>>(e);
            var function = lambda.Compile();

            Assert.That(function(), Is.EqualTo("Goodbye"));
        }
    }
}
