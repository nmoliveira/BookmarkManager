using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace BookmarkManager.Helpers
{
    public class UnityDependencyResolver: IDependencyResolver
    {
        private readonly IUnityContainer _Container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            _Container = container;
        }

        public object GetService(Type serviceType)
        {
            if (!_Container.IsRegistered(serviceType) && (serviceType.IsAbstract || serviceType.IsInterface))
                return null;

            return _Container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _Container.ResolveAll(serviceType);
        }
    }
}