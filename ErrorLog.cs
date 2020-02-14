using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGM_Transformer
{
    public static class ErrorLog
    {
        /// <summary>
        /// Write a message to the error log: mgmuser.LogError
        /// </summary>
        /// <param name="sErrorMsg"></param>
        public static void Write(string sErrorMsg)
        {
            sErrorMsg = sErrorMsg.Replace("'", "''");
            
            string sQuery = "INSERT INTO LogErrors(ErrorMsg) VALUES('" + sErrorMsg + "')";
            DataLink.Update(sQuery, DataLinkCon.mgmuser);
        }
    }
}