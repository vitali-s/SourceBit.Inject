namespace SourceBit.Inject.Tests.Data
{
    public class SimpleServiceWithOneSimpleDependency : ISimpleServiceWithOneSimpleDependency
    {
        private readonly ISimpleService _simpleService;

        public SimpleServiceWithOneSimpleDependency(ISimpleService simpleService)
        {
            _simpleService = simpleService;
        }
    }
}
