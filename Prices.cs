using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

namespace MGM_Transformer
{
    class Prices
    {
        private string sFileCustomAlum = @"\\macola\mgmdata\data\WebQuote\CustomStockAluminum.xls";     // L: = \\macola\mgmdata
        private string sFileCustomCopper = @"\\macola\mgmdata\data\WebQuote\CustomStockCopper.xls";
        private string sFileStockRep = @"\\macola\mgmdata\data\WebQuote\StockRepDistributors.xls";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // Get row counts in SQL Server table.
        protected int GetRowCounts(int priceType)
        {
            int iRowCount = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString()))
            {
                string sSelect = "select count(*) from ";

                switch (priceType)
                {
                    case 1:
                        sSelect = sSelect + "TestCustomStockPrice";     // ImportCustomPriceAluminum
                        break;
                    case 2:
                        sSelect = sSelect + "TestCustomStockPrice";      // ImportCustomPriceCopper
                        break;
                    case 3:
                        sSelect = sSelect + "TestStockRepXRef";          // ImportStockRepXRef
                        break;
                };

                SqlCommand cmd = new SqlCommand(sSelect, conn);
                conn.Open();

                iRowCount = (int)cmd.ExecuteScalar();
            }
            return iRowCount;
        }

        // Retrieve data from the Excel spreadsheet.
        protected DataTable RetrieveData(string sConn)
        {
            DataTable dtExcel = new DataTable();

            using (OleDbConnection conn = new OleDbConnection(sConn))
            {
                string sSelect = "select * from [Sheet1$]";

                OleDbDataAdapter da = new OleDbDataAdapter(sSelect, conn);

                da.Fill(dtExcel);
            }

            return dtExcel;
        }

        protected void SqlBulkCopyImport(DataTable dtExcel, int priceType)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString()))
            {
                conn.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    string dest = "";
                    switch (priceType)
                    {
                        case 1:
                            dest = "TestCustomStockPrice";          // ImportCustomPriceAluminum
                            break;
                        case 2:
                            dest = "TestCustomStockPrice";          // ImportCustomPriceCopper
                            break;
                        case 3:
                            dest = "TestStockRepXRef";              // ImportStockRepXRef
                            break;
                    }

                    bulkCopy.DestinationTableName = "dbo." + dest;

                    foreach (DataColumn dc in dtExcel.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                    }

                    bulkCopy.WriteToServer(dtExcel);
                }
            }
        }

        public bool Import(int priceType)
        {
            string sFileName = "";
            switch (priceType)
            {
                case 1:
                    sFileName = sFileCustomAlum;
                    break;
                case 2:
                    sFileName = sFileCustomCopper;
                    break;
                case 3:
                    sFileName = sFileStockRep;
                    break;
            }

            if (File.Exists(sFileName))
            {
                // Excel 2000-2003.  Not using Server.MapPath(sFileName) because it maps relative to C: drive.
                string sExcelConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFileName + ";Extended Properties='Excel 8.0;HDR=YES;'";

                // Excel 2007-2010.
                // string sExcelConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath(sFileName) + ";Extended Properties='Excel 12.0 Xml;HDR=YES;'";

                DataTable dtExcel = RetrieveData(sExcelConn);

                int iStartCount = GetRowCounts(priceType);       // May use later, to verify count of records updated.

                SqlBulkCopyImport(dtExcel, priceType);

                int iEndCount = GetRowCounts(priceType);         // May use later, to verify count of records updated.
            }

            if (priceType == 3)
            {
                if (!File.Exists(sFileCustomAlum) && !File.Exists(sFileCustomCopper) && !File.Exists(sFileStockRep))
                    return false;

                UpdateSQLTables();

                if (File.Exists(sFileCustomAlum)) File.Delete(sFileCustomAlum);
                if (File.Exists(sFileCustomCopper)) File.Delete(sFileCustomCopper);
                if (File.Exists(sFileStockRep)) File.Delete(sFileStockRep);
            }
            return true;
        }

        // Execute a stored procedure that updates SQL tables based 
        protected void UpdateSQLTables()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString()))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("usp_Test", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }


        public decimal ValidPrice(string Price)
        {
            string s = Price;

            if (s == null || s == "")
                return 0.00M;

            decimal decPrice = 0.00M;
            try
            {
                decPrice = Convert.ToDecimal(Price);
            }
            catch
            {
                decPrice = 0.00M;
            }
            return decPrice;
        }

        public int UpdateSinglePrice(int RepDistributorID, string Price)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
            
            decimal decPrice = ValidPrice(Price);

            SqlCommand cmd = new SqlCommand("usp_StockRepXRef_Price_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@StockRepID", SqlDbType.Int);
            cmd.Parameters.Add("@Price", SqlDbType.Money);

            cmd.Parameters["@StockRepID"].Value = RepDistributorID;
            cmd.Parameters["@Price"].Value = decPrice;

            try
            {
                con.Open();
                int _recordsAffected = cmd.ExecuteNonQuery();
                return _recordsAffected;
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
            finally
            {
                con.Close();
            }
        }
    }
}
