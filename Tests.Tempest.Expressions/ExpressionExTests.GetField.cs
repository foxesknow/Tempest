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
        public void GetField_Static()
        {
            var field = ExpressionEx.GetField(() => FieldTest.StaticField);
            Assert.That(field, Is.Not.Null);
        }

        [Test]
        public void GetField_Instance()
        {
            var field = ExpressionEx.GetField((FieldTest f) => f.InstanceField);
            Assert.That(field, Is.Not.Null);
        }

        [Test]
        public void GetField_Property()
        {
            Assert.Catch(() => ExpressionEx.GetField(() => Console.WindowHeight));
        }

        class FieldTest
        {
            public static int StaticField = 0;
            public int InstanceField = 1;
        }
    }
}
