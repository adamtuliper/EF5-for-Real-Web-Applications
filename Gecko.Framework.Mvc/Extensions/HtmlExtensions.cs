using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace System.Web.Mvc
{

    public static class HtmlExtensions
    {
        //public static string ActionValidationSummary(this HtmlHelper html, string action)
        //{
        //    string currentAction = html.ViewContext.RouteData.Values["action"].ToString();

        //    //if (currentAction.ToLower() == action.ToLower())
        //    //    return html.ValidationExtensions.ValidationSummary(); // return html.ValidationSummary();

        //    return string.Empty;
        //}

        /// <summary>
        /// Looks for a value named "StatusMessage" in tempdata and in turn creates a pretty div to show a status message.
        /// This relies on the jQuery plugin purr, so include the purr script (jquery.purr.js) and note the images required in /scripts/jPurr below
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlString DisplayStatusMessage(this HtmlHelper html)
        {
            string statusMessage = (string)html.ViewContext.TempData["StatusMessage"];

            if (!string.IsNullOrEmpty((string)html.ViewContext.TempData["StatusMessage"]))
            {

                StringBuilder sb = new StringBuilder();
                sb.Append(@"<script type=""text/javascript"">");
                sb.Append("\r\nalert('bip');");

                sb.Append(@"var notice = '<div class=""notice"">'
                                  + '<div class=""notice-body"">'
                                      + '<img src=""" + UrlHelper.GenerateContentUrl("~/Scripts/jPurr/info.png", html.ViewContext.HttpContext) +
                                                     @""" alt="""" />'
                                      + '<h3>Edit Customer</h3>'
                                      + '<p>" + statusMessage + @"</p>'
                                  + '</div>'
                                  + '<div class=""notice-bottom"">'
                                  + '</div>'
                              + '</div>';");
                sb.Append("\r\n");
                sb.Append("try{");
                sb.Append("\r\n");
                sb.Append("$(notice).purr({usingTransparentPNG: true});");
                sb.Append("\r\n");
                sb.Append("}");
                sb.Append("\r\n");
                sb.Append("catch(err){");
                sb.Append("\r\n");
                //Errors would likely occur when the purr script is not included or is included AFTER this script is generated.
                sb.Append("alert('An error occured trying to show a status message. The error will show after this (its for diagnostic purposes only), however the status message was:" + statusMessage + "');");
                sb.Append("alert(err);");
                sb.Append("\r\n");
                sb.Append("}");
                sb.Append("\r\n");
                sb.Append("</script>\r\n");
                return new HtmlString(sb.ToString());

                //</text>
            }
            else
            {
                return new HtmlString("");
            }

        }
    }
}
