using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityDemoSite.DataAccess.Entities
{
	public class Customer
	{
	    public Customer()
		{
			this.Orders = new List<Order>();
		}

		public int CustomerId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
        [Timestamp()]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public byte[] Timestamp { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
	}
}

