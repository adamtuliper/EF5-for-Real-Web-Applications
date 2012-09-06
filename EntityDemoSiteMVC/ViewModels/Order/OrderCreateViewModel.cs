using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityDemoSite.Domain;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EntityDemoSiteMVC.ViewModels.Order
{
    public class OrderCreateViewModel
    {
        public int CustomerId { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime? EstimatedShipDate { get; set; }
        public DateTime? ActualShipDate { get; set; }
        public int ShipTypeId { get; set; }

        [ReadOnly(true)]
        public IEnumerable<SelectListItem> Customers { get; set; }

        [Display(Name = "Shipping Method")]
        //[ReadOnly(true)]
        public IEnumerable<SelectListItem> ShipTypes {get;set;}

    }
}