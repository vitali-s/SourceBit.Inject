namespace SourceBit.Inject.Tests.Data
{
    [Inject(LifeTypes.PerDependency)]
    public class PerDependencySimpleService : IPerDependencySimpleService
    {
    }
}
