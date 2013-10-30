using NUnit.Framework;
using SourceBit.Inject.Exceptions;
using SourceBit.Inject.Tests.Data;

namespace SourceBit.Inject.Tests
{
    public class ContainerTests : UnitTest
    {
        [Test]
        public void CreateContainer_ForNewContainer_ReturnTheInstanceOfIContainer()
        {
            Assert.IsInstanceOf<IContainer>(Container, "Container should be an instance of IContainer interface.");
        }

        [Test]
        public void Resolve_ForNotRegistredInstanceUsingTypeParameter_ThrowException()
        {
            var exception = Assert.Throws<InstanceNotRegisteredException>(() => Container.Resolve(typeof(ISimpleService)));

            Assert.That(exception.Message, Is.EqualTo("Instance 'SourceBit.Inject.Tests.Data.ISimpleService' is not registred in container."));
        }

        [Test]
        public void Resolve_ForNotRegistredInstanceUsingGenericParameter_ThrowException()
        {
            var exception = Assert.Throws<InstanceNotRegisteredException>(() => Container.Resolve<ISimpleService>());

            Assert.That(exception.Message, Is.EqualTo("Instance 'SourceBit.Inject.Tests.Data.ISimpleService' is not registred in container."));
        }
    }
}
