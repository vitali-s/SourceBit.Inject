namespace SourceBit.Inject.ResolvingStrategies
{
    internal class PerDependencyResolvingStrategy : IResolvingStrategy
    {
        public object Resolve(Registration registration)
        {
            object instance = registration.CreateInstance();

            return instance;
        }
    }
}
