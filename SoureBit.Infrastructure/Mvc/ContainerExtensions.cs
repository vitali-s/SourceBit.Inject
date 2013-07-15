using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using SoureBit.Infrastructure.Inject;

namespace SoureBit.Infrastructure.Mvc
{
    public static class ContainerExtensions
    {
        public static void RegisterControllers(this Container container, Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t => typeof(IController).IsAssignableFrom(t) && t.Name.EndsWith("Controller"))
                .ToList();

            foreach (var type in types)
            {
                container.Register(type, type, LifeTypes.PerDependency);
            }
        }
    }
}
