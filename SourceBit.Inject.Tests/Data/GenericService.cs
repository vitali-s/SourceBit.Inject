namespace SourceBit.Inject.Tests.Data
{
    [Inject]
    public class GenericService<TModel, TData, TEntity> : IGenericService<TModel, TData, TEntity>
    {
    }
}
