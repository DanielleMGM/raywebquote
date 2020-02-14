using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Net;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Web.Configuration;


namespace MGM_Transformer
{
    public enum enumDisplay
    {
        All
       ,Notes
       ,Price
    }

    public partial class QuoteBody : System.Web.UI.UserControl
    {
        Quotes q = new Quotes();
        DataValidation dv = new DataValidation();
        RepObject r = new RepObject();
        Transformer t = new Transformer();

        // Used to prevent quote saving when refreshing the screen
        // after a button press.
        bool bButtonPress = false;

        void Session_Start(object sender, EventArgs e)
        {
            Response.Redirect("http://www.mgmtransformer.com");

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["IsLoggedIn"]) == 0)
                Response.Redirect("http://www.mgmtransformer.com");

            if (IsPostBack)
            {
                // If copying a quote, passes message back.
                string sMsg = Request.QueryString["msg"];
                lblQuoteCopied.Text = sMsg;

                // Done with regional postbacks.
                string sBtnPressed = hidButtonPressed.Value;

                if (String.IsNullOrEmpty(sBtnPressed) == false)
                {
                    if (sBtnPressed == "btnSave")
                    {
                        SaveItem(false);
                    }

                    hidButtonPressed.Value = null;
                }

                //if (ViewState["ShowInventory"].ToString() == "true")
                //    pnlInventory.Visible = true;
                //else
                //    pnlInventory.Visible = false;


                //if (rblStandardOrCustom.SelectedItem != null)
                //{
                //    if(rblStandardOrCustom.SelectedItem.Text == "Custom")
                //       pnlInventory.Visible = false;
                //}
               
            }
            else
            {
                // Done with full page postbacks.
                lblRepID.Text = (lblRepID.Text == "") ? Session["RepID"].ToString() : lblRepID.Text;
                lblRepDistributorID.Text = (lblRepDistributorID.Text == "") ? Session["RepDistributorID"].ToString() : lblRepDistributorID.Text;

                string sUserName = Session["UserName"].ToString();
                RepObject r = new RepObject();

                // Reset to the current Rep on this screen.
                Session["RepDistributorID"] = Session["RepID"];
                Session["RepDistributorName"] = Session["RepName"];

                // The Expedite fees list changes whenever the item changes.
                LoadExpedite(0,0,false);

                LoadDefaultDetails();

                int iQuoteID = int.Parse(Session["QuoteID"].ToString());

                if (iQuoteID > 0)
                {
                    LoadQuote(iQuoteID);
                    LoadQuoteItems();
                }
                else
                {
                    lblRep.Text = Session["RepName"].ToString();
                    lblRepDistributor.Text = Session["RepDistributorName"].ToString();
                    lblCreatedOn.Text = DateTime.Now.ToString("d");
                    lblStatus.Text = "Cart";
                    int iRepID = Convert.ToInt32(Session["RepID"]);
                    LoadRep(iRepID, sUserName);

                    lblFullNameLatest.Text = sUserName;
                    lblFullNameCreated.Text = Session["FullName"].ToString();

                    int iRepDistributorID = Convert.ToInt32(Session["RepDistributorID"]);
                    SwapDistributors(iRepDistributorID);                        // Set default.
                    LoadCustomers(iRepDistributorID, "");
                }
                int iQuoteNo = Convert.ToInt32(lblQuoteNo.Text);
                int iQuoteVer = Convert.ToInt32(lblQuoteNoVer.Text);

                string sQuoteOriginCode = lblQuoteOriginCode.Text.ToString();

                SetQuoteNoDisplay(iQuoteID, iQuoteNo, iQuoteVer, sQuoteOriginCode);

                SetStandardOrCustom(true);

                txtCompany.Focus();

                SetInventoryVisible();

            }
                      
        }

        private void SetInventoryVisible()
        {
            int iRepID = Convert.ToInt32(Session["RepID"]);

            string sSQL = "select WarehousePref1,WarehousePref2,WarehousePref3 from Rep where RepID = " + iRepID;// 123";// + iRepID;// 123";// + iRepID;
            DataSet ds = DataLink.Select(sSQL, DataLinkCon.mgmuser);

            int iWarehousePref1 = Utility.GetIntValue(ds, "WarehousePref1");
            int iWarehousePref2 = Utility.GetIntValue(ds, "WarehousePref2");
            int iWarehousePref3 = Utility.GetIntValue(ds, "WarehousePref3");

            if (iWarehousePref1 == -1 && iWarehousePref2 == -1 && iWarehousePref3 == -1)
            {
                ViewState["ShowInventory"] = "false";
                pnlInventory.Visible = false;
            }
            else
            {
                //added 7-17-19
                if (rblStandardOrCustom.SelectedValue != "Custom")
                {
                    ViewState["ShowInventory"] = "true";
                    ViewState["WarehousePref1"] = Utility.GetIntValue(ds, "WarehousePref1").ToString();
                    ViewState["WarehousePref2"] = Utility.GetIntValue(ds, "WarehousePref2").ToString();
                    ViewState["WarehousePref3"] = Utility.GetIntValue(ds, "WarehousePref3").ToString();
                    pnlInventory.Visible = true;
                }
                else
                {
                    ViewState["ShowInventory"] = "false";
                    pnlInventory.Visible = false;
                }
            }

        }



        protected void Page_Unload(object sender, EventArgs e)
        {

            //if (!Page.IsPostBack)
            //{
            //    // Force a save if we leave the screen.
            string sQuoteID = lblQuoteID.Text.ToString();
            int iQuoteID = (sQuoteID == null || sQuoteID == "") ? 0 : Convert.ToInt32(sQuoteID);
            if (iQuoteID > 0 && bButtonPress == false)
            {
                if (Convert.ToInt32(Session["IsTimeout"]) == 0)
                    SaveQuote(iQuoteID);
            }
            bButtonPress = false;
            //}
        }
        protected void LoadQuote(int iQuoteID)
        {
            // Get the status of who is seeing this information.
            // -------------------------------------------------
            // Admin functions as a Manager, in contrast to Internal, which is an internal rep.
            bool bAdmin = Convert.ToBoolean(Session["Admin"]);

            // If Internal, doesn't overwrite with latest prices while in Cart status.
            bool bInternal = Convert.ToBoolean(Session["Internal"]);

            // If Admin, automatically Internal.
            bInternal = bAdmin == true || bInternal;

            /***************************************************/
            DataTable dtQ = q.QuoteSelect(iQuoteID, bInternal);
            /***************************************************/

            DataRow drQ = dtQ.Rows[0];

            lblQuoteID.Text = iQuoteID.ToString();
            lblQuoteNo.Text = drQ["QuoteNo"].ToString();
            lblQuoteNoVer.Text = drQ["QuoteNoVer"].ToString();
            lblQuoteOriginCode.Text = drQ["QuoteOriginCode"].ToString();
            txtProject.Text = drQ["Projectname"].ToString();
            lblProject.Text = txtProject.Text;
            hidUserNameCreated.Value = drQ["UserName"].ToString();      // User who created the quote.
            lblFullNameCreated.Text = drQ["FullName"].ToString();

            hidUserNameLatest.Value = drQ["UserNameLast"].ToString();
            lblFullNameLatest.Text = drQ["FullNameLast"].ToString();

            lblRepID.Text = drQ["RepID"].ToString();
            lblRepDistributorID.Text = drQ["RepDistributorID"].ToString();

            string sUserName = drQ["UserName"].ToString();
            if (String.IsNullOrEmpty(sUserName) == true)
            {
                sUserName = Session["UserName"].ToString();
            }

            lblCreatedPhone.Text = drQ["CreatedPhone"].ToString();

            int iRepID = Convert.ToInt32(drQ["RepID"]);

            // Expect hidUserNameCreated, lblFullNameCreated (created)
            // and hidUserNameLatest, lblFullNameLatest (latest) to already be populated.
            LoadRep(iRepID, sUserName);

            /* Approvals
             * =========
             * EXTERNAL - CART - NEEDS APPROVAL
             * --------------------------------
             * Approval Required message, Notes Request, Approval Reason Calc.
             * Instead of Finalize button, 'Request Approval' button appears.
             * 
             * INTERNAL - CART - NEEDS APPROVAL
             * --------------------------------
             * Same as above, except Notes Approval also appears.
             *
             * MANAGER - CART - NEEDS APPROVAL
             * --------------------------------
             * Same as above, except radio button Approve / Deny also appears.
             *
             * MANAGER - ACTION - FINALIZE
             * ---------------------------
             * If Approve or Deny not selected, warning message.  No action.
             * If Approve selected, sends approval email, and says so.  All approval info removed from screen.  Finalized.
             * If Deny selected, sends denial email and says so.  All approval info stays on screen.  Still in Cart.
             * 
             * CART - DENIED
             * -------------
             * No change, except that Approve or Deny is selected when opened.
             * 
             * FINALIZED - APPROVED
             * --------------------
             * Approved, By, Date messages shown.
             */
            Session["QuoteID"] = iQuoteID;

            bool bHasOrders = Convert.ToBoolean(drQ["HasOrders"]);
            divOrders.Visible = bHasOrders;
            // Refreshing data source.
            gvOrders.DataBind();

            // Get the values for all approval related information.
            bool bApprovalReqd = Convert.ToBoolean(drQ["ApprovalReqd"]);
            lblApprovalReqd.Text = bApprovalReqd ? "APPROVAL REQUIRED." : "";

            bool bApprovalPending = Convert.ToBoolean(drQ["ApprovalRequested"]);
            lblApprovalReqd.Text = bApprovalPending ? "APPROVAL Requested." : lblApprovalReqd.Text;

            lblStatus.Text = drQ["Status"].ToString();
            bool bCart = (string.IsNullOrEmpty(lblStatus.Text) || lblStatus.Text == "Cart") ? true : false;

            // LostReason and LostTo are part of this code.
            lblProgress.Text = drQ["Progress"].ToString();

            string sApprovalReasonCalc = drQ["ApprovalReasonCalc"].ToString();
            sApprovalReasonCalc = String.IsNullOrEmpty(sApprovalReasonCalc) == true ? "" : sApprovalReasonCalc;
            ApprovalDisplay(sApprovalReasonCalc);

            // Triggers Request Approval if anything entered in it, even if no other conditions warrant.
            txtNotesInternal.Text = drQ["NotesInternal"].ToString();

            lblApprovedBy.Text = drQ["ApprovedByInits"].ToString();
            string sApprovedDate = drQ["ApprovedDate"].ToString();
            lblApprovedDate.Text = (sApprovedDate == "") ? null : Convert.ToDateTime(sApprovedDate).ToShortDateString();

            txtCustomerID.Text = drQ["CustomerID"].ToString();
            txtCustomerContactID.Text = drQ["CustomerContactID"].ToString();
            txtCompany.Text = drQ["Company"].ToString();
            lblCompany.Text = txtCompany.Text;
            txtCity.Text = drQ["City"].ToString();
            lblCity.Text = txtCity.Text;
            txtContactName.Text = drQ["ContactName"].ToString();

            string sStatus = drQ["Status"].ToString();

            lblContactName.Text = txtContactName.Text;
            txtEmail.Text = drQ["Email"].ToString();
            lblEmail.Text = txtEmail.Text;
            txtNotes.Text = drQ["Notes"].ToString();
            txtNotesPDF.Text = drQ["NotesPDF"].ToString();
            txtNotesRequest.Text = drQ["NotesRequest"].ToString();
            txtNotesInternal.Text = drQ["NotesInternal"].ToString();
            lblNotesPDF.Text = txtNotesPDF.Text;
            txtPDFURL.Text = drQ["PDFURL"].ToString();

            // Don't show notes, but show if there ARE any notes.
            ViewShowNotesLink(false, false);
            Decimal totalPrice = 0;
            Decimal.TryParse(drQ["TotalPrice"].ToString(), out totalPrice);

            lblTotalPrice.Text = totalPrice.ToString("C2");

            UpdateFreightIncluded(iQuoteID);

            DateTime CreatedOn = Convert.ToDateTime(drQ["Created_on"]);
            lblCreatedOn.Text = CreatedOn.ToString("d");
            string sFinalizedOn = drQ["FinalizedOn"].ToString();

            sFinalizedOn = string.IsNullOrEmpty(sFinalizedOn) ? "" : sFinalizedOn;
            DateTime FinalizedOn;
            if (sFinalizedOn != "")
            {
                FinalizedOn = Convert.ToDateTime(sFinalizedOn);
                lblFinalizedOn.Text = FinalizedOn.ToString("d");
            }
            else
            {
                lblFinalizedOn.Text = "";
            }



            lblRep.Text = drQ["Rep"].ToString();
            lblRepDistributor.Text = drQ["RepDistributor"].ToString();

            int iRepDistributorID = 0;
            int.TryParse(drQ["RepDistributorID"].ToString(), out iRepDistributorID);

            SwapDistributors(iRepDistributorID);

            LoadCustomers(iRepDistributorID, "");

            Boolean bOEM = drQ["IsOEM"].ToString() == "" ? false : Convert.ToBoolean(drQ["IsOEM"]);
            chkOEM.Checked = bOEM;

            Boolean bStockOnly = drQ["IsStockOnly"].ToString() == "" ? false : Convert.ToBoolean(drQ["IsStockOnly"]);
            Boolean bNoDrawingsAttached = drQ["IsNoDrawingsAttached"].ToString() == "" ? false : Convert.ToBoolean(drQ["IsNoDrawingsAttached"]);
            Boolean bWiringDiagram = drQ["IsWiringDiagram"].ToString() == "" ? false : Convert.ToBoolean(drQ["IsWiringDiagram"]);

            chkNoDrawingsAttached.Checked = bNoDrawingsAttached;

            // lblStockQuote.Text = (bStockOnly == false) ? "" : "Stock order status not reported.  Contact factory for order information.";

            chkNoFreeShipping.Checked = Convert.ToBoolean(drQ["NoFreeShipping"]);

            lblQuoteCopied.Text = drQ["CopyMessage"].ToString();

            NameReopen();

            // Default to not showing Notes.
            ViewShowNotesLink(false, false);

            // Display either Preview Quote or Show Quote on the button face.
            NameShowQuote();

            Display(enumDisplay.All);
        }

        /// <summary>
        /// Called when changing selected Customer.
        /// </summary>
        /// <param name="iCustomerID"></param>
        protected void LoadCustomer(int iCustomerContactID)
        {
            Customer c = new Customer();
            DataTable dt = c.ByCustomerID(iCustomerContactID);
            DataRow dr = dt.Rows[0];

            txtCustomerID.Text = dr["CustomerID"].ToString();
            txtCustomerContactID.Text = dr["CustomerContactID"].ToString();
            txtCompany.Text = dr["Company"].ToString();
            txtCity.Text = dr["City"].ToString();
            txtContactName.Text = dr["ContactName"].ToString();
            txtEmail.Text = dr["Email"].ToString();

            int iRepDistributorID = 0;
            // Return to main Rep if selecting the null value.
            if (ddlCustomer.SelectedValue == "0")
            {
                iRepDistributorID = Convert.ToInt32(lblRepID.Text);
                Session["RepDistributorID"] = iRepDistributorID.ToString();
            }
            else
            {
                iRepDistributorID = Convert.ToInt32(dr["RepDistributorID"]);
            }
            // Possibly swap Rep Distributor.
            SwapDistributors(iRepDistributorID);
        }

        // Called when refreshing Quote Items list.
        // Uses
        protected void LoadQuoteItems()
        {
            // Refreshing data source.
            gvQuoteItems.DataBind();            // This refreshes the data after an insert.
        }

        protected void LoadDefaultDetails()
        {
            ClearEntries("All");
            ResetDeleteWarning();

            divOrders.Visible = false;

            // Hidden field.
            txtDetailID.Text = "0";
            txtQuoteDetailsID.Text = "0";

            // Values "Standard" or "Custom" based on StockUnitID or CustomStockID having a value.
            rblStandardOrCustom.SelectedIndex = -1;
            // Values "Aluminum" or "Copper".
            rblWindings.SelectedValue = "Aluminum";
            // Values "Single" or "Three".
            rblPhase.SelectedValue = "Three";

            bool bCustom = true;
            bool bPhaseSingle = false;
            bool bWindingsCopper = false;
            bool bZigZag = false;
            LoadKVA(bCustom, bPhaseSingle, bWindingsCopper, bZigZag);

            LoadEfficiency("");

            LoadPrimaryVoltage(true, false, false);
            LoadSecondaryVoltage(true, false, false);
            LoadKFactor("Three", "75", false, false);
            LoadTempRise("Three", "75", "K-1 (STD)", false);
            rblElectrostaticShield.SelectedIndex = 0;

            LoadFrequency();
            LoadSoundReduct(0);       //Load one record until an item is selected.
            lblEfficiencyValue.Text = "";
            LoadEnclosure(1, 1);
            rblMadeInUSA.SelectedValue = "None";
            LoadSpecialFeatures();
            LoadCaseColor("");
            LoadCaseSizes();

            hidKitHasRainHood.Value = "";
            ClearList(chkLstSpecialFeatures, false);
            chkLstSpecialFeatures.Items[0].Selected = true;
            txtSpecialFeatureNotes.Text = "";

            lblCatalogNo.Text = "";
            lblCatalogNoExt.Text = "";
            lblKitName.Text = "";
            lblKitPrice.Text = "";
            txtQuantity.Text = "";
            lblUnitPrice.Text = "";
            lblTotalExtPrice.Text = "";
            chkForExport.Checked = false;
        }

        protected void LoadQuoteDetails(int iQuoteID, int iDetailID)
        {
            // If request being edited, cancel it.
            CancelRequest();
            ResetDeleteWarning();

            DataTable dtQD = q.QuoteDetailsSelect(iQuoteID, iDetailID);
            DataRow drQD = dtQD.Rows[0];

            bool bInternal = Convert.ToBoolean(Session["Internal"]);

            // Hidden fields.
            string sDetailID = drQD["DetailID"].ToString();
            txtQuoteDetailsID.Text = drQD["QuoteDetailsID"].ToString();

            if (sDetailID == null || sDetailID == "" || sDetailID == "0")
                return;

            string sSameAsStock = drQD["IsSameAsStock"].ToString();
            sSameAsStock = sSameAsStock == "True" ? "1" : "0";
            hidIsMatch.Value = sSameAsStock;

            string sStockID = drQD["StockID"].ToString();
            lblStockID.Text = (sStockID == null || sStockID == "") ? "0" : sStockID;

            string sCustomID = drQD["CustomID"].ToString();
            lblCustomID.Text = (sCustomID == null || sCustomID == "") ? "0" : sCustomID;

            // Values "Standard" or "Custom" based on StockUnitID or CustomStockID having a value.
            rblStandardOrCustom.SelectedValue = drQD["StandardOrCustom"].ToString();
            bool bCustom = (rblStandardOrCustom.SelectedValue == "Custom") ? true : false;
            SetStandardOrCustom(bCustom);

            // Values "Aluminum" or "Copper".
            rblWindings.SelectedValue = drQD["Windings"].ToString();
            // Values "Single" or "Three".
            rblPhase.SelectedValue = drQD["Phase"].ToString();

            bool bWindingAluminum = (rblWindings.SelectedValue == "Aluminum") ? true : false;
            bool bPhaseSingle = (rblPhase.SelectedValue == "Single") ? true : false;
            bool bZigZag = (rblSpecialTypes.SelectedValue == "Zig Zag" || rblSpecialTypes.SelectedValue == "Harmonic Mitigating") ? true : false;
            LoadKVA(bCustom, bPhaseSingle, !bWindingAluminum, bZigZag);
            decimal dKVA = 0;
            string sKVAEntered = drQD["KVAEntered"].ToString();               // Table has prepended spaces in it, for legibility.
            decimal.TryParse(sKVAEntered, out dKVA);
            decimal dKVAUsed = 0;
            string sKVAUsed = drQD["KVAUsed"].ToString();
            decimal.TryParse(sKVAUsed, out dKVAUsed);
            if (dKVA == 0 && dKVAUsed > 0)
            {
                sKVAEntered = sKVAUsed;
            }

            hidKVAUsed.Value = dKVAUsed.ToString();

            bool bHideKVA = Convert.ToBoolean(drQD["IsHideKVA"]);
            bool bHideVoltPrimary = Convert.ToBoolean(drQD["IsHideVoltPrimary"]);
            bool bHideVoltSecondary = Convert.ToBoolean(drQD["IsHideVoltSecondary"]);

            chkHideKVA.Checked = bHideKVA;
            chkHideVoltPrimary.Checked = bHideVoltPrimary;
            chkHideVoltSecondary.Checked = bHideVoltSecondary;

            string sPhase = (bPhaseSingle == true) ? "Single" : "Three";
            sKVAEntered = dv.KVAEntry(sPhase, sKVAEntered, bInternal);

            // Strip any zero after the KVA.
            txtKVA.Text = dv.KVAFormat(sKVAEntered);

            // Load hidden value with current value of KVA, so can return to it if necessary.
            hidKVA.Value = txtKVA.Text;

            lblKVAUsed.Text = sKVAUsed;

            if (!bCustom)
            {
                sKVAUsed = MatchKVA(sKVAEntered);
                SetKVA(sKVAEntered);
            }

            string sKVA = txtKVA.Text.ToString();

            decimal.TryParse(sKVA, out dKVA);

            int iKva = 0;
            iKva = Utility.IntFromString(sKVA);
            LoadSoundReduct(iKva);

            string sCustomerTagNo = drQD["CustomerTagNo"].ToString();
            txtCustomerTagNo.Text = sCustomerTagNo;

            string sEfficiencyCode = drQD["EfficiencyCode"].ToString();
            if (String.IsNullOrEmpty(sEfficiencyCode) == true)
                sEfficiencyCode = "DOE2016";

            lblEfficiencyValue.Text = sEfficiencyCode;

            string sEfficiencyCodeIsSetByAdmin = drQD["EfficiencyIsSetByAdmin"].ToString();

            string sEfficiencyCodeCalc = drQD["EfficiencyCodeCalc"].ToString();

            lblEfficiencyCodeCalc.Text = sEfficiencyCodeCalc == "" ? sEfficiencyCode : sEfficiencyCodeCalc;

            lblEfficiencyIsSetByAdmin.Text = sEfficiencyCodeIsSetByAdmin == "True" ? "1" : "0";

            string sEfficiencyExemptReason = drQD["EfficiencyExemptReason"].ToString();
            txtExemptReason.Text = sEfficiencyExemptReason;

            txtCatalogNoOEM.Text = drQD["CatalogNoOEM"].ToString();

            LoadEfficiency(sEfficiencyCode);

            if (!bCustom)
            {
                LoadConfiguration(dKVA, bPhaseSingle, !bWindingAluminum);

                // For copied in quotes, the TP1 configuration may not be available.
                // In addition, the D16 configuration may not be available.
                // If so, it will copy all characteristics in EXCEPT the configuration.
                string sConfigD16 = "D16: " + drQD["Configuration"].ToString();
                string sConfigTP1 = "TP1: " + drQD["Configuration"].ToString();
                if (sEfficiencyCode == "DOE2016")
                {
                    if (SetConfiguration(sConfigD16) == true)
                    {
                        ddlConfiguration.SelectedValue = sConfigD16;
                    }
                    else if (SetConfiguration(sConfigTP1) == true)
                    {
                        ddlConfiguration.SelectedValue = sConfigTP1;
                    }
                }
                else
                {
                    if (SetConfiguration(sConfigTP1) == true)
                    {
                        ddlConfiguration.SelectedValue = sConfigTP1;
                    }
                    else if (SetConfiguration(sConfigD16) == true)
                    {
                        ddlConfiguration.SelectedValue = sConfigD16;
                    }
                }

                SetKVA(sKVAUsed);
            }
            else
            {
                rblPrimaryDW.SelectedValue = drQD["PrimaryVoltageDW"].ToString().Substring(0, 1);    // Delta or Wye.
                ChangePrimaryDW(rblPrimaryDW.SelectedValue.ToString());
                bool bDeltaPrimary = (rblPrimaryDW.SelectedValue == "W") ? false : true;            // If not W, then D (default).

                rblSecondaryDW.SelectedValue = drQD["SecondaryVoltageDW"].ToString().Substring(0, 1);   // Delta or Wye.
                ChangeSecondaryDW(rblSecondaryDW.SelectedValue.ToString());
                bool bWyeSecondary = (rblSecondaryDW.SelectedValue == "W") ? true : false;

                string sPrimaryVoltage = drQD["PrimaryVoltage"].ToString();
                string sSecondaryVoltage = drQD["SecondaryVoltage"].ToString();

                // The special types are being loaded "early", so we can get the right voltages.
                // If Harmonic Mitigating or Zig Zag are selected, we'll use special ZZ voltages in the dropdown.
                string sSpecialTypes = drQD["SpecialTypeCode"].ToString();
                rblSpecialTypes.SelectedValue = (sSpecialTypes == null || sSpecialTypes == "") ? "None" : sSpecialTypes;

                bool bHarmonicMitigating = sSpecialTypes == "Harmonic Mitigating" || sSpecialTypes == "Zig Zag" ? true : false;

                LoadPrimaryVoltage(bDeltaPrimary, bHarmonicMitigating, bPhaseSingle);
                LoadSecondaryVoltage(bWyeSecondary, bHarmonicMitigating, bPhaseSingle);

                // Display voltages either as dropdown (reps) or entry (admin).
                lblPrimaryVoltageInvalid.Text = "";
                lblSecondaryVoltageInvalid.Text = "";

                if (bInternal == true)
                {
                    txtPrimaryVoltage.Text = sPrimaryVoltage;
                    txtSecondaryVoltage.Text = sSecondaryVoltage;
                    if (ddlPrimaryVoltage.Items.Count > 0)
                        ddlPrimaryVoltage.SelectedIndex = 0;
                    if (ddlSecondaryVoltage.Items.Count > 0)
                        ddlSecondaryVoltage.SelectedIndex = 0;
                }
                else
                {
                    SetPrimaryVoltage(sPrimaryVoltage);
                    if (ddlPrimaryVoltage.SelectedIndex == 0)
                        lblPrimaryShow.Text = "<br />" + sPrimaryVoltage;
                    else
                        lblPrimaryShow.Text = "";

                    SetSecondaryVoltage(sSecondaryVoltage);
                    if (ddlSecondaryVoltage.SelectedIndex == 0)
                        lblSecondaryShow.Text = "<br />" + sSecondaryVoltage;
                    else
                        lblSecondaryShow.Text = "";
                }

                bool bStepUp = Convert.ToBoolean(drQD["IsStepUp"]);
                hidIsStepUp.Value = bStepUp == true ? "1" : "0";

                string sKFactor = drQD["KFactor"].ToString();

                string sTempEntered = drQD["TempEntered"].ToString();
                string sTempUsed = drQD["TempUsed"].ToString();

                if (sTempEntered == "" && sTempUsed != "")
                {
                    sTempEntered = sTempUsed;
                }

                sTempEntered = dv.PrependSpaces(sTempEntered, 3);

                string sEnclosure = ddlEnclosure.SelectedValue;
                t.Enclosure = sEnclosure;
                bool bTenv = t.TotallyEnclosedNonVentilated;

                LoadTempRise(sPhase, sKVA, sKFactor, bTenv);
                SetTempRise(sTempEntered);

                ddlTempRise.Enabled = (bCustom) ? true : false;

                lblTempUsed.Text = sTempUsed;

                rblElectrostaticShield.SelectedValue = drQD["ElectrostaticShield"].ToString();
                rblElectrostaticShield.Enabled = (bCustom) ? true : false;

                bool bApprovalReqd = Convert.ToBoolean(drQD["ApprovalReqd"]);
                lblApprovalReqd.Text = (bApprovalReqd == true) ? "APPROVAL REQUIRED." : lblApprovalReqd.Text;       // Leave as is if this item does not require approval.
                lblApprovalRequested.Text = Convert.ToBoolean(drQD["ApprovalRequested"]) == true ? "APPROVAL REQUESTED" : "";
                string sApprovalReasonQuote = drQD["ApprovalReasonCalc"].ToString();
                ApprovalDisplay(sApprovalReasonQuote);

                // ***************************************************
                // More Options
                // ***************************************************
                string sFrequency = drQD["FrequencyCode"].ToString();
                ddlFrequency.SelectedValue = (sFrequency == null || sFrequency == "") ? "60 Hz (STD)" : sFrequency;

                string sSoundReduct = drQD["SoundReductCode"].ToString();
                if (sSoundReduct != "" && sSoundReduct != "0")
                {
                    if (FindSoundReduct(sSoundReduct) == true)
                    {
                        ddlSoundReduct.SelectedValue = sSoundReduct;
                    }
                }

                bool bForExport = Convert.ToBoolean(drQD["IsForExport"]);
                chkForExport.Checked = bForExport;

                // Reload K-Factor and Temp Rise based on Phase and KVA entered.
                bool bExempt = IsExempt();
                LoadKFactor(sPhase, sKVA, bExempt, bTenv);

                // Load K-20 if selected, otherwise, the default.
                if (FindSpecialFeature("K-Factor 20 (K-20)") == true)
                {
                    ddlKFactor.SelectedValue = "K-20";
                }
                else
                {
                    // KFactor may be K-4 for Drive Isolation, yet not be available for high KVA's.
                    // If so, switches to K-1, but saves as K-4.
                    if (sKFactor == "None" || sKFactor == "None (STD)" || sKFactor == "K-1")
                    {
                        sKFactor = "K-1 (STD)";
                    }
                    if (ddlKFactor.Items.FindByText(sKFactor) != null)
                        ddlKFactor.SelectedValue = sKFactor;
                    else
                        sKFactor = "K-1 (STD)";

                    if (ddlKFactor.Items.FindByText(sKFactor) != null)
                        ddlKFactor.SelectedValue = sKFactor;

                    ddlKFactor.Enabled = (bCustom) ? true : false;
                }

                SetEfficiency(sEfficiencyCode);

                lblCaseSize.Text = drQD["CaseSize"].ToString();
                t.CaseSize = lblCaseSize.Text;

                lblCaseSizeCalc.Text = drQD["CaseSizeCalc"].ToString();

                // Show the calculated case size only if it differs from what is entered.
                CaseSizeCalcDisplay();

                ddlCaseSizes.SelectedValue = drQD["CaseSize"].ToString();

                string sEnclosureMtl = drQD["EnclosureMtlCode"].ToString();
                // HRPO (STD)
                // 304 Stainless Steel
                // 316 Stainless Steel
                // Core and Coil Only
                t.EnclosureMaterial = sEnclosureMtl;

                bool bTENV = Convert.ToBoolean(drQD["TotallyEnclosed"]);
                t.TotallyEnclosedNonVentilated = bTENV;

                string sIndoorOutdoor = string.IsNullOrEmpty(drQD["Enclosure"].ToString()) ? "" : drQD["Enclosure"].ToString();
                // Indoor
                // Indoor/Outdoor
                // Outdoor
                t.IndoorOutdoor = sIndoorOutdoor;
                lblEnclosureData.Text = sIndoorOutdoor;

                t.KitQty = Convert.ToInt32(drQD["KitQuantity"]);

                // Expect ddlEnclosure to have all values available at this point
                SetEnclosure(t.Enclosure);

                // Switch options for Case Color based on whether or not we've selected Stainless Steel.
                LoadCaseColor(sEnclosure);

                // Sets values of NEMA related fields, including whether TENV is enabled or not.
                //EnclosureUpdate();

                string sCaseColor = drQD["CaseColorCode"].ToString();
                sCaseColor = (sCaseColor == "") ? "ANSI 61 (STD)" : sCaseColor;
                SetCaseColor(sCaseColor);

                txtCaseColorOther.Text = drQD["CaseColorOther"].ToString();


                chkMarineDuty.Checked = Convert.ToBoolean(drQD["MarineDuty"]);

                string sMadeInUSACodes = drQD["MadeInUSACodes"].ToString();

                rblMadeInUSA.SelectedValue = "None";

                // Looping this way because we used to allow multiple selections.
                for (int i = 0; i < rblMadeInUSA.Items.Count; i++)
                {
                    if (rblMadeInUSA.Items[i].Value == sMadeInUSACodes)
                    {
                        rblMadeInUSA.SelectedValue = sMadeInUSACodes;
                        break;
                    }
                }

                bool bMadeInUSA = (sMadeInUSACodes != null && sMadeInUSACodes != "") ? true : false;

                string sSpecialFeatures = drQD["SpecialFeatureCodes"].ToString();
                CheckboxListLoad(chkLstSpecialFeatures, sSpecialFeatures);

                string sSpecialFeatureNotes = drQD["SpecialFeatureNotes"].ToString();

                txtSpecialFeatureNotes.Text = sSpecialFeatureNotes;

                // Might have been disabled if one of the following special types had been previously selected.
                ddlKFactor.Enabled = true;

                switch (sSpecialTypes)
                {
                    case "Drive Isolation":
                    case "Harmonic Mitigating":
                    case "Zig Zag":
                        ddlKFactor.Enabled = false;
                        break;
                }

                // ***************************************************
                // Shows / Hides More Options and listboxes.
                // ***************************************************
                bool bHasSpecialFeatures = HasSpecialFeatures();

                txtTapsOEM.Text = drQD["TapsOEM"].ToString();
                txtImpedanceOEM.Text = drQD["ImpedanceOEM"].ToString();

                hidDetailNotesInternal.Value = drQD["NotesInternal"].ToString();

                //ClearList(chkLstSpecialFeatures, false);

                // ***************************************************
            }

            int iShipDays = Convert.ToInt32(drQD["ShipDays"]);
            lblShipDays.Text = iShipDays.ToString();        // Hidden.

            BuildCatalogNo(drQD["CatalogNumber"].ToString());

            string sQty = drQD["Quantity"].ToString();
            txtQuantity.Text = dv.NumberFormat(sQty, 3, 0);

            // Default unit price goes into label.
            string sUnitPrice = drQD["Price"].ToString();
            lblUnitPrice.Text = dv.NumberFormat(sUnitPrice, 9, 2);
            txtUnitPrice.Text = lblUnitPrice.Text;                      // Visible if no price change.
            txtUnitPriceChanged.Text = lblUnitPrice.Text;               // Visible if price change.

            // Actual unit price goes into textbox.
            string sUnitPriceCalc = drQD["PriceCalc"].ToString();
            lblUnitPriceCalc.Text = dv.NumberFormat(sUnitPriceCalc, 9, 2);

            DisplayPrice("Unit", "LoadQuoteDetails");

            lblWBKitName.Text = drQD["WBName"].ToString();
            lblWBKitID.Text = drQD["WBID"].ToString();

            bool bKitWB = (lblWBKitID.Text == null || lblWBKitName.Text == "" || lblWBKitID.Text == "0") ? false : true;

            if (bKitWB)
            {
                txtWBKitNumber.Text = drQD["WBNumber"].ToString();                   // Hidden field.
                lblWBKitName.Text = drQD["WBName"].ToString();
                string sWBQty = drQD["WBQuantity"].ToString();
                txtWBKitQty.Text = dv.NumberFormat(sWBQty, 3, 0);
                lblWBKitQtyOrig.Text = txtWBKitQty.Text;

                // Default WB price goes into label.
                string sWBPriceCalc = drQD["WBPriceCalc"].ToString();
                lblWBKitPriceCalc.Text = sWBPriceCalc;

                // Actual WB price goes into textbox.
                string sWBPrice = drQD["WBPrice"].ToString();

                if (sWBPrice == "" && sWBPriceCalc != "")
                    sWBPrice = sWBPriceCalc;

                lblWBKitPrice.Text = sWBPrice;
                txtWBKitPrice.Text = sWBPrice;
                DisplayPrice("WBKit", "LoadQuoteDetails");
            }
            else
            {
                txtWBKitQty.Text = " 0";
            }

            lblKitName.Text = drQD["KitName"].ToString();
            lblKitID.Text = drQD["KitID"].ToString();

            // Load Transformer object.
            int iKitID = 0;
            if (int.TryParse(lblKitID.Text.ToString(), out iKitID) == true)
            {
                t.KitID = iKitID;
            }
            else
            {
                t.KitID = 0;
            }

            lblKitIDOrig.Text = drQD["KitID"].ToString();                           // Used when editing.

            bool bKit = (lblKitID.Text == null || lblKitName.Text == "" || lblKitID.Text == "0") ? false : true;

            if (bKit)
            {
                txtKitNumber.Text = drQD["KitNumber"].ToString();                   // Hidden field.
                lblKitName.Text = drQD["KitName"].ToString();
                string sKitQty = drQD["KitQuantity"].ToString();
                txtKitQty.Text = dv.NumberFormat(sKitQty, 3, 0);
                lblKitQtyOrig.Text = txtKitQty.Text;

                // Default kit price goes into label.
                string sKitPriceCalc = drQD["KitPriceCalc"].ToString();
                lblKitPriceCalc.Text = sKitPriceCalc;

                // Actual kit price goes into textbox.
                string sKitPrice = drQD["KitPrice"].ToString();

                if (sKitPrice == "" && sKitPriceCalc != "")
                    sKitPrice = sKitPriceCalc;

                txtKitPrice.Text = sKitPrice;
                lblKitPrice.Text = sKitPrice;
                DisplayPrice("Kit", "LoadQuoteDetails");
            }
            else
            {
                txtKitQty.Text = " 0";
            }

            lblRBKitName.Text = drQD["RBKitName"].ToString();
            lblRBKitID.Text = drQD["RBKitID"].ToString();
            lblRBKitNameOrig.Text = drQD["RBKitName"].ToString();

            bool bKitRB = (lblRBKitID.Text == null || lblRBKitName.Text == "" || lblRBKitID.Text == "0") ? false : true;

            txtRBKitNumber.Text = drQD["RBKitNumber"].ToString();                   // Hidden field.
            lblRBKitName.Text = drQD["RBKitName"].ToString();
            string sRBKitQty = drQD["RBKitQuantity"].ToString();
            txtRBKitQty.Text = dv.NumberFormat(sRBKitQty, 3, 0);
            lblRBKitQtyOrig.Text = txtRBKitQty.Text;

            // Default RB kit price goes into label.
            string sRBKitPriceCalc = drQD["RBKitPriceCalc"].ToString();
            lblRBKitPriceCalc.Text = sRBKitPriceCalc;

            // Actual RB kit price goes into textbox.
            string sRBKitPrice = drQD["RBKitPrice"].ToString();

            if (sRBKitPrice == "" && sRBKitPriceCalc != "")
                sRBKitPrice = sRBKitPriceCalc;

            txtRBKitPrice.Text = sRBKitPrice;
            lblRBKitPrice.Text = sRBKitPrice;
            DisplayPrice("RBKit", "LoadQuoteDetails");

            // OSHPD Kit.
            lblOPKitName.Text = drQD["OPKitName"].ToString();
            lblOPKitID.Text = drQD["OPKitID"].ToString();
            lblOPKitNameOrig.Text = drQD["OPKitName"].ToString();

            bool bKitOP = (lblOPKitID.Text == null || lblOPKitName.Text == "" || lblOPKitID.Text == "0") ? false : true;

            txtOPKitNumber.Text = drQD["OPKitNumber"].ToString();                   // Hidden field.
            lblOPKitName.Text = drQD["OPKitName"].ToString();
            string sOPKitQty = drQD["OPKitQuantity"].ToString();
            txtOPKitQty.Text = dv.NumberFormat(sOPKitQty, 3, 0);
            lblOPKitQtyOrig.Text = txtOPKitQty.Text;

            // Default OP kit price goes into label.
            string sOPKitPriceCalc = drQD["OPKitPriceCalc"].ToString();
            lblOPKitPriceCalc.Text = sOPKitPriceCalc;

            // Actual OP kit price goes into textbox.
            string sOPKitPrice = drQD["OPKitPrice"].ToString();

            if (sOPKitPrice == "" && sOPKitPriceCalc != "")
                sOPKitPrice = sOPKitPriceCalc;

            txtOPKitPrice.Text = sOPKitPrice;
            lblOPKitPrice.Text = sOPKitPrice;

            DisplayPrice("OPKit", "LoadQuoteDetails");

            // Lug Kit.
            lblLugKitName.Text = drQD["LugKitName"].ToString();
            lblLugKitID.Text = drQD["LugKitID"].ToString();
            lblLugKitNameOrig.Text = drQD["LugKitName"].ToString();

            bool bKitLug = (lblLugKitID.Text == null || lblLugKitName.Text == "" || lblLugKitID.Text == "0") ? false : true;

            txtLugKitNumber.Text = drQD["LugKitNumber"].ToString();                   // Hidden field.
            lblLugKitName.Text = drQD["LugKitName"].ToString();
            string sLugKitQty = drQD["LugKitQuantity"].ToString();
            txtLugKitQty.Text = dv.NumberFormat(sLugKitQty, 3, 0);
            lblLugKitQtyOrig.Text = txtLugKitQty.Text;

            // Default Lug kit price goes into label.
            string sLugKitPriceCalc = drQD["LugKitPriceCalc"].ToString();
            lblLugKitPriceCalc.Text = sLugKitPriceCalc;

            // Actual Lug kit price goes into textbox.
            string sLugKitPrice = drQD["LugKitPrice"].ToString();

            if (sLugKitPrice == "" && sLugKitPriceCalc != "")
                sLugKitPrice = sLugKitPriceCalc;

            txtLugKitPrice.Text = sLugKitPrice;
            lblLugKitPrice.Text = sLugKitPrice;

            DisplayPrice("LugKit", "LoadQuoteDetails");

            // Expedite.
            string sExpediteNoDays = drQD["ExpediteNoDays"].ToString();
            sExpediteNoDays = (sExpediteNoDays == "" || sExpediteNoDays == null) ? "0" : sExpediteNoDays;
            int iNoDays = 0;
            int.TryParse(sExpediteNoDays, out iNoDays);
            if (iNoDays > 0)
            {
                // This reloads ddlExpedite with current values, keeping prices the same.
                CatalogNumberUpdate(false);
            }

            // See if we're in a situation in which lead times are exact - 15 business days.
            bool bExact = LeadTimesExact();

            // Reduce ship days to 14 just for expedite if bExact is true, to avoid including a 15 business day expedite.
            int iExpediteShipDays = iShipDays;

            if (bExact == true && iShipDays == 15)
            {
                iExpediteShipDays = 14;
            }

            bool bLeadTimesExact = LeadTimesExact();
            LoadExpedite(iExpediteShipDays, iNoDays, bLeadTimesExact);

            ddlExpedite.SelectedValue = (sExpediteNoDays == "0" || sExpediteNoDays == "") ? "" : sExpediteNoDays;
            sExpediteNoDays = dv.NumberFormat(sExpediteNoDays, 2, 0);           // No of days 5 = " 5".


            // Default expedite price goes into label.
            string sExpeditePriceCalc = drQD["ExpeditePriceCalc"].ToString();
            lblExpeditePriceCalc.Text = sExpeditePriceCalc;

            // Actual expedite price goes into textbox.
            string sExpeditePrice = drQD["ExpeditePrice"].ToString();

            if (sExpeditePrice == "" && sExpeditePriceCalc != "")
                sExpeditePrice = sExpeditePriceCalc;

            txtExpeditePrice.Text = sExpeditePrice;
            lblExpeditePrice.Text = sExpeditePrice;

            decimal decUnitPrice = dv.DecimalFromText(lblUnitPrice.Text.ToString());
            Int16 iQty = dv.Int16FromText(txtQuantity.Text.ToString());

            decimal decExtPrice = (decimal)decUnitPrice * iQty;

            decimal decExpediteFees = dv.DecimalFromText(sExpeditePrice);
            decimal decExpediteFeesCalc = q.ExpediteFees(iQuoteID, decUnitPrice, iNoDays);
            decimal decExpediteExtFees = decExpediteFees * iQty;

            lblExpeditePrice.Text = dv.NumberFormat(decExpediteFees.ToString(), 9, 2);
            lblExpediteExtPrice.Text = dv.NumberFormat(decExpediteExtFees.ToString(), 9, 2);

            DisplayPrice("Expedite", "LoadQuoteDetails");

            bool bNoFreeShipping = Convert.ToBoolean(drQD["NoFreeShipping"]);
            chkNoFreeShipping.Checked = bNoFreeShipping;

            string sShipAmount = drQD["ShipAmount"].ToString();
            if (String.IsNullOrEmpty(sShipAmount) == false)
            {
                sShipAmount = dv.NumberFormat(sShipAmount, 9, 2, true);
            }
            txtShippingAmount.Text = sShipAmount;

            string sShipReason = drQD["ShipReason"].ToString();
            txtShippingReason.Text = sShipReason;
            lblShippingReason.Text = sShipReason;
            lblShippingAmount.Text = sShipAmount;
            lblShippingAmtExt.Text = sShipAmount;
            lblShippingAmountInvalid.Text = "";

            // Formats all the prices.
            UpdatePrices("LoadQuoteDetails");

            // Do this at the end so it doesn't get reset to zero.
            txtDetailID.Text = sDetailID;

            if (sDetailID == "0" || sDetailID == "" || sDetailID == null)
            {
                lblAddEdit.Text = "Add Item to Quote";
                lblAddEdit.ForeColor = Color.Black;
            }
            else
            {
                lblAddEdit.Text = "Editing Item #" + sDetailID;
                lblAddEdit.ForeColor = Color.Blue;
            }

            // Identify correct enclosure settings based on everything entered.
            // Calls usp_CatalogNumber.
            //EnclosureUpdate();

            // Disable approval while editing.
            EnableFinalizeButton();
            Display(enumDisplay.All);
        }

        protected void LoadKVA(bool bCustom, bool bPhaseSingle, bool bWindingsCopper, bool bZigZag)
        {
            lblGeneral.Visible = false;

            ddlKVA.Items.Clear();
            decimal decKva = 0;
            decimal decKvaMin = 0;
            decimal decKvaMax = 0;

            string sKva = txtKVA.Text;
            string sKvaTemp = "";

            // Save decimal value of current KVA.
            decimal.TryParse(sKva, out decKva);
            
            txtKVA.Text = "";

            DataTable dt = q.KVA(bCustom, bPhaseSingle, bWindingsCopper, bZigZag);
            ddlKVA.DataTextField = "KVA";
            ddlKVA.DataValueField = "KVA";

            DataRow dr = dt.NewRow();
            dr["KVA"] = "";
            dt.Rows.InsertAt(dr, 0);

            if (ddlKVA.SelectedIndex == -1)
                ddlKVA.SelectedValue = null;

            ddlKVA.DataSource = dt;
            ddlKVA.DataBind();

            int i = 0;
            for (i=0; i < ddlKVA.Items.Count; i++)
            {
                // Get the lowest KVA allowed.
                // NOTE: Item zero is blank.
                if (i == 1)
                {
                    sKvaTemp = ddlKVA.Items[i].Text.Trim();
                    decimal.TryParse(sKvaTemp, out decKvaMin);
                }
                // Get the highest KVA allowed.
                if (i == ddlKVA.Items.Count - 1)
                {
                    sKvaTemp = ddlKVA.Items[i].Text.Trim();
                    decimal.TryParse(sKvaTemp, out decKvaMax);
                }

                // If this KVA is in the list,
                if (ddlKVA.Items[i].Text.Trim() == sKva)
                {
                    // Put it back in the textbox.
                    txtKVA.Text = sKva;
                    break;
                }
            }

            // If our value isn't in the list, come up with a close approximation.
            if (txtKVA.Text == "") 
            {
                // Example:  80 KVA isn't in the list, but is in the range, so return it.
                if (decKvaMin <= decKva && decKva <= decKvaMax)
                {
                    txtKVA.Text = sKva;
                }
            }
        }

        protected void LoadCaseSizes()
        {
            ddlCaseSizes.Items.Clear();

            DataTable dt = q.CaseSizes();
            ddlCaseSizes.DataTextField = "CaseSize";
            ddlCaseSizes.DataValueField = "CaseSize";

            DataRow dr = dt.NewRow();
            dr["CaseSize"] = "";
            dt.Rows.InsertAt(dr, 0);

            ddlCaseSizes.DataSource = dt;

            try
            {
                ddlCaseSizes.DataBind();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception: " + ex);
            }
        }


        /// <summary>
        /// Set an Efficiency in the list.
        /// Translate it if necessary.
        /// </summary>
        /// <param name="sKVA"></param>
        protected void SetEfficiency(string sEfficiency)
        {
            switch (sEfficiency)
            {
                case "NEMA TP1":
                    sEfficiency = "EXEMPT";
                    break;
                case "NEMA Premium":
                    sEfficiency = "DOE2016";
                    break;
                case "DOE2016":
                    sEfficiency = "DOE2016";
                    break;
                default:
                    sEfficiency = "EXEMPT";
                    break;
            }
            lblEfficiencyValue.Text = sEfficiency;

            ddlEfficiency.SelectedValue = sEfficiency;
        }

        /// <summary>
        /// Set a KVA in the list.
        /// </summary>
        /// <param name="sKVA"></param>
        protected void SetKVA(string sKVA)
        {
            for (int i = 0; i < ddlKVA.Items.Count; i++)
            {
                if (ddlKVA.Items[i].Value == sKVA)
                {
                    ddlKVA.SelectedValue = sKVA;
                    break;
                }
            }
        }

        /// <summary>
        /// Set a Configuration in the list.
        /// </summary>
        /// <param name="sConfiguration"></param>
        protected bool SetConfiguration(string sConfiguration)
        {
            for (int i = 0; i < ddlConfiguration.Items.Count; i++)
            {
                if (ddlConfiguration.Items[i].Value == sConfiguration)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Set a KFactor in the list.
        /// If not found, default to K-1 (STD).
        /// </summary>
        /// <param name="sKFactor"></param>
        protected void SetKFactor(string sKFactor)
        {
            for (int i = 0; i < ddlKFactor.Items.Count; i++)
            {
                if (ddlKFactor.Items[i].Value == sKFactor)
                {
                    ddlKFactor.SelectedValue = sKFactor;
                    return;
                }
            }
            // If this code hits, none of the above were selected.
            ddlKFactor.SelectedValue = "K-1 (STD)"; // Default.

            // Turn off K-20 special feature if it's checked, because the
            // K-Factor is now K-1.
            if (FindSpecialFeature("K-Factor 20 (K-20)") == true)
            {
                chkLstSpecialFeatures.Items[4].Selected = false;
            }
        }

        /// <summary>
        /// Set a TempRise in the list.
        /// If not found, default to 150 (STD).
        /// </summary>
        /// <param name="sKFactor"></param>
        protected void SetTempRise(string sTempRise)
        {
            for (int i = 0; i < ddlTempRise.Items.Count; i++)
            {
                if (ddlTempRise.Items[i].Value == sTempRise)
                {
                    ddlTempRise.SelectedValue = sTempRise;
                    return;
                }
            }
        }

        /// <summary>
        /// Set a Case Color in the list.
        /// </summary>
        /// <param name="sCaseSize"></param>
        protected void SetCaseColor(string sCaseColor)
        {
            // If no case color, then out of range.
            if (sCaseColor != "")
            {
                for (int i = 0; i < ddlCaseColor.Items.Count; i++)
                {
                    if (ddlCaseColor.Items[i].Value == sCaseColor)
                    {
                        ddlCaseColor.SelectedValue = sCaseColor;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Set a Rep in the list.
        /// </summary>
        /// <param name="sCaseSize"></param>
        protected void SetRep(string sRep)
        {
            // If no Rep, then out of range.
            if (sRep != "")
            {
                for (int i = 0; i < ddlRep.Items.Count; i++)
                {
                    if (ddlRep.Items[i].Value == sRep)
                    {
                        ddlRep.SelectedValue = sRep;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// Set a sound reduction in the list.
        /// </summary>
        /// <param name="sCaseSize"></param>
        protected void SetSoundReduct(string sSoundReduct)
        {
            // If no sound reduction, then out of range.
            if (sSoundReduct != "")
            {
                for (int i = 0; i < ddlSoundReduct.Items.Count; i++)
                {
                    if (ddlSoundReduct.Items[i].Value == sSoundReduct)
                    {
                        ddlSoundReduct.SelectedValue = sSoundReduct;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Set an enclosure in the list.
        /// </summary>
        /// <param name="sCaseSize"></param>
        protected void SetEnclosure(string sEnclosure)
        {
            // If no enclosure, then out of range.
            if (sEnclosure != "")
            {
                for (int i = 0; i < ddlEnclosure.Items.Count; i++)
                {
                    if (ddlEnclosure.Items[i].Value == sEnclosure)
                    {
                        ddlEnclosure.SelectedValue = sEnclosure;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Set a Frequency in the list.
        /// </summary>
        /// <param name="sCaseSize"></param>
        protected void SetFrequency(string sFrequency)
        {
            // If no frequency, then out of range.
            if (sFrequency != "")
            {
                for (int i = 0; i < ddlFrequency.Items.Count; i++)
                {
                    if (ddlFrequency.Items[i].Value == sFrequency)
                    {
                        ddlFrequency.SelectedValue = sFrequency;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Set a Case Size in the list.
        /// </summary>
        /// <param name="sCaseSize"></param>
        protected void SetCaseSize(string sCaseSize)
        {
            // If no case size, then out of range.
            if (sCaseSize != "")
            {
                for (int i = 0; i < ddlCaseSizes.Items.Count; i++)
                {
                    if (ddlCaseSizes.Items[i].Value == sCaseSize)
                    {
                        ddlCaseSizes.SelectedValue = sCaseSize;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Select a primary voltage if possible.
        /// </summary>
        /// <param name="sPrimaryVoltage"></param>
        protected void SetPrimaryVoltage(string sPrimaryVoltage)
        {
            for (int i = 0; i < ddlPrimaryVoltage.Items.Count; i++)
            {
                if (ddlPrimaryVoltage.Items[i].Value == sPrimaryVoltage)
                {
                    ddlPrimaryVoltage.SelectedValue = sPrimaryVoltage;
                    break;
                }
            }
        }

        /// <summary>
        /// Select a secondary voltage if possible.
        /// </summary>
        /// <param name="sPrimaryVoltage"></param>
        protected void SetSecondaryVoltage(string sSecondaryVoltage)
        {
            for (int i = 0; i < ddlSecondaryVoltage.Items.Count; i++)
            {
                if (ddlSecondaryVoltage.Items[i].Value == sSecondaryVoltage)
                {
                    ddlSecondaryVoltage.SelectedValue = sSecondaryVoltage;
                    break;
                }
            }
        }


        /// <summary>
        /// Matching KVA is necessary because KVA values are formatted with leading spaces.
        /// </summary>
        /// <param name="sKVA"></param>
        /// <returns></returns>
        protected string MatchKVA(string sKVA)
        {
            decimal dKVA = 0;
            decimal.TryParse(sKVA, out dKVA);

            decimal dMatch = 0;
            string sMatch = "";

            for (int i = 0; i < ddlKVA.Items.Count; i++)
            {
                sMatch = ddlKVA.Items[i].Value.ToString().Trim();

                decimal.TryParse(sMatch, out dMatch);

                if (dMatch == dKVA)
                    return ddlKVA.Items[i].Value;
            }
            return "";
        }

        protected void LoadConfiguration(decimal dKVA, bool bPhaseSingle, bool bWindingsCopper)
        {
            string sConfig = "";
            if (ddlConfiguration.SelectedValue != null)
                sConfig = ddlConfiguration.SelectedValue;

            ddlConfiguration.Items.Clear();
            if (dKVA == 0)      // No KVA selected.
                return;

            DataTable dt = q.StockConfigurations(dKVA, bPhaseSingle, bWindingsCopper);
            ddlConfiguration.DataTextField = "Configuration";
            ddlConfiguration.DataValueField = "Configuration";

            DataRow dr = dt.NewRow();
            dr["Configuration"] = "";
            dt.Rows.InsertAt(dr, 0);

            ddlConfiguration.DataSource = dt;
            ddlConfiguration.DataBind();

            // Single phase has only one configuration.
            if (bPhaseSingle == true && ddlConfiguration.Items.Count > 1)
                ddlConfiguration.SelectedIndex = 1;
            // Restore value, if possible.
            else
            {
                for (int i = 0; i < ddlConfiguration.Items.Count; i++)
                {
                    if (ddlConfiguration.Items[i].Value == sConfig)
                    {
                        ddlConfiguration.Items[i].Selected = true;
                        break;
                    }
                }
            }
        }

        protected void LoadPrimaryVoltage(bool bDelta, bool bHarmonic, bool bSingle)
        {
            bool bMatch = false;
            bool bText = false;
            string sPV = "";
            if (ddlPrimaryVoltage.Items.Count > 0)
            {
                sPV = ddlPrimaryVoltage.SelectedValue;
            }
            if (sPV == "")
            {
                sPV = txtPrimaryVoltage.Text;
                bText = true;
            }

            ddlPrimaryVoltage.Items.Clear();

            DataTable dt = q.Voltage(true, bDelta, bHarmonic, bSingle);

            ddlPrimaryVoltage.DataTextField = "Voltage";
            ddlPrimaryVoltage.DataValueField = "Voltage";

            DataRow dr4 = dt.NewRow();           // Dividing row.
            dr4["Voltage"] = "";
            dt.Rows.InsertAt(dr4, 0);

            if (bSingle == false)
            {
                // Don't put these "convenience" rows in for Harmonic Mitigating.
                if (!bHarmonic)
                {
                    if (bDelta == true)
                    {
                        DataRow dr3 = dt.NewRow();
                        dr3["Voltage"] = "208 D";        // Third to appear.
                        dt.Rows.InsertAt(dr3, 0);

                        DataRow dr2 = dt.NewRow();
                        dr2["Voltage"] = "240 D";        // Second to appear.
                        dt.Rows.InsertAt(dr2, 0);

                        DataRow dr1 = dt.NewRow();
                        dr1["Voltage"] = "480 D";        // First to appear.
                        dt.Rows.InsertAt(dr1, 0);
                    }
                    else
                    {
                        DataRow dr2 = dt.NewRow();
                        dr2["Voltage"] = "208Y/120";        // Second to appear.
                        dt.Rows.InsertAt(dr2, 0);

                        DataRow dr1 = dt.NewRow();
                        dr1["Voltage"] = "480Y/277";        // First to appear.
                        dt.Rows.InsertAt(dr1, 0);
                    }
                }
            }
            else
            {
                DataRow dr1 = dt.NewRow();
                dr1["Voltage"] = "240x480";
                dt.Rows.InsertAt(dr1, 0);
            }

            DataRow dr = dt.NewRow();
            dr["Voltage"] = "";                     // Row at top.
            dt.Rows.InsertAt(dr, 0);

            ddlPrimaryVoltage.SelectedValue = "";
            ddlPrimaryVoltage.DataSource = dt;
            ddlPrimaryVoltage.DataBind();

            // Restore the value if possible.
            if (sPV != "")
            {
                for (int i = 0; i < ddlPrimaryVoltage.Items.Count; i++)
                {
                    dv.Debug(ddlPrimaryVoltage.Items[i].Text.Trim() + ' ' + sPV);

                    if (dv.VoltageCompare(ddlPrimaryVoltage.Items[i].Text.Trim(), sPV) == true)
                    {
                        bMatch = true;
                        if (bText == true || txtPrimaryVoltage.Visible == true)
                        {
                            txtPrimaryVoltage.Text = ddlPrimaryVoltage.Items[i].Text.Trim();
                        }
                        else
                        {
                            ddlPrimaryVoltage.SelectedValue = ddlPrimaryVoltage.Items[i].Text.Trim();
                        }
                        break;
                    }
                }
            }

            // Remove text if we didn't match on the dropdown.
            if (!bMatch)
            {
                txtPrimaryVoltage.Text = "";
            }
        }

        protected void LoadSecondaryVoltage(bool bWye, bool bHarmonic, bool bSingle)
        {
            bool bMatch = false;
            bool bText = false;
            string sSV = "";
            if (ddlSecondaryVoltage.Items.Count > 0)
            {
                sSV = ddlSecondaryVoltage.SelectedValue;
            }
            if (sSV == "")
            {
                sSV = txtSecondaryVoltage.Text;
                bText = true;
            }

            ddlSecondaryVoltage.Items.Clear();

            DataTable dt = q.Voltage(false, !bWye, bHarmonic, bSingle);

            ddlSecondaryVoltage.DataTextField = "Voltage";
            ddlSecondaryVoltage.DataValueField = "Voltage";

            DataRow dr4 = dt.NewRow();           // Dividing row.
            dr4["Voltage"] = "";
            dt.Rows.InsertAt(dr4, 0);

            if (bSingle == false)
            {
                // Don't put these "convenience" rows in for Harmonic Mitigating.
                if (!bHarmonic)
                {
                    if (bWye == true)
                    {
                        DataRow dr2 = dt.NewRow();
                        dr2["Voltage"] = "208Y/120";        // Second to appear.
                        dt.Rows.InsertAt(dr2, 0);

                        DataRow dr1 = dt.NewRow();
                        dr1["Voltage"] = "480Y/277";        // First to appear.
                        dt.Rows.InsertAt(dr1, 0);
                    }
                    else
                    {
                        DataRow dr3 = dt.NewRow();
                        dr3["Voltage"] = "208 D";        // Third to appear.
                        dt.Rows.InsertAt(dr3, 0);

                        DataRow dr2 = dt.NewRow();
                        dr2["Voltage"] = "480 D";        // Second to appear.
                        dt.Rows.InsertAt(dr2, 0);

                        DataRow dr1 = dt.NewRow();
                        dr1["Voltage"] = "240D/120CT";   // First to appear.
                        dt.Rows.InsertAt(dr1, 0);
                    }
                }
            }
            else
            {
                DataRow dr2 = dt.NewRow();
                dr2["Voltage"] = "240/480";          // Second to appear.
                dt.Rows.InsertAt(dr2, 0);

                DataRow dr1 = dt.NewRow();
                dr1["Voltage"] = "120/240";          // First to appear.
                dt.Rows.InsertAt(dr1, 0);
            }
            DataRow dr = dt.NewRow();
            dr["Voltage"] = "";                     // Row at top.
            dt.Rows.InsertAt(dr, 0);

            ddlSecondaryVoltage.SelectedValue = "";
            ddlSecondaryVoltage.DataSource = dt;
            ddlSecondaryVoltage.DataBind();

            // Restore the value if possible.
            if (sSV != "")
            {
                for (int i = 0; i < ddlSecondaryVoltage.Items.Count; i++)
                {
                    dv.Debug(ddlSecondaryVoltage.Items[i].Text.Trim() + ' ' + sSV);

                    if (dv.VoltageCompare(ddlSecondaryVoltage.Items[i].Text.Trim(), sSV) == true)
                    {
                        bMatch = true;
                        if (bText == true || txtSecondaryVoltage.Visible == true)
                        {
                            txtSecondaryVoltage.Text = ddlSecondaryVoltage.Items[i].Text.Trim();
                        }
                        else
                        {
                            ddlSecondaryVoltage.SelectedValue = ddlSecondaryVoltage.Items[i].Text.Trim();
                        }
                        break;
                    }
                }
            }

            // Remove text if we failed to match.
            if (bMatch == false)
            {
                txtSecondaryVoltage.Text = "";
            }
        }

        protected void LoadFrequency()
        {
            ddlFrequency.Items.Clear();

            DataTable dt = q.Frequency();
            ddlFrequency.DataTextField = "Frequency";
            ddlFrequency.DataValueField = "Frequency";

            SetFrequency("60 Hz (STD)");
            ddlFrequency.DataSource = dt;
            ddlFrequency.DataBind();
        }

        protected void LoadEfficiency(string sEfficiencyCode)
        {
            ddlEfficiency.Items.Clear();

            DataTable dt = q.Efficiencies();
            ddlEfficiency.DataTextField = "Efficiency";
            ddlEfficiency.DataValueField = "Efficiency";

            if (sEfficiencyCode == "DOE2016" || sEfficiencyCode == "EXEMPT")
                SetEfficiency(sEfficiencyCode);

            ddlEfficiency.DataSource = dt;
            ddlEfficiency.DataBind();
        }

        protected void LoadSoundReduct(int iKVA)
        {
            string sSoundReduct = "";
            if (ddlSoundReduct.SelectedValue != null)
                sSoundReduct = ddlSoundReduct.SelectedValue;

            ddlSoundReduct.Items.Clear();

            DataTable dt = q.SoundReduct(iKVA);
            ddlSoundReduct.DataTextField = "SoundReduct";
            ddlSoundReduct.DataValueField = "DBReduced";

            // Allow a dummy record for CustomStockId = 0, so system can perform normally
            // when there's no specific data.
            ddlSoundReduct.DataSource = dt;

            try
            {
                ddlSoundReduct.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex);
            }

            // Restore value, if possible.
            if (sSoundReduct != "")
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][1].ToString() == sSoundReduct)
                    {

                        SetSoundReduct(sSoundReduct);
                        break;
                    }
                }
            }
        }


        // Load the Enclosure list.
        protected void LoadEnclosure(int iAllowIndoor, int iAllowTENV)
        {
            ddlEnclosure.Items.Clear();
            ddlEnclosure.SelectedValue = null;

            DataTable dt = q.EnclosureList(iAllowIndoor, iAllowTENV);
            ddlEnclosure.DataTextField = "Enclosure";
            ddlEnclosure.DataValueField = "Enclosure";

            ddlEnclosure.DataSource = dt;

            try
            {
                ddlEnclosure.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex);
            }
        }

        protected void LoadSpecialFeatures()
        {
            chkLstSpecialFeatures.Items.Clear();

            bool bExempt = IsExempt();
            bool bInternal = Convert.ToBoolean(Session["Internal"]);
            string sPhase = rblPhase.SelectedValue;
            string sKVA = ddlKVA.SelectedValue;
            sKVA = string.IsNullOrEmpty(sKVA) == true ? "" : sKVA;
            if (sKVA == "") sKVA = txtKVA.Text.ToString();
            decimal dKVA = 0;
            decimal.TryParse(sKVA, out dKVA);

            // Using Exempt to cover whether or not K-20 appears in list.
            // If Exempt, K-20 does not appear.
            if (dKVA > 500) bExempt = true;

            DataTable dt = q.SpecialFeatures(bInternal, bExempt, sPhase);
            chkLstSpecialFeatures.DataTextField = "SpecialFeatures";
            chkLstSpecialFeatures.DataValueField = "SpecialFeatures";
            chkLstSpecialFeatures.DataSource = dt;
            chkLstSpecialFeatures.DataBind();

            // Set default to "None".
            chkLstSpecialFeatures.Items[0].Selected = true;
        }

        /// <summary>
        /// Return True if requested special feature found in current record.
        /// </summary>
        /// <param name="sFeatureName"></param>
        /// <returns></returns>
        protected bool FindSpecialFeature(string sFeatureName)
        {
            for (int i = 0; i < chkLstSpecialFeatures.Items.Count; i++)
            {
                if (chkLstSpecialFeatures.Items[i].Value == sFeatureName && chkLstSpecialFeatures.Items[i].Selected == true)
                    return true;
            }
            return false;
        }

        protected bool FindSoundReduct(string sSoundReduct)
        {
            for (int i = 0; i < ddlSoundReduct.Items.Count; i++)
            {
                if (ddlSoundReduct.Items[i].Value == sSoundReduct)
                    return true;
            }
            return false;
        }

        protected void LoadCaseColor(string sEnclosure)
        {
            bool bStainless = false;
            sEnclosure = string.IsNullOrEmpty(sEnclosure) == true ? "" : sEnclosure;
            if (sEnclosure.Length >= 9)
            {
                if (sEnclosure.Substring(0, 9) == "Stainless")
                    bStainless = true;
            }

            ddlCaseColor.Items.Clear();

            DataTable dt = q.CaseColor(bStainless);
            ddlCaseColor.DataTextField = "CaseColor";
            ddlCaseColor.DataValueField = "CaseColor";

            SetCaseColor("ANSI 61 (STD)");
            ddlCaseColor.DataSource = dt;
            ddlCaseColor.DataBind();
        }

        /// <summary>
        /// NOTE: bTenv is whether TENV is selected, not whether it is available.
        /// </summary>
        /// <param name="sPhase"></param>
        /// <param name="sKVA"></param>
        /// <param name="bExempt"></param>
        /// <param name="bTenv"></param>
        protected void LoadKFactor(string sPhase, string sKVA, bool bExempt, bool bTenv)
        {
            string sKFactor = "K-1 (STD)";

            if (ddlKFactor.Items.Count > 0)
            {
                sKFactor = ddlKFactor.SelectedValue;
            }

            ddlKFactor.Items.Clear();

            bool bAdmin = Convert.ToBoolean(Session["Admin"]);

            DataTable dt = q.KFactor(sPhase, sKVA, bExempt, bAdmin, bTenv);
            ddlKFactor.DataTextField = "KFactor";
            ddlKFactor.DataValueField = "KFactor";

            ddlKFactor.DataSource = dt;
            try
            {
                ddlKFactor.DataBind();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Exception: " + e);
            }

            SetKFactor(sKFactor);
        }

        /// <summary>
        /// Load Rep Name dropdown.
        /// </summary>
        /// <param name="iRepID"></param>
        protected void LoadRep(int iRepID, string sUserName)
        {
            if (sUserName == null || sUserName == "")
            {
                sUserName = Session["UserName"].ToString();
            }

            // These fields should already contain who created and last updated the quote.
            string sFullNameCreated = lblFullNameCreated.Text.ToString();
            string sUserNameCreated = hidUserNameCreated.Value.ToString();

            string sFullNameLatest = lblFullNameLatest.Text.ToString();
            string sUserNameLatest = hidUserNameLatest.Value.ToString();

            string sFullNameNow = Session["FullName"].ToString();
            string sUserNameNow = Session["UserName"].ToString();

            ddlRep.Items.Clear();

            // Get all the logins for this rep.
            DataTable dt = q.RepName(iRepID);
            DataRow dr;

            // Add the current user if not found in the list of reps.
            bool bAddUser = true;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                if (dr["UserName"].ToString() == sUserNameNow)
                {
                    bAddUser = false;
                    break;
                }
            }

            if (bAddUser == true)
            {
                dr = dt.NewRow();
                dr["FullName"] = sFullNameNow;
                dr["UserName"] = sUserNameNow;
                dt.Rows.InsertAt(dr, 0);
            }

            // Add the latest user if not found in the list of reps.
            bAddUser = true;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                if (dr["UserName"].ToString() == sUserNameLatest)
                {
                    bAddUser = false;
                    break;
                }
            }

            if (bAddUser == true)
            {
                dr = dt.NewRow();
                dr["FullName"] = sFullNameLatest;
                dr["UserName"] = sUserNameLatest;
                dt.Rows.InsertAt(dr, 0);
            }

            // Add theuser who created the quote if not found in the list of reps.
            bAddUser = true;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                if (dr["UserName"].ToString() == sUserNameCreated)
                {
                    bAddUser = false;
                    break;
                }
            }

            if (bAddUser == true)
            {
                dr = dt.NewRow();
                dr["FullName"] = sFullNameCreated;
                dr["UserName"] = sUserNameCreated;
                dt.Rows.InsertAt(dr, 0);
            }

            // Add regular users for this rep.
            ddlRep.DataTextField = "FullName";
            ddlRep.DataValueField = "UserName";

            ddlRep.DataSource = dt;
            ddlRep.DataBind();

            // The display should be the user name recorded as the creator of this quote.
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                if (dr["UserName"].ToString() == sUserName)
                {
                    SetRep(sUserName);
                    break;
                }
            }
        }

        /// <summary>
        /// Note: bTENV is whether or not TENV is actually selected, not whether it is available.
        /// </summary>
        /// <param name="sPhase"></param>
        /// <param name="sKVA"></param>
        /// <param name="sKFactor"></param>
        /// <param name="bTENV"></param>
        protected void LoadTempRise(string sPhase, string sKVA, string sKFactor, bool bTENV)
        {
            string sTempRise = "150 (STD)";
            if (ddlTempRise.Items.Count > 0)
            {
                sTempRise = ddlTempRise.SelectedValue;
            }

            ddlTempRise.Items.Clear();

            bool bAdmin = Convert.ToBoolean(Session["Admin"]);

            DataTable dt = q.TempRise(sPhase, sKVA, sKFactor, bAdmin, bTENV);
            ddlTempRise.DataTextField = "TempRise";
            ddlTempRise.DataValueField = "TempRise";

            try
            {
                ddlTempRise.DataSource = dt;
                ddlTempRise.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }

            // Add (STD) to default choice.
            if (ddlTempRise.Items.Count > 0)
            {
                ddlTempRise.Items[0].Text = "150 (STD)";
            }

            // Reload the temp rise if possible.
            SetTempRise(sTempRise);

        }

        /// <summary>
        /// Customer DropDown has positive values for CustomerContactID,
        /// and negative values for CustomerID (i.e. no specific CustomerContact record.)
        /// </summary>
        /// <param name="iRepID"></param>
        protected void LoadCustomers(int iRepID, string sCustomerPart)
        {
            RepObject r = new RepObject();
            DataTable dt = r.Customers(iRepID, sCustomerPart);

            ddlCustomer.Items.Clear();

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr["CustomerContactID"] = 0;
                dr["Customer"] = "";
                dt.Rows.InsertAt(dr, 0);

                ddlCustomer.DataTextField = "Customer";
                ddlCustomer.DataValueField = "CustomerContactID";
                ddlCustomer.DataSource = dt;
                ddlCustomer.DataBind();
            }
        }

        /// <summary>
        /// Load Expedite Fee List.
        /// </summary>
        protected void LoadExpedite(int iShipDaysMax, int iShipDaysSelected, bool bExact)
        {
            // Using Rep to have the list include only # days < Rep's No days for delivery.
            RepObject r = new RepObject();
            int iRepID = Convert.ToInt32(lblRepID.Text);
            DataTable dt = r.ExpediteFeesList(iShipDaysMax, bExact);
            bool bExpediteKept = true;

            ddlExpedite.Items.Clear();
            ddlExpedite.SelectedIndex = -1;
            ddlExpedite.SelectedValue = null;

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.NewRow();
                dr["DisplayDays"] = "";
                dr["NoDays"] = "";
                dt.Rows.InsertAt(dr, 0);

                ddlExpedite.DataTextField = "DisplayDays";
                ddlExpedite.DataValueField = "NoDays";
                ddlExpedite.DataSource = dt;
                ddlExpedite.DataBind();
            }
            if (iShipDaysSelected > 0)
            {
                bExpediteKept = false;
                for (int i = 0; i < ddlExpedite.Items.Count; i++)
                {
                    if (ddlExpedite.Items[i].Value == iShipDaysSelected.ToString())
                    {
                        ddlExpedite.SelectedIndex = i;
                        bExpediteKept = true;
                        break;
                    }
                }
            }
            if (bExpediteKept == false)
            {
                lblGeneral.Text = iShipDaysSelected.ToString().Trim() + " day expedite removed.<br />Not offered in this configuration.";
            }
         }

        /// <summary>
        /// Sets the full customer in the list of customers 
        /// if the first portion of that customer exists.
        /// </summary>
        /// <param name="sCustomer"></param>
        /// <returns></returns>
        protected bool SetCustomer(string sCustomer)
        {
            int iLen = sCustomer.Length;
            sCustomer = sCustomer.ToLower();
            string sText = "";
            string sFull = "";

            foreach (ListItem li in ddlCustomer.Items)
            {
                sFull = li.Text;
                sText = sFull.ToLower();
                if (iLen < sText.Length)
                {
                    sText = sText.Substring(0, iLen);
                }
                if (sText == sCustomer)
                {
                    // Store ID for future use if they say "Yes"
                    // to using it.
                    lblCompanyID.Text = li.Value.ToString();
                    return true;
                }
            }
            return false;
        }

        protected void SetStandardOrCustom(bool bCustom)
        {
            bool bEditing = false;
            if (txtDetailID.Text != null && txtDetailID.Text != "" && txtDetailID.Text != "0")
            {
                bEditing = true;
            }
            int iStdCustom = (bCustom == true) ? 2 : 1;

            lblStandardOrCustom.Text = (bCustom) ? "Custom Transformer" : "Stock Transformer";

            HideCopper();       // Hide Copper if Stock, Single Phase.

            // Remove options if switching to Standard.
            if (!bCustom)
            {
                ClearEntries("StandardOrCustom");
            }

            ResetDetail(iStdCustom, bEditing);

            // See if we can match the KVA.
            if (!bCustom)
            {
                MatchKVA(hidKVA.ToString());
            }
        }

        protected void btnCopyQuote_Click(object sender, EventArgs e)
        {
            int iQuoteID = Convert.ToInt32(lblQuoteID.Text);
            int iRepID = Convert.ToInt32(lblRepID.Text);
            int iRepDistributorID = Convert.ToInt32(Session["RepDistributorID"]);
            int iSameQuoteNo = (chkSameQuoteNo.Checked == true) ? 1 : 0;

            bool bInternal = Convert.ToBoolean(Session["Internal"]);
            string sQuoteCopyCode = (bInternal == true) ? "A" : "Q";        // A = Created by Internal (Admin).  Q = Regular quote.

            string sUserName = Session["UserName"].ToString();
            string sQuoteOriginCode = r.OriginCode(iRepID, sUserName);

            Quotes q = new Quotes();

            DataSet ds = q.QuoteCopy(iQuoteID, iSameQuoteNo, sUserName);

            iQuoteID = Convert.ToInt32(ds.Tables[0].Rows[0]["QuoteID"]);

            Session["QuoteID"] = iQuoteID;

            // Will be reset to False in Page_Unload();
            bButtonPress = true;

            // Bring up new quote.
            Response.Redirect(Request.Path);
        }

        protected void rblStandardOrCustom_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bCustom = IsCustom();

            // Set to Same as Stock when manually changed from Stock to Custom.
            if (String.IsNullOrEmpty(lblCatalogNo.Text.ToString()) == false && bCustom == true)
            {
                hidIsMatch.Value = "1";
            }

            SetStandardOrCustom(bCustom);

            //if (bCustom == true)                // Set Custom default to Shielded.
            //    rblElectrostaticShield.Text = "Shielded";
            //else
            //    rblElectrostaticShield.Text = "None";

            CatalogNumberUpdate(true);
            NextFocus("rblStandardOrCustom");

            //if (ddlKVA.Text == "" || ddlConfiguration.Text == "")
            //    tblInventory.Visible = false;
            //else
            //    tblInventory.Visible = true;
            ShowInventoryTable();
        }


        private void ShowInventoryTable()
        {

            if (ddlKVA.Text == "" || ddlConfiguration.Text == "")
                tblInventory.Visible = false;
            else
                tblInventory.Visible = true;
        }



        protected void rblWindings_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearEntries("Windings");

            string sWindings = rblWindings.SelectedValue;

            bool bCustom = IsCustom();

            bool bPhaseSingle = (rblPhase.SelectedValue == "Single") ? true : false;
            bool bWindingsCopper = (rblWindings.SelectedValue == "Copper") ? true : false;

            if (!bCustom)
                ChangePhase(bPhaseSingle, bWindingsCopper);

            RestoreEntries("Windings");

            // If any OSHPD kits had been selected when this was copper, they're not available now. 
            if (!bWindingsCopper)
                txtOPKitQty.Text = "";

            CatalogNumberUpdate(true);
            NextFocus("rblWindings");

            ShowInventoryTable();
        }

        protected void rblPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideCopper();       // Hide Copper if Stock, Single Phase.
            ClearEntries("Phase");

            bool bPhaseSingle = (rblPhase.SelectedValue == "Single") ? true : false;
            bool bWindingsCopper = (rblWindings.SelectedValue == "Copper") ? true : false;

            ChangePhase(bPhaseSingle, bWindingsCopper);

            RestoreEntries("Phase");
            ValidateSpecialTypes();

            CatalogNumberUpdate(true);
            NextFocus("rblPhase");

            ShowInventoryTable();
        }

        protected void rblPrimaryDW_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure Wye/Wye is selected if Auto Transformer is selected.
            if (!AutoTransformerSynch()) return;

            lblPrimaryShow.Text = "";

            string sDW = rblPrimaryDW.SelectedValue;
            bool bDelta = (sDW == "D") ? true : false;
            LoadPrimaryVoltage(bDelta, false, false);

            // Special processing for zig zag configuration.
            ZigZagSync("PrimaryDW");

            CatalogNumberUpdate(true);
            NextFocus("rblPrimaryDW");
        }

        protected void rblSecondaryDW_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Make sure Wye/Wye is selected if Auto Transformer is selected.
            if (!AutoTransformerSynch()) return;

            string sDW = rblSecondaryDW.SelectedValue;
            bool bDelta = (sDW == "D") ? true : false;
            DropDownList ddlSecondary = (DropDownList)upddlSecondaryVoltage.FindControl("SecondaryVoltage");
            ChangeSecondaryDW(sDW);

            // Special processing for zig zag configuration.
            ZigZagSync("SecondaryDW");

            // Remove Harmonic Mitigating / Zig Zag if Delta selected for secondary voltage.
            ValidateSpecialTypes();

            CatalogNumberUpdate(true);
            NextFocus("rblSecondaryDW");
        }

        protected void rblElectrostaticShield_SelectedIndexChanged(object sender, EventArgs e)
        {
            CatalogNumberUpdate(true);
            NextFocus("rblElectrostaticShield");
        }

        /// <summary>
        /// Called when changing one of these FieldName values: 1) Winding, 2) Phase, 3) KVA, 4) Primary, 5) StandardOrCustom
        /// </summary>
        protected void ClearEntries(string sFieldName)
        {
            lblGeneral.Text = "";
            lblKVAInvalid.Text = "";
            lblKVAUsed.Text = "";
            lblTempUsed.Text = "";

            chkHideKVA.Checked = false;
            chkHideVoltPrimary.Checked = false;
            chkHideVoltSecondary.Checked = false;

            // Backing up previous data.
            hidCatalogNo.Value = lblCatalogNo.Text;
            hidPhase.Value = rblPhase.SelectedValue;
            hidKVA.Value = txtKVA.Text;
            hidPrimaryVoltage.Value = String.IsNullOrEmpty(ddlPrimaryVoltage.SelectedValue) == true ? txtPrimaryVoltage.Text : ddlPrimaryVoltage.SelectedValue;
            hidPrimaryVoltageDW.Value = rblPrimaryDW.SelectedValue;
            hidSecondaryVoltage.Value = String.IsNullOrEmpty(ddlSecondaryVoltage.SelectedValue) == true ? txtSecondaryVoltage.Text : ddlSecondaryVoltage.SelectedValue;
            hidSecondaryVoltageDW.Value = rblSecondaryDW.SelectedValue;
            hidKFactor.Value = ddlKFactor.SelectedValue;
            hidTempRise.Value = ddlTempRise.SelectedValue;
            hidElectrostaticShield.Value = rblElectrostaticShield.SelectedValue;
            hidFreq.Value = ddlFrequency.SelectedValue;
            hidSoundLevel.Value = ddlSoundReduct.SelectedValue;
            hidEfficiency.Value = lblEfficiencyValue.Text;
            hidForExport.Value = chkForExport.Checked ? "True" : "False";
            hidCaseColor.Value = ddlCaseColor.SelectedValue;
            hidCaseColorOther.Value = txtCaseColorOther.Text;
            hidMarineDuty.Value = chkMarineDuty.Checked.ToString();

            string sMadeInUSAVAlues = rblMadeInUSA.SelectedValue;
            hidMadeInUSA.Value = sMadeInUSAVAlues;

            hidSpecialTypes.Value = rblSpecialTypes.SelectedValue;

            string sSpecialFeatures = CheckboxListValues(chkLstSpecialFeatures);
            hidSpecialFeatures.Value = sSpecialFeatures;

            hidSpecialFeatureNotes.Value = txtSpecialFeatureNotes.Text;
            hidCustomerTag.Value = txtCustomerTagNo.Text;
            hidQty.Value = txtQuantity.Text;
            hidWBKitQty.Value = txtWBKitQty.Text;
            hidKitQty.Value = txtKitQty.Text;
            hidRBKitQty.Value = txtRBKitQty.Text;
            hidOPKitQty.Value = txtOPKitQty.Text;

            hidExpediteDays.Value = ddlExpedite.SelectedValue;
            hidAdjustAmt.Value = txtShippingAmount.Text;
            hidAdjustReason.Value = txtShippingReason.Text;

            // Reset quantities.
            //txtQuantity.Text = "";
            //txtKitQty.Text = "";
            //txtRBKitQty.Text = "";
            //txtOPKitQty.Text = "";

            if (sFieldName == "All")
            {
                rblStandardOrCustom.SelectedIndex = -1;
            }

            // Clear lower display fields.
            lblKitName.Text = "";
            lblKitID.Text = "";
            //lblKitIDOrig.Text = "";     // Keep the previous KitID to see if it changes to a non-outdoor kit.
            lblKitPrice.Text = "";
            lblKitExtPrice.Text = "";

            lblExpeditePrice.Text = "";
            lblExpediteExtPrice.Text = "";

            if (sFieldName != "StandardOrCustom")
            {
                // Reset key values which are past these each time.
                SetKFactor("K-1 (STD)");
                if (FindSpecialFeature("K-Factor 20 (K-20)") == true)
                {
                    SetKFactor("K-20");
                }

                if (ddlTempRise.Items.Count > 0)
                    ddlTempRise.SelectedIndex = 0;
                rblElectrostaticShield.SelectedIndex = 0;

                if (sFieldName == "Winding")
                {
                    rblPhase.SelectedValue = "Three";
                }
                if (sFieldName == "Winding" || sFieldName == "Phase")
                {
                    txtKVA.Text = "";
                    if (ddlKVA.Items.Count > 0)
                        ddlKVA.SelectedIndex = 0;
                }
                // NO NEED TO HIDE VOLTAGES WHEN CHANGING KVA OR WINDING.  5/2/18
                if (sFieldName == "Phase")
                {
                    txtPrimaryVoltage.Text = "";
                    if (ddlPrimaryVoltage.Items.Count > 0)
                        ddlPrimaryVoltage.SelectedIndex = 0;
                    rblPrimaryDW.SelectedValue = "D";
                }

                if (sFieldName == "Winding" || sFieldName == "Phase" || sFieldName == "KVA" || sFieldName == "Primary")
                {
                    if (ddlSecondaryVoltage.Items.Count > 0)
                        ddlSecondaryVoltage.SelectedIndex = 0;
                    rblSecondaryDW.SelectedValue = "W";
                    txtSecondaryVoltage.Text = "";
                }
            }

            // This section reverses all of the More Options panel.
            // =========================================================

            SetFrequency("60 Hz (STD)");
            if (ddlSoundReduct.Items.Count > 0)
                ddlSoundReduct.SelectedIndex = 0;

            SetEnclosure("HRPO NEMA 3R Indoor/Outdoor");
            SetCaseColor("ANSI 61 (STD)");
            txtCaseColorOther.Text = "";

            rblMadeInUSA.SelectedValue = "None";
            chkMarineDuty.Checked = false;
            if (rblSpecialTypes.Items.Count > 0)
                rblSpecialTypes.SelectedIndex = 0;      // None.
            if (chkLstSpecialFeatures.Items.Count > 0)
            {
                chkLstSpecialFeatures.Items[0].Selected = true;     // None.
                for (int i = 1; i < chkLstSpecialFeatures.Items.Count; i++)
                {
                    chkLstSpecialFeatures.Items[i].Selected = false;
                }
            }
            txtSpecialFeatureNotes.Text = "";
            txtCustomerTagNo.Text = "";
            // =========================================================
        }

        /// <summary>
        /// Called after changing one of the primary features, such as Windings, Phase, KVA, or Primary Voltage.
        /// ClearEntries() was called first, and this restores as much as possible.
        /// </summary>
        protected void RestoreEntries(string sFieldName)
        {
            // Clear custom prices if anything pertaining to price has changed.
            ClearPrices();

            if (sFieldName != "Phase")
            {
                dv.RadioButtonListAdd(rblPhase, hidPhase.Value);
            }
            if (sFieldName != "KVA")
            {
                // Make sure the KVA value is in the dropdown.  If so, add it back as text.
                string sKVA = hidKVA.Value;

                if (dv.DropdownListAdd(ddlKVA, sKVA) == true)
                {
                    ddlKVA.SelectedIndex = -1;
                    txtKVA.Text = hidKVA.Value;
                }
                else
                {
                    // Values for three phase are "  12".
                    sKVA = " " + sKVA;
                    if (dv.DropdownListAdd(ddlKVA, sKVA) == true)
                    {
                        ddlKVA.SelectedIndex = -1;
                        txtKVA.Text = hidKVA.Value;
                    }
                    else
                    {
                        sKVA = " " + sKVA;
                        if (dv.DropdownListAdd(ddlKVA, sKVA) == true)
                        {
                            ddlKVA.SelectedIndex = -1;
                            txtKVA.Text = hidKVA.Value;
                        }
                    }
                }
            }

            // DON'T NEET TO HIDE VOLTAGES UNLESS SWITCHING FROM THREE PHASE TO SINGLE PHASE.  5/2/18
            //if (sFieldName != "Primary" && sFieldName != "Phase")
            //{
            //    dv.DropdownListAdd(ddlPrimaryVoltage, hidPrimaryVoltage.Value);
            //    if (rblPrimaryDW.Visible == true)
            //    {
            //        rblPrimaryDW.SelectedValue = hidPrimaryVoltageDW.Value.ToString();
            //    }

            //    if (txtPrimaryVoltage.Visible == true)
            //    {
            //        txtPrimaryVoltage.Text = hidPrimaryVoltage.Value.ToString();    // May be empty, if no longer available.
            //        if (ddlPrimaryVoltage.Items.Count > 0)
            //            ddlPrimaryVoltage.SelectedIndex = 0;                            // Remove selection from dropdown.
            //    }
            //}
            //if (sFieldName != "Secondary" && sFieldName != "Phase")
            //{
            //    dv.DropdownListAdd(ddlSecondaryVoltage, hidSecondaryVoltage.Value);
            //    if (rblSecondaryDW.Visible == true)
            //    {
            //        rblSecondaryDW.SelectedValue = hidSecondaryVoltageDW.Value.ToString();
            //    }

            //    if (txtSecondaryVoltage.Visible == true)
            //    {
            //        txtSecondaryVoltage.Text = ddlSecondaryVoltage.SelectedValue;     // May be empty, if no longer available.
            //        if (ddlSecondaryVoltage.Items.Count > 0)
            //            ddlSecondaryVoltage.SelectedIndex = 0;                            // Remove selection from dropdown.
            //    }
            //}

            // Make sure this value is available now.
            if (hidElectrostaticShield.Value == "None")
            {
                if (rblElectrostaticShield.Items[0].Enabled == true)
                    rblElectrostaticShield.SelectedValue = hidElectrostaticShield.Value;
            }
            else
            {
                rblElectrostaticShield.SelectedValue = hidElectrostaticShield.Value;
            }

            dv.DropdownListAdd(ddlKFactor, hidKFactor.Value);
            dv.DropdownListAdd(ddlTempRise, hidTempRise.Value);
            dv.DropdownListAdd(ddlFrequency, hidFreq.Value);
            dv.DropdownListAdd(ddlSoundReduct, hidSoundLevel.Value);
            lblEfficiencyValue.Text = hidEfficiency.Value;
            chkForExport.Checked = hidForExport.Value == "True" ? true : false;
            dv.DropdownListAdd(ddlCaseColor, hidCaseColor.Value);
            txtCaseColorOther.Text = hidCaseColorOther.Value;
            chkMarineDuty.Checked = (hidMarineDuty.Value == "True") ? true : false;

            string sMadeInUSA = hidMadeInUSA.Value;
            rblMadeInUSA.SelectedValue = sMadeInUSA;

            dv.RadioButtonListAdd(rblSpecialTypes, hidSpecialTypes.Value);

            string SpecialFeatures = hidSpecialFeatures.Value;
            CheckboxListLoad(chkLstSpecialFeatures, SpecialFeatures);

            txtSpecialFeatureNotes.Text = hidSpecialFeatureNotes.Value;

            txtSpecialFeatureNotes.Visible = (chkLstSpecialFeatures.Items[0].Selected == false) ? true : false;
            lblSpecialFeatureNotes.Visible = txtSpecialFeatureNotes.Visible;
            lblSpecialFeatureNotesReqd.Visible = txtSpecialFeatureNotes.Visible && String.IsNullOrEmpty(txtSpecialFeatureNotes.Text) == true;

            txtCustomerTagNo.Text = hidCustomerTag.Value;
            txtQuantity.Text = hidQty.Value;
            txtWBKitQty.Text = hidWBKitQty.Value;
            txtKitQty.Text = hidKitQty.Value;
            txtRBKitQty.Text = hidRBKitQty.Value;
            txtOPKitQty.Text = hidOPKitQty.Value;
            dv.DropdownListAdd(ddlExpedite, hidExpediteDays.Value);

            lblExpediteExtPrice.Text = String.IsNullOrEmpty(txtExpeditePrice.Text.ToString()) == true ? lblExpeditePrice.Text : txtExpeditePrice.Text;

            txtShippingAmount.Text = hidAdjustAmt.Value;
            txtShippingReason.Text = hidAdjustReason.Value;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool bSaved = SaveItem(false);

            // Hide notes after a save, if they're open.
            ViewShowNotesLink(false, false);

            if (!bSaved)
            {
                lblNotSaved.Visible = true;
            }
            else
            {
                if (Session["Internal"].ToString() == "1")
                    btnQuoteDetailsRpt.Visible = true;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            // Will be reset to False in Page_Unload();
            bButtonPress = true;
            ResetDetail(0, false);
            Display(enumDisplay.All);
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            ValidateEmail();
        }

        protected void ValidateEmail()
        {
            string s = txtEmail.Text;
            string sMsg = dv.EmailValid(s);
            lblEmailInvalid.Text = sMsg;

            if (String.IsNullOrEmpty(sMsg) == true)
            {
                rblStandardOrCustom.Focus();
                lblEmailInvalid.Visible = false;
            }
            else
                lblEmailInvalid.Visible = true;
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sCustomerContactID = ddlCustomer.Text.ToString();
            int iCustomerContactID = Convert.ToInt32(sCustomerContactID);

            Customer c = new Customer();
            DataTable dt = c.ByCustomerID(iCustomerContactID);

            txtCustomerContactID.Text = dt.Rows[0]["CustomerContactID"].ToString();
            txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
            txtCompany.Text = dt.Rows[0]["Company"].ToString();
            txtCity.Text = dt.Rows[0]["City"].ToString();

            txtEmail.Text = dt.Rows[0]["Email"].ToString();
            ValidateEmail();

            txtContactName.Text = dt.Rows[0]["ContactName"].ToString();
            int iRepDistributorID = Convert.ToInt32(dt.Rows[0]["RepDistributorID"]);
            SwapDistributors(iRepDistributorID);

            Display(enumDisplay.All);
        }

        protected void gvQuoteItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int iQuoteDetailsID = 0;
            int iDetailID = 0;
            int iQuoteID = Convert.ToInt32(lblQuoteID.Text);

            ResetDeleteWarning();

            if (e.CommandName == "Delete")
            {
                // iQuoteDetailsID.
                hidDetailID.Value = e.CommandArgument.ToString();
                int.TryParse(hidDetailID.Value, out iQuoteDetailsID);
                iDetailID = q.GetDetailID(iQuoteDetailsID);
                lblDeleteItem.Text = "Delete Item #" + iDetailID.ToString() + "?";
                lblDeleteItem.Visible = true;
                btnDeleteItemNo.Visible = true;
                btnDeleteItemYes.Visible = true;
            }
            // NOTE:  Having Select as anything other than a Command button did not seem to work.
            if (e.CommandName == "Select")
            {
                iQuoteDetailsID = Convert.ToInt32(e.CommandArgument);

                iDetailID = q.GetDetailID(iQuoteDetailsID);

                // Can't rely on memory variable at this point.
                LoadQuoteDetails(iQuoteID, iDetailID);
               
                
                //added 7-17-19
                ShowInventoryTable();
                UpdateInventory();
            
            }


        }
        /// <summary>
        /// Called when changing Phase, either by the field itself, or by loading data to edit.
        /// </summary>
        protected void ChangePhase(bool bPhaseSingle, bool bWindingsCopper)
        {
            // KVA should be reloaded when switching to Custom, so don't do it here.
            bool bCustom = IsCustom();
            bool bZigZag = (rblSpecialTypes.SelectedValue == "Zig Zag" || rblSpecialTypes.SelectedValue == "Harmonic Mitigating") ? true : false;

            LoadKVA(bCustom, bPhaseSingle, bWindingsCopper, bZigZag);

            string sPhase = bPhaseSingle == true ? "Single" : "Three";

            bool bExempt = IsExempt();
            LoadKFactor(sPhase, "75", bExempt, false);
            LoadTempRise(sPhase, "75", "K-1 (STD)", false);

            // Delta / Wye only applicable for Three Phase.

            // Defaults to Primary = Delta, Secondary = Wye.
            rblPrimaryDW.Text = "D";
            rblSecondaryDW.Text = "W";

            // Swaps out recordsource between Three Phase and Single Phase.
            if (ddlPrimaryVoltage.Items.Count > 0)
                ddlPrimaryVoltage.SelectedIndex = 0;
            if (ddlSecondaryVoltage.Items.Count > 0)
                ddlSecondaryVoltage.SelectedIndex = 0;

            LoadPrimaryVoltage(true, false, bPhaseSingle);
            LoadSecondaryVoltage(true, false, bPhaseSingle);

            if (FindSpecialFeature("K-Factor 20 (K-20)") == true)
            {
                SetKFactor("K-20");
            }

            decimal dKVA = 1;
            LoadConfiguration(dKVA, bPhaseSingle, bWindingsCopper);

        }

        /// <summary>
        /// Called when changing KVA.
        /// </summary>
        protected void ChangeKVA(decimal dKVA, bool bCustom, bool bPhaseSingle, bool bWindingsCopper, string sKFactor)
        {
            if (!bCustom)
            {
                LoadConfiguration(dKVA, bPhaseSingle, bWindingsCopper);
            }
            string sPhase = bPhaseSingle == true ? "Single" : "Three";
            string sKVA = dKVA.ToString();
            // Reset KFactor and TempRise if their values may not be in the list.

            // DON'T NEED TO RESET KFACTOR AND TEMP RISE WHEN CHANGING KVA. 5/2/18
            //if (!bPhaseSingle && dKVA >= 600 || bPhaseSingle && dKVA >= 500
            //    || sKFactor == "K-20" && dKVA > 500)
            //{
            //    if (ddlKFactor.Items.Count > 0)
            //        ddlKFactor.SelectedIndex = 0;
            //    hidKFactor.Value = "K-1 (STD)";     // These values get restored.
            //    sKFactor = "K-1 (STD)";

            //    // Reset chkLstSpecialFeatures to 'None'.
            //    hidSpecialFeatures.Value = "";
            //    LoadSpecialFeatures();

            //    if (ddlTempRise.Items.Count > 0)
            //        ddlTempRise.SelectedIndex = 0;
            //    hidTempRise.Value = "150 (STD)";
            //}

            bool bExempt = IsExempt();

            sKFactor = "K-1 (STD)";
            if (ddlKFactor.Items.Count > 0)
            {
                sKFactor = ddlKFactor.SelectedValue;
            }

            t.Phase = sPhase;
            t.KVA = dKVA;
            t.KFactor = sKFactor;
            t.CaseSize = lblCaseSize.Text;

            int iIndoorAllowed = 0;
            iIndoorAllowed = Convert.ToInt32(t.IndoorAllowed);
            int iTENVPossible = 0;
            iTENVPossible = Convert.ToInt32(t.TENVAvailable());

            // Remove TENV if it is selected when changing KVA to a size higher than permitted by TENV.
            string sEnclosure = "";
            if (ddlEnclosure.Items.Count > 0)
            {
                sEnclosure = ddlEnclosure.SelectedValue;
            }
            LoadEnclosure(iIndoorAllowed, iTENVPossible);
            // If switching from TENV to non-TENV, will default to the first item in the list.
            SetEnclosure(sEnclosure);

            bool bTenv = t.TotallyEnclosedNonVentilated;

            LoadKFactor(sPhase, sKVA, bExempt, bTenv);


            LoadTempRise(sPhase, sKVA, sKFactor, bTenv);
        }
        /// <summary>
        /// Called when changing KFactor, custom only.
        /// NOTE:  BELIEVE THIS IS OBSOLETE.
        /// </summary>
        /// <param name="sKFactor"></param>
        protected void ChangeKFactor(string sKFactor)
        {
            if (sKFactor == "K-1" || sKFactor == "None (STD)" || sKFactor == "K-1 (STD)")
            {
                rblElectrostaticShield.SelectedValue = "None";
                rblElectrostaticShield.Items[0].Enabled = true;
            }
            else
            {
                rblElectrostaticShield.SelectedValue = "Shielded";
                rblElectrostaticShield.Items[0].Enabled = false;
            }

            string sPhase = rblPhase.SelectedValue.ToString();
            string sKVA = dv.TextOrDropDown(txtKVA.Text.ToString(), ddlKVA.SelectedValue.ToString());

            decimal dKVA = 0;
            decimal.TryParse(sKVA, out dKVA);

            string sEnclosure = ddlEnclosure.SelectedValue;
            t.Enclosure = sEnclosure;
            bool bTenv = t.TotallyEnclosedNonVentilated;

            LoadTempRise(sPhase, sKVA, sKFactor, bTenv);
        }

        protected void ChangePrimaryDW(string sDW)
        {
            bool bDelta = (sDW == "D") ? true : false;
            LoadPrimaryVoltage(bDelta, false, false);
        }

        protected void ChangeSecondaryDW(string sDW)
        {
            bool bWye = (sDW == "W") ? true : false;
            LoadSecondaryVoltage(bWye, false, false);
        }

        protected void ddlKVA_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sKFactor = ddlKFactor.SelectedValue;

            lblKVAInvalid.Text = "";
            lblKVAInvalid.Visible = false;

            // ClearEntries("KVA");
            lblGeneral.Text = "";


            RadioButtonList rblPhase = (RadioButtonList)uprblPhase.FindControl("rblPhase");
            bool bPhaseSingle = (rblPhase.SelectedValue == "Single") ? true : false;
            DropDownList ddlKVA = (DropDownList)upddlKVA.FindControl("ddlKVA");
            decimal dKVA = 0;
            if (ddlKVA.SelectedValue != "")
                dKVA = Convert.ToDecimal(ddlKVA.SelectedValue);

            string sPhase = bPhaseSingle == true ? "Single" : "Three";

            // Move the value to the textbox.
            txtKVA.Text = dv.KVAFormat(dKVA.ToString());
            if (rblStandardOrCustom.SelectedValue == "Custom")
            {
                if (ddlKVA.Items.Count > 0)
                    ddlKVA.SelectedIndex = 0;
            }

            bool bWindingsCopper = (rblWindings.SelectedValue == "Copper") ? true : false;

            bool bCustom = IsCustom();

            ChangeKVA(dKVA, bCustom, bPhaseSingle, bWindingsCopper, sKFactor);
            // The entry for K-Factor and Temp Rise might change in ChangeKVA() 
            // before being restored with RestoreEntries().

            // NOT USING THIS. 5/2/18.
            // RestoreEntries("KVA");
            CatalogNumberUpdate(true);
            NextFocus("ddlKVA");

            ShowInventoryTable();
            UpdateInventory();
        }

      



        protected void ddlKFactor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DropDownList ddlKFactor = (DropDownList)upddlKFactor.FindControl("ddlKFactor");
            string sKFactor = ddlKFactor.SelectedValue.ToString();
            bool bK20 = false;

            ChangeKFactor(sKFactor);

            if (sKFactor == "K-20")
            {
                bK20 = true;
            }
            ResetElectrostaticShield();

            for (int i = 1; i < chkLstSpecialFeatures.Items.Count; i++)
            {
                if (chkLstSpecialFeatures.Items[i].Value == "K-Factor 20 (K-20)")
                {
                    chkLstSpecialFeatures.Items[i].Selected = bK20;
                    chkLstSpecialFeatures.Items[0].Selected = false;    // Uncheck "None".
                }
            }

            CatalogNumberUpdate(true);

            NextFocus("ddlKFactor");
        }

        /// <summary>
        /// Called when updating other values, to keep CatalogNumber up to date.
        /// bPriceReset = true when any configuration changed.
        ///                    This removes any special prices that might have been entered.
        /// </summary>
        protected void CatalogNumberUpdate(bool bPriceReset)
        {
            ResetDeleteWarning();

            int iRepDistributorID = 0;
            iRepDistributorID = Convert.ToInt32(lblRepDistributorID.Text);

            int iRepID = Convert.ToInt32(Session["RepID"]);

            lblCatalogNo.Text = "";
            lblCatalogNoExt.Text = "";
            //lblStockID.Text = "";  DeBard 8/1/18
            lblCustomID.Text = "";
            lblCaseSize.Text = "";
            lblCaseSizeCalc.Text = "";
            if (ddlCaseSizes.Items.Count > 0)
                ddlCaseSizes.SelectedIndex = -1;        // No selection.

            hidIsMatch.Value = "0";

            // Disabling this dropdown until we have a KVA.
            //ddlSoundReduct.Enabled = false;

            string sKVA = txtKVA.Text;
            if (sKVA == "") sKVA = ddlKVA.SelectedValue.ToString();

            if (sKVA == "")
            {
                Display(enumDisplay.All);
                return;
            }

            // Remove extraneous decimals.
            sKVA = dv.KVAFormat(sKVA);

            int iQuantity = 0;
            bool isNum = int.TryParse(txtQuantity.Text.ToString(), out iQuantity);
            iQuantity = (iQuantity == 0) ? 1 : iQuantity;

            int iQuoteDetailID = (txtQuoteDetailsID.Text == "" | txtQuoteDetailsID.Text == null) ? 0 : Convert.ToInt32(txtQuoteDetailsID.Text);
            string sStandardOrCustom = rblStandardOrCustom.SelectedValue;
            string sWindings = rblWindings.SelectedValue;
            string sPhase = rblPhase.SelectedValue;
            t.Phase = sPhase;
            string sKitNumber = txtKitNumber.Text;
            string sKitQuantity = txtKitQty.Text;
            string sConfiguration = "";
            string sTemp = "";
            string sKFactor = "";
            string sPrimVoltDW = "";
            string sPrimVolt = "";
            string sSecVoltDW = "";
            string sSecVolt = "";
            string sElecShield = "";
            string sCustomerTag = String.IsNullOrEmpty(txtCustomerTagNo.Text) == true ? "" : txtCustomerTagNo.Text.ToString();

            string sCaseColor = ddlCaseColor.SelectedValue;
            int iExpediteNoDays = 0;
            bool bNum = int.TryParse(ddlExpedite.SelectedValue.ToString(), out iExpediteNoDays);
            string sFrequency = ddlFrequency.SelectedValue;
            string sEfficiency = lblEfficiencyValue.Text;
            int iEfficiencySetByAdmin = 0;
            if (ddlEfficiency.Visible == true)
            {
                sEfficiency = ddlEfficiency.SelectedValue;

                int.TryParse(lblEfficiencyIsSetByAdmin.Text, out iEfficiencySetByAdmin);
            }

            // Turn off K-20 if DOE2016.  Only allow it if it is EXEMPT.
            //if (sEfficiency == "DOE2016" && FindSpecialFeature("K-Factor 20 (K-20)") == true)
            //{
            //    //Turn off special feature, and change K-Factor to default K-1 (STD).
            //    chkLstSpecialFeatures.Items[4].Selected = false;
            //    ddlKFactor.SelectedIndex = 0;
            //}

            int iForExport = chkForExport.Checked == true ? 1 : 0;
            string sMadeInUSA = rblMadeInUSA.SelectedValue;
            bool bMarineDuty = chkMarineDuty.Checked;
            string sSoundReduct = ddlSoundReduct.SelectedValue;
            string sSpecialFeature = CheckboxListValues(chkLstSpecialFeatures);
            string sSpecialType = rblSpecialTypes.SelectedValue;
            string sEnclosure = ddlEnclosure.SelectedValue;
            t.Enclosure = sEnclosure;
            if (t.Stainless == true)
                t.EnclosureMaterial = rblStainless.SelectedValue;
            int iTENV = Convert.ToInt32(t.TotallyEnclosedNonVentilated);

            int iStainlessSteel = t.Stainless == true ? 1 : 0;

            if (sStandardOrCustom == "Standard")
            {
                sConfiguration = ddlConfiguration.SelectedValue;

                sEfficiency = "TP-1";

                if (sConfiguration.Length > 3)
                {
                    if (sConfiguration.Substring(0, 3) == "D16")
                        sEfficiency = "DOE2016";

                    // Trim off the "D16: " or "TP1: " prefix.
                    sConfiguration = sConfiguration.Substring(5, sConfiguration.Length - 5);
                }

                if (sConfiguration == "")
                {
                    txtQuantity.Text = "";
                    txtKitQty.Text = "";
                    txtRBKitQty.Text = "";
                    txtOPKitQty.Text = "";

                    Display(enumDisplay.All);
                    return;
                }
            }
            else    // Custom.
            {
                sPrimVoltDW = rblPrimaryDW.SelectedValue;

                bool bInternal = Convert.ToBoolean(Session["Internal"]);

                // If internal, pulls voltage from textboxes, else dropdowns.
                if (bInternal == true)
                {
                    sPrimVolt = txtPrimaryVoltage.Text;
                    sSecVolt = txtSecondaryVoltage.Text;
                }
                else
                {
                    sPrimVolt = ddlPrimaryVoltage.SelectedValue;
                    sSecVolt = ddlSecondaryVoltage.SelectedValue;
                }

                // Choose hidden values if the screen values have been temporarily removed.
                //sPrimVolt = String.IsNullOrEmpty(sPrimVolt) == true ? hidPrimaryVoltage.Value : sPrimVolt;
                //sSecVolt = String.IsNullOrEmpty(sSecVolt) == true ? hidSecondaryVoltage.Value : sSecVolt;

                sSecVoltDW = rblSecondaryDW.SelectedItem.Value;

                sKFactor = "";
                if (ddlKFactor.Items.Count > 0)
                {
                    sKFactor = ddlKFactor.SelectedItem.Value;
                }

                // 8-15-18 Added to force K-20 to require approval for KVA <= 150.
                if (sKFactor == "K-20")
                {
                    sSpecialFeature = string.IsNullOrEmpty(sSpecialFeature) == true ? "" : sSpecialFeature;
                    if (sSpecialFeature != "" && sSpecialFeature[sSpecialFeature.Length - 1].ToString() != ";")
                    {
                        sSpecialFeature = sSpecialFeature + ";";
                    }
                    sSpecialFeature = sSpecialFeature + "K-20;";
                }

                sTemp = "";
                if (ddlTempRise.Items.Count > 0)
                {
                    sTemp = ddlTempRise.SelectedItem.Value;
                }
                sElecShield = rblElectrostaticShield.SelectedItem.Value;

                if (sPrimVolt == "" || sSecVolt == "" || sKFactor == "" || sTemp == "" || lblKVAInvalid.Text != "")
                {
                    Display(enumDisplay.All);
                    pnlMoreOptions.Visible = false;
                    return;
                }
            }

            // Price entered comes from txtUnitPriceChanged if there is anything entered there, else txtUnitPrice.
            string sPriceEntered = txtUnitPriceChanged.Text.ToString().Trim();
            sPriceEntered = string.IsNullOrEmpty(sPriceEntered) == true ? txtUnitPrice.Text.ToString().Trim() : sPriceEntered;
            decimal decPriceEntered = 0;
            decimal.TryParse(sPriceEntered, out decPriceEntered);

            int iKitQty = (txtKitQty.Text == "" || txtKitQty.Text == null) ? 0 : Convert.ToInt32(txtKitQty.Text);

            // Kit is set to No Charge if it is TENV and normally Indoor (>= 150 KVA).
            string sKitPrice = txtKitPrice.Text.ToString().Trim();
            decimal decKitPrice = 0;
            decimal.TryParse(sKitPrice, out decKitPrice);

            // OSHPD kits require approval.
            int iOSHPD = 0;
            string sOSHPD = txtOPKitQty.Text;
            int.TryParse(sOSHPD, out iOSHPD);

            // Lug kits stock only, McGee only.
            int iLug = 0;
            string sLug = txtLugKitQty.Text;
            int.TryParse(sLug, out iLug);

            // See if Internal prices apply.
            int iInternal = InternalPrices();

            string sUserName = hidUserNameCreated.Value.ToString();
            sUserName = String.IsNullOrEmpty(sUserName) == true ? Session["UserName"].ToString() : sUserName;

            // Remove Expedite, so prices will apply again.
            // if (ddlExpedite.Items.Count > 0)
            //     ddlExpedite.SelectedIndex = 0;      // None.

            // Reset all prices if the Catalog number is changing.
            if (bPriceReset == true)
                ClearPrices();

            int iQuoteID = string.IsNullOrEmpty(lblQuoteID.Text.ToString()) == true ? 0 : Convert.ToInt32(lblQuoteID.Text);

            bool bKvaHide = chkHideKVA.Checked;
            bool bPrimaryVoltHide = chkHideVoltPrimary.Checked;
            bool bSecondaryVoltHide = chkHideVoltSecondary.Checked;

            string sEnclosureMtl = "";
            sEnclosureMtl = t.EnclosureMaterial;
            string sNEMA = t.NEMA;

            DataTable dt = q.CatalogNo(sCaseColor, sConfiguration, sCustomerTag, sEfficiency, iEfficiencySetByAdmin, sElecShield,
                                iExpediteNoDays, iForExport, iLug, iOSHPD, bPriceReset, sFrequency, sKFactor,
                                iKitQty, sKVA, bKvaHide, sMadeInUSA, bMarineDuty, sNEMA, sPhase, decPriceEntered,
                                sPrimVolt, sPrimVoltDW, bPrimaryVoltHide, iQuantity, iQuoteID, iQuoteDetailID, iRepDistributorID,
                                sSecVolt, sSecVoltDW, bSecondaryVoltHide, sSoundReduct,
                                sSpecialFeature, sSpecialType, sEnclosureMtl,
                                sStandardOrCustom, sTemp,
                                iTENV, sUserName, sWindings);

            if (dt == null || dt.Rows.Count == 0)
            {
                lblNotSaved.Visible = true;
                return;
            }
            lblNotSaved.Visible = false;

            DataRow dr = dt.Rows[0];
            if (dr["InternalMsg"].ToString() == "Case size exceeded.")
            {
                lblCaseSizeExceeded.Visible = true;
            }
            else
            {
                lblCaseSizeExceeded.Visible = false;
            }

            lblStockID.Text = dr["StockID"].ToString();         // Hidden. 
            lblCustomID.Text = dr["CustomID"].ToString();       // Hidden. 

            string sNotesInternal = dr["InternalMsg"].ToString();

            // Add the internal notes, provided it doesn't already exist in it.
            hidDetailNotesInternal.Value = sNotesInternal;

            hidIsTapsNone.Value = dr["IsTapsNone"].ToString();
            hidIsMatch.Value = dr["IsMatch"].ToString();
            hidKitHasRainHood.Value = dr["KitHasRainHood"].ToString();
            hidIsStepUp.Value = dr["IsStepUp"].ToString();
            hidShipWeight.Value = dr["ShipWeight"].ToString();

            int iKva = 0;
            string sKva = txtKVA.Text;
            iKva = Utility.IntFromString(sKva);
            LoadSoundReduct(iKva);     // 4/2/19 Changed to get accurate acheivable levels.
            ddlSoundReduct.Enabled = true;

            // Same as Stock.  Remove any expedite settings.
            if (hidIsMatch.Value == "1")
            {
                if (ddlExpedite.Items.Count > 0)
                    ddlExpedite.SelectedIndex = 0;
                lblExpeditePrice.Text = "";
                lblExpeditePriceCalc.Text = "";
                txtExpeditePrice.Text = "";
                lblExpediteExtPrice.Text = "";
            }

            bool bApprovalReqd = Convert.ToBoolean(dr["ApprovalReqd"]);
            bool bApprovalReqdQuote = Convert.ToBoolean(dr["ApprovalReqdQuote"]);

            // Now returns approval required information from this procedure.

            // Approval Required is at the Quote level.  If it doesn't already exist, turn it on.
            // Otherwise, turn it off.
            if (bApprovalReqdQuote == true)
            {
                // This text may have more informative information in it than Approval Required,
                // so if it already has contents, don't replace it.
                if (String.IsNullOrEmpty(lblApprovalReqd.Text.ToString()) == true)
                    lblApprovalReqd.Text = "APPROVAL REQUIRED.";
            }
            else
            {
                lblApprovalReqd.Text = "";
            }

            // Hidden field, contains just one item's approval reason.
            string sApprovalReasonItem = dr["ApprovalReason"].ToString();
            lblItemApprovalReason.Text = sApprovalReasonItem;

            string sApprovalReasonQuote = dr["ApprovalReasonQuote"].ToString();
            ApprovalDisplay(sApprovalReasonQuote);

            string sEfficiencyCode = "";
            sEfficiencyCode = dr["EfficiencyCode"].ToString();
            lblEfficiencyValue.Text = sEfficiencyCode;

            bool bEfficiencySetByAdmin = false;
            // Default state for this is "".
            bEfficiencySetByAdmin = dr["EfficiencyIsSetByAdmin"].ToString() == "True" ? true : false;
            lblEfficiencyIsSetByAdmin.Text = bEfficiencySetByAdmin == true ? "1" : "0";

            string sEfficiencyCodeCalc = "";
            sEfficiencyCodeCalc = dr["EfficiencyCodeCalc"].ToString();
            lblEfficiencyCodeCalc.Text = sEfficiencyCodeCalc;

            if (ddlEfficiency.Visible == true)
            {
                if (bEfficiencySetByAdmin && sEfficiencyCode != "")
                {
                    SetEfficiency(sEfficiencyCode);
                }
                else
                {
                    SetEfficiency(sEfficiencyCodeCalc);
                }
            }
            hidEfficiency.Value = ddlEfficiency.SelectedValue;

            hidKVAUsed.Value = dr["KVAUsed"].ToString();

            // Special features might remove K-20 if not DOE 2016 EXEMPT.
            string sSpecialFeatures = "";
            sSpecialFeatures = dr["SpecialFeatures"].ToString();

            decimal dKVA = 0;
            decimal.TryParse(sKVA, out dKVA);
            t.KVA = dKVA;
            // Enable or disable K-Factor K-20 based on phase and KVA entered.
            bool bExempt = IsExempt();

            LoadSpecialFeatures();

            CheckboxListLoad(chkLstSpecialFeatures, sSpecialFeatures);

            if (rblStandardOrCustom.SelectedValue == "Custom")
            {
                sKFactor = dr["KFactor"].ToString();
                t.KFactor = sKFactor;
                SetKFactor(sKFactor);      // Make sure this KFactor is still available.  If not, defaults to K-1 (STD).

                if (FindSpecialFeature("K-Factor 20 (K-20)") == true)
                {
                    SetKFactor("K-20");
                }
            }

            string sShipDays = dr["ShipDays"].ToString();
            int iShipDays = 15;
            int.TryParse(sShipDays, out iShipDays);
            lblShipDays.Text = iShipDays.ToString();

            string sUnitPrice = dr["UnitPrice"].ToString();
            string sUnitPriceCalc = dr["UnitPriceCalc"].ToString();
            string sUnitPriceList = dr["UnitPriceList"].ToString();
            decimal decUnitPrice = 0;
            decimal.TryParse(sUnitPrice, out decUnitPrice);
            decimal decUnitPriceCalc = 0;
            decimal.TryParse(sUnitPriceCalc, out decUnitPriceCalc);
            decimal decUnitPriceList = 0;
            decimal.TryParse(sUnitPriceList, out decUnitPriceList);

            if (decUnitPrice == 0)
            {
                decUnitPrice = decUnitPriceCalc;
                sUnitPrice = sUnitPriceCalc;
            }
            if (decUnitPriceCalc != 0)
            {
                // Reset price if calculated price has changed.  11/12/18 DeBard added || lblUnitPrice.Text == "",
                // since in some cases lblUnitPrice.Text was empty.
                if (lblUnitPriceCalc.Text != dv.NumberFormat(sUnitPriceCalc, 9, 2) || lblUnitPrice.Text == "")
                {
                    lblUnitPriceCalc.Text = dv.NumberFormat(sUnitPriceCalc, 9, 2);
                    txtUnitPrice.Text = dv.NumberFormat(sUnitPriceCalc, 9, 2);
                    lblUnitPrice.Text = txtUnitPrice.Text;
                    txtUnitPriceChanged.Text = txtUnitPrice.Text;
                }
            }
            if (decUnitPriceList > 0)
            {
                // Reset price if calculated price has changed.
                lblUnitPriceList.Text = dv.NumberFormat(sUnitPriceList, 9, 2);
            }
            txtQuantity.Text = String.IsNullOrEmpty(txtQuantity.Text.ToString()) == true ? "  1" : txtQuantity.Text;

            lblKitID.Text = dr["KitID"].ToString();             // Hidden.
            lblLugKitID.Text = dr["LugKitID"].ToString();
            lblOPKitID.Text = dr["OPKitID"].ToString();
            lblRBKitID.Text = dr["RBKitID"].ToString();
            lblWBKitID.Text = dr["WBKitID"].ToString();

            lblKitName.Text = dr["KitName"].ToString();
            lblLugKitName.Text = dr["LugKitName"].ToString();
            lblRBKitName.Text = dr["RBKitName"].ToString();
            lblOPKitName.Text = dr["OPKitName"].ToString();
            lblWBKitName.Text = dr["WBKitName"].ToString();

            txtKitNumber.Text = dr["KitNumber"].ToString();
            txtLugKitNumber.Text = dr["LugKitNumber"].ToString();
            txtOPKitNumber.Text = dr["OPKitNumber"].ToString();
            txtRBKitNumber.Text = dr["RBKitNumber"].ToString();
            txtWBKitNumber.Text = dr["WBKitNumber"].ToString();

            // Kit Quantity.
            if (lblKitIDOrig.Text != "" && lblKitID.Text != "")
            {
                string sKitIDOrig = lblKitIDOrig.Text;
                int iKitIDOrig = 0;
                int.TryParse(sKitIDOrig, out iKitIDOrig);

                int iKitID = 0;
                string sKitID = lblKitID.Text;
                int.TryParse(sKitID, out iKitID);

                bool bWeatherOrig = false;
                bool bWeather = false;

                // If a change has been made such that we're still using a kit for weather shielding, keep it.
                // Otherwise, remove the kit.
                if (iKitIDOrig == 3 || iKitIDOrig == 4 || iKitIDOrig == 16)
                {
                    bWeatherOrig = true;
                }
                if (iKitID == 3 || iKitID == 4 || iKitID == 16)
                {
                    bWeather = true;
                }
                if (bWeatherOrig != bWeather)
                {
                    txtKitQty.Text = "  0";
                }
            }
            else
            {
                if (txtKitQty.Text == "  0" && lblKitQtyOrig.Text != "")
                {
                    txtKitQty.Text = lblKitQtyOrig.Text;
                }
            }

            iKitQty = 0;
            sKitQuantity = txtKitQty.Text.ToString();
            int.TryParse(sKitQuantity, out iKitQty);

            // Make sure the Kit Quantity is entered to get correct Indoor/Outdoor readings.
            t.KitQty = iKitQty;

            // Kit Price.
            sKitPrice = dr["KitPrice"].ToString();
            lblKitPrice.Text = sKitPrice;
            lblKitPriceCalc.Text = sKitPrice;
            txtKitPrice.Text = sKitPrice;
            DisplayPrice("Kit", "CatalogNumberUpdate");

            // RB Kit Quantity.
            if (lblRBKitName.Text != lblRBKitNameOrig.Text && lblRBKitNameOrig.Text != "")
            {
                if (lblRBKitQtyOrig.Text != "0")
                {
                    txtRBKitQty.Text = "  0";
                }
            }
            else
            {
                if (txtRBKitQty.Text == "  0" && lblRBKitQtyOrig.Text != "")
                {
                    txtRBKitQty.Text = lblRBKitQtyOrig.Text;
                }
            }

            // RB Kit Price.
            string sRBKitPrice = dr["RBKitPrice"].ToString();
            lblRBKitPrice.Text = sRBKitPrice;
            lblRBKitPriceCalc.Text = sRBKitPrice;
            txtRBKitPrice.Text = sRBKitPrice;
            DisplayPrice("RBKit", "CatalogNumberUpdate");

            // Lug Kit Price.
            string sLugKitPrice = dr["LugKitPrice"].ToString();
            lblLugKitPrice.Text = sLugKitPrice;
            lblLugKitPriceCalc.Text = sLugKitPrice;
            txtLugKitPrice.Text = sLugKitPrice;
            DisplayPrice("LugKit", "CatalogNumberUpdate");
            
            // OP Kit Quantity.
            // Applies across the board.

            // OP Kit Price.
            string sOPKitPrice = dr["OPKitPrice"].ToString();
            lblOPKitPrice.Text = sOPKitPrice;
            lblOPKitPriceCalc.Text = sOPKitPrice;
            txtOPKitPrice.Text = sOPKitPrice;
            DisplayPrice("OPKit", "CatalogNumberUpdate");

            string sOPKitQty = txtOPKitQty.Text.ToString();
            txtOPKitQty.Text = (String.IsNullOrEmpty(sOPKitQty)) ? "  0" : txtOPKitQty.Text;

            // WB Kit Price.
            string sWBKitPrice = dr["WBKitPrice"].ToString();
            lblWBKitPrice.Text = sWBKitPrice;
            lblWBKitPriceCalc.Text = sWBKitPrice;
            txtWBKitPrice.Text = sWBKitPrice;
            DisplayPrice("WBKit", "CatalogNumberUpdate");

            string sWBKitQty = txtWBKitQty.Text.ToString();
            txtWBKitQty.Text = (String.IsNullOrEmpty(sWBKitQty)) ? "  0" : txtWBKitQty.Text;

            // If you're changing a value, it overwrites any saved case size.
            lblCaseSizeCalc.Text = dr["CaseSize"].ToString();
            lblCaseSize.Text = lblCaseSizeCalc.Text;

            // Store Case Size in Transformer object.
            t.CaseSize = lblCaseSizeCalc.Text;

            // Find this case size in the list, if available.
            SetCaseSize(lblCaseSize.Text.ToString());
            t.CaseSize = lblCaseSize.Text.ToString();

            t.TotallyEnclosedNonVentilated = Convert.ToBoolean(dr["TENV"]);
            bool bTenv = t.TotallyEnclosedNonVentilated;
            LoadKFactor(sPhase, sKVA, bExempt, bTenv);

            t.IndoorOutdoor = dr["Enclosure"].ToString();
            lblEnclosureData.Text = t.IndoorOutdoor;

            // Update the enclosure dropdown with available options.
            int iTempRise = Convert.ToInt32(ddlTempRise.SelectedValue);
            t.TempRise = iTempRise;
            int iIndoorAllowed = 0;
            iIndoorAllowed = Convert.ToInt32(t.IndoorAllowed);
            int iTENVPossible = 0;
            iTENVPossible = Convert.ToInt32(t.TENVAvailable());

            LoadEnclosure(iIndoorAllowed, iTENVPossible);
            sEnclosure = t.Enclosure;
            SetEnclosure(sEnclosure);

            BuildCatalogNo(dr["CatalogNumber"].ToString());

            // Use the second line of the CatalogNumber to test what expedite options we offer.
            string sCatalogNo = lblCatalogNoExt.Text;

            string sValue = ddlExpedite.SelectedValue;

            int iShipDaysSelected = 0;
            int.TryParse(sValue, out iShipDaysSelected);

            // Change expedite number of days based on both rep and TENV.
            bool bExact = LeadTimesExact();
            LoadExpedite(iShipDays, iShipDaysSelected, bExact);

            // Show KVA and Temp used if Internal rep.
            lblKVAUsed.Text = dr["KVAUsed"].ToString();
            lblTempUsed.Text = dr["TempUsed"].ToString();

            EnableFinalizeButton();

            // Handle approvals.
            UpdatePrices("CatalogNumberUpdate");
            DisplayPrice("Unit", "CatalogNumberUpdate");

            Display(enumDisplay.All);
        }

        protected void ddlConfiguration_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearEntries("Primary");

            CatalogNumberUpdate(true);
            NextFocus("ddlConfiguration");

            ShowInventoryTable();
            UpdateInventory();

        }


        private void UpdateInventory()
        {
            if (ViewState["ShowInventory"] != null)
            {
                if (ViewState["ShowInventory"].ToString() == "true")
                {                   
                    GetInventory();
                }
            }
        }


        private void GetInventory()
        {
            string sCity = string.Empty;
            string sState = string.Empty;
            string sSQL = string.Empty;
            int iWarehousePref1;
            int iWarehousePref2;
            int iWarehousePref3;
            DataSet ds;
            decimal dKVA;


            bool bIsTP1 = ddlConfiguration.Text.Contains("TP1: ");

            sSQL = "select VoltageCategory,StockVoltageDisplay from StockVoltages where StockVoltage = '" + ddlConfiguration.Text.Replace("TP1: ", "").Replace("D16: ", "") + "'";
            ds = DataLink.Select(sSQL, DataLinkCon.mgmuser);

            string sVoltageCategory = Utility.GetStringValue(ds, "VoltageCategory");
            string sStockVoltageDisplay = Utility.GetStringValue(ds, "StockVoltageDisplay");

            if (sVoltageCategory == "")
                return;

            if (!decimal.TryParse(ddlKVA.Text, out dKVA))
            {
                ddlKVA.SelectedIndex = -1;
                return;
            }

            if (rblWindings.SelectedValue == "Copper")
                sVoltageCategory = sVoltageCategory.Trim() + "C";
            
            DataTable dtInven = InventoryReportLibrary.Common.GetInventoryData(Convert.ToDecimal(ddlKVA.Text),
                                         sVoltageCategory, (rblWindings.SelectedItem.Text == "Aluminum" ? "Al" : "Cu"), -1, -1);

            iWarehousePref1 = Convert.ToInt32(ViewState["WarehousePref1"] == null ? "-1" : ViewState["WarehousePref1"].ToString());


            if (dtInven != null)
            {

                IEnumerable<DataRow> rowsPref1 = dtInven.AsEnumerable().Select(r => r)
                           .Where(c => (Convert.ToInt32(c["Location"]) == iWarehousePref1) && (c["TP1"].ToString() == (bIsTP1 ? "TP1" : "")));

                if (rowsPref1.Count() > 0)
                {
                    var dataPref1 = rowsPref1.Select(z => new { City = z["CityAlias"].ToString(), State = z["State"].ToString(), Qty = Convert.ToDecimal(z["QtyOnHand"]) });

                    foreach (var pref1 in dataPref1)
                    {
                        lblWarehouse1.Text = pref1.City + "," + pref1.State;
                        lblWarehouse1Qty.Text = pref1.Qty.ToString("#") == "" ? "0" : pref1.Qty.ToString("#");
                    }
                }
            }
            else
            {
                sSQL = "select AddressCityAlias,AddressState from Warehouses where WarehouseNo = " + iWarehousePref1;
                ds = DataLink.Select(sSQL, DataLinkCon.bpss);

                sCity = Utility.GetStringValue(ds, "AddressCityAlias");
                sState = Utility.GetStringValue(ds, "AddressState");

                lblWarehouse1.Text = sCity + "," + sState;
                lblWarehouse1Qty.Text = "0";

            }


            iWarehousePref2 = Convert.ToInt32(ViewState["WarehousePref2"] == null ? "-1" : ViewState["WarehousePref2"].ToString());

            if (dtInven != null)
            {

                IEnumerable<DataRow> rowsPref2 = dtInven.AsEnumerable().Select(r => r)
                           .Where(c => (Convert.ToInt32(c["Location"]) == iWarehousePref2) && (c["TP1"].ToString() == (bIsTP1 ? "TP1" : "")));

                if (rowsPref2.Count() > 0)
                {
                    var dataPref2 = rowsPref2.Select(z => new { City = z["CityAlias"].ToString(), State = z["State"].ToString(), Qty = Convert.ToDecimal(z["QtyOnHand"]) });

                    foreach (var pref2 in dataPref2)
                    {
                        lblWarehouse2.Text = pref2.City + "," + pref2.State;
                        lblWarehouse2Qty.Text = pref2.Qty.ToString("#") == "" ? "0" : pref2.Qty.ToString("#");
                    }
                }

            }
            else
            {
                sSQL = "select AddressCityAlias,AddressState from Warehouses where WarehouseNo = " + iWarehousePref2;
                ds = DataLink.Select(sSQL, DataLinkCon.bpss);

                sCity = Utility.GetStringValue(ds, "AddressCityAlias");
                sState = Utility.GetStringValue(ds, "AddressState");

                lblWarehouse2.Text = sCity + "," + sState;
                lblWarehouse2Qty.Text = "0";

            }

            iWarehousePref3 = Convert.ToInt32(ViewState["WarehousePref3"] == null ? "-1" : ViewState["WarehousePref3"].ToString());

            if (dtInven != null)
            {
                IEnumerable<DataRow> rowsPref3 = dtInven.AsEnumerable().Select(r => r)
                           .Where(c => (Convert.ToInt32(c["Location"]) == iWarehousePref3) && (c["TP1"].ToString() == (bIsTP1 ? "TP1" : "")));

                if (rowsPref3.Count() > 0)
                {

                    var dataPref3 = rowsPref3.Select(z => new { City = z["CityAlias"].ToString(), State = z["State"].ToString(), Qty = Convert.ToDecimal(z["QtyOnHand"]) });

                    foreach (var pref3 in dataPref3)
                    {
                        lblWarehouse3.Text = pref3.City + "," + pref3.State;
                        lblWarehouse3Qty.Text = pref3.Qty.ToString("#") == "" ? "0" : pref3.Qty.ToString("#");
                    }
                }

            }
            else
            {
                sSQL = "select AddressCityAlias,AddressState from Warehouses where WarehouseNo = " + iWarehousePref3;
                ds = DataLink.Select(sSQL, DataLinkCon.bpss);

                sCity = Utility.GetStringValue(ds, "AddressCityAlias");
                sState = Utility.GetStringValue(ds, "AddressState");

                lblWarehouse3.Text = sCity + "," + sState;
                lblWarehouse3Qty.Text = "0";

            }


            //~/ InventoryPDF.aspx ? Agent = -1 & Name = Please + select + an + agent...& All = true & KVA = 45 & VoltageCat = CS & VoltageDisp = 240 D - 208Y / 120(CS) & Windings = Al & Searching = true


            //hlFullInventory.NavigateUrl = "~/InventoryPDF.aspx?Agent=-1&Name=''&All=true&KVA=0&VoltageCat=ALL&VoltageDisp=&Windings=&Searching=false";

            //hlFullInventory.NavigateUrl = "~/InventoryPDF.aspx?Agent=-1&Name=''&All=true&" +
            //                          "KVA=" + ddlKVA.Text +
            //                          "&VoltageCat=" + sVoltageCategory +
            //                          "&VoltageDisp=" + sStockVoltageDisplay +
            //                          "&Windings=" + (rblWindings.SelectedItem.Text == "Aluminum" ? "Al" : "Cu") +
            //                          "&Searching=true";

            hlFullInventory.NavigateUrl = "~/InventoryPDF.aspx?Agent=-1&Name=''&All=true&KVA=0&VoltageCat=ALL&VoltageDisp=&Windings=&Searching=false";

        }



        protected void ddlPrimaryVoltage_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bInternal = Convert.ToBoolean(Session["Internal"]);
            if (bInternal == true)
            {
                txtPrimaryVoltage.Text = ddlPrimaryVoltage.SelectedValue;
                lblPrimaryVoltageInvalid.Text = "";
                if (ddlPrimaryVoltage.Items.Count > 0)
                    ddlPrimaryVoltage.SelectedIndex = 0;
            }

            // We've selected something from the list, so it's standard.
            // Remove the display of nonstandard voltages.
            lblPrimaryShow.Text = "";

            //ClearEntries("Primary");

            CatalogNumberUpdate(true);
            NextFocus("ddlPrimaryVoltage");

            //RestoreEntries("Primary");
        }

        protected void ddlSecondaryVoltage_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bInternal = Convert.ToBoolean(Session["Internal"]);
            if (bInternal == true)
            {
                txtSecondaryVoltage.Text = ddlSecondaryVoltage.SelectedValue;
                lblSecondaryVoltageInvalid.Text = "";
                if (ddlSecondaryVoltage.Items.Count > 0)
                    ddlSecondaryVoltage.SelectedIndex = 0;
                //ClearEntries("Secondary");
            }

            // We've selected something from the list, so it's standard.
            // Remove the display of nonstandard voltages.
            lblSecondaryShow.Text = "";

            CatalogNumberUpdate(true);
            NextFocus("ddlSecondaryVoltage");

            //RestoreEntries("Secondary");
        }

        protected void ddlTempRise_SelectedIndexChanged(object sender, EventArgs e)
        {
            CatalogNumberUpdate(true);
            NextFocus("ddlTempRise");
        }

        /// <summary>
        /// Resets detail in general, or just between standard and custom.
        /// iStdCustom = 0  (In general)
        /// iStdCustom = 1  Setting to Standard.
        /// iStdCustom = 2  Setting to Custom.
        /// </summary>
        /// <param name="iStdCustom"></param>
        /// <param name="bEditing"></param>
        protected void ResetDetail(int iStdCustom, bool bEditing)
        {
            lblItemCopied.Text = "";
            lblKVAInvalid.Text = "";
            lblKVAInvalid.Visible = false;
            lblPrimaryVoltageInvalid.Text = "";
            lblSecondaryVoltageInvalid.Text = "";
            lblExcessAccessories.Visible = false;
            lblGeneral.Text = "";

            if (!bEditing)
            {
                lblAddEdit.Text = "Add Item to Quote";
                lblAddEdit.ForeColor = Color.Black;
            }

            string sKVA = "";

            rblElectrostaticShield.SelectedValue = "None";

            bool bInternal = Convert.ToBoolean(Session["Internal"]);
            // Hide capability of editing KVA.
            bool bWindingsCopper = rblWindings.SelectedValue == "Copper" ? true : false;

            switch (iStdCustom)
            {
                case 0:     // Resetting.  Not switching between Standard and Custom.

                    rblStandardOrCustom.SelectedIndex = -1;

                    txtQuoteDetailsID.Text = "0";
                    txtDetailID.Text = "0";

                    txtPrimaryVoltage.Text = "";
                    txtSecondaryVoltage.Text = "";
                    lblPrimaryVoltageInvalid.Text = "";
                    lblSecondaryVoltageInvalid.Text = "";

                    hidIsMatch.Value = "0";
                    lblCaseSize.Text = "";
                    lblCaseSizeCalc.Text = "";
                    if (ddlCaseSizes.Items.Count > 0)
                        ddlCaseSizes.SelectedIndex = -1;
                    lblLeadTimes.Text = "";

                    txtQuantity.Text = "";
                    txtKitQty.Text = "";
                    txtRBKitQty.Text = "";
                    txtOPKitQty.Text = "";
                    txtWBKitQty.Text = "";

                    lblTotalExtPrice.Text = "";
                    lblKitExtPrice.Text = "";
                    lblRBKitExtPrice.Text = "";
                    lblOPKitExtPrice.Text = "";
                    lblWBKitExtPrice.Text = "";
                    lblExpediteExtPrice.Text = "";
                    lblShippingAmtExt.Text = "";

                    txtUnitPrice.Text = "";
                    txtKitPrice.Text = "";
                    txtRBKitPrice.Text = "";
                    txtOPKitPrice.Text = "";
                    txtWBKitPrice.Text = "";
                    txtExpeditePrice.Text = "";

                    txtShippingReason.Text = "";
                    txtShippingAmount.Text = "";
                    lblShippingAmount.Text = "";
                    lblShippingReason.Text = "";

                    rblWindings.SelectedValue = "Aluminum";
                    rblPhase.SelectedValue = "Three";
                    rblPrimaryDW.SelectedValue = "D";
                    rblSecondaryDW.SelectedValue = "W";
                    txtKVA.Text = "";
                    txtPrimaryVoltage.Text = "";
                    txtSecondaryVoltage.Text = "";

                    lblKitName.Text = "";
                    lblKitID.Text = "";
                    lblKitIDOrig.Text = "";
                    lblKitPrice.Text = "";
                    lblKitExtPrice.Text = "";

                    lblExpeditePrice.Text = "";
                    lblExpediteExtPrice.Text = "";
                    ddlExpedite.SelectedValue = "";

                    LoadKVA(true, false, false, false);
                    LoadPrimaryVoltage(true, false, false);
                    LoadSecondaryVoltage(true, false, false);
                    LoadFrequency();
                    int iKva = 0;
                    string sKva = txtKVA.Text;
                    iKva = Utility.IntFromString(sKva);
                    LoadSoundReduct(iKva);
                    LoadEnclosure(1, 1);
                    rblMadeInUSA.SelectedValue = "None";
                    LoadSpecialFeatures();
                    LoadCaseColor("");
                    LoadCaseSizes();

                    SetFrequency("60 Hz (STD)");
                    if (ddlSoundReduct.Items.Count > 0)
                        ddlSoundReduct.SelectedIndex = 0;
                    SetEnclosure("HRPO NEMA 1 Indoor");
                    SetCaseColor("ANSI 61 (STD)");
                    txtCaseColorOther.Text = "";

                    chkLstSpecialFeatures.SelectedIndex = -1;
                    chkLstSpecialFeatures.Items[0].Selected = true;
                    txtSpecialFeatureNotes.Text = "";

                    chkMarineDuty.Checked = false;
                    hidTENV.Value = "0";

                    if (rblSpecialTypes.Items.Count > 0)
                        rblSpecialTypes.SelectedIndex = 0;

                    txtCustomerTagNo.Text = "";

                    break;

                case 1:     // Switching from Custom to Stock.

                    sKVA = txtKVA.Text;
                    bool bSinglePhase = rblPhase.SelectedValue == "Single" ? true : false;
                    bool bZigZag = (rblSpecialTypes.SelectedValue == "Zig Zag" || rblSpecialTypes.SelectedValue == "Harmonic Mitigating") ? true : false;

                    LoadKVA(false, bSinglePhase, bWindingsCopper, bZigZag);
                    string sKVAUsed = MatchKVA(sKVA);
                    SetKVA(sKVAUsed);

                    // TO DO:  Refresh ddlKVA.
                    // Copy KVA value to Stock.
                    string sPrimaryVoltage = ddlPrimaryVoltage.SelectedValue ?? "";
                    if (sPrimaryVoltage == "") sPrimaryVoltage = txtPrimaryVoltage.Text.ToString();

                    string sSecondaryVoltage = ddlSecondaryVoltage.SelectedValue ?? "";
                    if (sSecondaryVoltage == "") sSecondaryVoltage = txtSecondaryVoltage.Text.ToString();

                    decimal dKVA;
                    decimal.TryParse(sKVA, out dKVA);
                    bool bPhaseSingle = rblPhase.SelectedValue == "Single" ? true : false;

                    LoadConfiguration(dKVA, bPhaseSingle, bWindingsCopper);

                    var dtStock = q.ConfigurationToStock(sPrimaryVoltage, sSecondaryVoltage);

                    if (dtStock.Rows.Count > 0)
                    {
                        if (dtStock.Rows[0]["Configuration"].ToString() != "")
                        {
                            ListItem li = ddlConfiguration.Items.FindByValue(dtStock.Rows[0]["Configuration"].ToString());

                            if (li != null)
                                ddlConfiguration.SelectedValue = dtStock.Rows[0]["Configuration"].ToString();
                            else
                                ddlConfiguration.SelectedIndex = -1;
                        }
                    }

                    hidIsMatch.Value = "0";

                    CatalogNumberUpdate(true);     // Keeps custom prices.

                    break;

                case 2:     // Switching to Custom from Stock, or by default, simply bringing up empty record.

                    //rblStandardOrCustom.SelectedValue = "Custom";
                    //SetStandardOrCustom(true);
                    bPhaseSingle = rblPhase.SelectedValue == "Single" ? true : false;
                    // Show capability of editing KVA.

                    sKVA = ddlKVA.SelectedValue;
                    bZigZag = (rblSpecialTypes.SelectedValue == "Zig Zag" || rblSpecialTypes.SelectedValue == "Harmonic Mitigating") ? true : false;

                    LoadKVA(true, bPhaseSingle, bWindingsCopper, bZigZag);
                    SetKVA(sKVA);
                    txtKVA.Text = sKVA;
                    ddlKVA.SelectedIndex = 0;

                    LoadPrimaryVoltage(true, false, bPhaseSingle);
                    LoadSecondaryVoltage(true, false, bPhaseSingle);

                    string sConfiguration = ddlConfiguration.SelectedValue ?? "";
                    DataTable dtCustom = q.ConfigurationToCustom(sConfiguration);
                    if (dtCustom.Rows[0]["PrimaryVoltage"].ToString() != "")
                    {
                        rblPrimaryDW.SelectedValue = dtCustom.Rows[0]["PrimaryVoltageDW"].ToString();

                        if (bPhaseSingle == false)
                            ChangePrimaryDW(rblPrimaryDW.SelectedValue.ToString(CultureInfo.InvariantCulture));
                        rblSecondaryDW.SelectedValue = dtCustom.Rows[0]["SecondaryVoltageDW"].ToString();

                        if (bPhaseSingle == false)
                            ChangeSecondaryDW(rblSecondaryDW.SelectedValue.ToString(CultureInfo.InvariantCulture));

                        if (bInternal == true)
                        {
                            txtPrimaryVoltage.Text = dtCustom.Rows[0]["PrimaryVoltage"].ToString();
                            txtSecondaryVoltage.Text = dtCustom.Rows[0]["SecondaryVoltage"].ToString();

                            // Display nulls for dropdowns.
                            if (ddlPrimaryVoltage.Items.Count > 0)
                                ddlPrimaryVoltage.SelectedIndex = 0;
                            if (ddlSecondaryVoltage.Items.Count > 0)
                                ddlSecondaryVoltage.SelectedIndex = 0;
                        }
                        else
                        {
                            SetPrimaryVoltage(dtCustom.Rows[0]["PrimaryVoltage"].ToString());

                            SetSecondaryVoltage(dtCustom.Rows[0]["SecondaryVoltage"].ToString());
                        }
                    }

                    SetFrequency("60 Hz (STD)");
                    SetEnclosure("HRPO NEMA 3R Indoor/Outdoor");
                    SetCaseColor("ANSI 61 (STD)");

                    break;
            }

            lblEnclosureData.Text = "";
            lblCatalogNo.Text = "";
            lblCatalogNoExt.Text = "";

            // Called by SetStandardOrCustom().
            if (iStdCustom > 0)
            {
                Display(enumDisplay.All);
                return;
            }

            ddlConfiguration.SelectedValue = "";
            txtKVA.Text = "";
            rblPrimaryDW.SelectedValue = "D";
            if (ddlPrimaryVoltage.Items.Count > 0)
                ddlPrimaryVoltage.SelectedIndex = 0;
            rblSecondaryDW.SelectedValue = "W";
            if (ddlSecondaryVoltage.Items.Count > 0)
                ddlSecondaryVoltage.SelectedIndex = 0;
            SetKFactor("K-1 (STD)");

            if (ddlTempRise.Items.Count > 0)
                SetTempRise("150");

            lblUnitPrice.Text = "";
            lblItemExtPrice.Text = "";
            lblTotalExtPrice.Text = "";

            lblOPKitQtyOrig.Text = "";
            lblKitQtyOrig.Text = "";
            lblRBKitQtyOrig.Text = "";
            lblOPKitQtyOrig.Text = "";

            LoadQuoteItems();

            Display(enumDisplay.All);
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            // 3 columns = 999 max
            string sQty = txtQuantity.Text.ToString();
            sQty = sQty.Trim();
            if (sQty == "" || sQty == "0")
            {
                lblQuantityInvalid.Text = "";
                txtQuantity.Text = " 1";
            }
            else
            {
                string sMsg = dv.QuantityValid(sQty);
                if (sMsg != "")
                {
                    lblQuantityInvalid.Text = sMsg;
                }
                else
                {
                    lblQuantityInvalid.Text = "";

                    sQty = dv.NumberFormat(sQty, 3, 0);

                    sQty = sQty.Trim();
                    if (sQty == "" | sQty == "0")
                    {
                        txtQuantity.Text = " 1";
                    }
                    else
                    {
                        txtQuantity.Text = sQty;
                    }
                }
            }
            PriceChange();
            UpdatePrices("txtQuantity");

            CatalogNumberUpdate(false);
        }

        protected void btnReduceQty_Click(object sender, EventArgs e)
        {
            string sQty = dv.ChangeTextQuantity(txtQuantity.Text, false, 999);
            txtQuantity.Text = dv.NumberFormat(sQty.ToString(), 3, 0);
            PriceChange();
            UpdatePrices("btnReduceQty");

            CatalogNumberUpdate(false);
        }

        protected void btnAddQty_Click(object sender, EventArgs e)
        {
            string sQty = dv.ChangeTextQuantity(txtQuantity.Text, true, 999);
            txtQuantity.Text = dv.NumberFormat(sQty.ToString(), 3, 0);
            PriceChange();

            UpdatePrices("btnAddQty");
            CatalogNumberUpdate(false);
        }

        protected void btnKitReduceQty_Click(object sender, EventArgs e)
        {
            ResetKitNames();
            string sQty = dv.ChangeTextQuantity(txtKitQty.Text, false, 999);
            txtKitQty.Text = dv.NumberFormat(sQty.ToString(), 3, 0);

            UpdatePrices("btnKitReduce");
            CatalogNumberUpdate(true);      // Removes custom pricing, because may change configuration.  If removing last rain kit, will change Catalog number.
        }

        protected void btnKitAddQty_Click(object sender, EventArgs e)
        {
            ResetKitNames();
            string sQty = dv.ChangeTextQuantity(txtKitQty.Text, true, 999);
            txtKitQty.Text = dv.NumberFormat(sQty.ToString(), 3, 0);

            UpdatePrices("btnKitAdd");
            CatalogNumberUpdate(false);      // Removes custom pricing, because may change configuration. If adding a rain kit, will change Catalog number.
        }
        protected void UpdatePrices(string sCallingField)
        {
            // When a quote is copied in, the Quantity and Price are zero.  If this is such a situation,
            // get the latest price.

            // Expect to have fresh prices if quantity was zero.
            // Quantity would be zero for copied quote items.
            decimal dUnitPrice = DisplayPrice("Unit", sCallingField);
            decimal dKitPrice = DisplayPrice("Kit", sCallingField);
            decimal dLugKitPrice = DisplayPrice("LugKit", sCallingField);
            decimal dRBKitPrice = DisplayPrice("RBKit", sCallingField);
            decimal dOPKitPrice = DisplayPrice("OPKit", sCallingField);
            decimal dWBKitPrice = DisplayPrice("WBKit", sCallingField);
            decimal dShipPrice = DisplayPrice("Ship", sCallingField);

            // updates lblExpeditePrice.
            decimal dExpeditePrice = DisplayPrice("Expedite", sCallingField);

            int iQty = 0;
            if (!(lblQuantityInvalid.Visible && txtQuantity.Text == ""))
            {
                string sQty = (txtQuantity.Text == null || txtQuantity.Text.Trim() == "") ? "1" : txtQuantity.Text.ToString();
                iQty = Convert.ToInt32(sQty);
                txtQuantity.Text = dv.NumberFormat(sQty, 3, 0);
            }

            string sKitQty = (txtKitQty.Text == null || txtKitQty.Text.Trim() == "") ? "0" : txtKitQty.Text.ToString();
            int iKitQty = Convert.ToInt32(sKitQty);

            // Possibly change the drop down selection based on adding a rain hood or louver.
            if (sCallingField == "btnKitAdd" || sCallingField == "btnKitReduce" || sCallingField == "txtKitQty")
            {
                EnclosureKitChange(iKitQty);
            }

            string sRBKitQty = (txtRBKitQty.Text == null || txtRBKitQty.Text.Trim() == "") ? "0" : txtRBKitQty.Text.ToString();
            int iRBKitQty = Convert.ToInt32(sRBKitQty);

            string sOPKitQty = (txtOPKitQty.Text == null || txtOPKitQty.Text.Trim() == "") ? "0" : txtOPKitQty.Text.ToString();
            int iOPKitQty = Convert.ToInt32(sOPKitQty);

            string sLugKitQty = (txtLugKitQty.Text == null || txtLugKitQty.Text.Trim() == "") ? "0" : txtLugKitQty.Text.ToString();
            int iLugKitQty = Convert.ToInt32(sLugKitQty);

            string sWBKitQty = (txtWBKitQty.Text == null || txtWBKitQty.Text.Trim() == "") ? "0" : txtWBKitQty.Text.ToString();
            int iWBKitQty = Convert.ToInt32(sWBKitQty);

            bool bExpedite = ddlExpedite.SelectedIndex == 0 ? false : true;

            decimal dExtPrice = iQty * dUnitPrice;
            decimal dKitExtPrice = iKitQty * dKitPrice;
            decimal dLugKitExtPrice = iLugKitQty * dLugKitPrice;
            decimal dRBKitExtPrice = iRBKitQty * dRBKitPrice;
            decimal dOPKitExtPrice = iOPKitQty * dOPKitPrice;
            decimal dWBKitExtPrice = iWBKitQty * dWBKitPrice;
            decimal dExpediteExtPrice = (bExpedite == true) ? iQty * dExpeditePrice : 0;

            txtExpeditePrice.Text = (dExpeditePrice >= 0) ? dv.NumberFormat(dExpeditePrice.ToString(), 9, 2) : "";
            //lblExpeditePrice.Text = txtExpeditePrice.Text;

            decimal dTotalExtPrice = 0;

            if (iQty > 0 && dUnitPrice == 0)
            {
                dTotalExtPrice = 0;
            }
            else
            {
                dTotalExtPrice = dExtPrice + dKitExtPrice + dRBKitExtPrice + dOPKitExtPrice + dWBKitExtPrice + dExpediteExtPrice;
            }

            bool bInternal = Convert.ToBoolean(Session["Internal"]);

            // Only show See Factory for Pricing message for external reps when price is zero.
            lblSeeFactory.Visible = (dUnitPrice == 0 && !bInternal) ? true : false;

            //txtUnitPrice.Text = dv.NumberFormat(dUnitPrice.ToString(), 9, 2);
            //txtUnitPriceChanged.Text = txtUnitPrice.Text;

            lblItemExtPrice.Text = dv.NumberFormat(dExtPrice.ToString(), 12, 2);
            lblKitExtPrice.Text = dv.NumberFormat(dKitExtPrice.ToString(), 12, 2);
            lblLugKitExtPrice.Text = dv.NumberFormat(dLugKitExtPrice.ToString(), 12, 2);
            lblRBKitExtPrice.Text = dv.NumberFormat(dRBKitExtPrice.ToString(), 12, 2);
            lblWBKitExtPrice.Text = dv.NumberFormat(dWBKitExtPrice.ToString(), 12, 2);
            lblExpediteExtPrice.Text = dv.NumberFormat(dExpediteExtPrice.ToString(), 12, 2);

            lblTotalExtPrice.Text = dv.NumberFormat(dTotalExtPrice.ToString(), 12, 2);

            btnSave.Visible = (lblTotalExtPrice.Visible == true && lblKVAInvalid.Visible == false && lblKitQtyInvalid.Visible == false
                            && lblRBKitQtyInvalid.Visible == false && lblOPKitQtyInvalid.Visible == false && lblLugKitQtyInvalid.Visible == false 
                            && lblWBKitQtyInvalid.Visible == false) ? true : false;

            // Shows a warning if the quantity of accessories ordered exceeds the quantity of items ordered on this line.

            // If Kit is not a wall bracket, iWBKitQty = 0.  If it is a wall bracket, then either you choose a side mount or bottom mount, but not both,
            // so (iKitQty + iWBKitQty) is your bracket quantity.

            lblExcessAccessories.Visible = (iKitQty + iWBKitQty > iQty || iRBKitQty > iQty || iOPKitQty > iQty || iLugKitQty > iQty) ? true : false;

            Display(enumDisplay.Price);
        }

        protected void txtKitQty_TextChanged(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = txtKitQty.Text.ToString();
            sQty = sQty.Trim();
            if (sQty == "")
            {
                lblKitQtyInvalid.Text = "";
            }
            else
            {
                string sMsg = dv.QuantityValid(sQty);
                if (sMsg != "")
                {
                    lblKitQtyInvalid.Text = sMsg;
                    txtKitQty.Text = "";
                }
                else
                {
                    lblKitQtyInvalid.Text = "";

                    // 3 columns = 999 max
                    txtKitQty.Text = dv.NumberFormat(sQty, 3, 0);
                }
            }

            UpdatePrices("txtKitQty");
            CatalogNumberUpdate(true);      // Removes custom pricing, because may change configuration. If adding a rain kit, will change Catalog number.
        }

        protected void gvQuoteItems_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            LoadQuoteItems();
        }

        protected void btnEmail_Click(object sender, EventArgs e)
        {
            int iQuoteID = int.Parse(Session["QuoteID"].ToString());

            // Will be reset to False in Page_Unload();
            bButtonPress = true;

            Session["PageName"] = "Email";
            Response.Redirect("Email.aspx?QuoteID=" + iQuoteID.ToString());
        }

        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            Finalize(false);
        }


        /// <summary>
        /// Enable Finalized button if Project name entered.
        /// </summary>
        protected void EnableFinalizeButton()
        {
            bool bApprovalStarted = txtNotesRequest.Visible;
            bool bEnable = (txtCompany.Text == null || txtCompany.Text == "") ? false : true;
            bool bEnableFinalize = false;
            bool bFinalized = (lblStatus.Text == "Cart" || lblStatus.Text == "PendAppr") ? false : true;
            bool bRequestEntered = (String.IsNullOrEmpty(txtNotesRequest.Text) == true) ? false : true;
            bool bSaveVisible = btnSave.Visible;
            bool bStandard = (rblStandardOrCustom.SelectedValue == "Standard") ? true : false;

            // Do we have a price?
            if (bEnable)
            {
                string s = lblTotalPrice.Text;
                if (!String.IsNullOrEmpty(s))
                {
                    s = s.Substring(1, s.Length - 1);       // Remove $ at left.
                    decimal dTotalPrice = decimal.Parse(s);
                    if (dTotalPrice > 0)
                    {
                        bEnableFinalize = true;
                    }
                }
            }

            // If save button is visible, don't enable, since we want the user to save first.
            // If approval is required, hasn't yet been requested, and we don't have the necessary information, don't enable.
            if (bEnableFinalize)
            {
                if (bSaveVisible)
                {
                    bEnableFinalize = false;
                }
                else if (!bFinalized && bApprovalStarted && !bRequestEntered)
                {
                    bEnableFinalize = false;
                }
            }

            btnFinalize.Enabled = bEnableFinalize;
            btnApprove.Enabled = bEnableFinalize;
        }

        /// <summary>
        /// Return True if quote is ready to finalize.
        /// </summary>
        /// <returns></returns>
        protected bool ValidQuote(int iQuoteID)
        {
            string s = txtCompany.Text;
            if (String.IsNullOrEmpty(s))
            {
                lblCompanyReqd.Visible = true;
                lblFinalizeErrors.Text = "Please enter Company name.";
                lblFinalizeErrors.Visible = true;
                txtCompany.Focus();
                return false;
            }

            if (lblEmailInvalid.Visible)
            {
                lblFinalizeErrors.Text = "Please correct email.";
                lblFinalizeErrors.Visible = true;
                txtEmail.Focus();
                return false;
            }

            if (!q.QuoteHasItems(iQuoteID))
            {
                lblFinalizeErrors.Text = "No items yet selected.";
                lblFinalizeErrors.Visible = true;
                return false;
            }

            // Obsolete...
            //if (!q.QuoteHasNotes(iQuoteID))
            //{
            //    lblFinalizeErrors.Text = "Hidden KVA, Primary or Secondary Voltages require Internal - PRIVATE notes.";
            //    lblFinalizeErrors.Visible = true;
            //    return false;
            //}

            lblFinalizeErrors.Visible = false;
            return true;
        }
        /// <summary>
        /// Save Quote information, not Quote Detail.
        /// Returns QuoteID.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <returns></returns>
        protected int SaveQuote(int iQuoteID)
        {
            string sProject = txtProject.Text.ToString();
            int iCustomerID = 0;
            if (!string.IsNullOrEmpty(txtCustomerID.Text))
            {
                iCustomerID = Convert.ToInt32(txtCustomerID.Text);
            }

            // Pressing the Back button after having logged out
            // produces an error because the system somehow loads the QuoteID = 999 into lblQuoteID,
            // and I can't find out where it does this or why -- though I expect that taking an int
            // value of a null Session["QuoteID"] produces 999.
            if (iQuoteID == 999 && iCustomerID == 0)
            {
                return 0;
            }

            int iCustomerContactID = 0;
            if (txtCustomerContactID.Text != null && txtCustomerContactID.Text != "")
            {
                iCustomerContactID = Convert.ToInt32(txtCustomerContactID.Text);
            }
            string sCompany = txtCompany.Text.ToString();
            string sCity = txtCity.Text.ToString();
            string sContactName = txtContactName.Text.ToString();

            string sEmail = txtEmail.Text.ToString();

            string sNotes = txtNotes.Text.ToString();
            string sNotesInternal = txtNotesInternal.Text.ToString();
            string sNotesPDF = txtNotesPDF.Text.ToString();
            // Synchronize the PDF notes displayed with the latest ones entered.
            lblNotesPDF.Text = sNotesPDF;

            string sNotesRequest = txtNotesRequest.Text.ToString();

            int iRepID = Convert.ToInt32(lblRepID.Text);
            int iRepDistributorID = Convert.ToInt32(lblRepDistributorID.Text);

            string sUserName = ddlRep.SelectedValue.ToString();
            string sUserNameLast = Session["UserName"].ToString();

            string sQuoteCurrent = lblFullNameLatest.Text.ToString();

            sQuoteCurrent = String.IsNullOrEmpty(sQuoteCurrent) == true ? sUserNameLast : sQuoteCurrent;
            if (sQuoteCurrent == null || sQuoteCurrent == "")
            {
                lblQuotedByReqd.Visible = true;
                lblNotSaved.Visible = true;
                return 0;
            }
            else
            {
                lblQuotedByReqd.Visible = false;
                lblNotSaved.Visible = false;
            }

            string sUserNameApproval = Session["UserName"].ToString();

            // Returns "Q" for external reps, the login's last initial letter for internal reps.
            // RepObject r = new RepObject();
            string sQuoteOriginCode = r.OriginCode(iRepID, sUserName);
            bool bHasQuoteId = iQuoteID > 0 ? true : false;

            bool bNoDrawingsAttached = chkNoDrawingsAttached.Checked;
            bool bNoFreeShipping = chkNoFreeShipping.Checked;
            bool bOEM = chkOEM.Checked;
            bool bWiringDiagram = UseWiringDiagram();       // IEM only.

            DataSet ds = q.QuoteUpdate(iQuoteID, sCity, sCompany, sContactName,
                                    iCustomerContactID, iCustomerID, sEmail, bNoDrawingsAttached, bOEM, bWiringDiagram, bNoFreeShipping,
                                    sNotes, sNotesInternal, sNotesPDF, sNotesRequest,
                                    sProject, sQuoteOriginCode, iRepDistributorID, iRepID,
                                    sUserName, sUserNameLast);

            DataRow dr = ds.Tables[0].Rows[0];

            int iRepDistributorIDNew = Convert.ToInt32(ds.Tables[0].Rows[0]["RepDistributorID"]);

            if (iRepDistributorID != iRepDistributorIDNew)
                Session["RepDistributorID"] = iRepDistributorIDNew;

            iQuoteID = Convert.ToInt32(dr["QuoteID"]);
            lblQuoteID.Text = iQuoteID.ToString();
            lblQuoteNo.Text = dr["QuoteNo"].ToString();
            lblQuoteNoVer.Text = dr["QuoteNoVer"].ToString();

            txtCustomerID.Text = dr["CustomerID"].ToString();
            txtCustomerContactID.Text = dr["CustomerContactID"].ToString();

            // Store the new Quote Origin Code when saved, if it is a new quote.
            if (!bHasQuoteId)
            {
                lblQuoteOriginCode.Text = sQuoteOriginCode;
            }
            return iQuoteID;
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            DataValidation dv = new DataValidation();
            string sPath = dv.CurrentDir(true);
            int iQuoteID = Convert.ToInt32(lblQuoteID.Text);
            bool bOEMEmail;
            bool bCreatePDF = false;

            SaveQuote(iQuoteID);

            string sStatus = lblStatus.Text.ToString();

            bool bOEM = chkOEM.Checked;
            bool bNoPrices = lblApprovalReqd.Visible == true || lblApprovalRequested.Visible == true ? true : false;
            bool bFinalized = btnFinalize.Visible == false && bNoPrices == false && btnSubmit.Visible == false ? true : false;

            // Don't create a PDF if it is Finalized, as this was done as part of the finalization process.
            // This avoids creating a new PDF.
            if (!bFinalized)
            {
                bCreatePDF = q.CreatePDF(iQuoteID, bNoPrices, bOEM, bFinalized, false, out bOEMEmail);

            }
            string sURL = q.QuotePDFUrl(iQuoteID, false, false);


            // Hard-coding to production since we don't seem to have access rights to QA.
            // **************************************************************************
            string sPathFull = "";
            if (Convert.ToBoolean(WebConfigurationManager.AppSettings["LocalMachine"]) == true)
                sPathFull = WebConfigurationManager.AppSettings["LocalWebSiteUrl"] + sURL;
            else
                sPathFull = "https://MGMQuotation.MGMTransformer.com//MGMQuotation//pdfs//" + sURL;// +
            // (HttpContext.Current.IsDebuggingEnabled ? "?PassThru=true" : "");
            //string sPathFull = "https://WebQuotes//" + sURL;
            // **************************************************************************

            // Open PDF in another browser.
            ResponseHelper.Redirect(sPathFull, "_blank", "");

            //System.Diagnostics.Process.Start(sPath + "\\" + sURL);
        }

        protected void gvQuoteItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvQuoteItems.SelectedIndex = -1;
            gvQuoteItems.DataBind();

        }

        protected bool IsCustom()
        {
            RadioButtonList rbl = (RadioButtonList)uprblStandardOrCustom.FindControl("rblStandardOrCustom");
            return (rbl.SelectedValue.ToString() == "Custom") ? true : false;
        }

        protected void btnRBKitReduceQty_Click(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = dv.ChangeTextQuantity(txtRBKitQty.Text, false, 999);
            txtRBKitQty.Text = dv.NumberFormat(sQty.ToString(), 3, 0);
            UpdatePrices("btnRBKitReduceQty");
            CatalogNumberUpdate(false);
        }

        protected void btnRBKitAddQty_Click(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = dv.ChangeTextQuantity(txtRBKitQty.Text, true, 999);
            txtRBKitQty.Text = dv.NumberFormat(sQty.ToString(), 3, 0);
            UpdatePrices("btnRBKitAddQty");
            CatalogNumberUpdate(false);
        }

        protected void txtRBKitQty_TextChanged(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = txtRBKitQty.Text.ToString();
            sQty = sQty.Trim();
            if (sQty == "")
            {
                lblRBKitQtyInvalid.Text = "";
                lblRBKitQtyInvalid.Visible = false;
            }
            else
            {
                string sMsg = dv.QuantityValid(sQty);
                if (sMsg != "")
                {
                    lblRBKitQtyInvalid.Text = sMsg;
                    lblRBKitQtyInvalid.Visible = true;
                    txtRBKitQty.Text = "";
                }
                else
                {
                    lblRBKitQtyInvalid.Text = "";
                    lblRBKitQtyInvalid.Visible = false;

                    // 3 columns = 999 max
                    txtRBKitQty.Text = dv.NumberFormat(sQty, 3, 0);
                }
            }
            UpdatePrices("btnRBKitQty");
            CatalogNumberUpdate(false);
        }

        protected void btnOPKitReduceQty_Click(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = dv.ChangeTextQuantity(txtOPKitQty.Text, false, 999);
            txtOPKitQty.Text = dv.NumberFormat(sQty.ToString(), 3, 0);
            UpdatePrices("btnOPKitReduceQty");
            OSHPD_Qty_Change();
            CatalogNumberUpdate(false);
        }

        protected void btnOPKitAddQty_Click(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = dv.ChangeTextQuantity(txtOPKitQty.Text, true, 999);
            txtOPKitQty.Text = dv.NumberFormat(sQty.ToString(), 3, 0);
            UpdatePrices("btnOPKitAddQty");
            OSHPD_Qty_Change();
            CatalogNumberUpdate(false);
        }

        protected void btnLugKitAddQty_Click(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = dv.ChangeTextQuantity(txtLugKitQty.Text, true, 999);
            txtLugKitQty.Text = dv.NumberFormat(sQty.ToString(), 3, 0);
            UpdatePrices("btnLugKitAddQty");
            OSHPD_Qty_Change();
            CatalogNumberUpdate(false);
        }
        protected void btnLugKitReduceQty_Click(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = dv.ChangeTextQuantity(txtLugKitQty.Text, false, 999);
            txtLugKitQty.Text = dv.NumberFormat(sQty.ToString(), 3, 0);
            UpdatePrices("btnLugKitReduceQty");
            OSHPD_Qty_Change();
            CatalogNumberUpdate(false);
        }


        protected void txtOPKitQty_TextChanged(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = txtOPKitQty.Text.ToString();
            sQty = sQty.Trim();
            if (sQty == "")
            {
                lblOPKitQtyInvalid.Text = "";
                lblOPKitQtyInvalid.Visible = false;
            }
            else
            {
                string sMsg = dv.QuantityValid(sQty);
                if (sMsg != "")
                {
                    lblOPKitQtyInvalid.Text = sMsg;
                    lblOPKitQtyInvalid.Visible = true;
                    txtOPKitQty.Text = "";
                }
                else
                {
                    lblOPKitQtyInvalid.Text = "";
                    lblOPKitQtyInvalid.Visible = false;

                    // 3 columns = 999 max
                    txtOPKitQty.Text = dv.NumberFormat(sQty, 3, 0);
                }
            }
            UpdatePrices("txtOPKitQty");
            OSHPD_Qty_Change();
            CatalogNumberUpdate(false);
        }

        protected void txtLugKitQty_TextChanged(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = txtLugKitQty.Text.ToString();
            sQty = sQty.Trim();
            if (sQty == "")
            {
                lblLugKitQtyInvalid.Text = "";
                lblLugKitQtyInvalid.Visible = false;
            }
            else
            {
                string sMsg = dv.QuantityValid(sQty);
                if (sMsg != "")
                {
                    lblLugKitQtyInvalid.Text = sMsg;
                    lblLugKitQtyInvalid.Visible = true;
                    txtLugKitQty.Text = "";
                }
                else
                {
                    lblLugKitQtyInvalid.Text = "";
                    lblLugKitQtyInvalid.Visible = false;

                    // 3 columns = 999 max
                    txtLugKitQty.Text = dv.NumberFormat(sQty, 3, 0);
                }
            }
            UpdatePrices("txtLugKitQty");
            OSHPD_Qty_Change();
            CatalogNumberUpdate(false);
        }

        /// <summary>
        /// Updates the text in the FreightIncluded label when conditions change.
        /// </summary>
        /// <param name="iQuoteID"></param>
        protected void UpdateFreightIncluded(int iQuoteID)
        {
            if (iQuoteID == 0)
            {
                lblShippingTitle.Visible = false;
            }
            else
            {
                lblShippingTitle.Visible = true;
                lblShipping.Text = q.FreightIncluded(iQuoteID);
            }
        }

        // Called when changing Customers, to see if we need to change the RepDistributorID.
        protected void SwapDistributors(int iRepDistributorID)
        {
            if (iRepDistributorID == 0)
                return;

            // Update RepDistributor info.
            Session["RepDistributorID"] = iRepDistributorID;

            RepObject ro = new RepObject();

            DataTable dt = ro.GetName(iRepDistributorID);
            string sRepName = dt.Rows[0]["Full_Name"].ToString();

            bool bSpecialPricing = Convert.ToBoolean(dt.Rows[0]["SpecialPricing"]);

            Session["RepDistributorName"] = sRepName;
            lblRepDistributorID.Text = iRepDistributorID.ToString();
            lblRepDistributor.Text = sRepName;

            int iRepID = Convert.ToInt32(lblRepID.Text);

            lblPricing.Visible = bSpecialPricing;
            lblRepDistributor.Visible = bSpecialPricing;
            lblRepDistributorAlt.Visible = bSpecialPricing;

        }

        /// <summary>
        /// Builds a one or two field Catalog Number, to prevent screen from 
        /// displaying in an odd manner.
        /// </summary>
        /// <param name="sCatalogNo"></param>
        protected void BuildCatalogNo(string sCatalogNo)
        {
            int iPos = sCatalogNo.IndexOf("-");
            int iLen = sCatalogNo.Length;

            string sCatalog1 = sCatalogNo;
            string sCatalog2 = "";

            if (iLen - iPos > 8)
            {
                sCatalog1 = sCatalogNo.Substring(0, iPos + 1);
                sCatalog2 = sCatalogNo.Substring(iPos + 1, iLen - iPos - 1);
            }

            lblCatalogNo.Text = sCatalog1;
            lblCatalogNoExt.Text = sCatalog2;
        }

        /// <summary>
        /// Builds the Quote number and display.  Called by LoadQuote() and SaveItem().
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <param name="iQuoteNo"></param>
        /// <param name="iQuoteVer"></param>
        /// <param name="sQuoteOriginCode"></param>
        protected void SetQuoteNoDisplay(int iQuoteID, int iQuoteNo, int iQuoteVer, string sQuoteOriginCode)
        {
            // This should already be set.
            //lblQuoteID.Text = iQuoteID.ToString();

            string sQuoteNoVer = "W" + sQuoteOriginCode + iQuoteNo.ToString("D5");

            if (iQuoteVer > 1)
                sQuoteNoVer = sQuoteNoVer + '.' + iQuoteVer.ToString();

            lblQuoteNoAndVer.Text = sQuoteNoVer;
            Session["QuoteNoDisplay"] = sQuoteNoVer;

            if (iQuoteID == 0)
            {
                lblQuoteIDPrefix.Text = "New Quote";
            }
            else
            {
                lblQuoteIDPrefix.Text = "Quote #";
            }
        }

        protected void gvQuoteItems_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void txtCompany_TextChanged(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
            Display(enumDisplay.All);
        }

        /// <summary>
        /// Force the Focus, since the update code loses the tab order.
        /// </summary>
        /// <param name="sCtlName"></param>
        protected void NextFocus(string sCtlName)
        {
            bool bStandard = (rblStandardOrCustom.SelectedValue == "Standard") ? true : false;

            switch (sCtlName)
            {
                case "rblStandardOrCustom":
                    rblWindings.Focus();
                    break;
                case "rblWindings":
                    if (bStandard)
                        rblPhase.Focus();
                    else
                        ddlKVA.Focus();
                    break;
                case "rblPhase":
                    ddlKVA.Focus();
                    break;
                case "ddlKVA":
                    if (bStandard)
                        ddlConfiguration.Focus();
                    else
                        rblPrimaryDW.Focus();
                    break;
                case "ddlConfiguration":
                    btnAddQty.Focus();
                    break;
                case "rblPrimaryDW":
                    ddlPrimaryVoltage.Focus();
                    break;
                case "ddlPrimaryVoltage":
                    rblSecondaryDW.Focus();
                    break;
                case "rblSecondaryDW":
                    ddlSecondaryVoltage.Focus();
                    break;
                case "ddlSecondaryVoltage":
                    ddlKFactor.Focus();
                    break;
                case "ddlKFactor":
                    ddlTempRise.Focus();
                    break;
                case "ddlTempRise":
                    rblElectrostaticShield.Focus();
                    break;
                case "rblElectrostaticShield":
                    btnAddQty.Focus();
                    break;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            // This button should only be visible if the quote is finalized.
            int iQuoteID = Convert.ToInt32(lblQuoteID.Text);

            // When reopening, always keep exactly the same number, regardless of the status of the chkSameQuoteNo checkbox.
            // This is new, because allowing to change the number here did not produce a continuing sequence of version numbers.
            string sUserName = Session["UserName"].ToString();

            // Changes status back to Cart.
            q.QuoteEdit(iQuoteID, sUserName);

            // Redraw the page.
            Response.Redirect(Request.RawUrl);
        }

        protected void ddlExpedite_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblGeneral.Text = "";

            Display(enumDisplay.Price);

            UpdatePrices("ddlExpedite");
        }

        /// <summary>
        /// Display and return value of Price, for updating extended values.
        /// </summary>
        /// <param name="sPriceName"></param>
        /// <param name="sCallingField"></param>
        /// <returns></returns>
        protected decimal DisplayPrice(string sPriceName, string sCallingField)
        {
            decimal decPrice = 0;
            decimal decAmt = 0;
            decimal decLblAmt = 0;

            // Special handling for shipping amount.
            if (sPriceName == "Ship")
            {
                string sShipAmount = txtShippingAmount.Text.ToString();
                if (String.IsNullOrEmpty(sShipAmount) == false)
                {
                    txtShippingAmount.Text = dv.NumberFormat(txtShippingAmount.Text.ToString(), 9, 2, true);
                    lblShippingAmtExt.Text = txtShippingAmount.Text;
                    decimal.TryParse(txtShippingAmount.Text.ToString(), out decPrice);
                    string sShipReason = txtShippingReason.Text.ToString();
                    lblShippingReasonInvalid.Visible = (String.IsNullOrEmpty(sShipReason)) ? true : false;

                    return decPrice;
                }
                return 0;
            }

            // Expedite pulls the price into the label first.
            if ((sPriceName == "Expedite" && (sCallingField == "ddlExpedite" || sCallingField == "LoadQuoteDetail"
                                || sCallingField == "CatalogNumberUpdate"))
                || sCallingField == "txtUnitPrice")
            {
                int iQuoteID = Convert.ToInt32(lblQuoteID.Text);

                string sPrice = txtUnitPrice.Text.ToString();
                decimal dUnitPrice = 0;
                decimal.TryParse(txtUnitPrice.Text.ToString(), out dUnitPrice);
                if (dUnitPrice == 0)
                {
                    sPrice = lblUnitPrice.Text.ToString();
                    decimal.TryParse(sPrice, out dUnitPrice);
                }

                string sExpedite = ddlExpedite.SelectedValue.ToString().Trim();
                sExpedite = (sExpedite == null || sExpedite == "") ? "0" : sExpedite;
                int iNoDays = Convert.ToInt32(sExpedite);
                int iExpediteFees = q.ExpediteFees(iQuoteID, dUnitPrice, iNoDays);
                lblExpeditePriceCalc.Text = dv.NumberFormat(iExpediteFees.ToString(), 9, 2);
                lblExpeditePrice.Text = lblExpeditePriceCalc.Text;
                string sQty = String.IsNullOrEmpty(txtQuantity.Text.ToString()) == true ? "1" : txtQuantity.Text.ToString();
                int iQty = 1;
                int.TryParse(sQty, out iQty);
                decimal decExpediteExt = iQty * iExpediteFees;
                lblExpediteExtPrice.Text = dv.NumberFormat(decExpediteExt.ToString(), 9, 2);


                if (sPriceName == "Expedite")
                {
                    decPrice = Convert.ToDecimal(iExpediteFees);
                    return decPrice;
                }
                // Overwrite what's been entered for Expedite Price when Unit Price changes.
                if (txtExpeditePrice.Visible == true && (sCallingField == "txtUnitPrice" || sCallingField == "CatalogNumberUpdate"))
                    txtExpeditePrice.Text = lblExpeditePrice.Text;
            }

            Label lbl = (Label)FindControl("lbl" + sPriceName + "Price");
            string slblAmt = (lbl.Text == null || lbl.Text.Trim() == "") ? "0" : lbl.Text.ToString();
            TextBox txt = (TextBox)FindControl("txt" + sPriceName + "Price");
            string stxtAmt = (txt.Text == null || txt.Text.Trim() == "") ? "0" : txt.Text.ToString();

            // Check label or text for amount.  Use whichever we find.
            decimal decUnitPrice = 0;
            string sUnitPrice = slblAmt;
            decimal.TryParse(sUnitPrice, out decUnitPrice);

            if (decUnitPrice == 0)
            {
                sUnitPrice = stxtAmt;
                decimal.TryParse(sUnitPrice, out decUnitPrice);
            }

            // Handle Visibility and formatting.

            bool bInternal = Convert.ToBoolean(Session["Internal"]);

            switch (sPriceName)
            {
                case "Unit":
                    // Formatting only. Doesn't change visibility.
                    TextBox txtUnitQty = (TextBox)FindControl("txtQuantity");
                    string sQuantity = txtUnitQty.Text.ToString().Trim();
                    txtUnitQty.Text = dv.NumberFormat(sQuantity, 3, 0);
                    if (bInternal == true)
                    {
                        lbl.Visible = false;
                        txt.Visible = true;
                        txt.Text = dv.NumberFormat(sUnitPrice, 9, 2);
                    }
                    else
                    {
                        lbl.Visible = true;
                        lbl.Text = dv.NumberFormat(sUnitPrice, 9, 2);
                        txt.Visible = false;
                    }

                    // Show only txtUnitPrice or txtUnitPriceChanged.  Never both.
                    // Base it on whether or not txtUnitPrice = lblUnitPriceCalc.  Yes: Show txtUnitPrice.
                    //                                                              No: Show txtUnitPriceChanged.
                    decimal decUnitPriceCalc = 0;
                    string sUnitPriceCalc = lblUnitPriceCalc.Text.ToString();
                    decimal.TryParse(sUnitPriceCalc, out decUnitPriceCalc);

                    bool bChanged = (decUnitPrice != decUnitPriceCalc) ? true : false;

                    txtUnitPrice.Visible = bInternal && !bChanged;
                    txtUnitPriceChanged.Visible = bInternal && bChanged;

                    break;
                case "Kit":
                case "RBKit":
                case "OPKit":
                case "LugKit":
                case "WBKit":

                    // Formatting and visibility.
                    TextBox txtQty = (TextBox)FindControl("txt" + sPriceName + "Qty");
                    string sQty = (txtQty.Text == null || txtQty.Text.Trim() == "") ? "0" : txtQty.Text.ToString();
                    int iQty = (sQty == null) ? 0 : Convert.ToInt32(sQty);
                    txtQty.Text = dv.NumberFormat(sQty, 3, 0);

                    if (bInternal == true)
                    {
                        lbl.Visible = false;
                        txt.Visible = true;
                        txt.Text = dv.NumberFormat(stxtAmt, 9, 2);
                    }
                    else
                    {
                        lbl.Visible = true;
                        lbl.Text = dv.NumberFormat(slblAmt, 9, 2);
                        txt.Visible = false;
                    }
                    break;

                case "Expedite":

                    if (bInternal == true)
                    {
                        lbl.Visible = false;
                        txt.Visible = true;
                        txt.Text = dv.NumberFormat(stxtAmt, 9, 2);
                    }
                    else
                    {
                        lbl.Visible = true;
                        lbl.Text = dv.NumberFormat(slblAmt, 9, 2);
                        txt.Visible = false;
                    }
                    break;
            }

            // Handle overwriting of textbox based on calling field.
            switch (sPriceName)
            {
                case "Unit":
                case "Kit":
                case "RBKit":
                case "OPKit":
                case "WBKit":
                    // Forces price overwrite if empty, or changing quantity.

                    decimal.TryParse(stxtAmt, out decAmt);
                    decimal.TryParse(slblAmt, out decLblAmt);

                    if (decAmt < 0 && decLblAmt != 0)
                    {
                        txt.Text = lbl.Text;
                    }
                    break;
                case "Expedite":
                    if (sCallingField == "ddlExpedite")
                    {
                        txt.Text = lbl.Text;
                    }
                    break;
            }

            // Return the price of whichever control is visible.
            if (txt.Visible == true)
            {
                decPrice = dv.DecimalFromText(txt.Text.ToString());
            }
            else
            {
                if (sPriceName == "Unit")
                {
                    if (txtUnitPriceChanged.Visible == true)
                        decPrice = dv.DecimalFromText(txtUnitPriceChanged.Text.ToString());
                    else
                        decPrice = dv.DecimalFromText(lblUnitPrice.Text.ToString());
                }
                else
                    decPrice = dv.DecimalFromText(lbl.Text.ToString()); ;
            }

            return decPrice;
        }
        /// <summary>
        /// Necessary because of warning when kits were ordered, then removed,
        /// and a new kit is now going to be ordered.
        /// </summary>
        protected void ResetKitNames()
        {
            lblAccessoryChange.Visible = false;
            lblKitIDOrig.Text = lblKitID.Text;
            lblOPKitNameOrig.Text = lblOPKitName.Text;
            lblRBKitNameOrig.Text = lblRBKitName.Text;
            lblWBKitNameOrig.Text = lblWBKitName.Text;

            lblKitQtyOrig.Text = "";
            lblOPKitQtyOrig.Text = "";
            lblRBKitQtyOrig.Text = "";
            lblWBKitQtyOrig.Text = "";
        }

        protected void ViewShowNotesLink(bool bShow, bool bForce)
        {
            lblShowNotes.Text = (bShow == true) ? "true" : "false";
            pnlNotes.Visible = bShow;
            lnkbHideNotes.Visible = bShow;
            lnkbShowNotes.Visible = !bShow;
            btnSaveNotes.Visible = bShow;

            if (!bShow)
            {
                NotesSaved(false);

                // Turning off Request Approval button, if selected.
                if (bForce == true)
                {
                    lblbtnRequestApprovalClicked.Text = "false";
                    if (String.IsNullOrEmpty(txtNotesRequest.Text.ToString()) == false)
                        txtNotesRequest.Text = null;
                }

                bool bHasNotes = (!String.IsNullOrEmpty(txtNotes.Text.ToString().Trim())
                                || !String.IsNullOrEmpty(txtNotesPDF.Text.ToString().Trim())
                                || !String.IsNullOrEmpty(txtNotesInternal.Text.ToString().Trim())
                                || !String.IsNullOrEmpty(txtNotesRequest.Text.ToString().Trim()));
                if (bHasNotes)
                    lnkbShowNotes.Text = "Show Notes (Notes exist)";
                else
                    lnkbShowNotes.Text = "Show Notes";
            }
            else
            {
                // This is not whether or not request notes exist.  It is whether they have been asked for.
                bool bHasRequestNotes = lblbtnRequestApprovalClicked.Text.ToString() == "true" ? true : false;

                // Also make this true if there is anything already in the field.
                bHasRequestNotes = !String.IsNullOrEmpty(txtNotesRequest.Text.ToString()) ? true : bHasRequestNotes;

                if (bHasRequestNotes)
                    lnkbHideNotes.Text = "Hide Notes (Remove Request)";
                else
                    lnkbHideNotes.Text = "Hide Notes";
            }

            Display(enumDisplay.Notes);
        }

        // Save an individual item.
        protected bool SaveItem(bool bCopy)
        {
            lblNotSaved.Visible = false;

            bool bHasRequired = SaveItem_HasRequired();
            if (!bHasRequired)
            {
                lblNotSaved.Text = "Check required fields.<br />NOT saved.";
                lblNotSaved.Visible = true;
                return false;
            }

            lblNotSaved.Text = "Error.<br />NOT saved.";

            // Don't save if an invalid quantity just entered,
            // and this button clicked.
            UpdatePrices("SaveItem");
            if (lblQuantityInvalid.Text != "" || lblKitQtyInvalid.Visible || lblTotalExtPrice.Text.ToString().Trim() == "0"
                    || lblPrimaryVoltageInvalid.Text != "" || lblSecondaryVoltageInvalid.Text != "" || lblShippingReasonInvalid.Visible == true)
            {
                lblNotSaved.Visible = true;
                return false;
            }

            int iQuoteId = 0;
            if (!string.IsNullOrEmpty(lblQuoteID.Text))
            {
                iQuoteId = Convert.ToInt32(lblQuoteID.Text);
            }

            // Save Quote information, and create a new Quote before saving the details.
            // May update hidChanged if anything has changed at the quote level.
            // ====================================
            int iQuoteIdNew = SaveQuote(iQuoteId);
            // ====================================

            int iDetailID = 0;
            if (txtDetailID.Text != null && txtDetailID.Text != "")
            {
                iDetailID = Convert.ToInt32(txtDetailID.Text);
            }

            string sCustomID = lblCustomID.Text.ToString();
            string sStockID = lblStockID.Text.ToString();

            int iCustomID = 0;
            int iStockID = 0;

            int.TryParse(sCustomID, out iCustomID);
            int.TryParse(sStockID, out iStockID);

            string sIsMatch = hidIsMatch.Value.ToString();

            int iSameAsStock = 0;
            int.TryParse(sIsMatch, out iSameAsStock);

            // Let code saving this item determine if it matches a stock item.
            string sStandardOrCustom = rblStandardOrCustom.SelectedValue.ToString();

            string sWindings = rblWindings.SelectedValue.ToString();
            string sPhase = rblPhase.SelectedValue.ToString();

            string sKVA = dv.TextOrDropDown(txtKVA.Text.ToString(), ddlKVA.SelectedValue.ToString());
            decimal dKVAEntered = 0;
            decimal.TryParse(sKVA, out dKVAEntered);
            decimal dKVAUsed = 0;
            decimal.TryParse(hidKVAUsed.Value, out dKVAUsed);

            if (dKVAEntered > 0 && dKVAUsed == 0)
                dKVAUsed = dKVAEntered;

            int iHideKVA = 0;
            if (chkHideKVA.Checked == true) iHideKVA = 1;

            string sConfiguration = ddlConfiguration.SelectedValue.ToString();
            string sPrimaryVoltageDW = rblPrimaryDW.SelectedValue.ToString();
            string sSecondaryVoltageDW = rblSecondaryDW.SelectedValue.ToString();
            string sPrimaryVoltage = "";
            string sSecondaryVoltage = "";

            bool bInternal = Convert.ToBoolean(Session["Internal"]);
            if (bInternal == true)
            {
                sPrimaryVoltage = txtPrimaryVoltage.Text.ToString();
                sSecondaryVoltage = txtSecondaryVoltage.Text.ToString();
            }
            else
            {
                sPrimaryVoltage = ddlPrimaryVoltage.SelectedValue.ToString();
                sSecondaryVoltage = ddlSecondaryVoltage.SelectedValue.ToString();
            }

            int iHideVoltPrimary = 0;
            if (chkHideVoltPrimary.Checked == true) iHideVoltPrimary = 1;

            int iHideVoltSecondary = 0;
            if (chkHideVoltSecondary.Checked == true) iHideVoltSecondary = 1;

            int iIsStepUp = Convert.ToInt32(hidIsStepUp.Value);

            string sKFactor = ddlKFactor.SelectedValue.ToString();

            string sTemperature = ddlTempRise.SelectedValue.ToString();
            string sElectrostaticShield = rblElectrostaticShield.SelectedValue.ToString();

            // Saves just one item's approval reason.
            string sItemApprovalReason = lblItemApprovalReason.Text.ToString();
            string sQuoteApprovalReason = lblApprovalReasonQuote.Text.ToString();

            bool bApprovalReqd = lblApprovalReqd.Text == "1" ? true : false;

            string sCaseColorCode = ddlCaseColor.SelectedValue.ToString();
            string sCaseColorOther = txtCaseColorOther.Text.ToString();
            txtCaseColorOther.Text = sCaseColorOther.ToUpper();
            sCaseColorOther = sCaseColorOther.ToUpper();

            lblCaseColorOtherReqd.Visible = (sCaseColorCode == "OTHER" && String.IsNullOrEmpty(sCaseColorOther) == true) ? true : false;

            // Remove other color if not saving as "Other", in case the user switched back to a normal color after entering.
            sCaseColorOther = (sCaseColorCode == "Other") ? sCaseColorOther : "";

            string sCaseSize = ddlCaseSizes.Visible ? ddlCaseSizes.SelectedValue : lblCaseSize.Text.ToString();
            string sCaseSizeCalc = String.IsNullOrEmpty(lblCaseSizeCalc.Text) ? lblCaseSize.Text.ToString() : lblCaseSizeCalc.Text.ToString();
            string sCatalogNumber = lblCatalogNo.Text.ToString() + lblCatalogNoExt.Text.ToString();

            if (ddlCaseSizes.SelectedIndex == -1)
            {
                lblCaseSizeRequired.Visible = true;
                return false;
            }
            lblCaseSizeRequired.Visible = false;

            // This loads enclosure material and TENV.
            t.Enclosure = ddlEnclosure.SelectedValue.ToString();
            if (t.Stainless == true)
                t.EnclosureMaterial = rblStainless.SelectedValue;

            string sEnclosureMtl = t.EnclosureMaterial;
            string sEnclosure = lblEnclosureData.Text;
            bool bTotallyEnclosed = t.TotallyEnclosedNonVentilated;

            string sTapsNone = hidIsTapsNone.Value;
            int iIsTapsNone = 0;
            int.TryParse(sTapsNone, out iIsTapsNone);

            string sFrequencyCode = ddlFrequency.SelectedValue.ToString();
            string sEfficiencyCode = lblEfficiencyValue.Text.ToString();

            string sEfficiencyCodeCalc = lblEfficiencyCodeCalc.Text;

            int iEfficiencySetByAdmin = 0;
            if (ddlEfficiency.Visible == true)
            {
                sEfficiencyCode = ddlEfficiency.SelectedValue;
                int.TryParse(lblEfficiencyIsSetByAdmin.Text, out iEfficiencySetByAdmin);
            }

            string sEfficiencyExemptReason = lblEfficiencyExemptReason.Text;
            if (txtExemptReason.Visible == true)
                sEfficiencyExemptReason = txtExemptReason.Text;

            int iForExport = chkForExport.Checked ? 1 : 0;
            string sMadeInUSACodes = rblMadeInUSA.SelectedValue;
            bool bMarineDuty = Convert.ToBoolean(chkMarineDuty.Checked);

            int iShipDays = (lblShipDays.Text == "") ? 0 : Convert.ToInt32(lblShipDays.Text);

            string sSoundReductCode = ddlSoundReduct.SelectedValue.ToString();
            string sSpecialFeatureCodes = CheckboxListValues(chkLstSpecialFeatures);
            string sSpecialFeatureNotes = txtSpecialFeatureNotes.Text.ToString();
            string sSpecialTypeCode = rblSpecialTypes.SelectedValue.ToString();

            string sTapsOEM = txtTapsOEM.Text.ToString();
            string sImpedanceOEM = txtImpedanceOEM.Text.ToString();

            // Prevent saving Harmonic Mitigating or Zig Zag when secondary voltage is Delta type, because it doesn't apply,
            // though it should already be set this way.
            if (sSecondaryVoltageDW == "D" && (sSpecialTypeCode == "Harmonic Mitigating" || sSpecialTypeCode == "Zig Zag"))
            {
                sSpecialTypeCode = "None";
            }

            int iTempEntered = Convert.ToInt32(ddlTempRise.SelectedValue);

            int iTempUsed = 0;
            string sTempUsed = lblTempUsed.Text;
            int.TryParse(sTempUsed, out iTempUsed);

            string sCustomerTagNo = txtCustomerTagNo.Text.ToString();
            sCustomerTagNo = String.IsNullOrEmpty(sCustomerTagNo) == true ? "" : sCustomerTagNo.ToUpper();

            string sNotesInternal = hidDetailNotesInternal.Value.ToString();

            string sCatalogNoOEM = txtCatalogNoOEM.Text.ToString();

            string sKitNumber = txtKitNumber.Text.ToString();

            string sKitWBNumber = txtWBKitNumber.Text.ToString();
            int iKitWBQuantity = 0;

            if (txtWBKitQty.Text != null && txtWBKitQty.Text != "")
            {
                iKitWBQuantity = Convert.ToInt32(txtWBKitQty.Text.ToString().Trim());
            }
            decimal decKitWBPrice = 0;
            if (txtWBKitPrice.Visible)
            {
                decKitWBPrice = Convert.ToDecimal(txtWBKitPrice.Text.ToString().Trim());
            }
            else if (lblWBKitPrice.Text != null && lblWBKitPrice.Text != "")
            {
                decKitWBPrice = Convert.ToDecimal(lblWBKitPrice.Text.ToString().Trim());
            }

            int iKitQuantity = 0;
            if (txtKitQty.Text != null && txtKitQty.Text != "")
            {
                iKitQuantity = Convert.ToInt32(txtKitQty.Text.ToString().Trim());
            }
            decimal decKitPrice = 0;
            if (txtKitPrice.Visible)
            {
                decKitPrice = Convert.ToDecimal(txtKitPrice.Text.ToString().Trim());
            }
            else if (lblKitPrice.Text != null && lblKitPrice.Text != "")
            {
                decKitPrice = Convert.ToDecimal(lblKitPrice.Text.ToString().Trim());
            }

            string sKitRBNumber = txtRBKitNumber.Text.ToString();
            int iKitRBQuantity = 0;

            if (txtRBKitQty.Text != null && txtRBKitQty.Text != "")
            {
                iKitRBQuantity = Convert.ToInt32(txtRBKitQty.Text.ToString().Trim());
            }
            decimal decKitRBPrice = 0;
            if (txtRBKitPrice.Visible)
            {
                decKitRBPrice = Convert.ToDecimal(txtRBKitPrice.Text.ToString().Trim());
            }
            else if (lblRBKitPrice.Text != null && lblRBKitPrice.Text != "")
            {
                decKitRBPrice = Convert.ToDecimal(lblRBKitPrice.Text.ToString().Trim());
            }

            string sKitLugNumber = txtLugKitNumber.Text.ToString();
            int iKitLugQuantity = 0;
            if (txtLugKitQty.Text != null && txtLugKitQty.Text != "")
            {
                iKitLugQuantity = Convert.ToInt32(txtLugKitQty.Text.ToString().Trim());
            }
            decimal decKitLugPrice = 0;

            if (txtLugKitPrice.Visible)
            {
                decKitLugPrice = Convert.ToDecimal(txtLugKitPrice.Text.ToString().Trim());
            }
            else if (lblLugKitPrice.Text != null && lblLugKitPrice.Text != "")
            {
                decKitLugPrice = Convert.ToDecimal(lblLugKitPrice.Text.ToString().Trim());
            }

            string sKitOPNumber = txtOPKitNumber.Text.ToString();
            int iKitOPQuantity = 0;
            if (txtOPKitQty.Text != null && txtOPKitQty.Text != "")
            {
                iKitOPQuantity = Convert.ToInt32(txtOPKitQty.Text.ToString().Trim());
            }
            decimal decKitOPPrice = 0;

            if (txtOPKitPrice.Visible)
            {
                decKitOPPrice = Convert.ToDecimal(txtOPKitPrice.Text.ToString().Trim());
            }
            else if (lblOPKitPrice.Text != null && lblOPKitPrice.Text != "")
            {
                decKitOPPrice = Convert.ToDecimal(lblOPKitPrice.Text.ToString().Trim());
            }

            lblAccessoryChange.Visible = false;
            int iQuantity = 0;
            if (txtQuantity.Text != null && txtQuantity.Text != "")
            {
                iQuantity = Convert.ToInt32(txtQuantity.Text.ToString().Trim());
            }
            decimal decPriceEntered = 0;

            if (txtUnitPrice.Visible && txtUnitPrice.Text != null && txtUnitPrice.Text != "")
            {
                decPriceEntered = Convert.ToDecimal(txtUnitPrice.Text.ToString().Trim());
            }
            else if (txtUnitPriceChanged.Visible && txtUnitPriceChanged.Text != null && txtUnitPriceChanged.Text != "")
            {
                decPriceEntered = Convert.ToDecimal(txtUnitPriceChanged.Text.ToString().Trim());
            }
            else if (lblUnitPrice.Text != null && lblUnitPrice.Text != "")
            {
                decPriceEntered = Convert.ToDecimal(lblUnitPrice.Text.ToString().Trim());
            }

            decimal decPriceCalced = 0;
            decimal decPriceList = 0;
            if (lblUnitPriceList.Text != null && lblUnitPriceList.Text != "")
            {
                string sPriceList = lblUnitPriceList.Text.Trim();
                decimal.TryParse(sPriceList, out decPriceList);
            }

            if (lblUnitPriceCalc.Text != null && lblUnitPriceCalc.Text != "")
            {
                string sPriceCalc = lblUnitPriceCalc.Text.Trim();
                decimal.TryParse(sPriceCalc, out decPriceCalced);
            }

            int iExpediteDays = 0;
            decimal decExpeditePrice = 0;
            if (ddlExpedite.Visible && ddlExpedite.SelectedValue != null && ddlExpedite.SelectedValue != "")
            {
                iExpediteDays = Convert.ToInt32(ddlExpedite.SelectedValue.ToString().Trim());
            }
            if (txtExpeditePrice.Visible && txtExpeditePrice.Text != null && txtExpeditePrice.Text != "")
            {
                decExpeditePrice = Convert.ToDecimal(txtExpeditePrice.Text.ToString().Trim());
            }
            else if (lblExpeditePrice.Text != null && lblExpeditePrice.Text != "")
            {
                decExpeditePrice = Convert.ToDecimal(lblExpeditePrice.Text.ToString().Trim());
            }

            string sShippingReason = txtShippingReason.Text.ToString();
            string sShippingAmt = txtShippingAmount.Text.ToString();
            string sShipWeight = hidShipWeight.Value;
            int iShipWeight = 0;
            int.TryParse(sShipWeight, out iShipWeight);
            decimal decShippingAmt = 0;
            decimal.TryParse(sShippingAmt, out decShippingAmt);

            string sUserName = Session["UserName"].ToString();

            bool bSaved = false;

            DataTable dt = q.InsertQuoteItem(iQuoteIdNew, iDetailID, sItemApprovalReason, sQuoteApprovalReason,
                                                sCaseColorCode, sCaseColorOther, sCaseSize, sCaseSizeCalc,
                                                sCatalogNumber, sCatalogNoOEM, iCustomID, sCustomerTagNo, sEfficiencyCode, sEfficiencyExemptReason,
                                                iEfficiencySetByAdmin, sElectrostaticShield,
                                                sEnclosure, sEnclosureMtl, iExpediteDays, decExpeditePrice, sFrequencyCode, sImpedanceOEM,
                                                iForExport, iHideKVA, iHideVoltPrimary, iHideVoltSecondary,
                                                iSameAsStock, iIsStepUp, iIsTapsNone, sKFactor,
                                                sKitNumber, iKitQuantity, decKitPrice, sKitRBNumber, decKitRBPrice, iKitRBQuantity,
                                                sKitLugNumber, decKitLugPrice, iKitLugQuantity,
                                                sKitOPNumber, decKitOPPrice, iKitOPQuantity,
                                                sKitWBNumber, decKitWBPrice, iKitWBQuantity, dKVAEntered, dKVAUsed,
                                                sMadeInUSACodes, bMarineDuty, sNotesInternal, sPhase,
                                                decPriceCalced, decPriceEntered, decPriceList,
                                                sPrimaryVoltageDW, sPrimaryVoltage, iQuantity, sSecondaryVoltageDW,
                                                sSecondaryVoltage, decShippingAmt, iShipDays, sShippingReason, iShipWeight,
                                                sSoundReductCode, sSpecialFeatureCodes, sSpecialFeatureNotes,
                                                sSpecialTypeCode, sStandardOrCustom, sConfiguration, iStockID,
                                                sTapsOEM, iTempEntered, iTempUsed,
                                                bTotallyEnclosed, sUserName, sWindings);

            bool bStockOnly = false;

            if (dt == null || dt.Rows.Count == 0)
            {
                lblStockQuote.Text = "";

                return false;
            }
            else
            {
                DataRow dr = dt.Rows[0];

                int iQuoteID = Convert.ToInt32(dr["QuoteID"]);

                bool bChanged = Convert.ToBoolean(dr["IsChanged"]);

                // Change who last worked on this quote.
                if (bChanged == true)
                {
                    lblFullNameLatest.Text = Session["UserName"].ToString();
                }

                lblNotSaved.Visible = false;
                bSaved = true;

                //bStockOnly = dr["IsStockOnly"].ToString() == "" ? false : Convert.ToBoolean(dr["IsStockOnly"]);

                //lblStockQuote.Text = (bStockOnly == false) ? "" : "Stock order status not reported.  Contact factory for order information.";

                LoadDefaultDetails();

                //// Reload quote items.
                //LoadQuoteItems();
            }

            if (bSaved == true)
            {
                //*******************************************************************************************
                // COPY ROW.
                //*******************************************************************************************
                DataTable dtCopy;
                DataRow dr;

                if (bCopy == true)
                {
                    // CopyQuoteItem returns the same dataset as InsertQuoteItem, but for a different record.
                    dtCopy = q.CopyQuoteItem(iQuoteIdNew, iDetailID);

                    if (dtCopy.Rows.Count == 0)
                    {
                        lblNotSaved.Visible = true;
                        return false;
                    }

                    // The following row will be for the newly copied record.
                    dr = dtCopy.Rows[0];

                    iDetailID = Convert.ToInt32(dr["DetailID"]);
                }
                else
                {
                    // The row is for the initially saved record.
                    dr = dt.Rows[0];

                    Decimal dTotalPrice = Convert.ToDecimal(dr["TotalPrice"]);
                    //Boolean bIsApprovalReqd = Convert.ToBoolean(dr["ApprovalReqd"]);
                    //String sApprovalReasonCalc = dr["ApprovalReasonCalc"].ToString();

                    //lblApprovalReqd.Text = sApprovalReasonCalc == "" ? "" : "APPROVAL REQUIRED.";

                    //lblApprovalReasonCalc.Text = String.IsNullOrEmpty(sApprovalReasonCalc) == true ? "" : sApprovalReasonCalc;

                    lblTotalPrice.Text = dTotalPrice.ToString("C2");
                }
                //*******************************************************************************************

            }

            UpdateFreightIncluded(iQuoteIdNew);

            if (iQuoteId == 0 && iQuoteIdNew > 0)
            {
                lblQuoteID.Text = iQuoteIdNew.ToString();
                Session["QuoteID"] = iQuoteIdNew;
                int iQuoteNo = Convert.ToInt32(lblQuoteNo.Text);
                string sQuoteOriginCode = lblQuoteOriginCode.Text.ToString();
                SetQuoteNoDisplay(iQuoteIdNew, iQuoteNo, 1, sQuoteOriginCode);
                iQuoteId = iQuoteIdNew;
            }

            dsQuote.DataBind();

            ResetDetail(0, false);

            EnableFinalizeButton();

            // Will be reset to False in Page_Unload();
            bButtonPress = true;

            Display(enumDisplay.All);

            // After saving the copy, now bring it up in Edit.
            if (bCopy == true)
            {
                lblItemCopied.Text = "Item copied.";
            }

            return true;
        }

        protected void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {
            txtUnitPriceChanged.Text = txtUnitPrice.Text;
            if (String.IsNullOrEmpty(txtUnitPrice.Text.ToString().Trim()) == true)
            {
                txtUnitPrice.Text = dv.NumberFormat(lblUnitPriceCalc.Text.ToString(), 9, 2);
                txtUnitPriceChanged.Text = txtUnitPrice.Text;
                lblUnitPrice.Text = txtUnitPrice.Text;
            }

            PriceChange();
        }

        protected void txtKitPrice_TextChanged(object sender, EventArgs e)
        {
            string sKitPrice = txtKitPrice.Text.ToString();
            sKitPrice = sKitPrice.Trim();
            if (sKitPrice == "")
            {
                lblKitPriceInvalid.Text = "";
                lblKitPriceInvalid.Visible = false;
                txtKitPrice.Text = lblKitPriceCalc.Text;
            }
            else
            {
                string sMsg = dv.PriceValid(sKitPrice);
                if (sMsg != "")
                {
                    lblKitPriceInvalid.Text = sMsg;
                    lblKitPriceInvalid.Visible = true;
                    txtKitPrice.Text = "";
                }
                else
                {
                    lblKitPriceInvalid.Text = "";
                    lblKitPriceInvalid.Visible = false;

                    sKitPrice = dv.NumberFormat(sKitPrice, 9, 2);
                    txtKitPrice.Text = sKitPrice;
                }
            }
            UpdatePrices("txtKitPrice");
        }

        protected void txtRBKitPrice_TextChanged(object sender, EventArgs e)
        {
            string sRBKitPrice = txtRBKitPrice.Text.ToString();
            sRBKitPrice = sRBKitPrice.Trim();
            if (sRBKitPrice == "")
            {
                lblRBKitPriceInvalid.Text = "";
                lblRBKitPriceInvalid.Visible = false;
                txtRBKitPrice.Text = lblRBKitPriceCalc.Text;
            }
            else
            {
                string sMsg = dv.PriceValid(sRBKitPrice);
                if (sMsg != "")
                {
                    lblRBKitPriceInvalid.Text = sMsg;
                    lblRBKitPriceInvalid.Visible = true;
                    txtRBKitPrice.Text = "";
                }
                else
                {
                    lblRBKitPriceInvalid.Text = "";
                    lblRBKitPriceInvalid.Visible = false;

                    sRBKitPrice = dv.NumberFormat(sRBKitPrice, 9, 2);
                    txtRBKitPrice.Text = sRBKitPrice;
                }
            }
            UpdatePrices("txtRBKitPrice");
        }

        protected void txtOPKitPrice_TextChanged(object sender, EventArgs e)
        {
            string sOPKitPrice = txtOPKitPrice.Text.ToString();
            sOPKitPrice = sOPKitPrice.Trim();
            if (sOPKitPrice == "")
            {
                lblOPKitPriceInvalid.Text = "";
                lblOPKitPriceInvalid.Visible = false;
                txtOPKitPrice.Text = lblOPKitPriceCalc.Text;
            }
            else
            {
                string sMsg = dv.PriceValid(sOPKitPrice);
                if (sMsg != "")
                {
                    lblOPKitPriceInvalid.Text = sMsg;
                    lblOPKitPriceInvalid.Visible = true;
                    txtOPKitPrice.Text = "";
                }
                else
                {
                    lblOPKitPriceInvalid.Text = "";
                    lblOPKitPriceInvalid.Visible = false;

                    sOPKitPrice = dv.NumberFormat(sOPKitPrice, 9, 2);
                    txtOPKitPrice.Text = sOPKitPrice;
                }
            }
            UpdatePrices("txtOPKitPrice");
        }

        protected void txtLugKitPrice_TextChanged(object sender, EventArgs e)
        {
            string sLugKitPrice = txtLugKitPrice.Text.ToString();
            sLugKitPrice = sLugKitPrice.Trim();
            if (sLugKitPrice == "")
            {
                lblLugKitPriceInvalid.Text = "";
                lblLugKitPriceInvalid.Visible = false;
                txtLugKitPrice.Text = lblLugKitPriceCalc.Text;
            }
            else
            {
                string sMsg = dv.PriceValid(sLugKitPrice);
                if (sMsg != "")
                {
                    lblLugKitPriceInvalid.Text = sMsg;
                    lblLugKitPriceInvalid.Visible = true;
                    txtLugKitPrice.Text = "";
                }
                else
                {
                    lblLugKitPriceInvalid.Text = "";
                    lblLugKitPriceInvalid.Visible = false;

                    sLugKitPrice = dv.NumberFormat(sLugKitPrice, 9, 2);
                    txtLugKitPrice.Text = sLugKitPrice;
                }
            }
            UpdatePrices("txtLugKitPrice");
        }

        
        
        
        
        protected void txtExpeditePrice_TextChanged(object sender, EventArgs e)
        {
            string sExpeditePrice = txtExpeditePrice.Text.ToString();
            sExpeditePrice = sExpeditePrice.Trim();
            if (sExpeditePrice == "")
            {
                lblExpeditePriceInvalid.Text = "";
                lblExpeditePriceInvalid.Visible = false;
                txtExpeditePrice.Text = lblExpeditePriceCalc.Text;
            }
            else
            {
                string sMsg = dv.PriceValid(sExpeditePrice);
                if (sMsg != "")
                {
                    lblExpeditePriceInvalid.Text = sMsg;
                    lblExpeditePriceInvalid.Visible = true;
                    txtExpeditePrice.Text = "";
                }
                else
                {
                    lblExpeditePriceInvalid.Text = "";
                    lblExpeditePriceInvalid.Visible = false;

                    sExpeditePrice = dv.NumberFormat(sExpeditePrice, 9, 2);
                    txtExpeditePrice.Text = sExpeditePrice;
                }
            }
            UpdatePrices("txtExpeditePrice");
        }

        /// <summary>
        /// Returns 1 if using Internal prices on this quote.
        /// </summary>
        /// <returns></returns>
        protected int InternalPrices()
        {
            string sOrigin = lblQuoteOriginCode.Text.ToString();
            sOrigin = String.IsNullOrEmpty(sOrigin) == true ? "" : sOrigin;

            if (sOrigin == "" || dv.StringFromText(lblQuoteID.Text) == "0")
            {
                // Both Internal reps and Internal managers / admin are considered "Internal".
                int iInternal = Convert.ToInt32(Session["Internal"]);
                return iInternal;
            }

            // OriginCode Q used for external reps.
            if (sOrigin == "Q")
                return 0;

            return 1;
        }
        /// <summary>
        /// Clear all user-entered Internal prices.  Called by RestoreEntries(), to process whether or not anything involving prices has changed.
        /// </summary>
        protected void ClearPrices()
        {
            bool bClear = false;

            // Check the entire CatalogNo.  If anything has changed, clear all manually entered prices.
            if (String.IsNullOrEmpty(hidCatalogNo.Value) == false && String.IsNullOrEmpty(lblCatalogNo.Text) == false
                && hidCatalogNo.Value != lblCatalogNo.Text) bClear = true;

            // Adding a second electrostatic shield is not reflected in the Catalog No., so it is here treated separately.
            if (String.IsNullOrEmpty(hidElectrostaticShield.Value) == false
                && hidElectrostaticShield.Value != rblElectrostaticShield.Text) bClear = true;


            // Check all the options.  If any have changed, revert to original prices.
            if (String.IsNullOrEmpty(hidFreq.Value) == false
                && hidFreq.Value != ddlFrequency.SelectedValue) bClear = true;

            if (String.IsNullOrEmpty(hidSoundLevel.Value) == false
                && hidSoundLevel.Value != ddlSoundReduct.SelectedValue) bClear = true;

            if (String.IsNullOrEmpty(hidCaseColor.Value) == false
                && hidCaseColor.Value != ddlCaseColor.SelectedValue) bClear = true;

            if (String.IsNullOrEmpty(hidMarineDuty.Value) == false
                && hidMarineDuty.Value != chkMarineDuty.Text) bClear = true;

            if (String.IsNullOrEmpty(hidMadeInUSA.Value) == false
                && hidMadeInUSA.Value != rblMadeInUSA.SelectedValue) bClear = true;

            if (String.IsNullOrEmpty(hidSpecialTypes.Value) == false
                && hidSpecialTypes.Value != rblSpecialTypes.SelectedValue) bClear = true;

            if (String.IsNullOrEmpty(hidSpecialFeatures.Value) == false
                && hidSpecialFeatures.Value != CheckboxListValues(chkLstSpecialFeatures)) bClear = true;

            if (String.IsNullOrEmpty(hidCustomerTag.Value) == false
                && String.IsNullOrEmpty(hidCustomerTag.Value) != String.IsNullOrEmpty(txtCustomerTagNo.Text.ToString())) bClear = true;

            // If anything has changed, reset custom prices to calculated price.
            if (bClear)
            {
                txtUnitPrice.Text = dv.NumberFormat(lblUnitPriceCalc.Text.ToString(), 9, 2);
                txtWBKitPrice.Text = dv.NumberFormat(lblWBKitPriceCalc.Text.ToString(), 9, 2);
                txtKitPrice.Text = dv.NumberFormat(lblKitPriceCalc.Text.ToString(), 9, 2);
                txtRBKitPrice.Text = dv.NumberFormat(lblRBKitPriceCalc.Text.ToString(), 9, 2);
                txtOPKitPrice.Text = dv.NumberFormat(lblOPKitPriceCalc.Text.ToString(), 9, 2);
                txtExpeditePrice.Text = dv.NumberFormat(lblExpeditePriceCalc.Text.ToString(), 9, 2);
            }
        }

        protected void btnWBReduceQty_Click(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = dv.ChangeTextQuantity(txtWBKitQty.Text, false, 999);
            txtWBKitQty.Text = dv.NumberFormat(sQty.ToString(), 3, 0);

            UpdatePrices("btnWBKitReduceQty");
            CatalogNumberUpdate(false);
        }

        protected void btnWBAddQty_Click(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = dv.ChangeTextQuantity(txtWBKitQty.Text, true, 999);
            txtWBKitQty.Text = dv.NumberFormat(sQty.ToString(), 3, 0);

            UpdatePrices("btnWBKitAddQty");
            CatalogNumberUpdate(false);
        }

        protected void txtWBKitQty_TextChanged(object sender, EventArgs e)
        {
            ResetKitNames();

            string sQty = txtWBKitQty.Text.ToString();
            sQty = sQty.Trim();
            if (sQty == "")
            {
                lblWBKitQtyInvalid.Text = "";
                lblWBKitQtyInvalid.Visible = false;
            }
            else
            {
                string sMsg = dv.QuantityValid(sQty);
                if (sMsg != "")
                {
                    lblWBKitQtyInvalid.Text = sMsg;
                    lblWBKitQtyInvalid.Visible = true;
                    txtWBKitQty.Text = "";
                }
                else
                {
                    lblWBKitQtyInvalid.Text = "";
                    lblWBKitQtyInvalid.Visible = false;

                    // 3 columns = 999 max
                    txtWBKitQty.Text = dv.NumberFormat(sQty, 3, 0);
                }
            }
            UpdatePrices("btnWBKitQty");
            CatalogNumberUpdate(false);
        }

        protected void txtWBKitPrice_TextChanged(object sender, EventArgs e)
        {
            string sWBKitPrice = txtWBKitPrice.Text.ToString();
            sWBKitPrice = sWBKitPrice.Trim();

            if (sWBKitPrice == "")
            {
                lblWBKitPriceInvalid.Text = "";
                lblWBKitPriceInvalid.Visible = false;
                txtWBKitPrice.Text = lblWBKitPriceCalc.Text;
            }
            else
            {
                string sMsg = dv.PriceValid(sWBKitPrice);
                if (sMsg != "")
                {
                    lblWBKitPriceInvalid.Text = sMsg;
                    lblWBKitPriceInvalid.Visible = true;
                    txtWBKitPrice.Text = "";
                }
                else
                {
                    lblWBKitPriceInvalid.Text = "";
                    lblWBKitPriceInvalid.Visible = false;

                    sWBKitPrice = dv.NumberFormat(sWBKitPrice, 9, 2);
                    txtWBKitPrice.Text = sWBKitPrice;
                }
            }
            UpdatePrices("txtWBKitPrice");
        }

        protected void lnkbHideNotes_Click(object sender, EventArgs e)
        {
            ViewShowNotesLink(false, true);
        }

        protected void lnkbShowNotes_Click(object sender, EventArgs e)
        {
            ViewShowNotesLink(true, false);
        }

        protected void ddlFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            CatalogNumberUpdate(true);
        }

        protected void ddlSoundReduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            CatalogNumberUpdate(true);
        }

        protected void ddlCaseColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetOtherColor();

            CatalogNumberUpdate(true);
        }

        protected void txtKVA_TextChanged(object sender, EventArgs e)
        {
            lblKVAInvalid.Text = "";
            lblKVAInvalid.Visible = false;
            lblGeneral.Text = "";

            // Replace this KVA with one from dropdown if available.
            string sKVA = txtKVA.Text.ToString();

            // Allow null string.
            if (sKVA.Trim() == "")
            {
                return;
            }

            string sPhase = rblPhase.SelectedValue.ToString();

            decimal dKVA;
            if (!Decimal.TryParse(sKVA, out dKVA))
            {
                lblKVAInvalid.Text = "Expect number.";
                lblKVAInvalid.Visible = true;
                lblCatalogNo.Text = "";
                pnlMoreOptions.Visible = false;
                return;
            }

            if (dKVA < 10 && sPhase == "Single")
            {
                lblKVAInvalid.Text = "KVA = 10 is minimum.";
                lblKVAInvalid.Visible = true;
                lblCatalogNo.Text = "";

                // Hide Save button.
                Display(enumDisplay.All);
                pnlMoreOptions.Visible = false;
                return;
            }

            if (dKVA < 15 && sPhase == "Three")
            {
                lblKVAInvalid.Text = "KVA = 15 is minimum.";
                lblKVAInvalid.Visible = true;
                lblCatalogNo.Text = "";

                // Hide Save button.
                Display(enumDisplay.All);
                pnlMoreOptions.Visible = false;
                return;
            }

            if (dKVA > 1000 && sPhase == "Three")
            {
                lblKVAInvalid.Text = "Contact factory for KVA > 1000.";
                lblKVAInvalid.Visible = true;
                lblCatalogNo.Text = "";

                // Hide Save button.
                Display(enumDisplay.All);
                pnlMoreOptions.Visible = false;
                return;
            }

            if (dKVA > 500 && sPhase == "Single")
            {
                lblKVAInvalid.Text = "Contact factory for single phase KVA > 500.";
                lblKVAInvalid.Visible = true;
                lblCatalogNo.Text = "";

                // Hide Save button.
                Display(enumDisplay.All);
                pnlMoreOptions.Visible = false;
                return;
            }

            bool bCustom = rblStandardOrCustom.SelectedValue == "Custom" ? true : false;
            bool bPhaseSingle = rblPhase.SelectedValue == "Single" ? true : false;
            bool bWindingsCopper = rblWindings.SelectedValue == "Copper" ? true : false;
            string sKFactor = ddlKFactor.SelectedValue;
            ChangeKVA(dKVA, bCustom, bPhaseSingle, bWindingsCopper, sKFactor);

            // Remove decimals, except for 112.5 and 37.5.
            // txtKVA.Text = dv.KVAEntry(sPhase, dKVA.ToString());
            bool bInternal = Convert.ToBoolean(Session["Internal"]);
            string sKVAEntered = dv.KVAEntry(sPhase, dKVA.ToString(), bInternal);

            // Set entered KVA to max KVA if entered > max.
            int iKVACount = ddlKVA.Items.Count;
            string sKVAMax = ddlKVA.Items[iKVACount - 1].Value;

            decimal decKVAEntered = dKVA;
            decimal decKVAMax = 0;

            bool isMaxNum = decimal.TryParse(sKVAMax, out decKVAMax);
            if (isMaxNum == true)
            {
                bool isEnteredNum = decimal.TryParse(sKVAEntered, out decKVAEntered);
                if (isEnteredNum == true)
                {
                    if (decKVAEntered > decKVAMax)
                    {
                        lblGeneral.Text = "NOTE: " + decKVAEntered.ToString() + " KVA not available for this configuration.";

                        decKVAEntered = -1;
                    }
                }
            }

            if (decKVAEntered == -1)
            {
                txtKVA.Text = "";
            }
            else {
                // Strip any zero after the KVA.
                txtKVA.Text = dv.KVAFormat(decKVAEntered.ToString());
            }

            // Default list to blank.
            // DON'T NEED TO DO THIS.  5/2/18
            //if (ddlKVA.Items.Count > 0)
            //    ddlKVA.SelectedIndex = 0;

            // DON'T NEED TO DO THIS.  5/2/18
            // RestoreEntries("KVA");

            CatalogNumberUpdate(true);

        }

        protected void chkListSpecialFeatures_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Uncheck all special features if None is currently being selected.
            ClearList(chkLstSpecialFeatures, true);

            // Turn on or off the K-20 KFactor.
            if (FindSpecialFeature("K-Factor 20 (K-20)") == true)
            {
                SetKFactor("K-20");
            }
            else
            {
                if (ddlKFactor.SelectedValue == "K-20")
                    SetKFactor("K-1 (STD)");
            }

            CatalogNumberUpdate(true);

            if (txtSpecialFeatureNotes.Visible == true)
                txtSpecialFeatureNotes.Focus();

            //// This has to come at the end, after Electrostatic Shield is re-enabled, if changed from K-20 to another value.
            //ResetElectrostaticShield();
        }

        protected void ResetElectrostaticShield()
        {
            if (ddlKFactor.SelectedValue == "K-20")
            {
                if (rblElectrostaticShield.SelectedIndex == 0)
                    rblElectrostaticShield.SelectedIndex = 1;
            }
            else
            {
                if (rblElectrostaticShield.SelectedIndex == 1 && rblElectrostaticShield.Items[0].Enabled == true)
                    rblElectrostaticShield.SelectedIndex = 0;
            }
        }

        protected void rblSpecialTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bDeltaPrimary = rblPrimaryDW.SelectedValue == "D" ? true : false;
            bool bHarmonic = false;     // Either Harmonic Mitigating or Zig Zag.
            bool bInternal = Session["Internal"].ToString() == "1" ? true : false;
            bool bSingle = rblPhase.SelectedValue == "Single" ? true : false;
            bool bWyeSecondary = rblSecondaryDW.SelectedValue == "W" ? true : false;
            bool bZigZag = rblSpecialTypes.SelectedValue == "Zig Zag";

            if (rblSpecialTypes.SelectedValue == "None")
            {
                if (rblPrimaryDW.Visible == true && rblPrimaryDW.Enabled == true)
                {
                    rblPrimaryDW.SelectedValue = "D";   // Restore Delta primary.
                    bDeltaPrimary = true;
                    bHarmonic = false;
                    LoadPrimaryVoltage(bDeltaPrimary, bHarmonic, bSingle);
                }
            }
            // Default to K-4 and Shielded for Drive Isolation.
            else if (rblSpecialTypes.SelectedValue == "Drive Isolation")
            {
                // If K-4 is not available, don't allow Drive Isolation.
                if (ddlKFactor.Items.FindByText("K-4") == null)
                {
                    rblSpecialTypes.SelectedIndex = 0;
                    return;
                }
                ddlKFactor.SelectedValue = "K-4";       // 10/9/17 - Force K-4 if DIT selected.
                rblElectrostaticShield.SelectedValue = "Shielded";
                ddlKFactor.Enabled = false;
            }
            // Default to K-1 for Harmonic Mitigating / Zig Zag.
            else if (rblSpecialTypes.SelectedValue == "Harmonic Mitigating" || rblSpecialTypes.SelectedValue == "Zig Zag")
            {
                bHarmonic = true;
                ddlKFactor.Enabled = true;
                ddlKFactor.SelectedValue = "K-1 (STD)";

                // Default to having secondary voltage as Zig Zag, unless primary voltage is already selected.
                rblSecondaryDW.Items[2].Enabled = true;

                rblSecondaryDW.SelectedValue = "Z";
            }
            else
            {
                // Restore default value if K-4 had been selected.
                if (ddlKFactor.SelectedValue == "K-4")
                {
                    ddlKFactor.SelectedIndex = 0;
                    rblElectrostaticShield.SelectedValue = "None";
                }
                ddlKFactor.Enabled = true;
            }

            if (rblSpecialTypes.SelectedValue != "Zig Zag")
            {
                chkHideVoltPrimary.Checked = false;
            }

            // Make sure Wye/Wye is selected if Auto Transformer is selected.
            if (!AutoTransformerSynch())
            {
                // Primary and secondary might have changed.  If so, don't reload with the wrong one.
                bDeltaPrimary = rblPrimaryDW.SelectedValue == "D" ? true : false;
                bWyeSecondary = rblSecondaryDW.SelectedValue == "W" ? true : false;
            }

            // if ((bHarmonic && bInternal) || !bZigZag)
            if (bHarmonic)
            {
                string sPrimaryVoltageDropdown = ddlPrimaryVoltage.SelectedValue.ToString();
                string sSecondaryVoltageDropdown = ddlSecondaryVoltage.SelectedValue.ToString();

                bool bHarmPrimary = false;
                bool bHarmSecondary = false;

                // Get the correct values in the dropdowns.
                if (rblPrimaryDW.SelectedValue == "Z")
                {
                    bHarmPrimary = true;
                }
                if (rblSecondaryDW.SelectedValue == "Z")
                {
                    bHarmSecondary = true;
                }

                LoadPrimaryVoltage(bDeltaPrimary, bHarmPrimary, bSingle);
                LoadSecondaryVoltage(bWyeSecondary, bHarmSecondary, bSingle);

                // Change the values to have ZZ instead of Y or vice versa.
                string sPrimaryVoltageText = txtPrimaryVoltage.Text.ToString();
                string sSecondaryVoltageText = txtSecondaryVoltage.Text.ToString();

                sPrimaryVoltageText = string.IsNullOrEmpty(sPrimaryVoltageText) == true ? "" : sPrimaryVoltageText;
                sPrimaryVoltageDropdown = string.IsNullOrEmpty(sPrimaryVoltageDropdown) == true ? "" : sPrimaryVoltageDropdown;
                sSecondaryVoltageText = string.IsNullOrEmpty(sSecondaryVoltageText) == true ? "" : sSecondaryVoltageText;
                sSecondaryVoltageDropdown = string.IsNullOrEmpty(sSecondaryVoltageDropdown) == true ? "" : sSecondaryVoltageDropdown;

                string sPrimaryRevisedText = dv.VoltageReplace(sPrimaryVoltageText, bHarmPrimary);
                string sPrimaryRevisedDropdown = dv.VoltageReplace(sPrimaryVoltageDropdown, bHarmPrimary);
                string sSecondaryRevisedText = dv.VoltageReplace(sSecondaryVoltageText, bHarmSecondary);
                string sSecondaryRevisedDropdown = dv.VoltageReplace(sSecondaryVoltageDropdown, bHarmSecondary);

                if (sPrimaryVoltageText != sPrimaryRevisedText) txtPrimaryVoltage.Text = sPrimaryRevisedText;
                if (sPrimaryVoltageDropdown != sPrimaryRevisedDropdown && !bHarmonic)
                {
                    ddlPrimaryVoltage.SelectedValue = sPrimaryRevisedDropdown;
                }
                else if (sPrimaryVoltageDropdown != "" && !bHarmonic)
                {
                    ddlPrimaryVoltage.SelectedValue = sPrimaryVoltageDropdown;
                }

                if (sSecondaryVoltageText != sSecondaryRevisedText) txtSecondaryVoltage.Text = sSecondaryRevisedText;
                if (sSecondaryVoltageDropdown != sSecondaryRevisedDropdown && !bHarmonic)
                {
                    ddlSecondaryVoltage.SelectedValue = sSecondaryRevisedDropdown;
                }
                else if (sSecondaryVoltageDropdown != "" && !bHarmonic)
                {
                    ddlSecondaryVoltage.SelectedValue = sSecondaryVoltageDropdown;
                }
            }

            ZigZagSync("SpecialTypes");

            CatalogNumberUpdate(true);
        }

        protected void chkMarineDuty_CheckedChanged(object sender, EventArgs e)
        {
            CatalogNumberUpdate(true);
        }

        /// <summary>
        /// Return the list of values in a checkbox list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected string CheckboxListValues(CheckBoxList list)
        {
            string sValues = "";

            // Skip i = 0 (None)
            for (int i = 1; i < list.Items.Count; i++)
            {
                if (list.Items[i].Selected)
                {
                    sValues = sValues + list.Items[i].Value + ";";
                }
            }

            return sValues;
        }

        protected void CheckboxListLoad(CheckBoxList list, string sValues)
        {
            int iPos = 0;
            string sVal = "";

            sValues = sValues.Trim();

            if (sValues == null || sValues == "")
            {
                // Set default to "None" (index == 0).
                list.Items[0].Selected = true;
                ClearList(list, false);
                return;
            }

            // Append a semicolon if there is none at the end.
            if (sValues.Substring(sValues.Length - 1, 1) != ";")
            {
                sValues = sValues + ";";
            }

            // Remove any previous selections.
            foreach (ListItem listitem in list.Items)
            {
                listitem.Selected = false;
            }

            ListItem li;

            if (sValues == null || sValues == "") return;

            while (sValues != "")
            {
                iPos = sValues.IndexOf(";");
                if (iPos > 0)
                {
                    // ex: Test;Testing;
                    //     1234567890123
                    // 1, 5 - 1 = 1, 4
                    sVal = sValues.Substring(0, iPos);

                    li = list.Items.FindByValue(sVal);
                    if (li != null && li.Enabled == true)       // Don't add back values for currently disabled items, like K-20.
                        li.Selected = true;

                    if (sValues.Length == iPos + 1)
                    {
                        sValues = "";
                        break;
                    }
                    else
                    {
                        // ex: Test;Testing;
                        //     1234567890123
                        // 5 + 1, 13 - 5  = 6, 8
                        sValues = sValues.Substring(iPos + 1, sValues.Length - iPos - 1);
                    }
                }
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            int iQuoteID = Convert.ToInt32(lblQuoteID.Text);

            SaveQuote(iQuoteID);

            string sUserNameApproval = Session["UserName"].ToString();

            string sEnvironment = dv.Environment();

            // Finalize - approved.
            if (Finalize(false) == false)
                return;

            // Sends out approval or denial email.  Approval email has PDF attached.  Denied does not.
            q.QuoteApprove(iQuoteID, 1, 0, sUserNameApproval, sEnvironment);

            LoadQuote(iQuoteID);

            lblEmailSent.Visible = true;

            // Prevent saving twice.
            bButtonPress = true;
        }

        protected void btnQuoteDetailsRpt_Click(object sender, EventArgs e)
        {
            // Run the Quote Details report.
            string sQuoteID = lblQuoteID.Text.ToString();

            string sRedirect = "~/QuoteDetailsPDF.aspx?QuoteID=" + sQuoteID;

            // open PDF.
            ResponseHelper.Redirect(sRedirect, "_blank", "");
        }

        /// <summary>
        /// Returns True if this quote item uses any special features, and not just those in the Special Features checklist.
        /// </summary>
        /// <returns></returns>
        protected bool HasSpecialFeatures()
        {
            bool bHasSF = false;

            // Frequency.
            string sFreq = ddlFrequency.SelectedValue.ToString();
            bHasSF = (sFreq == "50 Hz") ? true : false;
            // Sound reduction.
            if (!bHasSF)
            {
                string sSR = ddlSoundReduct.SelectedValue.ToString();
                bHasSF = (sSR == "0" || sSR == "" || sSR == null) ? false : true;
            }
            // Enclosure.
            if (!bHasSF)
            {
                string sStainless = ddlEnclosure.SelectedValue.ToString();
                bHasSF = (sStainless == "304 Stainless Steel" || sStainless == "316 Stainless Steel") ? true : false;
            }
            // Totally Enclosed.
            if (!bHasSF)
            {
                bHasSF = (hidTENV.Value == "1") ? true : false;
            }
            // Marine Duty.
            if (!bHasSF)
            {
                bHasSF = (chkMarineDuty.Checked) ? true : false;
            }
            // Made in USA.
            if (!bHasSF)
            {
                bHasSF = (rblMadeInUSA.SelectedValue == "None") ? false : true;
            }
            // Case Color.
            if (!bHasSF)
            {
                string sCaseColor = ddlCaseColor.SelectedValue.ToString();
                bHasSF = (sCaseColor == "ANSI 61 (STD)" || sCaseColor == "" || sCaseColor == null) ? false : true;
            }
            // Special Type.
            if (!bHasSF)
            {
                string sSpecial = rblSpecialTypes.SelectedValue.ToString();
                bHasSF = (sSpecial == "None" || sSpecial == "" || sSpecial == null) ? false : true;
            }
            // Special Features.
            if (!bHasSF)
            {
                // If no item in the check list is selected, index = -1.
                bHasSF = (chkLstSpecialFeatures.SelectedIndex == -1) ? false : true;
            }
            // Customer Tag No.
            if (!bHasSF)
            {
                // If no item in the check list is selected, index = -1.
                bHasSF = String.IsNullOrWhiteSpace(txtCustomerTagNo.Text) ? false : true;
            }

            return bHasSF;
        }

        protected void btnRequestApproval_Click(object sender, EventArgs e)
        {
            ShowRequestReason(true);
        }

        /// <summary>
        /// Show or hide request reason section.
        /// Keeping this out of the Display() procedure, since this specialized code
        /// is only used briefly before finalizing.
        /// </summary>
        /// <param name="bShow"></param>
        protected void ShowRequestReason(bool bShow)
        {
            lblbtnRequestApprovalClicked.Text = bShow ? "true" : "false";

            ViewShowNotesLink(bShow, false);

            if (bShow)
            {
                txtNotesRequest.Focus();
            }


            Display(enumDisplay.Notes);

        }

        /// <summary>
        /// Either Finalize, or save quote and request approval.
        /// </summary>
        /// <param name="bRequestApproval"></param>
        protected bool Finalize(bool bRequestApproval)
        {
            bool bSuccess = false;
            bool bOEMEmail;

            // Force a save, if the record has not been saved.
            if (btnSave.Visible == true)
            {
                if (SaveItem(false) == true)
                {
                    lblFinalized.Text = "Unsaved item was saved.  Please review quote.  Click FINALIZE again if okay.";
                    lblFinalized.Visible = true;
                }
                return false;
            }

            // This used to be initials, but now is the user name.
            string sUserName = ddlRep.SelectedValue.ToString();
            if (sUserName == null || sUserName == "")
            {
                lblFinalized.Text = "Please provide Quoted By.  Click FINALIZE again when completed.";
                lblFinalized.Visible = true;
                return false;
            }

            int iQuoteID = int.Parse(Session["QuoteID"].ToString());
            int iQuoteIDNew = 0;
            iQuoteIDNew = SaveQuote(iQuoteID);        // Don't finalize.  Request approval.

            if (!ValidQuote(iQuoteID))
                return false;

            ResetDetail(0, false);

            lblFinalized.Text = "In progress...";
            lblFinalized.Visible = true;

            // Expect new QuoteID to already be set by this point.
            if (bRequestApproval == true)
            {
                bool bOEM = chkOEM.Checked;

                bSuccess = q.CreatePDF(iQuoteIDNew, false, bOEM, true, false, out bOEMEmail);

                string sUserNameApproval = Session["UserName"].ToString();
                string sEnvironment = dv.Environment();

                q.QuoteApprove(iQuoteID, 0, 1, sUserNameApproval, sEnvironment);
            }
            else
            {
                q.QuoteFinalize(iQuoteID, Session["UserName"].ToString());
            }
            UpdateFreightIncluded(iQuoteIDNew);

            if (bRequestApproval == true)
            {
                lblStatus.Text = "PendAppr";
                lblFinalized.Text = "";
            }
            else
            {
                bool bOEM = chkOEM.Checked;
                bool bFinalized = false;
                if (bOEM == true) bFinalized = true;

                bSuccess = q.CreatePDF(iQuoteIDNew, false, bOEM, bFinalized, false, out bOEMEmail);

                if (bSuccess == true)
                {
                    // Load labels, so when the Display hides the textboxes, they will have data in them.
                    lblCompany.Text = txtCompany.Text;
                    lblProject.Text = txtProject.Text;
                    lblCity.Text = txtCity.Text;
                    lblContactName.Text = txtContactName.Text;
                    lblEmail.Text = txtEmail.Text;

                    lblStatus.Text = "Finalized";
                    lblFinalized.Text = "Finalized.";
                    DateTime dt = DateTime.Today;
                    lblFinalizedOn.Text = dt.ToString("d");
                    lblProgress.Text = "Finalized";
                }
                else
                {
                    lblFinalized.Text = "Error when finalizing.";
                }
            }

            // Change face on PDF button.
            NameShowQuote();

            Display(enumDisplay.All);

            // Show this after Diplay() hides it.
            if (bSuccess == true)
            {
                lblFinalized.Visible = true;
            }

            // Will be reset to False in Page_Unload();
            bButtonPress = true;

            return bSuccess;
        }

        protected void ApprovalDisplay(string sApprovalReason)
        {
            sApprovalReason = String.IsNullOrEmpty(sApprovalReason) == true ? "" : sApprovalReason;

            // Store the actual values.
            bool bApprovalReqd = false;
            lblApprovalReqd.Text = "0";

            if (sApprovalReason != "")
            {
                bApprovalReqd = true;
                lblApprovalReqd.Text = "1";
            }

            // Adjust for display purposes.
            if (bApprovalReqd == true)
            {
                lblApprovalReasonQuote.Text = sApprovalReason;
                lblApprovalReasonCalc.Text = "<b><font color='red'>APPROVAL REQUIRED:</font></b>&nbsp;" + sApprovalReason;
                lblApprovalReasonCalc.Visible = true;
            }
            else
            {
                lblApprovalReasonQuote.Text = "";
                lblApprovalReasonCalc.Text = "";
                lblApprovalReasonCalc.Visible = false;
            }
        }

        protected void ddlRep_SelectedIndexChanged(object sender, EventArgs e)
        {
            hidUserNameCreated.Value = ddlRep.SelectedValue.ToString();
            lblFullNameLatest.Text = ddlRep.SelectedItem.ToString();
        }

        protected void btnCopyItem_Click(object sender, EventArgs e)
        {
            SaveItem(true);
        }

        /// <summary>
        /// Manage all aspects of screen display, including hiding messages that were displayed after calling Display().
        /// e   All   (changes any displays.)
        ///     Notes (changes notes displays only.)
        ///     Price (changes price displays only.)
        /// </summary>
        protected void Display(enumDisplay e)
        {
            bool bActive = Session["Inactive"].ToString() == "0" ? true : false;
            bool bAdmin = Convert.ToBoolean(Session["Admin"]);
            bool bAdminQuote = dv.IsAdminQuote(lblQuoteNoAndVer.Text.ToString());
            bool bApprovalReqd = (lblApprovalReqd.Text.ToString() == "0" || lblApprovalReqd.Text.ToString() == "") ? false : true;
            bool bApprovalRequested = (String.IsNullOrEmpty(txtNotesRequest.Text.ToString()) == true && lblbtnRequestApprovalClicked.Text == "false") ? false : true;
            bool bApprovalPending = (lblStatus.Text == "PendAppr") ? true : false;
            bool bCaseSizeReq = ddlCaseSizes.SelectedIndex == -1 ? true : false;
            bool bCatalogValid = (String.IsNullOrEmpty(lblCatalogNo.Text) == true) ? false : true;
            bool bChanged = txtUnitPriceChanged.Visible;
            bool bEdit = lblAddEdit.Text == "Add Item to Quote" ? false : true;
            bool bEnable = (String.IsNullOrEmpty(txtCompany.Text.ToString()) == true) ? false : true;
            bool bExempt = lblEfficiencyValue.Text == "EXEMPT" || lblEfficiencyValue.Text == "TP-1" ? true : false;
            bool bExemptCalc = lblEfficiencyCodeCalc.Text == "EXEMPT" ? true : false;
            bool bD16Calc = lblEfficiencyCodeCalc.Text == "DOE2016" ? true : false;

            // Efficiency is valid if it's DOE2016, marked as DOE2016 by admin, EXEMPT, or marked as Exempt by Admin, and has a reason for it.
            // 8/28/18 DeBard - Changed to allow D0E2016 as valid efficiency.
            bool bEfficiencyValid = bD16Calc || (ddlEfficiency.SelectedValue == "DOE2016" && ddlEfficiency.Visible == true)
                        || bExemptCalc && String.IsNullOrEmpty(lblEfficiencyExemptReason.Text) == false;

            bool bEnclosureCanBeOutdoor = t.IndoorAllowed == true && t.IndoorOutdoor == "Indoor" && t.EnclosureMaterial != "Core and Coil Only";
            bool bEnclosureCanBeIndoor = t.IndoorAllowed == true && (t.IndoorOutdoor == "Indoor/Outdoor" || t.IndoorOutdoor == "Outdoor") && t.EnclosureMaterial != "Core and Coil Only";
            bool bEnclosureStainless = t.EnclosureMaterial == "304 Stainless Steel" || t.EnclosureMaterial == "316 Stainless Steel" ? true : false;
            bool bEnclosureColor = t.EnclosureMaterial == "Core and Coil Only" ? false : true;
            bool bEnclosureDualRated = t.IndoorOutdoor == "Indoor/Outdoor" && !bEnclosureCanBeIndoor && !bEnclosureCanBeOutdoor ? true : false;
            bool bExpedite = (rblStandardOrCustom.SelectedValue == "Custom") ? true : false;
            bool bExpediteSelected = ddlExpedite.SelectedIndex == 0 ? false : true;
            string sEnclosure = ddlEnclosure.SelectedValue;
            bool bExpediteLimited = sEnclosure == "304 Stainless Steel" || sEnclosure == "316 Stainless Steel" ? true : false;
            bool bFinalizeEnabled = (btnFinalize.Visible == true && btnFinalize.Enabled == true) ? true : false;
            bool bFinalized = (lblStatus.Text == "Cart" || lblStatus.Text == "PendAppr") ? false : true;
            bool bInternal = Convert.ToBoolean(Session["Internal"]);
            bool bHarmonicZigZag = rblPhase.SelectedValue == "Three" ? true : false;
            bool bHasPrice = lblSeeFactory.Visible ? false : true;
            bool bHasType = rblStandardOrCustom.SelectedIndex > -1 ? true : false;
            bool bKit = (lblKitID.Text != null && lblKitID.Text != "" && lblKitID.Text != "0");
            bool bKitRB = (lblRBKitID.Text != null && lblRBKitID.Text != "" && lblRBKitID.Text != "0");

            // NOTE: Only showing lug kit for internal and 
            // McGee (RepNo=66, RepID=18) or Griesser (RepNo = 95, ID = 23) or Mulcrone (RepNo = 39, ID = 118).
            int iRepID = Convert.ToInt32(Session["RepID"]);
            bool bKitLug = (lblLugKitID.Text != null && lblLugKitID.Text != "" && lblLugKitID.Text != "0");
            bool bKitWB = (lblWBKitID.Text != null && lblWBKitID.Text != "" && lblWBKitID.Text != "0");
            bool bLeadTimesExact = LeadTimesExact();
            bool bNew = (lblQuoteIDPrefix.Text == "New Quote") ? true : false;
            bool bNewItem = (String.IsNullOrEmpty(lblCatalogNo.Text) == true) ? true : false;
            bool bNotesInternal = (String.IsNullOrEmpty(txtNotesInternal.Text.ToString()) == true) ? false : true;
            bool bNotesRequest = (String.IsNullOrEmpty(txtNotesRequest.Text.ToString()) == true) ? false : true;
            bool bOEM = chkOEM.Checked;
            bool bPhaseSingle = rblPhase.SelectedValue == "Single" ? true : false;
            bool bSaveVisible = btnSave.Visible;
            bool bShowEfficiency = chkForExport.Checked == false && hidTENV.Value == "0" &&
                            rblSpecialTypes.SelectedValue != "Auto Transformer" && rblSpecialTypes.SelectedValue != "Drive Isolation" &&
                            rblSpecialTypes.SelectedValue != "Scott-T" &&
                            ddlFrequency.SelectedValue != "50/60 Hz" && ddlFrequency.SelectedValue != "400 Hz";
            bool bShippingAmtExists = (dv.DecimalFromText(lblShippingAmount.Text.ToString()) > 0);
            // Special features needs a note if Non-standard Taps || Special Impedance || Specific Dimensions.
            bool bSpecialFeatureNeedsNote = SpecialFeaturesNoteRequired();
            bool bSpecialType = rblSpecialTypes.SelectedValue != "None";
            bool bStandard = (rblStandardOrCustom.SelectedValue == "Standard") ? true : false;
            bool bSameAsStock = !bStandard && Convert.ToBoolean(hidIsMatch.Value == "1" ? "true" : "false");
            bool bStepUp = hidIsStepUp.Value == "1" ? true : false;
            bool bTotalExists = (String.IsNullOrEmpty(lblTotalExtPrice.Text) == true) ? false : true;
            bool bZigZag = rblSpecialTypes.SelectedValue == "Harmonic Mitigating" ||
                                     rblSpecialTypes.SelectedValue == "Zig Zag" ? true : false;

            // Build a reason for why this transformer is exempt, if it is.
            string sEfficiencyExemptReason = "";
            if (bShowEfficiency == false)
            {
                if (chkForExport.Checked == true
                || hidTENV.Value == "1"
                || rblSpecialTypes.SelectedValue == "Auto Transformer"
                || rblSpecialTypes.SelectedValue == "Drive Isolation"
                || rblSpecialTypes.SelectedValue == "Scott-T"
                || rblSpecialTypes.SelectedValue == "Zig Zag"
                || ddlFrequency.SelectedValue == "50/60 Hz"
                || ddlFrequency.SelectedValue == "400 Hz")
                {
                    // We don't need ForExport if anything else makes this exempt.
                    if (hidTENV.Value == "1"
                        || rblSpecialTypes.SelectedValue == "Auto Transformer"
                        || rblSpecialTypes.SelectedValue == "Drive Isolation"
                        || rblSpecialTypes.SelectedValue == "Scott-T"
                        || rblSpecialTypes.SelectedValue == "Zig Zag"
                        || ddlFrequency.SelectedValue == "50/60 Hz"
                        || ddlFrequency.SelectedValue == "400 Hz")
                    {
                        chkForExport.Checked = false;
                    }

                    sEfficiencyExemptReason = "EXEMPT:<br />";
                    if (chkForExport.Checked == true) sEfficiencyExemptReason = sEfficiencyExemptReason + "For Export<br />";
                    if (hidTENV.Value == "1") sEfficiencyExemptReason = sEfficiencyExemptReason + "TENV<br />";
                    if (rblSpecialTypes.SelectedValue == "Auto Transformer") sEfficiencyExemptReason = sEfficiencyExemptReason + "Auto Transformer<br />";
                    if (rblSpecialTypes.SelectedValue == "Drive Isolation") sEfficiencyExemptReason = sEfficiencyExemptReason + "Drive Isolation<br />";
                    if (rblSpecialTypes.SelectedValue == "Scott-T") sEfficiencyExemptReason = sEfficiencyExemptReason + "Scott-T<br />";
                    if (rblSpecialTypes.SelectedValue == "Zig Zag") sEfficiencyExemptReason = sEfficiencyExemptReason + "Zig Zag<br />";
                    if (ddlFrequency.SelectedValue == "50/60 Hz") sEfficiencyExemptReason = sEfficiencyExemptReason + "50/60 Hz<br />";
                    if (ddlFrequency.SelectedValue == "400 Hz") sEfficiencyExemptReason = sEfficiencyExemptReason + "400 Hz<br />";

                    sEfficiencyExemptReason = sEfficiencyExemptReason.Substring(0, sEfficiencyExemptReason.Length - 6);
                }

                lblEfficiencyExemptReason.Text = sEfficiencyExemptReason;
            }

            bool bShowOptions = (rblStandardOrCustom.SelectedValue == "Standard" && String.IsNullOrEmpty(ddlConfiguration.SelectedValue) == false)
                            || ((String.IsNullOrEmpty(txtPrimaryVoltage.Text) == false || String.IsNullOrEmpty(ddlPrimaryVoltage.SelectedValue) == false)
                                && (String.IsNullOrEmpty(txtSecondaryVoltage.Text) == false || String.IsNullOrEmpty(ddlSecondaryVoltage.SelectedValue) == false));

            if (e == enumDisplay.All)
            {

                // ====================================================================
                // Quote Panel
                // ====================================================================
                // --------------------------------------------------------------------
                // Quote Number
                // --------------------------------------------------------------------
                // lblQuoteIDPrefix             Never visible.
                // lblQuoteID                   Never visible.
                // lblQuoteNo                   For version = 1, QuoteID = QuoteNo.  Not true for versions > 1.  Never visible.
                // lblQuoteNoVer                Never visible.
                // lblQuoteOriginCode           Never visible.  Set to internal name if that rep created it.
                // lblQuoteNoAndVer             Always visible.

                lblQuoteNoAndVer.Visible = !bNew;

                // txtDetailID                  Never visible.  This is the Item #:  1, 2, 3...       
                // txtQuoteDetailsID            Never visible.  This is the unique ID of the detail.             
                // --------------------------------------------------------------------
                // Quote Status
                // --------------------------------------------------------------------
                // lblQuoteCopy                 Visible immediately after copying a quote, which sets Session["QuoteCopy"] = 1, then reloads page.
                // --------------------------------------------------------------------
                // Rep
                // --------------------------------------------------------------------
                // lblRep                       Visible when set.
                // lblRepID                     Never visible.
                // lblRepDistributor            Visible when set.
                // lblRepDistributorID          Never visible.
                // ddlRep                       Always visible.  Allows selection of rep login within quote.
                ddlRep.Visible = !bFinalized;
                // lblLatest
                // lblFullNameLatest
                // lblFullNameCreated                  Selected display name from ddlRep dropdown.  Not internal rep's name.
                lblFullNameCreated.Visible = bFinalized;
                // hidUserNameCreated.Visible = bFinalized;
                // hidUserNameCreated                  Selected user name (login name - not display name) from ddlRep dropdown.  Not internal rep's name.
                // lblCreatedPhoneTitle
                lblCreatedPhoneTitle.Visible = bInternal && String.IsNullOrEmpty(lblCreatedPhone.Text.ToString()) == false;
                // lblCreatedPhone
                lblCreatedPhone.Visible = bInternal;
                // lblRepDistributorAlt
                // --------------------------------------------------------------------
                // Company
                // --------------------------------------------------------------------
                // txtCompany
                txtCompany.Visible = !bFinalized;
                // txtCustomerContactID                 Never visible.
                // txCustomerID                         Never visible.
                // lblCompany
                lblCompany.Visible = bFinalized;
                // lblCompanyID                         Never visible.
                // lblCompanyRequired
                lblCompanyReqd.Visible = btnFinalize.Visible && String.IsNullOrEmpty(txtCompany.Text.ToString()) == true;
                // lblOrSelect
                btnFilter.Visible = !bFinalized && ddlCustomer.Items.Count > 0;
                // ddlCustomer
                ddlCustomer.Visible = !bFinalized && ddlCustomer.Items.Count > 0;
                // txtProject
                txtProject.Visible = !bFinalized;
                // lblProject
                lblProject.Visible = bFinalized;
                // txtCity
                txtCity.Visible = !bFinalized;
                // lblCity
                lblCity.Visible = bFinalized;
                // txtContactName
                txtContactName.Visible = !bFinalized;
                // lblContactName
                lblContactName.Visible = bFinalized;
                // txtEmail
                txtEmail.Visible = !bFinalized;
                // lblEmail
                lblEmail.Visible = bFinalized;
                // lblEmailInvalid
                // lblCreatedOn                         Never visisble.
                // lblCreatedLabel
                // lblCreatedOn
                lblCreatedLabel.Visible = !bFinalized;
                lblCreatedOn.Visible = !bFinalized;
                // lblFinalizedLabel
                // lblFinalizedOn
                lblFinalizedLabel.Visible = bFinalized;
                lblFinalizedOn.Visible = bFinalized;


            }
            if (e == enumDisplay.All || e == enumDisplay.Notes)
            {
                // --------------------------------------------------------------------
                // Show / Hide Notes
                // --------------------------------------------------------------------
                // lnkbShowNotes
                lnkbShowNotes.Visible = bEnable && !pnlNotes.Visible && !bApprovalReqd;
                // lnkbHideNotes
                lnkbHideNotes.Visible = bEnable && pnlNotes.Visible && !bApprovalReqd;
            }
            if (e == enumDisplay.All || e == enumDisplay.Price)
            {
                // --------------------------------------------------------------------
                // Quote Status, Price
                // --------------------------------------------------------------------
                // lblStatus
                // txtPDFUrl
                // lblShipping
                // uptblPrice - Don't show prices if approval is required, since they're not firm yet.
                pnlTotalPrice.Visible = !bNew && (bInternal || !(bApprovalReqd && !bFinalized));
                // lblTotalPriceHdr         
                // lblTotalPrice
            }
            if (e == enumDisplay.All || e == enumDisplay.Notes)
            {
                // --------------------------------------------------------------------
                // Main buttons
                // --------------------------------------------------------------------
                // btnSubmittal
                btnSubmittal.Visible = bInternal;
                // btnFinalize
                btnFinalize.Visible = !bNew && (bAdmin || (!bApprovalReqd && !bApprovalRequested && !bApprovalPending)) && !bFinalized;
                if (btnFinalize.Visible)
                {
                    EnableFinalizeButton();
                }
                // btnSubmit
                btnSubmit.Visible = !bFinalized && !bAdmin && !bApprovalPending && (bApprovalReqd || lblbtnRequestApprovalClicked.Text == "true");
                btnSubmit.Enabled = !bSaveVisible;   // Enable only when not saving.
                // btnRequestApproval
                btnRequestApproval.Visible = !bNew && !bFinalized && !bAdmin && !btnSubmit.Visible && !bApprovalPending;
                btnRequestApproval.Enabled = !bNew && !bSaveVisible;
                // btnEdit
                btnEdit.Visible = bFinalized && bAdmin;
                // lblFinalizeErrors
                // btnCopyQuote
                btnCopyQuote.Visible = bFinalized;
                // btnPDF
                btnPDF.Visible = !bNew;
                btnPDF.Enabled = !bSaveVisible;
                // btnQuoteDetailsRpt
                btnQuoteDetailsRpt.Visible = bInternal && !bNew;
                // btnEmail
                btnEmail.Visible = bFinalized;
                // lblFinalized - Set by Finalize button after calling Display.
                lblFinalized.Visible = false;

                // lblNotSaved - Set by Save button after calling display
                lblNotSaved.Visible = false;
                // lblEmailSent
                // chkSameQuoteNo
                bFinalizeEnabled = (btnFinalize.Visible == true && btnFinalize.Enabled == true) ? true : false;
                chkSameQuoteNo.Visible = bFinalized;
                chkNoDrawingsAttached.Visible = bInternal;
                chkNoDrawingsAttached.Enabled = !bFinalized;
                chkOEM.Visible = bInternal;

                // --------------------------------------------------------------------
                // Approval
                // --------------------------------------------------------------------
                // lblApprovalReqd
                // lblApprovalReqdExplanation
                lblApprovalReqdExplanation.Visible = lblApprovalReqd.Visible && !bAdmin;
                // lblApprovalReasonCalc
                lblApprovalReasonCalc.Visible = bApprovalReqd && !bFinalized;
                // lblApprovalRequested
                lblApprovalRequested.Visible = bApprovalPending;
                // btnApprove
                btnApprove.Visible = (bApprovalReqd || bApprovalPending) && !bAdminQuote && !bFinalized && bAdmin;
                // lblApprovedBy
                lblApprovedBy.Visible = bInternal && bApprovalReqd && bFinalized;
                // lblApprovedDate
                lblApprovedDate.Visible = bInternal && bApprovalReqd && bFinalized;
                // Hide Request Approval button right after approval has been requested.
                // --------------------------------------------------------------------
                // Progress
                // --------------------------------------------------------------------
                // upProgFinalize
                // upProgApprove
                // --------------------------------------------------------------------
                // Horizontal status messages
                // --------------------------------------------------------------------
                // lblSalesOrderNo
                // lblPurchaseOrderNo
                // lblShipDate
                // lblOrderDate
                // lblProNumber
                // lblItemCopied
                // --------------------------------------------------------------------
                // Notes section
                // --------------------------------------------------------------------
                // Visible when:  1) Show notes button pressed
                //                2) Request button pressed
                //                3) Approval required
                //                4) Hidden KVA or voltages, and no internal notes.

                // txtNotes       Not restricted.  Visible when panel visible.  
                // --------------------------------------------------------------------
                pnlNotes.Visible = bApprovalReqd || btnSubmit.Visible || lblbtnRequestApprovalClicked.Text == "true"
                                || lblShowNotes.Text == "true";
                // txtNotesPDF    Title is constant.  Switches between texbox and label.
                txtNotesPDF.Visible = bInternal && !bFinalized;
                // lblNotesPDF
                lblNotesPDF.Visible = !bInternal || bFinalized;

                // lblMGMNotes
                lblMGMNotes.Visible = bInternal || bNotesInternal;
                // lblOnQuote
                lblOnQuote.Visible = bInternal || bNotesInternal;

                lblApprovalRequest.Visible = btnSubmit.Visible || bNotesRequest;
                lblEmailedtoMGM.Visible = btnSubmit.Visible || bNotesRequest;

                // --------------------------------------------------------------------
                // lblInternalPrivate       Section swaps between private notes and request for approval.
                lblInternalPrivate.Visible = bInternal;
                lblPlusMinus.Visible = bInternal;
                // lblNotSeenOnPDF
                lblNotSeenOnPDF.Visible = bInternal;
                // txtNotesInternal         
                txtNotesInternal.Visible = bInternal;
                // --------------------------------------------------------------------
                // txtNotesRequest      
                txtNotesRequest.Visible = btnSubmit.Visible || bNotesRequest;
                // lblRequestReasonTitle
                // lblRequestSubtitle
                // lblNotesRequest
                // lblNotesRequestRequired
                lblNotesRequestRequired.Visible = btnSubmit.Visible && String.IsNullOrEmpty(txtNotesRequest.Text.ToString()) && !bApprovalReqd;
            }
            if (e == enumDisplay.All)
            {
                // ====================================================================
                // Add Items Panel
                // ====================================================================
                // pnlAddItem
                pnlAddItem.Visible = bEnable && !bFinalized && (!bApprovalPending || bInternal);
                // --------------------------------------------------------------------
                // Top row - Standard or custom
                // --------------------------------------------------------------------
                // rblStandardOrCustom
                rblStandardOrCustom.Enabled = bActive;
                // lblIsMatch
                lblMatch.Visible = bSameAsStock;
                // lblExcessAccessories
                // --------------------------------------------------------------------
                // Second row - Add items
                // --------------------------------------------------------------------
                uptblMainEntry.Visible = pnlAddItem.Visible && rblStandardOrCustom.SelectedIndex > -1;
                // --------------------------------------------------------------------
                // Windings
                // --------------------------------------------------------------------
                // rblWindings
                // --------------------------------------------------------------------
                // Phase
                // --------------------------------------------------------------------
                // rblPhase
                // --------------------------------------------------------------------
                // KVA
                // --------------------------------------------------------------------
                // ddlKVA
                // lblKVAOr
                lblKVAOr.Visible = !bStandard;
                // txtKVA
                txtKVA.Visible = !bStandard;
                // lblKVAInvalid
                // lblKVAUsed
                lblHideKVANewLine.Visible = bInternal & !bStandard;
                chkHideKVA.Visible = bInternal & !bStandard;
                lblHideKVA.Visible = bInternal & !bStandard;
                // --------------------------------------------------------------------
                // Voltage
                // --------------------------------------------------------------------
                // lblConfiguration
                lblConfiguration.Visible = bStandard;
                // ddlConfiguration
                ddlConfiguration.Visible = bStandard;
                // lblPrimaryVoltage
                lblPrimaryVoltage.Visible = !bStandard;
                // rblPrimaryDW
                rblPrimaryDW.Visible = !bStandard && !bPhaseSingle;
                // ddlPrimaryVoltage
                ddlPrimaryVoltage.Visible = !bStandard;
                // lblPrimaryOr
                lblPrimaryOr.Visible = bInternal && !bStandard;
                // txtPrimaryVoltage
                txtPrimaryVoltage.Visible = bInternal && !bStandard;
                // lblPrimaryVoltageInvalid
                lblHideVoltPrimaryNewLine.Visible = bInternal & !bStandard;
                chkHideVoltPrimary.Visible = bInternal & !bStandard;
                lblHideVoltPrimary.Visible = bInternal & !bStandard;
                // lblSecondaryVoltage
                lblSecondaryVoltage.Visible = !bStandard;
                // rblSecondaryDW
                rblSecondaryDW.Visible = !bStandard && !bPhaseSingle;

                // ddlSecondaryVoltage
                ddlSecondaryVoltage.Visible = !bStandard;
                // lblSecondaryOr
                lblSecondaryOr.Visible = bInternal && !bStandard;
                // txtSecondaryVoltage
                txtSecondaryVoltage.Visible = bInternal && !bStandard;
                lblHideVoltSecondaryNewLine.Visible = bInternal & !bStandard;
                chkHideVoltSecondary.Visible = bInternal && !bStandard;
                lblHideVoltSecondary.Visible = bInternal & !bStandard;

                // lblStepUp
                lblStepUp.Visible = bStepUp;
                // lblSecondaryVoltageInvalid
                // --------------------------------------------------------------------
                // K-Factor
                // --------------------------------------------------------------------
                // lblKFactor
                lblKFactor.Visible = !bStandard;
                // ddlKFactor
                ddlKFactor.Visible = !bStandard;
                if (ddlKFactor.Items.Count > 0 && rblSpecialTypes.Items.Count > 0)
                {
                    ddlKFactor.Items[0].Enabled = (rblSpecialTypes.SelectedValue == "Drive Isolation") ? false : true;
                }
                // --------------------------------------------------------------------
                // Temp Rise
                // --------------------------------------------------------------------
                // lblTempRise
                lblTempRise.Visible = !bStandard;
                // ddlTempRise
                ddlTempRise.Visible = !bStandard;
                // lblTempUsed
                // --------------------------------------------------------------------
                // Electrostatic Shield
                // --------------------------------------------------------------------
                // lblElectrostaticShield   -- Using display:none to remove its position.
                lblElectrostaticShield.Style["display"] = bStandard ? "none" : "block";

                // rblElectrostaticShield
                rblElectrostaticShield.Style["display"] = bStandard ? "none" : "block";
                // Enable No Electrostatic Shield only with K-1 (None).
                rblElectrostaticShield.Items[0].Enabled = (ddlKFactor.SelectedValue == "K-1" || ddlKFactor.SelectedValue == "None (STD)" || ddlKFactor.SelectedValue == "K-1 (STD)") ? true : false;
                // 
                // ====================================================================
                // More Options Panel
                // ====================================================================
                // pnlMoreOptions
                pnlMoreOptions.Visible = !bStandard && bEnable && !bFinalized && (!bApprovalPending || bInternal)
                    && rblStandardOrCustom.SelectedIndex > -1 && bShowOptions;     // Not available for stock.
                // --------------------------------------------------------------------
                // Frequency
                // --------------------------------------------------------------------
                // ddlFrequency
                // --------------------------------------------------------------------
                // Sound Reduction
                // --------------------------------------------------------------------
                // ddlSoundReduct
                //
                // Efficiency
                // --------------------------------------------------------------------
                // lblEfficiency
                lblEfficiencyValue.Visible = bShowEfficiency; // && (!bAdmin || bExemptCalc);
                ddlEfficiency.Visible = false; // bAdmin && !bExemptCalc;
                chkForExport.Visible = !bExempt || chkForExport.Checked;
                lblEfficiencyExemptReason.Visible = !bShowEfficiency;
                lblExemptReason.Visible = bAdmin && !bExemptCalc && bExempt;
                txtExemptReason.Visible = bAdmin && !bExemptCalc && bExempt;

                // --------------------------------------------------------------------
                // Enclosure
                // --------------------------------------------------------------------
                // ddlEnclosure
                lblRainHoodAvail.Visible = bEnclosureCanBeOutdoor;
                lblRainHoodUsed.Visible = bEnclosureCanBeIndoor;
                lblDualRated.Visible = bEnclosureDualRated;

                rblStainless.Visible = bEnclosureStainless;
                // --------------------------------------------------------------------
                // Case Color
                // --------------------------------------------------------------------
                // ddlCaseColor
                // Hide color unless HRPO or stainless.
                ddlCaseColor.Visible = bEnclosureColor;
                ANSI49.Visible = bEnclosureColor == false ? false : ANSI49.Visible;
                ANSI61.Visible = bEnclosureColor == false ? false : ANSI61.Visible;
                lblCaseColorOther.Visible = bEnclosureColor == false ? false : lblCaseColorOther.Visible;
                txtCaseColorOther.Visible = bEnclosureColor == false ? false : txtCaseColorOther.Visible;
                // upCaseImages61
                // upCaseImages49
                // txtCaseColorOther
                if (ddlCaseColor.Visible == true)
                {
                    switch (ddlCaseColor.SelectedValue)
                    {
                        case "ANSI 61 (STD)":
                            ANSI49.Visible = false;
                            ANSI61.Visible = true;
                            lblCaseColorOther.Visible = false;
                            txtCaseColorOther.Visible = false;
                            break;
                        case "ANSI 49":
                            ANSI49.Visible = true;
                            ANSI61.Visible = false;
                            lblCaseColorOther.Visible = false;
                            txtCaseColorOther.Visible = false;
                            break;
                        case "Other":
                            ANSI49.Visible = false;
                            ANSI61.Visible = false;
                            lblCaseColorOther.Visible = ddlEnclosure.SelectedIndex == 0 ? true : false;
                            txtCaseColorOther.Visible = ddlEnclosure.SelectedIndex == 0 ? true : false;
                            break;
                    }
                }

                lblCaseColorOtherReqd.Visible = (txtCaseColorOther.Visible == true && String.IsNullOrEmpty(txtCaseColorOther.Text.ToString()) == true) ? true : false;

                bool bMtlStandard;
                if (ddlEnclosure.SelectedValue == "No enclosure - Core and Coil Only")
                {
                    bMtlStandard = false;
                    // Reset color value to standard before turning off.

                    SetCaseColor("ANSI 61 (STD)");
                }
                else
                {
                    bMtlStandard = true;
                }

                lblCaseColor.Visible = bMtlStandard;
                ddlCaseColor.Visible = bMtlStandard;

                ANSI49.Visible = (bMtlStandard == true && ddlCaseColor.SelectedValue == "ANSI 49") ? true : false;
                ANSI61.Visible = (bMtlStandard == true && ddlCaseColor.SelectedValue == "ANSI 61 (STD)") ? true : false;
                lblCaseColorOther.Visible = (bMtlStandard == true && ddlCaseColor.SelectedValue == "Other") ? true : false;
                txtCaseColorOther.Visible = (bMtlStandard == true && ddlCaseColor.SelectedValue == "Other") ? true : false;

                // --------------------------------------------------------------------
                // Totally Enclosed
                // --------------------------------------------------------------------
                // hidTENV
                // --------------------------------------------------------------------
                // Marine Duty
                // --------------------------------------------------------------------
                // chkMarineDuty
                // --------------------------------------------------------------------
                // Made In USA
                // --------------------------------------------------------------------
                // rblMadeInUSA
                // --------------------------------------------------------------------
                // Special Types
                // --------------------------------------------------------------------
                // rblSpecialTypes
                rblSpecialTypes.Items.FindByValue("Harmonic Mitigating").Enabled = bHarmonicZigZag;
                rblSpecialTypes.Items.FindByValue("Zig Zag").Enabled = bHarmonicZigZag;
                // --------------------------------------------------------------------
                // Special Features
                // --------------------------------------------------------------------
                // chkSpecialFeatures
                // lblSpecialFeaturesExist
                // chkLstSpecialFeatures
                // lblSpecialFeaturesNote
                lblSpecialFeatureNotes.Visible = bSpecialFeatureNeedsNote;
                // txtSpecialFeatureNotes
                txtSpecialFeatureNotes.Visible = bSpecialFeatureNeedsNote;
                // lblSpecialFeatureNotesReqd
                lblSpecialFeatureNotesReqd.Visible = bSpecialFeatureNeedsNote && String.IsNullOrEmpty(txtSpecialFeatureNotes.Text.ToString());

                // Taps OEM
                lblTapsOEM.Visible = bOEM;
                txtTapsOEM.Visible = bOEM;

                //Impedance OEM
                lblImpedanceOEM.Visible = bOEM;
                txtImpedanceOEM.Visible = bOEM;
                // --------------------------------------------------------------------
                // Tag No
                // --------------------------------------------------------------------
                // txtCustomerTagNo
                //
            }
            if (e == enumDisplay.All || e == enumDisplay.Price)
            {
                // ====================================================================
                // Totals Panel
                // ====================================================================
                // OEM Catalog Number
                txtCatalogNoOEM.Visible = bOEM;

                // pnlTotals
                pnlTotals.Visible = bCatalogValid && !bFinalized && !bNewItem && (!bApprovalPending || bInternal);
                // --------------------------------------------------------------------
                // Enclosure info
                // --------------------------------------------------------------------
                // lblCaseSize
                lblCaseSize.Visible = !bInternal || bStandard;
                // ddlCaseSizes
                ddlCaseSizes.Visible = bInternal && !bStandard;
                // lblEnclosure
                // lblEnclosureData
                lblEnclosureData.Visible = true;    // !bStandard;
                // lblCatalogNo
                // lblCatalogNoExt
                // lblStockID
                // lblCustomID
                // --------------------------------------------------------------------
                // Quantity
                // --------------------------------------------------------------------
                // txtQuantity
                // lblQuantityReqd
                lblQuantityReqd.Visible = String.IsNullOrEmpty(txtQuantity.Text.ToString()) && !bNew;

                // lblQuantityInvalid
                lblQuantityInvalid.Visible = String.IsNullOrEmpty(lblQuantityInvalid.Text.ToString()) && !bNew;

                // --------------------------------------------------------------------
                // Price
                // --------------------------------------------------------------------
                // txtUnitPrice                 Set in DisplayPrice().
                // txtUnitPriceChanged
                // lblUnitPrice
                lblUnitPrice.Visible = !bInternal && bHasPrice && !(bApprovalReqd && !bFinalized);
                // lblUnitPriceSign
                lblUnitPriceSign.Visible = lblUnitPrice.Visible || txtUnitPrice.Visible || bChanged;
                // lblUnitPriceCalcTitle
                lblUnitPriceCalcTitle.Visible = bInternal && bChanged;
                // lblUnitPriceCalcSign
                lblUnitPriceCalcSign.Visible = bInternal && bChanged;
                // lblUnitPriceCalc
                lblUnitPriceCalc.Visible = bInternal && bChanged;
                // lblUnitPriceInvalid
                // lblItemExtPrice
                lblItemExtPrice.Visible = (bInternal || !(bApprovalReqd && !bFinalized)) && bHasPrice;
                // lblItemExtPriceSign
                lblItemExtPriceSign.Visible = bInternal || !(bApprovalReqd && !bFinalized);

                // lblTotalExtPriceSign
                lblTotalExtPriceSign.Visible = bInternal || !(bApprovalReqd && !bFinalized);
                // lblTotalExtPrice
                lblTotalExtPrice.Visible = bInternal || !(bApprovalReqd && !bFinalized);
                // --------------------------------------------------------------------
                // Kit
                // --------------------------------------------------------------------
                // WBInfo
                WBKitInfo.Visible = bKitWB;

                // imgBracketSide
                // imgBracketTop

                imgBracketBottom.Visible = bKitWB;
                imgBracketSide.Visible = bKitWB;

                // lblWBKitName
                // txtWBKitNumber
                // txtWBKitQty
                // lblWBKitQtyInvalid
                // lblWBKitQtyOrig
                // lblWBKitNameOrig
                // txtWBKitPrice
                txtWBKitPrice.Visible = bInternal;
                // lblWBKitPrice
                lblWBKitPrice.Visible = !bInternal;
                // lblWBKitPriceInvalid
                // lblWBKitExtPriceSign
                lblWBKitExtPriceSign.Visible = String.IsNullOrEmpty(lblWBKitExtPrice.Text.ToString()) == true ? false : true;
                // lblWBKitExtPrice
                // KitInfo
                KitInfo.Visible = bKit;
                // lblKitName
                // txtKitNumber
                // txtKitQty
                // lblKitQtyInvalid
                // lblKitQtyOrig
                // lblKitIDOrig
                // txtKitPrice
                txtKitPrice.Visible = bInternal && !String.IsNullOrEmpty(txtWBKitQty.Text.ToString());
                // lblKitPrice
                lblKitPrice.Visible = !bInternal;
                // lblKitPriceInvalid
                // lblKitExtPriceSign
                lblKitExtPriceSign.Visible = String.IsNullOrEmpty(lblKitExtPrice.Text.ToString()) == true ? false : true;
                // lblKitExtPrice
                // --------------------------------------------------------------------
                // RB Kit
                // --------------------------------------------------------------------
                // RBKitInfo
                RBKitInfo.Visible = bKitRB;
                // lblRBKitName
                // txtRBKitNumber
                // txtRBKitQty
                // lblRBKitQtyInvalid
                // lblRBKitQtyOrig
                // lblRBKitNameOrig
                // txtRBKitPrice
                txtKitPrice.Visible = bInternal;
                // lblRBKitPrice
                lblRBKitPrice.Visible = !bInternal;
                // lblRBKitPriceInvalid
                // lblRBKitExtPriceSign
                lblRBKitExtPriceSign.Visible = String.IsNullOrEmpty(lblRBKitExtPrice.Text.ToString()) == true ? false : true;
                // lblRBKitExtPrice
                // --------------------------------------------------------------------
                // OP Kit
                // --------------------------------------------------------------------
                // OPKitInfo
                // OPKitInfo.Visible = bKitOP;
                // lblOPKitName
                // txtOPKitNumber
                // txtOPKitQty
                // lblOPKitQtyInvalid
                // lblOPKitQtyOrig
                // lblOPKitNameOrig
                // txtOPKitPrice
                txtOPKitPrice.Visible = bInternal;
                // lblOPKitPrice
                lblOPKitPrice.Visible = !bInternal;
                // lblOPKitPriceInvalid
                // lblOPKitExtPriceSign
                lblOPKitExtPriceSign.Visible = String.IsNullOrEmpty(lblOPKitExtPrice.Text.ToString()) == true ? false : true;
                // lblOPKitExtPrice
                // --------------------------------------------------------------------
                // Lug Kit
                // --------------------------------------------------------------------
                // LugKitInfo
                LugKitInfo.Visible = bKitLug;
                // lblLugKitName
                // txtLugKitNumber
                // txtLugKitQty
                // lblLugKitQtyInvalid
                // lblLugKitQtyOrig
                // lblLugKitNameOrig
                // txtLugKitPrice
                txtLugKitPrice.Visible = bInternal;
                // lblLugKitPrice
                lblLugKitPrice.Visible = !bInternal;
                // lblLugKitPriceInvalid
                // lblLugKitExtPriceSign
                lblLugKitExtPriceSign.Visible = String.IsNullOrEmpty(lblLugKitExtPrice.Text.ToString()) == true ? false : true;
                // lblLugKitExtPrice
            
                // --------------------------------------------------------------------
                // Expedite
                // --------------------------------------------------------------------
                // ExpediteKitInfo
                ExpediteKitInfo.Visible = bExpedite && ddlExpedite.Items.Count > 0 && !bStandard;
                // ddlExpedite
                ddlExpedite.Enabled = bTotalExists && !bStandard;
                // Limited availability
                lblExpediteLimit.Visible = bExpediteLimited && ddlExpedite.Enabled;
                // ddlExpedite
                // txtExpeditePrice
                txtExpeditePrice.Visible = bInternal && bExpediteSelected;
                // lblExpeditePriceSign
                lblExpeditePriceSign.Visible = bExpediteSelected && (bInternal || (!(bApprovalReqd && !bFinalized)));
                // lblExpeditePrice
                lblExpeditePrice.Visible = !bInternal && bExpediteSelected && !(bApprovalReqd && !bFinalized);
                // lblExpeditePriceInvalid
                // lblExpediteExtPriceSign
                lblExpediteExtPriceSign.Visible = bExpediteSelected && (bInternal || !(bApprovalReqd && !bFinalized));
                lblExpediteExtPrice.Visible = bExpediteSelected && (bInternal || !(bApprovalReqd && !bFinalized));

                // --------------------------------------------------------------------
                // Shipping
                // --------------------------------------------------------------------
                // ShippingInfo
                ShippingInfo.Visible = bInternal || bShippingAmtExists;
                // txtShippingReason
                txtShippingReason.Visible = bInternal;
                // lblShippingReason
                lblShippingReason.Visible = !bInternal && bShippingAmtExists;
                // txtShippingAmount
                txtShippingAmount.Visible = bInternal;
                // lblShippingAmount
                lblShippingAmount.Visible = !bInternal && bShippingAmtExists;
                // lblShippingAmountInvalid                     Set True outside of this procedure.
                lblShippingAmountInvalid.Visible = false;
                // lblShippingReasonInvalid                     Set True outside of this procedure.
                lblShippingReasonInvalid.Visible = false;
                // lblShippingAmtExtSign
                lblShippingAmtExtSign.Visible = String.IsNullOrEmpty(lblShippingAmtExt.Text.ToString()) == true ? false : true;
                // lblShippingAmtExt

                chkNoFreeShipping.Visible = !bStandard;

                // --------------------------------------------------------------------
                // Save / Copy buttons
                // --------------------------------------------------------------------
                // btnSave
                btnSave.Visible = (lblUnitPriceInvalid.Visible == false && lblKVAInvalid.Visible == false && lblKitQtyInvalid.Visible == false
                    && lblRBKitQtyInvalid.Visible == false && lblLugKitQtyInvalid.Visible == false
                    && lblWBKitQtyInvalid.Visible == false) ? true : false;

                if (btnSave.Visible == true) btnSave.Enabled = bTotalExists;

                // btnCopyItem
                btnCopyItem.Visible = btnSave.Visible && bEdit;
                btnCopyItem.Enabled = btnSave.Enabled;

                // lblNotSaved          Set after call to Display()
                // --------------------------------------------------------------------
                // Shipping Info
                // --------------------------------------------------------------------
                string sLeadTimes;
                string sShipDays = lblShipDays.Text.ToString();
                int iShipDays = 0;
                int.TryParse(sShipDays, out iShipDays);

                if (iShipDays == 0)
                {
                    sLeadTimes = "Subject to availability";
                }
                else if (iShipDays == 10)
                {
                    sLeadTimes = "10 business days";
                }
                else if (iShipDays < 15)
                {
                    sLeadTimes = "2 to 3 weeks";
                }
                else if (iShipDays == 15 && bLeadTimesExact)
                {
                    sLeadTimes = "15 business days";
                }
                else if (iShipDays < 21)
                {
                    sLeadTimes = "3 to 4 weeks";
                }
                else if (iShipDays < 25)
                {
                    sLeadTimes = "4 to 5 weeks";
                }
                else
                {
                    sLeadTimes = "5 to 6 weeks";
                }

                lblLeadTimes.Text = sLeadTimes;

                // lblCaseSizeRequired
                lblCaseSizeRequired.Visible = bCaseSizeReq;
                // lblLeadTimesTitle
                lblLeadTimesTitle.Visible = bTotalExists;
                // lblLeadTimes
                lblLeadTimes.Visible = bTotalExists;
                // lblShipDays
                // txtShipWeeks
                // lblShipWeeks
                // lblAccessoryChange
                //
            }
            if (e == enumDisplay.All)
            {
                // ====================================================================
                // Quote Items
                // ====================================================================
                // pnlQuoteItems
                pnlQuoteItems.Visible = bEnable;
                // gvQuoteItems
                gvQuoteItems.Columns[0].Visible = bActive && !bFinalized && (!bApprovalPending || bInternal);
                gvQuoteItems.Columns[1].Visible = bActive && !bFinalized && (!bApprovalPending || bInternal);
                // ====================================================================
            }
        }

        protected void txtPrimaryVoltage_TextChanged(object sender, EventArgs e)
        {
            // Show error if value entered not numeric, D, Y, or /.
            string sPrimaryVoltage = txtPrimaryVoltage.Text.ToString();

            sPrimaryVoltage = sPrimaryVoltage.ToUpper();
            txtPrimaryVoltage.Text = sPrimaryVoltage;

            if (string.IsNullOrEmpty(sPrimaryVoltage) == false)
            {
                string sErrorMsg = dv.ValidVoltage(sPrimaryVoltage);
                lblPrimaryVoltageInvalid.Text = sErrorMsg;
            }
            else
            {
                lblPrimaryVoltageInvalid.Text = "";
            }
            bool bDelta = rblPrimaryDW.SelectedIndex == 0 ? true: false;
            bool bSingle = rblPhase.SelectedIndex == 1 ? true : false;

            sPrimaryVoltage = VoltageText(sPrimaryVoltage, bDelta, bSingle);
            txtPrimaryVoltage.Text = sPrimaryVoltage;

            CatalogNumberUpdate(true);
        }

        protected void txtSecondaryVoltage_TextChanged(object sender, EventArgs e)
        {
            // Show error if value entered not numeric, D, Y, ZZ or /.
            string sSecondaryVoltage = txtSecondaryVoltage.Text.ToString();

            bool bHarmonic = rblSpecialTypes.SelectedValue == "Harmonic Mitigating"
                          || rblSpecialTypes.SelectedValue == "Zig Zag" ? true : false;

            // Replaces Y with ZZ or vice-versa.
            sSecondaryVoltage = dv.VoltageReplace(sSecondaryVoltage, bHarmonic);

            sSecondaryVoltage = sSecondaryVoltage.ToUpper();
            txtSecondaryVoltage.Text = sSecondaryVoltage;

            if (string.IsNullOrEmpty(sSecondaryVoltage) == false)
            {
                string sErrorMsg = dv.ValidVoltage(sSecondaryVoltage);
                lblSecondaryVoltageInvalid.Text = sErrorMsg;
            }
            else
            {
                lblSecondaryVoltageInvalid.Text = "";
            }
            bool bDelta = rblSecondaryDW.SelectedIndex == 0 ? false : true;
            bool bSingle = rblPhase.SelectedIndex == 1 ? true : false;

            sSecondaryVoltage = VoltageText(sSecondaryVoltage, bDelta, bSingle);
            txtSecondaryVoltage.Text = sSecondaryVoltage;

            CatalogNumberUpdate(true);
        }

        protected void txtShippingAmount_TextChanged(object sender, EventArgs e)
        {
            string sShippingAmount = txtShippingAmount.Text.ToString();
            decimal decAmount = 0;
            decimal.TryParse(sShippingAmount, out decAmount);

            lblShippingAmtExtSign.Visible = decAmount == 0 ? false : true;
            lblShippingAmountInvalid.Visible = decAmount == 0 ? true : false;

            sShippingAmount = dv.NumberFormat(sShippingAmount, 9, 2, true);
            txtShippingAmount.Text = sShippingAmount;

            UpdatePrices("txtShippingAmount");

        }

        protected void txtShippingReason_TextChanged(object sender, EventArgs e)
        {
            string sShippingReason = txtShippingReason.Text.ToString();

            if (String.IsNullOrEmpty(sShippingReason) == true)
            {
                txtShippingAmount.Text = "";
                lblShippingAmtExt.Text = "";
                lblShippingAmtExtSign.Visible = false;
                lblShippingAmountInvalid.Text = "";
                lblShippingAmountInvalid.Visible = false;
            }

            lblShippingReasonInvalid.Visible = false;
            lblNotSaved.Visible = false;

            // Turn off warning if reason not entered.
            UpdatePrices("txtShippingAmount");
        }

        /// <summary>
        /// Verify required fields for saving an item.
        /// </summary>
        /// <returns></returns>
        protected bool SaveItem_HasRequired()
        {
            // Case Color Other.
            if (txtCaseColorOther.Visible == true)
            {
                string sCaseColorOther = txtCaseColorOther.Text.ToString();
                lblCaseColorOtherReqd.Visible = (String.IsNullOrEmpty(sCaseColorOther) == true) ? true : false;

                // Don't continue if Other Color is visible and not filled in.
                if (lblCaseColorOtherReqd.Visible == true)
                {
                    return false;
                }
            }

            // Special Feature Notes.
            if (SpecialFeaturesNotePresent() == false)
            {
                return false;
            }

            lblShippingAmountInvalid.Text = "";
            lblShippingAmountInvalid.Visible = false;

            string sShipAmt = txtShippingAmount.Text.ToString();
            string sShipReason = txtShippingReason.Text.ToString();

            sShipAmt = String.IsNullOrEmpty(sShipAmt) == true ? "" : sShipAmt;
            sShipReason = String.IsNullOrEmpty(sShipReason) == true ? "" : sShipReason;

            // Require either both shipping amount and reason are empty, or both are populated.
            if (sShipAmt != "" && sShipReason == "")
            {
                lblShippingReasonInvalid.Visible = true;
                return false;
            }
            if (sShipAmt == "" && sShipReason != "")
            {
                lblShippingAmountInvalid.Text = "Please enter shipping amount.";
                lblShippingAmountInvalid.Visible = true;
                return false;
            }

            // Anything else goes here.

            return true;
        }

        /// <summary>
        /// Uncheck it if None (first choice) is checked.
        /// </summary>
        protected void ClearList(CheckBoxList chkList, bool bFromList)
        {
            int index = 0;

            // If "None" (index == 0) is checked, uncheck all.
            if (bFromList == true)
            {
                string result = Request.Form["__EVENTTARGET"];
                string[] checkedBox = result.Split('$'); ;
                index = int.Parse(checkedBox[checkedBox.Length - 1]);
            }

            if (index == 0)
            {
                chkList.Items[0].Selected = true;

                for (int i = 1; i < chkList.Items.Count; i++)
                {
                    chkList.Items[i].Selected = false;
                }
            }
            // Otherwise, uncheck "None" (index == 0)
            else
            {
                chkList.Items[0].Selected = false;
            }
        }


        /// <summary>
        /// Request approval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string sApprovalReqd = lblApprovalReqd.Text;
            Boolean bApprovalReqd = String.IsNullOrEmpty(sApprovalReqd) == true ? false : true;

            // Only require text if approval is not already required.
            if (!bApprovalReqd)
            {
                string sRequest = txtNotesRequest.Text;
                if (String.IsNullOrEmpty(sRequest) == true)
                {
                    lblNotesRequestRequired.Visible = true;
                    return;
                }
            }

            lblNotesRequestRequired.Visible = false;

            //            ShowRequestReason(false);

            Finalize(true);

        }

        /// <summary>
        /// Cancel the request for approval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtNotesRequest.Text = null;

            CancelRequest();
        }

        /// <summary>
        /// Cancel request for approval.  Called when clicking Cancel button,
        /// or editing when request information is open.
        /// </summary>
        protected void CancelRequest()
        {
            btnFinalize.Focus();

            ShowRequestReason(false);
        }

        protected void txtUnitPriceChanged_TextChanged(object sender, EventArgs e)
        {
            txtUnitPrice.Text = txtUnitPriceChanged.Text;

            if (String.IsNullOrEmpty(txtUnitPrice.Text.ToString().Trim()) == true)
            {
                txtUnitPrice.Text = dv.NumberFormat(lblUnitPriceCalc.Text.ToString(), 9, 2);
                txtUnitPriceChanged.Text = txtUnitPrice.Text;
                lblUnitPrice.Text = txtUnitPrice.Text;
            }

            PriceChange();
        }

        /// <summary>
        /// Change a unit price for a transformer.
        /// </summary>
        protected void PriceChange()
        {
            string sUnitPrice = txtUnitPrice.Text.ToString();
            string sUnitPriceChanged = txtUnitPriceChanged.Text.ToString();

            sUnitPrice = sUnitPrice.Trim();
            sUnitPriceChanged = sUnitPriceChanged.Trim();
            if ((sUnitPrice == "" || sUnitPrice == "0") && sUnitPriceChanged != "" && sUnitPriceChanged != "0")
            {
                sUnitPrice = sUnitPriceChanged;
            }

            if (sUnitPrice == "" || sUnitPrice == "0")
            {
                lblUnitPriceInvalid.Text = "";
                lblUnitPriceInvalid.Visible = false;
                txtUnitPrice.Text = lblUnitPriceCalc.Text;
            }
            else
            {
                string sMsg = dv.PriceValid(sUnitPrice);
                if (sMsg != "")
                {
                    lblUnitPriceInvalid.Text = sMsg;
                    lblUnitPriceInvalid.Visible = true;
                    txtUnitPrice.Text = "";
                }
                else
                {
                    lblUnitPriceInvalid.Text = "";
                    lblUnitPriceInvalid.Visible = false;

                    sUnitPrice = dv.NumberFormat(sUnitPrice, 9, 2);
                    txtUnitPrice.Text = sUnitPrice;
                }
            }

            decimal decEnteredPrice = 0;
            decimal.TryParse(sUnitPrice, out decEnteredPrice);

            string sPriceCalced = lblUnitPriceCalc.Text.ToString();
            decimal decPriceCalced = 0;
            decimal.TryParse(sPriceCalced, out decPriceCalced);

            // Synchronize txtUnitPrice and txtUnitPriceChanged to have same values.
            //txtUnitPriceChanged.Text = txtUnitPrice.Text;

            // Gets error messages if amount entered is out of range.

            // Replace prices, which were overwritten here by calculated price in the last call.

            txtUnitPrice.Text = dv.NumberFormat(decEnteredPrice.ToString(), 9, 2);
            lblUnitPrice.Text = txtUnitPrice.Text;
            txtUnitPriceChanged.Text = txtUnitPrice.Text;

            UpdatePrices("Unit");
            DisplayPrice("Unit", "txtUnitPrice");

            Display(enumDisplay.Price);
        }

        /// <summary>
        /// Used to determine if we need to check for Request Notes because a Special Feature requiring such a note has been entered.
        /// </summary>
        /// <returns></returns>
        protected bool SpecialFeaturesNoteRequired()
        {
            bool bSpecialFeatureNeedsNote = false;

            for (int i = 0; i < chkLstSpecialFeatures.Items.Count; i++)
            {
                if (chkLstSpecialFeatures.Items[i].Selected == true && chkLstSpecialFeatures.Items[i].Text != "None"
                            && chkLstSpecialFeatures.Items[i].Text != "K-Factor 20 (K-20)")
                {
                    bSpecialFeatureNeedsNote = true;
                    break;
                }
            }

            return bSpecialFeatureNeedsNote;
        }

        /// <summary>
        /// Process when entering a special feature, or saving a record.
        /// </summary>
        /// <returns></returns>
        protected bool SpecialFeaturesNotePresent()
        {
            bool bSpecialFeaturesNoteRequired = SpecialFeaturesNoteRequired();
            if (bSpecialFeaturesNoteRequired)
            {
                string sSpecialFeatureNotes = txtSpecialFeatureNotes.Text.ToString();
                bool bNeedsNote = (String.IsNullOrEmpty(sSpecialFeatureNotes) == true) ? true : false;

                // Don't continue if Request Notes is not filled in.
                if (bNeedsNote == true)
                {
                    txtSpecialFeatureNotes.Focus();
                    lblSpecialFeatureNotes.Visible = true;
                    txtSpecialFeatureNotes.Visible = true;
                    lblSpecialFeatureNotesReqd.Visible = true;
                    return false;
                }
            }
            else
            {
                // If no special feature notes are required, remove any that exist.
                txtSpecialFeatureNotes.Text = "";
                lblSpecialFeatureNotes.Visible = false;
                txtSpecialFeatureNotes.Visible = false;
                lblSpecialFeatureNotesReqd.Visible = false;
            }
            return true;
        }

        protected void txtSpecialFeatureNotes_TextChanged(object sender, EventArgs e)
        {

            CatalogNumberUpdate(false);

            Display(enumDisplay.All);
        }

        protected void rblMadeInUSA_SelectedIndexChanged(object sender, EventArgs e)
        {
            CatalogNumberUpdate(true);
        }

        // If TENV is selected, don't allow Rain Hood or Rodent/Bird Shield, if they are already selected.
        protected void ClearTENVOptions()
        {
            if (hidTENV.Value == "1")
            {
                int iKitNo = 0;
                string sKitNo = txtKitNumber.Text.ToString();
                int.TryParse(sKitNo, out iKitNo);

                // Rain Hood = 3 or 4.  Doesn't apply if TENV is selected.
                if (iKitNo == 3 || iKitNo == 4)
                {
                    txtKitQty.Text = "";
                    lblKitQtyInvalid.Visible = false;
                }

                // Rodent/Bird Shield also doesn't apply if TENV is selected.
                txtRBKitQty.Text = "";
                lblRBKitQtyInvalid.Visible = false;
            }
        }

        /// <summary>
        /// Reset Efficiency if Frequency = 50 Hz or TENV is checked.
        /// </summary>
        protected void ResetOtherColor()
        {
            if (ddlEnclosure.SelectedValue != "HRPO (STD)" || ddlCaseColor.SelectedValue != "OTHER")
                txtCaseColorOther.Text = "";
        }

        /// <summary>
        /// Change the label text on the Copy Quote button.
        /// </summary>
        protected void NameReopen()
        {
            if (chkSameQuoteNo.Checked == true)
            {
                btnCopyQuote.Text = "Copy Quote-Ver#";
            }
            else
            {
                btnCopyQuote.Text = "Copy Quote-New#";
            }

        }

        /// <summary>
        /// Show or preview quote on button face.
        /// </summary>
        protected void NameShowQuote()
        {
            if (lblStatus.Text == "Cart" || lblStatus.Text == "PendAppr")
            {
                btnPDF.Text = "Preview Quote";
            }
            else
            {
                btnPDF.Text = "Show Quote";
            }
        }

        protected void chkSameQuoteNo_CheckedChanged(object sender, EventArgs e)
        {
            // Change caption on Re-Open and Copy Quotes button.
            NameReopen();
        }

        protected void txtNotesRequest_TextChanged(object sender, EventArgs e)
        {
            NotesSaved(false);
            Display(enumDisplay.Notes);
        }

        /// <summary>
        /// This is required because we don't want to always have to click twice after exiting a field and clicking a button,
        /// which the ScriptManager approach seems to require.
        /// </summary>
        protected void CleanupValidationMessages()
        {
            // If the user has entered request notes, remove the display.
            if (String.IsNullOrEmpty(txtNotesRequest.Text.ToString()) == false && lblNotesRequestRequired.Visible == true)
                lblNotesRequestRequired.Visible = false;

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            int iRepID = Convert.ToInt32(Session["RepID"]);
            String sCustomerPart = String.IsNullOrEmpty(txtCompany.Text) ? "" : txtCompany.Text.ToString();

            if (btnFilter.Text == "Clear")
            {
                sCustomerPart = "";
                btnFilter.Text = "Filter";
            }
            else
            {
                btnFilter.Text = "Clear";
            }

            LoadCustomers(iRepID, sCustomerPart);
        }

        /// <summary>
        /// Build lblCaseSizeCalcDisplay.
        /// </summary>
        protected void CaseSizeCalcDisplay()
        {
            string sSize = "";
            string sSizeCalc = String.IsNullOrEmpty(lblCaseSizeCalc.Text.ToString()) ? "" : lblCaseSizeCalc.Text.ToString();

            if (ddlCaseSizes.Visible == true)
            {
                sSize = ddlCaseSizes.SelectedValue.ToString();
            }
            else
            {
                sSize = lblCaseSize.Text.ToString();
            }

            // Update Transformer object with Case Size.
            t.CaseSize = sSize;

            // Set equal if empty.
            sSize = sSize == "" ? sSizeCalc : sSize;
            sSizeCalc = sSizeCalc == "" ? sSize : sSizeCalc;

            if (ddlCaseSizes.SelectedIndex == -1)
            {
                sSize = "";
            }

            // Display the calculated size with parentheses around it only if it differs from the one entered.
            lblCaseSizeCalcDisplay.Text = sSize == sSizeCalc ? "" : "(" + sSizeCalc + ")";

            lblCaseSizeRequired.Visible = ddlCaseSizes.SelectedIndex == -1;
        }

        /// <summary>
        /// Change the display of calculated case size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCaseSizes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CaseSizeCalcDisplay();
        }

        /// <summary>
        /// Hide copper if stock and single phase.  Called when changing to single phase.
        /// </summary>
        protected void HideCopper()
        {
            if (rblPhase.SelectedValue == "Single" && rblStandardOrCustom.SelectedValue == "Standard")
            {
                // Switch to Aluminum if Copper was selected, then hide copper.
                rblWindings.SelectedValue = "Aluminum";
                rblWindings.Items[1].Enabled = false;
            }
            else
            {
                rblWindings.Items[1].Enabled = true;
            }
        }

        /// <summary>
        /// This saves the quote.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveNotes_Click(object sender, EventArgs e)
        {
            int iQuoteId = 0;
            if (!string.IsNullOrEmpty(lblQuoteID.Text))
            {
                iQuoteId = Convert.ToInt32(lblQuoteID.Text);
            }

            // Don't save if there's no items in the quote.
            if (iQuoteId > 0)
            {
                SaveQuote(iQuoteId);
                NotesSaved(true);           // Show "Notes Saved." and increment, if clicked more than once.
            }
        }


        /// <summary>
        /// Show or Hide the "Notes saved." notification.
        /// If showing multiple times, include increment.
        /// </summary>
        protected void NotesSaved(bool bShow)
        {
            if (bShow == true)
            {
                if (lblMsgNotesSaved.Visible == false)
                {
                    lblMsgNotesSaved.Visible = true;
                }
                else
                {
                    int i = 2;
                    string sText = lblMsgNotesSaved.Text;
                    int iTo = sText.IndexOf(")");
                    if (iTo > 0)
                    {
                        // Example:  a(1).  iFrom = 3.  iTo = 3.  sNo = "3".
                        int iFrom = sText.IndexOf("(") + 1;
                        iTo = iTo - 1;
                        string sNo = sText.Substring(iFrom, iTo - iFrom + 1);
                        i = Convert.ToInt32(sNo) + 1;

                    }
                    lblMsgNotesSaved.Text = "Notes saved (" + i.ToString() + ").";
                }
            }
            else
            {
                lblMsgNotesSaved.Text = "Notes saved.";
                lblMsgNotesSaved.Visible = false;
            }
        }

        /// <summary>
        /// Return whether or not a transformer is EXEMPT from DOE2016 requirements.
        /// </summary>
        protected bool IsExempt()
        {
            if (lblEfficiencyValue.Text != "DOE2016")
            {
                return true;
            }
            return false;
        }

        protected void ddlEnclosure_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bStainless = rblStainless.Visible;

            lblErrorMessage.Text = "";

            // Change the values if necessary.
            EnclosureUpdate();

            // Change to No Paint as default if selecting a Stainless Steel value for the first time.
            if (!bStainless && rblStainless.Visible == true)
            {
                SetCaseColor("No Paint");
            }
        }

        /// <summary>
        /// Synchronize fields with selected Enclosure.
        /// Returns: True if successful, False if error message.
        /// </summary>
        protected bool EnclosureUpdate()
        {
            string sEnclosure = string.IsNullOrEmpty(ddlEnclosure.SelectedValue) ? "" : ddlEnclosure.SelectedValue;

            bool bTENV = false;
            string sCaseColor = string.IsNullOrEmpty(ddlCaseColor.SelectedValue) ? "" : ddlCaseColor.SelectedValue;
            string sEnclosureMtl = "";
            string sIndoorOutdoor = "";
            string sNEMA = "";

            switch (sEnclosure)
            {
                case "HRPO NEMA 1 Indoor":
                    sEnclosureMtl = "HRPO (STD)";
                    sIndoorOutdoor = "Indoor";
                    sNEMA = "01";
                    // Prevent saving the color as "No Paint" if not Stainless.
                    sCaseColor = sCaseColor == "No Paint" ? "" : sCaseColor;
                    break;
                case "HRPO NEMA 3R Indoor/Outdoor":
                    sEnclosureMtl = "HRPO (STD)";
                    sIndoorOutdoor = "Outdoor";
                    sNEMA = "3R";
                    // Prevent saving the color as "No Paint" if not Stainless.
                    sCaseColor = sCaseColor == "No Paint" ? "" : sCaseColor;
                    break;
                case "HRPO NEMA 4 Indoor/Outdoor TENV":
                    sEnclosureMtl = "HRPO (STD)";
                    sIndoorOutdoor = "Outdoor";
                    sNEMA = "04";
                    bTENV = true;
                    // Prevent saving the color as "No Paint" if not Stainless.
                    sCaseColor = sCaseColor == "No Paint" ? "" : sCaseColor;
                    break;
                case "Stainless NEMA 1 Indoor":
                    if (rblStainless.Visible == true)
                        sEnclosureMtl = rblStainless.SelectedValue;
                    else
                        sEnclosureMtl = "304 Stainless Steel";
                    sIndoorOutdoor = "Indoor";
                    sNEMA = "01";
                    sCaseColor = sCaseColor == "" ? "No Paint" : sCaseColor;
                    break;
                case "Stainless NEMA 3RX Indoor/Outdoor":
                    if (rblStainless.Visible == true)
                        sEnclosureMtl = rblStainless.SelectedValue;
                    else
                        sEnclosureMtl = "304 Stainless Steel";
                    sIndoorOutdoor = "Outdoor";
                    sNEMA = "3RX";
                    sCaseColor = sCaseColor == "" ? "No Paint" : sCaseColor;
                    break;
                case "Stainless NEMA 4X Indoor/Outdoor TENV":
                    if (rblStainless.Visible == true)
                        sEnclosureMtl = rblStainless.SelectedValue;
                    else
                        sEnclosureMtl = "304 Stainless Steel";
                    sIndoorOutdoor = "Outdoor";
                    sNEMA = "4X";
                    bTENV = true;
                    sCaseColor = sCaseColor == "" ? "No Paint" : sCaseColor;
                    break;
                case "No enclosure - Core and Coil Only":
                    sEnclosureMtl = "Core and Coil Only";
                    sIndoorOutdoor = "Indoor";
                    sNEMA = "CC";
                    txtKitQty.Text = "0";
                    // Prevent saving the color as "No Paint" if not Stainless.
                    sCaseColor = sCaseColor == "No Paint" ? "" : sCaseColor;
                    break;
            }

            if (bTENV == true)
                hidTENV.Value = "1";
            else
                hidTENV.Value = "0";

            t.IndoorOutdoor = sIndoorOutdoor;
            t.Enclosure = sEnclosure;

            string sPhase = rblPhase.SelectedValue;
            string sKFactor = ddlKFactor.SelectedValue;

            string sKVA = txtKVA.Text.ToString();
            if (String.IsNullOrEmpty(sKVA) == true)
                sKVA = ddlKVA.SelectedValue.ToString();

            LoadTempRise(sPhase, sKVA, sKFactor, bTENV);

            t.EnclosureMaterial = sEnclosureMtl;

            t.NEMA = sNEMA;
            t.TotallyEnclosedNonVentilated = bTENV;

            // Load data necessary for this calculation.
            t.CaseSize = lblCaseSize.Text.ToString();
            t.KFactor = ddlKFactor.SelectedValue;

            string sTempRise = ddlTempRise.SelectedValue;
            int iTempRise = 0;
            int.TryParse(sTempRise, out iTempRise);
            t.TempRise = iTempRise;

            decimal decKVA = 0;
            if (decimal.TryParse(sKVA, out decKVA) == true)
                t.KVA = decKVA;

            t.Phase = rblPhase.SelectedValue;

            int iQty = 0;
            int.TryParse(txtQuantity.Text, out iQty);

            int iKitQty = 0;
            if (int.TryParse(txtKitQty.Text.ToString(), out iKitQty) == true)
                t.KitQty = iKitQty;

            hidTENV.Value = t.TotallyEnclosedNonVentilated == true ? "1" : "0";

            // Updates kit quantity to add or remove kit based on what was entered.
            if (t.IndoorAllowed == true && t.IndoorOutdoor == "Indoor" && sIndoorOutdoor == "Outdoor")
            {
                if (t.KitQty == 0)
                {
                    t.KitQty = iQty;
                    txtKitQty.Text = dv.NumberFormat(t.KitQty.ToString(), 3, 0);
                    lblKitIDOrig.Text = lblKitID.Text;        // Set to override comparison between previous and current kits having to do with weather.
                }
            }
            else if (t.IndoorOutdoor == "Outdoor" && sIndoorOutdoor == "Indoor")
            {
                if (t.KitQty > 0)
                {
                    t.KitQty = 0;
                    txtKitQty.Text = dv.NumberFormat(t.KitQty.ToString(), 3, 0);
                }
            }
            // Remove kit if we're indoors or TENV.
            else if (sIndoorOutdoor == "Indoor" || t.TotallyEnclosedNonVentilated == true)
            {
                if (t.KitQty > 0)
                {
                    t.KitQty = 0;
                    txtKitQty.Text = dv.NumberFormat(t.KitQty.ToString(), 3, 0);
                }
            }
            // Refresh dropdown list for stainless steel.  Default to No Paint if Stainless.
            LoadCaseColor(sEnclosure);

            // If Stainless Steel, default to No Paint.
            if (sCaseColor != "")
            {
                SetCaseColor(sCaseColor);
            }

            CatalogNumberUpdate(false);
            return true;
        }

        /// <summary>
        /// Called when changing the number of kits, to update enclosure information.
        /// </summary>
        /// <returns></returns>
        protected void EnclosureKitChange(int iKitQty)
        {
            // May recalculate Indoor/Outdoor as well as Enclosure type.
            t.KitQty = iKitQty;

            // Change displayed type.
            string sEnclosure = t.Enclosure;
            if (sEnclosure != "")
            {
                SetEnclosure(sEnclosure);
                Display(enumDisplay.All);
            }
        }



        protected void chkForExport_CheckedChanged(object sender, EventArgs e)
        {
            if (hidEfficiencyExempt.Value != null)
            {
                if (!chkForExport.Checked && hidEfficiencyExempt.Value == hidEfficiencyExempt.Value)
                    ddlEfficiency.SelectedIndex = ddlEfficiency.Items.IndexOf(ddlEfficiency.Items.FindByText(hidEfficiencyExempt.Value));

                if (!chkForExport.Checked && hidEfficiencyExempt.Value != hidEfficiencyExempt.Value)
                    ddlEfficiency.SelectedIndex = ddlEfficiency.Items.IndexOf(ddlEfficiency.Items.FindByText(hidEfficiencyExempt.Value));

                ddlEfficiency_SelectedIndexChanged(ddlEfficiency, e);
            }

            CatalogNumberUpdate(true);
        }

        protected void ddlEfficiency_SelectedIndexChanged(object sender, EventArgs e)
        {
            hidEfficiencyExempt.Value = ddlEfficiency.SelectedItem.Text;
            lblEfficiencyValue.Text = ddlEfficiency.SelectedValue;

            // Remove reason, either because we're no longer saying it is exempt, 
            // or because we're going to provide a new reason.
            txtExemptReason.Text = "";
            lblEfficiencyIsSetByAdmin.Text = "1";

            CatalogNumberUpdate(true);
        }

        protected void txtExemptReason_TextChanged(object sender, EventArgs e)
        {
            lblEfficiencyExemptReason.Text = txtExemptReason.Text;
            Display(enumDisplay.All);
        }

        protected void btnDeleteItemYes_Click(object sender, EventArgs e)
        {
            int iQuoteDetailsID = Convert.ToInt32(hidDetailID.Value);
            // Can't rely on memory variable at this point.
            Session["DetailID"] = 0;
            DataRow dr = q.QuoteDetailsDelete(iQuoteDetailsID);

            // If we deleted the adjustment row, and Free Shipping was checked, uncheck it.
            bool bNoFreeShipping = Convert.ToBoolean(dr["NoFreeShipping"]);
            chkNoFreeShipping.Checked = bNoFreeShipping;

            // Refresh entire screen.
            Response.Redirect(Request.Path);

            ResetDeleteWarning();
        }

        protected void btnDeleteItemNo_Click(object sender, EventArgs e)
        {
            ResetDeleteWarning();
        }

        protected void ResetDeleteWarning()
        {
            lblDeleteItem.Visible = false;
            btnDeleteItemYes.Visible = false;
            btnDeleteItemNo.Visible = false;
        }

        // Called when changing Phase or secondary voltage.
        // Prevents having Harmonic Mitigating or Zig Zag for Single Phase or when the Secondary is Delta.
        protected void ValidateSpecialTypes()
        {
            string sSpecialType = rblSpecialTypes.SelectedValue;

            // Only concerned with these special types.
            switch (sSpecialType)
            {
                case "Harmonic Mitigating":
                case "Zig Zag":
                    break;
                default:
                    return;
            }

            // Don't allow either single phase or secondary voltage delta for harmonic mitigating / zig zag.
            if (rblPhase.SelectedValue == "Single" || rblSecondaryDW.SelectedValue == "D")
            {
                rblSpecialTypes.SelectedValue = "None";
            }
        }

        protected void chkHideKVA_CheckedChanged(object sender, EventArgs e)
        {
            CatalogNumberUpdate(false);
        }

        protected void chkHideVoltPrimary_CheckedChanged(object sender, EventArgs e)
        {
            // When checked, Efficiency is set to EXEMPT, so this will undo that.
            // If other conditions subsequently make it EXEMPT, then it will automatically reset.
            // If no other conditions make it EXEMPT, then it will stay at DOE2016.
            if (chkHideVoltPrimary.Checked == false)
            {
                SetEfficiency("DOE2016");
                // Don't allow this to be unchecked if Zig Zag is selected.
                //if (rblSpecialTypes.SelectedValue == "Zig Zag")
                //    chkHideVoltPrimary.Checked = true;
            }

            CatalogNumberUpdate(false);
        }

        protected void chkHideVoltSecondary_CheckedChanged(object sender, EventArgs e)
        {
            // When checked, Efficiency is set to EXEMPT, so this will undo that.
            // If other conditions subsequently make it EXEMPT, then it will automatically reset.
            // If no other conditions make it EXEMPT, then it will stay at DOE2016.
            if (chkHideVoltSecondary.Checked == false)
                SetEfficiency("DOE2016");

            CatalogNumberUpdate(false);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {

            string sCatalogNo = "";
            string sConfiguration = "";
            string sEfficiency = "";
            string sShield = "";
            string sFreq = "";
            string sKFactor = "";
            string sKVA = "";
            string sKVACode = "";
            string sNema = "";
            string sPhase = "";
            string sPrimary = "";
            string sPrimaryDW = "";
            string sSecondary = "";
            string sSecondaryDW = "";
            string sSoundLevel = "";
            string sTemperature = "";
            bool bTapsNonStandard = false;
            string sWinding = "";
            string sErrorMsg = "";
            bool bIsValid = false;
            int iTapsCode = -1;

            sCatalogNo = tbCatalogNo.Text;

            bButtonPress = true;
            ResetDetail(0, false);

            DataTable dt = q.GetCatalogInfo(sCatalogNo);

            lblErrorMessage.Text = "";

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sConfiguration = (dr["Configuration"] == null ? "" : dr["Configuration"].ToString());
                    sEfficiency = (dr["Efficiency"] == null ? "" : dr["Efficiency"].ToString());
                    sShield = (dr["ElectrostaticShield"] == null ? "" : dr["ElectrostaticShield"].ToString());
                    sNema = (dr["EnclosureType"] == null ? "" : dr["EnclosureType"].ToString());  // NEMA code:  e.g. 3RX.
                    sFreq = (dr["Frequency"] == null ? "" : dr["Frequency"].ToString());
                    sKFactor = (dr["KFactor"] == null ? "" : dr["KFactor"].ToString());
                    sKVA = (dr["KVA"] == null ? "" : dr["KVA"].ToString());
                    sKVACode = (dr["KVACode"] == null ? "" : dr["KVACode"].ToString());
                    sPhase = (dr["Phase"] == null ? "" : dr["Phase"].ToString());
                    sPrimary = (dr["PrimaryVoltage"] == null ? "" : dr["PrimaryVoltage"].ToString());
                    sPrimaryDW = (dr["PrimaryVoltageDW"] == null ? "" : dr["PrimaryVoltageDW"].ToString());
                    sSecondary = (dr["SecondaryVoltage"] == null ? "" : dr["SecondaryVoltage"].ToString());
                    sSecondaryDW = (dr["SecondaryVoltageDW"] == null ? "" : dr["SecondaryVoltageDW"].ToString());
                    sSoundLevel = (dr["SoundLevel"] == null ? "" : dr["SoundLevel"].ToString());
                    sTemperature = (dr["Temperature"] == null ? "" : dr["Temperature"].ToString());
                    sWinding = (dr["Winding"] == null ? "" : dr["Winding"].ToString());
                    sErrorMsg = (dr["Msg"] == null ? "" : dr["Msg"].ToString());
                    bIsValid = (Convert.ToBoolean(dr["IsValid"]));
                    bTapsNonStandard = ((dr["TapsNonStandard"].ToString() == "") == true ? false : Convert.ToBoolean(dr["TapsNonStandard"]));

                    if (Int32.TryParse(dr["TapsCode"].ToString(), out iTapsCode) == false)
                        iTapsCode = -1;

                }

                if (sShield == "") { sShield = "0"; }
                if (sShield == "SH") { sShield = "1"; }

                if (sFreq == "50") { sFreq = "50/60 Hz"; }
                if (sFreq == "60") { sFreq = "60 Hz (STD)"; }
                if (sFreq == "400") { sFreq = "400 Hz"; }

                if (sKFactor == "K-1") { sKFactor = "K-1 (STD)"; }

                sKVA = dv.KVAFormat(sKVACode);
                //if (sKVACode == "10") { sKVA = "  10  "; }
                //if (sKVACode == "15") { sKVA = "  15  "; }
                //if (sKVACode == "25") { sKVA = "  25  "; }
                //if (sKVACode == "30") { sKVA = "  30  "; }
                //if (sKVACode == "37.5") { sKVA = "  37.5"; }
                //if (sKVACode == "45") { sKVA = "  45  "; }
                //if (sKVACode == "50") { sKVA = "  50  "; }
                //if (sKVACode == "75") { sKVA = "  75  "; }
                //if (sKVACode == "100") { sKVA = "  100  "; }
                //if (sKVACode == "112.5") { sKVA = " 112.5"; }
                //if (sKVACode == "150") { sKVA = " 150  "; }
                //if (sKVACode == "167") { sKVA = " 167  "; }
                //if (sKVACode == "225") { sKVA = " 225  "; }
                //if (sKVACode == "250") { sKVA = " 250  "; }
                //if (sKVACode == "300") { sKVA = " 300  "; }
                //if (sKVACode == "333") { sKVA = " 333  "; }
                //if (sKVACode == "350") { sKVA = " 350  "; }
                //if (sKVACode == "500") { sKVA = " 500  "; }
                //if (sKVACode == "750") { sKVA = " 750  "; }
                //if (sKVACode == "1000") { sKVA = "1000  "; }

                if (sPrimaryDW == "D") { sPrimaryDW = "Delta"; } else { sPrimaryDW = "Wye"; }
                if (sSecondaryDW == "D") { sSecondaryDW = "Delta"; } else { sSecondaryDW = "Wye"; }

                if (sTemperature == "150") { sTemperature = "150 (STD)"; }

                if (!bIsValid)
                {
                    lblErrorMessage.Text = sErrorMsg;
                    tbCatalogNo.Text = tbCatalogNo.Text.ToUpper();
                    return;
                }

                if (sConfiguration == "")
                {
                    rblStandardOrCustom.SelectedIndex = rblStandardOrCustom.Items.IndexOf(rblStandardOrCustom.Items.FindByText("Custom"));
                    rblStandardOrCustom_SelectedIndexChanged(sender, e);
                }
                else
                {
                    rblStandardOrCustom.SelectedIndex = rblStandardOrCustom.Items.IndexOf(rblStandardOrCustom.Items.FindByText("Stock"));
                    rblStandardOrCustom_SelectedIndexChanged(sender, e);
                    t.Custom = rblStandardOrCustom.SelectedValue == "Standard" ? false : true;
                }

                if (rblWindings.Items.FindByText(sWinding) == null)
                {
                    rblWindings.SelectedIndex = -1;
                    lblErrorMessage.Text = sErrorMsg;
                }
                else
                {
                    rblWindings.SelectedIndex = rblWindings.Items.IndexOf(rblWindings.Items.FindByText(sWinding));
                    rblWindings_SelectedIndexChanged(sender, e);
                    t.Windings = rblWindings.SelectedValue;
                }


                if (rblPhase.Items.FindByText(sPhase) == null)
                {
                    rblPhase.SelectedIndex = -1;
                    lblErrorMessage.Text = sErrorMsg;
                }
                else
                {
                    rblPhase.SelectedIndex = rblPhase.Items.IndexOf(rblPhase.Items.FindByText(sPhase));
                    rblPhase_SelectedIndexChanged(sender, e);
                    t.Phase = rblPhase.SelectedValue;
                }


                //if (ddlKVA.Items.FindByText(sKVA) == null)
                //{
                //    ddlKVA.SelectedIndex = -1;
                //    lblErrorMessage.Text = sErrorMsg;
                //}
                //else
                //{
                    if (sConfiguration != "")
                    {
                        ddlKVA.SelectedIndex = ddlKVA.Items.IndexOf(ddlKVA.Items.FindByText(sKVA));
                        ddlKVA_SelectedIndexChanged(sender, e);
                    }

                    if (sConfiguration == "")
                    {
                        decimal dKva = 0;
                        decimal.TryParse(sKVA, out dKva);
                        if (dKva < 9)
                        {
                            lblErrorMessage.Text = "KVA's start at 9 KVA.";
                        }
                        if (dKva > 1000)
                        {
                            lblErrorMessage.Text = "Please contact factory for KVA's > 1000.";
                        }
                        
                        txtKVA.Text = sKVA;
                        txtKVA_TextChanged(sender, e);
                        t.KVA = Convert.ToDecimal(sKVA);
                    }
                //}

                if (sConfiguration != "")
                {
                    ddlConfiguration.SelectedIndex = ddlConfiguration.Items.IndexOf(ddlConfiguration.Items.FindByText(sEfficiency + ": " + sConfiguration));
                    ddlConfiguration_SelectedIndexChanged(sender, e);
                    return;
                }


                // Assert:  Custom only from here...

                if (sPhase == "Three")
                {
                    if (rblPrimaryDW.Items.FindByText(sPrimaryDW) == null)
                    {
                        rblPrimaryDW.SelectedIndex = -1;
                        lblErrorMessage.Text = sErrorMsg;
                    }
                    else
                    {
                        rblPrimaryDW.SelectedIndex = rblPrimaryDW.Items.IndexOf(rblPrimaryDW.Items.FindByText(sPrimaryDW));
                        rblPrimaryDW_SelectedIndexChanged(sender, e);
                    }
                }


                if (ddlPrimaryVoltage.Items.FindByText(sPrimary) == null)
                {
                    ddlPrimaryVoltage.SelectedIndex = -1;
                    lblErrorMessage.Text = sErrorMsg;
                }
                else
                {
                    ddlPrimaryVoltage.SelectedIndex = ddlPrimaryVoltage.Items.IndexOf(ddlPrimaryVoltage.Items.FindByText(sPrimary));
                    ddlPrimaryVoltage_SelectedIndexChanged(sender, e);
                }


                if (sPhase == "Three")
                {
                    if (rblSecondaryDW.Items.FindByText(sSecondaryDW) == null)
                    {
                        rblSecondaryDW.SelectedIndex = -1;
                        lblErrorMessage.Text = sErrorMsg;
                    }
                    else
                    {
                        rblSecondaryDW.SelectedIndex = rblSecondaryDW.Items.IndexOf(rblSecondaryDW.Items.FindByText(sSecondaryDW));
                        rblSecondaryDW_SelectedIndexChanged(sender, e);
                    }
                }

                if (ddlSecondaryVoltage.Items.FindByText(sSecondary) == null)
                {
                    ddlSecondaryVoltage.SelectedIndex = -1;
                    lblErrorMessage.Text = sErrorMsg;
                }
                else
                {
                    ddlSecondaryVoltage.SelectedIndex = ddlSecondaryVoltage.Items.IndexOf(ddlSecondaryVoltage.Items.FindByText(sSecondary));
                    ddlSecondaryVoltage_SelectedIndexChanged(sender, e);
                }


                if (ddlKFactor.Items.FindByText(sKFactor) == null)
                {
                    ddlKFactor.SelectedIndex = -1;
                    lblErrorMessage.Text = sErrorMsg;
                }
                else
                {
                    ddlKFactor.SelectedIndex = ddlKFactor.Items.IndexOf(ddlKFactor.Items.FindByText(sKFactor));
                    t.KFactor = ddlKFactor.SelectedValue;
                }


                if (ddlTempRise.Items.FindByText(sTemperature) == null)
                {
                    ddlTempRise.SelectedIndex = -1;
                    lblErrorMessage.Text = sErrorMsg;
                }
                else
                {
                    ddlTempRise.SelectedIndex = ddlTempRise.Items.IndexOf(ddlTempRise.Items.FindByText(sTemperature));
                    t.TempRise = Convert.ToInt32(ddlTempRise.SelectedValue);
                }


                if (rblElectrostaticShield.Items.FindByText(sShield) == null)
                {
                    rblElectrostaticShield.SelectedIndex = -1;
                    lblErrorMessage.Text = sErrorMsg;
                }
                else
                {
                    rblElectrostaticShield.SelectedIndex = rblElectrostaticShield.Items.IndexOf(rblElectrostaticShield.Items.FindByText(sShield));
                }

                if (ddlFrequency.Items.FindByText(sFreq) == null)
                {
                    ddlFrequency.SelectedIndex = -1;
                    lblErrorMessage.Text = sErrorMsg;
                }
                else
                {
                    ddlFrequency.SelectedIndex = ddlFrequency.Items.IndexOf(ddlFrequency.Items.FindByText(sFreq));
                }

                if (ddlSoundReduct.Items.FindByText(sSoundLevel) == null)
                {
                    ddlSoundReduct.SelectedIndex = -1;
                    lblErrorMessage.Text = sErrorMsg;
                }
                else
                {
                    ddlSoundReduct.SelectedIndex = ddlSoundReduct.Items.IndexOf(ddlSoundReduct.Items.FindByText(sSoundLevel));
                }

                if (ddlEfficiency.Items.FindByText(sEfficiency) == null)
                {
                    ddlEfficiency.SelectedIndex = -1;
                }
                else
                {
                    ddlEfficiency.SelectedIndex = ddlEfficiency.Items.IndexOf(ddlEfficiency.Items.FindByText(sEfficiency));
                }

                // This may change when going through CatalogNumberUpdate(), if no rain hood / louvers is provided.
                t.NEMA = sNema;

                if (ddlEnclosure.Items.FindByText(t.Enclosure) == null)
                {
                    ddlEnclosure.SelectedIndex = -1;
                    lblErrorMessage.Text = sErrorMsg;
                }
                else
                {
                    ddlEnclosure.SelectedIndex = ddlEnclosure.Items.IndexOf(ddlEnclosure.Items.FindByText(t.Enclosure));
                }

                if (bTapsNonStandard)
                {
                    chkLstSpecialFeatures.SelectedIndex = chkLstSpecialFeatures.Items.IndexOf(chkLstSpecialFeatures.Items.FindByText("Non-standard Taps"));
                }

            }

            if (sErrorMsg != "")
            {
                tbCatalogNo.Text = "";
            }
            else
            {
                CatalogNumberUpdate(true);
            }

            // Clear text box.
            tbCatalogNo.Text = "";

            // Verify t.NEMA against sNema.
            string sEnclosureData = lblEnclosureData.Text;

            if (sEnclosureData == "Indoor")
            {
                switch (sNema)
                {
                    case "3R":
                    case "3RX":
                    case "04":
                    case "4X":
                        txtKitQty.Text = " 1";
                        CatalogNumberUpdate(true);
                        break;
                }
             }

            switch (iTapsCode)
            {
                case 1:
                    txtSpecialFeatureNotes.Text = "No Taps";
                    break;
                case 2:
                    txtSpecialFeatureNotes.Text = "2 - 2½% FCAN and 2-2½% FCBN";
                    break;
                case 3:
                    txtSpecialFeatureNotes.Text = "2 - 2½% FCAN and 2-4½% FCBN";
                    break;
                case 4:
                    txtSpecialFeatureNotes.Text = "4-2½% FCBN";
                    break;
                case 5:
                    txtSpecialFeatureNotes.Text = "1-5% FCAN and 1-5% FCBN";
                    break;
                case 6:
                    txtSpecialFeatureNotes.Text = "2 - 5% FCBN";
                    break;
                case 7:
                    txtSpecialFeatureNotes.Text = "2 - 2½% FCAN and 1-2½% FCBN";
                    break;
                case 8:
                    txtSpecialFeatureNotes.Text = "1 - 5% FCAN and 2-5% FCBN";
                    break;
                default:
                    txtSpecialFeatureNotes.Text = "";
                    break;
            }



        }

        protected void btnClearCatalogNo_Click(object sender, EventArgs e)
        {
            tbCatalogNo.Text = "";
            lblErrorMessage.Text = "";
            bButtonPress = true;
            ResetDetail(0, false);
            Display(enumDisplay.All);
            tbCatalogNo.Focus();
        }

        protected void rblStainless_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnclosureUpdate();
        }

        protected void lnkbHideCatalogNo_Click(object sender, EventArgs e)
        {
            pnlCatalogNumber.Visible = false;
            lnkbShowCatalogNo.Visible = true;
            lnkbHideCatalogNo.Visible = false;
        }

        protected void lnkbShowCatalogNo_Click(object sender, EventArgs e)
        {
            pnlCatalogNumber.Visible = true;
            lnkbHideCatalogNo.Visible = true;
            lnkbShowCatalogNo.Visible = false;
        }

        // When adding or removing an OSHPD kit, require or remove requirement for approval.
        protected void OSHPD_Qty_Change()
        {
            CatalogNumberUpdate(false);
        }

        protected void chkOEM_CheckedChanged(object sender, EventArgs e)
        {
            Display(enumDisplay.All);
        }

        protected void chkNoDrawingsAttached_CheckedChanged(object sender, EventArgs e)
        {
            Display(enumDisplay.All);
        }

        protected void txtNotesInternal_TextChanged(object sender, EventArgs e)
        {
            Display(enumDisplay.All);
        }

        /// <summary>
        /// Duplicate the function of a Windows Forms application MessageBox.
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Message"></param>
        public static void ShowMsg(String Message)
        {
            Page pg = HttpContext.Current.Handler as Page;

            pg.ClientScript.RegisterStartupScript(
               pg.GetType(),
               "MessageBox",
               "<script language='javascript'>alert('" + Message + "');</script>"
            );
        }

        protected void btnSubmittal_Click(object sender, EventArgs e)
        {
            int iQuoteID = int.Parse(Session["QuoteID"].ToString());

            // Will be reset to False in Page_Unload();
            bButtonPress = true;

            Session["PageName"] = "Email";
            Response.Redirect("Submittal.aspx");
        }

        // Called when selecting Auto Transformer or a Delta/Wye option.
        // Returns:  True: No change necessary.
        //           False: Made a change.
        protected bool AutoTransformerSynch()
        {
            bool bOK = true;

            if (rblSpecialTypes.SelectedValue != "Auto Transformer")
                return bOK;

            if (rblPrimaryDW.SelectedValue == "D")
            {
                bOK = false;

                // Get this before reloading voltage!
                string sPrimaryVoltage = ddlPrimaryVoltage.SelectedValue;

                rblPrimaryDW.SelectedValue = "W";

                // Load Wye from dropdown.
                bool bSingle = rblPhase.SelectedValue == "Single" ? true : false;
                LoadPrimaryVoltage(false, false, bSingle);

                bool bText = false;
                sPrimaryVoltage = string.IsNullOrEmpty(sPrimaryVoltage) == true ? "" : sPrimaryVoltage;
                string sPrimaryVoltageNew = "";

                if (sPrimaryVoltage == "")
                {
                    sPrimaryVoltage = txtPrimaryVoltage.Text;
                    bText = true;
                }
                // If we have a voltage either from text or dropdown, continue.
                if (sPrimaryVoltage != "")
                {
                    sPrimaryVoltageNew = dv.VoltageConvertWye(sPrimaryVoltage);

                    if (bText == true)
                    {
                        txtPrimaryVoltage.Text = sPrimaryVoltageNew;
                    }
                    else
                    {
                        SetPrimaryVoltage(sPrimaryVoltageNew);
                    }
                }
            }
            if (rblSecondaryDW.SelectedValue == "D")
            {
                bOK = false;

                // Get this before reloading voltage!
                string sSecondaryVoltage = ddlSecondaryVoltage.SelectedValue;

                rblSecondaryDW.SelectedValue = "W";

                // Load Wye from dropdown.
                bool bSingle = rblPhase.SelectedValue == "Single" ? true : false;
                LoadSecondaryVoltage(true, false, bSingle);

                bool bText = false;
                sSecondaryVoltage = string.IsNullOrEmpty(sSecondaryVoltage) == true ? "" : sSecondaryVoltage;
                string sSecondaryVoltageNew = "";

                if (sSecondaryVoltage == "")
                {
                    sSecondaryVoltage = txtSecondaryVoltage.Text;
                    bText = true;
                }
                // If we don't have a voltage either from text or dropdown, we're done.
                if (sSecondaryVoltage == "")
                {
                    return bOK;
                }
                sSecondaryVoltageNew = dv.VoltageConvertWye(sSecondaryVoltage);

                if (bText == true)
                {
                    txtSecondaryVoltage.Text = sSecondaryVoltageNew;
                }
                else
                {
                    SetSecondaryVoltage(sSecondaryVoltageNew);
                }
            }
            return bOK;
        }

        /// <summary>
        /// Handle Zig Zag settings.
        /// Called after changing secondary D/W/Z settings for three phase, 
        /// and after changing special types.
        /// Parameters: sCallingForm in ('PrimaryDW', 'SecondaryDW', 'SpecialTypes').
        /// 
        /// Outcome: Sec Volt => Delta, Wye, turns off Zig Zag.
        /// 08/23/18 Removed logic forcing primary to be Wye if Zig Zag selected for secondary.
        /// </summary>
        /// <param name="sCallingControl"></param>
        protected void ZigZagSync(string sCallingControl)
        {
            bool bHarmMit = false;
            bool bZigZag = false;

            t.TypeSpecial = rblSpecialTypes.SelectedValue;
            bHarmMit = t.TypeSpecial == "Harmonic Mitigating" ? true : false;

            // Expect changes in one of these three controls to call this function.
            switch (sCallingControl)
            {
                case "PrimaryDW":

                    break;

                case "SecondaryDW":

                    if (rblSecondaryDW.SelectedValue == "Z") bZigZag = true;
                    break;

                case "SpecialTypes":

                    // Checks for existing Zig Zag or Harmonic Mitigating being selected.
                    bZigZag = t.ZigZag;
                    break;

                default:
                    return;
            }

            // Single phase doesn't offer Zig Zag or Harmonic Mitigating.
            t.Phase = rblPhase.SelectedValue;

            bool bCustom = (rblStandardOrCustom.SelectedValue == "Custom") ? true : false;
            bool bWindingsCopper = (rblWindings.SelectedValue == "Copper") ? true : false;
            bool bPhaseSingle = (rblPhase.SelectedValue == "Single") ? true : false;

            LoadKVA(bCustom, bPhaseSingle, bWindingsCopper, bZigZag);

            bool bZigZagSecondary = rblSecondaryDW.SelectedValue == "Z" ? true : false;

            // Only continue if there's possibly something needing adjustment for zig zag.
            if (bZigZag == false && bZigZagSecondary == false)
            {
                if (rblSpecialTypes.SelectedValue == "Zig Zag" || rblSpecialTypes.SelectedValue == "Harmonic Mitigating")
                {
                    rblSpecialTypes.SelectedValue = "None";
                    chkHideKVA.Checked = false;
                    chkHideVoltPrimary.Checked = false;
                }

                return;
            }

            // ASSERT
            // ======
            // Zig zag condition exists from some means.

            // Don't check Hide KVA or Hide Primary for Harmonic Mitigating.
            chkHideKVA.Checked = !bHarmMit;
            chkHideVoltPrimary.Checked = !bHarmMit;

            // Single phase doesn't have Delta/Wye/ZigZag, but just in case, set the values to defaults.
            if (t.Phase == "Single")
            {
                if (bZigZag == true)
                {
                    // Turn Special Types to None.
                    rblSpecialTypes.SelectedValue = "None";
                }

                if (bZigZagSecondary == true)
                {
                    // Make secondary wye.
                    rblSecondaryDW.SelectedValue = "W";
                    LoadSecondaryVoltage(false, false, false);
                }
                return;
            }

            bool bDeltaSecondary = rblSecondaryDW.SelectedValue == "D" ? true : false;

            switch (sCallingControl)
            {
                case "SecondaryDW":

                    // Turning Zig Zag on.
                    if (bZigZagSecondary == true)
                    {
                        // Change the drop down to ZigZag.
                        LoadSecondaryVoltage(false, true, false);

                        if (rblSpecialTypes.SelectedValue != "Harmonic Mitigating" && rblSpecialTypes.SelectedValue != "Zig Zag")
                        {
                            // Turn Special Types to Zig Zag if not already Zig Zag or Harmonic Mitigating.
                            rblSpecialTypes.SelectedValue = "Zig Zag";
                        }
                    }
                    // Turning Zig Zag off.
                    else if (bDeltaSecondary == true)
                    {
                        // Change the drop down to Delta.
                        LoadSecondaryVoltage(false, false, false);

                        if (rblSpecialTypes.SelectedValue == "Harmonic Mitigating" || rblSpecialTypes.SelectedValue == "Zig Zag")
                        {
                            // Turn Special Types to None.
                            rblSpecialTypes.SelectedValue = "None";
                        }
                    }
                    // Selecting Wye. Switches Zig Zag to Secondary.
                    else
                    {
                        // Change the drop down to Wye.
                        LoadSecondaryVoltage(true, false, false);

                    }
                    break;

                case "SpecialTypes":

                    if (bZigZag == true)
                    {
                        // 08/23/18 DeBard - Removed logic which changed primary to Wye.

                        // Switch secondary to Zig Zag.
                        rblSecondaryDW.SelectedValue = "Z";
                        LoadSecondaryVoltage(false, true, false);
                    }
                    else
                    {
                        // Switch secondary to wye if Zig Zag.
                        if (bZigZagSecondary == true)
                        {
                            rblSecondaryDW.SelectedValue = "W";

                            chkHideKVA.Checked = false;
                            chkHideVoltPrimary.Checked = false;

                            LoadSecondaryVoltage(true, false, false);
                        }
                    }
                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// If KVA is between 150 and 500, lead times might be expressed exactly (i.e. 15 days),
        /// provided the ship days calculates out to 15 (i.e. no adders).
        /// </summary>
        /// <returns></returns>
        protected bool LeadTimesExact()
        {
            string sKVA = txtKVA.Text;
            sKVA = string.IsNullOrEmpty(sKVA) ? "" : sKVA;
            if (sKVA == "" && ddlKVA.SelectedIndex > -1)
            {
                sKVA = ddlKVA.SelectedValue;
            }
            sKVA = string.IsNullOrEmpty(sKVA) ? "" : sKVA;
            int iKVA = 0;
            int.TryParse(sKVA, out iKVA);

            if (iKVA >= 151 && iKVA <= 500)
            {
                return true;
            }
            return false;
        }

        protected bool UseWiringDiagram()
        {
            // IEM only.
            if (Convert.ToInt32(Session["RepID"]) == 123)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Leaves valid voltages.  Doctors invalid ones.
        /// </summary>
        /// <param name="sVoltageIn"></param>
        /// <param name="bDelta"></param>
        /// <returns></returns>
        protected string VoltageText(string sVoltageIn, bool bDelta, bool bSinglePhase)
        {
            sVoltageIn = sVoltageIn == null ? "" : sVoltageIn;

            // Don't add Delta or Wye for single phase.
            if (bSinglePhase == true)
            {
                return sVoltageIn;
            }

            // If Voltage is already properly formatted, don't continue.
            if (sVoltageIn.IndexOf("/") > 0 || sVoltageIn.IndexOf(@"\") > 0
                || sVoltageIn.IndexOf(" D") > 0)
            {
                return sVoltageIn;
            }
            if (sVoltageIn.IndexOf(@"Y/") > 0)
            {
                if (sVoltageIn.Length > sVoltageIn.IndexOf(@"Y/") + 2)
                {
                    return sVoltageIn;
                }
            }

            // Assert:  Voltage is a single number.

            // Delta.
            if (bDelta == true)
            {
                return sVoltageIn + " D";
            }
            
            // Assert:  Wye.
            int iVoltage = 0;
            int.TryParse(sVoltageIn, out iVoltage);

            if (iVoltage == 0)
            {
                return sVoltageIn;
            }
            
            int iDenom = (int)(Math.Round(iVoltage / 1.732));
            
            return sVoltageIn + @"Y/" + iDenom.ToString();
        }
    }
}