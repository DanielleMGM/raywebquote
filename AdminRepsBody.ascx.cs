using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace MGM_Transformer
{
    public partial class AdminRepsBody : System.Web.UI.UserControl
    {
        DataValidation dv = new DataValidation();
        RepObject r = new RepObject();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["IsLoggedIn"]) == 0)
                Response.Redirect("http://www.mgmtransformer.com");

            lblEMailPersonError.Text = "";
            lblEMail.Text = "";
            lblErrorGoalAmount.Text = "";

            if (!IsPostBack)
            {
               
                if (Session["RepIdBackup"] != null)
                {
                    ShowData();
                    Session["RepIdBackup"] = null;
                }
                else
                {
                    Session["RepIdBackup"] = Session["RepID"];
                    Reset();
                }
                // Go back to active screen.
                foreach (String key in Request.QueryString.AllKeys)
                {
                    if (key == "Process")
                    {
                        string sProcess = Request.QueryString[key];
                        ProcessButton(sProcess);
                    }
                }

            }

        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Convert.ToInt32(Session["RepIDBackup"]) != 0)
                {
                    Session["RepID"] = Session["RepIdBackup"];
                    Session["RepDistributorID"] = Session["RepIdBackup"];
                }
            }
        }
        
        protected void gvRepList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRep")
            {
                int RepId = Convert.ToInt32(e.CommandArgument);
                Session["RepID"] = RepId;
                Session["RepDistributorID"] = RepId;
                Session["Price"] = 0.00M;

                ShowData();
            }
        }
        protected void gvLogins_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string sUserName = "";
            if (e.CommandName == "Select")
            {
                // Cell 1.
                sUserName = e.CommandArgument.ToString();

                UIHelper u = new UIHelper();

                GridViewRow currentRow = (GridViewRow)UIHelper.FindAncestor(e.CommandSource as Control, typeof(GridViewRow));

                string sPassword = gvLogins.DataKeys[currentRow.RowIndex].Values[1].ToString();
                string sFullName = gvLogins.DataKeys[currentRow.RowIndex].Values[2].ToString();
                string sLastActivity = gvLogins.DataKeys[currentRow.RowIndex].Values[3].ToString();
                string sCreated = gvLogins.DataKeys[currentRow.RowIndex].Values[4].ToString();
                
                string sEmail = currentRow.Cells[3].Text.ToString();
                string sPhone = currentRow.Cells[4].Text.ToString();
                string sPhoneType = currentRow.Cells[5].Text.ToString();
                string sPhoneStatus = currentRow.Cells[6].Text.ToString();
                string sVPN_IP = currentRow.Cells[7].Text.ToString();

                if (sPassword == "&nbsp;") sPassword = "";
                if (sEmail == "&nbsp;") sEmail = "";
                if (sPhone == "&nbsp;") sPhone = "";
                if (sVPN_IP == "&nbsp;") sVPN_IP = "";

                txtUserName.Text = sUserName;
                txtPassword.Text = sPassword;
                txtFullNameLogin.Text = sFullName;
                txtLoginEmail.Text = sEmail;
                txtLoginPhone.Text = sPhone;
                lblPhoneType.Text = sPhoneType;
                lblPhoneStatus.Text = sPhoneStatus;

                Session["UserName"] = sUserName.ToString();

                // Show Grant Access button only if access has been requested.
                btnGrantAccess.Visible = (lblPhoneStatus.Text == "1 Request") ? true: false;

                lblLoginCreated.Text = sCreated;
                lblLoginLastActivity.Text = sLastActivity;
                txtVPN_IP.Text = sVPN_IP;

                btnLoginAdd.Visible = false;

                btnLoginUpdate.Visible = true;
                btnLoginCancel.Visible = true;
                btnLoginDelete.Visible = true;
            }
            if (e.CommandName == "Delete")
            {
                sUserName = e.CommandArgument.ToString();
                r.DeleteLogin(sUserName);

                // Allows this page to stay as is, but with gridview redrawn.
                // Tried everything to get this to work more easily...
                Session["RepIdBackup"] = Session["RepID"];

                string sURL = dv.BaseUrl(Request.RawUrl);
                Response.Redirect(sURL + "?Process=Logins");
            }

        }
        
        protected void gvRepIPs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string sIP_Address = "";
            string sDescription = "";

            if (e.CommandName == "Select")
            {
                sIP_Address = e.CommandArgument.ToString();

                UIHelper u = new UIHelper();

                GridViewRow currentRow = (GridViewRow)UIHelper.FindAncestor(e.CommandSource as Control, typeof(GridViewRow));
                sDescription = currentRow.Cells[3].Text.ToString();

                txtIPAddress.Text = sIP_Address;
                txtIPDescription.Text = sDescription;

                btnAddIP.Visible = false;

                btnUpdateIP.Visible = true;
                btnCancelIP.Visible = true;
                btnDeleteIP.Visible = true;

            }
            if (e.CommandName == "Delete")
            {
                sIP_Address = e.CommandArgument.ToString();
                DeleteRepIP(sIP_Address);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            RepObject r = new RepObject();
            int RepID = r.Insert();
            Session["RepID"] = RepID;
            Session["RepDistributorID"] = RepID;

            RefreshData();
            ShowData();
        }
        protected void btnGeneralUpdate_Click(object sender, EventArgs e)
        {
            Update();
        }

        protected void btnGeneralCancel_Click(object sender, EventArgs e)
        {
            Reset();
        }

        protected void btnGeneralDelete_Click(object sender, EventArgs e)
        {
            RadioButtonList v = (RadioButtonList)pnlGeneral.FindControl("rblVerifyDelete");
            Label lv = (Label)pnlGeneral.FindControl("lblVerifyDelete");
           
            v.SelectedValue = null;
            v.Visible = true;
            lv.Visible = true;
        }

        protected void btnUpdateIP_Click(object sender, EventArgs e)
        {
            int iRepID = Convert.ToInt32(Session["RepID"]);
            string sIPAddress = txtIPAddress.Text.ToString();
            string sIPDescription = txtIPDescription.Text.ToString();
            r.RepIP_Insert(iRepID, sIPAddress, sIPDescription);
            ClearIP();
        }
        protected void btnCancelIP_Click(object sender, EventArgs e)
        {
            ClearIP();
        }
        protected void btnDeleteIP_Click(object sender, EventArgs e)
        {
            string sIP_Address = txtIPAddress.Text;
            DeleteRepIP(sIP_Address);
            ClearIP();
        }

        protected void ClearIP()
        {
            txtIPAddress.Text = "";
            txtIPDescription.Text = "";
            
            btnAddIP.Visible = true;

            btnUpdateIP.Visible = false;
            btnCancelIP.Visible = false;
            btnDeleteIP.Visible = false;

            gvRepIPs.DataBind();
        }
        
        
        protected void LoadData()
        {
            ClearLogin();
            ClearIP();

            // General.
            // ===========
            int RepID = Convert.ToInt32(Session["RepID"]);
            if (!r.RepExists(RepID))
                return;

            string s = "";
            string sDisplayName;

            DataSet ds = r.Select(RepID);
            DataRow dr = ds.Tables[0].Rows[0];

            sDisplayName = dr["Display_Name"].ToString();

            txtFull_Name.Text = dr["Full_Name"].ToString();
            lblTitle.Text = txtFull_Name.Text;
            lblRepID.Text = dr["RepID"].ToString();

            s = dr["MGMAgentNo"].ToString();
            txtMGMAgentNo.Text = dv.NumberFormat(s, 4, 0);

            LoadSecurityLevels();
            s = dr["SecurityLevel"].ToString();
            ddlSecurityLevel.SelectedValue = s;

            s = dr["PriceMultiplier"].ToString();
            txtPriceMultiplier.Text = dv.NumberFormat(s, 4, 2);

            txtLeadTimes.Text = dr["LeadTimeNoDays"].ToString();

            txtEmail.Text = dr["Email"].ToString();
            txtPhone.Text = dr["Phone"].ToString(); 
            
            // Handles nulls.
            lblCreated_on.Text = dv.DateFormat(dr["Created_on"].ToString());
            lblLast_activity.Text = dv.DateFormat(dr["Last_Activity"].ToString());

            SetInternalRep(RepID, s);               // Set value for Session field RepIDPrices.

            // Agent Members.
            // ===========
            dsDistributors.DataBind();
            gvDistributors.DataBind();

            // Logins.
            // ===========
            gvLogins.DataBind();

            // IP Addresses.
            // ===========
            gvRepIPs.DataBind();

            // Stock Prices.
            // ===========
            dsRepProductPrices.DataBind();
            gvRepProductPrices.DataBind();

            //Rep. Maintanence
            //============
            tbAgentPerson.Text = DBNull.Value.Equals(dr["ReportsEmailTo"]) ? "" : dr["ReportsEmailTo"].ToString();
            tbAgentReportEMail.Text = DBNull.Value.Equals(dr["ReportsEmail"]) ? "" : dr["ReportsEmail"].ToString();



            DataSet dsGoals;
            decimal dGoalAmount;
            string sSQL = "select LatestYear,GoalName,GoalAmount,AgentName from DistributorDashboardGoals";
            dsGoals = DataLink.Select(sSQL, DataLinkCon.bpss);


            List<string> lstLatestYear = Utility.GetValueList<string>(dsGoals, "LatestYear");
            lstLatestYear = lstLatestYear.Select(c => c).Distinct().ToList();
            ddGoalYear.DataSource = lstLatestYear;
            ddGoalYear.DataBind();

            List<string> lstGoalName = Utility.GetValueList<string>(dsGoals, "GoalName");
            lstGoalName = lstGoalName.Select(c => c).Distinct().ToList();
            ddGoalName.DataSource = lstGoalName;
            ddGoalName.DataBind();


            if(dsGoals.Tables.Count > 0)
            {
                if(dsGoals.Tables[0].Rows.Count > 0)
                {

                    IEnumerable<decimal> dRows = dsGoals.Tables[0].AsEnumerable().Select(row => row)
                              .Where(v => v["AgentName"].ToString().Trim() == sDisplayName.Trim())
                              .Select(n => Convert.ToDecimal(n["GoalAmount"]));


                    if (dRows.Count() > 0)
                    {
                        dGoalAmount = dRows.First();
                        if (dGoalAmount == 0)
                            tbGoalAmount.Text = "0.00";
                        else
                            tbGoalAmount.Text = dGoalAmount.ToString("#,##.00");
                        //string sTemp = string.Format("{0:C}", dGoalAmount);
                        //tbGoalAmount.Text = sTemp.Substring(1, sTemp.Length - 1);// string.Format("{0:C}", dGoalAmount); 
                    }
                    else
                    {
                        dGoalAmount = 0;
                        tbGoalAmount.Text = dGoalAmount.ToString("0.00");
                    }

                   
                }

            }

            ProcessButton(hidOption.Value);// "General");
        }

        protected void LoadSecurityLevels()
        {
            DropDownList ddl = (DropDownList)pnlGeneral.FindControl("ddlSecurityLevel");

            Codes cd = new Codes("SecurityLevel");
            string s = "";
            ListItem li;

            li = new ListItem("Select Security Type", "");
            ddl.Items.Add(li);

            DataSet ds = cd.Select();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                s = dr["SecurityLevel"].ToString();

                li = new ListItem(s, s);
                ddl.Items.Add(li);
            }
        }

        protected void RefreshProductPrices()
        {
            int RepID = Convert.ToInt32(Session["RepID"]);

            DropDownList ddl = (DropDownList)pnlGeneral.FindControl("ddlSecurityLevel");

            string SecurityLevel = ddl.SelectedValue;

            SetInternalRep(RepID, SecurityLevel);

            RepObject r = new RepObject();
            int recordsaffected = r.RefreshProductList(RepID, SecurityLevel);

            RefreshData();
        }

        protected void ShowData()
        {
            int RepID = Convert.ToInt32(Session["RepID"]);

            RepObject r = new RepObject();
            bool RepExists = r.RepExists(RepID);

            EditRegion.Visible = RepExists;
 
            if (RepExists)
            {
                LoadData();

            }
        }

        protected void Reset()
        {
            Session["RepID"] = 0;
            Session["RepDistributorID"] = 0;
            ShowData();
        }

        protected void txtFull_Name_TextChanged(object sender, EventArgs e)
        {
            bool valid = MainValidate("txtFull_Name");
        }
        protected void txtVPN_IP_TextChanged(object sender, EventArgs e)
        {
            bool valid = MainValidate("txtVPN_IP");
        }

        protected void ddlSecurityLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool valid = MainValidate("ddlSecurityLevel");

            if (valid)
            {
                int RepID = Convert.ToInt32(Session["RepID"]);
                string sl = ddlSecurityLevel.SelectedValue;

                SetInternalRep(RepID, sl);

                RepObject r = new RepObject();
                int recordsaffected = r.RefreshProductList(RepID, sl);

                RefreshData();
            }
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            bool valid = MainValidate("txtEmail");
        }

        protected void txtPassword_TextChanged(object sender, EventArgs e)
        {
            bool valid = LoginValidate("txtPassword");
        }

        protected void txtPriceMultiplier_TextChanged(object sender, EventArgs e)
        {
            bool valid = MainValidate("txtPriceMultiplier");
        }

        protected void txtMGMAgentNo_TextChanged(object sender, EventArgs e)
        {
            bool valid = MainValidate("txtMGMAgentNo");
        }

        protected void txtLoginEmail_TextChanged(object sender, EventArgs e)
        {
            bool valid = LoginValidate("txtLoginEmail");
        }

        protected void txtInitials_TextChanged(object sender, EventArgs e)
        {
            bool valid = LoginValidate("txtInitials");
        }

        protected void Update()
        {
            bool valid = MainValidate("*");

            if (!valid) return;

            RepObject r = new RepObject();
            string s;

            r.RepId = Convert.ToInt32(lblRepID.Text);
            r.Full_Name = txtFull_Name.Text;

            string sLeadTimes = dv.StringFromText(txtLeadTimes.Text);
            if (sLeadTimes != "")
                r.LeadTimeNoDays = Convert.ToInt32(txtLeadTimes.Text);
            r.Email = txtEmail.Text.ToString();
            r.MGMAgentNo = Convert.ToInt32(txtMGMAgentNo.Text);
            r.Password = txtPassword.Text.ToString();
            r.Phone = txtPhone.Text.ToString();

            s = txtPriceMultiplier.Text.ToString();
            r.PriceMultiplier = Convert.ToDecimal(s);
            r.SecurityLevel = ddlSecurityLevel.SelectedValue.ToString();
            //r.UserName = txtUserName.Text.ToString();

            int recordsaffected = r.Update();
            // Allows this page to stay as is, but with gridview redrawn.
            // Tried everything to get this to work more easily...
            Session["RepIdBackup"] = Session["RepID"];

            string sURL = dv.BaseUrl(Request.RawUrl);
            Response.Redirect(sURL + "?Process=General");

            //Reset();

        }

        /// <summary>
        /// Both inserts and updates logins.
        /// </summary>
        protected void InsertUpdateLogin()
        {
            bool valid = LoginValidate("*");

            if (!valid) return;

            RepObject r = new RepObject();
            
            int iRepId = Convert.ToInt32(Session["RepID"]);
            string sUserName = txtUserName.Text;
            string sFullName = txtFullNameLogin.Text;
            string sPassword = txtPassword.Text;
            string sPhoneType = lblPhoneType.Text;
            string sLoginEmail = txtLoginEmail.Text;
            string sLoginPhone = txtLoginPhone.Text;
            string sVPN_IP = txtVPN_IP.Text;

            r.InsertUpdateLogin(iRepId, sUserName, sFullName, sPassword, sLoginEmail, sLoginPhone, sPhoneType,  sVPN_IP);

            if (!lblLoginErrors.Visible)
                ClearLogin();

            // Allows this page to stay as is, but with gridview redrawn.
            // Tried everything to get this to work more easily...
            Session["RepIdBackup"] = Session["RepID"];

            string sURL = dv.BaseUrl(Request.RawUrl);
            Response.Redirect(sURL + "?Process=Logins");
        }
        
        protected bool MainValidate(string ctlName)
        {
            bool bErrors = false;
            lblErrors.Visible = false;

            // Perform validation for one or all controls in the section.
            if (ctlName == "*" || ctlName == "txtFull_Name")
            {

                TextBox txt = (TextBox)pnlGeneral.FindControl("txtFull_Name");
                Label lblR = (Label)pnlGeneral.FindControl("lblFull_NameReqd");
                Label lblC = (Label)pnlGeneral.FindControl("lblFull_NameChange");
                Label lblV = (Label)pnlGeneral.FindControl("lblFull_NameInvalid");

                string s = "";
                
                s = txt.Text;
                lblR.Visible = (s == null || s == "") ? true : false;
                lblC.Visible = false;
                lblV.Visible = false;
                if (!lblR.Visible)
                {
                    // User has not changed default name.
                    if (s == "Name")
                        lblC.Visible = true;
                }
                if (!lblC.Visible)
                {
                    RepObject r = new RepObject();
                    int id = r.RepNameExists(s);
                    int RepID = Convert.ToInt32(Session["RepId"]);

                    // If we found an ID for this name that's not the current record, a conflict is found.
                    if (id > 0 && id != RepID)
                        lblV.Visible = true;
                }

                if (!bErrors) if (lblC.Visible || lblR.Visible || lblV.Visible) bErrors = true;
            }

            if (ctlName == "*" || ctlName == "txtFullNameLogin")
            {

                TextBox txt = (TextBox)pnlLogins.FindControl("txtFullNameLogin");
                Label lblR = (Label)pnlLogins.FindControl("lblFullNameLoginReqd");
               
                string s = "";

                s = txt.Text;
                lblR.Visible = (s == null || s == "") ? true : false;

                if (!bErrors) if (lblR.Visible) bErrors = true;
            }
            
            if (ctlName == "*" || ctlName == "txtMGMAgentNo")
            {
                TextBox txt = (TextBox)pnlGeneral.FindControl("txtMGMAgentNo");
                Label lblV = (Label)pnlGeneral.FindControl("lblMGMAgentNoInvalid");
                Label lblR = (Label)pnlGeneral.FindControl("lblMGMAgentNoReqd");

                string s = "";

                s = txt.Text;

                // Validate MGM Agent No rules.
                lblV.Visible = false;
                if (s.Length > 0)
                {
                    lblR.Visible = false;
                    DataValidation dv = new DataValidation();
                    s = dv.NumberFormat(s, txtMGMAgentNo.Columns, 0);

                    lblV.Visible = (s == null || s == "") ? true : false;

                    if (s.Length > 0)
                        txt.Text = s;
                }
                else
                {
                    lblR.Visible = true;
                }

                if (!bErrors) if (lblR.Visible || lblV.Visible) bErrors = true;
            }

            if (ctlName == "*" || ctlName == "ddlSecurityLevel")
            {
                DropDownList ddl = (DropDownList)pnlGeneral.FindControl("ddlSecurityLevel");
                Label lblR = (Label)pnlGeneral.FindControl("lblSecurityLevelReqd");

                string s = "";

                s = ddl.SelectedValue;
                lblR.Visible = (s == null || s == "") ? true : false;

                if (!bErrors) if (lblR.Visible) bErrors = true;
            }

            if (ctlName == "*" || ctlName == "txtEmail")
            {
                TextBox txt = (TextBox)pnlGeneral.FindControl("txtEmail");
                Label lblV = (Label)pnlGeneral.FindControl("lblEmailInvalid");

                lblV.Visible = false;
                string sEmail = txt.Text;

                // Validate email rules.
                string sValid = dv.EmailValid(sEmail);

                if (sValid != "")
                {
                    lblV.Text = sValid;
                    lblV.Visible = true;
                }   

                if (!bErrors) if (lblV.Visible) bErrors = true;
            }

            if (ctlName == "*" || ctlName == "txtPhone")
            {
                TextBox txt = (TextBox)pnlGeneral.FindControl("txtPhone");
                Label lblV = (Label)pnlGeneral.FindControl("lblPhoneInvalid");

                DataValidation dv = new DataValidation();

                string sPhoneIn = txt.Text;
                string sPhoneOut = dv.PhoneFmt(sPhoneIn);

                if (sPhoneIn != "" && sPhoneOut == "")
                {
                    lblV.Visible = true;
                }
                else
                {
                    lblV.Visible = false;
                    txtPhone.Text = sPhoneOut;
                }

                if (!bErrors) if (lblV.Visible) bErrors = true;
            }

            if (ctlName == "*" || ctlName == "txtPriceMultiplier")
            {

                TextBox txt = (TextBox)pnlGeneral.FindControl("txtPriceMultiplier");
                Label lblR = (Label)pnlGeneral.FindControl("lblPriceMultiplierReqd");
                Label lblRules = (Label)pnlGeneral.FindControl("lblPriceMultiplierRules");
                Label lblV = (Label)pnlGeneral.FindControl("lblPriceMultiplierInvalid");

                string s = "";

                s = txt.Text;
                lblR.Visible = (s == null || s == "") ? true : false;

                // Validate Price Multiplier rules.
                lblV.Visible = false;
                if (!lblR.Visible)
                {
                    DataValidation dv = new DataValidation();
                    s = dv.NumberFormat(s, txtPriceMultiplier.Columns, 2);
                    if (s.Length > 0)
                    {
                        Single pm = Convert.ToSingle(s);
                        if (pm < .8 || pm > 1.2)
                        {
                            lblV.Visible = true;
                        }
                        else
                        {
                            lblV.Visible = false;
                            txtPriceMultiplier.Text = s;
                        }
                    }
                    else
                        lblV.Visible = true;
                }

                if (!bErrors) if (lblR.Visible || lblV.Visible) bErrors = true;
            }

            lblErrors.Visible = bErrors;
            return !bErrors;
        }

        protected bool LoginValidate(string ctlName)
        {
            bool bErrors = false;
            lblLoginErrors.Visible = false;

            // Perform validation for one or all controls in the section.
            if (ctlName == "*" || ctlName == "txtUserName")
            {
                TextBox txt = (TextBox)pnlLogins.FindControl("txtUserName");
                Label lblR = (Label)pnlLogins.FindControl("lblUserNameReqd");
                Label lblV = (Label)pnlLogins.FindControl("lblUserNameInvalid");
              

                string sUserName = txt.Text;

                if (sUserName == "")
                {
                    lblR.Visible = true;
                }
                else
                {
                    lblR.Visible = false;
                    string sInvalid = dv.UserNameValid(sUserName);

                    if (sInvalid == "")
                    {
                        lblV.Visible = false;
                    }
                    else
                    {
                        lblV.Visible = true;
                        lblV.Text = sInvalid;
                    }
                }
                if (!bErrors) if (lblR.Visible || lblV.Visible) bErrors = true;
            }

            if (ctlName == "*" || ctlName == "txtPassword")
            {
                TextBox txt = (TextBox)pnlLogins.FindControl("txtPassword");
                Label lblR = (Label)pnlLogins.FindControl("lblPasswordReqd");
                Label lblV = (Label)pnlLogins.FindControl("lblPasswordInvalid");
               
                string sPassword = txt.Text;

                if (sPassword == "")
                {
                    lblR.Visible = true;
                }
                else
                {
                    lblR.Visible = false;
                    string sInvalid = dv.PasswordValid(sPassword);

                    if (sInvalid == "")
                    {
                        lblV.Visible = false;
                    }
                    else
                    {
                        lblV.Visible = true;
                        lblV.Text = sInvalid;
                    }
                }
                if (!bErrors) if (lblR.Visible || lblV.Visible) bErrors = true;
            }

            if (ctlName == "*" || ctlName == "txtLoginEmail")
            {
                TextBox txt = (TextBox)pnlLogins.FindControl("txtLoginEmail");
                Label lblV = (Label)pnlLogins.FindControl("lblLoginEmailInvalid");
               
                lblV.Visible = false;
                string sEmail = txt.Text;

                // Validate email rules.
                string sValid = dv.EmailValid(sEmail);

                if (sValid != "")
                {
                    lblV.Text = sValid;
                    lblV.Visible = true;
                }   
            
                if (!bErrors) if (lblV.Visible) bErrors = true;
            }

            if (ctlName == "*" || ctlName == "txtLoginPhone")
            {
                TextBox txt = (TextBox)pnlLogins.FindControl("txtLoginPhone");
                Label lblV = (Label)pnlLogins.FindControl("lblLoginPhoneInvalid");

                DataValidation dv = new DataValidation();

                string sPhoneIn = txt.Text;
                string sPhoneOut = dv.PhoneFmt(sPhoneIn);

                if (sPhoneIn != "" && sPhoneOut == "")
                {
                    lblV.Visible = true;
                }
                else
                {
                    lblV.Visible = false;
                    txtPhone.Text = sPhoneOut;
                }

                if (!bErrors) if (lblV.Visible) bErrors = true;
            }

            if (ctlName == "*" || ctlName == "txtVPN_IP")
            {
                TextBox txt = (TextBox)pnlLogins.FindControl("txtVPN_IP");
                Label lblV = (Label)pnlLogins.FindControl("lblVPN_IPInvalid");
              
                string s = "";

                s = txt.Text;

                // Validate MGM Agent No rules.
                lblV.Visible = false;
                if (s.Length > 0)
                {
                    DataValidation dv = new DataValidation();
                    if(!dv.IPAddressValid(s))
                        s = "";

                    lblV.Visible = (s == null || s == "") ? true : false;

                    if (s.Length > 0)
                        txt.Text = s;
                }
 
                if (!bErrors) if (lblV.Visible) bErrors = true;
            }

            lblLoginErrors.Visible = bErrors;

            return !bErrors;
        }

        protected void RefreshData()
        {
            dsRep.DataBind();
            gvRepList.DataBind();

            dsRepProductPrices.DataBind();
            gvRepProductPrices.DataBind();
        }

        protected void rblVerifyDelete_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList v = (RadioButtonList)pnlGeneral.FindControl("rblVerifyDelete");
            Label lv = (Label)pnlGeneral.FindControl("lblVerifyDelete");

            string s = v.SelectedValue;

            lv.Visible = false;
            v.Visible = false;

            if (s == "Yes")
                DeleteRep();
        }

        protected void DeleteRep()
        {
            RepObject r = new RepObject();
            int RepID = Convert.ToInt32(Session["RepID"]);
            if (r.RepExists(RepID))
            {
                int recordsaffected = r.Delete(RepID);
            }
            Reset();

            // Refresh page, since Reset() didn't seem to do it.
            string sURL = dv.BaseUrl(Request.RawUrl);
            Response.Redirect(sURL + "?Process=General");
        }

        // Resets Session field RepIDPrices based on input parameters.
        protected void SetInternalRep(int RepID, string SecurityLevel)
        {
            //Label lbl = (Label)upgvRepProductPrices.FindControl("lblInternalRepDistributors");

            //if (SecurityLevel == "Internal Rep")
            //    Session["RepDistributorID"] = 18;        // Mc Gee
            //else
            //    Session["RepDistributorID"] = RepID;
        }

        protected void gvRepProductPrices_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvRepProductPrices.EditIndex = e.NewEditIndex;
            gvRepProductPrices.DataBind();

        }
        protected void gvRepProductPrices_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = (GridViewRow)gvRepProductPrices.Rows[e.RowIndex];
            Label lbl = (Label)row.FindControl("lblStockRepID");
            TextBox txt = (TextBox)row.FindControl("txtPrice");

            string s = lbl.Text;
            int StockRepID = (s == null || s == "") ? 0 : Convert.ToInt32(s);
            Session["RepDistributorID"] = StockRepID;

            string Price = txt.Text;
            Prices p = new Prices();
            decimal prc = p.ValidPrice(Price);
            Session["Price"] = prc;

            gvRepProductPrices.EditIndex = -1;
            gvRepProductPrices.DataBind();
        }
        protected void gvRepProductPrices_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvRepProductPrices.EditIndex = -1;
        }

        protected void gvRepProductPrices_RowUpdated(Object sender, GridViewUpdatedEventArgs e)
        {
            // Use the Exception property to determine whether an exception
            // occurred during the update operation.
            if (e.Exception == null)
            {
                // Sometimes an error might occur that does not raise an 
                // exception, but prevents the update operation from 
                // completing. Use the AffectedRows property to determine 
                // whether the record was actually updated. 
                if (e.AffectedRows == 1)
                {
                    // Use the Keys property to get the value of the key field.
                    String keyFieldValue = e.Keys["CustomerID"].ToString();

                }
                else
                {
                    // When an error occurs, keep the GridView
                    // control in edit mode.
                    // e.KeepInEditMode = true;          
                }
            }
            else
            {
                // Insert the code to handle the exception.
                Response.Write(e.Exception.Message);

                // Use the ExceptionHandled property to indicate that the 
                // exception is already handled.
                e.ExceptionHandled = true;

                e.KeepInEditMode = true;
            }
        }

        protected void txtPhone_TextChanged(object sender, EventArgs e)
        {
            bool valid = MainValidate("txtPhone");
        }

        protected void txtFullNameLogin_TextChanged(object sender, EventArgs e)
        {
            bool valid = MainValidate("txtFullNameLogin");
        }

        protected void txtLoginPhone_TextChanged(object sender, EventArgs e)
        {
            bool valid = MainValidate("txtLoginPhone");
        }
 
        protected void txtUserName_TextChanged(object sender, EventArgs e)
        {
            bool valid = LoginValidate("txtUserName");
        }

        protected void txtIP_TextChanged(object sender, EventArgs e)
        {
            bool valid = MainValidate("txtIP");
        }

        protected void txtIPAddress_TextChanged(object sender, EventArgs e)
        {
            Label lbl = (Label)pnlIPAddresses.FindControl("lblIPAddressReqd");
            Label lblV = (Label)pnlIPAddresses.FindControl("lblIPAddressInvalid");
            Label lblE = (Label)pnlIPAddresses.FindControl("lblAddIPErrors");
            Label lblOK = (Label)pnlIPAddresses.FindControl("lblAddIPOkay");

            lbl.Visible = false;
            lblE.Visible = false;
            lblOK.Visible = false;

            bool valid = dv.IPAddressValid(txtIPAddress.Text);
            lblV.Visible = !valid;
        }

        protected void btnAddIP_Click(object sender, EventArgs e)
        {
            TextBox txtIP = (TextBox)pnlIPAddresses.FindControl("txtIPAddress");
            TextBox txtDesc = (TextBox)pnlIPAddresses.FindControl("txtIPDescription");
            Label lbl = (Label)pnlIPAddresses.FindControl("lblIPAddressReqd");
            Label lblV = (Label)pnlIPAddresses.FindControl("lblIPAddressInvalid");
            Label lblE = (Label)pnlIPAddresses.FindControl("lblAddIPErrors");
            Label lblOK = (Label)pnlIPAddresses.FindControl("lblAddIPOkay");
            GridView gvRepIPs = (GridView)pnlIPAddresses.FindControl("gvRepIPs");


            string sIP = txtIP.Text.ToString();
            string sDesc = txtDesc.Text.ToString();

            lbl.Visible = (sIP == null || sIP == "") ? true : false;

            lblE.Visible = (lbl.Visible || lblV.Visible) ? true : false;

            if (lblE.Visible)
                return;

            RepObject r = new RepObject();

            int iRepID = Convert.ToInt32(Session["RepID"]);

            r.RepIP_Insert(iRepID, sIP, sDesc);

            lblOK.Visible = true;
            lblE.Visible = false;

            ClearIP();

            // Allows this page to stay as is, but with gridview redrawn.
            // Tried everything to get this to work more easily...
            Session["RepIdBackup"] = Session["RepID"];

            string sURL = dv.BaseUrl(Request.RawUrl);
            Response.Redirect(sURL + "?Process=IPAddresses");
        }

        protected void btnGrantAccess_Click(object sender, EventArgs e)
        {
            RepObject r = new RepObject();
            int iRepID = Convert.ToInt32(Session["RepID"]);
            string sUserName = Session["UserName"].ToString();

            int iReturn = r.PhoneAccessGrant(sUserName, iRepID);

            if (iReturn == -1)
            {
                lblPhoneStatus.Text = "2 Grant";

                btnLoginUpdate.Focus();
                btnGrantAccess.Visible = false;
            }
        }

        protected void DeleteRepIP(string sIP_Address)
        {
            RepObject r = new RepObject();
            int iRepID = Convert.ToInt32(Session["RepID"]);

            r.RepIP_Delete(iRepID, sIP_Address);

            // Allows this page to stay as is, but with gridview redrawn.
            // Tried everything to get this to work more easily...
            Session["RepIdBackup"] = Session["RepID"];

            string sURL = dv.BaseUrl(Request.RawUrl);
            Response.Redirect(sURL + "?Process=IPAddresses");
        }

        // Remove selected Rep if paging.
        protected void gvRepList_PageIndexChanged(object sender, EventArgs e)
        {
            Reset();
        }

        protected void gvDistributors_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRep")
            {
                int RepId = Convert.ToInt32(e.CommandArgument);
                Session["RepID"] = RepId;
                Session["RepDistributorID"] = 0;
                Session["Price"] = 0.00M;
              

                ShowData();
            }
        }

        protected void gvRepList_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvDistributors_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvRepIPs_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvRepProductPrices_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void txtLeadTimes_TextChanged(object sender, EventArgs e)
        {
            txtLeadTimes.Text = dv.NumberFormat(txtLeadTimes.Text, 2, 0);
        }

        protected void lnkbGeneral_Click(object sender, EventArgs e)
        {
            hidOption.Value = "General";
            ProcessButton("General");
            
        }

        protected void lnkbAgentMembers_Click(object sender, EventArgs e)
        {
            hidOption.Value = "AgentMembers";
            ProcessButton("AgentMembers");
        }

        protected void lnkbLogins_Click(object sender, EventArgs e)
        {
            hidOption.Value = "Logins";
            ProcessButton("Logins");
        }

        protected void lnkbIPAddresses_Click(object sender, EventArgs e)
        {
            hidOption.Value = "IPAddresses";
            ProcessButton("IPAddresses");
        }

        protected void lnkbStockPrices_Click(object sender, EventArgs e)
        {
            hidOption.Value = "StockPrices";
            ProcessButton("StockPrices");
        }

        protected void FormatSection()
        {
            string sHiddenOption = hidOption.Value;

            lnkbGeneral.Visible = (sHiddenOption == "General") ? false : true;
            lnkbAgentMembers.Visible = (sHiddenOption == "AgentMembers") ? false : true;
            lnkbLogins.Visible = (sHiddenOption == "Logins") ? false : true;
            lnkbIPAddresses.Visible = (sHiddenOption == "IPAddresses") ? false : true;
            lnkbStockPrices.Visible = (sHiddenOption == "StockPrices") ? false : true;
            lnkbRepSetup.Visible = (sHiddenOption == "ReportSetup") ? false : true;


            lblGeneral.Visible = (sHiddenOption == "General") ? true : false;
            lblAgentMembers.Visible = (sHiddenOption == "AgentMembers") ? true : false;
            lblLogins.Visible = (sHiddenOption == "Logins") ? true : false;
            lblIPAddresses.Visible = (sHiddenOption == "IPAddresses") ? true : false;
            lblStockPrices.Visible = (sHiddenOption == "StockPrices") ? true : false;
            lblRepSetup.Visible = (sHiddenOption == "ReportSetup") ? true : false;


            pnlGeneral.Visible = lblGeneral.Visible;
            pnlAgentMembers.Visible = lblAgentMembers.Visible;
            pnlLogins.Visible = lblLogins.Visible;
            pnlIPAddresses.Visible = lblIPAddresses.Visible;
            pnlStockPrices.Visible = lblStockPrices.Visible;
            pnlRepMaint.Visible = lblRepSetup.Visible;


            if (sHiddenOption == "AgentMembers")
            {
                string s = txtMGMAgentNo.Text.ToString();
                lblAgentMembersTitle.Text = "Agent #" + s + " Members";
            }
        }

        protected void ProcessButton(string sTarget)
        {
            hidOption.Value = sTarget;

            FormatSection();
        }

        protected void btnLoginAdd_Click(object sender, EventArgs e)
        {
            InsertUpdateLogin();
        }
        protected void btnLoginUpdate_Click(object sender, EventArgs e)
        {
            InsertUpdateLogin();
        }
        protected void btnLoginCancel_Click(object sender, EventArgs e)
        {
            ClearLogin();
        }
        protected void btnLoginDelete_Click(object sender, EventArgs e)
        {
            LoginValidate("txtUserName");
            string sUserName = txtUserName.Text;
            r.DeleteLogin(sUserName);
            ClearLogin();
        }

        protected void ClearLogin()
        {
            txtUserName.Text = "";
            txtFullNameLogin.Text = "";
            txtPassword.Text = "";
            txtLoginPhone.Text = "";
            lblPhoneType.Text = "";
            lblPhoneStatus.Text = "";
            txtVPN_IP.Text = "";
            txtLoginEmail.Text = "";
            lblLoginCreated.Text = "";
            lblLoginLastActivity.Text = "";

            btnLoginAdd.Visible = true;
            btnLoginUpdate.Visible = false;
            btnLoginCancel.Visible = false;
            btnLoginDelete.Visible = false;

            gvLogins.DataBind();
        }

        protected void lnkbRepMaintenance_Click(object sender, EventArgs e)
        {
            hidOption.Value = "ReportSetup";
            ProcessButton("ReportSetup");
        }



        protected void UpdateReportGoalAmount_Click(object sender, EventArgs e)
        {
            string sGoalName = ddGoalName.Text;
            string sGoalYear = ddGoalYear.Text;
            string sSQL = string.Empty;

            if (IsValidDollarAmount(tbGoalAmount.Text.Replace(",","")))
            {
                lblErrorGoalAmount.ForeColor = System.Drawing.Color.Green;

                sSQL = "select * from DistributorDashboardGoals where AgentID = " + Convert.ToInt32(Session["RepID"]) + " and GoalName = '" + sGoalName + "' and LatestYear = '" + sGoalYear + "'";
                DataSet dsGoals = DataLink.Select(sSQL, DataLinkCon.bpss);
                
                if(dsGoals.Tables.Count > 0)
                {
                    if(dsGoals.Tables[0].Rows.Count > 0)
                    {
                        sSQL = "update DistributorDashboardGoals set GoalAmount = '" + tbGoalAmount.Text + "' where AgentID = " + Convert.ToInt32(Session["RepID"]) +
                            " and GoalName = '" + sGoalName + "' and LatestYear = '" + sGoalYear + "'";

                        DataLink.Update(sSQL, DataLinkCon.bpss);
                    }
                    else
                    {
                        sSQL = "select Display_Name from Rep where RepID = " + Convert.ToInt32(Session["RepID"]);
                        string sRepName = Utility.GetStringValue(DataLink.Select(sSQL, DataLinkCon.mgmuser), "Display_Name");


                        sSQL = "insert into DistributorDashboardGoals (AgentID,AgentName,GoalName,GoalAmount,LatestYear,LatestAmount) values (" + Convert.ToInt32(Session["RepID"]) +
                                 ",'" + sRepName + "','" + sGoalName + "'," + tbGoalAmount.Text + ",'" + sGoalYear + "','0.00)";

                        DataLink.Update(sSQL, DataLinkCon.bpss);
                    }
                    
                }

                lblErrorGoalAmount.Text = "Goal Amount Updated";
            }
            else
            {
                
                 lblErrorGoalAmount.ForeColor = System.Drawing.Color.Red;
                 lblErrorGoalAmount.Text = "Improper Goal Amount";
            }
        }

        protected bool IsValidDollarAmount(string sText)
        {
            decimal d;
            return decimal.TryParse(sText, out d);
        }



        protected void UpdateReportEMail_Click(object sender, EventArgs e)
        {
            if (IsValidEmail(tbAgentReportEMail.Text))
            {
                lblEMail.ForeColor = System.Drawing.Color.Green;
                DataLink.Update("update Rep set ReportsEmail = '" + tbAgentReportEMail.Text + "' where RepID = " + Convert.ToInt32(Session["RepID"]), DataLinkCon.mgmuser);
                lblEMail.Text = "E-Mail Address Updated";
            }
            else
            {
                if (tbAgentReportEMail.Text == "")
                {
                    lblEMail.ForeColor = System.Drawing.Color.Green;
                    DataLink.Update("update Rep set ReportsEmail = null where RepID = " + Convert.ToInt32(Session["RepID"]), DataLinkCon.mgmuser);
                    lblEMail.Text = "E-Mail Address Cleared";
                }
                else
                {
                    lblEMail.ForeColor = System.Drawing.Color.Red;
                    lblEMail.Text = "Improper E-Mail Format";
                }
            }
        }

        protected void UpdateReportEMailPerson_Click(object sender, EventArgs e)
        {
           if(!DataLink.Update("update Rep set ReportsEmailTo = '" + tbAgentPerson.Text + "' where RepID = " + Convert.ToInt32(Session["RepID"]), DataLinkCon.mgmuser))
            {
                lblEMailPersonError.ForeColor = System.Drawing.Color.Red;
                lblEMailPersonError.Text = "Agent Name Failed to Update.";
            }
           else
            {
                lblEMailPersonError.ForeColor = System.Drawing.Color.Green;
                lblEMailPersonError.Text = "Agent Name Updated.";

            }
        }


        private bool IsValidEmail(string email)
        {
            bool bRetVal = false;
            string[] sEmails = email.Split(';');

            foreach (string sEmail in sEmails)
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(sEmail);
                    bRetVal = addr.Address == sEmail;

                    if (bRetVal == false)
                        return false;
                }
                catch
                {
                    return false;
                }
            }

            return bRetVal;
        }
        
    }
}