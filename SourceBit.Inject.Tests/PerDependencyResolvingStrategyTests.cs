using NUnit.Framework;
using SourceBit.Inject.Tests.Data;

namespace SourceBit.Inject.Tests
{
    public class PerDependencyResolvingStrategyTests : UnitTest
    {
        [Test]
        public void Resolve_ForInstanceRegisteredAsSingleInstanceUsignTypeParameter_ReturnInstance()
        {
            Container.Register(typeof(SimpleService), typeof(ISimpleService)).AsPerDependencyInstance();

            var createdContainer = Container.Resolve(typeof(ISimpleService));

            Assert.That(createdContainer, Is.Not.Null);
        }

        [Test]
        public void Resolve_ForInstanceRegisteredAsSingleInstanceUsignGenericParameter_ReturnInstance()
        {
            Container.Register<SimpleService, ISimpleService>().AsPerDependencyInstance();

            var createdContainer = Container.Resolve<ISimpleService>();

            Assert.That(createdContainer, Is.Not.Null);
        }

        [Test]
        public void Resolve_ForMultipleTimesAndInstanceRegisteredAsSingleInstanceUsignGenericParameter_ReturnTheSameInstance()
        {
            Container.Register<SimpleService, ISimpleService>().AsPerDependencyInstance();

            var firstService = Container.Resolve<ISimpleService>();

            var secondService = Container.Resolve<ISimpleService>();

            Assert.That(firstService, Is.Not.EqualTo(secondService));
        }

        [Test]
        public void Resolve_ForGenericInterface_ReturnGenericImplementations()
        {
            Container.Register(typeof(SimpleGenericService<>), typeof(ISimpleGenericService<>)).AsPerDependencyInstance();

            var simpleGenericService = Container.Resolve<ISimpleGenericService<SimpleModel>>();

            Assert.That(simpleGenericService, Is.InstanceOf<SimpleGenericService<SimpleModel>>());
        }

        [Test]
        public void Resolve_ForSimpleServiceWithDependency_ReturnTheService()
        {
            Container.Register<SimpleService, ISimpleService>().AsPerDependencyInstance();
            Container.Register<SimpleServiceWithOneSimpleDependency, ISimpleServiceWithOneSimpleDependency>().AsPerDependencyInstance();

            var service = Container.Resolve<ISimpleServiceWithOneSimpleDependency>();

            Assert.That(service, Is.InstanceOf<ISimpleServiceWithOneSimpleDependency>());
        }

        [Test]
        public void Resolve_ForOpenGenericServiceWithDependency_ReturnTheService()
        {
            Container.Register<SimpleService, ISimpleService>().AsPerDependencyInstance();
            Container.Register(typeof(SimpleGenericServiceWithOneSimpleDependency<>), typeof(ISimpleGenericServiceWithOneSimpleDependency<>)).AsPerDependencyInstance();

            var service = Container.Resolve<ISimpleGenericServiceWithOneSimpleDependency<SimpleModel>>();

            Assert.That(service, Is.InstanceOf<SimpleGenericServiceWithOneSimpleDependency<SimpleModel>>());
        }

        [Test]
        public void Resolve_ForGenericServiceWithDependency_ReturnTheService()
        {
            Container.Register<SimpleService, ISimpleService>().AsPerDependencyInstance();
            Container.Register<SimpleGenericServiceWithOneSimpleDependency<SimpleModel>, ISimpleGenericServiceWithOneSimpleDependency<SimpleModel>>().AsPerDependencyInstance();

            var service = Container.Resolve<ISimpleGenericServiceWithOneSimpleDependency<SimpleModel>>();

            Assert.That(service, Is.InstanceOf<SimpleGenericServiceWithOneSimpleDependency<SimpleModel>>());
        }
    }
}
