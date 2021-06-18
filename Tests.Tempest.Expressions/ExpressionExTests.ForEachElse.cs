using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Tempest.Expressions;

using NUnit.Framework;
using System.Collections;

namespace Tests.Tempest.Expressions
{
    public partial class ExpressionExTests
    {
        [Test]
        public void ForEachElse()
        {
            var parameter = ExpressionEx.Parameter<List<string>>();

            Expression<Action> consoleWriteLine = () => Console.WriteLine((object)null);
            var writeLine = ExpressionEx.GetMethod(consoleWriteLine);

            var body = ExpressionEx.ForEachElse
            (
                parameter,
                (v, b, c) => Expression.Call(null, writeLine, v),
                Expression.Call(null, writeLine, Expression.Constant("hello"))
            );

            var lambda = Expression.Lambda<Action<List<string>>>(body, parameter);
            var action = lambda.Compile();

            var sequence = new List<string>{"Rod", "Jane", "Freddy"};
            action(sequence);
        }

        [Test]
        public void ForEachElse_Break()
        {
            var parameter = ExpressionEx.Parameter<List<string>>();

            Expression<Action> consoleWriteLine = () => Console.WriteLine((object)null);
            var writeLine = ExpressionEx.GetMethod(consoleWriteLine);

            var body = ExpressionEx.ForEachElse
            (
                parameter,
                (v, b, c) => Expression.Block
                (
                    Expression.Call(null, writeLine, v),
                    Expression.Break(b)
                ),
                Expression.Call(null, writeLine, Expression.Constant("hello")) // This won't be "printed"
            );

            var lambda = Expression.Lambda<Action<List<string>>>(body, parameter);
            var action = lambda.Compile();

            var sequence = new List<string>{"Rod", "Jane", "Freddy"};
            action(sequence);
        }
    }
}
