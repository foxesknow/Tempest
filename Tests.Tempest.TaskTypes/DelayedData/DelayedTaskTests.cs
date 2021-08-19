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
    public class DelayedTaskTests
    {
        [Test]
        public async ValueTask AsTask()
        {
            var delayed = new Delayed<int>(100);
            Assert.That(delayed.HasValue, Is.True);
            Assert.That(await delayed.AsTask(), Is.EqualTo(100));
        }

        [Test]
        public async ValueTask Value_FromFactory()
        {
            var numberOfCalls = 0;

            var delayed = new Delayed<int>(() => 
            {
                numberOfCalls++;
                return 100;
            });

            // As it's a lambda we'll only fetch the value when asked
            Assert.That(delayed.HasValue, Is.False);

            Assert.That(await delayed.AsTask(), Is.EqualTo(100));
            Assert.That(numberOfCalls, Is.EqualTo(1));

            // Now we've got the value!
            Assert.That(delayed.HasValue, Is.True);
            Assert.That(await delayed.AsTask(), Is.EqualTo(100));
            Assert.That(numberOfCalls, Is.EqualTo(1));
        }

        [Test]
        public async ValueTask Value_FromFactory_Task_After_Delay()
        {
            var numberOfCalls = 0;

            var delayed = new Delayed<int>(() => 
            {
                numberOfCalls++;
                return 100;
            });

            // As it's a lambda we'll only fetch the value when asked
            Assert.That(delayed.HasValue, Is.False);

            Assert.That(await delayed, Is.EqualTo(100));
            Assert.That(numberOfCalls, Is.EqualTo(1));

            // Now we've got the value!
            Assert.That(delayed.HasValue, Is.True);
            Assert.That(await delayed.AsTask(), Is.EqualTo(100));
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

            Assert.CatchAsync(async () => await delayed.AsTask());
            Assert.That(numberOfCalls, Is.EqualTo(1));

            // Now we've got the value!
            Assert.That(delayed.HasValue, Is.True);
            Assert.CatchAsync(async () => await delayed.AsTask());
            Assert.That(numberOfCalls, Is.EqualTo(1));
        }
    }
}
