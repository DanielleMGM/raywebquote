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
    public partial class GiftCardPromotionPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataValidation dv = new DataValidation();
            string sPath = dv.CurrentDir(true);
            string sFN = sPath +  "pdfs//GiftCardPromotion.pdf";

            ReportDocument r = new ReportDocument();
            try
            {

                DateTime dtFrom = Convert.ToDateTime(Request.QueryString["DateFrom"]);
                DateTime dtTo = Convert.ToDateTime(Request.QueryString["DateTo"]);
                int iRepID = Convert.ToInt32(Request.QueryString["RepID"]);     // If zero, selecting all.

                // Update order totals for this report.
                RefreshData(dtFrom, dtTo);

                string sRpt = "GiftCardPromotion.rpt";
                r.Load(System.AppDomain.CurrentDomain.BaseDirectory + sRpt);

                r.SetParameterValue("@from_date", dtFrom);
                r.SetParameterValue("@to_date", dtTo);
                r.SetParameterValue("@rep_id", iRepID);

                sFN = "C:\\MGMQuotation\\pdfs\\GiftCardPromotion.pdf";
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

        protected void RefreshData(DateTime dtFrom, DateTime dtTo)
        {
            string sSQL = "usp_Quote_OrderSummary";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("from_date", dtFrom);
            cmd.Parameters.AddWithValue("to_date", dtTo);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            using (con)
            {
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                }
            }
        }
    }
}