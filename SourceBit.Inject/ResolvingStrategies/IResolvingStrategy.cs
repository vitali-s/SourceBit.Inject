namespace SourceBit.Inject.ResolvingStrategies
{
    public interface IResolvingStrategy
    {
        object Resolve(Registration registration);
    }
}
