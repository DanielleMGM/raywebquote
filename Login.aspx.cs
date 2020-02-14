using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Duo;
using System.IO;                              // Two factor security from www.duosecurity.com - DuoWeb.dll

namespace MGM_Transformer
{
      
    
    public partial class Login : System.Web.UI.Page
    {
        Quotes q = new Quotes();
        Security s = new Security();
        string sIP = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try {
                if (!IsPostBack)
                {
                    txtUserName.Focus();
                }
            }
            catch (Exception ex)
            {
                string sMsg = "Error message when logging in: " + ex.Message;
                q.AddToTestLog(sMsg);
                return;
            }
        }

        protected bool SubmitLogin()
        {
            lblInvalid.Text = "Processing...";

            Session["ValidIP"] = false;

            for (int i = 1; i <= 5; i++)
            {
                sIP = Request.UserHostAddress;

                Session["WriteOutIP"] = sIP;

                // use for logging IP Addresses
                //List<string> lst = new List<string>();
                //lst.Add(DateTime.Now + sIP);
                //File.WriteAllLines("C:\\WebLog\\log.txt", lst);

                if (String.IsNullOrEmpty(sIP) == false)
                {
                    break;
                }
                System.Threading.Thread.Sleep(1000);
            }

            Session["ValidIP"] = false;

            // Set True if using LocalHost.
            if (sIP == "::1")
            {
                Session["ValidIP"] = true;
            }
            else
            {
                // Retry whether or not we have a valid IP, 
                // as the system seems to jump to the conclusion 
                // that the IP is invalid.
                for (int i = 1; i <= 5; i++)
                {
                    if (s.ValidIpAddress(sIP))
                    {
                        // Comment this to test messages without valid IP.
                        Session["ValidIP"] = true;
                        break;
                    }

                    System.Threading.Thread.Sleep(1000);
                }
            }

            if (Convert.ToBoolean(Session["ValidIP"]) == false)
            {
                btnLogin.Focus();
                txtUserName.Focus();
            }

            string sUserName = txtUserName.Text;
            string sPassword = txtPassword.Text;

            if (string.IsNullOrEmpty(sUserName) == true)
            {
                lblInvalid.Text = "Please provide user name.";
                lblInvalid.Visible = true;
                txtUserName.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(sPassword) == true)
            {
                lblInvalid.Text = "Please provide password.";
                lblInvalid.Visible = true;
                txtPassword.Focus();
                return false;
            }
            lblInvalid.Visible = false;

            Session["UserName"] = sUserName;
            Session["UserNameFilter"] = "";         // Used on Home screen to filter to our quotes only.
            Session["Password"] = sPassword;

            bool bValidIP = Convert.ToBoolean(Session["ValidIP"]);

            string sOutcome = "";
            int iSecurityLevel = 0;
            bool bUserName = false;
            string sFullName = "";
            string sMGMAgentNo = "";
            string sRepName = "";
            string sEmail = "";
            string sPhone = "";
            string sPhoneProgress = "";
            string sPhoneType = "";
            string sSecurityLevel = "";

            int RepID = s.ValidateUser(sUserName, sPassword, out iSecurityLevel,
                                out sOutcome, out bUserName, out sFullName, out sMGMAgentNo, out sRepName,
                                out sEmail, out sPhone, out sPhoneType, out sPhoneProgress, out sSecurityLevel);

            if (sOutcome == "Welcome!")
            {
                lblInvalid.Visible = false;
                Session["Admin"] = (iSecurityLevel == 2) ? 1 : 0;
                Session["Configuration"] = "240V DELTA PRIMARY - 208Y/120";
                Session["SecurityLevel"] = sSecurityLevel;
                Session["Email"] = sEmail;
                Session["FullName"] = sFullName;
                Session["Inactive"] = "0";
                Session["QuoteCopy"] = 0;
                Session["QuoteCreator"] = sUserName;            // May change to a rep login if internal rep creating on their behalf.
                Session["QuoteNo"] = 0;
                Session["QuoteNoDisplay"] = "";
                Session["QuoteNoVer"] = 1;
                Session["CustomerID"] = 0;
                Session["Internal"] = (iSecurityLevel > 0) ? 1 : 0;
                Session["IsLoggedIn"] = 1;
                Session["IsSubmittal"] = 0; 
                Session["IsTimeout"] = 0;
                Session["Last90Days"] = 1;
                Session["LoginFilter"] = 0;
                Session["MGMAgentNo"] = sMGMAgentNo;
                Session["PageName"] = "Home";
                Session["Password"] = sPassword;
                Session["Phase"] = "Three";
                Session["Phone"] = sPhone;
                Session["PhoneProgress"] = sPhoneProgress;
                Session["PhoneType"] = sPhoneType;
                Session["RepDistributorID"] = RepID;
                Session["RepDistributorName"] = sRepName;
                Session["RepID"] = RepID;
                Session["RepIDRpt"] = 0;
                Session["RepName"] = sRepName;
                Session["UserName"] = sUserName;
                Session["Winding"] = "Aluminum";
                Session["UserNameFilter"] = sUserName;
                Session["SearchText"] = "";

                HttpCookie mgmCookie = new HttpCookie("MGMLoggedIn");

                mgmCookie["LoggedIn"] = "True";
                Response.Cookies.Add(mgmCookie);
                
                // If Duo Security set up and permission granted, redirect.
                    
                if (!bValidIP)
                {
                    // Use DuoSecurity only when IP test fails.
                    if (sPhoneProgress == "2 Grant")
                    {
                        lblInvalid.Text = "Opening Duo Security...";
                        lblInvalid.Visible = true;
                        upPanelDuo.Visible = true;

                        DuoSecuritySetup();
                    }
                    else
                    {
                        lblInvalid.Text = "IP " + sIP + " not recognized and<br />Remote access not enabled.<br /><br />Please contact MGM support.";
                        lblInvalid.Visible = true;

                        txtUserName.Focus();
                    }
                }
                else
                {
                    // Record that this user logged in.
                    WriteToLog(sUserName);
                    
                    Response.Redirect("Home.aspx");
                    //Response.Redirect("MaintWarning.aspx");
                    //Response.Redirect("MaintDown.aspx");
                }
            }
            else
            {
                if (bUserName == true)
                {
                    lblInvalid.Text = "Error: " + sOutcome == null ? "": sOutcome;
                    lblInvalid.Visible = true;

                    txtUserName.Focus();
                }
                else
                {
                    lblInvalid.Text = "Error: " + sOutcome == null ? "" : sOutcome;
                    lblInvalid.Visible = true;

                    txtPassword.Focus();
                }
                return false;
            }
            
            return true;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SubmitLogin();
        }
 
        protected void DuoSecuritySetup() 
        {
            string sUserName = txtUserName.Text.ToLower();
            
            // Duo Security keys.
            // ==================
            string sAKey = "shkSTwAvzsCa2xX82D7WhwDldSnoQR0zla5Or9Ph";  // "Application" internally generated key, not known to Duo Security.
            string sHost = "api-19435623.duosecurity.com";              // Generic.  Passed on to Javascript in DuoSecurity.aspx.
            string sIKey = "DIJQ1QIOX66I8TTOIIWQ";                      // "Public" identification of this instance.
            string sSignRequest = "";                                   // Completed by DuoSecurity before going to DuoSecurity.aspx.
            string sSKey = "t4X9wYHt9wYdnpA9c8eDlsytpy8IyWV58cia21Bs";  // "Private" key associated with this instance.
            // =======================================================

            // Get signature request from DuoWeb.dll.
            sSignRequest = Duo.Web.SignRequest(sIKey, sSKey, sAKey, sUserName);

            Session["DuoAKey"] = sAKey;                                 // Used in JavaScript for DuoSecurity.
            Session["DuoHost"] = sHost;                                 // Used in JavaScript for DuoSecurity.
            Session["DuoIKey"] = sIKey;                                 // Used in JavaScript for DuoSecurity.
            Session["DuoSignRequest"] = sSignRequest;                   // Used in JavaScript for DuoSecurity.
            Session["DuoSKey"] = sSKey;                                 // Used in JavaScript for DuoSecurity.


            string csScriptName = "DuoScript";
            Type csType = this.GetType();

            ClientScriptManager cs = Page.ClientScript;

            string sScript = "<script src='Scripts/Duo-Web-v1.bundled.js'></script>" +
                            "<script>document.getElementById('loginform').style.display = 'none';" +
                            "Duo.init({'host': '" + sHost +
                            "','sig_request': '" + sSignRequest +
                            "','post_action': 'Home.aspx'});</script>";


            // Register script, complete with server variables, on the client page.
            if (!cs.IsStartupScriptRegistered(csType, csScriptName))
            {
                cs.RegisterStartupScript(csType, csScriptName, sScript, false);
            }
        }


        /// <summary>
        /// Clear the error message when changing user name or password.
        /// </summary>
        protected void ClearErrorMessage()
        {
            lblInvalid.Text = "";
            lblInvalid.Visible = false;
            
        }

        protected void txtUserName_TextChanged(object sender, EventArgs e)
        {
            ClearErrorMessage();
        }

        protected void txtPassword_TextChanged(object sender, EventArgs e)
        {
            ClearErrorMessage();
        }

        /// <summary>
        /// Logs that this user opened Rep Portal.
        /// </summary>
        /// <param name="sUserName"></param>
        protected void WriteToLog(string sUserName)
        {
            string sSql = "INSERT INTO LogUserActivity(UserName) " + Environment.NewLine +
                        "VALUES ('" + sUserName + "')";
            DataLinkCon dlc = DataLinkCon.mgmuser;

            DataLink.Update(sSql, dlc);
        }
    }
}

 
    
    
    
    
