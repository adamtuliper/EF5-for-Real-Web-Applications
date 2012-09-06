using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using EntityDemoSite.Domain.Entities;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using EntityDemoSite.DataAccess.Repositories;
using EntityDemoSiteMVC.ViewModels.Customer;
using EntityDemoSite.Domain;
using AutoMapper;
using Gecko.Framework.Mvc.ActionFilters;
using StackExchange.Profiling;

namespace EntityDemoSiteMVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MapEntityExceptionsToModelErrorsAttribute());
            filters.Add(new HandleConcurrencyExceptionAttribute());

        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal) { MiniProfiler.Start(); } //or any number of other checks, up to you 
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

        protected void Application_Start()
        {

            MiniProfilerEF.Initialize();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //var container = new UnityContainer().LoadConfiguration();
            //container.RegisterType<ICustomerRepository, CustomerRepository>();

            //If we want ALL types to go through the unity then we enable this.
            //DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            Bootstrapper.Initialise();

            //Automapper
            Mapper.CreateMap<Customer, CustomerCreateViewModel>();
            Mapper.CreateMap<Customer, CustomerEditViewModel>();

        }
    }
}