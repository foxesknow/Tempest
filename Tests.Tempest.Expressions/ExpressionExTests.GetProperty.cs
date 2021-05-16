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
        [TestCase]
        public void GetProperty_Static()
        {
            var property = ExpressionEx.GetProperty(() => Console.WindowHeight);
            Assert.That(property, Is.Not.Null);
        }

        [TestCase]
        public void GetProperty_Instance()
        {
            var property = ExpressionEx.GetProperty((List<int> list) => list.Count);
            Assert.That(property, Is.Not.Null);
        }

        [TestCase]
        public void GetProperty_Method()
        {
            Assert.Catch(() => ExpressionEx.GetProperty(() => Console.ReadLine()));
        }
    }
}
