using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Data.Entity.Validation;

namespace Gecko.Framework.Mvc.ActionFilters
{
    /// <summary>
    /// Author: Adam Tuliper
    /// adam.tuliper@gmail.com
    /// completedevelopment.blogspot.com
    /// www.secure-coding.com
    /// Use freely, just please retain original credit.
    /// 
    /// This filter will add errors to ModelState when an entity fails validation and raises DbEntityValidationException
    /// The properties in the entity should share the same name as the ViewModel properties otherwise
    /// the errors will show up in the validation summary as 'general' errors not assigned to a property
    /// Also note in MVC4 this IS NOT REQUIRED as this is finally handled by the framework then.
    /// </summary>
    public class MapEntityExceptionsToModelErrorsAttribute : FilterAttribute, IExceptionFilter
    {

        public MapEntityExceptionsToModelErrorsAttribute()
        {
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled && filterContext.Exception is DbEntityValidationException)
            {
                var model = filterContext.Controller.ViewData.Model;

                foreach (var error in ((DbEntityValidationException)filterContext.Exception).EntityValidationErrors.First().ValidationErrors)
                {
                    //If the error does not exist in the modelstate, then we will simply add it as a general validation message.
                    var keys = filterContext.Controller.ViewData.ModelState.Keys;
                    if (string.IsNullOrEmpty(error.PropertyName))
                    {
                        filterContext.Controller.ViewData.ModelState.AddModelError("", error.ErrorMessage);
                    }
                    else if (filterContext.Controller.ViewData.ModelState[error.PropertyName].Errors.Count == 0)
                    {
                        filterContext.Controller.ViewData.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    else
                    {
                        //Ensure this same message doesn't already exist. If ModelState is detected to have failed,
                        //for instance from a [Required] field, then we don't want to re-add the error. However if we have
                        //a more serious validation error, possibly stemming from some other data validation maybe not related to 
                        //our viewmodel (invalid dat in db, or some other code error bypassing validations)
                        //then we want to add the error. This can happen on Save() and not caught in modelstate.
                        bool found = false;
                        foreach (var errorItem in filterContext.Controller.ViewData.ModelState[error.PropertyName].Errors)
                        {
                            if (errorItem.ErrorMessage == error.ErrorMessage)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            filterContext.Controller.ViewData.ModelState.AddModelError("", error.ErrorMessage);
                        }
                    }
                }
                //No need to have other exception filters run
                filterContext.ExceptionHandled = true;

                filterContext.Result = new ViewResult() { ViewData = filterContext.Controller.ViewData };
            }
        }
    }
}
