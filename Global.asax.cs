using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Windows.Forms;

namespace MGM_Transformer
{
    public class Global : System.Web.HttpApplication
    {
        Quotes q = new Quotes();

        protected void Session_Start(object sender, EventArgs e)
        {

            string sMsg = "Message when starting session.";
            q.AddToTestLog(sMsg);

            try 
            {
                if (Session["RepDistributorID"] == null)
                {
                    Session["Admin"] = 0;
                    Session["Internal"] = 0;
                    Session["RepID"] = 0;
                    Session["QuoteId"] = 0;
                    Session["RepDistributorID"] = 0;               // Used for pricing.
                }
            }
            catch (Exception ex)
            {
                sMsg = "Error message when starting session: " + ex.Message;
                q.AddToTestLog(sMsg);
                
                // Response.Write("Exception occurred: " + ex + ".");
            }
        }

        protected void Application_BeginRequest()
        {
            //Quotes q = new Quotes();

            //string sMsg = "Message when beginning request.";
            //q.AddToTestLog(sMsg);

            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
            }
            catch (Exception ex)
            {
                //sMsg = "Error message when beginning request: " + ex.Message;
                //q.AddToTestLog(sMsg);
                return;
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            string sMsg = "Error when starting application.";
            q.AddToTestLog(sMsg);

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}