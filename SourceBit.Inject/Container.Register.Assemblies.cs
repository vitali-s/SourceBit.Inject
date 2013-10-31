using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SourceBit.Inject.RegistrationStrategies;

namespace SourceBit.Inject
{
    public sealed partial class Container
    {
        public void RegisterByAttributes(params string[] assembliesNames)
        {
            int length = assembliesNames.Length;

            var assemblies = new Assembly[length];

            for (int index = 0; index < length; index++)
            {
                assemblies[index] = Assembly.Load(assembliesNames[index]);
            }

            RegisterByAttributes(assemblies);
        }

        public void RegisterByAttributes(params Assembly[] assemblies)
        {
            int length = assemblies.Length;

            for (int index = 0; index < length; index++)
            {
                RegisterByAttributes(assemblies[index]);
            }
        }

        public void RegisterByAttributes(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();

            int length = types.Length;

            for (int index = 0; index < length; index++)
            {
                var type = types[index];

                if (!type.IsClass)
                {
                    continue;
                }

                if (type.IsAbstract)
                {
                    continue;
                }

                object[] attributes = type.GetCustomAttributes(typeof(InjectAttribute), true);

                if (attributes.Length != 1)
                {
                    continue;
                }

                var injectAttribute = attributes[0] as InjectAttribute;

                RegisterByAttribute(type, injectAttribute);
            }
        }

        public void RegisterByAttribute(Type type, InjectAttribute attribute)
        {
            var interfaces = new Type[] { };

            // Get as types
            if (attribute.InjectType == InjectType.AsInterface)
            {
                interfaces = type.GetInterfaces().Except(type.BaseType.GetInterfaces()).ToArray();

                if (interfaces.Length > 1)
                {
                    interfaces = interfaces.Except(interfaces.SelectMany(t => t.GetInterfaces())).ToArray();
                }

                if (interfaces.Length == 0)
                {
                    interfaces = type.BaseType.GetInterfaces();
                }
            }
            else if (attribute.InjectType == InjectType.AsSelf)
            {
                interfaces = new[] { type };
            }

            if (interfaces.Length == 0)
            {
                throw new Exception();
            }

            Register(type, interfaces, (int)attribute.LifeType);
        }

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
                Register(types[index], lifeType);
            }
        }

        public void Register(Type type, int lifeType)
        {
            Type[] abstractions = GetInterfacesForInjection(type);

            Register(type, abstractions, lifeType);
        }

        private Type[] GetInterfacesForInjection(Type type)
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
