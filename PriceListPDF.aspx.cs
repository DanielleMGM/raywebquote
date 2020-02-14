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

namespace MGM_Transformer
{
    public partial class PriceListPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataValidation dv = new DataValidation();
            string sPath = dv.CurrentDir(true);
            string sFN = sPath + "pdfs//PriceList.pdf";
            string sPriceType = Request.QueryString["winding"].ToString();
            string sEfficiency = Request.QueryString["efficiency"].ToString();
            int iRepID = Convert.ToInt32(Session["RepDistributorID"]);

            ReportDocument r = new ReportDocument();
            try
            {
                string sSource = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "PriceList.rpt");
                r.Load(sSource);

                r.SetParameterValue("@winding", sPriceType);
                r.SetParameterValue("@rep_id", iRepID);
                r.SetParameterValue("@efficiency", sEfficiency);
                
                sFN = "C:\\MGMQuotation\\pdfs\\PriceList.pdf";

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