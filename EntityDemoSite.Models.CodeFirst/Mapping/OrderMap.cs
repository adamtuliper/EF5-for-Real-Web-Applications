using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Common;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EntityDemoSite.Domain;
using EntityDemoSite.Domain.Entities;

namespace EntityDemoSite.Domain.Mapping
{
	public class OrderMap : EntityTypeConfiguration<Order>
	{
		public OrderMap()
		{
			// Primary Key
			this.HasKey(t => t.OrderId);

			// Properties
			// Table & Column Mappings
			this.ToTable("Order");
			this.Property(t => t.OrderId).HasColumnName("OrderId");
			this.Property(t => t.CustomerId).HasColumnName("CustomerId");
			this.Property(t => t.OrderTotal).HasColumnName("OrderTotal");
			this.Property(t => t.EstimatedShipDate).HasColumnName("EstimatedShipDate");
			this.Property(t => t.ActualShipDate).HasColumnName("ActualShipDate");
			this.Property(t => t.ShipTypeId).HasColumnName("ShipTypeId");

			// Relationships
            this.HasRequired(t => t.Customer)
				.WithMany(t => t.Orders)
				.HasForeignKey(d => d.CustomerId);

            this.HasRequired(t => t.ShipType);
				
		}
	}
}

