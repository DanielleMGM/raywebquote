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
    public partial class AdminPendingApprovals : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataValidation dv = new DataValidation();
            string sPath = dv.CurrentDir(true);

            string sFN = sPath + "pdfs//AdminPendingApprovals.pdf";

            ReportDocument r = new ReportDocument();
            try
            {

                string sRpt = "AdminPendingApprovalsPDF.rpt";
                string sFNandPath = System.AppDomain.CurrentDomain.BaseDirectory + sRpt;
                r.Load(sFNandPath);

                sFN = "C:\\MGMQuotation\\pdfs\\AdminPendingApprovals.pdf";

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