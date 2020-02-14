using AgentDashboard;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
    public partial class AgentDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sAgentNo = "";
            string sAgentName = "";
            string sError;
                       
            string sFN = WebConfigurationManager.AppSettings["SavePDFPath"].ToString() + "AgentDashboard.pdf";


            string sYearFrom = Request.QueryString["YearFrom"].ToString();
            string sYearTo = Request.QueryString["YearTo"].ToString();

            if(Convert.ToInt32(Session["Internal"]) != 1)
            {
                sAgentNo = Agent.GetAgentCode(Agent.GetMGMAgentNo(Session["RepName"].ToString()), Session["RepName"].ToString());
                sAgentName = Session["RepName"].ToString();
            }
            else
            {
                sAgentNo = Request.QueryString["AgentNo"].ToString();
                sAgentName = Request.QueryString["AgentName"].ToString().Replace("88", "&");
            }
           

            string sAgentCode = Agent.GetAgentCode(Convert.ToInt32(sAgentNo), sAgentName);

            AgentDashboardPDF adPDF = new AgentDashboardPDF(new DateTime(Convert.ToInt32(sYearFrom), 1, 1),
                 new DateTime(Convert.ToInt32(sYearTo), 12, 31), sAgentName == "All Agents" ? "All Agents" : sAgentCode, sAgentName, sAgentNo,sFN);


            if (!adPDF.CreatePDF(out sError))
                System.Web.HttpContext.Current.Response.Write("<h1>No Data Found</h1>");
            else
            {
                System.Web.HttpContext.Current.Response.ClearContent();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                System.Web.HttpContext.Current.Response.WriteFile(sFN);
            }
      

        }
    }
}