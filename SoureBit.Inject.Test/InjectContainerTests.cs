using SoureBit.Infrastructure.Inject;
using Xunit;

namespace SoureBit.Inject.Test
{
    public class InjectContainerTests
    {
        [Fact]
        public void Register_ForAssembliesList_ShouldNotBeCalled()
        {
            // Assemblies names
            InjectContainer.Container.Register();
        }

        [Fact]
        public void Register_ForAssembly()
        {
            // Assemblt
            InjectContainer.Register();
        }

        [Fact]
        public void Register_ForType()
        {
            // Type
            InjectContainer.Register();
        }
    }
}
