using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using Tempest.Expressions;

using NUnit.Framework;

namespace Tests.Tempest.Expressions
{
    public partial class ExpressionExTests
    {
        [Test]
        public void GetConstructor()
        {
            var constructor = ExpressionEx.GetConstructor(() => new List<int>());
            Assert.That(constructor, Is.Not.Null);
        }

        [Test]
        public void GetConstructor_NotNew()
        {
            Assert.Catch(() => ExpressionEx.GetConstructor(() => "hello"));
        }
    }
}
