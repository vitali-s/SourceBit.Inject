namespace SoureBit.Infrastructure.Inject
{
    public class InjectContainer
    {
        private static readonly object LockObject = new object();
        private static IContainer _innerContainer;

        static InjectContainer()
        {
            Clear();
        }

        public static void Clear()
        {
            lock (LockObject)
            {
                _innerContainer = new Container();
            }
        }
    }
}
