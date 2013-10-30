using System;

namespace SourceBit.Inject.ResolvingStrategies
{
    public interface IResolvingStrategy
    {
        object Resolve(Type instanceType, Func<object> createInstance);
    }
}
