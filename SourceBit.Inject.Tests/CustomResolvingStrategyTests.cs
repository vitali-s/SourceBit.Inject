using NUnit.Framework;
using SourceBit.Inject.Exceptions;
using SourceBit.Inject.Tests.Data;

namespace SourceBit.Inject.Tests
{
    public class CustomResolvingStrategyTests
    {
        [Test]
        public void Register_ForNotRegistredResolvingStrategy_ThrowResolvingStrategyNotFoundException()
        {
            var container = new Container();

            container.Register<SimpleService, ISimpleService>().AsCustomResolvingStrategy();

            var exception = Assert.Throws<ResolvingStrategyNotFoundException>(() => container.Resolve<ISimpleService>());

            Assert.That(exception.Message, Is.EqualTo("Resolving strategy '255' is not registred in container."));
        }
    }
}
