using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tempest.TaskTypes.StandardLazy;

using NUnit.Framework;

namespace Tests.Tempest.TaskTypes.StandardLazy
{
    [TestFixture]
    public class StandardLazyTests
    {
        [Test]
        public async ValueTask FromValue()
        {
            var lazy = new Lazy<int>(100);
            var value = await lazy;
            Assert.That(value, Is.EqualTo(100));
        }

        [Test]
        public async ValueTask FromFunction()
        {
            var lazy = new Lazy<int>(() => 101);
            var value = await lazy;
            Assert.That(value, Is.EqualTo(101));
        }

        [Test]
        public void ThrowsException()
        {
            var lazy = new Lazy<int>(() => throw new Exception());
            Assert.CatchAsync(async () => await lazy);
        }
    }
}
