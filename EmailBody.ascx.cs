using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using NLog;
using System.Net.Mail;
using System.Web.Configuration;

namespace MGM_Transformer
{
    public partial class EmailBody : System.Web.UI.UserControl
    {
        int QuoteId;
        DataValidation dv = new DataValidation();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        string PDFUrl;
        string ProjectName;

        void Session_Start(object sender, EventArgs e)
        {
            Response.Redirect("http://www.mgmtransformer.com");

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["IsLoggedIn"]) == 0)
                Response.Redirect("http://www.mgmtransformer.com");


            if (!IsPostBack)
            {
                LoadData();
                txtToName.Focus();
            }

        }

        protected void LoadData()
        {
            QuoteId = Convert.ToInt32(Request.QueryString["QuoteID"]);
            bool bSubmittal = rblQuoteOrSubmittal.SelectedValue == "1"? true: false;
            Session["IsSubmittal"] = rblQuoteOrSubmittal.SelectedValue == "1" ? "1" : "0";

            LoadEmails();

            lblDate.Text = DateTime.Now.ToShortDateString();

            SqlCommand cmd;
            SqlConnection con;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("usp_Email_20180206", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@quote_id", QuoteId);
                cmd.Parameters.AddWithValue("@is_submittal", bSubmittal);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    lblFromName.Text = sdr["EmailFromName"].ToString();
                    lblFromEmail.Text = sdr["EmailFromEmail"].ToString();

                    // Don't show Copy Associate if we don't have an email on file for them.
                    string sFrom = lblFromEmail.Text;
                    string sTitle = "";
                    if (bSubmittal == true)
                    {
                        sTitle = "Submittal ";
                    }
                    else
                    {
                        sTitle = "Quote ";
                    }
                    sTitle += sdr["QuoteNum"].ToString();

                    lblTitle.Text = "Email " + sTitle;
                    lblEmailTitle.Text = sTitle + " Email History";

                    txtSubject.Text = "MGM Transformer " + sTitle;

                    txtToName.Text = sdr["EmailToName"].ToString().Trim();
                    txtToEmail.Text = sdr["EmailToEmail"].ToString().Trim();
                    txtBody.Text = sdr["Body"].ToString();
                    PDFUrl = sdr["PDFUrl"].ToString();
                    ProjectName = sdr["ProjectName"].ToString().Trim();
                }

                cmd.Dispose();
                con.Close();
                con.Dispose();

                // Refresh the details, especially when called after clicking Submittal checkbox.
                gvQuoteHistory.DataBind();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException(string.Format("Error ({0}): {1}", ex.Number, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                throw new ApplicationException(string.Format("Error: {0}", ex.Message));
            }
            catch (Exception ex)
            {
                // You might want to pass these errors
                // back out to the User.
                throw new ApplicationException(string.Format("Error: {0}", ex.Message));
            }

            if (ProjectName != "" && ProjectName != null)
            {
                lblTitle.Text += "  Project:" + ProjectName;
                txtSubject.Text += "  Project:" + ProjectName;
            }
        }

        protected void LoadEmails()
        {
            gvQuoteHistory.DataSource = dsEmailHistory;
            gvQuoteHistory.DataBind();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (btnSend.Text == "OK")
            {
                ReturnHome();
                return;
            }

            // Introducing delay for demonstration.
            // System.Threading.Thread.Sleep(3000);

            bool bValid = PageValidate("*");
            if (!bValid)
                return;

            bool bNoPrice = chkNoPrices.Checked;
            bool bSubmittal = rblQuoteOrSubmittal.SelectedValue == "1" ? true : false;

            Label lblServerErrors = (Label)uplblErrors.FindControl("lblServerErrors");
            Label lblSent = (Label)uplblErrors.FindControl("lblSent");

            string sSaveResults = Save(bNoPrice);

            // Save also sends the email, and stores it in a table.
            if (sSaveResults == "")
            {
                lblServerErrors.Visible = false;
                lblSent.Visible = true;
                btnSend.Text = "OK";
                btnReturn.Visible = false;

                lblToName.Text = txtToName.Text;
                lblToEmail.Text = txtToEmail.Text;
                lblCCName.Text = txtCCName.Text;
                lblCCEmail.Text = txtCCEmail.Text;
                lblSubject.Text = txtSubject.Text;
                lblBody.Text = txtBody.Text;

                lblToName.Visible = true;
                lblToEmail.Visible = true;
                lblCCName.Visible = true;
                lblCCEmail.Visible = true;
                lblSubject.Visible = true;
                lblBody.Visible = true;

                txtToName.Visible = false;
                txtToEmail.Visible = false;
                txtCCName.Visible = false;
                txtCCEmail.Visible = false;
                txtSubject.Visible = false;
                txtBody.Visible = false;

                LoadEmails();
            }
            else
            {
                lblServerErrors.Text = sSaveResults;
                lblServerErrors.Visible = true;
                lblSent.Visible = false;
            }

            // Turn the button back on.
            btnSend.Enabled = true;
        }

        protected void SendTest()
        {
            if (Convert.ToBoolean(WebConfigurationManager.AppSettings["UseSMTPMail"]))
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString()))
                {
                    SqlCommand cmd = new SqlCommand("usp_Email_Send_Test_20180803", con);
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        int numrows = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        System.Console.WriteLine("Exception: " + ex.ToString());
                    }
                    finally
                    {
                        cmd.Dispose();
                        con.Close();
                    }
                }
            }
        }


        private string SendSQLMail(string sPDF,bool bNoPrice,bool bSubmittal)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("usp_Email_Send_20190117", con);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@quote_id", QuoteId);
                    cmd.Parameters.AddWithValue("@to_name", txtToName.Text.ToString());
                    cmd.Parameters.AddWithValue("@to_email", txtToEmail.Text.ToString());
                    cmd.Parameters.AddWithValue("@cc_name", txtCCName.Text.ToString());
                    cmd.Parameters.AddWithValue("@cc_email", txtCCEmail.Text.ToString());
                    cmd.Parameters.AddWithValue("@subj", txtSubject.Text.ToString());
                    cmd.Parameters.AddWithValue("@body_text", txtBody.Text.ToString());
                    cmd.Parameters.AddWithValue("@attach", sPDF);
                    cmd.Parameters.AddWithValue("@send", 1);
                    cmd.Parameters.AddWithValue("@is_no_price", bNoPrice);
                    cmd.Parameters.AddWithValue("@is_submittal", bSubmittal);

                    con.Open();
                    int numrows = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    DataLink.Update("insert into EMailPendingLog (Attempts,DateTimeAdded,DateTimeVerified,EmailTypeCode,ErrorText,QuoteEmailID,[Status],UserEmail,UserName) values (" +
                                              "1,GetDate(),null,'Email Screen','" + ex.Message + "',-1,',0,'" + HttpContext.Current.Session["Email"].ToString() +
                                              "','" + HttpContext.Current.Session["UserName"].ToString() + "')", DataLinkCon.mgmuser);
                    return "Failed to create log entry.";
                   
                }
                finally
                {
                    cmd.Dispose();
                    con.Close();
                }

                DataLink.Update("insert into EMailPendingLog (Attempts,DateTimeAdded,DateTimeVerified,EmailTypeCode,ErrorText,QuoteEmailID,[Status],UserEmail,UserName) values (" +
                                          "1,GetDate(),GetDate(),'Email Screen',''," + Utility.GetQuoteEmailID(QuoteId) + ",1,'" + HttpContext.Current.Session["Email"].ToString() +
                                          "','" + HttpContext.Current.Session["UserName"] + "')", DataLinkCon.mgmuser);
            }

            return "";

     }


        protected string Save(bool bNoPrice)
        {
            bool bRetValue = false;
            bool bSubmittal = rblQuoteOrSubmittal.SelectedValue == "1" ? true : false;
            string sUserName = Session["UserName"].ToString();
            string sUserEmail = Session["Email"].ToString();
            bool bOEMSuccess = false;

            MailAddress from = null;

            QuoteId = Convert.ToInt32(Request.QueryString["QuoteID"]);
            Quotes q = new Quotes();

            int iQuoteID = QuoteId;


            if (bSubmittal == true)
            {
                if (q.SubmittalValid(iQuoteID) == false)
                {
                    return "Submittal not set up.";
                }
            }

            bool bSuccess = false;
            if (bNoPrice == true || bSubmittal == true)
            {
                bSuccess = q.CreatePDF(iQuoteID, bNoPrice, false, true, bSubmittal, out bOEMSuccess);

                // Don't continue if unsuccessful either in generating a PDF or Word document.
                if (!bSuccess || !bOEMSuccess)
                    return "Unable to create PDF.";
            }

            string sPDF = GetPDFPath(bNoPrice, bSubmittal);


            if(Convert.ToBoolean(WebConfigurationManager.AppSettings["UseSQLMail"]))
            {
                SendSQLMail(sPDF, bNoPrice, bSubmittal);
            }

            if (Convert.ToBoolean(WebConfigurationManager.AppSettings["UseSMTPMail"]))
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString()))
                {
                    
                    SqlCommand cmd = new SqlCommand("usp_Email_Send_20190117", con);
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@quote_id", QuoteId);
                        cmd.Parameters.AddWithValue("@to_name", txtToName.Text.ToString());
                        cmd.Parameters.AddWithValue("@to_email", txtToEmail.Text.ToString());
                        cmd.Parameters.AddWithValue("@cc_name", txtCCName.Text.ToString());
                        cmd.Parameters.AddWithValue("@cc_email", txtCCEmail.Text.ToString());
                        cmd.Parameters.AddWithValue("@subj", txtSubject.Text.ToString());
                        cmd.Parameters.AddWithValue("@body_text", txtBody.Text.ToString());
                        cmd.Parameters.AddWithValue("@attach", sPDF);
                        cmd.Parameters.AddWithValue("@send", 0);
                        cmd.Parameters.AddWithValue("@is_no_price", bNoPrice);
                        cmd.Parameters.AddWithValue("@is_submittal", bSubmittal);

                        con.Open();
                        int numrows = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        System.Console.WriteLine("Exception: " + ex.ToString());
                        return "Unable to send email.";
                    }
                    finally
                    {
                        bRetValue = true;
                        cmd.Dispose();
                        con.Close();
                    }

                    if (bRetValue)
                    {
                        try
                        {
                            if (!Convert.ToBoolean(WebConfigurationManager.AppSettings["UpdateEmailTablesOnly"]))
                            {

                                SmtpClient client = new SmtpClient(WebConfigurationManager.AppSettings["ExchangeServerIPAddress"]);

                                client.UseDefaultCredentials = true;

                                MailAddress to = new MailAddress(Session["Email"].ToString(), Session["RepName"].ToString(), System.Text.Encoding.UTF8);


                                if (Session["Internal"].ToString() == "1")
                                {
                                    from = new MailAddress(Session["Email"].ToString(), "MGM Quote System", System.Text.Encoding.UTF8);
                                }
                                else
                                {
                                    from = new MailAddress("quotes@mgmtransformer.com", "MGM Quote System", System.Text.Encoding.UTF8);
                                }

                                MailMessage message = new MailMessage(from, to);
                                message.Body = txtBody.Text.ToString();

                                message.BodyEncoding = System.Text.Encoding.UTF8;
                                message.Subject = txtSubject.Text.ToString();
                                message.SubjectEncoding = System.Text.Encoding.UTF8;
                                if (string.IsNullOrWhiteSpace(sPDF) == false)
                                    message.Attachments.Add(new Attachment(sPDF));

                                
                                foreach (string sEmail in txtCCEmail.Text.Split(';'))
                                {
                                    if (Utility.IsValidEmail(sEmail))
                                        message.CC.Add(sEmail);
                                }


                                if(Utility.IsValidEmail(txtToEmail.Text.ToString()))
                                    message.CC.Add(txtToEmail.Text.ToString());

                                client.Send(message);
                                message.Dispose();
                            }


                            DataLink.Update("insert into EMailPendingLog (Attempts,DateTimeAdded,DateTimeVerified,EmailTypeCode,ErrorText,QuoteEmailID,[Status],UserEmail,UserName) values (" +
                                            "1,GetDate(),GetDate(),'Email Screen',''," + Utility.GetQuoteEmailID(QuoteId) + ",1,'" + HttpContext.Current.Session["Email"].ToString() +
                                            "','" + HttpContext.Current.Session["UserName"] + "')", DataLinkCon.mgmuser);

                        }
                        catch (Exception ex) 
                        {
                            DataLink.Update("insert into EMailPendingLog (Attempts,DateTimeAdded,DateTimeVerified,EmailTypeCode,ErrorText,QuoteEmailID,[Status],UserEmail,UserName) values (" +
                                                "1,GetDate(),null,'Email Screen','" + ex.Message + "',-1,',0,'" + HttpContext.Current.Session["Email"].ToString() +
                                                "','" + HttpContext.Current.Session["UserName"].ToString() + "')", DataLinkCon.mgmuser);
                            return ex.Message + Environment.NewLine + "Failed to create log entry.";
                        }
 
                    }

                }

            }

            return "";
        }

       
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            ReturnHome();
        }

        protected void WindowClose()
        {
            string close = @"<script type='text/javascript'>
                            window.returnValue = true;
                            window.close();
                            </script>";
            base.Response.Write(close);
        }

        protected void txtToEmail_TextChanged(object sender, EventArgs e)
        {
            bool valid = PageValidate("txtToEmail");
        }

        protected void txtCName_TextChanged(object sender, EventArgs e)
        {
            bool valid = PageValidate("txtCCName");
        }
        protected void txtCCEmail_TextChanged(object sender, EventArgs e)
        {
            bool valid = PageValidate("txtCCEmail");
        }

        protected void txtSubject_TextChanged(object sender, EventArgs e)
        {
            bool valid = PageValidate("txtSubject");
        }

        protected void txtBody_TextChanged(object sender, EventArgs e)
        {
            bool valid = PageValidate("txtBody");
        }

        protected bool PageValidate(string ctlName)
        {
            // Perform validation for one or all controls on the form.
            if (ctlName == "*" || ctlName == "txtToName")
            {
                TextBox txt = (TextBox)uptxtToName.FindControl("txtToName");
                Label lblR = (Label)uptxtToName.FindControl("lblToNameReqd");
                string s = txt.Text;

                lblR.Visible = (s == null || s == "") ? true : false;
            }
            if (ctlName == "*" || ctlName == "txtToEmail")
            {
                TextBox txt = (TextBox)uptxtToName.FindControl("txtToEmail");
                Label lblR = (Label)uptxtToName.FindControl("lblToEmailReqd");
                Label lblV = (Label)uptxtToName.FindControl("lblToEmailInvalid");
                string s = txt.Text;

                lblR.Visible = (s == null || s == "") ? true : false;
                lblV.Text = dv.EmailValid(s);
                lblV.Visible = (lblV.Text == null || lblV.Text == "") ? false : true;       // Opposite of normal.
            }
            if (ctlName == "*")     // Checking for email if cc: name entered.  Only done when sending.
            {
                TextBox txtN = (TextBox)uptxtCCName.FindControl("txtCCName");
                TextBox txtE = (TextBox)uptxtCCEmail.FindControl("txtCCEmail");
                Label lblR = (Label)uptxtCCEmail.FindControl("lblCCEmailReqd");
                string sN = txtN.Text;
                string sE = txtE.Text;

                lblR.Visible = ((sE == null || sE == "") && sN != "" & sN != null) ? true : false;
            }
            if (ctlName == "*" || ctlName == "txtCCEmail")
            {
                TextBox txt = (TextBox)uptxtCCEmail.FindControl("txtCCEmail");
                Label lblV = (Label)uptxtCCEmail.FindControl("lblCCEmailInvalid");
                string s = txt.Text;

                lblV.Text = dv.EmailValid(s);
                lblV.Visible = (lblV.Text == null || lblV.Text == "") ? false : true;   // Opposite of normal.
            }
            if (ctlName == "*" || ctlName == "txtSubject")
            {
                TextBox txt = (TextBox)uptxtSubject.FindControl("txtSubject");
                Label lblR = (Label)uptxtSubject.FindControl("lblSubjectReqd");
                string s = txt.Text;

                lblR.Visible = (s == null || s == "") ? true : false;
            }
            if (ctlName == "*" || ctlName == "txtBody")
            {
                TextBox txt = (TextBox)uptxtBody.FindControl("txtBody");
                Label lblR = (Label)uptxtBody.FindControl("lblBodyReqd");
                string s = txt.Text;

                lblR.Visible = (s == null || s == "") ? true : false;
            }

            Label lblE = (Label)uplblErrors.FindControl("lblErrors");
            Label lblS = (Label)uplblErrors.FindControl("lblServerErrors");
            Label lblOK = (Label)uplblErrors.FindControl("lblSent");

            lblS.Visible = false;
            lblOK.Visible = false;

            if (lblToNameReqd.Visible || lblToEmailReqd.Visible ||
                lblToEmailInvalid.Visible || lblCCEmailInvalid.Visible ||
                lblCCEmailReqd.Visible ||
                lblSubjectReqd.Visible || lblBodyReqd.Visible)
            {
                if (ctlName == "*")
                {
                    lblE.Visible = true;
                    return false;
                }
            }
            else
            {
                lblE.Visible = false;
            }
            return true;
        }

        protected string GetPDFPath(bool bNoPrices, bool bSubmittal)
        {
            QuoteId = Convert.ToInt32(Request.QueryString["quoteid"]);
            //string path = dv.CurrentDir(false);
            string path = "C:\\MGMQuotation\\pdfs\\";
            string filename = "";
            string pdf = bNoPrices == true ? "PDFUrlNoPrice" : "PDFUrl";
            
            pdf = bSubmittal == true ? "PDFSubmittal" : pdf;


            SqlCommand cmd;
            SqlConnection con;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("SELECT " + pdf + " FROM Quote WHERE QuoteID = @quote_id", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@quote_id", SqlDbType.Int);
                cmd.Parameters["@quote_id"].Value = QuoteId;
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    filename = sdr[pdf].ToString();
                }
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException(string.Format("Error ({0}): {1}", ex.Number, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                throw new ApplicationException(string.Format("Error: {0}", ex.Message));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Error: {0}", ex.Message));
            }

            return path + filename;
        }

        protected void txtToName_TextChanged(object sender, EventArgs e)
        {
            bool valid = PageValidate("txtToName");
            
            if (valid)
                UpdateBody();
        }

        /// <summary>
        /// Called by OK (after save) and Return..
        /// </summary>
        protected void ReturnHome()
        {
            Session["PageName"] = "Quote";
            Response.Redirect("Quote.aspx");
        }

        protected void gvQuoteHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string onmouseoverStyle = "this.style.backgroundColor='lightyellow';style.cursor='hand'";
            string onmouseoutStyle = "this.style.backgroundColor='#@BackColor'";
            string rowBackColor = String.Empty;
            bool isGridEmpty = Convert.ToBoolean(ViewState["emptyGrid"]);

            if (e.Row.RowType == DataControlRowType.DataRow && isGridEmpty == false)
            {
                e.Row.Attributes.Add("onmouseover", onmouseoverStyle);
                e.Row.Attributes.Add("onmouseout", onmouseoutStyle.Replace("#@BackColor", rowBackColor));
            }
        }

        /// <summary>
        /// Called after modifying the To: to change the Dear ____.
        /// </summary>
        protected void UpdateBody()
        {
            string sTo = txtToName.Text.ToString();
            if (sTo == "" || sTo == null) return;

            string sBody = txtBody.Text.ToString();
            int iBegin = sBody.IndexOf("Dear ");
            int iEnd = sBody.IndexOf(",");
            if (iBegin < 0 || iEnd == 0) return;

            string sNewBody = "Dear " + sTo + sBody.Substring(iEnd, sBody.Length - iEnd);
            txtBody.Text = sNewBody;
        }

        protected void rblQuoteOrSubmittal_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check in advance if submittals are properly set up.

            if (rblQuoteOrSubmittal.SelectedValue == "1")
            {
                int iQuoteID = Convert.ToInt32(Session["QuoteID"]);
                Quotes q = new Quotes();
                if (q.SubmittalValid(iQuoteID) == false)
                {
                    lblSubmittalInvalid.Visible = true;
                }
                else
                {
                    lblSubmittalInvalid.Visible = false;
                }
            }
            else
            {
                lblSubmittalInvalid.Visible = false;
            }
            
            //Session["IsSubmittal"] = rblQuoteOrSubmittal.SelectedValue;
            // Change the titles, email text, and history set.
            LoadData();
        }

        protected void btnSendTest_Click(object sender, EventArgs e)
        {
            SendTest();
        }
    }
}