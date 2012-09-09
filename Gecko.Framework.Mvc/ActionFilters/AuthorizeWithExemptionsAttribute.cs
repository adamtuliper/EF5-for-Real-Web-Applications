using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security;

namespace Gecko.Framework.Mvc.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public class AuthorizeWithExemptionsAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            ActionDescriptor action = filterContext.ActionDescriptor;
            bool IsUnsecured = action.GetCustomAttributes(typeof(UnsecuredActionAttribute), true).Count() > 0;


            //If doesn't have UnsecuredActionAttribute - then do the authorization
            filterContext.HttpContext.SkipAuthorization = IsUnsecured;

            base.OnAuthorization(filterContext);
        }
    }
}