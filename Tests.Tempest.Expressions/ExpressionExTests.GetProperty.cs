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
            Expression<Func<int>> windowHeight = () => Console.WindowHeight;
            var property = ExpressionEx.GetProperty(windowHeight);

            Assert.That(property, Is.Not.Null);
        }

        [TestCase]
        public void GetProperty_Instance()
        {
            Expression<Func<List<int>, int>> count = list => list.Count;
            var property = ExpressionEx.GetProperty(count);

            Assert.That(property, Is.Not.Null);
        }

        [TestCase]
        public void GetProperty_Method()
        {
            Expression<Func<string>> readLine= () => Console.ReadLine();
            Assert.Catch(() => ExpressionEx.GetProperty(readLine));
        }
    }
}
