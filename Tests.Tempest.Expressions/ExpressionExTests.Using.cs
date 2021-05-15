﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;

using Tempest.Expressions;

using NUnit.Framework;

namespace Tests.Tempest.Expressions
{
    public partial class ExpressionExTests
    {
        [Test]
        public void Using_Class_Implicit()
        {
            var p = ExpressionEx.Parameter<DisposableClass_Implicit>();
            var body = ExpressionEx.Using(p, Expression.Constant("hello"));
            var lambda = Expression.Lambda<Action<DisposableClass_Implicit>>(body, p);
            var action = lambda.Compile();

            var target = new DisposableClass_Implicit();
            Assert.That(target.Disposed, Is.False);

            action(target);
            Assert.That(target.Disposed, Is.True);
        }

        [Test]
        public void Using_Class_Explicit()
        {
            var p = ExpressionEx.Parameter<DisposableClass_Explicit>();
            var body = ExpressionEx.Using(p, Expression.Constant("hello"));
            var lambda = Expression.Lambda<Action<DisposableClass_Explicit>>(body, p);
            var action = lambda.Compile();

            var target = new DisposableClass_Explicit();
            Assert.That(target.Disposed, Is.False);

            action(target);
            Assert.That(target.Disposed, Is.True);
        }


        class DisposableClass_Implicit : IDisposable
        {
            public bool Disposed{get; set;}

            public void Dispose()
            {
                this.Disposed = true;
            }
        }

        class DisposableClass_Explicit : IDisposable
        {
            public bool Disposed{get; set;}

            void IDisposable.Dispose()
            {
                this.Disposed = true;
            }
        }
    }
}
