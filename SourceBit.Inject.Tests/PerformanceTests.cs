using Autofac;
using NUnit.Framework;
using SourceBit.Inject.Tests.Data;

namespace SourceBit.Inject.Tests
{
    public class PerformanceTests : UnitTest
    {
        [Test]
        public void Resolve_ForSingleInstance()
        {
            var container = new Container();
            container.Register<SimpleService, ISimpleService>().AsSingleInstance();

            var conainerbuilder = new ContainerBuilder();
            conainerbuilder.RegisterType<SimpleService>().As<ISimpleService>().SingleInstance();
            var autofacContainer = conainerbuilder.Build();

            int numberOfTimes = 100000;

            Measure(() =>
            {
                for (int i = 0; i < numberOfTimes; i++)
                {
                    container.Resolve<ISimpleService>();
                }
            });

            Measure(() =>
            {
                for (int i = 0; i < numberOfTimes; i++)
                {
                    autofacContainer.Resolve<ISimpleService>();
                }
            });
        }

        [Test]
        public void Resolve_ForPerDependencyInstance()
        {
            var container = new Container();
            container.Register<SimpleService, ISimpleService>().AsPerDependencyInstance();

            var conainerbuilder = new ContainerBuilder();
            conainerbuilder.RegisterType<SimpleService>().As<ISimpleService>().InstancePerDependency();
            var autofacContainer = conainerbuilder.Build();

            int numberOfTimes = 10000;

            Measure(() =>
            {
                for (int i = 0; i < numberOfTimes; i++)
                {
                    container.Resolve<ISimpleService>();
                }
            });

            Measure(() =>
            {
                for (int i = 0; i < numberOfTimes; i++)
                {
                    autofacContainer.Resolve<ISimpleService>();
                }
            });
        }
    }
}
