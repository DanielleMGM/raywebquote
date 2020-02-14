using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Printing;
using System.Threading.Tasks.Schedulers;
using System.Collections;
using System.Net;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NLog;

namespace MGM_Transformer
{
    public partial class pdf : System.Web.UI.Page
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataValidation dv = new DataValidation();

            //create document
            Document document = new Document();
            try
            {
                int iQuoteID = int.Parse(Request.QueryString["QuoteID"].ToString());

                Quotes q = new Quotes();
                DataSet ds = q.QuotePDFSelect(iQuoteID);

                string sQuoteNum = ds.Tables[0].Rows[0]["QuoteNum"].ToString();
                sQuoteNum = sQuoteNum.Substring(2, sQuoteNum.Length - 2);           // Strip off leading WQ.

                string sRepName = ds.Tables[0].Rows[0]["Rep"].ToString();
                sRepName = sRepName.Replace(" ", "");
                
                // Generate PDF File Name.
                string sProjectName = RemoveSpecialCharacters(ds.Tables[0].Rows[0]["ProjectName"].ToString());

                sProjectName = (sProjectName == null || sProjectName == "") ? "":"_" + sProjectName;

                string pdfFileName = "MGM_" + sRepName + "_" + sQuoteNum + sProjectName + ".pdf";

                document = new Document(iTextSharp.text.PageSize.A4_LANDSCAPE.Rotate(), 25, 0, 20, 0);

                string sPath = dv.CurrentDir(true); 

                PdfWriter.GetInstance(document, new FileStream(sPath + "pdfs//" + pdfFileName, FileMode.Create));
                document.Open();
                document.AddAuthor("MGM Transformer Company");

                WebClient cl = new WebClient();
                String htmlText; List<IElement> htmlarraylist; string strImage = ""; iTextSharp.text.Image chartImg;

                //Add Page one
                document.NewPage();

                chartImg = iTextSharp.text.Image.GetInstance(Server.MapPath("images/MGMLOGO.jpg"));
                chartImg.ScalePercent(float.Parse("20"));
                chartImg.Alignment = iTextSharp.text.Image.UNDERLYING;
                document.Add(chartImg);

                //string baseUrl = "https://mgmquotation.mgmtransformer.com/mgmquotation/mgmquotationqa/";
                // string baseUrl = "https://localhost/";
                //string baseUrl = Request.Url.ToString().Replace(Request.Url.PathAndQuery, "");

                string[] split = new string[] { "pdf" };
                string baseUrl = Request.Url.ToString().ToLower().Split(split, StringSplitOptions.None)[0];
 
                htmlText = cl.DownloadString(baseUrl + "pdf_page1.aspx?QuoteID=" + iQuoteID + "&QuoteName=" + sRepName + "_" + iQuoteID);

                var sr = new StringReader(htmlText);
                htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(sr, null);
                htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null);
                for (int k = 0; k < htmlarraylist.Count; k++)
                {
                    IElement ele = (IElement)htmlarraylist[k];
                    PdfPTable tbl = ele as PdfPTable;
                    if (tbl != null && tbl.NumberOfColumns == 17)
                    {
                        // #, KVA, Phase, Primary Voltage, Secondary Voltage, TempRise, Hertz, Winding Material, Electrostatic Shield,
                        // Enclosure, Sound Level, TP1, K-Factor, Unit Price, Qty, Extended Price, Catalog Number or Accessory
                        tbl.SetWidths(new float[] { 2F, 2F, 2F, 3F, 3F, 2F, 1F, 3F, 3F, 4F, 2F, 1F, 3F, 2F, 1F, 3F, 6F });
                        document.Add(ele);
                    }
                    else
                    {
                        document.Add(ele);
                    }
                }

                //Create page onwards
                //Set scale default to 45%
                string scale = "45"; 
                string unitCase = ""; 
                string data = "";
                string sPrimaryVoltageDW = "";
                string sSecondaryVoltageDW = "";

                DataTable dt = ds.Tables[1];            // QuoteDetails.
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["KitID"].ToString() == "" || dr["KitID"].ToString() == "0")
                    {
                        document.SetMargins(50, 0, 0, 0);
                        document.NewPage();

                        sPrimaryVoltageDW = DeltaWye(dr["PrimaryVoltageDW"].ToString(), dr["Phase"].ToString());
                        sSecondaryVoltageDW = DeltaWye(dr["SecondaryVoltageDW"].ToString(), dr["Phase"].ToString());

                        data = "?quoteid=" + iQuoteID.ToString() +
                            "&quotenum=" + ds.Tables[0].Rows[0]["QuoteNum"].ToString() + 
                            "&quotename=" + ds.Tables[0].Rows[0]["QuoteName"].ToString() +
                            "&detailiddisplay=" + dr["DetailIDDisplay"].ToString() + 
                            "&kva=" + dr["KVA"].ToString() +
                            "&pvolts=" + dr["PrimaryVoltage"].ToString() +
                            "&svolts=" + dr["SecondaryVoltage"].ToString() +
                            "&pvoltsdw=" + sPrimaryVoltageDW +
                            "&svoltsdw=" + sSecondaryVoltageDW +
                            "&electrostaticshield=" + dr["ElectrostaticShield"].ToString() +
                            "&phase=" + dr["Phase"].ToString() +
                            "&hertz=" + dr["Hertz"].ToString() +
                            "&temp=" + dr["Temperature"].ToString() +
                            "&winding=" + dr["Windings"].ToString() +
                            "&kfactor=" + dr["KFactor"].ToString() +
                            "&tp1=" + dr["TP1"].ToString() +
                            "&soundlevel=" + dr["SoundLevel"].ToString() +
                            "&weight=" + dr["Weight"].ToString() +
                            "&quotenumber=" + sRepName + "_" + iQuoteID +
                            "&item=" + dr["DetailID"].ToString() +
                            "&enclosure=" + dr["Enclosure"].ToString() +
                            "&company=" + ds.Tables[0].Rows[0]["Company"].ToString();

                        htmlText = cl.DownloadString(baseUrl + "pdf_page2.aspx" + data);
                        htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null);
                        //add the case image
                        unitCase = dr["UnitCase"].ToString();
                        scale = "45.5";
                        switch (unitCase)
                        {
                            case "A":
                                strImage = "CASE_GPA.jpg";
                                break;
                            case "B": 
                                strImage = "CASE_GPB.jpg"; 
                                break;
                            case "B+":
                                strImage = "CASE_GPB+.jpg";
                                break;
                            case "C": 
                                strImage = "CASE_GPC.jpg"; 
                                break;
                            case "C+":
                                strImage = "CASE_GPC+.jpg";
                                break;
                            case "D":
                                strImage = "CASE_GPD.jpg";
                                break;
                            case "E":
                                strImage = "CASE_GPE.jpg";
                                break;
                            case "F":
                                strImage = "CASE_GPF.jpg";
                                break;
                        }
                        chartImg = iTextSharp.text.Image.GetInstance(Server.MapPath("cases/" + strImage));
                        chartImg.ScalePercent(float.Parse(scale));
                        chartImg.Alignment = iTextSharp.text.Image.UNDERLYING;
                        document.Add(chartImg);
                        for (int k = 0; k < htmlarraylist.Count; k++)
                        {
                            document.Add((IElement)htmlarraylist[k]);
                        }
                    }
                }
                SetPDFFileName(pdfFileName);
                Response.Clear();
                //Response.Write(pdfFileName); // Not used in ASP version.
            }
            catch (Exception ex)
            {
                logger.Error("pdf:" + ex.Message + " " + ex.StackTrace);
            }
            finally
            {
                if (document.IsOpen())
                    document.Close();
            }
        }

        public static string RemoveSpecialCharacters(string str) {
           StringBuilder sb = new StringBuilder();
           foreach (char c in str) {
              if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_') {
                 sb.Append(c);
              }
           }
           return sb.ToString();
        }

        private void SetPDFFileName(string pdfFileName)
        {
            int iQuoteID = int.Parse(Request.QueryString["QuoteID"].ToString());
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
            con.Open();
            string strSql = "UPDATE Quote SET PDFUrl = @PDF, Status='Finalized', Finalized_on='" + 
                                        DateTime.Now.ToShortDateString() + "' WHERE Quote.QuoteID = @QUOTEID";
            SqlCommand com = new SqlCommand(strSql, con);
            com.Parameters.Add("@QUOTEID", SqlDbType.Int).Value = iQuoteID;
            com.Parameters.Add("@PDF", SqlDbType.VarChar).Value = pdfFileName;

            com.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Values for Connection, converting D to Delta, W to Wye, and any Single Phase to N/A.
        /// </summary>
        /// <param name="sDeltaWye"></param>
        /// <param name="bSingle"></param>
        /// <returns></returns>
        private string DeltaWye(string sDW, string sPhase)
        {
            if (sPhase == "Single") return "N/A";

            if (sDW == "D") return "Delta";

            if (sDW == "W") return "Wye";

            if (sDW == "-") return "-";

            return "";
        }
    }
}