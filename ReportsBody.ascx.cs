using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Web.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using AgentDashboard;
using M1Report;
using InventoryReportLibrary;

namespace MGM_Transformer
{
    public partial class ReportsBody : System.Web.UI.UserControl
    {
        Quotes q = new Quotes();
        DataValidation dv = new DataValidation();
       // DistributorSalesReport _dsr;

        List<string> lstYears;
        private int _iPlusYears;
        private int _iMinusYears;
        private int _iStartYear;
        private int _iCurrentYear;
        private int _iEndYear;

        void Session_Start(object sender, EventArgs e)
        {
            Response.Redirect("http://www.mgmtransformer.com");

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["IsLoggedIn"]) == 0)
                Response.Redirect("http://www.mgmtransformer.com");


            lblDSRNoResults.Text = "";

            if (!IsPostBack)
            {
             
                lblReportsTitle.Text = Session["RepName"].ToString() + " Reports";

                if (txtFrom.Text == null || txtFrom.Text == "")
                    txtFrom.Text = dv.DefaultDateString("PrevMonday");

                if (txtTo.Text == null || txtTo.Text == "")
                    txtTo.Text = dv.DefaultDateString("PrevFriday");

                int iAdmin = Convert.ToInt32(Session["Admin"]);
                rblAll.Visible = (iAdmin == 1) ? true : false;

                if (Convert.ToBoolean(Session["Internal"]) == true)
                {
                    LoadAgents(true);
                    LoadDistributorSalesReport();
                }

                LoadDistDashboardYears();

                LoadWarehouseAgents();
                LoadInventory();

                FormatSection();
              

            }
         }

        private void LoadDistDashboardYears()
        {
            for (int i = DateTime.Now.Year - 10; i <= DateTime.Now.Year; i++)
            {
                ddlDistributorDashboardFromYear.Items.Add(i.ToString());
                ddlDistributorDashboardToYear.Items.Add(i.ToString());
            }

        }

        

        private void LoadInventory()
        {
            List<StockInventoryItem> lstKVA = new List<StockInventoryItem>();
            List<StockVoltages> lstStockVoltages;

            rblWindings.Items.Clear();
            rblWindings.DataSource = StockInventoryItem.GetAllItems().Select(r => r.Windings).Distinct().ToList().OrderBy(e => e);
            rblWindings.DataBind();
            rblWindings.SelectedIndex = 0;

            ddVoltage.Items.Clear();

            lstStockVoltages = StockVoltages.GetAllItems().Where(s => s.Windings == rblWindings.Items[0].Text).ToList();
            lstStockVoltages.Insert(0,new StockVoltages());
            ddVoltage.DataSource = lstStockVoltages;
            ddVoltage.DataTextField = "StockVoltageDisplay";
            ddVoltage.DataValueField = "StockVoltage";
            ddVoltage.DataBind();

            ddKVA.Items.Clear();
            lstKVA.Insert(0, new StockInventoryItem());
            ddKVA.DataSource = lstKVA;
            ddKVA.DataTextField = "sKVA";
            ddKVA.DataValueField = "KVA";
            ddKVA.DataBind();
        }
        
          
        
        protected void txtFrom_TextChanged(object sender, EventArgs e)
        {
            if (txtFrom.Text != null && txtFrom.Text != "")
            {
                lblFromReqd.Visible = false;
                string s = dv.DateValid(txtFrom.Text);

                if (s == "")
                {
                    txtFrom.Text = dv.DateFormat(txtFrom.Text);
                    lblFromInvalid.Visible = false;
                }
                else
                {
                    lblFromInvalid.Text = s;
                    lblFromInvalid.Visible = true;
                }
            }
            else
            {
                lblFromInvalid.Visible = false;
            }

        }

        protected void txtTo_TextChanged(object sender, EventArgs e)
        {
            if (txtTo.Text != null && txtTo.Text != "")
            {
                lblToReqd.Visible = false;
                string s = dv.DateValid(txtTo.Text);

                if (s == "")
                {
                    txtTo.Text = dv.DateFormat(txtTo.Text);
                    lblToInvalid.Visible = false;
                }
                else
                {
                    lblToInvalid.Text = s;
                    lblToInvalid.Visible = true;
                }
            }
            else
            {
                lblToInvalid.Visible = false;
            }

        }

        /// <summary>
        /// Called after validating individual fields.
        /// </summary>
        protected void ValidateFromToDates()
        {
            if (txtFrom.Text != null && txtFrom.Text != "" && txtTo.Text != null && txtTo.Text != "")
            {
                DateTime dtFrom = Convert.ToDateTime(txtFrom.Text);
                DateTime dtTo = Convert.ToDateTime(txtTo.Text);
                string s = dv.DateFromToValid(dtFrom, dtTo);

                if (s == "")
                {
                    lblFromInvalid.Visible = false;
                    lblToInvalid.Visible = false;
                }
                else
                {
                    lblFromInvalid.Text = s;
                    lblFromInvalid.Visible = true;
                }
            }
        }

        /// <summary>
        /// Return True if From / To are both entered and valid.
        /// </summary>
        /// <returns></returns>
        protected bool ValidateDateRange()
        {
            if (txtFrom.Text == null || txtFrom.Text == "")
            {
                lblFromReqd.Visible = true;
            }
            if (txtTo.Text == null || txtTo.Text == "")
            {
                lblToReqd.Visible = true;
            }

            if (lblFromReqd.Visible)
            {
                txtFrom.Focus();
            }
            else if (lblToReqd.Visible)
            {
                txtTo.Focus();
            }
            else
            {
                lblFromReqd.Visible = false;
                lblToReqd.Visible = false;

                ValidateFromToDates();
                if (!lblFromInvalid.Visible)
                {
                    return true;
                }
            }
            return false;
        }

        protected void lnkbPerformance_Click(object sender, EventArgs e)
        {
            ProcessButton("Performance");
        }
   

        protected void FormatSection()
        {

            string sHiddenOption = hidOption.Value;
            int iAdmin = Convert.ToInt32(Session["Admin"]);
            int iInternal = Convert.ToInt32(Session["Internal"]);

            btnCSV.Visible = true;
            btnPDF.Visible = true;

            DSR.Visible = false;
            ddlAgentsWithWarehouse.Visible = false;

            InventoryTable.Visible = false;

            lnkbPerformance.Visible = (sHiddenOption == "Performance") ? false : true;
            lblPerformance.Visible = (sHiddenOption == "Performance") ? true : false;
            txtFrom.Visible = lblFrom.Visible = sHiddenOption == "Performance" ? true : txtFrom.Visible;
            txtTo.Visible = lblTo.Visible = sHiddenOption == "Performance" ? true : txtTo.Visible;


            lnkbGiftCard.Visible = (sHiddenOption == "Gift Card") ? false : true;
            lblGiftCard.Visible = (sHiddenOption == "Gift Card") ? true : false;
            txtFrom.Visible = lblFrom.Visible = sHiddenOption == "Gift Card" ? true : txtFrom.Visible;
            txtTo.Visible = lblTo.Visible = sHiddenOption == "Gift Card" ? true : txtTo.Visible;


            lnkbAgentStockPrices.Visible = (iAdmin == 0) || (sHiddenOption == "Agent Stock Prices") ? false : true;
            lblAgentStockPrices.Visible = (iAdmin == 1) && (sHiddenOption == "Agent Stock Prices") ? true : false;

            // This is only for Admin.
            lblQuotesPendingApproval.Visible = (iAdmin == 1 && sHiddenOption == "Pending Approvals") ? true : false;
            lnkbQuotesPendingApproval.Visible = (iAdmin == 1 && sHiddenOption != "Pending Approvals") ? true : false;

            // From / To dates don't always apply.
            pnlCriteria.Visible = !lblAgentStockPrices.Visible && !lblQuotesPendingApproval.Visible;


            if (lnkbAgentStockPrices.Visible == false && lblAgentStockPrices.Visible == false)
                agentstockprices.Attributes["class"] = "lnkhidden";
            if (lnkbQuotesPendingApproval.Visible == false && lblQuotesPendingApproval.Visible == false)
                pendingapprovals.Attributes["class"] = "lnkhidden";

            rblAll.Visible = (sHiddenOption == "M1 Customer Sales") ? false : true;
            rblProductCat.Visible = (sHiddenOption == "M1 Customer Sales") ? true : false;
            lblAgentReqd.Visible = false;
            ddlAgents.Visible = (sHiddenOption == "M1 Customer Sales" && iInternal == 1) ? true : false;
            btnCSV.Visible = (sHiddenOption == "M1 Customer Sales") ? true : false;
            txtFrom.Visible = lblFrom.Visible = sHiddenOption == "M1 Customer Sales" ? true : txtFrom.Visible;
            txtTo.Visible = lblTo.Visible = sHiddenOption == "M1 Customer Sales" ? true : txtTo.Visible;
            lnkbM1CustSalesInfo.Visible = (sHiddenOption == "M1 Customer Sales") ? false : true;
            lblM1CustSalesInfo.Visible = (sHiddenOption == "M1 Customer Sales") ? true : false;

            btnPDF.Visible = ((sHiddenOption == "M1 Customer Sales") && (cbUseDSR.Checked == true)) ? false : btnPDF.Visible;
            txtFrom.Visible = ((sHiddenOption == "M1 Customer Sales") && (cbDSRQuarters.Checked == true)) ? false : txtFrom.Visible;
            lblFrom.Visible = ((sHiddenOption == "M1 Customer Sales") && (cbDSRQuarters.Checked == true)) ? false : lblFrom.Visible;
            txtTo.Visible = ((sHiddenOption == "M1 Customer Sales") && (cbDSRQuarters.Checked == true)) ? false : txtTo.Visible;
            lblTo.Visible = ((sHiddenOption == "M1 Customer Sales") && (cbDSRQuarters.Checked == true)) ? false : lblTo.Visible;

            //Distributed Sales Report Inputs
            DSR.Visible = (sHiddenOption == "M1 Customer Sales" && ((Convert.ToInt32(Session["RepID"]) == 65) || (iAdmin == 1))) ? true : false;


            //quote status
            rblAll.Visible = sHiddenOption == "Quote Status" ? false : rblAll.Visible;
            rblProductCat.Visible = sHiddenOption == "Quote Status" ? false : rblProductCat.Visible;
            ddlAgents.Visible = sHiddenOption == "Quote Status" ? false : ddlAgents.Visible;
            btnCSV.Visible = sHiddenOption == "Quote Status" ? false : btnCSV.Visible;
            txtFrom.Visible = lblFrom.Visible = sHiddenOption == "Quote Status" ? true : txtFrom.Visible;
            txtTo.Visible = lblTo.Visible = sHiddenOption == "Quote Status" ? true : txtTo.Visible;
            lnkbQuoteStatus.Visible = (sHiddenOption == "Quote Status") ? false : true;
            lblQuoteStatus.Visible = (sHiddenOption == "Quote Status") ? true : false;
            // end quote status


            btnCSV.Visible = (sHiddenOption == "Inventory") ? true : btnCSV.Visible;
            InventoryTable.Visible = (sHiddenOption == "Inventory") ? true : false;
            ddlAgentsWithWarehouse.Visible = rbSelectAgentInventory.Checked ? true : false;
            //btnExportToExcel.Visible = sHiddenOption == "Inventory" ? false : btnExportToExcel.Visible;
            rblProductCat.Visible = sHiddenOption == "Inventory" ? false : rblProductCat.Visible;
            rblAll.Visible = sHiddenOption == "Inventory" ? false : rblAll.Visible;
            txtFrom.Visible = lblFrom.Visible = sHiddenOption == "Inventory" ? false : txtFrom.Visible;
            txtTo.Visible = lblTo.Visible = sHiddenOption == "Inventory" ? false : txtTo.Visible;
            lnkbInventoryPerItem.Visible = (sHiddenOption == "Inventory") ? false : true;
            lblInventoryPerItem.Visible = (sHiddenOption == "Inventory") ? true : false;
            
            
            m1report.Visible = false;
            m1report.Attributes["class"] = "lnkhidden";

            if (Convert.ToInt32(Session["RepID"]) == 65 || Session["RepDistributorName"].ToString() == "Griesser Sales" || Convert.ToInt32(Session["Internal"]) == 1)
            {
                m1report.Visible = true;
                m1report.Attributes["class"] = "lnknotselected";
              
            }


            distributordashboard.Attributes["class"] = "lnkhidden";
            distributordashboard.Attributes["class"] = "lnknotselected";
          

            lnkDistributorDashboard.Visible = (sHiddenOption == "Agent Dashboard") ? false : true;
            lblDistributorDashboard.Visible = (sHiddenOption == "Agent Dashboard") ? true : false;
            txtFrom.Visible = lblFrom.Visible = sHiddenOption == "Agent Dashboard" ? false : txtFrom.Visible;
            txtTo.Visible = lblTo.Visible = sHiddenOption == "Agent Dashboard" ? false : txtTo.Visible;
            ddlAgents.Visible = (sHiddenOption == "Agent Dashboard" && iInternal == 1) ? true : ddlAgents.Visible;
            rblAll.Visible = (sHiddenOption == "Agent Dashboard") ? false : rblAll.Visible;
            rblProductCat.Visible = (sHiddenOption == "Agent Dashboard") ? false : rblProductCat.Visible;
            ddlDistributorDashboardFromYear.Visible = (sHiddenOption == "Agent Dashboard") ? true : false;
            ddlDistributorDashboardToYear.Visible = (sHiddenOption == "Agent Dashboard") ? true : false;
            lblDistDashFrom.Visible = (sHiddenOption == "Agent Dashboard") ? true : false;
            lblDistDashTo.Visible = (sHiddenOption == "Agent Dashboard") ? true : false;


            if (Session["SecurityLevel"] != null)
            {
                if (Session["SecurityLevel"].ToString() == "Special Pricing")
                {
                    lnkbGiftCard.Visible = (Session["SecurityLevel"].ToString() == "Special Pricing" ? false : true);
                    lblGiftCard.Visible = (Session["SecurityLevel"].ToString() == "Special Pricing" ? false : true);
                    lnkbGiftCard.Attributes["class"] = "lnkhidden";
                    giftcard.Attributes["class"] = "lnkhidden";

                    lnkDistributorDashboard.Visible = (Session["SecurityLevel"].ToString() == "Special Pricing" ? false : true);
                    lblDistributorDashboard.Visible = (Session["SecurityLevel"].ToString() == "Special Pricing" ? false : true);
                    lblDistributorDashboard.Attributes["class"] = "lnkhidden";
                }
            }




            rblAll.Visible = (sHiddenOption == "Performance" && iInternal != 1) ? false : rblAll.Visible;
            rblAll.Visible = (sHiddenOption == "Gift Card" && iInternal != 1) ? false : rblAll.Visible;


            if (Session["RepName"].ToString() == "IEM")
            {
                lnkbGiftCard.Visible = false;
                lblGiftCard.Visible = false;
                lnkbGiftCard.Attributes["class"] = "lnkhidden";
                giftcard.Attributes["class"] = "lnkhidden";
            }


            
            if (rblAll.Visible == false)
                rblAll.SelectedIndex = 0;
        }

        protected void ProcessButton(string sTarget)
        {
            lblNotEnoughDataDashboard.Text = "";
            hidOption.Value = sTarget;
            lblReportName.Text = sTarget + " Report";

            FormatSection();
        }


        /// <summary>
        /// Preview the selected report with the selected options.
        /// </summary>
        /// <param name="sTarget"></param>
        protected void Preview(string sTarget)
        {
            
            string sRptName = "";
            string sUserName = Session["UserName"].ToString();
            string sAgentName = "";
            int iInternal = Convert.ToInt32(Session["Internal"]);
            string sAgent = "";
            string sAgentCode = "";
            string sError;

            if (sTarget == "Performance")
            {
                sRptName = "PerformancePDF";
            }
            else if (sTarget == "Gift Card")
            {
                sRptName = "GiftCardPromotionPDF";
            }
            else if (sTarget == "Agent Stock Prices")
            {
                sRptName = "AgentStockPricesPDF";
            }
            else if (sTarget == "Pending Approvals")
            {
                sRptName = "AdminPendingApprovals";
            }
            else if (sTarget == "M1 Customer Sales")
            {
                sRptName = "M1CustSales";
            }

            else if (sTarget == "Quote Status")
            {
                sRptName = "QuoteStatusPDF";
            }
            else if (sTarget == "Inventory")
            {
                sRptName = "InventoryPDF";
            }
            else if (sTarget == "Agent Dashboard")
            {
                sRptName = "AgentDashboard"; 
            }
            else return;

            string sRedirect = "";
            if (pnlCriteria.Visible == true)
            {
                if (ValidateDateRange() == true)
                {
                    string sRepID = "";
                    // 0 = Current Rep - Always this choice if not Admin.
                    if (rblAll.SelectedValue == "0" || rblAll.Visible == false)
                    {
                        sRepID = Session["RepID"].ToString();
                    }
                    // 1 = All Reps.
                    else
                    {
                        sRepID = "0";
                    }

                    string sDateFrom = txtFrom.Text.ToString();
                    string sDateTo = txtTo.Text.ToString();

                    if (sTarget == "M1 Customer Sales")
                    {
                        string sProductCat = rblProductCat.SelectedValue;

                        if (ddlAgents.Visible == true)
                        {
                            sRepID = ddlAgents.SelectedValue;
                        }
                        else
                        {
                            sRepID = Session["MGMAgentNo"].ToString();
                        }

                        sRepID = string.IsNullOrEmpty(sRepID) ? "-1" : sRepID;
                        lblAgentReqd.Visible = false;

                        // If no rep selected, show Agent required and exit.
                        if (sRepID == "-1")
                        {
                            lblAgentReqd.Visible = true;
                            return;
                        }

                        sRedirect = "~/" + sRptName + ".aspx?AgentNo=" + sRepID + "&DateFrom=" + sDateFrom +
                                            "&DateTo=" + sDateTo + "&ProductCat=" + sProductCat;
                    }
                    else if (sTarget == "Performance")
                    {
                        sRedirect = "~/" + sRptName + ".aspx?DateFrom=" + sDateFrom +
                                            "&DateTo=" + sDateTo + "&RepID=" + sRepID + "&UserName=" + sUserName;
                    }
                    else if (sTarget == "Quote Status")
                    {
                        sRedirect = "~/" + sRptName + ".aspx?DateFrom=" + sDateFrom +
                                             "&DateTo=" + sDateTo + "&UserName=" + sUserName;
                    }
                    else if (sTarget == "Inventory")
                    {
                        if (ddlAgentsWithWarehouse.Visible == true)
                        {
                            sRepID = ddlAgentsWithWarehouse.SelectedValue;
                        }
                        else
                        {
                            sRepID = "-1";// Session["MGMAgentNo"].ToString();
                        }


                        if (rbSelectAgentInventory.Checked)
                        {
                            sRepID = string.IsNullOrEmpty(sRepID) ? "-1" : sRepID;
                            lblAgentReqd.Visible = false;

                            // If no rep selected, show Agent required and exit.
                            if (sRepID == "-1")
                            {
                                lblAgentReqd.Visible = true;
                                return;
                            }

                        }

                        sAgentName = ddlAgentsWithWarehouse.Items[ddlAgentsWithWarehouse.SelectedIndex].ToString();


                        sRedirect = "~/" + sRptName + ".aspx?Agent=" + sRepID + "&Name=" + Server.UrlEncode(sAgentName) + "&All=" +
                                                                     (rbAllAgentInventory.Checked ? "true" : "false") + "&KVA=0&VoltageCat=ALL&VoltageDisp=&Windings=&Searching=false";


                        if (rbSearchAvailability.Checked)
                        {


                            sRedirect = "~/" + sRptName + ".aspx?Agent=" + sRepID + "&Name=" + Server.UrlEncode(sAgentName) +
                                        "&All=true&KVA=" + (ddKVA.SelectedItem.Text == "ALL" ? "0" : ddKVA.SelectedItem.Text) +
                                        "&VoltageCat=" + StockVoltages.GetCategory(ddVoltage.SelectedItem.Text, rblWindings.SelectedItem.Text) +
                                        "&VoltageDisp=" + ddVoltage.SelectedItem.Text +
                                        "&Windings=" + (rblWindings.SelectedItem.Text == "Aluminum" ? "Al" : "Cu") +
                                        "&Searching=true";


                        }
                     
                    }
                    else if (sTarget == "Agent Dashboard")
                    {

                       
                        int _iStartDate = Convert.ToInt32(ddlDistributorDashboardFromYear.SelectedItem.ToString());
                        int _iEndDate = Convert.ToInt32(ddlDistributorDashboardToYear.SelectedItem.ToString());

                        if(_iEndDate - _iStartDate + 1 > 6)
                        {
                            lblNotEnoughDataDashboard.Text = "Maximum 6 Years Time Span.";
                            return;
                        }

                        if (Convert.ToInt32(Session["Internal"]) == 1)
                            sAgent = Agent.GetAgentCode(Convert.ToInt32(ddlAgents.SelectedItem.Value), ddlAgents.SelectedItem.Text);
                        else
                            sAgent = Agent.GetAgentCode(Agent.GetMGMAgentNo(Session["RepName"].ToString()), Session["RepName"].ToString());

                        if (sAgent == "")
                        {
                            lblNotEnoughDataDashboard.Text = "Agent information not found.";
                            return;
                        }


                        sAgentName = Session["RepName"].ToString(); 

                        if (ddlAgents.Visible)
                        {
                            if ((ddlAgents.SelectedItem.Value == "-1") && (iInternal == 1))
                            {
                                lblInventoryExternal.ForeColor = System.Drawing.Color.Red;
                                lblInventoryExternal.Text = "Agent Required";
                                lblInventoryExternal.Visible = true;
                                return;
                            }

                            sAgent = ddlAgents.SelectedItem.Value;
                            sAgentName = ddlAgents.SelectedItem.Text;
                        }

                        if (Convert.ToInt32(Session["Internal"]) == 1)
                            sAgentCode = Agent.GetAgentCode(Convert.ToInt32(sAgent), ddlAgents.SelectedItem.Text);
                        else
                            sAgentCode = Agent.GetAgentCode(Convert.ToInt32(sAgent),sAgentName);


                        AgentDashboardPDF adPDF = new AgentDashboardPDF(new DateTime(Convert.ToInt32(ddlDistributorDashboardFromYear.SelectedItem.ToString()), 1, 1),
                                                                       new DateTime(Convert.ToInt32(ddlDistributorDashboardToYear.SelectedItem.ToString()), 12, 31),
                                                                       sAgentName == "All Agents" ? "All Agents" : sAgentCode, sAgentName,sAgent,"");

                        if (!adPDF.IsThereData)
                        {
                            lblNotEnoughDataDashboard.Text = "Not enough Data to generate Dashboard";
                            return;
                        }
                        else
                        {
                         
                            if(WebConfigurationManager.AppSettings["SaveAgentDashboard"] != null &&
                                WebConfigurationManager.AppSettings["SaveAgentDashboard"].ToString() == "1")
                            {
                                string sURL = "Agent Dashboard - [" + sAgentName.Replace("&","and") + "," + ddlDistributorDashboardFromYear.SelectedItem.ToString() + "," +
                                                                                         ddlDistributorDashboardToYear.SelectedItem.ToString() + "]" + ".pdf";
                                sURL = sURL.Replace("&", "and");

                                if (WebConfigurationManager.AppSettings["LocalMachine"] != null && 
                                    Convert.ToBoolean(WebConfigurationManager.AppSettings["LocalMachine"]) == true)
                                {

                                    if(WebConfigurationManager.AppSettings["LocalMachinePath"] == null)
                                    {
                                        lblDSRNoResults.Text = "No Path Setting";
                                        return;
                                    }

                                    adPDF.FileName = WebConfigurationManager.AppSettings["LocalMachinePath"] + sURL;
                                    adPDF.CreatePDF(out sError);


                                    if(WebConfigurationManager.AppSettings["LocalWebSiteURL"] == null)
                                    {
                                        lblDSRNoResults.Text = "Redirect Failed.";
                                        return;
                                    }
                                    ResponseHelper.Redirect(WebConfigurationManager.AppSettings["LocalWebSiteURL"] + sURL, "_blank", "");
                                }
                                else
                                {
                                    adPDF.FileName = "C:\\MGMQuotation\\pdfs\\" + sURL;
                                    adPDF.CreatePDF(out sError);
                                    ResponseHelper.Redirect("https://MGMQuotation.MGMTransformer.com//MGMQuotation//pdfs//" + sURL, "_blank", "");

                                }
                            }
                            else
                            sRedirect = "~/" + sRptName + ".aspx?YearFrom=" + ddlDistributorDashboardFromYear.SelectedItem.ToString() +
                                            "&YearTo=" + ddlDistributorDashboardToYear.SelectedItem.ToString() + "&AgentNo=" + sAgent +
                                            "&AgentName=" + sAgentName.Replace("&", "88");
                        }

                    }
                    else
                    {
                        sRedirect = "~/" + sRptName + ".aspx?DateFrom=" + sDateFrom +
                                            "&DateTo=" + sDateTo + "&RepID=" + sRepID;
                    }
                }
            }
            else
            {
                sRedirect = "~/" + sRptName + ".aspx";
            }

            // open PDF.
            ResponseHelper.Redirect(sRedirect, "_blank", "");
        }
   
        protected void btnPDF_Click(object sender, EventArgs e)
        {
            Preview(hidOption.Value);
        }

        protected void lnkbGiftCard_Click(object sender, EventArgs e)
        {
            ProcessButton("Gift Card");
        }

        protected void lnkbAgentStockPrices_Click(object sender, EventArgs e)
        {
            ProcessButton("Agent Stock Prices");
        }

        protected void lnkbQuotesPendingApproval_Click(object sender, EventArgs e)
        {
            ProcessButton("Pending Approvals");
        }

        protected void lnkbM1CustSalesInfo_Click(object sender, EventArgs e)
        {

            txtFrom.Visible = true;
            uptxtTo.Visible = true;
            lblFrom.Visible = true;
            uptxtToLbl.Visible = true;

            cbDSRQuarters_CheckedChanged(sender, e);


            LoadAgents(true);
            ProcessButton("M1 Customer Sales");
        }

        protected void LoadWarehouseAgents()
        {

            string[] sExcludedAgents = { };

            int iInternal = Convert.ToInt32(Session["Internal"]);

            DataTable dtAgentsWithWarehouse = q.AgentsWithWarehouses(iInternal == 1 ? false : true);
            ddlAgentsWithWarehouse.DataTextField = "Distributor";
            ddlAgentsWithWarehouse.DataValueField = "MGMAgentNo";

            if (WebConfigurationManager.AppSettings["ExcludeAgentCode"] != null)
            {
                sExcludedAgents = WebConfigurationManager.AppSettings["ExcludeAgentCode"].ToString().Split(';');
            }

            foreach (string sExclude in sExcludedAgents)
            {
                if (sExclude != "") 
                    dtAgentsWithWarehouse = dtAgentsWithWarehouse.AsEnumerable().Select(r => r).Where(d => Convert.ToInt32(d["MGMAgentNo"]) != Convert.ToInt32(sExclude)).Select(v => v).CopyToDataTable();
            }

            //remove MGM
            dtAgentsWithWarehouse = dtAgentsWithWarehouse.AsEnumerable().Select(r => r).Where(d => Convert.ToInt32(d["MGMAgentNo"]) != 0).Select(v => v).CopyToDataTable();

            DataRow drAgentWarehouse = dtAgentsWithWarehouse.NewRow();// dtWithoutMGM.NewRow();
            drAgentWarehouse["MGMAgentNo"] = -1;
            drAgentWarehouse["Distributor"] = "Please select an agent...";
            dtAgentsWithWarehouse.Rows.InsertAt(drAgentWarehouse, 0);

            ddlAgentsWithWarehouse.DataSource = dtAgentsWithWarehouse;// dtWithoutMGM;

            ddlAgentsWithWarehouse.DataBind();
            
            
            
            
            
            //int iInternal = Convert.ToInt32(Session["Internal"]);
            //DataTable dtWithoutMGM = new DataTable();

            //DataTable dtAgentsWithWarehouse = q.AgentsWithWarehouses(iInternal == 1 ? false : true);
            //ddlAgentsWithWarehouse.DataTextField = "Distributor";
            //ddlAgentsWithWarehouse.DataValueField = "MGMAgentNo";

            //dtWithoutMGM = dtAgentsWithWarehouse.AsEnumerable().Select(r => r).Where(d => Convert.ToInt32(d["MGMAgentNo"]) != 0).Select(v => v).CopyToDataTable();

            //DataRow drAgentWarehouse = dtWithoutMGM.NewRow();
            //drAgentWarehouse["MGMAgentNo"] = -1;
            //drAgentWarehouse["Distributor"] = "Please select an agent...";
            //dtWithoutMGM.Rows.InsertAt(drAgentWarehouse, 0);

            //ddlAgentsWithWarehouse.DataSource = dtWithoutMGM;

            //ddlAgentsWithWarehouse.DataBind();
        }

        protected void LoadAgents(bool bIncludeAll)
        {

            string[] sExcludedAgents = { };

            ddlAgents.Items.Clear();

            DataTable dt = q.Agents();
            ddlAgents.DataTextField = "Distributor";
            ddlAgents.DataValueField = "MGMAgentNo";

            //if (WebConfigurationManager.AppSettings["ExcludeAgentCode"] != null)
            //{
            //    sExcludedAgents = WebConfigurationManager.AppSettings["ExcludeAgentCode"].ToString().Split(';');
            //}

            //foreach (string sExclude in sExcludedAgents)
            //{
            //    if (sExclude != "")
            //        dt = dt.AsEnumerable().Select(r => r).Where(d => Convert.ToInt32(d["MGMAgentNo"]) != Convert.ToInt32(sExclude)).Select(v => v).CopyToDataTable();
            //}


            DataRow dr = dt.NewRow();
            dr["MGMAgentNo"] = -1;
            dr["Distributor"] = "Please select an agent...";
            dt.Rows.InsertAt(dr, 0);

            if (bIncludeAll)
            {
                dr = dt.NewRow();
                dr["MGMAgentNo"] = -2;
                dr["Distributor"] = "All Agents";
                dt.Rows.InsertAt(dr, 1);
            }
            
            ddlAgents.DataSource = dt;
            ddlAgents.DataBind();
            
            
            //ddlAgents.Items.Clear();

            //DataTable dt = q.Agents();
            //ddlAgents.DataTextField = "Distributor";
            //ddlAgents.DataValueField = "MGMAgentNo";

            //DataRow dr = dt.NewRow();
            //dr["MGMAgentNo"] = -1;
            //dr["Distributor"] = "Please select an agent...";
            //dt.Rows.InsertAt(dr, 0);

            //ddlAgents.DataSource = dt;
            //ddlAgents.DataBind();

        }

        private void LoadDistributorSalesReport()
        {
            lstYears = new List<string>();
         

            try
            {
                _iMinusYears = Convert.ToInt32(WebConfigurationManager.AppSettings["YearStartMinus"]);
            }
            catch (Exception ex) { _iMinusYears = 10; }

            try
            {
                _iPlusYears = Convert.ToInt32(WebConfigurationManager.AppSettings["YearStartPlus"]);
            }
            catch (Exception ex) { _iPlusYears = 5; }



            _iCurrentYear = DateTime.Now.Year;
            _iStartYear = _iCurrentYear - _iMinusYears;
            _iEndYear = _iCurrentYear + _iPlusYears;


            if (_iStartYear >= _iEndYear)
                _iStartYear = 2005;

            lstYears.Add("");

            for (int i = _iStartYear; i < _iEndYear; i++)
            {
                lstYears.Add(i.ToString());
            }

            ddlDSRYear.DataSource = lstYears;
            ddlDSRYear.DataBind();
            ddlDSRYear.SelectedIndex = 0;

        }



        protected void lnkInventory_Click(object sender, EventArgs e)
        {
            ProcessButton("Inventory");
        }           

        protected void lnkTransaction_Click(object sender, EventArgs e)
        {
            ProcessButton("Inventory Transaction History");
        }

        protected void lnkQuoteStatus_Click(object sender, EventArgs e)
        {
            ProcessButton("Quote Status");
        }

        protected void lnkInventoryPerItem_Click(object sender, EventArgs e)
        {
            ProcessButton("Inventory");
        }

        protected void lnkDistDashboard_Click(object sender, EventArgs e)
        {
            LoadAgents(false);
            ProcessButton("Agent Dashboard");
        }


        protected void rbSelectAgentInventory_CheckedChanged(object sender, EventArgs e)
        {
            ddlAgentsWithWarehouse.Visible = rbSelectAgentInventory.Checked;
            InventorySelections.Visible = !rbSelectAgentInventory.Checked;
            lblAgentReqd.Visible = false;
        }

        protected void rbAllAgentInventory_CheckedChanged(object sender, EventArgs e)
        {
            ddlAgentsWithWarehouse.Visible = !rbAllAgentInventory.Checked;
            InventorySelections.Visible = !rbAllAgentInventory.Checked;
            lblAgentReqd.Visible = false;
        }

        protected void ddlAgentsWithWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblAgentReqd.Visible = false;
            lblNoInventoryData.Text = "";
        }


        private bool DownloadInventoryCSV()
        {
            int iInternal = Convert.ToInt32(Session["Internal"]);
            string sRepID = Session["RepID"].ToString();
            int iAgentNo = Utility.GetAgentNoFromRepID(Convert.ToInt32(sRepID));
            bool bAll = rbAllAgentInventory.Checked;
            bool bSearch = rbSearchAvailability.Checked;
            bool bAgent = rbSelectAgentInventory.Checked;
            string sKVA = ddKVA.SelectedItem.Text == "ALL" ? "0" : ddKVA.SelectedItem.Text;
            string sCategory = StockVoltages.GetCategory(ddVoltage.SelectedItem.Text, rblWindings.SelectedItem.Text);
            string sWinding = rblWindings.SelectedItem.Text == "Aluminum" ? "Al" : "Cu";
            string sAgentWarehouse = ddlAgentsWithWarehouse.SelectedItem.Text;

            InventoryReportCSV irCDV = new InventoryReportCSV(iInternal, sRepID, iAgentNo, bAll, bSearch, bAgent, 
                sKVA, sCategory, sWinding, sAgentWarehouse, Response,false);

            return irCDV.DownloadInventoryCSV();

        }

        private void DownloadM1CSV()
       {
            string sRepID = "";
            string sProductCat = rblProductCat.SelectedValue;
            lblSelectAgent.Text = "";


            if (Session["Internal"].ToString() == "0")
                sRepID = Session["RepID"].ToString();
            else
                sRepID = ddlAgents.SelectedValue;


            M1ReportCSV m1CSV = new M1ReportCSV(DateTime.Parse(txtFrom.Text), DateTime.Parse(txtTo.Text), sProductCat,
                                 Convert.ToInt32(Session["Internal"]), Session["RepID"].ToString(), ddlAgents.SelectedValue, sRepID, Response);
            
            
            //M1RepPortalCSV m1CSV = new M1RepPortalCSV(DateTime.Parse(txtFrom.Text), DateTime.Parse(txtTo.Text), sProductCat,
            //                    Convert.ToInt32(Session["Internal"]), Session["RepID"].ToString(), ddlAgents.SelectedValue, Response);
            
            
            
            if (ddlAgents.SelectedValue.Contains("-1"))
            {
                lblSelectAgent.Text = "Please Select an Agent";
                return;
            }

            if (m1CSV.RowCount > 0)
                m1CSV.DownloadCSV();
            else
                lblSelectAgent.Text = "No Results";
           
       }


        protected void btnCSV_Click(object sender, EventArgs e)
        {
            DateTime startDateDSR;
            DateTime endDateDSR;
            int iDSRYear;
            string sAgentCode;
            int iMGMAgentNo = -1;
            string sAgentName = "";
            AgentDashboardCSV adCSV;

            switch (hidOption.Value)
            {
                case "Inventory":
                    if (DownloadInventoryCSV())
                        lblNoInventoryData.Text = "No Data";
                    break;
                case "M1 Customer Sales":
                    if (cbUseDSR.Checked)
                    {
                        if (!cbDSRQuarters.Checked)
                        {
                            if (!VerifyDSRDates())
                                return;
                        }

                        iMGMAgentNo = Convert.ToInt32(ddlAgents.SelectedItem.Value);
                        sAgentCode = Agent.GetAgentCode(iMGMAgentNo, ddlAgents.SelectedItem.Text);
                        sAgentName = ddlAgents.SelectedItem.Text;

                        if (sAgentCode == "" && sAgentName != "All Agents")
                        {
                            lblDSRNoResults.Text = "No Results";
                            return;
                        }


                        if (cbDSRQuarters.Checked)
                        {
                            if (!Int32.TryParse(ddlDSRYear.SelectedItem.ToString(), out iDSRYear))
                            {
                                lblDSRNoResults.Text = "Please Select Quarter Year";
                                return;
                            }
                            
                            startDateDSR = new DateTime(Convert.ToInt32(ddlDSRYear.SelectedItem.ToString()), 1, 1);
                            endDateDSR = startDateDSR.AddYears(1);
                            endDateDSR = endDateDSR.AddDays(-1);


                            adCSV = new AgentDashboardCSV(startDateDSR, endDateDSR, sAgentName == "All Agents" ? "All Agents" : sAgentCode,
                                                                          (rblProductCat.SelectedItem.Value == "AS" || rblProductCat.SelectedItem.Value == "ALL"),
                                                                           (rblProductCat.SelectedItem.Value == "AA" || rblProductCat.SelectedItem.Value == "ALL"),
                                                                           (rblProductCat.SelectedItem.Value == "ALL"), cbDSRQuarters.Checked, 
                                                                           Convert.ToInt32(ddlDSRYear.SelectedItem.ToString()),Response,true,"");

                        }
                        else
                        {

                            adCSV = new AgentDashboardCSV(DateTime.Parse(txtFrom.Text), DateTime.Parse(txtTo.Text),
                                                          sAgentName == "All Agents" ? "All Agents" : sAgentCode,
                                                          (rblProductCat.SelectedItem.Value == "AS" || rblProductCat.SelectedItem.Value == "ALL"),
                                                          (rblProductCat.SelectedItem.Value == "AA" || rblProductCat.SelectedItem.Value == "ALL"),
                                                          (rblProductCat.SelectedItem.Value == "ALL"), cbDSRQuarters.Checked,
                                                          DateTime.Now.Year,Response,true,"");

                        }


                        if (!adCSV.DownloadCSV())
                            lblDSRNoResults.Text = "No Results";
                        else
                            lblDSRNoResults.Text = "";

                    }
                    else
                        DownloadM1CSV();
                    break;
                default:
                    break;
            }
        }


        private bool VerifyDSRDates()
        {
            bool retValue = true;
            DateTime dtFrom;
            DateTime dtTo;

            if(!DateTime.TryParse(txtFrom.Text, out dtFrom))
            {
               retValue = false;
            }

            if (!DateTime.TryParse(txtTo.Text, out dtTo))
            {
                retValue = false;
            }
           
            return retValue;

        }



        protected void rbSearchAvailability_CheckedChanged(object sender, EventArgs e)
        {
            InventoryTable.Visible = rbSearchAvailability.Checked;
            ddlAgentsWithWarehouse.Visible = !rbSearchAvailability.Checked;
            lblAgentReqd.Visible = false;

            if (rbSearchAvailability.Checked)
                InventorySelections.Visible = true;
            else
                InventorySelections.Visible = false;
        }

        protected void rblWindings_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<StockInventoryItem> lstKVA = new List<StockInventoryItem>();
            List<StockVoltages> lstStockVltgs;

            ddVoltage.Items.Clear();
            lstStockVltgs = StockVoltages.GetAllItems().Where(s => s.Windings == rblWindings.Items[rblWindings.SelectedIndex].Text).ToList();
            lstStockVltgs.Insert(0, new StockVoltages());
            ddVoltage.DataSource = lstStockVltgs;
            ddVoltage.DataTextField = "StockVoltageDisplay";
            ddVoltage.DataValueField = "StockVoltage";
            ddVoltage.DataBind();


            ddKVA.Items.Clear();
            lstKVA.Insert(0, new StockInventoryItem());
            ddKVA.DataSource = lstKVA;
            ddKVA.DataTextField = "sKVA";
            ddKVA.DataValueField = "KVA";
            ddKVA.DataSource = lstKVA;
            ddKVA.DataBind();

            lblNoInventoryData.Text = "";
        }

        protected void ddVoltage_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<StockInventoryItem> lstKVA = new List<StockInventoryItem>();

            ddKVA.Items.Clear();

            lstKVA = StockInventoryItem.GetAllItems().Select(r => r)
                .Where(t => (t.Windings == rblWindings.SelectedItem.Text) && (t.Configuration == ddVoltage.SelectedItem.Value))
                .Distinct(new StockInventoryItemComparer()).ToList();

            lstKVA.Insert(0, new StockInventoryItem());
            ddKVA.DataSource = lstKVA;
            ddKVA.DataTextField = "sKVA";
            ddKVA.DataValueField = "KVA";
            ddKVA.DataBind();

            lblNoInventoryData.Text = "";
        }

        protected void cbDSRQuarters_CheckedChanged(object sender, EventArgs e)
        {
            if(cbDSRQuarters.Checked)
            {
                ddlDSRYear.Enabled = true;
                ddlDSRYear.SelectedIndex = ddlDSRYear.Items.IndexOf(ddlDSRYear.Items.FindByText(DateTime.Now.Year.ToString()));
                txtFrom.Visible = false;
                lblFrom.Visible = false;
                txtTo.Visible = false;
                lblTo.Visible = false;
            }
            else
            {
                ddlDSRYear.Enabled = false;
                ddlDSRYear.SelectedIndex = 0;
                txtFrom.Visible = true;
                lblFrom.Visible = true;
                txtTo.Visible = true;
                lblTo.Visible = true;
            }
        }

        protected void cbUseDSR_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUseDSR.Checked)
            {
                btnPDF.Visible = false;
                btnCSV.Visible = true;
                cbDSRQuarters.Enabled = true;
            }
            else
            {
                btnPDF.Visible = true;
                btnCSV.Visible = true;
                cbDSRQuarters.Checked = false;
                cbDSRQuarters.Enabled = false;
                txtFrom.Visible = true;
                lblFrom.Visible = true;
                txtTo.Visible = true;
                lblTo.Visible = true;
                ddlDSRYear.SelectedIndex = 0;
            }
        }

        protected void ddlDSRYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDSRNoResults.Text = "";
        }

        protected void ddlAgentsDSR_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDSRNoResults.Text = "";
        }

        protected void ddlAgents_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblInventoryExternal.Visible = false;
            lblSelectAgent.Text = "";
        }

        protected void ddlDistributorDashboardFromYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblNotEnoughDataDashboard.Text = "";
        }

        protected void ddlDistributorDashboardToYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblNotEnoughDataDashboard.Text = "";
        }

        protected void ddKVA_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblNoInventoryData.Text = "";
        }
    }
}