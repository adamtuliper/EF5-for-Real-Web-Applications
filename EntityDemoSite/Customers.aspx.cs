using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using EntityDemoSite.DataAccess.Repositories;

namespace EntityDemoSite
{
    public partial class Customers : System.Web.UI.Page
    {
        private DropDownList statesDropDownList;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Alternatively without the data source.
            //using (CustomerRepository repository = new CustomerRepository())
            //{
            //CustomersGridView.DataSource = repository.GetAll();
            //CustomersGridView.DataBind();
            //}
        }

        protected void cmbStates_Init(object sender, EventArgs e)
        {
            statesDropDownList = sender as DropDownList;
        }

        protected void CustomersGridView_RowEditing(object sender, GridViewUpdateEventArgs e)
        {

        }
        protected void CustomersGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void CustomersGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void CustomerObjectDataSource_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                CheckForEfExceptions(e, "delete");
            }
        }

        protected void CustomerObjectDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                CheckForEfExceptions(e, "update");
            }
        }

        private void CheckForEfExceptions(ObjectDataSourceStatusEventArgs e, string function)
        {
            if (e.Exception.InnerException is DbUpdateConcurrencyException)
            {
                var concurrencyExceptionValidator = new CustomValidator
                                                        {
                                                            IsValid = false,
                                                            ErrorMessage =
                                                                "The record you attempted to edit or delete was modified by another " +
                                                                "user after you got the original value. The edit or delete operation was canceled " +
                                                                "and the other user's values have been displayed so you can " +
                                                                "determine whether you still want to edit or delete this record."
                                                        };
                Page.Validators.Add(concurrencyExceptionValidator);
                e.ExceptionHandled = true;
            }
            else if (e.Exception.InnerException is DbEntityValidationException)
            {
                var concurrencyExceptionValidator = new CustomValidator();
                concurrencyExceptionValidator.IsValid = false;
                StringBuilder errors = new StringBuilder();
                foreach (var err in ((DbEntityValidationException)e.Exception.InnerException).EntityValidationErrors)
                {
                    foreach (var msg in err.ValidationErrors)
                    {
                        var validator = new CustomValidator();
                        validator.IsValid = false;
                        validator.ErrorMessage = msg.ErrorMessage;
                        Page.Validators.Add(validator);
                        e.ExceptionHandled = true;
                    }
                }
            }
        }
    }
}