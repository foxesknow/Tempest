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
        public void ConditionalProperty_ObjectToValue()
        {
            var p1 = Expression.Parameter<List<int>>();
            var property = Expression.GetProperty((List<int> list) => list.Count);

            var call = p1.ConditionalProperty(property);
            var lambda = Expression.Lambda<Func<List<int>, int?>>(call, p1);
            var function = lambda.Compile();
            
            var numbers = new List<int>{1, 1, 2, 3, 5, 8};
            var answer = function(numbers);

            Assert.That(answer, Is.Not.Null);
            Assert.That(answer.Value, Is.EqualTo(6));
        }

        [Test]
        public void ConditionalProperty_ObjectToValue_Null()
        {
            var p1 = Expression.Parameter<List<int>>();
            var property = Expression.GetProperty((List<int> list) => list.Count);

            var call = p1.ConditionalProperty(property);
            var lambda = Expression.Lambda<Func<List<int>, int?>>(call, p1);
            var function = lambda.Compile();
            
            var answer = function(null);

            Assert.That(answer, Is.Null);
        }

        [Test]
        public void ConditionalProperty_ObjectToObject()
        {
            var p1 = Expression.Parameter<User>();
            var property = Expression.GetProperty((User user) => user.Name);

            var call = p1.ConditionalProperty(property);
            var lambda = Expression.Lambda<Func<User, string>>(call, p1);
            var function = lambda.Compile();
            
            var user = new User("Rod");
            var answer = function(user);

            Assert.That(answer, Is.Not.Null);
            Assert.That(answer, Is.EqualTo("Rod"));
            Assert.That(user.OverloadCalls, Is.EqualTo(0));
        }

        [Test]
        public void ConditionalProperty_ObjectToObject_Null()
        {
            var p1 = Expression.Parameter<User>();
            var property = Expression.GetProperty((User user) => user.Name);

            var call = p1.ConditionalProperty(property);
            var lambda = Expression.Lambda<Func<User, string>>(call, p1);
            var function = lambda.Compile();
            
            var answer = function(null);

            Assert.That(answer, Is.Null);
        }

        [Test]
        public void Conditional_Calls_Properties()
        {
            var p1 = Expression.Parameter<User>();
            var getName = Expression.GetProperty((User user) => user.Name);
            var toString = Expression.GetMethod((string s) => s.ToString());
            var getLength = Expression.GetProperty((string s) => s.Length);

            var chain = p1.ConditionalProperty(getName).ConditionalCall(toString).ConditionalProperty(getLength);

            var lambda = Expression.Lambda<Func<User, int?>>(chain, p1);
            var function = lambda.Compile();
            
            Assert.That(function(null), Is.Null);

            var user = new User("Freddy");
            var answer = function(user);
            Assert.That(answer, Is.Not.Null);
            Assert.That(answer.Value, Is.EqualTo(6));
        }

        [Test]
        public void Conditional_Calls_Properties_NullName()
        {
            var p1 = Expression.Parameter<User>();
            var getName = Expression.GetProperty((User user) => user.Name);
            var toString = Expression.GetMethod((string s) => s.ToString());
            var getLength = Expression.GetProperty((string s) => s.Length);

            var chain = p1.ConditionalProperty(getName).ConditionalCall(toString).ConditionalProperty(getLength);

            var lambda = Expression.Lambda<Func<User, int?>>(chain, p1);
            var function = lambda.Compile();
            
            var user = new User(null);
            var answer = function(user);
            Assert.That(answer, Is.Null);
        }

        [Test]
        public void Conditional_Calls_Properties_NullName_ValueType()
        {
            var p1 = Expression.Parameter<User>();
            var getBirthday = Expression.GetProperty((User user) => user.Birthday);
            var getAge = Expression.GetProperty((Birthday b) => b.Age);
            
            var chain = p1.ConditionalProperty(getBirthday).ConditionalProperty(getAge);

            var lambda = Expression.Lambda<Func<User, int?>>(chain, p1);
            var function = lambda.Compile();
            
            Assert.That(function(null), Is.Null);
        }
    }
}
