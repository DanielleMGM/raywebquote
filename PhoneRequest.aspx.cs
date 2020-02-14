using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace MGM_Transformer
{
    public partial class PhoneRequest : System.Web.UI.Page
    {
        DataValidation dv = new DataValidation();
        RepObject r = new RepObject();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void LoadData()
        {
            string sUserName = Session["UserName"].ToString();
            string sFullName = Session["FullName"].ToString();
            string sEmail = Session["Email"].ToString();
            string sPhone = Session["Phone"].ToString();
            string sPhoneType = Session["PhoneType"].ToString();
            string sPassword = Session["Password"].ToString();

            txtUserName.Text = sUserName;
            txtFullName.Text = sFullName;
            txtEmail.Text = sEmail;
            txtPassword.Text = sPassword;
            txtPhone.Text = sPhone;

            if (sPhoneType != "")
            {
                rblPhoneType.SelectedValue = sPhoneType;
            }

            // Load Rep Name dropdown.
            LoadRep();

            // Get the specifics for this user name.
            LoadLogin(sUserName);

            // Show data related to phone type.
            ShowControls();

        }

        /// <summary>
        /// Load Rep Name dropdown.
        /// </summary>
        /// <param name="iRepID"></param>
        protected void LoadRep()
        {
            string sUserName = Session["UserName"].ToString();

            int iRepID = Convert.ToInt32(Session["RepID"]);
            DataTable dtRep = r.GetRepID(sUserName);
            if (dtRep.Rows.Count > 0)
            {
                iRepID = Convert.ToInt32(dtRep.Rows[0]["RepID"]);
            }
            ddlRep.Items.Clear();

            Quotes q = new Quotes();

            DataTable dt = q.RepName(iRepID);

            ddlRep.DataTextField = "FullName";
            ddlRep.DataValueField = "UserName";

            ddlRep.DataSource = dt;
            ddlRep.DataBind();

            ddlRep.SelectedValue = sUserName;
            txtUserName.Text = sUserName;
        }

        /// <summary>
        /// After changing Login, load stored information.
        /// </summary>
        /// <param name="sUserName"></param>
        protected void LoadLogin(string sUserName)
        {
            RepObject r = new RepObject();
            DataSet ds = r.PhoneAccessData(sUserName);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                Session["FullName"] = dr["FullName"].ToString();
                Session["Password"] = dr["Password"].ToString();
                Session["Email"] = dr["Email"].ToString();
                Session["Phone"] = dr["Phone"].ToString();
                Session["PhoneProgress"] = dr["PhoneProgressCode"].ToString();
                Session["PhoneType"] = dr["PhoneType"].ToString();
            }

            txtFullName.Text = Session["FullName"].ToString();
            txtPassword.Text = Session["Password"].ToString();
            txtEmail.Text = Session["Email"].ToString();
            txtPhone.Text = Session["Phone"].ToString();
            chkNewUser.Checked = false;

            ShowControls();
        }

        protected void ShowControls()
        {
            string sPhoneType = Session["PhoneType"].ToString().Trim();
            string sPhoneProgress = Session["PhoneProgress"].ToString().Trim();

            btnRequestAccess.Visible = (sPhoneProgress == "2 Grant") ? false : true;

            switch (sPhoneType)
            {
                case "iPhone":

                    lblCodeReaderInstructions.Visible = true;

                    iphoneReader.Visible = true;
                    androidReader.Visible = false;

                    switch (sPhoneProgress)
                    {
                        case "1 Request":
                            lblRequested.Visible = true;

                            lblNotAvailable.Visible = false;
                            iphoneInstall.Visible = false;
                            androidInstall.Visible = false;
                            break;
                        case "2 Grant":
                            iphoneInstall.Visible = true;

                            lblNotAvailable.Visible = false;
                            lblRequested.Visible = false;
                            androidInstall.Visible = false;
                            break;
                        default:
                            lblNotAvailable.Visible = true;

                            lblRequested.Visible = false;
                            iphoneInstall.Visible = false;
                            androidInstall.Visible = false;
                            break;
                    }
                    break;

                case "Android":
                case "Blackberry":
                case "Windows":

                    lblCodeReaderInstructions.Visible = true;
                    iphoneReader.Visible = false;
                    androidReader.Visible = true;
                    
                    switch (sPhoneProgress)
                    {
                        case "1 Request":
                            lblRequested.Visible = true;

                            lblNotAvailable.Visible = false;
                            iphoneInstall.Visible = false;
                            androidInstall.Visible = false;
                            break;
                        case "2 Grant":
                            androidInstall.Visible = true;

                            lblNotAvailable.Visible = false;
                            lblRequested.Visible = false;
                            iphoneInstall.Visible = false;
                            break;
                        default:
                            lblNotAvailable.Visible = true;

                            lblRequested.Visible = false;
                            iphoneInstall.Visible = false;
                            androidInstall.Visible = false;
                            break;
                    }
                    break;

                case "Other":

                    lblCodeReaderInstructions.Visible = true;
                   
                    iphoneReader.Visible = false;
                    androidReader.Visible = true;
                    
                    lblNotAvailable.Visible = true;
                    lblRequested.Visible = false;
                    iphoneInstall.Visible = false;
                    androidInstall.Visible = false;

                    break;

                default:
                    
                    lblCodeReaderInstructions.Visible = false;

                    iphoneReader.Visible = false;
                    androidReader.Visible = false;
                    
                    lblNotAvailable.Visible = true;
                    lblRequested.Visible = false;
                    iphoneInstall.Visible = false;
                    androidInstall.Visible = false;

                    break;
            }
        }

        protected void btnRequestAccess_Click(object sender, EventArgs e)
        {
            // Require the fields.
            lblStatus.Text = "";
            lblStatus.Visible = false;
            string sErrors = "";

            string sPhoneType = Session["PhoneType"].ToString();
            if (String.IsNullOrEmpty(sPhoneType) == true)
            {
                sErrors = sErrors + "Phone Type";
            }
            if (String.IsNullOrEmpty(txtUserName.Text.ToString()) == true)
            {
                if (sErrors != "") sErrors = sErrors + ", ";
                sErrors = sErrors + "Login ID";
            }
            if (String.IsNullOrEmpty(txtFullName.Text.ToString()) == true)
            {
                if (sErrors != "") sErrors = sErrors + ", ";
                sErrors = sErrors + "Full Name";
            }
            if (String.IsNullOrEmpty(txtPassword.Text.ToString()) == true)
            {
                if (sErrors != "") sErrors = sErrors + ", ";
                sErrors = sErrors + "Password";
            }
            if (String.IsNullOrEmpty(txtEmail.Text.ToString()) == true)
            {
                if (sErrors != "") sErrors = sErrors + ", ";
                sErrors = sErrors + "Email";
            }
            if (String.IsNullOrEmpty(txtPhone.Text.ToString()) == true)
            {
                if (sErrors != "") sErrors = sErrors + ", ";
                sErrors = sErrors + "Phone";
            }

            if (sErrors != "") {
                sErrors = sErrors + " missing. Please correct.";

                lblStatus.Text = sErrors;
                lblStatus.Visible = true;
                return;
            } 

            // Assert:  We have all the fields we need.

            RepObject r = new RepObject();

            string sUserName = txtUserName.Text;
            string sFullName = txtFullName.Text;
            int iRepID = Convert.ToInt32(Session["RepID"]);
            int iIsNew = chkNewUser.Checked ? 1 : 0;
            string sPassword = txtPassword.Text.ToString();
            string sEmail = txtEmail.Text.ToString();
            string sPhone = txtPhone.Text.ToString();

            // Request phone access.
            if (r.PhoneAccessRequest(sUserName, sFullName, iRepID, iIsNew, sPassword, sEmail, sPhoneType, sPhone) == -1)
            {
                lblStatus.Text = "Request for mobile phone access submitted.";
            }
            else {
                lblStatus.Text = "There was an ERROR in your request for mobile phone access.  Please contact MGM.";
            }
            lblStatus.Visible = true;
        }

        protected void btnPhoneOther_Click(object sender, EventArgs e)
        {
            Session["PhoneType"] = "Other";
            ShowControls();
        }

        protected void ddlRep_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUserName.Text = ddlRep.SelectedValue;
            string sUserName = txtUserName.Text.ToString();

            chkNewUser.Checked = false;
            CheckIfUserFound();

            // Get all relevant information.
            LoadLogin(sUserName);
        }

        protected void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // Produces an error message if what was entered is invalid.
            string sPasswordError = dv.PasswordValid(txtPassword.Text.ToString());

            lblStatus.Text = sPasswordError;
            if (sPasswordError != "")
            {
                txtPassword.Focus();
            }
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            // Produces an error message if what was entered is invalid.
            string sEmailError = dv.EmailValid(txtEmail.Text.ToString());

            lblStatus.Text = sEmailError;
            if (sEmailError != "")
            {
                txtEmail.Focus();
            }
        }

        protected void txtPhone_TextChanged(object sender, EventArgs e)
        {
            // Converts what was entered into a standard phone format.
            string sPhone = dv.PhoneFmt(txtPhone.Text);
            string sPhoneError = "";

            if (sPhone == "") sPhoneError = "Invalid phone number. Expect: (999) 999-9999 x9999";

            lblStatus.Text = sPhoneError;
            if (sPhoneError != "")
            {
                txtPhone.Focus();
            }
            else
            {
                // Convert what was entered into nicely formatted phone number.
                txtPhone.Text = sPhone;
            }
        }

        protected void txtUserName_TextChanged(object sender, EventArgs e)
        {
            CheckIfUserFound();

            // Default Full Name to User Name.
            string sUserName = txtUserName.Text.ToString();
            txtFullName.Text = sUserName;
        }

        /// <summary>
        /// Prevent marking someone as New if user name is in the Rep list.
        /// </summary>
        /// <param name="sUserName"></param>
        /// <returns></returns>
        protected void CheckIfUserFound()
        {
            lblStatus.Visible = false;
            string sUserName = txtUserName.Text.ToString();

            if (string.IsNullOrEmpty(sUserName) == true)
                return;

            for (int i = 0; i < ddlRep.Items.Count; i++)
            {
                if (ddlRep.Items[i].Value == sUserName  && chkNewUser.Checked == true)
                {
                    chkNewUser.Checked = false;
                    lblStatus.Text = sUserName + " is already defined.";
                    lblStatus.Visible = true;
                }
            }
        }

        protected void chkNewUser_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfUserFound();
        }

        protected void rblPhoneType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PhoneType"] = rblPhoneType.SelectedValue;
            ShowControls();
        }

     }
}