using SourceBit.Inject.RegistrationStrategies;

namespace SourceBit.Inject.Tests.Data
{
    public static class CustomResolvingStrategy
    {
        public static void AsCustomResolvingStrategy(this ITypeRegistration typeRegistration)
        {
            var currentItemRegistration = typeRegistration as TypeRegistration;

            currentItemRegistration.Container.Register(currentItemRegistration, byte.MaxValue);
        }
    }
}
