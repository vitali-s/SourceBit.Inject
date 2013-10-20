using NUnit.Framework;
using SourceBit.Inject.Tests.Data;

namespace SourceBit.Inject.Tests
{
    public class SingleInstanceResolvingStrategyTests
    {
        [Test]
        public void Resolve_ForInstanceRegisteredAsSingleInstanceUsignTypeParameter_ReturnInstance()
        {
            var container = new Container();

            container.Register(typeof(SimpleService), typeof(ISimpleService)).AsSingleInstance();

            var createdContainer = container.Resolve(typeof(ISimpleService));

            Assert.That(createdContainer, Is.Not.Null);
        }

        [Test]
        public void Resolve_ForInstanceRegisteredAsSingleInstanceUsignGenericParameter_ReturnInstance()
        {
            var container = new Container();

            container.Register<SimpleService, ISimpleService>().AsSingleInstance();

            var createdContainer = container.Resolve<ISimpleService>();

            Assert.That(createdContainer, Is.Not.Null);
        }

        [Test]
        public void Resolve_ForMultipleTimesAndInstanceRegisteredAsSingleInstanceUsignGenericParameter_ReturnTheSameInstance()
        {
            var container = new Container();

            container.Register<SimpleService, ISimpleService>().AsSingleInstance();

            var firstService = container.Resolve<ISimpleService>();

            var secondService = container.Resolve<ISimpleService>();

            Assert.That(firstService, Is.EqualTo(secondService));
        }
    }
}
