using System;
using System.Collections.Generic;
using System.Reflection;

namespace SourceBit.Inject.RegistrationStrategies
{
    public class AssembliesRegistration : IAssembliesRegistration
    {
        private readonly Container _container;
        private readonly Assembly[] _assemblies;
        private readonly List<Func<Type, bool>> _filters;

        public AssembliesRegistration(Container container, Assembly[] assemblies)
        {
            _container = container;
            _assemblies = assemblies;
            _filters = new List<Func<Type, bool>>();
        }

        public Assembly[] Assemblies
        {
            get { return _assemblies; }
        }

        public List<Func<Type, bool>> Filters
        {
            get { return _filters; }
        }

        public IAssembliesRegistration Where(Func<Type, bool> predicate)
        {
            _filters.Add(predicate);

            return this;
        }

        public void AsSingleInstance()
        {
            _container.Register(this, (int)LifeTypes.Single);
        }

        public void AsPerDependencyInstance()
        {
            _container.Register(this, (int)LifeTypes.PerDependency);
        }
    }
}
