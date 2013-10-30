using System.Collections;
using System.Collections.Generic;
using SourceBit.Inject.ResolvingStrategies;

namespace SourceBit.Inject
{
    public partial class Container : IContainer
    {
        private static readonly object LockObject = new object();

        private readonly Hashtable _registrations;
        private readonly IDictionary<int, IResolvingStrategy> _resolvingStrategies;

        public Container()
        {
            _registrations = new Hashtable();

            _resolvingStrategies = new Dictionary<int, IResolvingStrategy>
            {
                { 0, new SingleInstanceResolvingStrategy() },
                { 1, new PerDependencyResolvingStrategy() }
            };
        }
    }
}
