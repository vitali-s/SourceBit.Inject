namespace SourceBit.Inject.Tests.Data
{
    public interface ISimpleGenericInterface<TModel> where TModel : class
    {
        void Get();
    }
}