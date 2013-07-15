namespace SoureBit.Infrastructure.Inject
{
    public static class InjectContainer
    {
        private static readonly IContainer InnerContainer = new Container();

        public static IContainer Container
        {
            get { return InnerContainer; }
        }
    }
}
