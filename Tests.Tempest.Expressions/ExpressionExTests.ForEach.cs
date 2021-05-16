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
        public void ForEach()
        {
            var parameter = ExpressionEx.Parameter<List<string>>();

            Expression<Action> consoleWriteLine = () => Console.WriteLine((object)null);
            var writeLine = ExpressionEx.GetMethod(consoleWriteLine);

            var body = ExpressionEx.ForEach
            (
                parameter,
                (v, b, c) => Expression.Call(null, writeLine, v)
            );

            var lambda = Expression.Lambda<Action<List<string>>>(body, parameter);
            var action = lambda.Compile();

            var sequence = new List<string>{"Rod", "Jane", "Freddy"};
            action(sequence);
        }
    }
}
