using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace MGM_Transformer
{
    public partial class CustomerBody : System.Web.UI.UserControl
    {
        DataValidation dv = new DataValidation();
        Customer cust = new Customer();
        int _RepID;

        public int RepID
        {
            get { return _RepID; }
            set { _RepID = value; }
        }

        void Session_Start(object sender, EventArgs e)
        {
            Response.Redirect("http://www.mgmtransformer.com");

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["IsLoggedIn"]) == 0)
                Response.Redirect("http://www.mgmtransformer.com");


            RepID = Convert.ToInt32(Session["RepID"]);
                         
         
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void LoadData()
        {
            LoadCustomers();
            SetTitle();
        }


        private void SetDataSet()
        {
            CustomerList.SelectParameters.Clear();

            if (ViewState["searching"] != null)
            {
                if (ViewState["searching"].ToString() == "true")
                {
                    CustomerList.SelectCommand = "usp_Customer_List_Search";
                    CustomerList.SelectParameters.Add("rep_id", Session["RepDistributorID"].ToString());
                    CustomerList.SelectParameters.Add("search_string", ViewState["searchtext"].ToString());
                }
                else
                {
                    CustomerList.SelectCommand = "usp_Customer_List";
                    CustomerList.SelectParameters.Add("rep_id", Session["RepDistributorID"].ToString());
                }
            }
            else
            {
                CustomerList.SelectCommand = "usp_Customer_List";
                CustomerList.SelectParameters.Add("rep_id", Session["RepDistributorID"].ToString());

            }

        }



        protected void LoadCustomers()
        {
            SetDataSet();
            gvCustomerList.DataBind();
        }

        protected void LoadCustomer(int iCustomerContactID)
        {
            DataTable dt = cust.ByCustomerID(iCustomerContactID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                chkObsolete.Checked = Convert.ToBoolean(dr["Obsolete"]);
                txtCustomerID.Text = dr["CustomerID"].ToString();
                txtCustomerContactID.Text = dr["CustomerContactID"].ToString();
                txtCompany.Text = dr["Company"].ToString();
                lblCompany.Text = dr["Company"].ToString();
                txtRepDistributorID.Text = dr["RepDistributorID"].ToString();
                txtRepID.Text = dr["RepID"].ToString();
                txtCity.Text = dr["City"].ToString();
                txtContactName.Text = dr["ContactName"].ToString();
                txtEmail.Text = dr["Email"].ToString();
                

                DisplayEdit(true);
           }
        }

        protected void gvCustomerList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int iCustomerContactID = Convert.ToInt32(e.CommandArgument);

            LoadCustomer(iCustomerContactID);
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            PageValidate("txtEmail");
        }

        protected bool PageValidate(string ctlName)
        {
            lblErrors.Visible = true;

            // Perform validation for one or all controls on the form.
            if (ctlName == "*" || ctlName == "txtEmail")
            {
                TextBox txt = (TextBox)uptxtEmail.FindControl("txtEmail");
                Label lblV = (Label)uptxtEmail.FindControl("lblEmailInvalid");
                lblV.Visible = false;
                string sEmail = txt.Text;
                string sValid = dv.EmailValid(sEmail);
                if (sValid != "")
                {
                    lblV.Text = sValid;
                    lblV.Visible = true;
                    return false;
                }
            }
            if (ctlName == "*" || ctlName == "chkObsolete" || ctlName == "txtCompany")
            {
                // Display errors if making a system customer obsolete or changing the company name.
                bool bSystemCustomer = IsSystemCustomer();

                if (bSystemCustomer)
                {
                    lblInvalidCompany.Visible = false;
                    if (txtCompany.Text != lblCompany.Text)
                    {
                        lblInvalidCompany.Visible = true;
                        return false;
                    }
                    lblInvalidObsolete.Visible = false;
                    if (chkObsolete.Checked)
                    {
                        lblInvalidObsolete.Visible = true;
                        return false;
                    }
                }
            }

            lblErrors.Visible = false;
            return true;
        }

        /// <summary>
        /// Returns True if RepID <> RepDistributorID, indicating it is a System Customer.
        /// </summary>
        /// <returns></returns>
        protected bool IsSystemCustomer()
        {
            TextBox txtRepID = (TextBox) uptxtCompany.FindControl("txtRepID");
            TextBox txtRepDistributorID = (TextBox)uptxtCompany.FindControl("txtRepDistributorID");

            int iRepID = Convert.ToInt32(txtRepID.Text);
            int iRepDistributorID = Convert.ToInt32(txtRepDistributorID.Text);

            if (iRepID != iRepDistributorID)
                return true;

            return false;
        }
        
         // Called to save the work done so far.
        protected bool Save()
        {
            if (!PageValidate("*"))
                return false;

            int iRepID = Convert.ToInt32(txtRepID.Text);
            int iRepDistributorID = Convert.ToInt32(txtRepDistributorID.Text);
            int iObsolete = (chkObsolete.Checked == true) ? 1 : 0;
            int iCustomerID = Convert.ToInt32(txtCustomerID.Text);
            int iApplyAll = (chkChgAllContacts.Checked == true) ? 1 : 0;
            int iCustomerContactID = Convert.ToInt32(txtCustomerContactID.Text);
            string sCompany = txtCompany.Text.ToString();
            string sCity = txtCity.Text.ToString();
            string sContactName = txtContactName.Text.ToString();
            string sEmail = txtEmail.Text.ToString();

            if (!cust.Update(iCustomerID, iApplyAll, iCustomerContactID, iObsolete, sCompany, sCity, sContactName, sEmail))
                return false;

            return true;
        }

        protected void SetTitle()
        {
            txtRepName.Text = Session["RepName"].ToString();
            lblCustomers.Text = txtRepName.Text.ToString() + " Customers";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            DisplayEdit(false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                DisplayEdit(false);
                LoadCustomers();
            }
            chkChgAllContacts.Checked = false;
        }

        protected void chkObsolete_CheckedChanged(object sender, EventArgs e)
        {
            if (PageValidate("chkObsolete"))
                DisplayEdit(true);
        }

        protected void DisplayEdit(bool bDisplay)
        {
            pnlEditCust.Visible = bDisplay;
            pnlRow.Visible = bDisplay;

            if (bDisplay == true)
            {
                bool bEdit = (chkObsolete.Checked == true) ? false : true;

                txtCompany.Enabled = bEdit;
                txtCity.Enabled = bEdit;
                txtContactName.Enabled = bEdit;
                txtEmail.Enabled = bEdit;
            }
            else
            {
                lblInvalidCompany.Visible = false;
                lblInvalidObsolete.Visible = false;
                lblErrors.Visible = false;
            }
        }

        protected void gvCustomerList_PageIndexChanged(object sender, EventArgs e)
        {
            SetDataSet();
            DisplayEdit(false);
        }

        protected void txtCompany_TextChanged(object sender, EventArgs e)
        {
            PageValidate("txtCompany");
        }

        protected void gvCustomerList_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (tbSearchCompany.Text == "")
                return;

            CustomerList.SelectCommand = "usp_Customer_List_Search";
            CustomerList.SelectParameters.Clear();
            CustomerList.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            CustomerList.SelectParameters.Add("search_string", tbSearchCompany.Text);

            if (tbSearchCompany.Text == "*")
            {
                if (Session["Internal"].ToString() == "1")
                    CustomerList.SelectParameters.Add("rep_id", "-1");
                else
                    CustomerList.SelectParameters.Add("rep_id", Convert.ToInt32(Session["RepID"]).ToString());
            }
            else
            {
                CustomerList.SelectParameters.Add("rep_id", Convert.ToInt32(Session["RepID"]).ToString());
            }
            
            gvCustomerList.DataBind();

            //hfSearching.Value = "true";
            //Session["searching"] = true;
            ViewState["searching"] = "true";
            ViewState["searchtext"] = tbSearchCompany.Text;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            CustomerList.SelectCommand = "usp_Customer_List";
            CustomerList.SelectParameters.Clear();
            CustomerList.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            CustomerList.SelectParameters.Add("rep_id", Convert.ToInt32(Session["RepID"]).ToString());

            gvCustomerList.DataBind();

            tbSearchCompany.Text = "";

            //hfSearching.Value = "false";
            //Session["searching"] = false;
            ViewState["searching"] = "false";
            ViewState["searchtext"] = "";
        }
    }
}