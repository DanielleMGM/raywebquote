using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using M1Report;

namespace MGM_Transformer
{
    public partial class M1CustSales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string sError;

            string sFN = WebConfigurationManager.AppSettings["SavePDFPath"].ToString() + "M1RepPortalPDF.pdf";


            DateTime dtFrom = Convert.ToDateTime(Request.QueryString["DateFrom"]);
            DateTime dtTo = Convert.ToDateTime(Request.QueryString["DateTo"]);
            string sAgentNo = Request.QueryString["AgentNo"].ToString();
            string sProductCat = Request.QueryString["ProductCat"].ToString();


            //M1ReportPDF m1PDF = new M1ReportPDF(dtFrom, dtTo, sProductCat, sAgentNo, sFN);
            M1RepPortalPDF m1PDF = new M1RepPortalPDF(dtFrom, dtTo, sProductCat, sAgentNo, sFN);

            m1PDF.CreatePDF(out sError);

            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
            System.Web.HttpContext.Current.Response.WriteFile(sFN);


            
            //------------------------------
            
            //string sFN = WebConfigurationManager.AppSettings["SavePDFPath"].ToString() + "M1Report.pdf";


            //DateTime dtFrom = Convert.ToDateTime(Request.QueryString["DateFrom"]);
            //DateTime dtTo = Convert.ToDateTime(Request.QueryString["DateTo"]);
            //string sAgentNo = Request.QueryString["AgentNo"].ToString();
            //string sProductCat = Request.QueryString["ProductCat"].ToString();
     

            //M1ReportPDF m1PDF = new M1ReportPDF(dtFrom, dtTo, sProductCat, sAgentNo, sFN);

            //m1PDF.CreatePDF();

            //System.Web.HttpContext.Current.Response.ClearContent();
            //System.Web.HttpContext.Current.Response.ClearHeaders();
            //System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
            //System.Web.HttpContext.Current.Response.WriteFile(sFN);

        }
    }
}