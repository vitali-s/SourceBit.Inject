using System.Collections.Generic;
using NUnit.Framework;
using SourceBit.Inject.Exceptions;
using SourceBit.Inject.Tests.Data;

namespace SourceBit.Inject.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void CreateContainer_ForNewContainer_ReturnTheInstanceOfIContainer()
        {
            IContainer container = new Container();

            Assert.IsInstanceOf<IContainer>(container, "Container should be an instance of IContainer interface.");
        }

        [Test]
        public void Resolve_ForNotRegistredInstanceUsingTypeParameter_ThrowException()
        {
            var container = new Container();

            var exception = Assert.Throws<InstanceNotRegisteredException>(() => container.Resolve(typeof(ISimpleService)));

            Assert.That(exception.Message, Is.EqualTo("Instance 'SourceBit.Inject.Tests.Data.ISimpleService' is not registred in container."));
        }

        [Test]
        public void Resolve_ForNotRegistredInstanceUsingGenericParameter_ThrowException()
        {
            var container = new Container();

            var exception = Assert.Throws<InstanceNotRegisteredException>(() => container.Resolve<ISimpleService>());

            Assert.That(exception.Message, Is.EqualTo("Instance 'SourceBit.Inject.Tests.Data.ISimpleService' is not registred in container."));
        }
    }
}
