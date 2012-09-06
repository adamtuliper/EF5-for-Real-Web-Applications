using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EntityDemoSite
{
    public partial class CustomerAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void CustomerObjectDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                CheckForEfExceptions(e, "update");
            }
        }

        private void CheckForEfExceptions(ObjectDataSourceStatusEventArgs e, string function)
        {
            if (e.Exception.InnerException is OptimisticConcurrencyException)
            {
                var concurrencyExceptionValidator = new CustomValidator();
                concurrencyExceptionValidator.IsValid = false;
                concurrencyExceptionValidator.ErrorMessage =
                    "The record you attempted to edit or delete was modified by another " +
                    "user after you got the original value. The edit or delete operation was canceled " +
                    "and the other user's values have been displayed so you can " +
                    "determine whether you still want to edit or delete this record.";
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