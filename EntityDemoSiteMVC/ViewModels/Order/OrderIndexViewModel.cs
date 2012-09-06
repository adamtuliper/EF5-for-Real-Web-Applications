using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntityDemoSiteMVC.ViewModels.Order
{
    public class OrderIndexViewModel
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public decimal? OrderTotal { get; set; }
        public DateTime? EstimatedShipDate { get; set; }
        public DateTime? ActualShipDate { get; set; }
        public string ShipName { get; set; }
    }
}