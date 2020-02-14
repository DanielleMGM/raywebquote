using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using NLog;
using System.Web.Configuration;

namespace MGM_Transformer
{
    public partial class SubmittalBody : System.Web.UI.UserControl
    {
        bool bShowDetails = false;
        DataValidation dv = new DataValidation();
        Quotes q = new Quotes();

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
                lblQuoteNo.Text = Session["QuoteNoDisplay"].ToString();

                int iQuoteID = Convert.ToInt32(Session["QuoteID"]);

                // Need to load Submittal Type before Quote, because Quote sets Submittal Type value.
                LoadSubmittalType();
                LoadQuote(iQuoteID);

                // Explicitly add HTML functionality to ASP controls, which get that functionality as a pass-through, but with a warning.
                txtSubmittalNote.Attributes.Add("onChange", "onChange();");
                txtSubmittalNote.Attributes.Add("onKeyUp", "onKeyUp();");
            }
        }

        protected void gvQuoteItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int iQuoteID = Convert.ToInt32(Session["QuoteID"]);
                int iQuoteDetailsID = Convert.ToInt32(e.CommandArgument);

                int iDetailID = q.GetDetailID(iQuoteDetailsID);

                // Can't rely on memory variable at this point.
                LoadSubmittal(iQuoteID, iDetailID);
                bShowDetails = true;
                ShowControls();
            }
        }

        protected void LoadQuote(int iQuoteID)
        {
            DataTable dtQD = q.SubmittalLoad(iQuoteID);
            DataRow drQD = dtQD.Rows[0];

            // PurchaseOrderNo
            string sPurchaseOrderNo = drQD["PurchaseOrderNo"].ToString();
            sPurchaseOrderNo = string.IsNullOrEmpty(sPurchaseOrderNo) == true ? "" : sPurchaseOrderNo;
            txtPurchaseOrderNo.Text = sPurchaseOrderNo;
            // SalesOrderNo						
            string sSalesOrderNo = drQD["SalesOrderNo"].ToString();
            sSalesOrderNo = string.IsNullOrEmpty(sSalesOrderNo) == true ? "" : sSalesOrderNo;
            txtSalesOrderNo.Text = sSalesOrderNo;

            string sSubmittalType = drQD["SubmittalType"].ToString();
            sSubmittalType = string.IsNullOrEmpty(sSubmittalType) == true ? "" : sSubmittalType;
            ddlSubmittalType.SelectedValue = sSubmittalType;

            // If this quote is defined as an OEM, show that the submittal will be defined as such.
            bool bOEM = q.IsOEM(iQuoteID);
            lblOEM.Visible = bOEM;
        }

        protected void LoadSubmittal(int iQuoteID, int iDetailID)
        {

            DataTable dtQD = q.SubmittalEdit(iQuoteID, iDetailID);
            DataRow drQD = dtQD.Rows[0];

            lblItemNo.Text = iDetailID.ToString();

            string sSubmittal = drQD["IsSubmittal"].ToString();
            chkSubmit.Checked = sSubmittal == "X" ? true : false;

            // CaseColorCode
            string sCaseColorCode = drQD["CaseColorCode"].ToString();
            sCaseColorCode = string.IsNullOrEmpty(sCaseColorCode) == true ? "" : sCaseColorCode;
            lblCaseColorCode.Text = sCaseColorCode;
            // CaseSize						
            string sCaseSize = drQD["CaseSize"].ToString();
            sCaseSize = string.IsNullOrEmpty(sCaseSize) == true ? "" : sCaseSize;
            lblCaseSize.Text = sCaseSize;
            // CatalogNumber
            string sCatalogNumber = drQD["CatalogNumber"].ToString();
            sCatalogNumber = string.IsNullOrEmpty(sCatalogNumber) == true ? "" : sCatalogNumber;
            lblCatalogNumber.Text = sCatalogNumber;
            // ElectrostaticShield
            string sElectrostaticShield = drQD["ElectrostaticShield"].ToString();
            sElectrostaticShield = string.IsNullOrEmpty(sElectrostaticShield) == true ? "" : sElectrostaticShield;
            lblElectrostaticShield.Text = sElectrostaticShield;
            // Enclosure
            string sEnclosure = drQD["Enclosure"].ToString();
            sEnclosure = string.IsNullOrEmpty(sEnclosure) == true ? "" : sEnclosure;
            lblEnclosure.Text = sEnclosure;
            // Efficiency
            string sEfficiency = drQD["Efficiency"].ToString();
            sEfficiency = string.IsNullOrEmpty(sEfficiency) == true ? "" : sEfficiency;
            lblEfficiency.Text = sEfficiency;
            // EfficiencyExemptReason
            string sEfficiencyExemptReason = drQD["EfficiencyExemptReason"].ToString();
            sEfficiencyExemptReason = string.IsNullOrEmpty(sEfficiencyExemptReason) == true ? "" : sEfficiencyExemptReason;
            // Trim the words "EXEMPT:" if present.
            if (sEfficiencyExemptReason.Contains("EXEMPT:<br />") == true)
            {
                int i = sEfficiencyExemptReason.IndexOf("EXEMPT:");
                sEfficiencyExemptReason = sEfficiencyExemptReason.Substring(i + 13, sEfficiencyExemptReason.Length - 13);
            }

            lblEfficiencyExemptReason.Text = sEfficiencyExemptReason;
            // EnclosureMtl
            string sEnclosureMtl = drQD["EnclosureMtl"].ToString();
            sEnclosureMtl = string.IsNullOrEmpty(sEnclosureMtl) == true ? "" : sEnclosureMtl;
            lblEnclosureMtl.Text = sEnclosureMtl;
            // Frequency
            string sFrequency = drQD["Frequency"].ToString();
            sFrequency = string.IsNullOrEmpty(sFrequency) == true ? "" : sFrequency;
            lblFrequency.Text = sFrequency;
            // ImpedanceNotes
            string sImpedanceNotes = drQD["ImpedanceNotes"].ToString();
            sImpedanceNotes = string.IsNullOrEmpty(sImpedanceNotes) == true ? "" : sImpedanceNotes;
            lblImpedanceNotes.Text = sImpedanceNotes;
            // KFactor
            string sKFactor = drQD["KFactor"].ToString();
            sKFactor = string.IsNullOrEmpty(sKFactor) == true ? "" : sKFactor;
            lblKFactor.Text = sKFactor;
            // KVA
            string sKVA = drQD["KVA"].ToString();
            sKVA = string.IsNullOrEmpty(sKVA) == true ? "" : sKVA;
            lblKVA.Text = sKVA;
            // MadeInUSACodes
            string sMadeInUSACodes = drQD["MadeInUSACodes"].ToString();
            sMadeInUSACodes = string.IsNullOrEmpty(sMadeInUSACodes) == true ? "" : sMadeInUSACodes;
            // Drop the word "None", if found.
            if (sMadeInUSACodes == "None") sMadeInUSACodes = "";

            lblMadeInUSACodes.Text = sMadeInUSACodes;
            // MarineDuty
            string sMarineDuty = drQD["MarineDuty"].ToString();
            sMarineDuty = string.IsNullOrEmpty(sMarineDuty) == true ? "" : sMarineDuty;
            lblMarineDuty.Text = sMarineDuty;
            // Phase
            string sPhase = drQD["Phase"].ToString();
            sPhase = string.IsNullOrEmpty(sPhase) == true ? "" : sPhase;
            lblPhase.Text = sPhase;
            // PriceUnit
            string sPriceUnit = drQD["PriceUnit"].ToString();
            sPriceUnit = string.IsNullOrEmpty(sPriceUnit) == true ? "0" : sPriceUnit;
            lblPriceUnit.Text = '$' + dv.NumberFormat(sPriceUnit, 9, 0);
            // PriceExt
            string sPriceExt = drQD["PriceExt"].ToString();
            sPriceExt = string.IsNullOrEmpty(sPriceExt) == true ? "0" : sPriceExt;
            lblPriceExt.Text = '$' + dv.NumberFormat(sPriceExt, 9, 0);
            // PrimaryVoltage
            string sPrimaryVoltage = drQD["PrimaryVoltage"].ToString();
            sPrimaryVoltage = string.IsNullOrEmpty(sPrimaryVoltage) == true ? "" : sPrimaryVoltage;
            lblPrimaryVoltage.Text = sPrimaryVoltage;
            // Quantity
            int iQuantity = Convert.ToInt32(drQD["Quantity"]);
            lblQuantity.Text = iQuantity.ToString();
            // SecondaryVoltage
            string sSecondaryVoltage = drQD["SecondaryVoltage"].ToString();
            sSecondaryVoltage = string.IsNullOrEmpty(sSecondaryVoltage) == true ? "" : sSecondaryVoltage;
            lblSecondaryVoltage.Text = sSecondaryVoltage;
            // SoundReductCode
            string sSoundReductCode = drQD["SoundReductCode"].ToString();
            sSoundReductCode = string.IsNullOrEmpty(sSoundReductCode) == true ? "" : sSoundReductCode;
            lblSoundReductCode.Text = sSoundReductCode;
            // SpecialFeatureCodes
            string sSpecialFeatureCodes = drQD["SpecialFeatureCodes"].ToString();
            sSpecialFeatureCodes = string.IsNullOrEmpty(sSpecialFeatureCodes) == true ? "" : sSpecialFeatureCodes;
            lblSpecialFeatureCodes.Text = sSpecialFeatureCodes;
            // SpecialFeatureNotes
            string sSpecialFeatureNotes = drQD["SpecialFeatureNotes"].ToString();
            sSpecialFeatureNotes = string.IsNullOrEmpty(sSpecialFeatureNotes) == true ? "" : sSpecialFeatureNotes;
            lblSpecialFeatureNotes.Text = sSpecialFeatureNotes;
            // SpecialTypeCode
            string sSpecialTypeCode = drQD["SpecialTypeCode"].ToString();
            sSpecialTypeCode = string.IsNullOrEmpty(sSpecialTypeCode) == true ? "" : sSpecialTypeCode;
            sSpecialTypeCode = sSpecialTypeCode == "None" ? "" : sSpecialTypeCode;
            lblSpecialTypeCode.Text = sSpecialTypeCode;
            // SubmittalNote
            string sSubmittalNote = drQD["SubmittalNote"].ToString();
            sSubmittalNote = string.IsNullOrEmpty(sSubmittalNote) == true ? "" : sSubmittalNote;
            txtSubmittalNote.Text = sSubmittalNote;
            // TapsNotes
            string sTapsNotes = drQD["TapsNotes"].ToString();
            sTapsNotes = string.IsNullOrEmpty(sTapsNotes) == true ? "" : sTapsNotes;
            lblTapsNotes.Text = sTapsNotes;
            // Temperature 
            string sTemperature = drQD["Temperature"].ToString();
            sTemperature = string.IsNullOrEmpty(sTemperature) == true ? "" : sTemperature;
            lblTemperature.Text = sTemperature;
            // TransformerType
            string sTransformerType = drQD["TransformerType"].ToString();
            sTransformerType = string.IsNullOrEmpty(sTransformerType) == true ? "" : sTransformerType;
            lblTransformerType.Text = sTransformerType;
           // Windings
            string sWindings = drQD["Windings"].ToString();
            sWindings = string.IsNullOrEmpty(sWindings) == true ? "" : sWindings;
            lblWindings.Text = sWindings;
        }

        protected void LoadSubmittalType()
        {
            ddlSubmittalType.Items.Clear();

            DataTable dt = q.SubmittalType();

            ddlSubmittalType.DataTextField = "SubmittalType";
            ddlSubmittalType.DataValueField = "SubmittalType";

            DataRow dr = dt.NewRow();           // Dividing row.
            dr["SubmittalType"] = "";
            dt.Rows.InsertAt(dr, 0);

            ddlSubmittalType.SelectedValue = "";
            ddlSubmittalType.DataSource = dt;
            ddlSubmittalType.DataBind();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            DataValidation dv = new DataValidation();
            string sPath = dv.CurrentDir(true);
            int iQuoteID = Convert.ToInt32(Session["QuoteID"]);
            bool bOEM = lblOEM.Visible;
            bool bOEMEmail;

            SaveQuote();

            // Make sure all required fields, including all item Submittal Notes, are entered.
            string sErrorMsg = q.SubmittalRequire(iQuoteID);
            lblErrorMsg.Text = sErrorMsg;
            if (sErrorMsg != "")
            {
                return;
            }

            if (q.CreatePDF(iQuoteID, false, bOEM, false, true, out bOEMEmail) == false)
            {
                return;
            }

            string sURL = q.QuotePDFUrl(iQuoteID, false, true);

            // Hard-coding to production since we don't seem to have access rights to QA.
            // **************************************************************************
            string sPathFull = "";
            if (Convert.ToBoolean(WebConfigurationManager.AppSettings["LocalMachine"]) == true)
                sPathFull = WebConfigurationManager.AppSettings["LocalWebSiteUrl"] + sURL;
            else
                sPathFull = "https://MGMQuotation.MGMTransformer.com//MGMQuotation//pdfs//" + sURL;// +
            // **************************************************************************

            // Open PDF in another browser.
            ResponseHelper.Redirect(sPathFull, "_blank", "");

            System.Diagnostics.Process.Start(sPathFull);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            SaveQuote();

            Session["PageName"] = "Quote";
            Response.Redirect("Quote.aspx");
        }

        protected void SaveQuote()
        {
            int iQuoteID = Convert.ToInt32(Session["QuoteID"]);
            string sPurchaseOrderNo = txtPurchaseOrderNo.Text;
            string sSalesOrderNo = txtSalesOrderNo.Text;
            string sSubmittalType = ddlSubmittalType.SelectedValue;
            bool bIncludeWiringDiagram = chkWiringDiagram.Checked;

            sPurchaseOrderNo = string.IsNullOrEmpty(sPurchaseOrderNo) == true ? "" : sPurchaseOrderNo;
            sSalesOrderNo = string.IsNullOrEmpty(sSalesOrderNo) == true ? "" : sSalesOrderNo;
            sSubmittalType = string.IsNullOrEmpty(sSubmittalType) == true ? "" : sSubmittalType;

            q.SubmittalQuoteSave(iQuoteID, bIncludeWiringDiagram, sPurchaseOrderNo, sSalesOrderNo, sSubmittalType);
        }

        protected void RequireFields()
        {
        }

        protected void ShowControls()
        {
            // Conditionally show the Editing panel.
            pnlEditing.Visible = bShowDetails;

            // Show red title if there is a note which won't appear on the submittal.
            lblEfficiencyExemptReasonNone.Visible = lblEfficiencyExemptReason.Text == "" ? true : false;
            lblEfficiencyExemptReasonTitle.Visible = lblEfficiencyExemptReason.Text == "" ? false : true;
            
            lblMarineDutyNone.Visible = lblMarineDuty.Text == "" ? true : false;
            lblMarineDutyTitle.Visible = lblMarineDuty.Text == "" ? false : true;

            lblMadeInUSACodesNone.Visible = lblMadeInUSACodes.Text == "" ? true : false;
            lblMadeInUSACodesTitle.Visible = lblMadeInUSACodes.Text == "" ? false : true;

            lblSpecialTypeCodeNone.Visible = lblSpecialTypeCode.Text == "" ? true : false;
            lblSpecialTypeCodeTitle.Visible = lblSpecialTypeCode.Text == "" ? false : true;

            lblSpecialFeatureCodesNone.Visible = lblSpecialFeatureCodes.Text == "" ? true : false;
            lblSpecialFeatureCodesTitle.Visible = lblSpecialFeatureCodes.Text == "" ? false : true;

            lblSpecialFeatureNotesNone.Visible = lblSpecialFeatureNotes.Text == "" ? true : false;
            lblSpecialFeatureNotesTitle.Visible = lblSpecialFeatureNotes.Text == "" ? false : true;

            lblTapsNotesNone.Visible = lblTapsNotes.Text == "" ? true : false;
            lblTapsNotesTitle.Visible = lblTapsNotes.Text == "" ? false : true;

            lblImpedanceNotesNone.Visible = lblImpedanceNotes.Text == "" ? true : false;
            lblImpedanceNotesTitle.Visible = lblImpedanceNotes.Text == "" ? false : true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int iQuoteID = Convert.ToInt32(Session["QuoteID"]);
            int iDetailID = Convert.ToInt32(lblItemNo.Text);

            bool bSelected = chkSubmit.Checked;

            string sSubmittalNote = txtSubmittalNote.Text;

            // Save the Submittal Note.
            q.SubmittalNoteSave(iQuoteID, iDetailID, sSubmittalNote, bSelected);

            // Refresh grid to show "X" as to whether notes were saved.
            gvQuoteItems.DataBind();

            bShowDetails = false;
            MsgClear();
            ShowControls();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            bShowDetails = false;
            ShowControls();
        }

        protected void MsgClear()
        {
            lblErrorMsg.Text = "";
        }

        protected void txtPurchaseOrderNo_TextChanged(object sender, EventArgs e)
        {
            string sPO = txtPurchaseOrderNo.Text;
            sPO = sPO.ToUpper();
            txtPurchaseOrderNo.Text = sPO;
        }

        protected void txtSalesOrderNo_TextChanged(object sender, EventArgs e)
        {
            string sSO = txtSalesOrderNo.Text;
            sSO = sSO.ToUpper();
            txtSalesOrderNo.Text = sSO;

        }

        // Turn on/off Submit if a note is entered.
        // You can recheck Submit if you want to submit without a note.
        protected void txtSubmittalNote_TextChanged(object sender, EventArgs e)
        {
            string sSubmittalNote = txtSubmittalNote.Text;
            sSubmittalNote = string.IsNullOrEmpty(sSubmittalNote) == true ? "" : sSubmittalNote;

            // Check the Submit button if there is a note.
            // However, don't uncheck it if there is no note, because notes are not required.
            if (sSubmittalNote != "")
            {
                chkSubmit.Checked = true;
            }
         }

        protected void chkSubmit_CheckedChanged(object sender, EventArgs e)
        {
            // Clear the note if we're not going to submit.
            if (chkSubmit.Checked == false)
            {
                txtSubmittalNote.Text = "";
            }
        }

        /// <summary>
        /// Check or uncheck all rows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSubmitAll_CheckedChanged(object sender, EventArgs e)
        {
            bool bCheck = chkSubmitAll.Checked;

            int iQuoteID = Convert.ToInt32(Session["QuoteID"]);

            q.SubmittalCheckAll(iQuoteID, bCheck);

            chkSubmit.Checked = bCheck;

            gvQuoteItems.DataBind();
        }
    }
}