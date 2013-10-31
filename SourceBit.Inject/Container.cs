using System.Collections;
using SourceBit.Inject.ResolvingStrategies;

namespace SourceBit.Inject
{
    public sealed partial class Container : IContainer
    {
        private static readonly object LockObject = new object();

        private readonly Hashtable _registrations;
        private readonly Hashtable _resolvingStrategies;

        public Container()
        {
            _registrations = new Hashtable();

            _resolvingStrategies = new Hashtable
            {
                { (int)LifeTypes.Single, new SingleInstanceResolvingStrategy() },
                { (int)LifeTypes.PerDependency, new PerDependencyResolvingStrategy() }
            };
        }
    }
}
