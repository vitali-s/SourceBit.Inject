using System;
using System.Collections.Generic;

namespace SourceBit.Inject.ResolvingStrategies
{
    internal class SingleInstanceResolvingStrategy : IResolvingStrategy
    {
        private static readonly object LockObject = new object();

        private readonly IDictionary<Type, object> _intances;

        public SingleInstanceResolvingStrategy()
        {
            _intances = new Dictionary<Type, object>();
        }

        public object Resolve(Registration registration)
        {
            object instance;

            _intances.TryGetValue(registration.Type, out instance);

            if (instance == null)
            {
                instance = registration.CreateInstance();

                lock (LockObject)
                {
                    _intances[registration.Type] = instance;
                }
            }

            return instance;
        }
    }
}
