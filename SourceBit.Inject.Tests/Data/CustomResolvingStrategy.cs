namespace SourceBit.Inject.Tests.Data
{
    public static class CustomResolvingStrategy
    {
        public static void AsCustomResolvingStrategy(this IRegistration registration)
        {
            var currentItemRegistration = registration as Registration;

            currentItemRegistration.ResolvingStrategyType = byte.MaxValue;
            currentItemRegistration.Register(currentItemRegistration);
        }
    }
}
