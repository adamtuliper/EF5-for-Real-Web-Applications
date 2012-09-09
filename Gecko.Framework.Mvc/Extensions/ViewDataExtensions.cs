using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;


namespace System.Web.Mvc
{
    public static class ViewDataExtensions
    {
        /// <summary>
        /// Gets the status messages from TempData and ViewData.
        /// When this is called, the status messages are cleared.
        /// </summary>
        /// <param name="viewData"></param>
        /// <param name="message"></param>
        public static HtmlString GetMessageSummary(this IDictionary<string, object> viewData)
        {

            //</div>", 
            //<label style="background-color: <%:  ViewData["JobStatusMessageColor"] %>">
            //<%: ViewData["StatusMessage"]%></label>

            List<string> messageItems = (List<string>)viewData["StatusMessages"];
            if (messageItems != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<br><div class=\"status-message\">\r\n<ul>");

                foreach (string item in messageItems)
                {
                    sb.Append(string.Format("<li>{0}</li>\r\n", item));
                }
                sb.Append("</ul>\r\n</div>");
                return new HtmlString(sb.ToString());
            }
            //Im assuming internally this is thread safe since its ViewData or TempData being used. it could be false but going with that for now : )
            viewData.Remove("StatusMessages");
            return new HtmlString("");
        }


        /// <summary>
        /// Adds a status message to ViewData, meant to be displayed on the _current_ request
        /// </summary>
        /// <param name="viewData">Since this is an extension method, this operates on TempData.AddStatusMessage</param>
        /// <param name="message">The message to be added to the collection</param>
        public static void AddStatusMessage(this ViewDataDictionary viewData, string message)
        {
            AddStatusMessageToDictionary(viewData, message);
        }
        
        /// <summary>
        /// Adds a status message to TempDate, meant to be displayed on the very _next_ request.
        /// </summary>
        /// <param name="viewData">Since this is an extension method, this operates on TempData.AddStatusMessage</param>
        /// <param name="message">The message to be added to the collection</param>
        public static void AddStatusMessage(this TempDataDictionary viewData , string message)
        {
            AddStatusMessageToDictionary(viewData, message);
        }


        /// <summary>
        /// Adds a message to be displayed to the user. This works with ViewData and TempData
        /// </summary>
        /// <param name="viewData"></param>
        /// <param name="message"></param>
        private static void AddStatusMessageToDictionary(this IDictionary<string, object> viewData, string message)
        {
            List<string> messageItems;
            if (viewData.ContainsKey("StatusMessages"))
            {

                messageItems = (List<string>)viewData["StatusMessages"];
            }
            else
            {
                messageItems = new List<string>(5);
                viewData["StatusMessages"] = messageItems;
            }

            messageItems.Add(message);

        }


    }
}