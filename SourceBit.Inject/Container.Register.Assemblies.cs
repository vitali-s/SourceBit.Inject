using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SourceBit.Inject.RegistrationStrategies;

namespace SourceBit.Inject
{
    public partial class Container
    {
        public IAssembliesRegistration Register(params Assembly[] assemblies)
        {
            var assembliesRegistration = new AssembliesRegistration(this, assemblies);

            return assembliesRegistration;
        }

        public void Register(AssembliesRegistration assembliesRegistration, int lifeType)
        {
            int count = assembliesRegistration.Assemblies.Length;
            int filtersCount = assembliesRegistration.Filters.Count;

            for (int index = 0; index < count; index++)
            {
                Assembly assembly = assembliesRegistration.Assemblies[index];

                IEnumerable<Type> types = assembly.GetTypes();

                Func<Type, bool> action = type => type.IsClass && !type.IsAbstract;

                for (int filterIndex = 0; filterIndex < filtersCount; filterIndex++)
                {
                    action += assembliesRegistration.Filters[filterIndex];
                }

                types = types.Where(action);

                Register(types.ToList(), lifeType);
            }
        }

        public void Register(List<Type> types, int lifeType)
        {
            int typesCount = types.Count;

            for (int index = 0; index < typesCount; index++)
            {
                Register(types[index], 0);
            }
        }

        public void Register(Type type, int lifeType)
        {
            Type[] abstractions = GetInterfacesForInjection(type);

            Register(type, abstractions, lifeType);
        }

        protected virtual Type[] GetInterfacesForInjection(Type type)
        {
            Type[] interfaces = type.GetInterfaces().Except(type.BaseType.GetInterfaces()).ToArray();

            if (interfaces.Length > 1)
            {
                interfaces = interfaces.Except(interfaces.SelectMany(t => t.GetInterfaces())).ToArray();
            }

            if (interfaces.Length == 0)
            {
                interfaces = type.BaseType.GetInterfaces();
            }

            if (interfaces.Length == 0)
            {
                interfaces = new[] { type };
            }

            for (int index = 0; index < interfaces.Length; index++)
            {
                var interfaceType = interfaces[index];

                if (interfaceType.IsInterface && interfaceType.IsGenericType)
                {
                    interfaces[index] = interfaceType.GetGenericTypeDefinition();
                }
            }

            return interfaces;
        }
    }
}
