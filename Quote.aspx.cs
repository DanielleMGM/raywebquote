using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace MGM_Transformer
{
    public partial class Quote : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string showMessage = Session["QuoteCopy"].ToString();
                                                
            if (String.IsNullOrEmpty(showMessage) == false && showMessage != "0")
                ClientScript.RegisterClientScriptBlock(GetType(), "MessageBox", "alert('" + showMessage + "')", true);
 
        }
    }
}