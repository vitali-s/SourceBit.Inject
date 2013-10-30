using System;
using System.Collections.Generic;
using NUnit.Framework;
using SourceBit.Inject.Tests.Data;

namespace SourceBit.Inject.Tests
{
    public class InstantiationTests : UnitTest
    {
        private List<Type> _dependencies;

        [Test]
        public void CreateActivator_ForPlainType_ReturnInstanceOfThatType()
        {
            var instance = Container.CreateActivator(typeof(SimpleService), out _dependencies).Invoke();

            Assert.That(instance, Is.InstanceOf<SimpleService>());
        }

        [Test]
        public void CreateActivator_ForOpenGenericType_ThrowAnException()
        {
            var exception = Assert.Throws<InvalidProgramException>(() => Container.CreateActivator(typeof(SimpleGenericService<>), out _dependencies).Invoke());

            Assert.That(exception.Message, Is.EqualTo("Common Language Runtime detected an invalid program."));
        }

        [Test]
        public void CreateActivator_ForDefinedGenericType_CorrectInstanceOfThatType()
        {
            var instance = Container.CreateActivator(typeof(SimpleGenericService<SimpleModel>), out _dependencies).Invoke();

            Assert.That(instance, Is.InstanceOf<SimpleGenericService<SimpleModel>>());
        }

        [Test]
        public void CreateActivator_ForDefinedMultipleGenericType_CorrectInstanceOfThatType()
        {
            var instance = Container.CreateActivator(typeof(GenericService<SimpleModel, SimpleModel, SimpleModel>), out _dependencies).Invoke();

            Assert.That(instance, Is.InstanceOf<GenericService<SimpleModel, SimpleModel, SimpleModel>>());
        }
    }
}
