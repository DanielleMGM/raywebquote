using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace MGM_Transformer
{
    public partial class StockListPricesPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            sPath = sPath.Replace("file:\\", "");
            sPath = sPath.Replace("bin", "pdfs");
            string sFN = sPath + "\\StockPriceList.pdf";

            ReportDocument r = new ReportDocument();
            try
            {
                r.Load(System.AppDomain.CurrentDomain.BaseDirectory + "StockPriceList.rpt");

                sFN = "C:\\MGMQuotation\\pdfs\\StockPriceList.pdf";
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