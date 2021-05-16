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
            Expression<Action> writeLine = () => Console.WriteLine((object)null);
            var method = ExpressionEx.GetMethod(writeLine);

            Assert.That(method, Is.Not.Null);
        }

        [TestCase]
        public void GetMethod_Static_Function()
        {
            Expression<Func<ConsoleKeyInfo>> readKey = () => Console.ReadKey(true);
            var method = ExpressionEx.GetMethod(readKey);

            Assert.That(readKey, Is.Not.Null);
        }

        [TestCase]
        public void GetMethod_Property()
        {
            Expression<Func<int>> windowHeight = () => Console.WindowHeight;
            Assert.Catch(() => ExpressionEx.GetMethod(windowHeight));
        }

        [TestCase]
        public void GetMethod_Instance_Action()
        {
            Expression<Action<List<int>>> add = list => list.Add(1);
            var method = ExpressionEx.GetMethod(add);

            Assert.That(method, Is.Not.Null);
        }

        [TestCase]
        public void GetMethod_Instance_Function()
        {
            Expression<Func<List<int>, int>> add = list => list.IndexOf(1);
            var method = ExpressionEx.GetMethod(add);

            Assert.That(method, Is.Not.Null);
        }
    }
}
