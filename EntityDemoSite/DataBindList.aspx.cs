using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EntityDemoSite.DataAccess;
using EntityDemoSite.Domain;
using EntityDemoSite.DataAccess.Repositories;
using EntityDemoSite.Domain.Entities;

namespace EntityDemoSite
{
    public partial class DataBindList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnDatabindOutsideOfContext_Click(object sender, EventArgs e)
        {

            IEnumerable<Customer> customers;
            using (var context = new EntityContext())
            {
                customers = context.Customers;//.Where(o=>o.FirstName.Contains("a"));
            }
            //Note intentional error
            customersView.DataSource = customers.ToArray();
            customersView.DataBind();
        }

        protected void btnDataBindInContext_Click(object sender, EventArgs e)
        {
            using (var context = new EntityContext())
            {
                //dont 
                //customersView.DataSource = from o in context.Customers select o;
                //do:
                
                customersView.DataSource = context.Customers.ToArray();
                customersView.DataBind();
            }
        }

        protected void btnDatabindRepository_Click(object sender, EventArgs e)
        {
            using (var repository = new CustomerRepository())
            {
                customersView.DataSource = repository.GetAll();
                customersView.DataBind();
            }
        }
    }
}