using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Net.Mail;
using System.IO;
//using Microsoft.Office.Interop.Outlook;

namespace MGM_Transformer
{
    public class Utility
    {

        static public int GetAgentNoFromRepID(int iRepID)
        {
            int iRetVal = -1;
            DataSet dsAgentNo;

            dsAgentNo = DataLink.Select("select MGMAgentNo from Rep where RepID = " + iRepID, DataLinkCon.mgmuser);

            if (dsAgentNo.Tables.Count > 0)
            {
                if (dsAgentNo.Tables[0].Rows.Count > 0)
                {
                    iRetVal = Convert.ToInt32(dsAgentNo.Tables[0].Rows[0]["MGMAgentNo"]);
                }

            }

            return iRetVal;
        }

        static public int GetQuoteEmailID(int iQuoteID)
        {
            int iQuoteEmailID = -1;

            DataSet dsQuote = DataLink.Select("select max(QuoteEmailID) as QuoteEmailID from QuoteEmails where QuoteID = " + iQuoteID, DataLinkCon.mgmuser);

            if (dsQuote.Tables.Count > 0)
            {
                if (dsQuote.Tables[0].Rows.Count > 0)
                {
                    if (!DBNull.Value.Equals(dsQuote.Tables[0].Rows[0]["QuoteEmailID"]))
                        iQuoteEmailID = Convert.ToInt32(dsQuote.Tables[0].Rows[0]["QuoteEmailID"]);
                }
            }

            return iQuoteEmailID;
        }


        static public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        static public string GetStringValue(DataSet ds, string sColumnName, int iIndex = 0)
        {
            string retVal = "";

            try
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > iIndex)
                    {
                        if (ds.Tables[0].Rows[iIndex].Table.Columns.Contains(sColumnName))
                        {
                            if (DBNull.Value.Equals(ds.Tables[0].Rows[iIndex][sColumnName]))
                                return null;

                            retVal = Convert.ToString(ds.Tables[0].Rows[iIndex][sColumnName]);
                        }
                    }
                }
            }
            catch (Exception ex) { return ""; }


            return retVal;
        }


        static public List<T> GetValueList<T>(DataSet ds, string sColumnName)
        {
            List<T> retVal = new List<T>();

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0].Table.Columns.Contains(sColumnName))
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (!DBNull.Value.Equals(dr[sColumnName]) && dr[sColumnName].ToString() != "")
                                retVal.Add((T)(dr[sColumnName]));

                        }
                    }
                }

            }

            return retVal;
        }

        static public decimal GetDecimalValue(DataSet ds, string sColumnName, int iIndex = 0)
        {
            decimal retValue = -1;

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > iIndex)
                {
                    if (ds.Tables[0].Rows[iIndex].Table.Columns.Contains(sColumnName))
                    {
                        if (!decimal.TryParse(ds.Tables[0].Rows[iIndex][sColumnName].ToString(), out retValue))
                            retValue = -1;
                    }
                }

            }

            return retValue;
        }

        /// <summary>
        /// If we have 112.5 KVA, for example, returns 112.
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        static public int IntFromString(string sValue)
        {
            if (sValue.IndexOf(".") > 0)
            {
                sValue = sValue.Substring(0, sValue.IndexOf("."));
            }
            
            int i = 0;

            int.TryParse(sValue, out i);
            return i;
        }


        static public int GetIntValue(DataSet ds, string sColumnName)
        {
            int retVal = -1;

            try
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0].Table.Columns.Contains(sColumnName))
                        {
                            if (ds.Tables[0].Rows[0][sColumnName] == DBNull.Value)
                                retVal = -1;
                            else
                                retVal = Convert.ToInt32(ds.Tables[0].Rows[0][sColumnName]);
                        }
                    }
                }
            }
            catch (Exception ex) { return -1; }
            return retVal;
        }

    }
}