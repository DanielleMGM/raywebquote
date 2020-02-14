using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace MGM_Transformer
{
    public partial class MGM : System.Web.UI.MasterPage
    {
        //Public values here can be late-bound to javascript in the ASPX page.
        public int iWarningTimeoutInMilliseconds;
        public int iSessionTimeoutInMilliseconds;
        public string sTargetURLForSessionTimeout;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Context.Session != null)
            {
                if (Session.IsNewSession)
                {
                    HttpCookie newSessionIdCookie = Request.Cookies["ASP.NET_SessionId"];
                    if (newSessionIdCookie != null)
                    {
                        string newSessionIdCookieValue = newSessionIdCookie.Value;
                        if (newSessionIdCookieValue != string.Empty)
                        {
                            // The means Session was timed out and new session was started.
                            Session["IsTimeout"] = 1;
                            Response.Redirect("http://www.mgmtransformer.com");

                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sTargetURLForSessionTimeout = "Login.aspx";
            int iNumberOfMinutesBeforeSessionTimeoutToWarnUser = 1;

            // ===============================================================
            // Client side scripts.
            // ===============================================================
            // Define the name and type of the client scripts on the page.
            //String sStartup = "startup";
            //Type csType = this.GetType();

            //// Get a ClientScriptManager reference from the Page class.
            //ClientScriptManager cs = Page.ClientScript;

            //// Check to see if the startup script is already registered.
            //if (!cs.IsStartupScriptRegistered(csType, sStartup))
            //{
            //    String sStartupFunc = "myStartup();";
            //    cs.RegisterStartupScript(csType, sStartup, sStartupFunc);
            //}

            // ===============================================================



            
            
            
            
            
            
            
            
            
            
            
            // Get the sessionState timeout from web.config.
            int iSessionTimeoutInMinutes = Session.Timeout;

            int iWarningTimeoutInMinutes = iSessionTimeoutInMinutes - iNumberOfMinutesBeforeSessionTimeoutToWarnUser;

            iWarningTimeoutInMilliseconds = iWarningTimeoutInMinutes * 60 * 1000;

            iSessionTimeoutInMilliseconds = iSessionTimeoutInMinutes * 60 * 1000;

            if (!this.IsPostBack)
            {
                // Hide warning message.
                divSessionTimeoutWarning.Style.Add("display", "none");
            }
        }
        protected void btnContinueWorking_Click(object sender, EventArgs e)
        {
            // Do nothing.  But the Session will be refreshed as a result of
            // this method being called, which is its entire purpose.
        }
    }
}