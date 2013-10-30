using System.Threading.Tasks;
using NUnit.Framework;
using Ploeh.AutoFixture;
using SourceBit.Inject.Tests.Data;

namespace SourceBit.Inject.Tests
{
    public class SingleInstanceResolvingStrategyTests : UnitTest
    {
        [Test]
        public void Resolve_ForInstanceRegisteredAsSingleInstanceUsignTypeParameter_ReturnInstance()
        {
            Container.Register(typeof(SimpleService), typeof(ISimpleService)).AsSingleInstance();

            var createdContainer = Container.Resolve(typeof(ISimpleService));

            Assert.That(createdContainer, Is.Not.Null);
        }

        [Test]
        public void Resolve_ForInstanceRegisteredAsSingleInstanceUsignGenericParameter_ReturnInstance()
        {
            Container.Register<SimpleService, ISimpleService>().AsSingleInstance();

            var createdContainer = Container.Resolve<ISimpleService>();

            Assert.That(createdContainer, Is.Not.Null);
        }

        [Test]
        public void Resolve_ForMultipleTimesAndInstanceRegisteredAsSingleInstanceUsignGenericParameter_ReturnTheSameInstance()
        {
            Container.Register<SimpleService, ISimpleService>().AsSingleInstance();

            var firstService = Container.Resolve<ISimpleService>();

            var secondService = Container.Resolve<ISimpleService>();

            Assert.That(firstService, Is.EqualTo(secondService));
        }

        [Test]
        public void Register_ForTheSameInstanceMultipleTimes_DoNotThrowAnException()
        {
            int times = Generation.Create<int>();

            for (int index = 0; index < times; index++)
            {
                Container.Register<SimpleService, ISimpleService>().AsSingleInstance();
            }
        }

        [Test]
        public void Register_ForMultipleTimesFromDifferentThreads_WithoutErrors()
        {
            Parallel.For(0, 100, (index, options) => Container.Register<SimpleService, ISimpleService>().AsSingleInstance());
        }

        [Test]
        public void Resovler_ForMultipleTimesFromDifferentThreads_WithoutErrors()
        {
            Container.Register<SimpleService, ISimpleService>().AsSingleInstance();

            Parallel.For(0, 100, (index, options) => Container.Resolve<ISimpleService>());
        }

        [Test]
        public void Resolve_ForGenericInterface_ReturnGenericImplementations()
        {
            Container.Register(typeof(SimpleGenericService<>), typeof(ISimpleGenericService<>)).AsSingleInstance();

            var simpleGenericService = Container.Resolve<ISimpleGenericService<SimpleModel>>();

            Assert.That(simpleGenericService, Is.InstanceOf<SimpleGenericService<SimpleModel>>());
        }

        [Test]
        public void Resolve_ForSimpleServiceWithDependency_ReturnTheService()
        {
            Container.Register<SimpleService, ISimpleService>().AsSingleInstance();
            Container.Register<SimpleServiceWithOneSimpleDependency, ISimpleServiceWithOneSimpleDependency>().AsSingleInstance();

            var service = Container.Resolve<ISimpleServiceWithOneSimpleDependency>();

            Assert.That(service, Is.InstanceOf<SimpleServiceWithOneSimpleDependency>());
        }

        [Test]
        public void Resolve_ForOpenGenericServiceWithDependency_ReturnTheService()
        {
            Container.Register<SimpleService, ISimpleService>().AsSingleInstance();
            Container.Register(typeof(SimpleGenericServiceWithOneSimpleDependency<>), typeof(ISimpleGenericServiceWithOneSimpleDependency<>)).AsSingleInstance();

            var service = Container.Resolve<ISimpleGenericServiceWithOneSimpleDependency<SimpleModel>>();

            Assert.That(service, Is.InstanceOf<SimpleGenericServiceWithOneSimpleDependency<SimpleModel>>());
        }

        [Test]
        public void Resolve_ForGenericServiceWithDependency_ReturnTheService()
        {
            Container.Register<SimpleService, ISimpleService>().AsSingleInstance();
            Container.Register<SimpleGenericServiceWithOneSimpleDependency<SimpleModel>, ISimpleGenericServiceWithOneSimpleDependency<SimpleModel>>().AsSingleInstance();

            var service = Container.Resolve<ISimpleGenericServiceWithOneSimpleDependency<SimpleModel>>();

            Assert.That(service, Is.InstanceOf<SimpleGenericServiceWithOneSimpleDependency<SimpleModel>>());
        }
    }
}