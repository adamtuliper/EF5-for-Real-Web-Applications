using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using EntityDemoSite.Domain;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Configuration;
using EntityDemoSite.Domain.Entities;
using Microsoft.Practices.Unity;
using EntityDemoSite.Domain.Mapping;

namespace EntityDemoSite.DataAccess
{
    public class EntityContext : DbContext
    {
        public EntityContext()
        {
            this.Configuration.ValidateOnSaveEnabled = true;
        }

        static EntityContext()
        {
            //for example to create a db- this is in the static initializer so 
            //only runs when the app domain loads.
            //Database.SetInitializer(new CreateDatabaseIfNotExists<EntityContext>());

            Database.SetInitializer<EntityContext>(null);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShipType> ShipTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //deprecated in 4.3 modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new OrderMap());
        }
    }

}

