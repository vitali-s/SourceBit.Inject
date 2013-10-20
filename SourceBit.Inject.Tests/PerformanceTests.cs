using System;
using System.Diagnostics;
using Autofac;
using NUnit.Framework;
using SourceBit.Inject.Tests.Data;

namespace SourceBit.Inject.Tests
{
    public class PerformanceTests
    {
        [Test]
        public void Resolve_ForSingleInstance()
        {
            var container = new Container();

            container.Register<SimpleService, ISimpleService>().AsSingleInstance();

            Measure(() =>
            {
                for (int i = 0; i < 10000000; i++)
                {
                    container.Resolve<ISimpleService>();
                }
            });

            var conainerbuilder = new ContainerBuilder();
            conainerbuilder.RegisterType<SimpleService>().As<ISimpleService>().SingleInstance();
            var autofacContainer = conainerbuilder.Build();

            Measure(() =>
            {
                for (int i = 0; i < 10000000; i++)
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

            Measure(() =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    container.Resolve<ISimpleService>();
                }
            });

            var conainerbuilder = new ContainerBuilder();
            conainerbuilder.RegisterType<SimpleService>().As<ISimpleService>().InstancePerDependency();
            var autofacContainer = conainerbuilder.Build();

            Measure(() =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    autofacContainer.Resolve<ISimpleService>();
                }
            });
        }
        

        private void Measure(Action action)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            action();

            stopwatch.Stop();

            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}
