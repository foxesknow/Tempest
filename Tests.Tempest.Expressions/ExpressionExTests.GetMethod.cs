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
        public void GetMethod_Static_Action()
        {
            var method = ExpressionEx.GetMethod(() => Console.WriteLine((object)null));

            Assert.That(method, Is.Not.Null);
        }

        [TestCase]
        public void GetMethod_Static_Action_Input()
        {
            var method = ExpressionEx.GetMethod((List<int> list) => list.Clear());

            Assert.That(method, Is.Not.Null);
        }

        [TestCase]
        public void GetMethod_Static_Function()
        {
            var method = ExpressionEx.GetMethod(() => Console.ReadKey(true));

            Assert.That(method, Is.Not.Null);
        }

        [TestCase]
        public void GetMethod_Property()
        {
            Assert.Catch(() => ExpressionEx.GetMethod(() => Console.WindowHeight));
        }

        [TestCase]
        public void GetMethod_Instance_Action()
        {
            var method = ExpressionEx.GetMethod((List<int> list) => list.Add(1));

            Assert.That(method, Is.Not.Null);
        }

        [TestCase]
        public void GetMethod_Instance_Function()
        {
            var method = ExpressionEx.GetMethod((List<int> list) => list.IndexOf(1));

            Assert.That(method, Is.Not.Null);
        }
    }
}
