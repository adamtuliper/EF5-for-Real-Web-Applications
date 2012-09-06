using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EntityDemoSite.Domain;
using EntityDemoSite.Models.CodeFirst.Mapping;

namespace EntityDemoSite.Models.CodeFirst
{
	public class EntityFrameworkDemoContextOLD : DbContext
	{
        static EntityFrameworkDemoContextOLD()
		{
            Database.SetInitializer<EntityFrameworkDemoContextOLD>(null);
		}

		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<ShipType> ShipTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new CustomerMap());
			modelBuilder.Configurations.Add(new OrderMap());
			modelBuilder.Configurations.Add(new ShipTypeMap());
		}
	}
}

