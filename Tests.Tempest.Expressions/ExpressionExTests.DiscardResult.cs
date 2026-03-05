using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Tempest.Expressions;

namespace Tests.Tempest.Expressions;

public partial class ExpressionExTests
{
    [Test]
    public void DiscardResult()
    {
        var number = Expression.DiscardResult(Expression.Constant(10));
        Assert.That(number.Type, Is.EqualTo(typeof(void)));

        var alreadyVoid = Expression.DiscardResult(Expression.Void);
        Assert.That(alreadyVoid.Type, Is.EqualTo(typeof(void)));
    }
}
