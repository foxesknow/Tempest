using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using Tempest.Expressions;

using NUnit.Framework;

namespace Tests.Tempest.Expressions
{
    [TestFixture]
    public partial class ExpressionExTests
    {
        [Test]
        public void Foo()
        {
            DoIt(() => Console.ReadLine());
        }

        void DoIt<T>(Expression<Func<T>> function)
        {
        }
    }
}
