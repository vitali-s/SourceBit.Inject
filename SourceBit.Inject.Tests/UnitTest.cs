using System;
using System.Diagnostics;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace SourceBit.Inject.Tests
{
    [TestFixture]
    public abstract class UnitTest
    {
        protected UnitTest()
        {
            Generation = new Fixture();
        }

        public Container Container { get; set; }

        public Fixture Generation { get; set; }

        [SetUp]
        public void Setup()
        {
            Container = new Container();
        }

        public void Measure(Action action)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            action();

            stopwatch.Stop();

            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}
