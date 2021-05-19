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
        public void ConditionCall_VoidMethod()
        {
            var p1 = ExpressionEx.Parameter<List<int>>();
            var method = ExpressionEx.GetMethod((List<int> list) => list.Clear());

            var call = ExpressionEx.ConditionalCall(p1, method);
            var lambda = Expression.Lambda<Action<List<int>>>(call, p1);
            var function = lambda.Compile();
            
            var numbers = new List<int>{1, 1, 2, 3, 5, 8};
            function(numbers);

            Assert.That(numbers.Count, Is.EqualTo(0));
        }

        [Test]
        public void ConditionCall_WithParameters()
        {
            var p1 = ExpressionEx.Parameter<List<int>>();
            var method = ExpressionEx.GetMethod((List<int> list) => list.GetRange(0, 4));

            var call = ExpressionEx.ConditionalCall(p1, method, Expression.Constant(0), Expression.Constant(4));
            var lambda = Expression.Lambda<Func<List<int>, List<int>>>(call, p1);
            var function = lambda.Compile();
            
            var numbers = new List<int>{1, 1, 2, 3, 5, 8};
            var answer = function(numbers);

            Assert.That(answer, Is.Not.Null);
            Assert.That(answer.Count, Is.EqualTo(4));
        }

        [Test]
        public void ConditionCall_ObjectToValue()
        {
            var target = Expression.Constant("Jack and Bob");
            var name = Expression.Constant("Bob");
            var method = ExpressionEx.GetMethod((string s) => s.IndexOf("f"));

            var call = ExpressionEx.ConditionalCall(target, method, name);
            var lambda = Expression.Lambda<Func<int?>>(call);
            var function = lambda.Compile();
            var answer = function();

            Assert.That(answer, Is.Not.Null);
            Assert.That(answer.Value, Is.EqualTo(9));
        }

        [Test]
        public void ConditionCall_ObjectToValue_Null()
        {
            var target = Expression.Constant(null, typeof(string));
            var name = Expression.Constant("Bob");
            var method = ExpressionEx.GetMethod((string s) => s.IndexOf("f"));

            var call = ExpressionEx.ConditionalCall(target, method, name);
            var lambda = Expression.Lambda<Func<int?>>(call);
            var function = lambda.Compile();
            var answer = function();

            Assert.That(answer, Is.Null);
        }

        [Test]
        public void ConditionCall_ObjectToObject()
        {
            var target = Expression.Constant("Jack and Bob");
            var index = Expression.Constant(9);
            var method = ExpressionEx.GetMethod((string s) => s.Substring(9));

            var call = ExpressionEx.ConditionalCall(target, method, index);
            var lambda = Expression.Lambda<Func<string>>(call);
            var function = lambda.Compile();
            var answer = function();

            Assert.That(answer, Is.Not.Null);
            Assert.That(answer, Is.EqualTo("Bob"));
        }

        [Test]
        public void ConditionCall_ObjectToObject_Null()
        {
            var target = Expression.Constant(null, typeof(String));
            var index = Expression.Constant(9);
            var method = ExpressionEx.GetMethod((string s) => s.Substring(9));

            var call = ExpressionEx.ConditionalCall(target, method, index);
            var lambda = Expression.Lambda<Func<string>>(call);
            var function = lambda.Compile();
            var answer = function();

            Assert.That(answer, Is.Null);
        }

        [Test]
        public void ConditionCall_ValueToObject()
        {
            var p1 = Expression.Parameter(typeof(int?));
            var method = ExpressionEx.GetMethod((int s) => s.ToString());

            var call = ExpressionEx.ConditionalCall(p1, method);
            var lambda = Expression.Lambda<Func<int?, string>>(call, p1);
            var function = lambda.Compile();
            var answer = function(10);

            Assert.That(answer, Is.Not.Null);
            Assert.That(answer, Is.EqualTo("10"));
        }

        [Test]
        public void ConditionCall_ValueToObject_Null()
        {
            var p1 = Expression.Parameter(typeof(int?));
            var method = ExpressionEx.GetMethod((int s) => s.ToString());

            var call = ExpressionEx.ConditionalCall(p1, method);
            var lambda = Expression.Lambda<Func<int?, string>>(call, p1);
            var function = lambda.Compile();
            var answer = function(null);

            Assert.That(answer, Is.Null);
        }

        [Test]
        public void ConditionCall_ValueToValue()
        {
            var p1 = Expression.Parameter(typeof(int?));
            var method = ExpressionEx.GetMethod((int s) => s.GetHashCode());

            var call = ExpressionEx.ConditionalCall(p1, method);
            var lambda = Expression.Lambda<Func<int?, int?>>(call, p1);
            var function = lambda.Compile();
            var answer = function(10);

            Assert.That(answer, Is.Not.Null);
            Assert.That(answer, Is.EqualTo(10.GetHashCode()));
        }

        [Test]
        public void ConditionCall_ValueToValue_Null()
        {
            var p1 = Expression.Parameter(typeof(int?));
            var method = ExpressionEx.GetMethod((int s) => s.GetHashCode());

            var call = ExpressionEx.ConditionalCall(p1, method);
            var lambda = Expression.Lambda<Func<int?, int?>>(call, p1);
            var function = lambda.Compile();
            var answer = function(null);

            Assert.That(answer, Is.Null);
        }

        [Test]
        public void ConditionCall_Overloaded_Equals_Not_Called()
        {
            var p1 = Expression.Parameter(typeof(User));
            var method = ExpressionEx.GetMethod((User s) => s.ToString());

            var call = ExpressionEx.ConditionalCall(p1, method);
            var lambda = Expression.Lambda<Func<User, string>>(call, p1);
            var function = lambda.Compile();
            
            var user = new User("Bob");
            var answer = function(user);

            Assert.That(answer, Is.Not.Null);
            Assert.That(answer, Is.EqualTo("Bob"));
            Assert.That(user.OverloadCalls, Is.EqualTo(0));
        }

        class User
        {
            public User(string name)
            {
                Name = name;
            }

            public string Name{get;}

            public int OverloadCalls{get; private set;}

            private void Touch()
            {
                OverloadCalls++;
            }

            public static bool operator==(User lhs, User rhs)
            {
                var (gotL, gotR) = (Object.ReferenceEquals(lhs, null), Object.ReferenceEquals(rhs, null));

                lhs?.Touch();
                rhs?.Touch();

                return (gotL, gotR) switch
                {
                    (true, true) => true,
                    (true, false) => false,
                    (false, true) => false,
                    (false, false) => lhs.Name == rhs.Name
                };
            }

            public static bool operator!=(User lhs, User rhs)
            {
                return !(lhs == rhs);
            }

            public override bool Equals(object obj)
            {
                if(ReferenceEquals(this, obj))
                {
                    return true;
                }

                if(ReferenceEquals(obj, null))
                {
                    return false;
                }

                return obj is User user && user.Name == Name;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }

            public override string ToString()
            {
                return Name;
            }
        }    
    }
}
