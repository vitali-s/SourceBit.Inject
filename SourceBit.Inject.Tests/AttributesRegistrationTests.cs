using System.Reflection;
using NUnit.Framework;
using SourceBit.Inject.Tests.Data;

namespace SourceBit.Inject.Tests
{
    public class AttributesRegistrationTests : UnitTest
    {
        [Test]
        public void Resolve_ForSimpleServiceWithInjectAttributeByInterface_ReturnSingleSimpleServiceInstance()
        {
            Container.RegisterByAttributes(Assembly.GetExecutingAssembly());

            var simpleService = Container.Resolve<ISimpleService>();

            Assert.That(simpleService, Is.InstanceOf<SimpleService>());
        }

        [Test]
        public void Resolve_ForSimpleServiceWithInjectAttributeByInterface_ReturnSingleInstance()
        {
            Container.RegisterByAttributes(Assembly.GetExecutingAssembly());

            var simpleService = Container.Resolve<ISimpleService>();

            var anotherSimpleService = Container.Resolve<ISimpleService>();

            Assert.That(simpleService, Is.EqualTo(anotherSimpleService));
        }

        [Test]
        public void Resolve_ForSimpleServiceWithPerdependencyLifeType_ReturnNewInstances()
        {
            Container.RegisterByAttributes(Assembly.GetExecutingAssembly());

            var simpleService = Container.Resolve<IPerDependencySimpleService>();

            var anotherSimpleService = Container.Resolve<IPerDependencySimpleService>();

            Assert.That(simpleService, Is.Not.EqualTo(anotherSimpleService));
        }

        [Test]
        public void Resolve_ForSimpleServiceWithInjectAttributeAsSelf_ReturnSingleSimpleServiceInstance()
        {
            Container.RegisterByAttributes(Assembly.GetExecutingAssembly());

            var simpleService = Container.Resolve<SelfSimpleService>();

            Assert.That(simpleService, Is.InstanceOf<SelfSimpleService>());
        }

        [Test]
        public void Resolve_ForSimpleServiceWithInjectAttribute_ReturnSingleInstance()
        {
            Container.RegisterByAttributes(Assembly.GetExecutingAssembly());

            var simpleService = Container.Resolve<SelfSimpleService>();

            var anotherSimpleService = Container.Resolve<SelfSimpleService>();

            Assert.That(simpleService, Is.EqualTo(anotherSimpleService));
        }

        [Test]
        public void Resolve_ForGenericServiceWithInjectAttribute_RetrunServiceInstance()
        {
            Container.RegisterByAttributes(Assembly.GetExecutingAssembly());

            var simpleService = Container.Resolve<IGenericService<SimpleModel, SimpleModel, SimpleModel>>();

            var anotherSimpleService = Container.Resolve<IGenericService<SimpleModel, SimpleModel, SimpleModel>>();

            Assert.That(simpleService, Is.EqualTo(anotherSimpleService));
        }
    }
}
