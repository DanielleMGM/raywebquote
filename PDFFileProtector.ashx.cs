using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;

namespace MGM_Transformer
{
    /// <summary>
    /// Summary description for PDFFileProtector
    /// </summary>
    public class PDFFileProtector : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string host = HttpContext.Current.Request.Url.Host.ToLower();
            string sFileName = context.Request.FilePath;
            string sIpClient = context.Request.UserHostAddress;

            if(context.Request.QueryString["PassThru"] != null)
            {
                context.Response.ContentType = "application/pdf";
                context.Response.TransmitFile(context.Server.MapPath(sFileName));
                return;
            }

            // if request is local IP then let through regardless of presence of cookie
            string sLocalIP = "";
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                    sLocalIP = addr.ToString();
            }



            if (context.Request.Cookies["MGMLoggedIn"] != null)
            {
                if (context.Request.Cookies["MGMLoggedIn"]["LoggedIn"] == "True")
                {

                    context.Response.ContentType = "application/pdf";
                    context.Response.TransmitFile(context.Server.MapPath(sFileName));
                }
                else
                {
                    if (sIpClient == sLocalIP)
                    {
                        context.Response.ContentType = "application/pdf";
                        context.Response.TransmitFile(context.Server.MapPath(sFileName));
                    }
                    else
                    {
                        context.Response.ContentType = "text/html";
                        context.Response.Write("<b>Access Denied.</b>");
                    }

                }
            }
            else
            {
                if (sIpClient == sLocalIP)
                {
                    context.Response.ContentType = "application/pdf";
                    context.Response.TransmitFile(context.Server.MapPath(sFileName));
                }
                else
                {
                    context.Response.ContentType = "text/html";
                    context.Response.Write("<b>Access Denied.</b>");
                }
            }
            
              
            //string host = HttpContext.Current.Request.Url.Host.ToLower();
            //string sFileName = context.Request.FilePath;
            //string sIp = context.Request.UserHostAddress;

            //if (context.Request.Cookies["MGMLoggedIn"] != null)
            //{
            //    if (context.Request.Cookies["MGMLoggedIn"]["LoggedIn"] == "True")
            //    {

            //        context.Response.ContentType = "application/pdf";
            //        context.Response.TransmitFile(context.Server.MapPath(sFileName));
            //    }
            //    else
            //    {
            //        if (sIp == "10.4.2.21")
            //        {
            //            context.Response.ContentType = "application/pdf";
            //            context.Response.TransmitFile(context.Server.MapPath(sFileName));
            //        }
            //        else
            //        {
            //            context.Response.ContentType = "text/html";
            //            context.Response.Write("<b>Access Denied</b>");
            //        }

            //    }
            //}
            //else
            //{
            //    if (sIp == "10.4.2.21")
            //    {
            //        context.Response.ContentType = "application/pdf";
            //        context.Response.TransmitFile(context.Server.MapPath(sFileName));
            //    }
            //    else
            //    {
            //        context.Response.ContentType = "text/html";
            //        context.Response.Write("<b>Access Denied</b>");
            //    }
            //}
            
            
            
            
            ////////////////////////////////////////////////////////////
            //string sFileName = context.Request.FilePath;

            //if (context.Request.Cookies["MGMLoggedIn"] != null)
            //{
            //    if (context.Request.Cookies["MGMLoggedIn"]["LoggedIn"] == "True")
            //    {

            //        context.Response.ContentType = "application/pdf";
            //        context.Response.TransmitFile(context.Server.MapPath(sFileName));
            //    }
            //    else
            //    {
            //        context.Response.ContentType = "text/html";
            //        context.Response.Write("<b>Access Denied</b>");

            //    }
            //}
            //else
            //{
            //    context.Response.ContentType = "text/html";
            //    context.Response.Write("<b>Access Denied</b>");
            //}
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}