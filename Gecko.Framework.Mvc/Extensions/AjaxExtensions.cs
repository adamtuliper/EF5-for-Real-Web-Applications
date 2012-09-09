using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc.Html;


namespace System.Web.Mvc
{
    public class AjaxDialogOptions
    {
        public AjaxDialogOptions()
        {
            URL = "";
            LoadingDivId = "";
            ErrorMessage = "Sorry but there was an error retrieving the data. Please re-try.";
            ShowErrorDetails = false;
            LogErrorToServer = false;
            ShowSaveButton = true;
            ShowCloseButton = true;
            DialogTitle = "";
            Width = 450;
            Height = 550;
            LinkText = "View";
            
        }

        public string URL;
        public string LoadingDivId;
        /// <summary>
        /// The error message to show when there has been an error. If one is not specified, a default is chosen.
        /// </summary>
        public string ErrorMessage;
        /// <summary>
        /// Shows the complete error to the client.
        /// </summary>
        public bool ShowErrorDetails;

        /// <summary>
        /// Will use ajax to attempt to send the error text to the server.
        /// </summary>
        public bool LogErrorToServer;
        public bool ShowSaveButton;
        public bool ShowCloseButton;
        public string DialogTitle;
        public int Width;
        public int Height;
        /// <summary>
        /// The text to show for the link to bring up this dialog.
        /// </summary>
        public string LinkText = "View";
        /// <summary>
        /// This is the id of the form that must be used to assign the dialog to. jQuery pops the dialogs out of its containing form so we need to put it back in.
        /// </summary>
        public string FormId;
    }

    public static class AjaxExtensions
    {

        /// <summary>
        /// 
        /// This requires
        /// </summary>
        /// <param name="viewData"></param>
        /// <param name="message"></param>
        public static HtmlString CreatejQueryDialog(this AjaxHelper ajaxHelper, AjaxDialogOptions dialogOptions)
        {
            //we cant have this as an MvcForm, since this likely is in a grid and we dont want forms all over. we also cant emit out a single item to the dom either.
            string href = string.Format("<a href=\"#\" onclick=\"showJQueryDialogForAction('{0}','{1}','{2}',{3},{4},{5},{6}, '{7}'); return false;\">{8}</a>",
                dialogOptions.LoadingDivId,
                dialogOptions.URL,
                dialogOptions.DialogTitle,
                dialogOptions.Width,
                dialogOptions.Height,
                dialogOptions.ShowSaveButton.ToString().ToLower(),
                dialogOptions.ShowCloseButton.ToString().ToLower(),
                dialogOptions.FormId,
                dialogOptions.LinkText
                );
            return new HtmlString(href);
        }

    }
}