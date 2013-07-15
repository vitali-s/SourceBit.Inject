using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SoureBit.Infrastructure.Inject
{
    internal class Container : IContainer
    {
        private readonly Dictionary<Type, Registration> _registrations = new Dictionary<Type, Registration>();
        private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();
        private readonly Dictionary<Type, Constructor> _constructors = new Dictionary<Type, Constructor>();

        // Extensions
        public void RegisterControllers(Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t => typeof(IController).IsAssignableFrom(t) && t.Name.EndsWith("Controller"))
                .ToList();

            foreach (var type in types)
            {
                Register(type, type, LifeTypes.PerDependency);
            }
        }

        public void RegisterAssemblies(params string[] assembliesNames)
        {
            var assemblies = _assemblyLocator.GetAssemblies(assembliesNames);

            foreach (var assembly in assemblies)
            {
                Register(assembly);
            }
        }

        public void Register(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();

            for (int index = 0; index < types.Length; index++)
            {
                Type type = types[index];

                var attributes = type.GetCustomAttributes(typeof(InjectAttribute), false);

                foreach (var attribute in attributes)
                {
                    var injectAttribute = attribute as InjectAttribute;

                    if (injectAttribute != null)
                    {
                        if (injectAttribute.InjectType == InjectType.AsSelf)
                        {
                            Register(type, type, injectAttribute.LifeType);
                        }
                        else
                        {
                            Type[] serviceTypes = type.GetInterfacesForInjection();

                            foreach (var serviceType in serviceTypes)
                            {
                                Register(serviceType, type, injectAttribute.LifeType);
                            }
                        }
                    }
                }
            }
        }

        public void Register<TInterface, TImplementation>(LifeTypes lifeType = LifeTypes.Single)
        {
            Register(typeof(TInterface), typeof(TImplementation), lifeType);
        }

        public void Register<TInterface>(object instance, LifeTypes lifeType = LifeTypes.Single)
        {
            Type type = instance.GetType();

            Register(typeof(TInterface), type, lifeType);

            _instances[type] = instance;
        }

        public void Register(Type service, Type implementation, LifeTypes lifeType = LifeTypes.Single)
        {
            if (service.IsGenericType && service.FullName == null)
            {
                service = service.GetGenericTypeDefinition();
            }

            _registrations[service] = new Registration(implementation, lifeType);
        }

        public TInterface Resolve<TInterface>(params object[] parameters)
        {
            return (TInterface)Resolve(typeof(TInterface), parameters);
        }

        public object Resolve(Type registrationType, params object[] parameters)
        {
            Type typeToGet = null;

            var registration = _registrations[registrationType] as Registration;

            if (registration == null)
            {
                if (registrationType.IsGenericType)
                {
                    registration = _registrations[registrationType.GetGenericTypeDefinition()] as Registration;

                    if (registration == null)
                    {
                        throw new ArgumentNullException();
                    }

                    typeToGet = registration.Type.MakeGenericType(registrationType.GetGenericArguments().Take(registration.Type.GetGenericArguments().Length).ToArray());
                }
                else
                {
                    return null;
                }
            }
            else
            {
                typeToGet = registration.Type;
            }

            switch (registration.LifeType)
            {
                case LifeTypes.Single:
                default:
                    return ResolveSingleInstance(typeToGet, parameters);

                case LifeTypes.PerDependency:
                    return ResolvePerDependencyInstance(typeToGet, parameters);

                case LifeTypes.PerRequest:
                    return ResolvePerRequestInstance(typeToGet, parameters);
            }
        }

        protected object ResolveSingleInstance(Type registrationType, object[] parameters)
        {
            object instance = _instances[registrationType];

            if (instance == null)
            {
                instance = CreateInstance(registrationType, parameters);

                lock (LockObject)
                {
                    _instances[registrationType] = instance;
                }
            }

            return instance;
        }

        protected object ResolvePerDependencyInstance(Type registrationType, object[] parameters)
        {
            return CreateInstance(registrationType, parameters);
        }

        protected object ResolvePerRequestInstance(Type registrationType, object[] parameters)
        {
            if (HttpContext.Current == null)
            {
                return ResolveSingleInstance(registrationType, parameters);
            }

            object instance = HttpContext.Current.Items[registrationType];

            if (instance == null)
            {
                instance = HttpContext.Current.Items[registrationType] = CreateInstance(registrationType, parameters);
            }

            return instance;
        }

        protected object CreateInstance(Type type, params object[] additionalParameters)
        {
            var constructor = _constructors[type] as Constructor;

            if (constructor == null)
            {
                ConstructorInfo[] constructors = type.GetConstructors();

                ConstructorInfo currentConstruction = constructors.First();

                if (constructors.Length > 1)
                {
                    currentConstruction = type.GetConstructor(Type.EmptyTypes);
                }

                constructor = new Constructor(currentConstruction);

                lock (LockObject)
                {
                    _constructors[type] = constructor;
                }
            }

            var objects = new List<object>();

            for (int index = 0; index < constructor.Parameters.Length; index++)
            {
                object parameter = Resolve(constructor.Parameters[index].ParameterType);

                if (parameter != null)
                {
                    objects.Add(parameter);
                }
            }

            objects.AddRange(additionalParameters);

            return constructor.ConstructorInfo.Invoke(objects.ToArray());
        }
    }
}
