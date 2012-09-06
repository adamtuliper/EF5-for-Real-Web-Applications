using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using EntityDemoSite.DataAccess.Repositories;
using EntityDemoSite.DataAccess.Interfaces;
using EntityDemoSite.DataAccess;
using EntityDemoSite.Domain.Interfaces;

namespace EntityDemoSiteMVC
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // e.g. container.RegisterType<ITestService, TestService>();            
            container.RegisterType<ICustomerRepository, CustomerRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IOrderRepository, OrderRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IShipTypeRepository, ShipTypeRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IStateRepository, StateRepository>(new HierarchicalLifetimeManager());

            //Does not implement IDisposable so we do not need a HierarchicalLifetimeManager
            container.RegisterType<ICustomerValidator, ICustomerValidator>();

            container.RegisterControllers();

            return container;
        }
    }
}