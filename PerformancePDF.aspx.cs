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

namespace MGM_Transformer
{
    public partial class PerformancePDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataValidation dv = new DataValidation();
            string sPath = dv.CurrentDir(true);
            string sFN = sPath +  "pdfs//PerformanceDetail.pdf";

            ReportDocument r = new ReportDocument();
            try
            {

                DateTime dtFrom = Convert.ToDateTime(Request.QueryString["DateFrom"]);
                DateTime dtTo = Convert.ToDateTime(Request.QueryString["DateTo"]);
                int iRepID = Convert.ToInt32(Request.QueryString["RepID"]);
                string sUserName = Request.QueryString["UserName"].ToString();

                if (iRepID > 0)
                {
                    // Changed from QueryString,
                    // so Admin will get requested Rep
                    // after changing on Home screen.
                    iRepID = Convert.ToInt32(Session["RepID"]);                 
                }

                string sRpt = (iRepID == 0) ? "PerformanceSummary.rpt" : "PerformanceDetail.rpt";

                // Don't have a UserName for the Summary report.
                if (iRepID == 0)
                {
                    sUserName = "";
                }

                r.Load(System.AppDomain.CurrentDomain.BaseDirectory + sRpt);

                r.SetParameterValue("@from_date", dtFrom);
                r.SetParameterValue("@to_date", dtTo);
                r.SetParameterValue("@rep_id", iRepID);
                r.SetParameterValue("@user_name", sUserName);

                sFN = "C:\\MGMQuotation\\pdfs\\PerformanceDetail.pdf";
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