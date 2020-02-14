using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGM_Transformer
{
    public partial class QuoteStatusPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataValidation dv = new DataValidation();
            string sPath = dv.CurrentDir(true);
            string sFN = sPath + "pdfs//QuoteStatus.pdf";

            ReportDocument r = new ReportDocument();
            try
            {

                DateTime dtFrom = Convert.ToDateTime(Request.QueryString["DateFrom"]);
                DateTime dtTo = Convert.ToDateTime(Request.QueryString["DateTo"]);
                string sUserName = Request.QueryString["UserName"].ToString();

                r.Load(System.AppDomain.CurrentDomain.BaseDirectory + "QuoteStatus.rpt");

                r.SetParameterValue("@from_date", dtFrom);
                r.SetParameterValue("@to_date", dtTo);
                r.SetParameterValue("@user_name", sUserName);
                r.SetParameterValue("FullName", Session["FullName"].ToString());

                sFN = "C:\\MGMQuotation\\pdfs\\QuoteStatus.pdf";
                r.ExportToDisk(ExportFormatType.PortableDocFormat, sFN);

                System.Web.HttpContext.Current.Response.ClearContent();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                System.Web.HttpContext.Current.Response.WriteFile(sFN);
            }
            catch (Exception ex)
            {
                Response.Write("Exception occurred: " + ex + ".");
            }
            finally
            {
                r.Dispose();
            }
        }
    }
}