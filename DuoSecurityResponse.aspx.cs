using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Duo;

namespace MGM_Transformer
{
    public partial class DuoSecurityResponse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // This page only gets called when the return from the DuoSecurity app is successful.
            // Its only function is to make sure that the user name returned is valid,
            // which should be 100% of the time.
            
            string sUserName = "";

            if (!IsPostBack)
            {
                var c = HttpContext.Current;
                string sSignResponse = c.Request["sig_response"];

                string sAKey = Session["DuoAKey"].ToString();
                string sIKey = Session["DuoIKey"].ToString();
                string sSKey = Session["DuoSKey"].ToString();

                // This should return the user's name.  If it fails, it returns Null.
                sUserName = Duo.Web.VerifyResponse(sIKey, sSKey, sAKey, sSignResponse).ToString();

                if (sUserName == "")
                {
                    // Alert the user that the login failed.
                    lblNotVerified.Visible = true;
                    Response.Redirect("DuoSecurityResponse.aspx");
                }
                else
                {
                    // Move on to the application.
                    Session["UserName"] = sUserName;
                    Response.Redirect("Home.aspx");
                }
            }
        }
    }
}