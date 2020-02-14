using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace MGM_Transformer
{
    public partial class AdminMaintenanceBody : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["IsLoggedIn"]) == 0)
                Response.Redirect("http://www.mgmtransformer.com");


            if (!IsPostBack)
            {
                Session["Winding"] = "Aluminum";
                
                FormatSection();
            }
        }

        // Import.
        protected void btnImport_Click(object sender, EventArgs e)
        {
            Prices p = new Prices();

            p.Import(1);          // Custom Aluminum.
            p.Import(2);          // Custom Copper.
            bool retval = p.Import(3);          // Stock / Reps.

            string sMsg = "";
            if (retval)
                sMsg = "Imported.";
            else
                sMsg = "No data to import.";

            Response.Write("<script>alert('" + sMsg + "');</script>");
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

        protected void lnkbCustomPriceList_Click(object sender, EventArgs e)
        {
            ProcessButton("CustomPriceList");
        }

        protected void lnkbExpedite_Click(object sender, EventArgs e)
        {
            ProcessButton("Expedite");
        }


        protected void FormatSection()
        {
            string sHiddenOption = hidOption.Value;

            lnkbCustomPriceList.Visible = (sHiddenOption == "CustomPriceList") ? false : true;
            lnkbExpedite.Visible = (sHiddenOption == "Expedite") ? false : true;
            lnkbGiftCardPmts.Visible = (sHiddenOption == "GiftCardPmts") ? false : true;

            lblCustomPriceList.Visible = (sHiddenOption == "CustomPriceList") ? true : false;
            lblExpedite.Visible = (sHiddenOption == "Expedite") ? true : false;
            lblGiftCardPmts.Visible = (sHiddenOption == "GiftCardPmts") ? true : false;

            pnlCustomPriceList.Visible = lblCustomPriceList.Visible;
            pnlExpediteFees.Visible = lblExpedite.Visible;
            pnlGiftCardPmts.Visible = lblGiftCardPmts.Visible;

            if (sHiddenOption == "GiftCardPmts")
            {
                LoadReps();
                LoadMonths();
                gvGiftCardPmts.DataBind();
            }
        }

        protected void ProcessButton(string sTarget)
        {
            hidOption.Value = sTarget;

            FormatSection();
        }

        protected void lnkbGiftCardPmts_Click(object sender, EventArgs e)
        {
            ProcessButton("GiftCardPmts");
        }

        /// <summary>
        /// Load a list of Reps for the Gift Card Program.
        /// </summary>
        protected void LoadReps()
        {
            ddlReps.Items.Clear();

            RepObject r = new RepObject();
            
            DataSet ds = r.List();

            ddlReps.DataTextField = "Full_Name";
            ddlReps.DataValueField = "RepID";

            DataRow dr = ds.Tables[0].NewRow();
            dr["RepID"] = "0";
            dr["Full_Name"] = "";
            ds.Tables[0].Rows.InsertAt(dr, 0);

            ddlReps.DataSource = ds.Tables[0];
            ddlReps.DataBind();
            ddlReps.Items[0].Selected = true;

        }

        protected void LoadMonths()
        {
            ddlMonths.Items.Clear();

            DataValidation dv = new DataValidation();

            DataTable dt = dv.Months();

            ddlMonths.DataTextField = "MonthName";
            ddlMonths.DataValueField = "MonthValue";

            DataRow dr = dt.NewRow();
            dr["MonthName"] = "";
            dr["MonthValue"] = "01/01/1900";
            
            dt.Rows.InsertAt(dr, 0);

            ddlMonths.DataSource = dt;
            ddlMonths.DataBind();
            ddlMonths.Items[0].Selected = true;
        }

        protected void ddlReps_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvGiftCardPmts.DataBind();
            ShowCheckAll();
        }
        protected void chkShowPaid_CheckedChanged(object sender, EventArgs e)
        {
            gvGiftCardPmts.DataBind();
        }
        protected void ddlMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvGiftCardPmts.DataBind();
            ShowCheckAll();
        }
        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            gvGiftCardPmts.DataBind();
            chkAll.Checked = false;
        }
        protected void ShowCheckAll()
        {
            if (ddlReps.SelectedValue != "0" || ddlMonths.SelectedValue != "1/1/1900 12:00:00 AM")
            {
                 lblChkAll.Visible = true;
                chkAll.Visible = true;
            }
            else
            {
                lblChkAll.Visible = false;
                chkAll.Visible = false;
            }
        }
    }
}