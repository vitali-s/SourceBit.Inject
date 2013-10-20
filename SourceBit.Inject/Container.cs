using System;
using System.Collections.Generic;
using SourceBit.Inject.Exceptions;
using SourceBit.Inject.ResolvingStrategies;

namespace SourceBit.Inject
{
    public class Container : IContainer
    {
        private static readonly object LockObject = new object();

        private readonly IDictionary<Type, Registration> _registrations;
        private readonly IDictionary<byte, IResolvingStrategy> _resolvingStrategies;

        public Container()
        {
            _registrations = new Dictionary<Type, Registration>();

            _resolvingStrategies = new Dictionary<byte, IResolvingStrategy>
            {
                { 0, new SingleInstanceResolvingStrategy() },
                { 1, new PerDependencyResolvingStrategy() }
            };
        }

        public IRegistration Register(Type type, params Type[] asTypes)
        {
            var registration = new Registration(Register, Resolve, type, asTypes);

            return registration;
        }

        public IRegistration Register<TImplementation, TAbstraction>() where TImplementation : class where TAbstraction : class
        {
            return Register(typeof(TImplementation), typeof(TAbstraction));
        }

        public TAbstraction Resolve<TAbstraction>() where TAbstraction : class
        {
            var service = Resolve(typeof(TAbstraction)) as TAbstraction;

            return service;
        }

        public object Resolve(Type type)
        {
            Registration registration;

            _registrations.TryGetValue(type, out registration);

            if (registration == null)
            {
                throw new InstanceNotRegisteredException(string.Format("Instance '{0}' is not registred in container.", type.FullName));
            }

            IResolvingStrategy resolvingStrategy;

            _resolvingStrategies.TryGetValue(registration.ResolvingStrategyType, out resolvingStrategy);

            if (resolvingStrategy == null)
            {
                throw new ResolvingStrategyNotFoundException(string.Format("Resolving strategy '{0}' is not registred in container.", registration.ResolvingStrategyType));
            }

            object instance = resolvingStrategy.Resolve(registration);

            return instance;
        }

        public void Release()
        {
        }

        private void Register(Registration registration)
        {
            lock (LockObject)
            {
                int registrations = registration.AsTypes.Count;

                for (int index = 0; index < registrations; index++)
                {
                    _registrations.Add(registration.AsTypes[index], registration);
                }
            }
        }
    }
}
