using System;

namespace SourceBit.Inject.ResolvingStrategies
{
    internal class PerDependencyResolvingStrategy : IResolvingStrategy
    {
        public object Resolve(Type instanceType, Func<object> createInstance)
        {
            object instance = createInstance();

            return instance;
        }
    }
}
