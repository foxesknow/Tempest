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

    [Test]
    public void TrueValue()
    {
        var lambda = Expression.Lambda<Func<bool>>(Expression.True);
        var function = lambda.Compile();

        var value = function();
        Assert.That(value, Is.True);
    }

    [Test]
    public void FalseValue()
    {
        var lambda = Expression.Lambda<Func<bool>>(Expression.False);
        var function = lambda.Compile();

        var value = function();
        Assert.That(value, Is.False);
    }

    [Test]
    public void IsInt32_0()
    {
        var lambda = Expression.Lambda<Func<int>>(Expression.ZeroInt32);
        var function = lambda.Compile();

        var value = function();
        Assert.That(value, Is.EqualTo(0));
    }

    [Test]
    public void NullValue()
    {
        var lambda = Expression.Lambda<Func<string>>(Expression.Null<string>());
        var function = lambda.Compile();

        var value = function();
        Assert.That(value, Is.Null);
    }

    [Test]
    public void NullValue_Type()
    {
        var lambda = Expression.Lambda<Func<string>>(Expression.Null(typeof(string)));
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
