using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using InventoryReportLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGM_Transformer
{
    public partial class InventoryPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sFN = WebConfigurationManager.AppSettings["SavePDFPath"].ToString() + "InventoryReport.pdf";

            string sKVA = Request.QueryString["KVA"].ToString(); 
            string sWinding = Request.QueryString["Windings"].ToString(); 
            string sVoltageDisp = sVoltageDisp = Request.QueryString["VoltageDisp"].ToString();;
            bool bSearching = Convert.ToBoolean(Request.QueryString["Searching"]);
            string sAgentCode = Request.QueryString["Agent"].ToString(); 
            string sAgentName = sAgentCode == "-1" ? "AllAgents" : Request.QueryString["Name"].ToString(); 
            bool bAll = Convert.ToBoolean(Request.QueryString["All"].ToString()); 
            int iInternal = Convert.ToInt32(Session["Internal"]);
            string sCategory = Request.QueryString["VoltageCat"].ToString();
            int iRepID = 0;
            int iWarehouseNo = -1;

            InventoryReportPDF irPDF = new InventoryReportPDF(sKVA, sWinding, sVoltageDisp, bSearching, sAgentCode, 
                          sAgentName, bAll, iInternal, sCategory, iRepID, iWarehouseNo, sFN,true,false);


            if (!irPDF.CreatePDF())
            {
                System.Web.HttpContext.Current.Response.Write("<h1>No Data Found</h1>");
            }
            else
            {
                System.Web.HttpContext.Current.Response.ClearContent();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                System.Web.HttpContext.Current.Response.WriteFile(sFN);
            }
            
            
            //irPDF.CreatePDF();

            //System.Web.HttpContext.Current.Response.ClearContent();
            //System.Web.HttpContext.Current.Response.ClearHeaders();
            //System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
            //System.Web.HttpContext.Current.Response.WriteFile(sFN);
          
        }
    }
}