using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace MGM_Transformer.followup
{
    public partial class Followup : System.Web.UI.Page
    {
        Quotes q = new Quotes();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProgress();
                LoadLostReason();
                LoadLostTo();
            }
        }


        protected void LoadProgress()
        {
            DataTable dt = q.ProgressCodes();

            ddlProgress.Items.Clear();

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr["Progress"] = "";
                dt.Rows.InsertAt(dr, 0);

                ddlProgress.DataTextField = "Progress";
                ddlProgress.DataValueField = "Progress";
                ddlProgress.DataSource = dt;
                ddlProgress.DataBind();
            }
        }
        protected void LoadLostReason()
        {
            DataTable dt = q.LostReasonCodes();

            ddlLostReason.Items.Clear();

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr["LostReason"] = "";
                dt.Rows.InsertAt(dr, 0);

                ddlLostReason.DataTextField = "LostReason";
                ddlLostReason.DataValueField = "LostReason";
                ddlLostReason.DataSource = dt;
                ddlLostReason.DataBind();
            }
        }

        protected void LoadLostTo()
        {
            DataTable dt = q.LostToCodes();

            ddlLostTo.Items.Clear();

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr["LostTo"] = "";
                dt.Rows.InsertAt(dr, 0);

                ddlLostTo.DataTextField = "LostTo";
                ddlLostTo.DataValueField = "LostTo";
                ddlLostTo.DataSource = dt;
                ddlLostTo.DataBind();
            }
        }
    }
}