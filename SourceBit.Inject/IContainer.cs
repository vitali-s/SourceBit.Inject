using System;

namespace SourceBit.Inject
{
    public interface IContainer
    {
        ITypeRegistration Register(Type type, params Type[] asTypes);

        ITypeRegistration Register<TImplementation, TAbstraction>() where TImplementation : class where TAbstraction : class;

        TAbstraction Resolve<TAbstraction>() where TAbstraction : class;

        object Resolve(Type type);
    }
}
