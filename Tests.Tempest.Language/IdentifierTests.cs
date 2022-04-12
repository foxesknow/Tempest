using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using Tempest.Language;

namespace Tests.Tempest.Language
{
    [TestFixture]
    public class IdentifierTests
    {
        [Test]
        public void DefaultInitialization()
        {
            Identifier name = default;
            Assert.That(name.IsInvalid, Is.True);
        }

        [Test]
        public void Initialization()
        {
            Identifier name = new("Sawyer");
            Assert.That(name.IsInvalid, Is.False);
            Assert.That(name.Name, Is.EqualTo("Sawyer"));
        }

        [Test]
        public void Initialization_Null()
        {
            Assert.Catch(() => new Identifier(null!));
        }

        [Test]
        public void Equality()
        {
            Identifier x = new("Jack");
            Identifier y = new("Jack");

            Assert.That(x, Is.EqualTo(y));
            Assert.That(x == y, Is.True);
            Assert.That(x.Equals(y), Is.True);
            Assert.That(x.Equals((object)y), Is.True);

            Identifier z = default;
            Assert.That(z, Is.EqualTo(z));
            Assert.That(z.Equals(z), Is.True);

            Assert.That(x, Is.Not.EqualTo(z));
            Assert.That(x == z, Is.False);
            Assert.That(x.Equals(z), Is.False);
            Assert.That(x.Equals((object)z), Is.False);
        }

        [Test]
        public void Inequality()
        {
            Identifier x = new("Jack");
            Identifier y = new("Kate");

            Assert.That(x, Is.Not.EqualTo(y));
            Assert.That(x == y, Is.False);
            Assert.That(x.Equals(y), Is.False);
            Assert.That(x.Equals((object)y), Is.False);

            Identifier z = default;

            Assert.That(x, Is.Not.EqualTo(z));
            Assert.That(x != z, Is.True);
        }

        [Test]
        public void HashCode()
        {
            Identifier x = new("Jack");
            Identifier y = new("Jack");
            Assert.That(x.GetHashCode(), Is.EqualTo(y.GetHashCode()));
        }

        [Test]
        public void HashCodeOnDefault()
        {
            Identifier x = default;
            Identifier y = default;
            Assert.That(x.GetHashCode(), Is.EqualTo(y.GetHashCode()));
        }

        [Test]
        public void AsString()
        {
            Identifier x = default;
            Assert.That(x.ToString(), Is.Not.Null & Has.Length.GreaterThan(0));

            Identifier y = new("Jack");
            Assert.That(y.ToString(), Is.Not.Null & Has.Length.GreaterThan(0));
        }
    }
}
