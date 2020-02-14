using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace MGM_Transformer
{
    public class Security
    {
        private DataValidation dv = new DataValidation();
        
        public int ValidateUser(string sUserName, string sPassword, out int iSecurityLevel, out string sOutcome, 
                                out bool bUserName, out string sFullName, out string sMGMAgentNo, out string sRepName,
                                out string sEmail, out string sPhone, out string sPhoneType, out string sPhoneProgress, out string sSecurity)
        {
            string sRepInactiveDate = "";
            string sLoginInactiveDate = "";
            int iRepID = 0;
            iSecurityLevel = 0;
            sFullName = "";
            sRepName = "";
            sEmail = "";
            sMGMAgentNo = "";
            sPhone = "";
            sPhoneProgress = "";
            sPhoneType = "";
            sSecurity = "";

            sUserName = (sUserName == null) ? "" : sUserName;
            sUserName = sUserName.Trim();

            sPassword = (sPassword == null) ? "" : sPassword;
            sPassword = sPassword.Trim();

            if (sUserName == null || sUserName == "")
            {
                sOutcome = "Please enter User Name.";
                bUserName = true;
                return 0;
            }
            if (sPassword == null || sPassword == "")
            {
                sOutcome = "Please enter Password.";
                bUserName = false;
                return 0;
            }

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand("SELECT r.RepID, r.SecurityLevel, r.Display_Name as RepName, r.MGMAgentNo, " +
                                    "l.UserName, l.FullName, l.Password, l.Email, l.PhoneSecurity as Phone, l.PhoneType, l.PhoneProgressCode," + 
                                    "ISNULL(r.InactiveDate,'01/01/1900') as RepInactiveDate, ISNULL(l.InactiveDate,'01/01/1900') as LoginInactiveDate " +
                                    "FROM Logins l JOIN Rep r ON l.RepID = r.RepID " +
                                    "WHERE l.UserName=@user_name", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@user_name", SqlDbType.VarChar, 50);
            cmd.Parameters["@user_name"].Value = sUserName;
            SqlDataReader dr;

            string sSecurityLevel = "";
            string sPasswordCheck = "";

            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    iRepID = Convert.ToInt32(dr["RepID"]);
                    sPasswordCheck = dr["Password"].ToString();
                    sFullName = dr["FullName"].ToString();
                    sRepName = dr["RepName"].ToString();
                    sSecurityLevel = dr["SecurityLevel"].ToString();
                    sEmail = dr["Email"].ToString();
                    sMGMAgentNo = dv.AgentNo(dr["MGMAgentNo"].ToString());          
                    sPhone = dr["Phone"].ToString();
                    sPhoneType = dr["PhoneType"].ToString();
                    sPhoneProgress = dr["PhoneProgressCode"].ToString();
                    sRepInactiveDate = String.Format("{0:d}", Convert.ToDateTime(dr["RepInactiveDate"]));
                    sLoginInactiveDate = String.Format("{0:d}", Convert.ToDateTime(dr["LoginInactiveDate"]));
                }
                dr.Close();
                con.Close();
                con.Dispose();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error in reading security:" + ex);
            }

            if (sPasswordCheck == "")
            {
                sOutcome = "User Name not recognized.";
                bUserName = true;
                return 0;
            }
            if (sPassword != sPasswordCheck)
            {
                sOutcome = "Password incorrect.";
                bUserName = false;
                return 0;
            }
            if (sRepInactiveDate != "1/1/1900")
            {
                sOutcome = sRepName + " is inactive.";
                bUserName = false;
                return 0;
            }
            if (sLoginInactiveDate != "1/1/1900")
            {
                sOutcome = sUserName + " is inactive.";
                bUserName = false;
                return 0;
            }


            // Outside Rep.
            iSecurityLevel = 0;
            if (sSecurityLevel == "Internal")
            {
                // Internal Rep.
                iSecurityLevel = 1;
            }
            else if(sSecurityLevel == "Internal Admin")
            {
                // Internal Manager / Admin.
                iSecurityLevel = 2;
            }
            else if (sSecurityLevel == "Special Pricing")
            {
                sSecurity = "Special Pricing";
            }


            sOutcome = "Welcome!";
            bUserName = true;

            return iRepID;
        }

        public void DetectUserFromIP(string sVPN_IP, out string sUserName, out string sPassword)
        {
            // If a VPN IP address is used, it will be stored in the Logins table to identify the user logging in
            // so they don't have to login both to the VPN and to the site.

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand("usp_Login_ByVPN_IP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@vpn_ip", SqlDbType.VarChar, 20);
            cmd.Parameters["@vpn_ip"].Value = sVPN_IP;
            SqlDataReader dr;

            sUserName = "";
            sPassword = "";

            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    sUserName = dr["UserName"].ToString();
                    sPassword = dr["Password"].ToString();
                }
                dr.Close();
                con.Close();
                con.Dispose();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error in reading Logins:" + ex);
            }
        }

        /// <summary>
        /// If IP address not on list, won't allow logon.
        /// </summary>
        /// <param name="sIP"></param>
        /// <returns></returns>
        public bool ValidIpAddress(string sIP)
        {
            bool bReturn = false;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand("SELECT * FROM IP_adresses WHERE IP_address = @IP", con);
                com.Parameters.Add("@IP", SqlDbType.VarChar, 100);
                com.Parameters["@IP"].Value = sIP;
                SqlDataReader sdr = com.ExecuteReader();
                if (sdr != null && sdr.HasRows)
                    bReturn = true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("IP Validation failed with error " + ex + ".");

                ErrorLog.Write("Invalid IP address " + sIP);
                return false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return bReturn;
        }

    }
}