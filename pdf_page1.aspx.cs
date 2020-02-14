using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace MGM_Transformer
{
    public partial class pdf_page1 : System.Web.UI.Page
    {
        Quotes q;
        protected DataSet dsQuotes;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["QuoteID"] != null)
            {
                int iQuoteID = int.Parse(Request.QueryString["QuoteID"].ToString());

                q = new Quotes();
                // Returns two tables:
                // Table 0: Quote
                // Table 1: QuoteDetails
                dsQuotes = q.QuotePDFSelect(iQuoteID);

                LoadRepeater();
            }
        }
        protected void LoadRepeater()
        {
            Repeater rptLineItems = (Repeater)Page.FindControl("rptLineItems");

            rptLineItems.DataSource = dsQuotes.Tables[1];
            rptLineItems.DataBind();
        }
    }
 }