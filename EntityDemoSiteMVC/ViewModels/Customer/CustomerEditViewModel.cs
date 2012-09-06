using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EntityDemoSiteMVC.ViewModels.Customer
{
    public class CustomerEditViewModel
    {
        public int CustomerId { get; set; }
        [Required()]
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public byte[] Timestamp { get; set; }
    }
}