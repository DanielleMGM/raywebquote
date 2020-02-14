using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;

namespace MGM_Transformer
{
    public partial class QuotePDFRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            sPath = sPath.Replace("file:\\", "");
            sPath = sPath.Replace("bin", "pdfs");
            string sFN = sPath + "\\QuotePDF.pdf";
 
            ReportDocument r = new ReportDocument();
            try
            {
                int iQuoteID = Convert.ToInt32(Request.QueryString["QuoteID"]);
 
                // Update the dataset first.
                Quotes q = new Quotes();
                DataTable dt = q.QuotePDF(iQuoteID);
                DataRow dr = dt.Rows[0];
                string sFileName = dr["PDFFileName"].ToString();
                string sPathAndFN = sPath + "\\" + sFileName;

                r.Load(System.AppDomain.CurrentDomain.BaseDirectory + "QuotePDF.rpt");

                r.SetParameterValue("@quote_id", iQuoteID);

                r.ExportToDisk(ExportFormatType.PortableDocFormat, sPathAndFN);

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