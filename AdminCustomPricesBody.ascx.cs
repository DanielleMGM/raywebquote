using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGM_Transformer
{
    public partial class AdminCustomPricesBody : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["Winding"] = "Aluminum";
            }
        }

        protected void rblWinding_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Winding"] = rblWinding.SelectedValue;
            dsCustomPrices.DataBind();
            gvCustomPrices.DataBind();
        }

        protected void gvCustomPrices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string onmouseoverStyle = "this.style.backgroundColor='lightyellow';style.cursor='hand'";
            string onmouseoutStyle = "this.style.backgroundColor='#@BackColor'";
            string rowBackColor = String.Empty;
            bool isGridEmpty = Convert.ToBoolean(ViewState["emptyGrid"]);

            if (e.Row.RowType == DataControlRowType.DataRow && isGridEmpty == false)
            {
                e.Row.Attributes.Add("onmouseover", onmouseoverStyle);
                e.Row.Attributes.Add("onmouseout", onmouseoutStyle.Replace("#@BackColor", rowBackColor));
            }
        }
    }
}