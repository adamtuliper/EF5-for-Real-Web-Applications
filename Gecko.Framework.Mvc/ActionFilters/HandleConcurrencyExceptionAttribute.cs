using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using System.Data.Entity.Infrastructure;

namespace Gecko.Framework.Mvc.ActionFilters
{
    /// <summary>
    /// Author: Adam Tuliper
    /// adam.tuliper@gmail.com
    /// completedevelopment.blogspot.com
    /// www.secure-coding.com
    /// Use freely, just please retain original credit.
    /// 
    /// This attribute attempts to intercept DbUpdateConcurrencyException to write out original/new values
    /// to the screen for the user to review.
    /// It assumes the following:
    /// 1. There is a [Timestamp] attribute on an entity framework model property
    /// 2. The only differences that we care about from the posted data to the record currently in the database are 
    /// only yhe model state field. We do not have access to a model at this point, as an exception was raised so there was no
    /// return View(model) that we have a model to process from.
    /// As such, we have to look at the fields in the modelstate and try to find matching fields on the entity and then display the differences.
    /// This may not work in all cases. 
    /// This class will look at your model to get the property names. It will then check your
    /// Entities current values vs. db values for these property names.
    /// The behavior can be changed.
    /// </summary>
    public class HandleConcurrencyExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        private PropertyMatchingMode _propertyMatchingMode;
        /// <summary>
        /// This defines when the concurrencyexception happens, 
        /// </summary>
        public enum PropertyMatchingMode
        {
            /// <summary>
            /// Uses only the field names in the model to check against the entity. This option is best when you are using 
            /// View Models with limited fields as opposed to an entity that has many fields. The ViewModel (or model) field names will
            /// be used to check current posted values vs. db values on the entity itself.
            /// </summary>
            UseViewModelNamesToCheckEntity = 0,
            /// <summary>
            /// Use any non-matching value fields on the entity (except timestamp fields) to add errors to the ModelState.
            /// </summary>
            UseEntityFieldsOnly = 1,
            /// <summary>
            /// Tells the filter to not attempt to add field differences to the model state.
            /// This means the end user will not see the specifics of which fields caused issues
            /// </summary>
            DontDisplayFieldClashes = 2
        }


        public HandleConcurrencyExceptionAttribute()
        {
            _propertyMatchingMode = PropertyMatchingMode.UseViewModelNamesToCheckEntity;
        }

        public HandleConcurrencyExceptionAttribute(PropertyMatchingMode propertyMatchingMode)
        {
            _propertyMatchingMode = propertyMatchingMode;
        }


        /// <summary>
        /// The main method, called by the mvc runtime when an exception has occured.
        /// This must be added as a global filter, or as an attribute on a class or action method.
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled && filterContext.Exception is DbUpdateConcurrencyException)
            {
                //Get original and current entity values
                DbUpdateConcurrencyException ex = (DbUpdateConcurrencyException)filterContext.Exception;
                var entry = ex.Entries.Single();
                //problems with ef4.1/4.2 here because of context/model in different projects.
                //var databaseValues = entry.CurrentValues.Clone().ToObject();
                //var clientValues = entry.Entity;
                //So - if using EF 4.1/4.2 you may use this workaround
                var clientValues = entry.CurrentValues.Clone().ToObject();
                entry.Reload();
                var databaseValues = entry.CurrentValues.ToObject();

                List<string> propertyNames;

                filterContext.Controller.ViewData.ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                        + "was modified by another user after you got the original value. The "
                        + "edit operation was canceled and the current values in the database "
                        + "have been displayed. If you still want to edit this record, click "
                        + "the Save button again to cause your changes to be the current saved values.");
                PropertyInfo[] entityFromDbProperties = databaseValues.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);

                if (_propertyMatchingMode == PropertyMatchingMode.UseViewModelNamesToCheckEntity)
                {
                    //We dont have access to the model here on an exception. Get the field names from modelstate:
                    propertyNames = filterContext.Controller.ViewData.ModelState.Keys.ToList();
                }
                else if (_propertyMatchingMode == PropertyMatchingMode.UseEntityFieldsOnly)
                {
                    propertyNames = databaseValues.GetType().GetProperties(BindingFlags.Public).Select(o => o.Name).ToList();
                }
                else
                {
                    filterContext.ExceptionHandled = true;
                    UpdateTimestampField(filterContext, entityFromDbProperties, databaseValues);
                    filterContext.Result = new ViewResult() { ViewData = filterContext.Controller.ViewData };
                    return;
                }



                UpdateTimestampField(filterContext, entityFromDbProperties, databaseValues);

                //Get all public properties of the entity that have names matching those in our modelstate.
                foreach (var propertyInfo in entityFromDbProperties)
                {

                    //If this value is not in the ModelState values, don't compare it as we don't want
                    //to attempt to emit model errors for fields that don't exist.

                    //Compare db value to the current value from the entity we posted.

                    if (propertyNames.Contains(propertyInfo.Name))
                    {
                        if (propertyInfo.GetValue(databaseValues, null) != propertyInfo.GetValue(clientValues, null))
                        {
                            var currentValue = propertyInfo.GetValue(databaseValues, null);
                            if (currentValue == null || string.IsNullOrEmpty(currentValue.ToString()))
                            {
                                currentValue = "Empty";
                            }

                            filterContext.Controller.ViewData.ModelState.AddModelError(propertyInfo.Name, "Current value: "
                                 + currentValue);
                        }
                    }

                    //TODO: hmm.... how can we only check values applicable to the model/modelstate rather than the entity we saved?
                    //The problem here is we may only have a few fields used in the viewmodel, but many in the entity
                    //so we could have a problem here with that.
                    //object o = propertyInfo.GetValue(myObject, null);
                }

                filterContext.ExceptionHandled = true;

                filterContext.Result = new ViewResult() { ViewData = filterContext.Controller.ViewData };
            }
        }



        /// <summary>
        /// Searches the database loaded entity values for a field that has a [Timestamp] attribute.
        /// It then writes a string version of ther byte[] timestamp out to modelstate, assuming 
        /// we have a timestamp field on the page that caused the concurrency exception.
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="entityFromDbProperties"></param>
        /// <param name="databaseValues"></param>
        private void UpdateTimestampField(ExceptionContext filterContext, PropertyInfo[] entityFromDbProperties, object databaseValues)
        {
            foreach (var propertyInfo in entityFromDbProperties)
            {
                var attributes = propertyInfo.GetCustomAttributesData();

                //If this is a timestamp field, we need to set the current value.
                foreach (CustomAttributeData attr in attributes)
                {
                    if (typeof(System.ComponentModel.DataAnnotations.TimestampAttribute).IsAssignableFrom(attr.Constructor.DeclaringType))
                    {
                        //This currently works only with byte[] timestamps. You can use dates as timestampts, but support is not provided here.
                        byte[] timestampValue = (byte[])propertyInfo.GetValue(databaseValues, null);
                        //we've found the timestamp. Add it to the model.
                        //filterContext.Controller.ViewData.ModelState.Add(propertyInfo.Name, );
                        //new ModelState{ Value= propertyInfo.GetValue(databaseValues,null) }

                        filterContext.Controller.ViewData.ModelState.Add(propertyInfo.Name, new ModelState());
                        filterContext.Controller.ViewData.ModelState.SetModelValue(propertyInfo.Name,
                            new ValueProviderResult(Convert.ToBase64String(timestampValue), Convert.ToBase64String(timestampValue), null));
                        break;
                    }
                }

            }
        }

    }
}
