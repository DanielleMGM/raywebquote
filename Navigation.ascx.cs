using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace MGM_Transformer
{
    public partial class Navigation : System.Web.UI.UserControl
    {

       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if logged out.
                string sPageCurrent = "";
                try
                {
                    sPageCurrent = Session["PageName"].ToString();
                }
                catch
                {
                    Response.Redirect("http://www.mgmtransformer.com");
                }

                int iAdmin = Convert.ToInt32(Session["Admin"]);

                // Testing
                //iAdmin = 0;

                FormatCurrentPage(sPageCurrent, iAdmin);
            }
        }

        protected void lnkbHome_Click(object sender, EventArgs e)
        {
            ProcessButton("Home");
        }

        protected void lnkbNewQuote_Click(object sender, EventArgs e)
        {
            // Start a new quote.
            Session["QuoteID"] = 0;
 
            ProcessButton("Quote");
        }

        protected void lnkbQuote_Click(object sender, EventArgs e)
        {
            ProcessButton("Quote");
        }

        protected void lnkbCustomer_Click(object sender, EventArgs e)
        {
            ProcessButton("Customers");
        }

        protected void lnkbStockPrices_Click(object sender, EventArgs e)
        {
            ProcessButton("StockPrices");
        }

        protected void lnkbReports_Click(object sender, EventArgs e)
        {
            ProcessButton("Reports");
        }

        protected void lnkbReps_Click(object sender, EventArgs e)
        {
            ProcessButton("AdminReps");
        }

        protected void lnkbMaintenance_Click(object sender, EventArgs e)
        {
            ProcessButton("AdminMaintenance");
        }

        /// <summary>
        /// Processes all button clicks.
        /// </summary>
        /// <param name="sTarget"></param>
        protected void ProcessButton(string sTarget)
        {
            lblError.Visible = false;

            // Prevent new quotes for inactive reps.
            if (sTarget == "Quote" && Session["QuoteID"].ToString() == "0")
            {
                bool bActive = (Session["Inactive"].ToString() == "0") ? true : false;
                if (!bActive)
                {
                    lblError.Text = "Inactive.  Cannot create a new quote.";
                    lblError.Visible = true;
                    // This is an ugly way to do it...
                    //System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('Inactive.  Cannot create a new quote.')</SCRIPT>");
                    return;
                }
            }
            
            Session["PageName"] = sTarget;

            Response.Redirect("~/" + sTarget + ".aspx");
        }

        /// <summary>
        /// Replace the current link with a label.
        /// </summary>
        public void FormatCurrentPage(string sPageCurrent, int iAdmin)
        {
            lnkbHome.Visible = (sPageCurrent == "Home") ? false : true;
            lnkbQuote.Visible = (sPageCurrent == "Email") ? true : false;
            lnkbCustomer.Visible = (sPageCurrent == "Customers") ? false : true;
            lnkbStockPrices.Visible = (sPageCurrent == "StockPrices") ? false : true;
            lnkbReports.Visible = (sPageCurrent == "Reports") ? false : true;
            lnkbReps.Visible = (sPageCurrent == "AdminReps" || iAdmin == 0) ? false : true;
            lnkbMaintenance.Visible = (sPageCurrent == "AdminMaintenance" || iAdmin == 0) ? false : true;

            lblHome.Visible = (sPageCurrent == "Home") ? true : false;
            lblQuote.Visible = (sPageCurrent == "Quote") ? true : false;
            lblEmail.Visible = (sPageCurrent == "Email") ? true : false;
            lblCustomer.Visible = (sPageCurrent == "Customers") ? true : false;
            lblStockPrices.Visible = (sPageCurrent == "StockPrices") ? true : false;
            lblReports.Visible = (sPageCurrent == "Reports") ? true : false;
            lblReps.Visible = (sPageCurrent == "AdminReps" && iAdmin == 1) ? true : false;
            lblMaintenance.Visible = (sPageCurrent == "AdminMaintenance" && iAdmin == 1) ? true : false;
        }

        protected void lnkbLogout_Click(object sender, EventArgs e)
        {
            Session["IsLoggedIn"] = 0;

            HttpCookie cookie = Response.Cookies["MGMLoggedIn"];
            cookie["LoggedIn"] = "False"; 
            
            Response.Redirect("http://www.mgmtransformer.com");
        }

        protected void lnkbDidYouKnow_Click(object sender, EventArgs e)
        {
            ProcessButton("Articles");
        }
    }
}