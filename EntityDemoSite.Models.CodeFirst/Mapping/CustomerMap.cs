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
	public class CustomerMap : EntityTypeConfiguration<Customer>
	{
		public CustomerMap()
		{
			// Primary Key
			this.HasKey(t => t.CustomerId);

			// Properties
			this.Property(t => t.FirstName)
				.HasMaxLength(50);

            // Properties
            this.Property(t => t.EmailAddress)
                .IsRequired()
                .HasMaxLength(50);
				
			this.Property(t => t.LastName)
				.IsRequired()
				.HasMaxLength(500);
				
			this.Property(t => t.Address)
				.IsRequired()
				.HasMaxLength(80);
				
			this.Property(t => t.City)
				.IsRequired()
				.HasMaxLength(100);
				
			this.Property(t => t.State)
				.IsRequired()
				.HasMaxLength(50);
				
			this.Property(t => t.Zip)
				.IsRequired()
				.HasMaxLength(20);
				
			this.Property(t => t.Timestamp)
				.IsRequired()
				.IsFixedLength()
				.HasMaxLength(8);
				
			// Table & Column Mappings
			this.ToTable("Customer");
			this.Property(t => t.CustomerId).HasColumnName("CustomerId");
			this.Property(t => t.FirstName).HasColumnName("FirstName");
			this.Property(t => t.LastName).HasColumnName("LastName");
			this.Property(t => t.Address).HasColumnName("Address");
			this.Property(t => t.City).HasColumnName("City");
			this.Property(t => t.State).HasColumnName("State");
			this.Property(t => t.Zip).HasColumnName("Zip");
			this.Property(t => t.Timestamp).HasColumnName("Timestamp");
		}
	}
}

