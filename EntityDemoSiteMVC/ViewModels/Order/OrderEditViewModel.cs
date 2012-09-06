using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace EntityDemoSiteMVC.ViewModels.Order
{
    public class OrderEditViewModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime? EstimatedShipDate { get; set; }
        public DateTime? ActualShipDate { get; set; }
        public int ShipTypeId { get; set; }
        [Display(Name = "Shipping Method")]
        [ReadOnly(true)]
        //[ScaffoldColumn(false)]
        public IEnumerable<SelectListItem> ShipTypes { get; set; }

    }
}