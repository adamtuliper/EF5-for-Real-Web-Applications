using System;
using System.Collections.Generic;

namespace EntityDemoSite.DataAccess.Entities
{
	public class Order
	{
		public int OrderId { get; set; }
		public Nullable<int> CustomerId { get; set; }
		public Nullable<decimal> OrderTotal { get; set; }
		public Nullable<System.DateTime> EstimatedShipDate { get; set; }
		public Nullable<System.DateTime> ActualShipDate { get; set; }
		public virtual Customer Customer { get; set; }
	}
}

