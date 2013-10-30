using System;
using System.Reflection;

namespace SourceBit.Inject
{
    public interface IAssembliesRegistration
    {
        IAssembliesRegistration Where(Func<Type, bool> predicate);

        void AsSingleInstance();

        void AsPerDependencyInstance();
    }
}
