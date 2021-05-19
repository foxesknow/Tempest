using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tempest.Expressions;

using NUnit.Framework;

namespace Tests.Tempest.Expressions
{
    [TestFixture]
    public class TypeExtensionsTests
    {
        [Test]
        public void TryGetGenericImplementation()
        {
            Assert.That(typeof(List<string>).TryGetGenericImplementation(typeof(IEnumerable<>), out var implementation), Is.True);
            Assert.That(implementation, Is.EqualTo(typeof(IEnumerable<string>)));
        }

        [Test]
        public void TryGetGenericImplementation_NotFound()
        {
            Assert.That(typeof(List<string>).TryGetGenericImplementation(typeof(IComparable<>), out var implementation), Is.False);
            Assert.That(implementation, Is.Null);
        }

        [TestCase(typeof(byte), ExpectedResult = true)]
        [TestCase(typeof(sbyte), ExpectedResult = true)]
        [TestCase(typeof(short), ExpectedResult = true)]
        [TestCase(typeof(ushort), ExpectedResult = true)]
        [TestCase(typeof(int), ExpectedResult = true)]
        [TestCase(typeof(uint), ExpectedResult = true)]
        [TestCase(typeof(long), ExpectedResult = true)]
        [TestCase(typeof(ulong), ExpectedResult = true)]
        [TestCase(typeof(float), ExpectedResult = false)]
        [TestCase(typeof(double), ExpectedResult = false)]
        [TestCase(typeof(char), ExpectedResult = false)]
        [TestCase(typeof(string), ExpectedResult = false)]
        [TestCase(typeof(object), ExpectedResult = false)]
        public bool IsIntegral(Type type)
        {
            return type.IsIntegral();
        }

        [TestCase(typeof(byte), ExpectedResult = true)]
        [TestCase(typeof(sbyte), ExpectedResult = false)]
        [TestCase(typeof(short), ExpectedResult = false)]
        [TestCase(typeof(ushort), ExpectedResult = true)]
        [TestCase(typeof(int), ExpectedResult = false)]
        [TestCase(typeof(uint), ExpectedResult = true)]
        [TestCase(typeof(long), ExpectedResult = false)]
        [TestCase(typeof(ulong), ExpectedResult = true)]
        public bool IsUnsigned(Type type)
        {
            return type.IsUnsigned();
        }

        [TestCase(typeof(double))]
        [TestCase(typeof(float))]
        [TestCase(typeof(string))]
        [TestCase(typeof(object))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(DateTime))]
        public void IsUnsigned_NotIntegralType(Type type)
        {
            Assert.Catch(() => type.IsUnsigned());
        }

        [TestCase(typeof(byte), ExpectedResult = false)]
        [TestCase(typeof(sbyte), ExpectedResult = true)]
        [TestCase(typeof(short), ExpectedResult = true)]
        [TestCase(typeof(ushort), ExpectedResult = false)]
        [TestCase(typeof(int), ExpectedResult = true)]
        [TestCase(typeof(uint), ExpectedResult = false)]
        [TestCase(typeof(long), ExpectedResult = true)]
        [TestCase(typeof(ulong), ExpectedResult = false)]
        public bool IsSigned(Type type)
        {
            return type.IsSigned();
        }

        [TestCase(typeof(double))]
        [TestCase(typeof(float))]
        [TestCase(typeof(string))]
        [TestCase(typeof(object))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(DateTime))]
        public void IsSigned_NotIntegralType(Type type)
        {
            Assert.Catch(() => type.IsSigned());
        }

        [TestCase(typeof(byte), ExpectedResult = 1)]
        [TestCase(typeof(sbyte), ExpectedResult = 1)]
        [TestCase(typeof(short), ExpectedResult = 2)]
        [TestCase(typeof(ushort), ExpectedResult = 2)]
        [TestCase(typeof(int), ExpectedResult = 4)]
        [TestCase(typeof(uint), ExpectedResult = 4)]
        [TestCase(typeof(long), ExpectedResult = 8)]
        [TestCase(typeof(ulong), ExpectedResult = 8)]
        public int IntegralSize(Type type)
        {
            return type.IntegralSize();
        }

        [TestCase(typeof(double))]
        [TestCase(typeof(float))]
        [TestCase(typeof(string))]
        [TestCase(typeof(object))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(DateTime))]
        public void IntegralSize_NotIntegralType(Type type)
        {
            Assert.Catch(() => type.IntegralSize());
        }

        [TestCase(typeof(object), ExpectedResult = true)]
        [TestCase(typeof(string), ExpectedResult = true)]
        [TestCase(typeof(List<int>), ExpectedResult = true)]
        [TestCase(typeof(List<string>), ExpectedResult = true)]
        [TestCase(typeof(Nullable<int>), ExpectedResult = true)]
        [TestCase(typeof(int), ExpectedResult = false)]
        public bool IsNullable(Type type)
        {
            return type.IsNullable();
        }
    }
}
