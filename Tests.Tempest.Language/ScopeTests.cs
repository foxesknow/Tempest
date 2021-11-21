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
    public class ScopeTests
    {
        [Test]
        public void Bind()
        {
            var scope = new Scope();

            var name = new Identifier("name");
            Assert.That(scope.IsBound(name), Is.False);

            scope.Bind(name, "Jack");
            Assert.That(scope.IsBound(name), Is.True);
        }
    }
}
