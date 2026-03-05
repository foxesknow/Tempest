using System;
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
        public void Chain_Conditionals()
        {
            var p1 = Expression.Parameter<User>();
            var getName = Expression.GetProperty((User user) => user.Name);
            var stringToString = Expression.GetMethod((string s) => s.ToString());
            var getLength = Expression.GetProperty((string s) => s.Length);
            var intToString = Expression.GetMethod((int i) => i.ToString());

            /*
             * For a User, user, this is:
             * 
             *   user?.Name?.ToString()?.Length?.ToString()?.Length
             */
            var chain = p1.Chain
            (
                expr => expr.ConditionalProperty(getName),
                expr => expr.ConditionalCall(stringToString),
                expr => expr.ConditionalProperty(getLength),
                expr => expr.ConditionalCall(intToString),
                expr => expr.ConditionalProperty(getLength)
            );

            var lambda = Expression.Lambda<Func<User, int?>>(chain, p1);
            var function = lambda.Compile();
            
            Assert.That(function(null), Is.Null);

            var user = new User("abcdefghijklmnop");
            var answer = function(user);
            Assert.That(answer, Is.Not.Null);
            Assert.That(answer.Value, Is.EqualTo(2));
        }

        [Test]
        public void Chain_ToVoid()
        {
            var p1 = Expression.Parameter<List<int>>();
            var getRange = Expression.GetMethod((List<int> list) => list.GetRange(0, 1));
            var clear = Expression.GetMethod((List<int> list) => list.Clear());

            /*
             * For a List<int>, list, this is:
             * 
             *   list.GetRange(..)?.Clear()
             */
            var chain = p1.Chain
            (
                expr => expr.ConditionalCall(getRange, Expression.Constant(0), Expression.Constant(3)),
                expr => expr.ConditionalCall(clear)
            );

            var numbers = new List<int>{1, 1, 2, 3, 5, 8};
            var lambda = Expression.Lambda<Action<List<int>>>(chain, p1);
            var function = lambda.Compile();
            
            Assert.That(numbers.Count, Is.EqualTo(6));
            function(numbers);
            Assert.That(numbers.Count, Is.EqualTo(6));

        }

        
        [Test]
        public void Chain_CallThenProperty()
        {
            var ctor = Expression.GetConstructor(() => new List<int>());
            var getRange = Expression.GetMethod((List<int> list) => list.GetRange(0, 1));
            var getCount = Expression.GetProperty((List<int> list) => list.Count);

            var p1 = Expression.Parameter<List<int>>();
            var body = p1.Chain
            (
                expr => Expression.Call(expr, getRange, Expression.Constant(0), Expression.Constant(3)),
                expr => Expression.Property(expr, getCount)
            );

            var lambda = Expression.Lambda<Func<List<int>, int>>(body, p1);
            var function = lambda.Compile();

            var numbers = new List<int>{1, 1, 2, 3, 5, 8};
            var count = function(numbers);
            Assert.That(count, Is.EqualTo(3));
        }
    }
}
