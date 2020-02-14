using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace MGM_Transformer
{
    public partial class HistoryBody : System.Web.UI.UserControl
    {
        int iRepID = 0;
        int iRepDistributorID = 0;
        RepObject r = new RepObject();
        string _QuoteName;

        public string QuoteName
        {
            get { return _QuoteName; }
            set { _QuoteName = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                iRepID = Convert.ToInt32(Session["RepID"]);
                iRepDistributorID = Convert.ToInt32(Session["RepDistributorID"]);

                txtRepDistributorID.Text = iRepDistributorID.ToString();
                txtCustomerID.Text = "0";

                LoadDistributors(iRepID);
                LoadCompanies(iRepDistributorID);

                LoadQuotes();
            }
        }

        protected void LoadDistributors(int RepID)
        {
            ddlDistributor.Items.Clear();

            DataTable dt = r.Distributors(RepID);
            ddlDistributor.DataTextField = "Distributor";
            ddlDistributor.DataValueField = "RepID";

            // Testing.
            DataRow dr = dt.NewRow();
            dr["Distributor"] = "GoTToGo Electric";
            dr["RepID"] = 64;
            dt.Rows.InsertAt(dr, 1);

            ddlDistributor.DataSource = dt;
            ddlDistributor.DataBind();

            iRepDistributorID = Convert.ToInt32(Session["RepDistributorID"]);
            ddlDistributor.SelectedValue = iRepDistributorID.ToString();
        }

        protected void LoadCompanies(int RepDistributorID)
        {
            ddlCompanies.Items.Clear();

            DataTable dt = r.Companies(RepDistributorID);
            ddlCompanies.DataTextField = "Company";
            ddlCompanies.DataValueField = "CustomerID";

            DataRow dr = dt.NewRow();
            dr["Company"] = "";
            dr["CustomerID"] = 0;
            dt.Rows.InsertAt(dr, 0);

            ddlCompanies.DataSource = dt;
            ddlCompanies.DataBind();

            int iCustomerID = Convert.ToInt32(Session["CustomerID"]);
            ddlCompanies.SelectedValue = iCustomerID.ToString();
        }

        protected void LoadQuotes()
        {
            SetQuoteName();
            lblQuote.Text = QuoteName;
            dsQuoteHistory.DataBind();
        }
        
        protected void ddlCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sCustomerID = ddlCompanies.SelectedValue;
            int iCustomerID = (sCustomerID == null || sCustomerID == "") ? 0 : Convert.ToInt32(sCustomerID);
            Session["CustomerID"] = iCustomerID;
            txtCustomerID.Text = iCustomerID.ToString();

            LoadQuotes();
        }

        protected void ddlDistributor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sRepID = ddlDistributor.SelectedValue;
            int iRepDistributorID = (sRepID == null || sRepID == "") ? 0 : Convert.ToInt32(sRepID);
            Session["RepDistributorID"] = iRepDistributorID;
            Session["RepDistributorName"] = ddlDistributor.SelectedItem;
            txtRepDistributorID.Text = iRepDistributorID.ToString();

            Session["CustomerID"] = "0";
            LoadCompanies(iRepDistributorID);
            LoadQuotes();
        }

        protected void SetQuoteName()
        {
            int iCustomerID = Convert.ToInt32(Session["CustomerID"]);

            QuoteName = "Quotes for ";

            if (iCustomerID == 0)
            {
                QuoteName += ddlDistributor.SelectedItem.ToString();
            }
            else
            {
                QuoteName += ddlCompanies.SelectedItem.ToString();
            }
        }

        protected void gvQuoteHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ProjectName")
            {
                int QuoteID = Convert.ToInt32(e.CommandArgument);

                Session["QuoteID"] = QuoteID;
                Session["PageName"] = "Quote";
                Response.Redirect("Quote.aspx");
            }
        }
        
    }
}