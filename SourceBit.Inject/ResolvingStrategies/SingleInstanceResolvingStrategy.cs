using System;
using System.Collections;

namespace SourceBit.Inject.ResolvingStrategies
{
    internal class SingleInstanceResolvingStrategy : IResolvingStrategy
    {
        private static readonly object LockObject = new object();

        private readonly Hashtable _intances;

        public SingleInstanceResolvingStrategy()
        {
            _intances = new Hashtable();
        }

        public object Resolve(Type instanceType, Func<object> createInstance)
        {
            object instance = _intances[instanceType];

            if (instance == null)
            {
                instance = createInstance();

                lock (LockObject)
                {
                    _intances[instanceType] = instance;
                }
            }

            return instance;
        }
    }
}
