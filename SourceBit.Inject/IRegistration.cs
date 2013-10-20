namespace SourceBit.Inject
{
    public interface IRegistration
    {
        void AsSingleInstance();

        void AsPerDependencyInstance();

        void AsPerThreadInstance();
    }
}
