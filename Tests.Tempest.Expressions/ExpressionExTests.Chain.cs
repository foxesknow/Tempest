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
        public void Chain_1()
        {
            var p1 = ExpressionEx.Parameter<User>();
            var getName = ExpressionEx.GetProperty((User user) => user.Name);
            var stringToString = ExpressionEx.GetMethod((string s) => s.ToString());
            var getLength = ExpressionEx.GetProperty((string s) => s.Length);
            var intToString = ExpressionEx.GetMethod((int i) => i.ToString());

            /*
             * For a User, user, this is:
             * 
             *   user?.Name?.ToString()?.Length?.ToString()?.Length
             */
            var chain = ExpressionEx.Chain
            (
                p1,
                expr => ExpressionEx.ConditionalProperty(expr, getName),
                expr => ExpressionEx.ConditionalCall(expr, stringToString),
                expr => ExpressionEx.ConditionalProperty(expr, getLength),
                expr => ExpressionEx.ConditionalCall(expr, intToString),
                expr => ExpressionEx.ConditionalProperty(expr, getLength)
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
        public void Chain_2()
        {
            var p1 = ExpressionEx.Parameter<List<int>>();
            var getRange = ExpressionEx.GetMethod((List<int> list) => list.GetRange(0, 1));
            var clear = ExpressionEx.GetMethod((List<int> list) => list.Clear());

            /*
             * For a List<int>, list, this is:
             * 
             *   list.GetRange(..)?.Clear()
             */
            var chain = ExpressionEx.Chain
            (
                p1,
                expr => ExpressionEx.ConditionalCall(expr, getRange, Expression.Constant(0), Expression.Constant(3)),
                expr => ExpressionEx.ConditionalCall(expr, clear)
            );

            var numbers = new List<int>{1, 1, 2, 3, 5, 8};
            var lambda = Expression.Lambda<Action<List<int>>>(chain, p1);
            var function = lambda.Compile();
            
            Assert.That(numbers.Count, Is.EqualTo(6));
            function(numbers);
            Assert.That(numbers.Count, Is.EqualTo(6));

        }

        
        [Test]
        public void Chain_3()
        {
            var ctor = ExpressionEx.GetConstructor(() => new List<int>());
            var getRange = ExpressionEx.GetMethod((List<int> list) => list.GetRange(0, 1));
            var getCount = ExpressionEx.GetProperty((List<int> list) => list.Count);

            var p1 = ExpressionEx.Parameter<List<int>>();
            var body = ExpressionEx.Chain
            (
                p1,
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
