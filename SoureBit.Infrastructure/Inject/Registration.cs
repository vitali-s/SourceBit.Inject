using System;

namespace SoureBit.Infrastructure.Inject
{
    /// <summary>
    /// Represents the options of the registered type in the IoC container.
    /// </summary>
    internal class Registration
    {
        private readonly Type _type;
        private readonly LifeTypes _lifeType;

        public Registration(Type type, LifeTypes lifeType)
        {
            _type = type;
            _lifeType = lifeType;
        }

        public Type Type
        {
            get { return _type; }
        }

        public LifeTypes LifeType
        {
            get { return _lifeType; }
        }
    }

}
