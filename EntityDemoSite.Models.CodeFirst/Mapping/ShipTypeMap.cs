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
	public class ShipTypeMap : EntityTypeConfiguration<ShipType>
	{
		public ShipTypeMap()
		{
			// Primary Key
			this.HasKey(t => t.ShipTypeId);

			// Properties
			this.Property(t => t.ShipName)
				.IsRequired()
				.HasMaxLength(100);
				
			// Table & Column Mappings
			this.ToTable("ShipType");
			this.Property(t => t.ShipTypeId).HasColumnName("ShipTypeId");
			this.Property(t => t.ShipName).HasColumnName("ShipName");
		}
	}
}

