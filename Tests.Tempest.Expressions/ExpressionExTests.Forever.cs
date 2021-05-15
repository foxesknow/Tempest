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
    [TestFixture]
    public partial class ExpressionExTests
    {
        [Test]
        public void Forever()
        {
            var variable = ExpressionEx.Variable<int>("counter");
            var forever = ExpressionEx.Forever((b, c) =>
            {
                return Expression.PostIncrementAssign(variable);
            });
        }
    }
}
