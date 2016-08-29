using Microsoft.Practices.Unity;
using ServiceStack.Configuration;

namespace Mitto.SS.SMS.Middleware.Core
{
    /// <summary>
    /// Inversion-of-Controls adaptor for Service Stack Dependency injection container
    /// </summary>
    public class UnityIocAdapter : IContainerAdapter
    {
        private readonly IUnityContainer container;

        public UnityIocAdapter(IUnityContainer container)
        {
            this.container = container;
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public T TryResolve<T>()
        {
            if (container.IsRegistered<T>())
            {
                return container.Resolve<T>();
            }

            return default(T);
        }
    }
}
