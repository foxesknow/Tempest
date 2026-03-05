using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Tempest.Expressions;

namespace Tests.Tempest.Expressions;

using NUnit.Framework;


public partial class ExpressionExTests
{
    [Test]
    public void DefaultFor_Struct()
    {
        var body = Expression.Default<DefaultForStruct>();
        var lambda = Expression.Lambda<Func<DefaultForStruct>>(body);
        var function = lambda.Compile();

        var value = function();
        Assert.That(value.ToString(), Is.EqualTo("Struct"));
    }

    [Test]
    public void DefaultFor_Class()
    {
        var body = Expression.Default<DefaultForClass>();
        var lambda = Expression.Lambda<Func<DefaultForClass>>(body);
        var function = lambda.Compile();

        var value = function();
        Assert.That(value, Is.Null);
    }

    private class DefaultForClass
    { 
    }

    private struct DefaultForStruct
    {
        public override string ToString()
        {
            return "Struct";
        }
    }
}
