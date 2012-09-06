using System;
using System.ComponentModel.DataAnnotations;

namespace EntityDemoSite.Domain.Entities
{
	public class Order
	{
		public int OrderId { get; set; }
		public int CustomerId { get; set; }
		public decimal OrderTotal { get; set; }
		public Nullable<System.DateTime> EstimatedShipDate { get; set; }
		public Nullable<System.DateTime> ActualShipDate { get; set; }
		public int ShipTypeId { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual ShipType ShipType { get; set; }
	}
}

