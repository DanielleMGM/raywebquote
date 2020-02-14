using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace MGM_Transformer
{
    public partial class StockPricesBody : System.Web.UI.UserControl
    {
        Quotes q = new Quotes();
        RepObject r = new RepObject();

        void Session_Start(object sender, EventArgs e)
        {
            Response.Redirect("http://www.mgmtransformer.com");

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["IsLoggedIn"]) == 0)
                Response.Redirect("http://www.mgmtransformer.com");


            if (!IsPostBack)
            {
                txtRepID.Text = Session["RepID"].ToString();
                txtRepDistributorID.Text = Session["RepDistributorID"].ToString();

                LoadConfiguration();
                LoadDistributors();
                SetListName();
                ShowControls();
            }
        }

        protected void LoadDistributors()
        {
            int iRepID = Convert.ToInt32(txtRepID.Text);
            ddlDistributor.Items.Clear();

            DataTable dt = r.Distributors(iRepID);
            ddlDistributor.DataTextField = "Distributor";
            ddlDistributor.DataValueField = "RepID";

            ddlDistributor.DataSource = dt;
            ddlDistributor.DataBind();

            int iRepDistributorID = Convert.ToInt32(Session["RepDistributorID"]);
            ddlDistributor.SelectedValue = iRepDistributorID.ToString();
        }

        protected void LoadConfiguration()
        {
            ddlConfigs.Items.Clear();

            bool bIncludeTP1 = true;
            string sRep = Session["RepName"].ToString();
            if (sRep == "IEM")
            {
                bIncludeTP1 = false;
            }

            DataTable dt = q.StockConfigs(bIncludeTP1);
            ddlConfigs.DataTextField = "Display";
            ddlConfigs.DataValueField = "SortOrder";

            ddlConfigs.DataSource = dt;
            ddlConfigs.DataBind();

            StockPriceListRefresh();
        }

        protected void ddlConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            StockPriceListRefresh();
        }

        protected void StockPriceListRefresh()
        {
            int iStockID = Convert.ToInt32(ddlConfigs.SelectedValue);

            DataTable dt = q.StockConfigs_GetParams(iStockID);

            if (dt.Rows.Count == 0)
                return;

            DataRow dr = dt.Rows[0];

            string sEfficiency = dr["Efficiency"].ToString().Trim();

            if (sEfficiency == "D16") sEfficiency = "DOE2016";

            Session["Efficiency"] = sEfficiency;
            Session["Phase"] = dr["Phase"].ToString().Trim();
            Session["Winding"] = dr["Windings"].ToString().Trim();
            Session["Configuration"] = dr["Configuration"].ToString().Trim();

            dt.Dispose();

            gvStockPrices.DataBind();
            RefeshListRowColor();
        }

        protected void SetListName()
        {
            string sRep = Session["RepDistributorName"].ToString();

            lblStockPrices.Text = "Stock prices for " + sRep;
        }

        protected void RefeshListRowColor()
        {
            string sWinding = Session["Winding"].ToString().Trim();

            if (sWinding == "Copper")
            {
                gvStockPrices.RowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FADFCA");
            }
            else
            {
                gvStockPrices.RowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#E6E7EB");
            }
        }

        protected void ddlDistributor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sRepID = ddlDistributor.SelectedValue;
            int iRepDistributorID = (sRepID == null || sRepID == "") ? 0 : Convert.ToInt32(sRepID);
            Session["RepDistributorID"] = iRepDistributorID;
            Session["RepDistributorName"] = ddlDistributor.SelectedItem;
            txtRepDistributorID.Text = iRepDistributorID.ToString();

            Session["CustomerID"] = "0";

            SetListName();
            ShowControls();
            StockPriceListRefresh();
        }

        protected void ShowControls()
        {
            string sRepID = txtRepID.Text;
            string sRepDistributorID = txtRepDistributorID.Text;

            if (sRepID == sRepDistributorID || sRepDistributorID == "")
            {
                lblRepDistributorAlt.Visible = false;
            }
            else
            {
                lblRepDistributorAlt.Visible = true;
            }

            if (sRepID == "123")    // IEM has ID= 123.
            {
                divTP1.Visible = false;
            }
        }
    }
}