using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SourceBit.Inject.RegistrationStrategies;

namespace SourceBit.Inject
{
    public partial class Container
    {
        public ITypeRegistration Register(Type type, params Type[] asTypes)
        {
            var typeRegistration = new TypeRegistration(this, type, asTypes);

            return typeRegistration;
        }

        public ITypeRegistration Register<TImplementation, TAbstraction>()
            where TImplementation : class
            where TAbstraction : class
        {
            return Register(typeof(TImplementation), typeof(TAbstraction));
        }

        public void Register(TypeRegistration typeRegistration, int lifeType)
        {
            Register(typeRegistration.Type, typeRegistration.AsTypes, lifeType);
        }
    }
}
