using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using Ionic.Zip;
using System.Web.Configuration;
using System.Net.Mail;
using System.Text;


namespace MGM_Transformer
{


    public class Quotes
    {
        DataValidation dv = new DataValidation();


        public DataTable M1Report(DateTime dtFrom, DateTime dtTo, string sRepNo, string sProductCat)
        {
            const string sSQL = "usp_Rpt_M1_Rep";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["bpss"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@rep_no", sRepNo);
            cmd.Parameters.AddWithValue("@date_from", dtFrom);
            cmd.Parameters.AddWithValue("@date_to", dtTo);
            cmd.Parameters.AddWithValue("@export", 1);
            cmd.Parameters.AddWithValue("@product", sProductCat);

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                    {
                        return dt;      // DataTable with no records in it.
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }



        public DataTable InventoryReport()
        {
            const string sSQL = "usp_RptInventoryByLocSource";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["bpss"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                    {
                        return dt;      // DataTable with no records in it.
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }


        public string GetEmailAddressFromRepID(string sRepID)
        {
            string sql = "";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());

            sql = "select Email1 from Rep where RepID = " + sRepID;
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            string sRetVal = "";

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    if (ds.Tables.Count == 0)
                        return "";

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0].Table.Columns.Contains("Email1"))
                                sRetVal = ds.Tables[0].Rows[0]["Email1"].ToString();

                        }
                    }


                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;

                }
            }

            return sRetVal;

        }





        public string GetRepIDFromQuoteID(string sQuoteID)
        {
            string sql = "";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());

            sql = "select RepID from Quote where QuoteID = " + sQuoteID;
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            string sRetVal = "";

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    if (ds.Tables.Count == 0)
                        return "";

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0].Table.Columns.Contains("RepID"))
                                sRetVal = ds.Tables[0].Rows[0]["RepID"].ToString();

                        }
                    }


                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;

                }
            }

            return sRetVal;
        }


        public DataTable M1CustomerReport(string sRepNo, DateTime dtFrom, DateTime dtTo, string sProduct)
        {
            const string sSQL = "usp_Rpt_M1_Rep";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["bpss"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();


            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@rep_no", sRepNo);
            cmd.Parameters.AddWithValue("@date_from", dtFrom);
            cmd.Parameters.AddWithValue("@date_to", dtTo);
            cmd.Parameters.AddWithValue("@product", sProduct);



            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                    {
                        return dt;      // DataTable with no records in it.
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }




        /// <summary>
        /// Record source for QuoteHistory screen on Home page.
        /// </summary>
        /// <param name="iCustomerID"></param>
        /// <param name="iLast90Days"></param>
        /// <param name="iQuoteNo"></param>
        /// <param name="iRepID"></param>
        /// <param name="sUserName"></param>
        /// <returns></returns>
        //public DataTable OrderHistory(int iCustomerID, int iLast90Days, int iQuoteNo, int iRepID, string sUserName, string sSearchCompany = "")
        //{

        //    const string sSQL = "usp_Order_History_20170831";
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
        //    SqlCommand cmd = new SqlCommand(sSQL, con);
        //    SqlDataAdapter da;
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();

        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@rep_id", iRepID);
        //    cmd.Parameters.AddWithValue("@customer_id", iCustomerID);
        //    cmd.Parameters.AddWithValue("@last_90_days", iLast90Days);
        //    cmd.Parameters.AddWithValue("@quote_no", iQuoteNo);
        //    cmd.Parameters.AddWithValue("@user_name", sUserName);

        //    cmd.Parameters.AddWithValue("@search_company", sSearchCompany);

        //    using (con)
        //    {
        //        try
        //        {
        //            con.Open();
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(ds);
        //            if (ds.Tables.Count > 0)
        //                return ds.Tables[0];
        //            else
        //            {
        //                return dt;      // DataTable with no records in it.
        //            }
        //        }
        //        catch (SqlException ex)
        //        {
        //            Console.WriteLine("Error:" + ex.Message);
        //            return null;
        //        }
        //    }
        //}

        /// <summary>
        /// Record source for QuoteHistory screen on Home page.
        /// </summary>
        /// <param name="iCustomerID"></param>
        /// <param name="iLast90Days"></param>
        /// <param name="iQuoteNo"></param>
        /// <param name="iRepID"></param>
        /// <param name="sUserName"></param>
        /// <returns></returns>
        public DataTable QuoteHistory(int iCustomerID, int iDown, int iQuoteNo, int iQuoteNoMin,
                                      int iQuoteNoVerMin, int iRepID, string sUserName)
        {
            const string sSQL = "usp_Quote_List_New";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@customer_id", iCustomerID);
            cmd.Parameters.AddWithValue("@down", iDown);
            cmd.Parameters.AddWithValue("@quote_no", iQuoteNo);
            cmd.Parameters.AddWithValue("@quote_no_min", iQuoteNoMin);
            cmd.Parameters.AddWithValue("@quote_no_ver_min", iQuoteNoMin);
            cmd.Parameters.AddWithValue("@rep_id", iRepID);
            cmd.Parameters.AddWithValue("@user_name", sUserName);

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                    {
                        return dt;      // DataTable with no records in it.
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// Record source for QuoteActive screen on Home page.
        /// </summary>
        /// <param name="iCustomerID"></param>
        /// <param name="iLast90Days"></param>
        /// <param name="iQuoteNo"></param>
        /// <param name="iRepID"></param>
        /// <param name="sUserName"></param>
        /// <returns></returns>
        //public DataTable QuoteActive(int iCustomerID, int iLast90Days, int iQuoteNo, int iRepID, string sUserName)
        //{
        //    const string sSQL = "usp_Quote_Active";
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
        //    SqlCommand cmd = new SqlCommand(sSQL, con);
        //    SqlDataAdapter da;
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();

        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@rep_id", iRepID);
        //    cmd.Parameters.AddWithValue("@customer_id", iCustomerID);
        //    cmd.Parameters.AddWithValue("@last_90_days", iLast90Days);
        //    cmd.Parameters.AddWithValue("@quote_no", iQuoteNo);
        //    cmd.Parameters.AddWithValue("@user_name", sUserName);

        //    using (con)
        //    {
        //        try
        //        {
        //            con.Open();
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(ds);
        //            if (ds.Tables.Count > 0)
        //                return ds.Tables[0];
        //            else
        //            {
        //                return dt;      // DataTable with no records in it.
        //            }
        //        }
        //        catch (SqlException ex)
        //        {
        //            Console.WriteLine("Error:" + ex.Message);
        //            return null;
        //        }
        //    }
        //}

        /// <summary>
        /// Record source for OrderHistory screen on Home page.
        /// </summary>
        /// <param name="iCustomerID"></param>
        /// <param name="iLast90Days"></param>
        /// <param name="iQuoteNo"></param>
        /// <param name="iRepID"></param>
        /// <param name="sUserName"></param>
        /// <returns></returns>
        //public DataTable OrderHistory(int iCustomerID, int iLast90Days, int iQuoteNo, int iRepID, string sUserName)
        //{

        //    const string sSQL = "usp_Order_History_New";
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
        //    SqlCommand cmd = new SqlCommand(sSQL, con);
        //    SqlDataAdapter da;
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();

        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@rep_id", iRepID);
        //    cmd.Parameters.AddWithValue("@customer_id", iCustomerID);
        //    cmd.Parameters.AddWithValue("@last_90_days", iLast90Days);
        //    cmd.Parameters.AddWithValue("@quote_no", iQuoteNo);
        //    cmd.Parameters.AddWithValue("@user_name", sUserName);

        //    using (con)
        //    {
        //        try
        //        {
        //            con.Open();
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(ds);
        //            if (ds.Tables.Count > 0)
        //                return ds.Tables[0];
        //            else
        //            {
        //                return dt;      // DataTable with no records in it.
        //            }
        //        }
        //        catch (SqlException ex)
        //        {
        //            Console.WriteLine("Error:" + ex.Message);
        //            return null;
        //        }
        //    }
        //}




        // Select used to refresh Customers data, which includes Quote Project name.
        public DataTable QuoteSelect(int iQuoteID, bool bInternal)
        {
            const string sSQL = "usp_Quote_Select";
            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlDataAdapter da;
            var cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@quote_id", SqlDbType.Int);
            cmd.Parameters.Add("@admin", SqlDbType.Bit);

            cmd.Parameters["@quote_id"].Value = iQuoteID;
            cmd.Parameters["@admin"].Value = bInternal;
            da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error in selecting quote:" + ex);
                    return null;
                }
            }
        }

        // Return True if Quote is Finalized.
        public bool QuoteFinalized(int iQuoteID)
        {
            string sSQL = "SELECT Status FROM Quote WHERE QuoteID=@quote_id";
            string status = "";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@quote_id", SqlDbType.Int);
            cmd.Parameters["@quote_id"].Value = iQuoteID;
            using (con)
            {
                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        status = dr["Status"].ToString();
                    }
                    dr.Close();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return false;
                }
            }
            if (status.ToLower() == "finalized")
                return true;

            return false;
        }

        /// <summary>
        /// Return True if this quote has at least some quantity of products, False otherwise.
        /// Used in assessing whether or not we can finalize a quote.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <returns></returns>
        public bool QuoteHasItems(int iQuoteID)
        {
            string sSQL = "SELECT sum(Quantity) TotalQuantity FROM QuoteDetails WHERE QuoteID=@quote_id";
            string sQty = "";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@quote_id", SqlDbType.Int);

            cmd.Parameters["@quote_id"].Value = iQuoteID;
            using (con)
            {
                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        sQty = dr["TotalQuantity"].ToString();
                    }
                    dr.Close();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return false;
                }
            }

            int iQty = (String.IsNullOrEmpty(sQty) == true) ? 0 : Convert.ToInt32(sQty);
            if (iQty > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Return True if this quote has at least one product with zero quantity,
        /// generated when copying a quote, False otherwise.
        /// Used in assessing whether or not we can finalize a quote.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <returns></returns>
        public bool QuoteHasEmptyItems(int iQuoteID)
        {
            string sSQL = "SELECT QuoteID FROM QuoteDetails WHERE QuoteID=@quote_id AND Quantity = 0";
            int iQuoteIDResult = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@quote_id", SqlDbType.Int);
            cmd.Parameters["@quote_id"].Value = iQuoteID;

            using (con)
            {
                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        iQuoteIDResult = Convert.ToInt32(dr["QuoteID"]);
                    }
                    dr.Close();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return false;
                }
            }

            if (iQuoteIDResult > 0)
                return true;

            return false;
        }


        /// <summary>
        /// Return True if this quote has at least one item with Hide KVA, Hide Primary, or Hide Secondary voltage,
        /// and has at least some MGM notes, or does not require them.
        /// Used in assessing whether or not we can finalize a quote.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <returns></returns>
        public bool QuoteHasNotes(int iQuoteID)
        {
            string sSQL = "usp_Quote_HasNotes";
            bool bHasNotes = false;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);

            using (con)
            {
                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        bHasNotes = Convert.ToBoolean(dr["HasNotes"]);
                    }
                    dr.Close();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return false;
                }
            }
            return bHasNotes;
        }

        public DataTable QuoteDetailsSelect(int iQuoteID, int iDetailID)
        {
            string sSQL = "usp_Quote_Details_Select_20190620";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@quote_id", SqlDbType.Int);
            cmd.Parameters.Add("@detail_id", SqlDbType.Int);

            cmd.Parameters["@quote_id"].Value = iQuoteID;
            cmd.Parameters["@detail_id"].Value = iDetailID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        public DataRow QuoteDetailsDelete(int iQuoteDetailsID)
        {
            string sSQL = "usp_Quote_Details_Delete";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@quote_details_id", SqlDbType.Int);
            cmd.Parameters["@quote_details_id"].Value = iQuoteDetailsID;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    da.Fill(ds);
                    return ds.Tables[0].Rows[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        public DataTable QuoteItemList(int QuoteID)
        {
            string sSQL = "usp_QuoteItemList";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@quote_id", SqlDbType.Int);
            cmd.Parameters["@quote_id"].Value = QuoteID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// This inserts a quote item, once all the information is valiated.
        /// It also updates it, if it exists, or deletes it if the quantity goes to zero.
        /// Returns: Total for Quote, to refresh screen in Ajax environment.
        /// </summary>
        public DataTable InsertQuoteItem(int iQuoteID, int iDetailID, string sApprovalReason, string sApprovalReasonQuote,
                                    string sCaseColorCode, string sCaseColorOther, string sCaseSize, string sCaseSizeCalc,
                                    string sCatalogNumber, string sCatalogNoOEM, int iCustomID, string sCustomerTagNo, string sEfficiency, string sEfficiencyExemptReason,
                                    int iEfficiencySetByAdmin, string sElectrostaticShield,
                                    string sEnclosure, string sEnclosureMtlCode, int iExpediteDays, decimal decExpeditePrice,
                                    string sFrequencyCode, string sImpedanceOEM, int iIsForExport, int iIsHideKVA, int iIsHideVoltPrimary, int iIsHideVoltSecondary,
                                    int iIsSameAsStock, int iIsStepUp, int iIsTapsNone, string sKFactor,
                                    string sKitNumber, int iKitQuantity, decimal decKitPrice, string sKitRBNumber, decimal decKitRBPrice, int iKitRBQuantity,
                                    string sKitLugNumber, decimal decKitLugPrice, int iKitLugQuantity,
                                    string sKitOPNumber, decimal decKitOPPrice, int iKitOPQuantity,
                                    string sKitWBNumber, decimal decKitWBPrice, int iKitWBQuantity, decimal dKVAEntered, decimal dKVAUsed,
                                    string sMadeInUSACodes, bool bMarineDuty, string sNotesInternal, string sPhase,
                                    decimal decPriceCalced, decimal decPriceEntered, decimal decPriceList,
                                    string sPrimaryVoltageDW, string sPrimaryVoltage, int iQuantity, string sSecondaryVoltageDW,
                                    string sSecondaryVoltage, decimal curShipAmount, int iShipDays, string sShipReason, int iShipWeight,
                                    string sSoundReductCode, string sSpecialFeatureCodes, string sSpecialFeatureNotes,
                                    string sSpecialTypeCode, string sStandardOrCustom, string sConfiguration, int iStockID, string sTapsOEM,
                                    int iTempEntered, int iTempUsed, bool bTotallyEnclosed, string sUserName, string sWindings)
        {
            string sSQL = "usp_QuoteItem_Insert_20190724";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            cmd.Parameters.AddWithValue("@detail_id", iDetailID);
            cmd.Parameters.AddWithValue("@approval_reason", sApprovalReason);
            cmd.Parameters.AddWithValue("@approval_reason_quote", sApprovalReasonQuote);
            cmd.Parameters.AddWithValue("@case_color_code", sCaseColorCode);
            cmd.Parameters.AddWithValue("@case_color_other", sCaseColorOther);
            cmd.Parameters.AddWithValue("@case_size", sCaseSize);
            cmd.Parameters.AddWithValue("@case_size_calc", sCaseSizeCalc);
            cmd.Parameters.AddWithValue("@catalog_number", sCatalogNumber);
            cmd.Parameters.AddWithValue("@catalog_number_oem", sCatalogNoOEM);
            cmd.Parameters.AddWithValue("@custom_id", iCustomID);
            cmd.Parameters.AddWithValue("@customer_tag_no", sCustomerTagNo);
            cmd.Parameters.AddWithValue("@efficiency", sEfficiency);
            cmd.Parameters.AddWithValue("@efficiency_exempt_reason", sEfficiencyExemptReason);
            cmd.Parameters.AddWithValue("@efficiency_set_by_admin", iEfficiencySetByAdmin);
            cmd.Parameters.AddWithValue("@electrostatic_shield", sElectrostaticShield);
            cmd.Parameters.AddWithValue("@enclosure", sEnclosure);
            cmd.Parameters.AddWithValue("@enclosure_mtl_code", sEnclosureMtlCode);
            cmd.Parameters.AddWithValue("@expedite_days", iExpediteDays);
            cmd.Parameters.AddWithValue("@expedite_price", decExpeditePrice);
            cmd.Parameters.AddWithValue("@frequency_code", sFrequencyCode);
            cmd.Parameters.AddWithValue("@impedance_oem", sImpedanceOEM);
            cmd.Parameters.AddWithValue("@is_for_export", iIsForExport);
            cmd.Parameters.AddWithValue("@is_hide_kva", iIsHideKVA);
            cmd.Parameters.AddWithValue("@is_hide_volt_primary", iIsHideVoltPrimary);
            cmd.Parameters.AddWithValue("@is_hide_volt_secondary", iIsHideVoltSecondary);
            cmd.Parameters.AddWithValue("@is_same_as_stock", iIsSameAsStock);
            cmd.Parameters.AddWithValue("@is_step_up", iIsStepUp);
            cmd.Parameters.AddWithValue("@is_taps_none", iIsTapsNone);
            cmd.Parameters.AddWithValue("@kfactor", sKFactor);
            cmd.Parameters.AddWithValue("@kit_lug_number", sKitLugNumber);
            cmd.Parameters.AddWithValue("@kit_lug_price", decKitLugPrice);
            cmd.Parameters.AddWithValue("@kit_lug_quantity", iKitLugQuantity);
            cmd.Parameters.AddWithValue("@kit_number", sKitNumber);
            cmd.Parameters.AddWithValue("@kit_op_number", sKitOPNumber);
            cmd.Parameters.AddWithValue("@kit_op_price", decKitOPPrice);
            cmd.Parameters.AddWithValue("@kit_op_quantity", iKitOPQuantity);
            cmd.Parameters.AddWithValue("@kit_price", decKitPrice);
            cmd.Parameters.AddWithValue("@kit_quantity", iKitQuantity);
            cmd.Parameters.AddWithValue("@kit_rb_number", sKitRBNumber);
            cmd.Parameters.AddWithValue("@kit_rb_price", decKitRBPrice);
            cmd.Parameters.AddWithValue("@kit_rb_quantity", iKitRBQuantity);
            cmd.Parameters.AddWithValue("@kit_wb_number", sKitWBNumber);
            cmd.Parameters.AddWithValue("@kit_wb_price", decKitWBPrice);
            cmd.Parameters.AddWithValue("@kit_wb_quantity", iKitWBQuantity);
            cmd.Parameters.AddWithValue("@kva_entered", dKVAEntered);
            cmd.Parameters["@kva_entered"].Precision = 9;
            cmd.Parameters["@kva_entered"].Scale = 1;
            cmd.Parameters.AddWithValue("@kva_used", dKVAUsed);
            cmd.Parameters["@kva_used"].Precision = 9;
            cmd.Parameters["@kva_used"].Scale = 1;
            cmd.Parameters.AddWithValue("@made_in_usa_codes", sMadeInUSACodes);
            cmd.Parameters.AddWithValue("@marine_duty", bMarineDuty);
            cmd.Parameters.AddWithValue("@notes_internal", sNotesInternal);
            cmd.Parameters.AddWithValue("@phase", sPhase);
            cmd.Parameters.AddWithValue("@price_calced", decPriceCalced);
            cmd.Parameters.AddWithValue("@price_entered", decPriceEntered);
            cmd.Parameters.AddWithValue("@price_list", decPriceList);
            cmd.Parameters.AddWithValue("@primary_dw", sPrimaryVoltageDW);
            cmd.Parameters.AddWithValue("@primary_voltage", sPrimaryVoltage);
            cmd.Parameters.AddWithValue("@quantity", iQuantity);
            cmd.Parameters.AddWithValue("@secondary_dw", sSecondaryVoltageDW);
            cmd.Parameters.AddWithValue("@secondary_voltage", sSecondaryVoltage);
            cmd.Parameters.AddWithValue("@ship_amount", curShipAmount);
            cmd.Parameters.AddWithValue("@ship_days", iShipDays);
            cmd.Parameters.AddWithValue("@ship_reason", sShipReason);
            cmd.Parameters.AddWithValue("@ship_weight", iShipWeight);
            cmd.Parameters.AddWithValue("@sound_reduct_code", sSoundReductCode);
            cmd.Parameters.AddWithValue("@special_feature_codes", sSpecialFeatureCodes);
            cmd.Parameters.AddWithValue("@special_feature_notes", sSpecialFeatureNotes);
            cmd.Parameters.AddWithValue("@special_type_code", sSpecialTypeCode);
            cmd.Parameters.AddWithValue("@standard_or_custom", sStandardOrCustom);
            cmd.Parameters.AddWithValue("@stock_configuration", sConfiguration);
            cmd.Parameters.AddWithValue("@stock_id", iStockID);
            cmd.Parameters.AddWithValue("@taps_oem", sTapsOEM);
            cmd.Parameters.AddWithValue("@temp_entered", iTempEntered);
            cmd.Parameters.AddWithValue("@temp_used", iTempUsed);
            cmd.Parameters.AddWithValue("@totally_enclosed", bTotallyEnclosed);
            cmd.Parameters.AddWithValue("@user_name", sUserName);
            cmd.Parameters.AddWithValue("@windings", sWindings);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Copy selected item.  Called after successfully saving the item to be copies.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <param name="iDetailID"></param>
        /// <returns></returns>
        public DataTable CopyQuoteItem(int iQuoteID, int iDetailID)
        {
            string sSQL = "usp_QuoteItem_Copy";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@quote_id", SqlDbType.Int);
            cmd.Parameters.Add("@detail_id", SqlDbType.Int);

            cmd.Parameters["@quote_id"].Value = iQuoteID;
            cmd.Parameters["@detail_id"].Value = iDetailID;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Select used to refresh KVA list.
         /// </summary>
        /// <param name="bCustom"></param>
        /// <param name="bWindingAluminum"></param>
        /// <param name="bPhaseSingle"></param>
        /// <returns></returns>
        public DataTable KVA(bool bCustom, bool bPhaseSingle, bool bWindingsCopper, bool bZigZag)
        {
            string sSQL = "SELECT KVAText as KVA FROM KVAs WHERE ";

            if (bCustom == true)
            {
                if (bPhaseSingle == true)
                {
                    sSQL = sSQL + "Custom_Single=1";

                    if (bZigZag == true)
                    {
                        sSQL = sSQL + " AND Custom_ZigZag=1";
                    }
                }
                else
                {
                    sSQL = sSQL + "Custom_Three=1";

                    if (bZigZag == true)
                    {
                        sSQL = sSQL + " AND Custom_ZigZag=1";
                    }
                }
            }
            else
            {
                if (bPhaseSingle == true)
                {
                    sSQL = sSQL + "StockSS_D16=1 OR StockSS_TP1=1";
                }
                else
                {
                    if (bWindingsCopper == true)
                    {
                        sSQL = sSQL + "StockDSC_D16=1 OR StockYSC_D16=1 OR " +
                                      "StockDSC_TP1=1 OR StockYSC_TP1=1";
                    }
                    else
                    {
                        sSQL = sSQL + "StockCS_D16=1 OR StockDS_D16=1 OR StockYS_D16=1 OR " +
                                      "StockCS_TP1=1 OR StockDS_TP1=1 OR StockYS_TP1=1";
                    }
                }
            }

            sSQL = sSQL + " ORDER BY KVAText";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        // Select used to refresh KFactor list (Custom only).
        /// <summary>
        /// Note: bTenv is whether TENV is selected, not whether it is available.
        /// </summary>
        /// <param name="sPhase"></param>
        /// <param name="sKVA"></param>
        /// <param name="bExempt"></param>
        /// <param name="bAdmin"></param>
        /// <param name="bTenv"></param>
        /// <returns></returns>
        public DataTable KFactor(string sPhase, string sKVA, bool bExempt, bool bAdmin, bool bTenv)
        {
            // AltKey1 = 1     Not used for KVA > 750, three phase.
            // AltKey1 = 2     Not used for KVA > 600, three phase.
            // bExempt = False  K-20 doesn't appear.
            string sSQL = "SELECT CodeValue as KFactor FROM Codes WHERE CodeType='FeatureKFactor'";

            // Allow Admin to pick any K-Factor.
            if (bAdmin == true)
            {
                sKVA = "75";  // default
            }

            decimal dKVA = 0;
            decimal.TryParse(sKVA, out dKVA);

            if (sPhase == "Three")
            {
                if (bTenv == false)
                {
                    if (dKVA > 750)
                    {
                        sSQL = sSQL + " AND ISNULL(AltKey1,'') NOT IN ('1','2')";  // K-1 only.
                    }
                    else if (dKVA > 600)
                    {
                        sSQL = sSQL + " AND ISNULL(AltKey1,'') != '2'";     // K-1, K-4, K-6.
                    }
                    // K-1, K-4, K-6, K-9, K-13
                    if (dKVA > 150)
                    {
                        sSQL = sSQL + " AND CodeValue <> 'K-20'";
                    }
                    // K-1, K-4, K-6, K-9, K-13, K-20
                }
                else
                {
                    // TENV.
                    if (dKVA > 300)
                    {
                        sSQL = " AND AltKey3 = '0'";        // No K-Factors.
                    }
                    else if (dKVA > 250)
                    {
                        sSQL = sSQL + " AND ISNULL(AltKey1,'') NOT IN ('1','2')";   // K-1 only.
                    }
                    else if (dKVA > 225)
                    {
                        sSQL = sSQL + " AND ISNULL(AltKey1,'') != '2'";     // K-1, K-4, K-6.
                    }
                    // K-1, K-4, K-6, K-9, K-13

                    // Believe that K-20 not available for TENV.
                    sSQL = sSQL + " AND CodeValue <> 'K-20'";
               }
            }
            else if (sPhase == "Single")
            {
                sSQL = sSQL + " AND CodeValue <> 'K-20'";
            }
            sSQL = sSQL + " ORDER BY SortOrder";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        // Select used to refresh Frequency list (Custom only).
        public DataTable Frequency()
        {
            string sSQL = "SELECT CodeValue as Frequency FROM Codes WHERE CodeType='FeatureFrequency' ORDER BY SortOrder";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        // Select used to refresh MadeInUSA list (Custom only).
        public DataTable MadeInUSA()
        {
            string sSQL = "SELECT CodeValue as MadeInUSA FROM Codes WHERE CodeType='FeatureMadeInUSA' ORDER BY SortOrder";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        // Select used to refresh MadeInUSA list (Custom only).
        public DataTable SpecialFeatures(bool bInternal, bool bExempt, string sPhase)
        {
            string sSQL = "SELECT CodeValue as SpecialFeatures FROM Codes WHERE CodeType='FeatureSpecial' ";
            // Only internal reps see code where AltKey1 = 'I' (Other Features).
            if (bInternal == false)
                sSQL += " AND ISNULL(AltKey1,'') <> 'I'";

            if (sPhase == "Single" || bExempt == true)
                sSQL += " AND CodeValue <> 'K-Factor 20 (K-20)'";

            sSQL += " ORDER BY SortOrder";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        // Select used to refresh SoundReduct list (Custom only).
        public DataTable SoundReduct(int iKVA)
        {
            string sSQL = "usp_Sound_Levels_20190402";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@kva_actual", iKVA);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        // Select used to refresh Enclosure list (Custom only).
        public DataTable EnclosureList(int iAllowOutdoor, int iAllowTENV)
        {
            string sSQL = "usp_Enclosure_List";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@allow_indoor", iAllowOutdoor);
            cmd.Parameters.AddWithValue("@allow_tenv", iAllowTENV);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        // Select used to refresh SpecialTypes list (Custom only).
        public DataTable SpecialTypes()
        {
            string sSQL = "SELECT CodeValue as SpecialTypes FROM Codes WHERE CodeType='FeatureSpecialTypes' ORDER BY SortOrder";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        // Select used to refresh CaseColor list (Custom only).
        public DataTable CaseColor(bool bStainless)
        {
            string sSQL = "SELECT CodeValue as CaseColor FROM Codes WHERE CodeType='FeatureCaseColor'";

            if (bStainless == false)
                sSQL = sSQL + " AND IsNull(AltKey1,'') <>'1'";

            sSQL = sSQL + " ORDER BY SortOrder";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Case Size admin dropdown.
        /// </summary>
        /// <returns></returns>
        public DataTable CaseSizes()
        {
            string sSQL = "SELECT CodeValue as CaseSize FROM Codes WHERE CodeType='CaseSize' ORDER BY SortOrder";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        public DataTable EncloseAttrib()
        {
            string sSQL = "SELECT CodeValue as EncloseAttrib FROM Codes WHERE CodeType='FeatureEncloseAttrib' ORDER BY SortOrder";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        // Select used to refresh TempRise list (Custom only).
        public DataTable TempRise(string sPhase, string sKVA, string sKFactor, bool bAdmin, bool bTENV)
        {
            // If Admin, allow them to see any TempRise, regardless.
            if (bAdmin == true)
            {
                sKVA = "75";
                sKFactor = "K-1 (STD)";
            }

            // Uses AltKey1 to deselect 80, then 115 C Rise if the case size gets too large.
            int iKFactorGroup = 1;
            switch (sKFactor)
            {
                case "K-1 (STD)":
                case "K-1":
                    iKFactorGroup = 1;
                    break;
                case "K-4":
                case "K-6":
                    iKFactorGroup = 2;
                    break;
                case "K-9":
                case "K-13":
                    iKFactorGroup = 3;
                    break;
                case "K-20":
                    iKFactorGroup = 4;
                    break;
            }

            string sSQL = "SELECT CodeValue as TempRise FROM Codes WHERE CodeType='FeatureTempRise'";
            decimal dKVA = 0;
            decimal.TryParse(sKVA, out dKVA);

            // Level:   0       No temperatures.
            //          1       150
            //          2       115, 150
            //          3       80, 115, 150

            int iLevel = 3;
            if (sPhase == "Three")
            {
                switch (iKFactorGroup)
                {
                    case 1:
                        // K-1. 
                        if (bTENV == false)
                        {
                            // > 1000 N/A
                            if (dKVA > 1000)
                                iLevel = 0;
                            // 751 - 1000 => 150
                            else if (dKVA > 750)
                                iLevel = 1;
                            // 9 - 750 => 80, 115, 150
                            break;
                        }
                        else
                        {
                            // > 300 N/A
                            if (dKVA > 300)
                                iLevel = 0;
                            // 251 - 300 => 150
                            else if (dKVA > 250)
                                iLevel = 1;
                            // 9 - 250 => 80, 115, 150
                            break;
                        }
                    case 2:
                        // K-4, K-6
                        if (bTENV == false)
                        {
                            // > 750  N/A
                            if (dKVA > 750)
                                iLevel = 0;
                            // 601 - 750 => 115, 150
                            else if (dKVA > 600)
                                iLevel = 2;
                            // 9 - 600 => 80, 115, 150
                            break;
                        }
                        else
                        {
                            // > 250  N/A
                            if (dKVA > 250)
                                iLevel = 0;
                            // 226 - 250 => 115, 150
                            else if (dKVA > 225)
                                iLevel = 2;
                            // 9 - 225 => 80, 115, 150
                            break;
                        }

                    case 3:
                        // K-9, K-13
                        if (bTENV == false)
                        {
                            // > 600  N/A
                            if (dKVA > 600)
                                iLevel = 0;
                            // 501 - 600 => 115, 150
                            else if (dKVA > 500)
                                iLevel = 2;
                            // 9 - 500 => 80, 115, 150
                            break;
                        }
                        else
                        {
                            // > 225  N/A
                            if (dKVA > 225)
                                iLevel = 0;
                            // 176 - 225 => 115, 150
                            else if (dKVA > 175)
                                iLevel = 2;
                            // 9 - 175 => 80, 115, 150
                            break;
                        }
                    case 4:
                        // K-20
                        // K-9, K-13
                        if (bTENV == false)
                        {
                            // > 150  N/A
                            if (dKVA > 150)
                                iLevel = 0;
                            // 9 - 150 => 80, 115, 150
                            break;
                        }
                        else
                        {
                            // K-20 not supported for TENV, I think.
                            iLevel = 0;
                            break;
                        }
                }
            }
            else if (sPhase == "Single")
            {
                switch (iKFactorGroup)
                {
                    case 1:
                        // K-1
                        // > 500  N/A
                        if (dKVA > 500)
                            iLevel = 0;
                        // 9 - 500 => 80, 115, 150
                        break;
                    case 2:
                        // K-4, K-6
                        // > 500  N/A
                        if (dKVA > 500)
                            iLevel = 0;
                        else if (dKVA > 333)
                            // 334 - 500 => 115, 150
                            iLevel = 2;
                        // 9 - 333 => 80, 115, 150
                        break;
                    case 3:
                        // K-9, K-13
                        // > 500  N/A 
                        if (dKVA > 500)
                            iLevel = 0;
                        // 334 - 500 => 150
                        else if (dKVA == 500)
                            iLevel = 1;
                        // 9 - 333 => 80, 115, 150
                        break;
                    case 4:
                        // K-20
                        // > 100  N/A 
                        if (dKVA > 100)
                            iLevel = 0;
                        // 9 - 100 => 80, 115, 150
                        break;
                }
            }

            switch (iLevel)
            {
                case 0:
                    sSQL = sSQL + " AND ISNULL(AltKey3,'') <> 'C'";             // Excludes all temperatures.
                    break;
                case 1:
                    sSQL = sSQL + " AND ISNULL(AltKey1,'') NOT IN ('1','2')";   // Excludes 80 and 115 C Rise
                    break;
                case 2:
                    sSQL = sSQL + " AND ISNULL(AltKey1,'') <> '2'";             // Excludes 80 C Rise.
                    break;
                case 3:
                    break;
            }

            sSQL = sSQL + " AND AltKey3='C' ORDER BY SortOrder";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);

                    int iCnt = ds.Tables[0].Rows.Count;
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        // Select Stock Configurations.
        public DataTable StockConfigurations(decimal dKVA, bool bPhaseSingle, bool bWindingsCopper)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand("usp_Stock_Configurations", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.Parameters.AddWithValue("@kva", dKVA);
            cmd.Parameters.AddWithValue("@phase_single", bPhaseSingle);
            cmd.Parameters.AddWithValue("@windings_copper", bWindingsCopper);

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Going from Stock to Custom, returns similar values for voltages, given configuration.
        /// </summary>
        /// <param name="sConfiguration"></param>
        /// <returns></returns>
        public DataTable ConfigurationToCustom(string sConfiguration)
        {
            string sSQL = "usp_Configuration_ToCustom";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@configuration", SqlDbType.VarChar, 50);

            cmd.Parameters["@configuration"].Value = sConfiguration;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Going from Custom to Stock, returns similar values for configuration, given voltages.
        /// </summary>
        /// <param name="sConfiguration"></param>
        /// <returns></returns>
        public DataTable ConfigurationToStock(string sPrimaryVoltage, string sSecondaryVoltage)
        {
            string sSQL = "usp_Configuration_ToStock";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@primary_voltage", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@secondary_voltage", SqlDbType.VarChar, 50);

            cmd.Parameters["@primary_voltage"].Value = sPrimaryVoltage;
            cmd.Parameters["@secondary_voltage"].Value = sSecondaryVoltage;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        // Select used to refresh custom Voltage lists (Primary or Secondary, Delta or Wye or Harmonic, Single or Three Phase.
        public DataTable Voltage(bool bPrimary, bool bDelta, bool bHarmonic, bool bSingle)
        {
            string sSQL = "SELECT Voltage FROM VoltageText WHERE ";

            if (bPrimary)
            {
                sSQL += "IsPrimary=1 AND ";   // Exclude 240D/120CT from Primary list.
            }
            else
            {
                sSQL += "IsSecondary=1 AND ";   // Exclude 800 D from Secondary list.
            }

            // IsWye = 0 for all single phase.
            if (bSingle)
            {
                sSQL += "IsSinglePhase=1";
            }
            else
            {
                sSQL += "IsSinglePhase=0 AND ";
                if (bHarmonic)
                {
                    sSQL += "IsHarmonicMitigating=1";
                }
                else if (bDelta)
                {
                    sSQL += "IsWye=0 AND IsHarmonicMitigating=0";
                }
                else
                {
                    sSQL += "IsWye=1";
                }
            }

            sSQL += " ORDER BY Seq";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns True if Phase selection is enabled, False otherwise.
        /// </summary>
        /// <param name="bCustom"></param>
        /// <param name="bWindingAluminum"></param>
        /// <returns></returns>
        public bool PhaseEnable(bool bCustom, bool bWindingAluminum)
        {
            if (bCustom || bWindingAluminum)
                return true;

            return false;
        }

        /// <summary>
        /// Selects (Standard) or builds (custom) Catalog number.
        /// Also brings back StockID (or CustomStockID), KitID, KitNumber, Enclosure, Case Size,
        /// Approval Required, and 
        /// Returns IsMatch = 1 if the custom configuration matches a standard one.
        /// </summary>
        public DataTable CatalogNo(string sCaseColor, string sConfiguration, string sCustomerTag,
                               string sEfficiency, int iEfficiencySetByAdmin, string sElecShield,
                               int iExpediteNoDays, int iForExport, int iLug, int iOSHPD, bool bPriceReset,
                               string sFrequency, string sKFactor, int iKitQty, string sKVA, bool bKvaHide, string sMadeInUSA,
                               bool bMarineDuty, string sNEMA, string sPhase, decimal decPriceEntered,
                               string sPrimaryVoltage, string sPrimaryVoltDW, bool bPrimaryVoltHide, int iQuantity, int iQuoteID, int iQuoteDetailsID,
                               int iRepDistributorID, string sSecondaryVoltage, string sSecondaryVoltDW, bool bSecondaryVoltHide,
                               string sSoundReduct, string sSpecialFeature, string sSpecialType,
                               string sEnclosureMtl, string sStandardOrCustom, string sTemperature,
                               int iTENV, string sUserName, string sWindings)
        {
            string sSQL = "usp_Catalog_Number_20190724";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@case_color", sCaseColor);
            cmd.Parameters.AddWithValue("@configuration", sConfiguration);
            cmd.Parameters.AddWithValue("@customer_tag", sCustomerTag);
            cmd.Parameters.AddWithValue("@efficiency", sEfficiency);
            cmd.Parameters.AddWithValue("@efficiency_set_by_admin", iEfficiencySetByAdmin);
            cmd.Parameters.AddWithValue("@electrostatic_shield", sElecShield);
            cmd.Parameters.AddWithValue("@enclosure_mtl", sEnclosureMtl);
            cmd.Parameters.AddWithValue("@expedite_no_days", iExpediteNoDays);
            cmd.Parameters.AddWithValue("@frequency", sFrequency);
            cmd.Parameters.AddWithValue("@is_for_export", iForExport);
            cmd.Parameters.AddWithValue("@is_lug_kit", iLug);
            cmd.Parameters.AddWithValue("@is_oshpd_kit", iOSHPD);
            cmd.Parameters.AddWithValue("@is_price_reset", bPriceReset);
            cmd.Parameters.AddWithValue("@kfactor", sKFactor);
            cmd.Parameters.AddWithValue("@kit_qty", iKitQty);
            cmd.Parameters.AddWithValue("@kva_hide", bKvaHide);
            cmd.Parameters.AddWithValue("@kva_text", sKVA);
            cmd.Parameters.AddWithValue("@login_name", sUserName);
            cmd.Parameters.AddWithValue("@made_in_usa_codes", sMadeInUSA);
            cmd.Parameters.AddWithValue("@marine_duty", bMarineDuty);
            cmd.Parameters.AddWithValue("@nema_code", sNEMA);
            cmd.Parameters.AddWithValue("@phase", sPhase);
            cmd.Parameters.AddWithValue("@price_entered", decPriceEntered);
            cmd.Parameters.AddWithValue("@primary_voltage", sPrimaryVoltage);
            cmd.Parameters.AddWithValue("@primary_volt_dw", sPrimaryVoltDW);
            cmd.Parameters.AddWithValue("@primary_volt_hide", bPrimaryVoltHide);
            cmd.Parameters.AddWithValue("@quantity", iQuantity);
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            cmd.Parameters.AddWithValue("@quote_details_id", iQuoteDetailsID);
            cmd.Parameters.AddWithValue("@rep_distributor_id", iRepDistributorID);
            cmd.Parameters.AddWithValue("@secondary_voltage", sSecondaryVoltage);
            cmd.Parameters.AddWithValue("@secondary_volt_dw", sSecondaryVoltDW);
            cmd.Parameters.AddWithValue("@secondary_volt_hide", bSecondaryVoltHide);
            cmd.Parameters.AddWithValue("@sound_reduct", sSoundReduct);
            cmd.Parameters.AddWithValue("@special_features", sSpecialFeature);
            cmd.Parameters.AddWithValue("@special_type", sSpecialType);
            cmd.Parameters.AddWithValue("@standard_or_custom", sStandardOrCustom);
            cmd.Parameters.AddWithValue("@temperature", sTemperature);
            cmd.Parameters.AddWithValue("@totally_enclosed", iTENV);
            cmd.Parameters.AddWithValue("@user_name", sUserName);
            cmd.Parameters.AddWithValue("@windings", sWindings);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Updates current Quote with possibly new information.
        /// Creates a new Customer if information in the selected customer has changed Company, City or Contact Name
        /// and the previous information was different from the current.  Returns QuoteID.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <param name="sProject"></param>
        /// <param name="iCustomerID"></param>
        /// <param name="sCompany"></param>
        /// <param name="sCity"></param>
        /// <param name="sContactName"></param>
        /// <param name="sEmail"></param>
        /// <param name="sNotes"></param>
        /// <returns></returns>
        public DataSet QuoteUpdate(int iQuoteID, string sCity, string sCompany, string sContactName,
                                   int iCustomerContactID, int iCustomerID, string sEmail, bool bNoDrawingsAttached,
                                   bool bOEM, bool bWiringDiagram, bool bNoFreeShipping, 
                                   string sNotes, string sNotesInternal, string sNotesPDF, string sNotesRequest,
                                   string sProject, string sQuoteOriginCode, int iRepDistributorID, int iRepID,
                                   string sUserName, string sUserNameLast)
        {
            string sSQL = "usp_Quote_Update_20190716";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            cmd.Parameters.AddWithValue("@city", sCity);
            cmd.Parameters.AddWithValue("@company", sCompany);
            cmd.Parameters.AddWithValue("@contact_name", sContactName);
            cmd.Parameters.AddWithValue("@customer_contact_id", iCustomerContactID);
            cmd.Parameters.AddWithValue("@customer_id", iCustomerID);
            cmd.Parameters.AddWithValue("@email", sEmail);
            cmd.Parameters.AddWithValue("@is_no_drawings_attached", bNoDrawingsAttached);
            cmd.Parameters.AddWithValue("@is_oem", bOEM);
            cmd.Parameters.AddWithValue("@is_wiring_diagram", bWiringDiagram);
            cmd.Parameters.AddWithValue("@no_free_shipping", bNoFreeShipping);
            cmd.Parameters.AddWithValue("@notes", sNotes);
            cmd.Parameters.AddWithValue("@notes_internal", sNotesInternal);
            cmd.Parameters.AddWithValue("@notes_pdf", sNotesPDF);
            cmd.Parameters.AddWithValue("@notes_request", sNotesRequest);
            cmd.Parameters.AddWithValue("@project", sProject);
            cmd.Parameters.AddWithValue("@quote_origin", sQuoteOriginCode);
            cmd.Parameters.AddWithValue("@rep_distributor_id", iRepDistributorID);
            cmd.Parameters.AddWithValue("@rep_id", iRepID);
            cmd.Parameters.AddWithValue("@user_name", sUserName);
            cmd.Parameters.AddWithValue("@user_name_last", sUserNameLast);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }


        /// <summary>
        /// Finalizes current Quote.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <returns></returns>
        public void QuoteFinalize(int iQuoteID, string sUserName)
        {
            string sSQL = "usp_Quote_Finalize_New";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            cmd.Parameters.AddWithValue("@user_name", sUserName);

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

        /// <summary>
        /// Delete Quote, only if no detail records, after inserting a detail fails.
        /// </summary>
        /// <param name="iQuoteID"></param>
        public void QuoteDelete(int iQuoteID)
        {
            string sSQL = "usp_Quote_Delete";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);

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


        /// <summary>
        /// Create request approval email or approval/denial email,
        /// and mark quote as finalized if approved.
        /// Environment:  DEV, PROD, QA.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <param name="iApprovedOrDenied"></param>
        /// <param name="iRequestApproval"></param>
        /// <param name="sUserNameApproval"></param>
        /// <returns></returns>
        public bool QuoteApprove(int iQuoteID, int iApprovedOrDenied, int iRequestApproval,
                            string sUserNameApproval, string sEnvironment)
        {
            string sEmailType = iRequestApproval == 1 ? "QuoteRequest" : "QuoteApprove";
            string sToEmail = string.Empty;
            string sBody = string.Empty;
            string sSubject = string.Empty;
            List<SqlParameter> lstParams = new List<SqlParameter>();
            List<SqlParameter> lstParamsEmail = new List<SqlParameter>();
            string sAttach = string.Empty;

            string sToName = string.Empty;
            string sCCEmail = string.Empty;
            string sCCName = string.Empty;

            //int iAttempts = -1;

            bool bRetValue = false;
            SqlConnection con;
            SqlCommand cmd;

            MailAddress from = null;


            // Old approach.

            if (Convert.ToBoolean(WebConfigurationManager.AppSettings["UseSQLMail"]))
            {
                // This version is for testing.
                using (con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString()))
                {
                    cmd = new SqlCommand("usp_Quote_Approve", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
                    cmd.Parameters.AddWithValue("@approved_or_denied", iApprovedOrDenied);
                    cmd.Parameters.AddWithValue("@request_approval", iRequestApproval);
                    cmd.Parameters.AddWithValue("@user_name_approval", sUserNameApproval);
                    cmd.Parameters.AddWithValue("@environment", sEnvironment);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error:" + ex);
                        bRetValue = false;
                    }
                }
            }

            // ===========
            // Production.
            // ===========


            if (Convert.ToBoolean(WebConfigurationManager.AppSettings["UseSMTPMail"]))
            {
                using (con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString()))
                {


                    cmd = new SqlCommand("usp_Quote_Approve_20171107", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
                    cmd.Parameters.AddWithValue("@approved_or_denied", iApprovedOrDenied);
                    cmd.Parameters.AddWithValue("@request_approval", iRequestApproval);
                    cmd.Parameters.AddWithValue("@user_name_approval", sUserNameApproval);
                    cmd.Parameters.AddWithValue("@environment", sEnvironment);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error:" + ex);
                        bRetValue = false;
                    }



                    DataSet dsEmailApprove = DataLink.Select("select * from QuoteEmails where QuoteID = " +
                          iQuoteID + " and QuoteEmailID = (select max(QuoteEmailID) from QuoteEmails where QuoteID = " + iQuoteID + ")", DataLinkCon.mgmuser);


                    if (dsEmailApprove.Tables.Count > 0)
                    {
                        if (dsEmailApprove.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsEmailApprove.Tables[0].Rows)
                            {
                                sToEmail = dr["EmailTo"].ToString();
                                sBody = dr["Body"].ToString();
                                sSubject = dr["Subject"].ToString();
                                sAttach = dr["Attachments"].ToString();
                                sToName = dr["EmailToName"].ToString();
                                sCCEmail = dr["EmailCC"].ToString();
                                sCCName = dr["EmailCCName"].ToString();
                            }
                        }
                    }


                    if (!Convert.ToBoolean(WebConfigurationManager.AppSettings["UpdateEmailTablesOnly"]))
                    {

                        SmtpClient client = new SmtpClient(WebConfigurationManager.AppSettings["ExchangeServerIPAddress"]);
                        client.UseDefaultCredentials = true;

                        string[] sTos = sToEmail.Split(';');
                        string[] sTosNames = sToName.Split(';');



                        MailAddress to = new MailAddress(sTos[0], sTosNames[0], System.Text.Encoding.UTF8);


                        if (HttpContext.Current.Session["Internal"].ToString() == "1")
                        {
                            from = new MailAddress(HttpContext.Current.Session["Email"].ToString(), HttpContext.Current.Session["UserName"].ToString(), System.Text.Encoding.UTF8);
                        }
                        else
                        {
                            from = new MailAddress("quotes@mgmtransformer.com", HttpContext.Current.Session["UserName"].ToString(), System.Text.Encoding.UTF8);
                        }


                        MailMessage message = new MailMessage(from, to);

                        for (int i = 1; i < sTos.Count(); i++)
                        {
                            if (sTosNames.Count() < (i + 1))
                                message.To.Add(new MailAddress(sTos[i], sTos[i], System.Text.Encoding.UTF8));
                            else
                                message.To.Add(new MailAddress(sTos[i], sTosNames[i], System.Text.Encoding.UTF8));
                        }


                        message.Body = sBody;

                        if (sCCEmail != "")
                            message.CC.Add(sCCEmail);

                        message.BodyEncoding = System.Text.Encoding.UTF8;
                        message.Subject = sSubject;
                        message.SubjectEncoding = System.Text.Encoding.UTF8;

                        if (sAttach != "")
                            message.Attachments.Add(new Attachment(sAttach));


                        client.Send(message);
                        message.Dispose();

                    }


                    try
                    {

                        DataLink.Update("insert into EMailPendingLog (Attempts,DateTimeAdded,DateTimeVerified,EmailTypeCode,ErrorText,QuoteEmailID,[Status],UserEmail,UserName) values (" +
                                            "1,GetDate(),GetDate(),'" + sEmailType + "',''," + Utility.GetQuoteEmailID(iQuoteID) + ",1,'" + HttpContext.Current.Session["Email"].ToString() +
                                            "','" + HttpContext.Current.Session["UserName"] + "')", DataLinkCon.mgmuser);

                    }
                    catch (Exception ex)
                    {

                        try
                        {
                            DataLink.Update("insert into EMailPendingLog (Attempts,DateTimeAdded,DateTimeVerified,EmailTypeCode,ErrorText,QuoteEmailID,[Status],UserEmail,UserName) values (" +
                                            "1,GetDate(),null,'" + sEmailType + "','" + ex.Message + "',-1,0,'" + HttpContext.Current.Request.QueryString["Email"].ToString() +
                                              "','" + HttpContext.Current.Session["UserName"].ToString() + "')", DataLinkCon.mgmuser);
                        }
                        catch (SqlException exSQL)
                        {
                            System.Console.WriteLine(exSQL.Message);

                            bRetValue = false;
                        }

                        bRetValue = false;
                    }
                    finally
                    {
                        bRetValue = true;
                        con.Close();
                    }

                }



            }


            return bRetValue;
        }


        /// <summary>
        /// Updates Notes after the quote has been finalized.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <param name="sNotes"></param>
        /// <returns></returns>
        public bool QuoteUpdateNotes(int iQuoteID, string sNotes)
        {
            string sSQL = "usp_Quote_Update_Notes";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@quote_id", SqlDbType.Int);
            cmd.Parameters.Add("@notes", SqlDbType.VarChar, 1000);

            cmd.Parameters["@quote_id"].Value = iQuoteID;
            cmd.Parameters["@notes"].Value = sNotes;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return Convert.ToBoolean(ds.Tables[0].Rows[0]["UpdateSuccessful"]);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Used in Stock Price Configurations screen to list configurations.
        /// Returns StockID (key), Display, Phase, Winding, Configuration fields.
        /// </summary>
        /// <returns></returns>
        public DataTable StockConfigs(bool bIncludeTP1)
        {
            string sSQL = "usp_Stock_Configs_20190924";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@sort_order", 0);
            cmd.Parameters.AddWithValue("@include_tp1", bIncludeTP1);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Used in Stock Price Configurations screen to get Phase, Winding and Configurations from selected StockID.
        /// Returns StockID (key), Display, Phase, Winding, Configuration fields.
        /// </summary>
        /// <returns></returns>
        public DataTable StockConfigs_GetParams(int iSortOrder)
        {
            string sSQL = "usp_Stock_Configs_New";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@sort_order", iSortOrder);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Copy a quote.  Expect quote to be Finalized at this point.
        /// Returns new QuoteNoVer, RepID and RepDistributorID.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <returns></returns>
        public DataSet QuoteCopy(int iQuoteID, int iSameQuoteNo, string sUserName)
        {
            string sSQL = "usp_Quote_Copy_20180724";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@quote_id", SqlDbType.Int);
            cmd.Parameters.Add("@same_quote_no", SqlDbType.Int);
            cmd.Parameters.Add("@user_name", SqlDbType.VarChar, 50);

            cmd.Parameters["@quote_id"].Value = iQuoteID;
            cmd.Parameters["@same_quote_no"].Value = iSameQuoteNo;
            cmd.Parameters["@user_name"].Value = sUserName;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Get PDFUrl from finalized Quote.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <returns></returns>
        public string QuotePDFUrl(int iQuoteID, bool bNoPrice, bool bSubmittal)
        {
            string sSQL = "SELECT QuoteNo, QuoteNoVer, PDFUrl, PDFSubmittal FROM Quote WHERE QuoteID = @quote_id";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@quote_id", SqlDbType.Int);
            cmd.Parameters["@quote_id"].Value = iQuoteID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            int iQuoteNo = 0;
            int iQuoteVersion = 0;

            string sPDFUrl = "";
            string sPDFUrlBestMatch = "";

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);

                    if (bSubmittal == true)
                        sPDFUrl = ds.Tables[0].Rows[0]["PDFSubmittal"].ToString();
                    else if (bNoPrice == true)
                        sPDFUrl = ds.Tables[0].Rows[0]["PDFUrlNoPrice"].ToString();
                    else
                        sPDFUrl = ds.Tables[0].Rows[0]["PDFUrl"].ToString();

                    iQuoteNo = Convert.ToInt32(ds.Tables[0].Rows[0]["QuoteNo"]);
                    iQuoteVersion = Convert.ToInt32(ds.Tables[0].Rows[0]["QuoteNoVer"]);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }

            sPDFUrlBestMatch = dv.PDFExists(sPDFUrl, iQuoteNo, iQuoteVersion, bNoPrice, bSubmittal);
            return sPDFUrlBestMatch;
        }

        /// <summary>
        /// Returns text of freight included message, based on the quote.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <returns></returns>
        public string FreightIncluded(int iQuoteID)
        {
            string sMsg = "";
            string sSQL = "usp_Quote_FreightIncluded";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@quote_id", SqlDbType.Int);

            cmd.Parameters["@quote_id"].Value = iQuoteID;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    switch (Convert.ToInt32(ds.Tables[0].Rows[0]["FreightIncluded"]))
                    {
                        case 0:
                            sMsg = "<b><font color='red'>Freight costs are NOT INCLUDED</font></b> on this quote, as it has only Stock items totalling < $1,000.";
                            break;
                        case 1:
                            sMsg = "<b>Freight costs</b> are included <b>to representative's sales territory</b>, as it has Stock items >= $1,000.";
                            break;
                        case 2:
                            sMsg = "<b>Freight</b> to any of 48 contiguous states comes at <b>no charge</b>, as it has Custom items.";
                            break;
                        case 3:         // No message, as there are no details on this quote.
                            sMsg = "";
                            break;
                    }
                    return sMsg;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        public DataTable BullRushSend(int iQuoteID)
        {
            string sSQL = "usp_BullRush_Send";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@quote_id", SqlDbType.Int);
            cmd.Parameters["@quote_id"].Value = iQuoteID;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Return a DetailID for a given QuoteDetailsID.
        /// </summary>
        /// <param name="iQuoteDetailsID"></param>
        /// <returns></returns>
        public Int16 GetDetailID(int iQuoteDetailsID)
        {
            string sSQL = "SELECT DetailID FROM QuoteDetails WHERE QuoteDetailsID=" + iQuoteDetailsID;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return Convert.ToInt16(ds.Tables[0].Rows[0]["DetailID"]);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return 0;
                }
            }
        }

        /// <summary>
        /// Turn status to Cart, by Edit button.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <returns></returns>
        public bool QuoteEdit(int iQuoteID, string sUserName)
        {
            string sSQL = "usp_Quote_Edit_New";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            cmd.Parameters.AddWithValue("@user_name", sUserName);

            using (con)
            {
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Return amount of Expedite Fees.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <param name="iDetailID"></param>
        /// <param name="iNoDays"></param>
        /// <returns></returns>
        public Int32 ExpediteFees(int iQuoteID, decimal dUnitPrice, int iNoDays)
        {
            string sSQL = "usp_ExpediteFees";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            SqlDataReader dr;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@quote_id", SqlDbType.Int);
            cmd.Parameters.Add("@unit_price", SqlDbType.Money);
            cmd.Parameters.Add("@no_days", SqlDbType.Int);

            cmd.Parameters["@quote_id"].Value = iQuoteID;
            cmd.Parameters["@unit_price"].Value = dUnitPrice;
            cmd.Parameters["@no_days"].Value = iNoDays;

            Int32 iExpediteFees = 0;
            using (con)
            {
                try
                {
                    con.Open();
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        iExpediteFees = Convert.ToInt32(dr["ExpediteFees"]);
                    }
                    return iExpediteFees;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return 0;
                }
            }
        }

        /// <summary>
        ///  Prepare the data for the Quote PDF.  Return the PDF File Name.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <returns></returns>
        public DataTable QuotePDF(int iQuoteID, string sUser, bool bNoPrice, bool bSubmittal)
        {
            string sSQL = "usp_Rpt_Quote_20191029";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            cmd.Parameters.AddWithValue("@login_name", sUser);
            cmd.Parameters.AddWithValue("@is_no_price", bNoPrice);
            cmd.Parameters.AddWithValue("@is_submittal", bSubmittal);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        // Rep Name dropdown.
        public DataTable RepName(int iRepID)
        {
            string sSQL = "SELECT UserName, FullName FROM Logins WHERE RepID = " + iRepID + " ORDER BY UserName";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Approves all prices, including kits.  Appends results to ApprovalReason.
        /// Uses PriceDiscount table, having bounds for increase and decrease percents.
        /// </summary>
        /// <param name="decPriceCalced"></param>
        /// <param name="decPriceEntered"></param>
        /// <returns></returns>
        public string ApprovalPrice(int iQuoteID, decimal decExpeditePriceCalced, decimal decExpeditePriceEntered,
                                decimal decKitPriceCalced, decimal decKitPriceEntered, decimal decKitRBPriceCalced, decimal decKitRBPriceEntered,
                                decimal decKitOPPriceCalced, decimal decKitOPPriceEntered, decimal decKitWBPriceCalced, decimal decKitWBPriceEntered,
                                decimal decUnitPriceCalced, decimal decUnitPriceEntered, string sKitName)
        {
            string sSQL = "usp_Approval_Price_New";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            cmd.Parameters.AddWithValue("@expedite_price_calced", decExpeditePriceCalced);
            cmd.Parameters.AddWithValue("@expedite_price_entered", decExpeditePriceEntered);
            cmd.Parameters.AddWithValue("@kit_name", sKitName);
            cmd.Parameters.AddWithValue("@kit_price_calced", decKitPriceCalced);
            cmd.Parameters.AddWithValue("@kit_price_entered", decKitPriceEntered);
            cmd.Parameters.AddWithValue("@kit_rb_price_calced", decKitRBPriceCalced);
            cmd.Parameters.AddWithValue("@kit_rb_price_entered", decKitRBPriceEntered);
            cmd.Parameters.AddWithValue("@kit_op_price_calced", decKitOPPriceCalced);
            cmd.Parameters.AddWithValue("@kit_op_price_entered", decKitOPPriceEntered);
            cmd.Parameters.AddWithValue("@kit_wb_price_calced", decKitWBPriceCalced);
            cmd.Parameters.AddWithValue("@kit_wb_price_entered", decKitWBPriceEntered);
            cmd.Parameters.AddWithValue("@unit_price_calced", decUnitPriceCalced);
            cmd.Parameters.AddWithValue("@unit_price_entered", decUnitPriceEntered);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0].Rows[0]["ApprovalReasonPrice"].ToString();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Return True if this rep can be allowed to see this quote.
        /// Internal reps, whether Admin or not, can see all quotes.
        /// </summary>
        /// <param name="iQuoteNo"></param>
        /// <param name="iRepID"></param>
        /// <returns></returns>
        public Boolean QuoteFilter(int iQuoteNo, int iRepID)
        {
            string sSQL = "usp_Quote_Filter";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@quote_no", iQuoteNo);
            cmd.Parameters.AddWithValue("@rep_id", iRepID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return Convert.ToBoolean(ds.Tables[0].Rows[0]["QuoteAllowed"]);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Records progess towards an order.  Comes in from email responses.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <param name="sLostReasonCode"></param>
        /// <param name="sLostToCode"></param>
        /// <param name="sNotes"></param>
        /// <param name="sProgressCode"></param>
        public void QuoteProgress(int iQuoteID, string sLostReasonCode,
                                    string sLostToCode, string sNotes, string sProgressCode)
        {
            const string sSQL = "usp_Quote_Progress";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@lost_reason_code", sLostReasonCode);
            cmd.Parameters.AddWithValue("@lost_to_code", sLostToCode);
            cmd.Parameters.AddWithValue("@notes", sNotes);
            cmd.Parameters.AddWithValue("@progress_code", sProgressCode);
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);

            using (con)
            {
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// List of Progress Codes for progress email.
        /// </summary>
        /// <returns></returns>
        public DataTable ProgressCodes()
        {
            const string sSQL = "SELECT CodeValue AS Progress FROM Codes WHERE CodeType='Progress' AND CodeValue != 'Ordered' ORDER BY SortOrder";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);

            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// List of Lost Reason Codes for progress email.
        /// </summary>
        /// <returns></returns>
        public DataTable LostReasonCodes()
        {
            const string sSQL = "SELECT CodeValue as LostReason FROM Codes WHERE CodeType='LostReason' ORDER BY SortOrder";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);

            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// List of Lost To Codes for progress email.
        /// </summary>
        /// <returns></returns>
        public DataTable LostToCodes()
        {
            const string sSQL = "SELECT CodeValue AS LostTo FROM Codes WHERE CodeType='LostTo' ORDER BY SortOrder";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);

            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// Update quote with data from gvQuote in the Home screen. (Progress towards sale.)
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <param name="sProgressCode"></param>
        /// <param name="sLostReasonCode"></param>
        /// <param name="sLostToCode"></param>
        /// <returns></returns>
        public string QuoteActiveUpate(int iQuoteID, string sProgressCode, string sLostReasonCode, string sLostToCode, string sFollowupDate)
        {
            const string sSQL = "usp_Quote_Active_Update_New";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            cmd.Parameters.AddWithValue("@progress_code", sProgressCode);
            cmd.Parameters.AddWithValue("@lost_reason_code", sLostReasonCode);
            cmd.Parameters.AddWithValue("@lost_to_code", sLostToCode);
            cmd.Parameters.AddWithValue("@followup_date", sFollowupDate);

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    return ds.Tables[0].Rows[0]["Msg"].ToString();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// Efficiency drop-down.
        /// </summary>
        /// <returns></returns>
        public DataTable Efficiencies()
        {
            string sSQL = "SELECT CodeValue AS Efficiency FROM Codes WHERE CodeType='FeatureEfficiencyNew' ORDER BY SortOrder";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Create and save the quote or submittal PDF, 
        /// optionally with no prices, for emailing as an attachment.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <param name="bNoPrice"></param>
        /// <param name="bSubmittal"></param>
        /// <param name="sUserName"></param>
        /// <returns></returns>
        //public bool CreatePDF(int iQuoteID, bool bNoPrice, bool bSubmittal)
        //{

        //    var r = new ReportDocument();
        //    try
        //    {
        //        // Update the dataset first.
        //        var q = new Quotes();
        //        string sUserName = HttpContext.Current.Session["UserName"].ToString();

        //        DataTable dt = q.QuotePDF(iQuoteID, sUserName, bNoPrice, bSubmittal);
        //        DataRow dr = dt.Rows[0];

        //        string sFileName = dr["PDFFileName"].ToString();
        //        string sBaseDir = System.AppDomain.CurrentDomain.BaseDirectory;
        //        string sPathAndFn = sBaseDir + "pdfs\\" + sFileName;
        //        string sRptSource = sBaseDir + "QuoteNew.rpt";
        //        r.Load(sRptSource);

        //        r.SetDatabaseLogon("user", "password", "mgmuser", "mgmPassword4");
        //        r.SetParameterValue("@quote_id", iQuoteID);

        //        // Delete this file if it exists, before writing out new one.
        //        string sExportPathAndFN = "C:\\MGMQuotation\\pdfs\\" + sFileName;

        //        if (File.Exists(sExportPathAndFN) == true)
        //        {
        //            File.Delete(sExportPathAndFN);
        //            // Sometimes doesn't delete the file.
        //            System.Threading.Thread.Sleep(2000);
        //        }

        //        r.ExportToDisk(ExportFormatType.PortableDocFormat, sExportPathAndFN);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return false;
        //    }
        //    finally
        //    {
        //        r.Dispose();
        //    }
        //    return true;
        //}

        public DataTable GetCatalogInfo(string sCatalogNo)
        {
            const string sSQL = "usp_Catalog_Build";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@catalog_no", sCatalogNo);

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                    {
                        return dt;      // DataTable with no records in it.
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }


        public DataTable InventoryByLocation(decimal iKVA, string sVoltageCategory, string sWinding, int iRepID, int iWarehouseNo)
        {
            const string sSQL = "usp_RptInventoryByLocSource_20170911";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["bpss"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            iKVA = Math.Floor(iKVA);


            cmd.Parameters.AddWithValue("@kva", iKVA);
            cmd.Parameters.AddWithValue("@voltage_category", sVoltageCategory);
            cmd.Parameters.AddWithValue("@winding", sWinding);
            cmd.Parameters.AddWithValue("@rep_id", iRepID);
            cmd.Parameters.AddWithValue("@warehouse_no", iWarehouseNo);
            cmd.CommandType = CommandType.StoredProcedure;


            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                    {
                        return dt;      // DataTable with no records in it.
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }



        }



        /// <summary>
        /// Get a list of Agents.
        /// </summary>
        /// <returns></returns>
        public DataTable AgentsWithWarehouses(bool bRemoveMGM)
        {
            const string sSQL = "usp_Agent_List_20190315";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@rep_id", 0);
            cmd.Parameters.AddWithValue("@include_only_warehouse", 1);

            if (bRemoveMGM)
                cmd.Parameters.AddWithValue("@remove_mgm", 1);
            else
                cmd.Parameters.AddWithValue("@remove_mgm", 0);

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                    {
                        return dt;      // DataTable with no records in it.
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }




        /// <summary>
        /// Get a list of Agents.
        /// </summary>
        /// <returns></returns>
        public DataTable Agents()
        {
            const string sSQL = "usp_Agent_List_20190315";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@rep_id", 0);

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                    {
                        return dt;      // DataTable with no records in it.
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// Get Excel export data for M1 report.
        /// </summary>
        /// <param name="sAgentNo"></param>
        /// <param name="iDateFrom"></param>
        /// <param name="iDateTo"></param>
        /// <returns></returns>
        public DataTable M1Data(string sAgentNo, int iDateFrom, int iDateTo)
        {
            const string sSQL = "usp_RptM1Source_Export";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@agent_no", sAgentNo);
            cmd.Parameters.AddWithValue("@date_from", iDateFrom);
            cmd.Parameters.AddWithValue("@date_to", iDateTo);

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                        return ds.Tables[0];
                    else
                    {
                        return dt;      // DataTable with no records in it.
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// Mail a Rich Text File.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <param name="sQuoteFull"></param>
        /// <param name="sToEmail"></param>
        /// <param name="sToName"></param>
        /// <param name="sRTFFullPath"></param>
        public bool MailAttach(int iQuoteID, string sQuoteFull, string sToEmail, string sToName, string sRTFFullPath)
        {
            bool bRetValue = true;
            int iAttempts = -1;
            MailAddress from = null;


            if (Convert.ToBoolean(WebConfigurationManager.AppSettings["UseSQLMail"]))
            {
                string sSQL = "usp_Email_Send_New";
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
                SqlCommand cmd = new SqlCommand(sSQL, con);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
                cmd.Parameters.AddWithValue("@to_email", sToEmail);
                cmd.Parameters.AddWithValue("@to_name", sToName);
                cmd.Parameters.AddWithValue("@cc_email", "Davis.Debard@mgmtransformer.com");
                cmd.Parameters.AddWithValue("@cc_name", "");
                cmd.Parameters.AddWithValue("@subj", sQuoteFull + " Word doc");
                cmd.Parameters.AddWithValue("@body_text", "NOTE:  Save this zip file on your machine.\nIn File Explorer, locate it, and extract it.\nOpen the DOC file in Word.\nSave it as a Word DOC (not a DOCX) with a different file name.\nIt will get MUCH SMALLER.  Attach this smaller file to an email.");
                cmd.Parameters.AddWithValue("@attach", sRTFFullPath);
                cmd.Parameters.AddWithValue("@is_no_price", 0);

                using (con)
                {
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error:" + ex.Message);
                        bRetValue = false;
                    }
                }

            }


            // Would be production, but not in use...

            if (Convert.ToBoolean(WebConfigurationManager.AppSettings["UseSMTPMail"]))
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString()))
                {
                    iQuoteID = Convert.ToInt32(HttpContext.Current.Request.QueryString["QuoteID"]);
                    //Quotes q = new Quotes();


                    SqlCommand cmd = new SqlCommand("usp_Email_Send_20171019", con);
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
                        cmd.Parameters.AddWithValue("@to_name", sToName);
                        cmd.Parameters.AddWithValue("@to_email", sToEmail);
                        cmd.Parameters.AddWithValue("@cc_name", "");
                        cmd.Parameters.AddWithValue("@cc_email", HttpContext.Current.Request.QueryString["Email"]);
                        cmd.Parameters.AddWithValue("@subj", sQuoteFull + " Word doc");
                        cmd.Parameters.AddWithValue("@body_text", "NOTE:  Save this zip file on your machine.\nIn File Explorer, locate it, and extract it.\nOpen the DOC file in Word.\nSave it as a Word DOC (not a DOCX) with a different file name.\nIt will get MUCH SMALLER.  Attach this smaller file to an email.");
                        cmd.Parameters.AddWithValue("@attach", sRTFFullPath);
                        cmd.Parameters.AddWithValue("@is_no_price", 0);
                        cmd.Parameters.AddWithValue("@mail_type", "Approve");
                        cmd.Parameters.AddWithValue("@user_name", HttpContext.Current.Session["UserName"]);

                        con.Open();
                        int numrows = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        System.Console.WriteLine(ex.Message);
                        bRetValue = false;
                    }
                    finally
                    {
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

                                MailAddress to = new MailAddress(sToEmail, "MGM Quote System", System.Text.Encoding.UTF8);


                                if (HttpContext.Current.Session["Internal"].ToString() == "1")
                                {
                                    from = new MailAddress(HttpContext.Current.Session["Email"].ToString(), "MGM Quote System", System.Text.Encoding.UTF8);
                                }
                                else
                                {
                                    from = new MailAddress("quotes@mgmtransformer.com", "MGM Quote System", System.Text.Encoding.UTF8);
                                }


                                MailMessage message = new MailMessage(from, to);
                                message.Body = "NOTE:  Save this zip file on your machine.\nIn File Explorer, locate it, and extract it.\nOpen the DOC file in Word.\nSave it as a Word DOC (not a DOCX) with a different file name.\nIt will get MUCH SMALLER.  Attach this smaller file to an email.";

                                message.BodyEncoding = System.Text.Encoding.UTF8;
                                message.Subject = sQuoteFull + " Word doc";
                                message.SubjectEncoding = System.Text.Encoding.UTF8;

                                client.Send(message);
                                message.Dispose();
                            }

                            iAttempts = DataLink.EmailPendingExists(Utility.GetQuoteEmailID(iQuoteID), DataLinkCon.mgmuser);

                            if (iAttempts > -1)
                            {
                                DataLink.Update("update EMailPendingLog set Attempts = ," + (iAttempts + 1) + ",DateTimeVerified = '" +
                                       DateTime.Now.ToShortDateString() + "',[Status] = 1 where QuoteEmailID = " + Utility.GetQuoteEmailID(iQuoteID), DataLinkCon.mgmuser);
                            }
                            else
                            {

                                DataLink.Update("insert into EMailPendingLog (Attempts,DateTimeAdded,DateTimeVerified,EmailTypeCode,ErrorText,QuoteEmailID,[Status],UserEmail,UserName) values (" +
                                                "1,GetDate(),GetDate(),'Mail Attach',''," + Utility.GetQuoteEmailID(iQuoteID) + ",1,'" + HttpContext.Current.Request.QueryString["Email"].ToString() +
                                                "','" + HttpContext.Current.Session["UserName"] + "')", DataLinkCon.mgmuser);
                            }
                        }
                        catch (Exception ex)
                        {
                            iAttempts = DataLink.EmailPendingExists(Utility.GetQuoteEmailID(iQuoteID), DataLinkCon.mgmuser);

                            if (iAttempts < 0)
                            {
                                DataLink.Update("insert into EMailPendingLog (Attempts,DateTimeAdded,DateTimeVerified,EmailTypeCode,ErrorText,QuoteEmailID,[Status],UserEmail,UserName) values (" +
                                                  "1,GetDate(),null,'Mail Attach','" + ex.Message + "',-1,',0,'" + HttpContext.Current.Request.QueryString["Email"].ToString() +
                                                    "','" + HttpContext.Current.Session["UserName"].ToString() + "')", DataLinkCon.mgmuser);
                            }

                            bRetValue = false;
                        }


                    }

                }
            }


            return bRetValue;
        }


        public bool MailOEM(int iQuoteID, string sQuoteFull, string sToEmail, string sToName, string sFullPath)
        {
            bool bRetValue = true;
            MailAddress from = null;

            if (Convert.ToBoolean(WebConfigurationManager.AppSettings["UseSQLMail"]))
            {

                string sSQL = "usp_Email_Send_New";
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
                SqlCommand cmd = new SqlCommand(sSQL, con);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
                cmd.Parameters.AddWithValue("@to_email", sToEmail);
                cmd.Parameters.AddWithValue("@to_name", sToName);
                cmd.Parameters.AddWithValue("@cc_email", HttpContext.Current.Request.QueryString["Email"].ToString());
                cmd.Parameters.AddWithValue("@cc_name", "");
                cmd.Parameters.AddWithValue("@subj", sQuoteFull + " Word doc");
                cmd.Parameters.AddWithValue("@body_text", "Please find a web quote OEM Word document at: " + sFullPath);
                cmd.Parameters.AddWithValue("@attach", "");
                cmd.Parameters.AddWithValue("@is_no_price", 0);

                using (con)
                {
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error:" + ex.Message);
                        bRetValue = false;
                    }
                }

            }


            if (Convert.ToBoolean(WebConfigurationManager.AppSettings["UseSMTPMail"]))
            {

                //sToName = HttpContext.Current.Session["UserName"].ToString();
                //sToEmail = HttpContext.Current.Session["Email"].ToString();

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString()))
                {

                    SqlCommand cmd = new SqlCommand("usp_Email_Send_20171107", con);
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
                        cmd.Parameters.AddWithValue("@to_name", sToName);
                        cmd.Parameters.AddWithValue("@to_email", sToEmail);
                        cmd.Parameters.AddWithValue("@cc_name", "");
                        cmd.Parameters.AddWithValue("@cc_email", HttpContext.Current.Session["Email"].ToString());
                        cmd.Parameters.AddWithValue("@subj", sQuoteFull + " Word doc");
                        cmd.Parameters.AddWithValue("@body_text", "Please find a web quote OEM Word document at: " + sFullPath);
                        cmd.Parameters.AddWithValue("@attach", "");
                        cmd.Parameters.AddWithValue("@is_no_price", 0);
                        cmd.Parameters.AddWithValue("@mail_type", "Approve");
                        cmd.Parameters.AddWithValue("@user_name", HttpContext.Current.Session["UserName"].ToString());

                        con.Open();
                        int numrows = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        System.Console.WriteLine(ex.Message);
                        bRetValue = false;
                    }
                    finally
                    {
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

                                MailAddress to = new MailAddress(sToEmail, "MGM Quote System", System.Text.Encoding.UTF8);

                                if (HttpContext.Current.Session["Email"].ToString() == "1")
                                {
                                    from = new MailAddress(HttpContext.Current.Session["Email"].ToString(), "MGM Quote System", System.Text.Encoding.UTF8);
                                }
                                else
                                {
                                    from = new MailAddress("quotes@mgmtransformer.com", "MGM Quote System", System.Text.Encoding.UTF8);
                                }


                                MailMessage message = new MailMessage(from, to);
                                message.Body = "Please find a web quote OEM Word document at: " + sFullPath;

                                message.BodyEncoding = System.Text.Encoding.UTF8;
                                message.Subject = sQuoteFull + " Word doc";
                                message.SubjectEncoding = System.Text.Encoding.UTF8;

                                client.Send(message);
                                message.Dispose();
                            }




                            DataLink.Update("insert into EMailPendingLog (Attempts,DateTimeAdded,DateTimeVerified,EmailTypeCode,ErrorText,QuoteEmailID,[Status],UserEmail,UserName) values (" +
                                                 "1,GetDate(),GetDate(),'OEM',''," + Utility.GetQuoteEmailID(iQuoteID) + ",1,'" + HttpContext.Current.Session["Email"].ToString() +
                                                 "','" + HttpContext.Current.Session["UserName"] + "')", DataLinkCon.mgmuser);


                        }
                        catch (Exception ex)
                        {


                            try
                            {
                                DataLink.Update("insert into EMailPendingLog (Attempts,DateTimeAdded,DateTimeVerified,EmailTypeCode,ErrorText,QuoteEmailID,[Status],UserEmail,UserName) values (" +
                                                      "1,GetDate(),null,'OEM','" + ex.Message + "',-1,0,'" + HttpContext.Current.Session["Email"].ToString() +
                                                        "','" + HttpContext.Current.Session["UserName"].ToString() + "')", DataLinkCon.mgmuser);
                            }
                            catch (SqlException exSQL)
                            {
                                System.Console.WriteLine(exSQL.Message);
                                bRetValue = false;
                            }

                            bRetValue = false;
                        }



                    }

                }
            }

            return bRetValue;
        }



        /// <summary>
        /// Return the name of a successfully zipped file.
        /// </summary>
        /// <param name="sPathAndFileNameNoExt"></param>
        /// <param name="sFileExt"></param>
        /// <returns></returns>
        public string ZipFileName(string sPathAndFileNameNoExt, string sFileExt)
        {
            // Strip a leading period if we have one.
            if (sFileExt.Substring(0, 1) == ".")
                sFileExt = sFileExt.Substring(1, sFileExt.Length - 1);

            // The original full path and file name.
            string sPathAndFileName = sPathAndFileNameNoExt + "." + sFileExt;

            // The zipped full path and file name.
            string sPathAndZipFileName = sPathAndFileNameNoExt + ".zip";


            using (ZipFile zip = new ZipFile())
            {
                try
                {
                    // Exclude path.
                    string sPath = "\\\\MGMQuotation\\pdfs";
                    zip.AddFile(sPathAndFileName, sPath);
                    zip.Save(sPathAndZipFileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return "";
                }
            }
            return sPathAndZipFileName;
        }

        /// <summary>
        /// Export M1 data to Excel
        /// </summary>
        /// <param name="sAgentNo"></param>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        public void ExportM1Data(string sAgentNo, DateTime dtFrom, DateTime dtTo)
        {
            int iDateFrom = 20161101;
            int iDateTo = 20161201;

            DataTable dt = new DataTable();

            dt = M1Data(sAgentNo, iDateFrom, iDateTo);

            // Not issuing a class-wide "using Microsoft.Office.Interop.Excel" statement because some of the names
            // such as DataTable conflict with SQL definitions.
            //var excelApp = new Microsoft.Office.Interop.Excel.Application();

            //excelApp.Visible = true;
            //excelApp.Workbooks.Add();

            //Microsoft.Office.Interop.Excel._Worksheet xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelApp.ActiveSheet;

            //xlWorkSheet.Cells[1, "A"] = "Agent #";
            //xlWorkSheet.Cells[1, "B"] = "Agent Name";
            //xlWorkSheet.Cells[1, "C"] = "Customer Name";
            //xlWorkSheet.Cells[1, "D"] = "Agent Name";
            //xlWorkSheet.Cells[1, "E"] = "Agent Name";
            //xlWorkSheet.Cells[1, "F"] = "Agent Name";
            //xlWorkSheet.Cells[1, "G"] = "Agent Name";
            //xlWorkSheet.Cells[1, "H"] = "Agent Name";
            //xlWorkSheet.Cells[1, "I"] = "Agent Name";
            //xlWorkSheet.Cells[1, "J"] = "Agent Name";
            //xlWorkSheet.Cells[1, "K"] = "Agent Name";
            //xlWorkSheet.Cells[1, "L"] = "Agent Name";
            //xlWorkSheet.Cells[1, "B"] = "Agent Name";

            //var row = 1;
            ////foreach (var acct in accounts)
            ////{
            //row++;
            //xlWorkSheet.Cells[row, "A"] = "A23";
            //xlWorkSheet.Cells[row, "B"] = 23;
            //}
        }

        /// <summary>
        /// Create and save the PDF, for preview or emailing as an attachment.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <param name="bNoPrice"></param>
        /// <param name="bOEM"></param>
        /// <param name="bFinalized"></param>
        /// <param name="bSubmittal"></param>
        /// <returns></returns>
        public bool CreatePDF(int iQuoteID, bool bNoPrice, bool bOEM, bool bFinalized, bool bSubmittal, out bool bOEMEmailSuccess)
        {
            bOEMEmailSuccess = true;

            var q = new Quotes();
            var r = new ReportDocument();
            try
            {
                // Update the dataset first.
                string sUserName = HttpContext.Current.Session["UserName"].ToString();

                // Rebuilds QuotePDF records for this QuoteID.
                // ===========================================
                DataTable dt = q.QuotePDF(iQuoteID, sUserName, bNoPrice, bSubmittal);
                // ======================================================
                DataRow dr = dt.Rows[0];

                string sFileName = dr["PDFFileName"].ToString();
                string sInternalFolder = dr["InternalFolder"].ToString();
                string sUserNameCreate = dr["FullName"].ToString();
                string sEmail = dr["Email"].ToString();

                string sBaseDir = System.AppDomain.CurrentDomain.BaseDirectory;

                // Hard code to Production, since we're having an error accessing the QA folder.
                //******************************************************************************
                string sPathAndFn = "";
                if (Convert.ToBoolean(WebConfigurationManager.AppSettings["LocalMachine"]) == true)
                {
                    sPathAndFn = WebConfigurationManager.AppSettings["LocalMachinePath"] + sFileName;
                }
                else
                    sPathAndFn = "C:\\MGMQuotation\\pdfs\\" + sFileName;
                //******************************************************************************

                string sRptSource = sBaseDir + "QuoteNew.rpt";

                r.Load(sRptSource);

                r.SetDatabaseLogon("user", "password", "mgmuser", "mgmPassword4");

                // This doesn't need the version flag.
                r.SetParameterValue("@quote_id", iQuoteID);

                HttpContext.Current.Response.Buffer = false;
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();

                // Delete this file if it exists, before writing out new one.

                if (File.Exists(sPathAndFn) == true)
                {
                    File.Delete(sPathAndFn);
                    // Sometimes doesn't delete the file.
                    System.Threading.Thread.Sleep(2000);
                }

                // Create a Word document for OEM's.
                if (bOEM == true && bFinalized == true)
                {
                    if (sFileName.Length > 4)
                    {
                        if (sFileName.Substring(sFileName.Length - 4, 4) == ".PDF")
                        {
                            sFileName = sFileName.Substring(0, sFileName.Length - 4);
                        }
                    }
                    sFileName = sFileName + ".DOC";

                    string sPathAndFnDoc = "\\\\MGMSVRW2K12FS\\users\\" + sInternalFolder + "\\" + sFileName;
                    //string sPathAndFnDoc = "\\\\mgmsvrw2k12fs\\mgmdata\\OEMQuotes\\" + sInternalFolder + "\\" + sFileName;

                    r.ExportToDisk(ExportFormatType.WordForWindows, sPathAndFnDoc);

                    //q.AddToTestLog("after " + sPathAndFnDoc);

                    // Default email and login name to blank, so it will send the email only to the quote originator,
                    // not the person finalizing it, which usually would be Sanju.

                    string sQuoteFull = HttpContext.Current.Session["QuoteNoDisplay"].ToString();

                    if (!q.MailOEM(iQuoteID, sQuoteFull, sEmail, sUserNameCreate, sPathAndFnDoc))
                        bOEMEmailSuccess = false;
                }

                r.ExportToDisk(ExportFormatType.PortableDocFormat, sPathAndFn);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Exception occurred: " + ex + ".");
                q.AddToTestLog("Exception occurred: " + ex + ".");
                return false;
            }
            finally
            {
                r.Dispose();
            }
            return true;
        }

        /// <summary>
        /// Edit submittal line.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <param name="iDetailID"></param>
        /// <returns></returns>
        public DataTable SubmittalEdit(int iQuoteID, int iDetailID)
        {
            string sSQL = "usp_Submittal_Edit";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            cmd.Parameters.AddWithValue("@detail_id", iDetailID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Load submittal quote information.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <returns></returns>
        public DataTable SubmittalLoad(int iQuoteID)
        {
            string sSQL = "usp_Submittal_Load";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        public void SubmittalNoteSave(int iQuoteID, int iDetailID, string sSubmittalNote, bool bSelected)
        {
            string sSQL = "usp_Submittal_Note_Save";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            cmd.Parameters.AddWithValue("@detail_id", iDetailID);
            cmd.Parameters.AddWithValue("@submittal_note", sSubmittalNote);
            cmd.Parameters.AddWithValue("@is_selected", bSelected);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
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

        public void SubmittalQuoteSave(int iQuoteID, bool bIncludeWiringDiagram, 
                                        string sPurchaseOrderNo, string sSalesOrderNo, string sSubmittalType)
        {
            string sSQL = "usp_Submittal_Quote_Save_20190816";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            cmd.Parameters.AddWithValue("@include_wiring_diagram", bIncludeWiringDiagram);
            cmd.Parameters.AddWithValue("@purchase_order_no", sPurchaseOrderNo);
            cmd.Parameters.AddWithValue("@sales_order_no", sSalesOrderNo);
            cmd.Parameters.AddWithValue("@submittal_type", sSubmittalType);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
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

        /// <summary>
        /// Return an error message if missing required fields at either the quote level or the quote detail level.
        /// </summary>
        /// <param name="iQuoteID"></param>
        public string SubmittalRequire(int iQuoteID)
        {
            string sErrorMsg = "";
            string sSQL = "usp_Submittal_Require";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    sErrorMsg = ds.Tables[0].Rows[0]["ErrorMsg"].ToString();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
            return sErrorMsg;
        }

        /// <summary>
        /// Add a character string up to 1,000 characters to the TestLog.
        /// </summary>
        /// <param name="iQuoteID"></param>
        public string AddToTestLog(string sValue)
        {
            string sErrorMsg = "";
            string sSQL = "usp_Admin_TestLog";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@value", sValue);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    sErrorMsg = ds.Tables[0].Rows[0]["ErrorMsg"].ToString();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
            return sErrorMsg;
        }
        /// <summary>
        /// Return True if the current quote has been identified as an OEM.
        /// </summary>
        /// <param name="iQuoteID"></param>
        public bool IsOEM(int iQuoteID)
        {
            bool bOEM = false;
            string sSQL = "select IsOEM from Quote where QuoteID=" + iQuoteID.ToString();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    bOEM = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsOEM"]);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return false;
                }
            }
            return bOEM;
        }

        /// <summary>
        /// Returns True when the submittal on this quote has been completed with valid entries.
        /// </summary>
        /// <param name="iQuoteID"></param>
        /// <returns></returns>
        public bool SubmittalValid(int iQuoteID)
        {
            string sSQL = "usp_Submittal_Require";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            string sErrorMsg = "";
            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);

                    if (ds.Tables.Count == 0)
                        return false;

                    if (ds.Tables[0].Rows.Count == 0)
                        return false;

                    sErrorMsg = ds.Tables[0].Rows[0][0].ToString();

                    if (sErrorMsg == "")
                        return true;
                    else
                    {
                        Console.WriteLine("Submittal invalid because: " + sErrorMsg);
                        return false;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return false;
                }
            }
        }

        public void SubmittalCheckAll(int iQuoteId, bool bCheck)
        {
            string sSQL = "usp_Submittal_CheckAll";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@quote_id", iQuoteId);
            cmd.Parameters.AddWithValue("@check_all", bCheck);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            using (con)
            {
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

        /// <summary>
        /// 12/10/18 DeBard added.
        /// </summary>
        /// <returns></returns>
        public DataTable SubmittalType()
        {
            string sSQL = "SELECT CodeValue AS SubmittalType FROM Codes WHERE CodeType='SubmittalType'";
            sSQL += " ORDER BY SortOrder";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            using (con)
            {
                try
                {
                    con.Open();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex);
                    return null;
                }
            }
        }

        public string ArticleFileName(int iArticleID)
        {
            string sSQL = "SELECT ArticleFileName FROM Articles WHERE ArticleID=" + iArticleID.ToString();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            SqlCommand cmd = new SqlCommand(sSQL, con);
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            cmd.CommandType = CommandType.Text;

            using (con)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error:" + ex.Message);
                    return null;
                }
            }
        }

    }
}