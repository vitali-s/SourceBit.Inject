using System;
using System.Collections.Generic;

namespace SourceBit.Inject
{
    public sealed partial class Container
    {
        public void Register(Type implementation, Type[] asTypes, int lifeType)
        {
            var typeDetails = new TypeDetails
            {
                Type = implementation,
                StrategyType = lifeType
            };

            if (implementation.IsGenericTypeDefinition)
            {
            }
            else
            {
                List<Type> dependencies;

                var activator = CreateActivator(typeDetails.Type, out dependencies);

                typeDetails.Dependencies = dependencies;

                typeDetails.Instantiator = delegate
                {
                    int dependenciesLength = typeDetails.Dependencies.Count;

                    var parameters = new object[dependenciesLength];

                    for (int index = 0; index < dependenciesLength; index++)
                    {
                        parameters[index] = Resolve(typeDetails.Dependencies[index]);
                    }

                    return activator(parameters);
                };
            }

            lock (LockObject)
            {
                int count = asTypes.Length;

                for (int index = 0; index < count; index++)
                {
                    _registrations[asTypes[index]] = typeDetails;
                }
            }
        }
    }
}
