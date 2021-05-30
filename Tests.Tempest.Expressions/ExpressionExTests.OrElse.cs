﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Tempest.Expressions;

using NUnit.Framework;

namespace Tests.Tempest.Expressions
{
    public partial class ExpressionExTests
    {
        [Test]
        public void OrElse()
        {
            Assert.That(Make(true, false), Is.EqualTo(true));
            Assert.That(Make(false, false), Is.EqualTo(false));
            Assert.That(Make(false, true), Is.EqualTo(true));
            Assert.That(Make(true, true), Is.EqualTo(true));
            Assert.That(Make(true, true, true, true, true, true, false), Is.EqualTo(true));

            static bool Make(params bool[] values)
            {
                var parts = values.Select(b => Expression.Constant(b));
                var body = ExpressionEx.OrElse(parts);
                var lambda = Expression.Lambda<Func<bool>>(body);

                var function = lambda.Compile();
                return function();
            }
        }

        [Test]
        public void OrElse_Not_Enough_Conditions()
        {
            Assert.Catch(() => ExpressionEx.OrElse(Expression.Constant(true)));
            Assert.Catch(() => ExpressionEx.OrElse());
        }
    }
}
