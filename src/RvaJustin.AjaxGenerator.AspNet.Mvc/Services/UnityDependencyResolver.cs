using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;

namespace RvaJustin.AjaxGenerator.AspNet.Mvc.Services
{
    public class UnityDependencyResolver : IDependencyResolver, IServiceProvider
    {
        private readonly IUnityContainer container;
        private readonly IDependencyResolver resolver;
        
        public static UnityDependencyResolver Current => DependencyResolver.Current.GetUnity();

        public UnityDependencyResolver(IUnityContainer container, IDependencyResolver resolver)
        {
            this.container = container;
            this.resolver = resolver;

            this.container.RegisterInstance(this, new ContainerControlledLifetimeManager());
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch
            {
                return resolver.GetService(serviceType);
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch
            {
                return resolver.GetServices(serviceType);
            }
        }
    }
}
