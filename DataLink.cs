using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MGM_Transformer
{
    public enum DataLinkCon { mgmuser = 0, bpss = 1 };
    
    
    public class DataLink
    {

        static public DataSet Select(string query, DataLinkCon dlc)
        {
            SqlConnection con = null;

            if (dlc == DataLinkCon.mgmuser)
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());

            if (dlc == DataLinkCon.bpss)
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["bpss"].ToString());

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds;

                }
                catch (SqlException ex)
                {
                    System.Console.WriteLine(ex.ToString());
                    return null;
                }
            }
        }

        static public bool Update(string qry, DataLinkCon dlc)
        {
            SqlConnection con = new SqlConnection();
            int iRowsAffected = 0;

            if (dlc == DataLinkCon.mgmuser)
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());

            if (dlc == DataLinkCon.bpss)
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["bpss"].ToString());

            SqlCommand cmd = new SqlCommand(qry, con);

            using (con)
            {
                try
                {
                    con.Open();
                    iRowsAffected = cmd.ExecuteNonQuery();

                    if (iRowsAffected == 0)
                        return false;
                    else
                        return true;
                }
                catch (SqlException ex) { return false; }
            }

        }


        static public bool ProcNoReturn(string sProcName, List<SqlParameter> lstParams, DataLinkCon dlc)
        {
            SqlConnection con = new SqlConnection();
            int iRowsAffected = 0;

            if (dlc == DataLinkCon.mgmuser)
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());

            if (dlc == DataLinkCon.bpss)
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["bpss"].ToString());

            SqlCommand cmd = new SqlCommand(sProcName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter s in lstParams)
            {
                cmd.Parameters.Add(s);
            }

            using (con)
            {
                try
                {
                    con.Open();
                    iRowsAffected = cmd.ExecuteNonQuery();

                    if (iRowsAffected == 0)
                        return false;
                    else
                        return true;
                }
                catch (SqlException ex) { return false; }
            }

        }




        static public DataSet StoredProcSelect(string sName, List<SqlParameter> lstParams,DataLinkCon dlc)
        {

            SqlConnection con = new SqlConnection();
            
            if (dlc == DataLinkCon.mgmuser)
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());

            if (dlc == DataLinkCon.bpss)
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["bpss"].ToString());
           
            string strSql = sName;
            SqlCommand comd = new SqlCommand(strSql, con);
            comd.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();

            foreach (SqlParameter param in lstParams)
                comd.Parameters.Add(param);
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = comd;
                sda.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.Message);
            }
            finally { }
            return ds;

        }

        static public int EmailPendingExists(int iEmailID, DataLinkCon dlc)
        {
            int iRetValue = -1;
            SqlConnection con = null;

            if (dlc == DataLinkCon.mgmuser)
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());

            if (dlc == DataLinkCon.bpss)
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["bpss"].ToString());

            SqlCommand cmd = new SqlCommand("select QuoteEmailID from EmailPendingLog where QuoteEmailID = " + iEmailID, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach(DataRow dr in ds.Tables[0].Rows)
                              iRetValue = Convert.ToInt32(dr["Attempts"]);
                        }
                    }

                }
                catch (SqlException ex)
                {
                    System.Console.WriteLine(ex.ToString());
                    iRetValue = -1;
                }
            }

            return iRetValue;
        }




    }
}