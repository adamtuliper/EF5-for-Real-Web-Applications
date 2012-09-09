using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gecko.Extensions
{
    public static class DeleteExtensions
    {
        /// <summary>
        /// Provides a link to use the new HttpMethodOverride to implement a delete verb.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="imageUrlPath"></param>
        /// <returns></returns>
        /// <example>
        /// <%=Html.DeleteLink("Delete", "Workouts", new { id = workout.Id }, "/images/remove-icon.png") %>
        /// </example>
        public static MvcHtmlString DeleteLink(this HtmlHelper helper, string actionName, string controllerName, object routeValues, string imageUrlPath)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = urlHelper.Action(actionName, controllerName, routeValues);

            var formTag = new TagBuilder("form");
            formTag.MergeAttribute("action", url);
            formTag.MergeAttribute("method", "post");
            var inputTag = new TagBuilder("input");
            inputTag.MergeAttribute("type", "image");
            inputTag.MergeAttribute("src", imageUrlPath);
            inputTag.MergeAttribute("alt", "Delete");

            formTag.InnerHtml = inputTag.ToString(TagRenderMode.SelfClosing) + helper.HttpMethodOverride(HttpVerbs.Delete);

            return MvcHtmlString.Create(formTag.ToString());
        }
    }
}