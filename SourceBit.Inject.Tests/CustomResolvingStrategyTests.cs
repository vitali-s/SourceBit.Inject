using NUnit.Framework;
using SourceBit.Inject.Exceptions;
using SourceBit.Inject.Tests.Data;

namespace SourceBit.Inject.Tests
{
    public class CustomResolvingStrategyTests : UnitTest
    {
        [Test]
        public void Register_ForNotRegistredResolvingStrategy_ThrowResolvingStrategyNotFoundException()
        {
            Container.Register<SimpleService, ISimpleService>().AsCustomResolvingStrategy();

            var exception = Assert.Throws<ResolvingStrategyNotFoundException>(() => Container.Resolve<ISimpleService>());

            Assert.That(exception.Message, Is.EqualTo("Resolving strategy '255' is not registred in container."));
        }
    }
}
