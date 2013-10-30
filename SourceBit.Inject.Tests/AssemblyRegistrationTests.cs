using System.Reflection;
using NUnit.Framework;
using SourceBit.Inject.Tests.Data;

namespace SourceBit.Inject.Tests
{
    public class AssemblyRegistrationTests : UnitTest
    {
        [Test]
        public void Register_ForAllTypesInTestingAssebmly_ResolveSimpleService()
        {
            Container.Register(Assembly.GetExecutingAssembly()).AsSingleInstance();

            var service = Container.Resolve<ISimpleService>();

            Assert.That(service, Is.InstanceOf<SimpleService>());
        }

        [Test]
        public void Register_ForAllTypesInTestingAssebmly_ResolveGenericService()
        {
            Container.Register(Assembly.GetExecutingAssembly()).AsSingleInstance();

            var service = Container.Resolve<ISimpleGenericService<SimpleModel>>();

            Assert.That(service, Is.InstanceOf<SimpleGenericService<SimpleModel>>());
        }
    }
}
