using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tempest.TaskTypes.DelayedData;

using NUnit.Framework;

namespace Tests.Tempest.TaskTypes.DelayedData
{
    [TestFixture]
    public class DelayedTests
    {
        [Test]
        public void Value()
        {
            var delayed = new Delayed<int>(100);
            Assert.That(delayed.HasValue, Is.True);
            Assert.That(delayed.Value(), Is.EqualTo(100));
        }

        [Test]
        public void Value_FromFactory()
        {
            var numberOfCalls = 0;

            var delayed = new Delayed<int>(() => 
            {
                numberOfCalls++;
                return 100;
            });

            // As it's a lambda we'll only fetch the value when asked
            Assert.That(delayed.HasValue, Is.False);

            Assert.That(delayed.Value(), Is.EqualTo(100));
            Assert.That(numberOfCalls, Is.EqualTo(1));

            // Now we've got the value!
            Assert.That(delayed.HasValue, Is.True);
            Assert.That(delayed.Value(), Is.EqualTo(100));
            Assert.That(numberOfCalls, Is.EqualTo(1));
        }

        [Test]
        public void FactoryThrowsException()
        {
            var numberOfCalls = 0;

            var delayed = new Delayed<int>(() => 
            {
                numberOfCalls++;
                throw new Exception();
            });

            // As it's a lambda we'll only fetch the value when asked
            Assert.That(delayed.HasValue, Is.False);

            Assert.Catch(() => delayed.Value());
            Assert.That(numberOfCalls, Is.EqualTo(1));

            // Now we've got the value!
            Assert.That(delayed.HasValue, Is.True);
            Assert.Catch(() => delayed.Value());
            Assert.That(numberOfCalls, Is.EqualTo(1));
        }
    }
}
