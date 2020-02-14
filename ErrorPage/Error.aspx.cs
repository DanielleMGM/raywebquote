using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGM_Transformer.ErrorPage
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.Request.Cookies["MGMLoggedIn"] != null)
            {
                if (Context.Request.Cookies["MGMLoggedIn"]["LoggedIn"] == "True")
                {
                    btnReturn.PostBackUrl = "~/Home.aspx";
                }
                else
                {
                    btnReturn.PostBackUrl = "~/Login.aspx";
                }
            }
            else
            {
                btnReturn.PostBackUrl = "~/Login.aspx";
            }
        }
    }
}