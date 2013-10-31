using System;

namespace SourceBit.Inject.RegistrationStrategies
{
    /// <summary>
    /// Represents the options of the registered type in the IoC container.
    /// </summary>
    public class TypeRegistration : ITypeRegistration
    {
        private readonly Container _container;
        private readonly Type _type;
        private readonly Type[] _asTypes;

        public TypeRegistration(Container container, Type type, params Type[] asTypes)
        {
            _container = container;
            _type = type;
            _asTypes = asTypes;
        }

        public Type Type
        {
            get { return _type; }
        }

        public Type[] AsTypes
        {
            get { return _asTypes; }
        }

        public Container Container
        {
            get { return _container; }
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
