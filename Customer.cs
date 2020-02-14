using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace MGM_Transformer
{
    public class Customer
    {
        // Select used to refresh Customers data for the selected Rep.
        public DataTable Select(int iRepID)
        {
            string sSQL = "SELECT c.CustomerID, c.Company, c.City, c.ContactName, c.Email " +
                            "FROM Customer c JOIN Quote q ON c.CustomerID = q.CustomerID WHERE q.RepID = @rep_id";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@rep_id", SqlDbType.Int);
            cmd.Parameters["@rep_id"].Value = iRepID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            try
            {
                con.Open();
                da.Fill(ds);
                con.Close();
                con.Dispose();
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error in selecting customer:" + ex);
            }
            return ds.Tables[0];
        }

        /// <summary>
        /// Selects an individual Customer / Customer Contact.
        /// KLUGE:  Since DropDownList only has a single key, it is a POSITIVE CustomerContactID
        ///         if there is a CustomerContacts record, else it is a NEGATIVE CustomerID.
        /// </summary>
        /// <param name="iCustomerContactID"></param>
        /// <returns></returns>
        public DataTable ByCustomerID(int iCustomerContactID)
        {
            string sSQL = "usp_Customer_Select";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@customer_contact_id", SqlDbType.Int);

            cmd.Parameters["@customer_contact_id"].Value = iCustomerContactID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            try
            {
                con.Open();
                da.Fill(ds);
                con.Close();
                con.Dispose();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error in selecting customer:" + ex);
            }
            return ds.Tables[0];
        }



        public DataTable List(int RepID)
        {
            string sSQL = "usp_Customer_list";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@rep_id", SqlDbType.Int);
            cmd.Parameters["@rep_id"].Value = RepID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            try
            {
                con.Open();
                da.Fill(ds);
                con.Close();
                con.Dispose();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error in listing customers:" + ex);
            }
            return ds.Tables[0];
        }

        /// <summary>
        /// This inserts a customer if unique information is found.  Otherwise, it updates.
        /// </summary>
        /// <param name="iCustomerID"></param>
        /// <param name="sCompany"></param>
        /// <param name="sCity"></param>
        /// <param name="sContactName"></param>
        /// <param name="sEmail"></param>
        /// <returns></returns>
        public bool Insert(int iRepID, int iRepDistributorID, int iCustomerID, int iCustomerContactID, 
                            string sCompany, string sCity, string sContactName, string sEmail, int iIsEdit)
        {
            bool bReturn = true;
            string sSQL = "usp_Customer_Insert";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@rep_id", SqlDbType.Int);
            cmd.Parameters.Add("@rep_distributor_id", SqlDbType.Int);
            cmd.Parameters.Add("@customer_id", SqlDbType.Int);
            cmd.Parameters.Add("@customer_contact_id", SqlDbType.Int);
            cmd.Parameters.Add("@company", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@city", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@contact_name", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@is_edit", SqlDbType.Int);

            cmd.Parameters["@rep_distributor_id"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@customer_id"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@customer_contact_id"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters["@rep_id"].Value = iRepID;
            cmd.Parameters["@rep_distributor_id"].Value = iRepDistributorID;
            cmd.Parameters["@customer_id"].Value = iCustomerID;
            cmd.Parameters["@customer_contact_id"].Value = iCustomerContactID;
            cmd.Parameters["@company"].Value = sCompany;
            cmd.Parameters["@city"].Value = sCity;
            cmd.Parameters["@contact_name"].Value = sContactName;
            cmd.Parameters["@email"].Value = sEmail;
            cmd.Parameters["@is_edit"].Value = iIsEdit;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error in inserting customer:" + ex);
                bReturn = false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return bReturn;
        }
 
        /// <summary>
        /// This either primarily inserts new customers, or deletes them.  Customer records are not really "updated".
        /// </summary>
        /// <param name="iRepID"></param>
        /// <param name="iCustomerID"></param>
        /// <param name="iCustomerContactID"></param>
        /// <param name="iObsolete"></param>
        /// <param name="sCompany"></param>
        /// <param name="sCity"></param>
        /// <param name="sContactName"></param>
        /// <param name="sEmail"></param>
        /// <returns></returns>
        public bool Insert(int iRepID, int iRepDistributorID, int iCustomerID, int iCustomerContactID, int iObsolete, int iEdit,
                                string sCompany, string sCity, string sContactName, string sEmail)
        {
            bool bReturn = true;

            string sSQL = "usp_Customer_Insert";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@rep_id", SqlDbType.Int);
            cmd.Parameters.Add("@rep_distributor_id", SqlDbType.Int);
            cmd.Parameters.Add("@customer_id", SqlDbType.Int);
            cmd.Parameters.Add("@customer_contact_id", SqlDbType.Int);
            cmd.Parameters.Add("@company", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@city", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@contact_name", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@is_edit", SqlDbType.Int);

            cmd.Parameters["@rep_distributor_id"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@customer_id"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@customer_contact_id"].Direction = ParameterDirection.InputOutput;

            cmd.Parameters["@rep_id"].Value = iRepID;
            cmd.Parameters["@rep_distributor_id"].Value = iRepDistributorID;
            cmd.Parameters["@customer_id"].Value = iCustomerID;
            cmd.Parameters["@customer_contact_id"].Value = iCustomerContactID;
            cmd.Parameters["@company"].Value = sCompany;
            cmd.Parameters["@city"].Value = sCity;
            cmd.Parameters["@contact_name"].Value = sContactName;
            cmd.Parameters["@email"].Value = sEmail;
            cmd.Parameters["@is_edit"].Value = iEdit;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error in inserting customer:" + ex);
                bReturn = false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return bReturn;
        }

        /// <summary>
        /// Called from Customer screen when editing.  Does not insert.
        /// </summary>
        /// <param name="iCustomerID"></param>
        /// <param name="iCustomerContactID"></param>
        /// <param name="iObsolete"></param>
        /// <param name="sCompany"></param>
        /// <param name="sCity"></param>
        /// <param name="sContactName"></param>
        /// <param name="sEmail"></param>
        /// <returns></returns>
        public bool Update(int iCustomerID, int iApplyAll, int iCustomerContactID, int iObsolete, 
                                string sCompany, string sCity, string sContactName, string sEmail)
        {
            bool bReturn = true;

            string sSQL = "usp_Customer_Update";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@customer_id", SqlDbType.Int);
            cmd.Parameters.Add("@customer_contact_id", SqlDbType.Int);
            cmd.Parameters.Add("@company", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@city", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@contact_name", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@is_obsolete", SqlDbType.Int);
            cmd.Parameters.Add("@apply_all", SqlDbType.Int);
 
            cmd.Parameters["@customer_id"].Value = iCustomerID;
            cmd.Parameters["@customer_contact_id"].Value = iCustomerContactID;
            cmd.Parameters["@company"].Value = sCompany;
            cmd.Parameters["@city"].Value = sCity;
            cmd.Parameters["@contact_name"].Value = sContactName;
            cmd.Parameters["@email"].Value = sEmail;
            cmd.Parameters["@is_obsolete"].Value = iObsolete;
            cmd.Parameters["@apply_all"].Value = iApplyAll;

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error in updating customer:" + ex);
                bReturn = false;
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