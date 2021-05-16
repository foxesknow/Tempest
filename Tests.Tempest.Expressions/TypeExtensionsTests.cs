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
    }
}
