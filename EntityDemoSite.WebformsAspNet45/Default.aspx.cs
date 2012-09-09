using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EntityDemoSite.DataAccess.Repositories;
using WebApplication1.ViewModels.Customer;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var repository = new CustomerRepository())
            {
                //Load it
                var customers = repository.GetAll();

                //Map it to view model
                var customerViewModels = customers.Select(c => new CustomerViewModel
                {
                    CustomerId = c.CustomerId,  
                    Address = c.Address,
                    City = c.City,
                    State = c.State,
                    FirstName = c.FirstName,
                    LastName = c.LastName
                }).ToArray();
                
                //Bind it. BAM
                GridView1.DataSource = customerViewModels;
                GridView1.DataBind();
            }

        }
    }
}