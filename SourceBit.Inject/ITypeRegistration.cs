namespace SourceBit.Inject
{
    public interface ITypeRegistration
    {
        void AsSingleInstance();

        void AsPerDependencyInstance();
    }
}
