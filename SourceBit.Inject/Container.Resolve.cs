using System;
using System.Collections.Generic;
using SourceBit.Inject.Exceptions;
using SourceBit.Inject.ResolvingStrategies;

namespace SourceBit.Inject
{
    public partial class Container
    {
        public TAbstraction Resolve<TAbstraction>() where TAbstraction : class
        {
            var service = Resolve(typeof(TAbstraction)) as TAbstraction;

            return service;
        }

        public object Resolve(Type type)
        {
            Type registerType = type;

            var typeDetails = _registrations[registerType] as TypeDetails;

            if (typeDetails == null)
            {
                if (type.IsGenericType)
                {
                    typeDetails = _registrations[type.GetGenericTypeDefinition()] as TypeDetails;
                }

                if (typeDetails == null)
                {
                    throw new InstanceNotRegisteredException(string.Format("Instance '{0}' is not registred in container.", type.FullName));
                }
            }

            IResolvingStrategy resolvingStrategy;

            _resolvingStrategies.TryGetValue(typeDetails.StrategyType, out resolvingStrategy);

            if (resolvingStrategy == null)
            {
                throw new ResolvingStrategyNotFoundException(string.Format("Resolving strategy '{0}' is not registred in container.", typeDetails.StrategyType));
            }

            Type typeToGet = typeDetails.Type;

            if (typeToGet.IsGenericTypeDefinition)
            {
                typeToGet = typeToGet.MakeGenericType(type.GetGenericArguments());

                List<Type> dependencies;

                var activator = CreateActivator(typeToGet, out dependencies);

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

            object instance = resolvingStrategy.Resolve(typeToGet, typeDetails.Instantiator);

            return instance;
        }
    }
}
