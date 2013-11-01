using System;

namespace SourceBit.Inject
{
    /// <summary>
    /// Registers the class in the Dependency Injection container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class InjectAttribute : Attribute
    {
        private readonly InjectType _injectType;
        private readonly LifeTypes _lifeType;

        public InjectAttribute(LifeTypes lifetype = LifeTypes.Single, InjectType injectType = InjectType.AsInterface)
        {
            _lifeType = lifetype;
            _injectType = injectType;
        }

        public InjectAttribute(InjectType injectType)
            : this(LifeTypes.Single, InjectType.AsSelf)
        {
        }

        public InjectType InjectType
        {
            get { return _injectType; }
        }

        public LifeTypes LifeType
        {
            get { return _lifeType; }
        }
    }
}
