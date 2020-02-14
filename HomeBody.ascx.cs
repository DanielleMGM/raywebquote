using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace MGM_Transformer
{

    public partial class HomeBody : System.Web.UI.UserControl
    {
        
        DataValidation dv = new DataValidation();
        RepObject r = new RepObject();
        Quotes q = new Quotes();
        protected string msProgressCode = "";
        protected string msLostReasonCode = "";

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
                tbSearchCompany.Focus();

                int iRepID = Convert.ToInt32(Session["RepID"]);
                int iRepDistributorID = Convert.ToInt32(Session["RepDistributorID"]);


                Session["QuoteNo"] = 0;
                Session["QuoteNoVer"] = 0;


                OrdersSorted.Value = "False";
                OrdersSortedDirection.Value = "DESC";
                OrdersCompanySearch.Value = "";
                OrdersSortedColumn.Value = "";

                QuotesSorted.Value = "False";
                QuotesSortedDirection.Value = "DESC";
                QuotesCompanySearch.Value = "";
                QuotesSortedColumn.Value = "";


                lblFullName.Text = Session["FullName"].ToString();

                if (Convert.ToInt32(Session["Internal"]) == 0)
                    cbPendingApproval.Visible = false;
                else
                    cbPendingApproval.Visible = true;

                ShowPendingApprovals.Value = false.ToString();

                LoadLogins();

                LoginFilterLoad();

                Display();

                hfCurrentVisibleGrid.Value = "Quotes";

                if(Convert.ToInt32(Session["Admin"]) != 1) 
                   cbPendingEmails.Visible = false;

            }
           
        }

        // Logins only available to Internal users.
        protected void LoadLogins()
        {
            string[] sExcludedAgents = { };
            
            bool bAdmin = Convert.ToBoolean(Session["Admin"]);

            bool bInternal = Convert.ToBoolean(Session["Internal"]);
            if (!bInternal && !bAdmin)
            {
                lblLogin.Visible = false;
                ddlLogin.Visible = false;
                return;
            }
            lblLogin.Visible = true;
            ddlLogin.Visible = true;

            ddlLogin.Items.Clear();

            // Passing in zero gives a list of all distributors, or logins.
            DataTable dt = r.Distributors(0);
            ddlLogin.DataTextField = "Distributor";
            ddlLogin.DataValueField = "RepID";

            //if (WebConfigurationManager.AppSettings["ExcludeRepId"] != null)
            //{
            //    sExcludedAgents = WebConfigurationManager.AppSettings["ExcludeRepId"].ToString().Split(';');
            //}

            //foreach (string sExclude in sExcludedAgents)
            //{
            //    if (sExclude != "")
            //        dt = dt.AsEnumerable().Select(row => row).Where(d => Convert.ToInt32(d["RepId"]) != Convert.ToInt32(sExclude)).Select(v => v).CopyToDataTable();
            //}



            ddlLogin.DataSource = dt;
            ddlLogin.DataBind();

            int iRepDistributorID = Convert.ToInt32(Session["RepID"]);
            ddlLogin.SelectedValue = iRepDistributorID.ToString();
        }

        protected void LoadCompanies(int RepDistributorID, string sUserName)
        {
           

            DataTable dt = r.Companies(RepDistributorID,sUserName);

            if (dt.Rows.Count == 0)
            {
              
                Session["CustomerID"] = 0;
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr["Company"] = "";
                dr["CustomerID"] = 0;
                dt.Rows.InsertAt(dr, 0);


                string sCustomerID = Session["CustomerID"].ToString();
                if (sCustomerID == null || sCustomerID == "")
                {
                    sCustomerID = "0";
                }
                int iCustomerID = Convert.ToInt32(sCustomerID);

            }

        }

        protected void LoadQuotes(bool bSearch = false,string sSearchString = "")
        {
            SetTitle();

            int iRepID = Convert.ToInt32(Session["RepID"]);
            int iCustomerID = Convert.ToInt32(Session["CustomerID"]);
            int iQuoteNo = 0;
            string sUserName = Session["UserNameFilter"].ToString();

            int iActiveRowCount = 0;
            int iHistoryRowCount = 0;
            
            string sViewOption = rblOrder.SelectedValue;

            switch (sViewOption)
            {
                case "Active":
                
                    LoadGrid(true,"",bSearch);
                    // Returns 0 if there is one dummy record.
                    iActiveRowCount = dv.GridCount(gvQuoteActive);
                    
                    break;
                
                case "Orders":

                    LoadOrders(false,iCustomerID, iQuoteNo, iRepID, sUserName,sSearchString,OrdersSortedColumn.Value);
                    
                    break;
            }

            // Switch to All quotes if the quote number being searched for isn't active.
            if (iHistoryRowCount > 0 && iActiveRowCount == 0 && rblOrder.SelectedIndex == 1)
            {
                rblOrder.SelectedIndex = 0;
                Display();
            }
        }

        protected void LoadOrders(bool bFlipSortDirection, int iCustomerID, int iQuoteNo, int iRepID, string sUserName,string sSearchCompany, string sSortColumn = "")
        {
            int iInternal = Int32.Parse(Session["Internal"].ToString());
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            ActivePanel.Visible = false;
            OrderPanel.Visible = true;
            pnlPendingEmails.Visible = false;
            cbPendingEmails.Checked = false;




            if (OrdersCompanySearch.Value ==  "")
            {
                sSearchCompany = "";
                gvOrderHistory.Columns[gvOrderHistory.Columns.Count - 1].Visible = false;
            }
            else
            {
                sSearchCompany = OrdersCompanySearch.Value;
                gvOrderHistory.Columns[gvOrderHistory.Columns.Count - 1].Visible = true;
            }


            sb.Append("declare @begin_date datetime" + Environment.NewLine);
            sb.Append("declare @external bit" + Environment.NewLine);
            sb.Append("declare @freight_included int" + Environment.NewLine);
            sb.Append("declare @quote_search bit" + Environment.NewLine);
            sb.Append("declare @customer_id int" + Environment.NewLine);
            sb.Append("declare @search_company varchar(50)" + Environment.NewLine);
            sb.Append("declare @quote_no int" + Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("\t set @customer_id = " + iCustomerID + Environment.NewLine);
            sb.Append("\t set @search_company = " + (sSearchCompany == "" ? "''" : "'" + sSearchCompany + "'") + Environment.NewLine);
            sb.Append("\t set @search_company = isnull(@search_company, '')" + Environment.NewLine);
            sb.Append("\t set @external = 0" + Environment.NewLine);
            sb.Append("\t set @quote_search = 0" + Environment.NewLine);
            sb.Append("\t set @quote_no = " + iQuoteNo + Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("\t if @quote_no > 0" + Environment.NewLine);
            sb.Append("\t\t begin" + Environment.NewLine);
            sb.Append("\t\t\t if exists(select *" + Environment.NewLine);
            sb.Append("\t\t\t\t from Rep" + Environment.NewLine);
            sb.Append("\t\t\t\t where RepID = " + iRepID + Environment.NewLine);
            sb.Append("\t\t\t\t and     SecurityLevel = 'External Rep')" + Environment.NewLine);
            sb.Append("\t\t\t begin" + Environment.NewLine);
            sb.Append("\t\t\t\t set @external = 1" + Environment.NewLine);
            sb.Append("\t\t\t end" + Environment.NewLine);
            sb.Append(Environment.NewLine);

            sb.Append("\t\t\t set @quote_search = 1" + Environment.NewLine);
            sb.Append("\t\t\t set @customer_id = 0" + Environment.NewLine);
            sb.Append("\t end" + Environment.NewLine);
          

            sb.Append("\t if not exists(select *" + Environment.NewLine);
            sb.Append("\t\t from Quote" + Environment.NewLine);
            sb.Append("\t\t where RepID = " + iRepID + ")" + Environment.NewLine);
            sb.Append("\t begin" + Environment.NewLine);
            sb.Append("\t\t return" + Environment.NewLine);
            sb.Append("\t end" + Environment.NewLine);

            sb.Append(Environment.NewLine);

            sb.Append("\t select q.QuoteID" + Environment.NewLine);
            sb.Append("\t\t ,c.Company" + Environment.NewLine);
            sb.Append("\t\t ,dbo.uf_QuoteNo(q.QuoteId, q.QuoteNo, 1, q.QuoteOriginCode) as QuoteNo" + Environment.NewLine);
            sb.Append("\t\t ,q.QuoteNoVer" + Environment.NewLine);
            sb.Append("\t\t ,isnull(cst.Company, '') + case isnull(cst.Company, '') when '' then 'No Company'" + Environment.NewLine);
            sb.Append("\t\t\t else case isnull(q.ProjectName, '') when '' then '' else ' - ' end  end + isnull(q.ProjectName, '') + " + Environment.NewLine);
            sb.Append("\t\t\t case isnull(cc.City, '') when '' then '' else ' - ' + cc.City end CompanyProjectCity" + Environment.NewLine);
            sb.Append("\t\t ,o.ShippingInstructions as ShippingInfo" + Environment.NewLine);
            sb.Append("\t\t ,o.SalesOrderNo" + Environment.NewLine);
            sb.Append("\t\t ,o.PurchaseOrderNo" + Environment.NewLine);
            sb.Append("\t\t ,o.ShipDate" + Environment.NewLine);
            sb.Append("\t\t ,o.StatusCode as [Status]" + Environment.NewLine);
            sb.Append("\t\t ,q.Notes" + Environment.NewLine);
            sb.Append("\t\t ,left(isnull(q.Notes, ''), 40) + case when len(isnull(q.Notes, '')) > 40 then '...' else '' end as NotesShort" + Environment.NewLine);
            sb.Append("\t\t ,o.Total as TotalPrice" + Environment.NewLine);
            sb.Append("\t\t ,q.UserName " + Environment.NewLine);

            sb.Append("\t\t ,r.Display_Name" + Environment.NewLine);

            sb.Append("\t from \t\t Quote q " + Environment.NewLine);
            sb.Append("\t join \t\t Customer cst " + Environment.NewLine);
            sb.Append("\t on \t\t\t q.CustomerID = cst.CustomerID " + Environment.NewLine);

            sb.Append("\t join \t\t Rep r" + Environment.NewLine);
            sb.Append("\t on \t\t\t q.RepID = r.RepID" + Environment.NewLine);


            sb.Append("\t left join \t CustomerContacts cc " + Environment.NewLine);
            sb.Append("\t on \t\t\t q.CustomerContactID = cc.CustomerContactID " + Environment.NewLine);
            sb.Append("\t join \t\t Logins l " + Environment.NewLine);
            sb.Append("\t on \t\t\t q.UserName = l.UserName " + Environment.NewLine);
            sb.Append("\t join  \t\t (select QuoteID, sum(price * quantity) as TotalPrice from QuoteDetails group by QuoteID) qdsum " + Environment.NewLine);
            sb.Append("\t on \t\t\t q.QuoteID = qdsum.QuoteID " + Environment.NewLine);
            sb.Append("\t join \t\t (select  QuoteID " + Environment.NewLine);
            sb.Append("\t\t\t\t\t\t ,SalesOrderNo" + Environment.NewLine);
            sb.Append("\t\t\t\t\t\t ,min(OrderDate) as OrderDate" + Environment.NewLine);
            sb.Append("\t\t\t\t\t\t ,min(PurchaseOrderNo) as PurchaseOrderNo" + Environment.NewLine);
            sb.Append("\t\t\t\t\t\t ,sum(isnull(Qty, 1)) as Qty" + Environment.NewLine);
            sb.Append("\t\t\t\t\t\t ,ShipDate" + Environment.NewLine);
            sb.Append("\t\t\t\t\t\t ,ShippingInstructions" + Environment.NewLine);
            sb.Append("\t\t\t\t\t\t ,StatusCode" + Environment.NewLine);
            sb.Append("\t\t\t\t\t\t ,sum(isnull(Total, 0)) as Total " + Environment.NewLine);
            sb.Append("\t\t\t\t from \t Orders " + Environment.NewLine);
            sb.Append("\t\t\t\t group by QuoteID" + Environment.NewLine);
            sb.Append("\t\t\t\t\t\t ,SalesOrderNo" + Environment.NewLine);
            sb.Append("\t\t\t\t\t\t ,ShipDate" + Environment.NewLine);
            sb.Append("\t\t\t\t\t\t ,ShippingInstructions" + Environment.NewLine);
            sb.Append("\t\t\t\t\t\t ,StatusCode) as o " + Environment.NewLine);
            sb.Append("\t on \t\t q.QuoteID = o.QuoteID " + Environment.NewLine);
            sb.Append("\t join \t Customer c " + Environment.NewLine);
            sb.Append("\t on      q.CustomerID = c.CustomerID " + Environment.NewLine);



            sb.Append("\t where \t isnull(q.QuoteNo, 0) = case @quote_search when 0 then isnull(q.QuoteNo, 0) else " + iQuoteNo + " end " + Environment.NewLine);
            sb.Append("\t and \t q.CustomerID = case " + iCustomerID + " when 0 then q.CustomerID else " + iCustomerID + " end " + Environment.NewLine);

            if (sSearchCompany == "" || iInternal == 0)
            {
                sb.Append("\t and \t q.RepID = case @quote_search when 0 then " + iRepID + " else case @external when 1 then " + iRepID + " else q.RepID end end " + Environment.NewLine);
                sb.Append("\t and \t q.UserName = case isnull('" + sUserName + "', '') when '' then q.UserName when '0' then q.UserName else '" + sUserName + "' end " + Environment.NewLine);
            }
            else
            {
                sb.Append("\t and \t (isnull(c.Company, '') like case @search_company when '' then c.Company else '%' + @search_company + '%' end " + Environment.NewLine);
                sb.Append("\t\t\t or \t\tisnull(q.ProjectName, '') like case @search_company when '' then q.ProjectName else '%' + @search_company + '%' end " + Environment.NewLine);
                sb.Append("\t\t\t or \t\tisnull(cc.City, '') like case @search_company when '' then cc.City else '%' + @search_company + '%' end " + Environment.NewLine);
                sb.Append("\t\t\t  or q.QuoteNo = " + (Regex.Match(sSearchCompany, @"\d+").Value == "" ? "''" : Regex.Match(sSearchCompany, @"\d+").Value)  + ")" + Environment.NewLine);
                sb.Append("\t\t\t  or q.QuoteNo like '%' + @search_company + '%'" + Environment.NewLine);
                sb.Append("\t\t\t  or o.PurchaseOrderNo like '%' + @search_company + '%'" + Environment.NewLine);
            }



            if (sSortColumn == "")
            {
                sb.Append("\t order by \t q.QuoteNo desc " + Environment.NewLine);
                sb.Append("\t\t\t\t ,q.QuoteNoVer desc " + Environment.NewLine);
                sb.Append("\t\t\t\t ,q.QuoteID desc" + Environment.NewLine);
            }
            else
            {
                if (bFlipSortDirection)
                    OrdersSortedDirection.Value = OrdersSortedDirection.Value == "ASC" ? "DESC" : "ASC";

                sb.Append(" order by	" + sSortColumn + " " + OrdersSortedDirection.Value + Environment.NewLine);
            }

            dsOrders.SelectCommand = sb.ToString();
            gvOrderHistory.DataSource = dsOrders;
            gvOrderHistory.DataBind();
            
        }

        /// <summary>
        /// Called when editing a grid.
        /// </summary>
        /// <param name="sGridName"></param>
        protected void LoadGrid(bool bFlipSortDir = true, string sSortColumn = "", bool bSearch = false, string sCustomerSearch = "", 
                                          bool bCustomerSearch = false, bool bShowPendingApp = false)
        {

            ActivePanel.Visible = true;
            OrderPanel.Visible = false;
            pnlPendingEmails.Visible = false;
            cbPendingEmails.Checked = false;


            if (QuotesCompanySearch.Value == "")
            {
                sCustomerSearch = "";
                bCustomerSearch = false;

                if(bShowPendingApp)
                   gvQuoteActive.Columns[gvQuoteActive.Columns.Count - 1].Visible = true;
                else
                   gvQuoteActive.Columns[gvQuoteActive.Columns.Count - 1].Visible = false;
            }
            else
            {
                sCustomerSearch = QuotesCompanySearch.Value;
                bCustomerSearch = true;
                gvQuoteActive.Columns[gvQuoteActive.Columns.Count - 1].Visible = true;
            }

            int iRepID = Convert.ToInt32(Session["RepID"]);
            int iCustomerID = Convert.ToInt32(Session["CustomerID"]);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            int iQuoteNo = 0;
            int.TryParse("0", out iQuoteNo);
            string sUserName = Session["UserNameFilter"].ToString();
            int iInternal = Int32.Parse(Session["Internal"].ToString());
            string sSearchRepID = "";

            if (bSearch)
            {
                sSearchRepID = q.GetRepIDFromQuoteID("0");
                sUserName = "";
                rblLoginFilter.SelectedIndex = rblLoginFilter.Items.IndexOf(rblLoginFilter.Items.FindByText("ALL"));
                ddlLogin.SelectedIndex = ddlLogin.Items.IndexOf(ddlLogin.Items.FindByValue(sSearchRepID));
            }

            if (rblLoginFilter.SelectedIndex == rblLoginFilter.Items.IndexOf(rblLoginFilter.Items.FindByText("ALL")))
                sUserName = "";

            sb.Append("declare @search_company varchar(50)" + Environment.NewLine);

            sCustomerSearch = sCustomerSearch.Replace("'", "''");

            sb.Append("set @search_company = " + (sCustomerSearch == "" ? "''" : "'" + sCustomerSearch + "'") + Environment.NewLine);
 
            sb.Append("select q.QuoteID" + Environment.NewLine);

            sb.Append(",c.Company" + Environment.NewLine);
            sb.Append(",r.RepID" + Environment.NewLine);

            sb.Append(",dbo.uf_QuoteNo(q.QuoteId, q.QuoteNo, 1, q.QuoteOriginCode) as QuoteNo" + Environment.NewLine);
            sb.Append(",q.QuoteNoVer" + Environment.NewLine);

            sb.Append(",q.ProjectName" + Environment.NewLine);

            sb.Append(",q.ApprovalReqd" + Environment.NewLine);
            sb.Append(",q.ApprovalRequested" + Environment.NewLine);
            sb.Append(",q.ApprovedOrDenied" + Environment.NewLine);


            sb.Append(",case isnull(q.Finalized_on,'1/1/1900') when '1/1/1900' then q.Created_on else q.Finalized_on end as QuoteDate" + Environment.NewLine);
            sb.Append(",case isnull(l.FullName,'') when '' then " + Environment.NewLine);
            sb.Append("case isnull(l2.FullName,'') when '' then r.Display_Name	else l2.FullName end " + Environment.NewLine);
            sb.Append("else l.FullName end as RepName" + Environment.NewLine);
            sb.Append(",cc.ContactName" + Environment.NewLine);
            sb.Append(",case isnull(q.ProgressCode,'') when '' then " + Environment.NewLine);
            sb.Append("case q.[Status] when 'PendAppr' then 'PendAppr'" + Environment.NewLine);
            sb.Append(" when 'Finalized' then 'Finalized'" + Environment.NewLine);
            sb.Append(" when 'Cart' then 'Cart' " + Environment.NewLine);
            sb.Append(" when 'PENDING' then 'ORDERED' when 'HOLD' then 'ORDERED' when 'RELEASE' then 'ORDERED' " + Environment.NewLine);
            sb.Append(" when 'CANCEL' then 'CANCELED' when 'COMPLETE' then 'SHIPPED' when 'Ordered' then 'Ordered' end " + Environment.NewLine);
            sb.Append("else q.ProgressCode end CurrentStatus " + Environment.NewLine);
            sb.Append(",case isnull(q.ProgressCode,'') when '' then q.[Status] else q.ProgressCode end as [Status]" + Environment.NewLine);
            sb.Append(",isnull(cst.Company, '') + case isnull(cst.Company,'') when '' then 'No Company' " + Environment.NewLine);
            sb.Append(" else case isnull(q.ProjectName,'') when '' then '' else ' - ' end  end + isnull(q.ProjectName,'') +" + Environment.NewLine);
            sb.Append(" case isnull(cc.City,'') when '' then '' else ' - ' + cc.City end CompanyProjectCity" + Environment.NewLine);
            sb.Append(",q.Notes" + Environment.NewLine);
            sb.Append(",case isnull(q.ApprovalReqd,0) when 0 then qdsum.TotalPrice " + Environment.NewLine);
            sb.Append(" else case q.[Status] when 'Cart' then null when 'PendAppr'then null else qdsum.TotalPrice end " + Environment.NewLine);
            sb.Append("end as TotalPrice" + Environment.NewLine);
            sb.Append(",q.UserName" + Environment.NewLine);
            sb.Append(",q.ProgressCode" + Environment.NewLine);
            sb.Append(",q.LostReasonCode" + Environment.NewLine);
            sb.Append(",q.LostToCode" + Environment.NewLine);

            sb.Append(",r.Display_Name" + Environment.NewLine);

            sb.Append(",convert(varchar(10), q.FollowupDate, 1) as FollowupDate" + Environment.NewLine);
            sb.Append(",'" + sUserName + "'" + Environment.NewLine);
            sb.Append(" from		Quote q" + Environment.NewLine);
            sb.Append(" left join	Rep r" + Environment.NewLine);
            sb.Append(" on			q.RepID = r.RepID" + Environment.NewLine);
            sb.Append(" left join	Customer cst" + Environment.NewLine);
            sb.Append(" on			q.CustomerID = cst.CustomerID" + Environment.NewLine);
            sb.Append(" left join	CustomerContacts cc" + Environment.NewLine);
            sb.Append(" on			q.CustomerContactID = cc.CustomerContactID" + Environment.NewLine);
            sb.Append(" left join	Logins l" + Environment.NewLine);
            sb.Append(" on			q.UserName = l.UserName" + Environment.NewLine);
            sb.Append(" left join	Logins l2" + Environment.NewLine);
            sb.Append(" on			q.UserNameLast = l2.UserName" + Environment.NewLine);
            sb.Append(" left join	(select QuoteID, sum(price * quantity) as TotalPrice from QuoteDetails group by QuoteID) qdsum" + Environment.NewLine);
            sb.Append(" on			q.QuoteID = qdsum.QuoteID" + Environment.NewLine);

            sb.Append(" join Customer c" + Environment.NewLine);
            sb.Append(" on q.CustomerID = c.CustomerID" + Environment.NewLine);

           
            sb.Append(" where		isnull(q.QuoteNo,0) = case " + (iQuoteNo == 0 ? "0" : "1") + " when 0 then isnull(q.QuoteNo,0) else 0 end" + Environment.NewLine);

            if(!bShowPendingApp)
                sb.Append(" and			q.CustomerID = case " + iCustomerID + " when 0 then q.CustomerID else " + iCustomerID + " end" + Environment.NewLine);
           
            if (!bCustomerSearch || iInternal == 0)
            {
                if (!bShowPendingApp)
                {
                    sb.Append(" and			q.RepID = case " + (bSearch ? iInternal.ToString() : "0") + " when 0 then " + iRepID + " else q.RepID end " + Environment.NewLine);
                    sb.Append(" and			q.UserName = case isnull('" + sUserName + "','') when '' then q.UserName when '0' then q.UserName else '" + sUserName + "' end" + Environment.NewLine);
                }
            }
            
            if (bCustomerSearch)
            {
                sb.Append(" and (c.Company like '%' + @search_company + '%'" + Environment.NewLine);
                sb.Append(" or q.ProjectName like '%' + @search_company + '%'" + Environment.NewLine);
                sb.Append(" or cc.City like '%' + @search_company + '%'" + Environment.NewLine);
                sb.Append(" or q.QuoteNo = " + (Regex.Match(sCustomerSearch, @"\d+").Value == "" ? "''" : Regex.Match(sCustomerSearch, @"\d+").Value) + ")" + Environment.NewLine);
                sb.Append(" or q.QuoteNo like '%' + @search_company + '%'" + Environment.NewLine);
                sb.Append(iInternal == 1 ? " and r.RepID = r.RepID" : " and r.RepID = " + iRepID);
            }

            if (bShowPendingApp)
            {
                sb.Append(" and q.[Status] = 'PendAppr'" + Environment.NewLine);
                sb.Append(" and r.RepID <> 74" + Environment.NewLine);
                sb.Append(" and r.RepID <> 75" + Environment.NewLine);
                sb.Append(" and r.RepID <> 2" + Environment.NewLine);
            }

            if (sSortColumn == "")
            {
                sb.Append(" order by	q.QuoteNo desc" + Environment.NewLine);
                sb.Append(",q.QuoteNoVer desc" + Environment.NewLine);
                sb.Append(",q.QuoteID desc" + Environment.NewLine);
            }
            else
            {

                if(bFlipSortDir)
                    QuotesSortedDirection.Value = QuotesSortedDirection.Value == "ASC" ? "DESC" : "ASC";

                sb.Append(" order by	" + sSortColumn + " " + QuotesSortedDirection.Value + Environment.NewLine);
                
            }

 

            dsQuoteActive.SelectCommand = sb.ToString();
            gvQuoteActive.DataSource = dsQuoteActive;
            gvQuoteActive.DataBind();

           

        }


        protected void SetTitle()
        {
            if (Session == null)
                Response.Redirect("http://www.mgmtransformer.com");

           
            bool bQuotes = rblOrder.SelectedIndex == 0 ? true : false;

            string sTitle = bQuotes == true ? "Quotes for <br />" : "Orders for<br />";

            if (rblLoginFilter.SelectedIndex == 0)
            {
                sTitle += Session["RepDistributorName"].ToString(); 
            }   
            else
            {
                sTitle += Session["FullName"].ToString();
            }

            if (Session["Inactive"].ToString() == "1")
            {
                sTitle += " (INACTIVE)";
            }

            lblQuote.Text = sTitle;

            string sFilterText = bQuotes == true ? "Quotes by " : "Orders by ";
            rblLoginFilter.Items[1].Text = sFilterText + Session["FullName"].ToString();

        }


        protected void gvQuoteActive_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblValid.Text = "";
            

            if (e.CommandName == "CompanyProjectCity")
            {
                int QuoteID = Convert.ToInt32(e.CommandArgument);

                Session["QuoteID"] = QuoteID;
                Session["PageName"] = "Quote";
                Response.Redirect("Quote.aspx");
            }
        

        }

        protected void gvQuoteActive_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    ShowHideEditArea(true, false);

                    // Progress Code.  In hidden edit template field.
                    Label lblProgressCode = (Label)e.Row.FindControl("lblProgressCode");
                    string sProgressCode = String.IsNullOrEmpty(lblProgressCode.Text.ToString()) ? "" : lblProgressCode.Text.ToString();

                    DataRow dr;
                    DropDownList ddlProgress= (DropDownList)e.Row.FindControl("ddlProgress");
                    DataTable dtP = q.ProgressCodes();
                    ddlProgress.Items.Clear();

                    if (dtP.Rows.Count > 0)
                    {
                        dr = dtP.NewRow();
                        dr["Progress"] = "";
                        dtP.Rows.InsertAt(dr, 0);

                        ddlProgress.DataTextField = "Progress";
                        ddlProgress.DataValueField = "Progress";
                        ddlProgress.DataSource = dtP;
                        ddlProgress.DataBind();

                        for (int i = 0; i < ddlProgress.Items.Count; i++)
                        {
                            if (ddlProgress.Items[i].Text == sProgressCode)
                            {
                                ddlProgress.SelectedValue = sProgressCode;
                                break;
                            }
                        }
                    }

                    // Lost Reason.
                    DropDownList ddlLostReason = (DropDownList)e.Row.FindControl("ddlLostReason");
                    Label lblLostReasonCode = (Label)e.Row.FindControl("lblLostReasonCode");

                    string sLostReason = string.IsNullOrEmpty(lblLostReasonCode.Text.ToString()) == true ? "" : lblLostReasonCode.Text.ToString();
                    sLostReason = string.IsNullOrEmpty(sLostReason) == true ? "" : sLostReason;

                    DataTable dtR = q.LostReasonCodes();
                    ddlLostReason.Items.Clear();
                    ddlLostReason.Items.Clear();

                    if (dtR.Rows.Count > 0)
                    {
                        dr = dtR.NewRow();
                        dr["LostReason"] = "";
                        dtR.Rows.InsertAt(dr, 0);

                        ddlLostReason.DataTextField = "LostReason";
                        ddlLostReason.DataValueField = "LostReason";
                        ddlLostReason.DataSource = dtR;
                        ddlLostReason.DataBind();

                        if (sLostReason != "")
                            ddlLostReason.SelectedValue = sLostReason;
                    }

                    // Lost To.
                    DropDownList ddlLostTo = (DropDownList)e.Row.FindControl("ddlLostTo");
                    Label lblLostToCode = (Label)e.Row.FindControl("lblLostToCode");
                    string sLostTo = string.IsNullOrEmpty(lblLostToCode.Text.ToString()) == true ? "" : lblLostToCode.Text.ToString();
                    DataTable dtT = q.LostToCodes();
                    ddlLostTo.Items.Clear();

                    if (dtT.Rows.Count > 0)
                    {
                        dr = dtT.NewRow();
                        dr["LostTo"] = "";
                        dtT.Rows.InsertAt(dr, 0);

                        ddlLostTo.DataTextField = "LostTo";
                        ddlLostTo.DataValueField = "LostTo";
                        ddlLostTo.DataSource = dtT;
                        ddlLostTo.DataBind();

                        if (sLostTo != "")
                            ddlLostTo.SelectedValue = sLostTo;
                    }

                    // Followup date.
                    Label lblFollowupDate = (Label)e.Row.FindControl("lblFollowupDate");
                    TextBox txtFollowupDate = (TextBox)e.Row.FindControl("txtFollowupDate");
                }
            }
        }

        /// <summary>
        /// Show or hide the edit area to the right of gvQuoteActive / gvQuoteHistory.
        /// </summary>
        /// <param name="bShow"></param>
        protected void ShowHideEditArea(bool bShow, bool bHistory)
        {
                this.gvQuoteActive.Columns[10].Visible = bShow;
                this.gvQuoteActive.Columns[11].Visible = bShow;
                this.gvQuoteActive.Columns[12].Visible = bShow;
            
        }
        
        
        protected void gvQuoteActive_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.gvQuoteActive.EditIndex = -1;
            LoadGrid();

            ShowHideEditArea(false, false);
        }

        protected void gvQuoteActive_RowEditing(object sender, GridViewEditEventArgs e)
        {
            lblValid.Text = "";
            msProgressCode = "";
            msLostReasonCode = "";

            this.gvQuoteActive.EditIndex = e.NewEditIndex;
            LoadGrid();
        }

        protected void gvQuoteActive_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int iQuoteID = Convert.ToInt32(gvQuoteActive.DataKeys[e.RowIndex].Value);

            DropDownList ProgressCode = (DropDownList)gvQuoteActive.Rows[e.RowIndex].FindControl("ddlProgress");
            DropDownList LostReasonCode = (DropDownList)gvQuoteActive.Rows[e.RowIndex].FindControl("ddlLostReason");
            DropDownList LostToCode = (DropDownList)gvQuoteActive.Rows[e.RowIndex].FindControl("ddlLostTo");

            // Set these protected level variables, so RowUpdated can use them as well.
            msProgressCode = String.IsNullOrEmpty(ProgressCode.SelectedValue) == true ? "" : ProgressCode.SelectedValue;
            msLostReasonCode = String.IsNullOrEmpty(LostReasonCode.SelectedValue) == true ? "" : LostReasonCode.SelectedValue;

            string sLostToCode = String.IsNullOrEmpty(LostToCode.SelectedValue) == true ? "" : LostToCode.SelectedValue;

            // Update the underlying data.
            string sMsg = q.QuoteActiveUpate(iQuoteID, msProgressCode, msLostReasonCode, sLostToCode,"");

            // If invalid, display error message.
            lblValid.Text = sMsg == ""? "": sMsg + "<br /><br />";

            this.gvQuoteActive.EditIndex = -1;
            LoadGrid();

            ShowHideEditArea(false, false);
        }

        protected void gvQuoteActive_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CloseEdit();

            gvQuoteActive.PageIndex = e.NewPageIndex;
            LoadGrid(false,QuotesSortedColumn.Value,false,"",false,Convert.ToBoolean(cbPendingApproval.Checked));

        }

        protected void gvQuoteHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CompanyProjectCity")
            {
                int QuoteID = Convert.ToInt32(e.CommandArgument);

                Session["QuoteID"] = QuoteID;
                Session["PageName"] = "Quote";
                Response.Redirect("Quote.aspx");
            }
        }

        protected void gvQuoteHistory_RowEditing(object sender, GridViewEditEventArgs e)
        {
            lblValid.Text = "";
            msProgressCode = "";
            msLostReasonCode = "";
            
        }

        protected void gvQuoteHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    ShowHideEditArea(true, true);

                    // Progress Code.  In hidden edit template field.
                    Label lblProgressCode = (Label)e.Row.FindControl("lblProgressCode");
                    string sProgressCode = String.IsNullOrEmpty(lblProgressCode.Text.ToString()) ? "" : lblProgressCode.Text.ToString();

                    DataRow dr;
                    DropDownList ddlProgress = (DropDownList)e.Row.FindControl("ddlProgress");
                    DataTable dtP = q.ProgressCodes();
                    ddlProgress.Items.Clear();

                    if (dtP.Rows.Count > 0)
                    {
                        dr = dtP.NewRow();
                        dr["Progress"] = "";
                        dtP.Rows.InsertAt(dr, 0);

                        ddlProgress.DataTextField = "Progress";
                        ddlProgress.DataValueField = "Progress";
                        ddlProgress.DataSource = dtP;
                        ddlProgress.DataBind();

                        for (int i = 0; i < ddlProgress.Items.Count; i++)
                        {
                            if (ddlProgress.Items[i].Text == sProgressCode)
                            {
                                ddlProgress.SelectedValue = sProgressCode;
                                break;
                            }
                        }
                    }

                    // Lost Reason.
                    DropDownList ddlLostReason = (DropDownList)e.Row.FindControl("ddlLostReason");
                    Label lblLostReasonCode = (Label)e.Row.FindControl("lblLostReasonCode");

                    string sLostReason = string.IsNullOrEmpty(lblLostReasonCode.Text.ToString()) == true ? "" : lblLostReasonCode.Text.ToString();
                    sLostReason = string.IsNullOrEmpty(sLostReason) == true ? "" : sLostReason;

                    DataTable dtR = q.LostReasonCodes();
                    ddlLostReason.Items.Clear();
                    ddlLostReason.Items.Clear();

                    if (dtR.Rows.Count > 0)
                    {
                        dr = dtR.NewRow();
                        dr["LostReason"] = "";
                        dtR.Rows.InsertAt(dr, 0);

                        ddlLostReason.DataTextField = "LostReason";
                        ddlLostReason.DataValueField = "LostReason";
                        ddlLostReason.DataSource = dtR;
                        ddlLostReason.DataBind();

                        if (sLostReason != "")
                            ddlLostReason.SelectedValue = sLostReason;
                    }

                    // Lost To.
                    DropDownList ddlLostTo = (DropDownList)e.Row.FindControl("ddlLostTo");
                    Label lblLostToCode = (Label)e.Row.FindControl("lblLostToCode");
                    string sLostTo = string.IsNullOrEmpty(lblLostToCode.Text.ToString()) == true ? "" : lblLostToCode.Text.ToString();
                    DataTable dtT = q.LostToCodes();
                    ddlLostTo.Items.Clear();

                    if (dtT.Rows.Count > 0)
                    {
                        dr = dtT.NewRow();
                        dr["LostTo"] = "";
                        dtT.Rows.InsertAt(dr, 0);

                        ddlLostTo.DataTextField = "LostTo";
                        ddlLostTo.DataValueField = "LostTo";
                        ddlLostTo.DataSource = dtT;
                        ddlLostTo.DataBind();

                        if (sLostTo != "")
                            ddlLostTo.SelectedValue = sLostTo;
                    }
                }
            }
        }



        protected void gvOrderHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CompanyProjectCity")
            {
                int QuoteID = Convert.ToInt32(e.CommandArgument);

                Session["QuoteID"] = QuoteID;
                Session["PageName"] = "Quote";
                Response.Redirect("Quote.aspx");
            }
        }

        protected void gvOrderHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrderHistory.PageIndex = e.NewPageIndex;
                      
            LoadQuotes();
        }

        protected void rblDateFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            CloseEdit();
            ResetIndices();
            LoadQuotes();
        }

        protected void ddlLogin_SelectedIndexChanged(object sender, EventArgs e)
        {

            btnClearCustomer_Click(sender, e);
            
            CloseEdit();
           
            ResetIndices();
            CompanyFilter();

            // Note:  Don't change the fact that this Rep is Internal.
            int iRepID = Convert.ToInt32(ddlLogin.SelectedValue);
            string sRepName = ddlLogin.SelectedItem.ToString();

            Session["RepID"] = iRepID;
            Session["RepDistributorID"] = iRepID;

            int iPos = 0;
            string sInactive = "0";
            iPos = sRepName.IndexOf(" (INACTIVE)");
            if (iPos > 0)
            {
                sInactive = "1";
                sRepName = sRepName.Substring(0, iPos);
            }

            Session["RepName"] = sRepName;
            Session["RepDistributorName"] = sRepName;
            Session["Inactive"] = sInactive;

            ResetIndices();

            LoadCompanies(iRepID,"");
            LoadQuotes();
        }
       
        protected void btnSearch_Click(object sender, EventArgs e)
        {
           
            CloseEdit();
            Search();
        }

        protected void Clear()
        {
            Session["QuoteNo"] = 0;
            Session["QuoteNoVer"] = 1;
        }

        /// <summary>
        /// Close any grid editing before working on the Active grid.
        /// </summary>
        protected void CloseEdit()
        {
            lblValid.Text = "";
            this.gvQuoteActive.EditIndex = -1;
        }

        /// <summary>
        /// Accept a quote number, and put it into Session variables.
        /// Used by Go button on Home page after data validation is done
        /// when entering a quote number.  The code repeats the validation
        /// in an abbreviated manner, just to be on the safe side.
        /// </summary>
        /// <param name="sQuoteNo"></param>
        /// <returns></returns>
        protected void QuoteNoAccept(string sQuoteNo)
        {
            sQuoteNo = sQuoteNo.Trim().ToUpper();
            if (String.IsNullOrEmpty(sQuoteNo))
            {
                Session["QuoteNo"] = 0;
                Session["QuoteNoVer"] = 1;
                return;
            }

            // Trim away left-hand WQ if any.
            if (sQuoteNo.Substring(0, 2) == "WQ")
                sQuoteNo = sQuoteNo.Substring(2, sQuoteNo.Length - 2);

            double Num;
            bool isNum = double.TryParse(sQuoteNo, out Num);

            if (!isNum) return;
            if (Num > 99999) return;
            if (Num < 100) return;

            int iQuoteNo = Convert.ToInt32(Num);
            int iQuoteNoVer = 1;
            int iPos = sQuoteNo.IndexOf(".");
            if (iPos > 0)
                iQuoteNoVer = Convert.ToInt32(sQuoteNo.Substring(iPos + 1, sQuoteNo.Length - (iPos + 1)));

            Session["QuoteNo"] = iQuoteNo.ToString();
            Session["QuoteNoVer"] = iQuoteNoVer.ToString();
        }

        protected void dsQuoteHistory_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.AffectedRows < 1 && Convert.ToInt32(Session["QuoteNo"]) > 0)
            {
                //lblSearch.Text = "No items found.";
            }
        }

        protected void dsOrderHistory_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.AffectedRows < 1 && Convert.ToInt32(Session["QuoteNo"]) > 0)
            {
                //lblSearch.Text = "No items found.";
            }
        }

        protected void Search()
        {
            ResetIndices();
            int iInternal = Int32.Parse(Session["Internal"].ToString());
            int iRepID = Convert.ToInt32(Session["RepID"]);
           
            LoadQuotes();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
           
            CloseEdit();
            Clear();

            Search();
 
            int iRepDistributorID = Convert.ToInt32(Session["RepDistributorID"]);
            ddlLogin.SelectedValue = iRepDistributorID.ToString();

            string sFilterText = "Quotes by " + Session["FullName"].ToString();
            rblLoginFilter.SelectedIndex = rblLoginFilter.Items.IndexOf(rblLoginFilter.Items.FindByText(sFilterText));

            rblLoginFilter_SelectedIndexChanged(sender, e);
            

        }

        protected void CompanyFilter()
        {
            Clear();
           

        }

        protected void btnClearCompanies_Click(object sender, EventArgs e)
        {
            CloseEdit();
            
            CompanyFilter();
        }


        private void LoginFilterLoad()
        {
            CloseEdit();
            int iLogin = (rblLoginFilter.SelectedValue == "All") ? 0 : 1;
            Session["UserNameFilter"] = (iLogin == 0) ? "" : Session["UserName"];
            int iRepDistributorID = Convert.ToInt32(Session["RepDistributorID"]);
            string sUserName = Session["UserName"].ToString();


            LoadCompanies(iRepDistributorID, sUserName);
            ResetIndices();
            LoadQuotes();



        }


        protected void rblLoginFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbPendingApproval.Checked = false;

            tbSearchCompany.Text = "";
            LoginFilterLoad();
        }

        /// <summary>
        /// Display characteristics of the screen.
        /// </summary>
        protected void Display()
        {
            int iOption = rblOrder.SelectedIndex;

            switch (iOption)
            {
                case 0:
                    ActivePanel.Visible = true;
                    OrderPanel.Visible = false;
                    break;
                
                case 1:
                    OrderPanel.Visible = true;
                    ActivePanel.Visible = false;
                    break;
            }
        }

        protected void rblOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            CloseEdit();
            Search();
            Display();
            tbSearchCompany.Text = "";

            cbPendingApproval.Checked = false;
            ShowPendingApprovals.Value = false.ToString();

            cbPendingApproval.Visible = rblOrder.SelectedIndex == 0 && Convert.ToInt32(Session["Internal"]) != 0 ? true : false;


            switch (rblOrder.SelectedIndex)
            {
                case 0:
                    hfCurrentVisibleGrid.Value = "Quotes";
                    FindWhat.Text = FindWhat.Text.Replace("PO#,", "");

                    break;
                case 1:
                    hfCurrentVisibleGrid.Value = "Orders";
                    FindWhat.Text = "PO#," + FindWhat.Text;
                    break;
                default:
                    break;
            }



        }            

        // Reset page indices to zero on grids.
        protected void ResetIndices()
        {
            gvQuoteActive.PageIndex = 0;
            gvOrderHistory.PageIndex = 0;
        }

 
        protected void rbAllCustomers_CheckedChanged(object sender, EventArgs e)
        {
            LoadQuotes();
        }

        protected void gvQuoteActive_Sorting(object sender, GridViewSortEventArgs e)
        {
            QuotesSortedColumn.Value = e.SortExpression;
            LoadGrid(true,e.SortExpression,false,"",false,cbPendingApproval.Checked);
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            ShowPendingApprovals.Value = false.ToString();
            cbPendingApproval.Checked = false;

            QuotesCompanySearch.Value = tbSearchCompany.Text;
            OrdersCompanySearch.Value = tbSearchCompany.Text;

            if (rblOrder.SelectedIndex == 0)
                LoadGrid(true,"", false, OrdersCompanySearch.Value, true);
            else
                LoadQuotes(false,QuotesCompanySearch.Value);

            btnClearCustomer.Focus();
       }

        protected void btnClearCustomer_Click(object sender, EventArgs e)
        {

            ShowPendingApprovals.Value = false.ToString();
            cbPendingApproval.Checked = false;

            tbSearchCompany.Text = "";
            QuotesCompanySearch.Value = "";
            OrdersCompanySearch.Value = "";

            QuotesSortedColumn.Value = "";
            OrdersSortedColumn.Value = "";

            gvQuoteActive.PageIndex = 0;
            gvOrderHistory.PageIndex = 0;


            if (rblOrder.SelectedIndex == 0)
                LoadGrid(true,"", false);
            else
                LoadQuotes(false,"");

            tbSearchCompany.Focus();
        }

        protected void gvOrderHistory_Sorting(object sender, GridViewSortEventArgs e)
        {
            int iRepID = Convert.ToInt32(Session["RepID"]);
            string sUserName = "";
            string sSearch = OrdersCompanySearch.Value;

            if (rblLoginFilter.SelectedIndex == 0)
                sUserName = "";
            else
                sUserName = Session["UserName"].ToString();

            OrdersSortedColumn.Value = e.SortExpression;
            LoadOrders(true,0, 0, iRepID, sUserName, sSearch, OrdersSortedColumn.Value);
            
        }

        protected void cbPendingApproval_CheckedChanged(object sender, EventArgs e)
        {
            rblLoginFilter.SelectedIndex = cbPendingApproval.Checked == true ? 0 : 1;
            ShowPendingApprovals.Value = cbPendingApproval.Checked.ToString();

            QuotesCompanySearch.Value = "";
            tbSearchCompany.Text = "";

            LoadGrid(true, "", false, "", false,cbPendingApproval.Checked);
        }

        protected void cbPendingEmails_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPendingEmails.Checked)
            {
                pnlPendingEmails.Visible = true;
                ActivePanel.Visible = false;
                OrderPanel.Visible = false;
                hfPreviousVisibleGrid.Value = hfCurrentVisibleGrid.Value;
                hfCurrentVisibleGrid.Value = "PendingEmails";

                LoadPendingEmails(Session["UserName"].ToString());

            }
            else
            {
                switch (hfPreviousVisibleGrid.Value)
                {
                    case "Quotes":
                        ActivePanel.Visible = true;
                        OrderPanel.Visible = false;
                        pnlPendingEmails.Visible = false;
                        hfCurrentVisibleGrid.Value = "Quotes";
                        break;
                    case "Orders":
                        OrderPanel.Visible = true;
                        ActivePanel.Visible = false;
                        pnlPendingEmails.Visible = false;
                        hfCurrentVisibleGrid.Value = "Orders";
                        break;
                    case "PendingEmails":
                        pnlPendingEmails.Visible = true;
                        ActivePanel.Visible = false;
                        OrderPanel.Visible = false;
                        hfCurrentVisibleGrid.Value = "PendingEmails";
                        break;
                    default:
                        break;
                }

            }
        }

        protected void gvPendingEmails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPendingEmails.PageIndex = e.NewPageIndex;
            LoadPendingEmails(Session["UserName"].ToString());
        }

        protected void LoadPendingEmails(string sUserName)
        {
            ActivePanel.Visible = false;
            OrderPanel.Visible = false;
            pnlPendingEmails.Visible = true;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
 
            sb.Append("select * " + Environment.NewLine);
            sb.Append("from EmailPendingLog " + Environment.NewLine);
            sb.Append("where UserName = '" + sUserName + "' " + Environment.NewLine);
            sb.Append("and DateTimeAdded > '" + DateTime.Now.AddDays(-1).ToShortDateString() + "'" + Environment.NewLine);


            dsPendingEmails.SelectCommand = sb.ToString();
            gvPendingEmails.DataSource = dsPendingEmails;

            gvPendingEmails.DataBind();

        }

        protected void gvPendingEmails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = Convert.ToDateTime(e.Row.Cells[1].Text).ToShortDateString();
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text).ToShortDateString();
            }
        }
    }
}