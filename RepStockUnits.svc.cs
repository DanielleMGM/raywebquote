using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MGM_Transformer
{
    public class StockUnit
    {
        [DataMember]
        public String phase;
        [DataMember]
        public String configuration;
        [DataMember]
        public String kva;
        [DataMember]
        public String catalog;
        [DataMember]
        public String alt;
        [DataMember]
        public String price;
        [DataMember]
        public String kit;
        [DataMember]
        public String kitprice;
        [DataMember]
        public String height;
        [DataMember]
        public String width;
        [DataMember]
        public String depth;
        [DataMember]
        public String weight;
        [DataMember]
        public String unitCase;
    }

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RepStockUnits" in code, svc and config file together.
    public class RepStockUnits : IRepStockUnits
    {
        #region --- Quote history --- 

        public string GetQuoteHistory()
        {
            string[] user = UserValidation.GetLoggedInUser();
            string _repId = user[0];
            if (_repId != "" && _repId.Length > 0)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
                con.Open();
                SqlCommand com = new SqlCommand("SELECT * FROM Quote WHERE RepID = " + _repId, con);
                SqlDataAdapter sda = new SqlDataAdapter(com);
                DataSet dsQuotes = new DataSet();
                sda.Fill(dsQuotes);
                con.Close();
                sda.Dispose();
                dsQuotes.Tables[0].TableName = "QuoteHistory";
                return Utilities.GetJSONString(dsQuotes.Tables[0]);
            }
            else
                return "login";
        }

        public string GetQuoteDetails(int quoteid)
        {
            string[] user = UserValidation.GetLoggedInUser();
            string _repId = user[0];
            if (_repId != "" && _repId.Length > 0)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
                con.Open();
                string strSql = "SELECT QuoteDetails.*, " +
                    "Stock.Windings as StockWindings, Stock.Phase as StockPhase, Stock.Configuration as StockConfiguration, Stock.KVA as StockKVA, Stock.CatalogNumber as StockCatalogNumber, Stock.AltNumber as StockAltNumber, Stock.Height as StockHeight, Stock.Width as StockWidth, Stock.Depth as StockDepth, Stock.Weight as StockWeight, Stock.UnitCase as StockUnitCase, " +
                    "CustomStock.Winding as CustomStockWindings, CustomStock.Phase as CustomStockPhase, CustomStock.KVA as CustomStockKVA, CustomStock.Temperature as CustomStockTemperature, CustomStock.KFactor as CustomStockKFactor, CustomStock.UnitCase as CustomStockUnitCase, CustomStock.Weight as CustomStockWeight, CustomStock.Price as CustomStockPrice, CustomStock.SoundLevels as CustomStockSoundLevels, CustomStock.AchieveableLevel as CustomStockAchieveableLevel, " +
                    "Kit.KitNumber, Kit.Price " +
                    "FROM QuoteDetails " +
                    "LEFT OUTER JOIN Stock on Stock.StockID = QuoteDetails.StockUnitID " +
                    "LEFT OUTER JOIN CustomStock on CustomStock.CustomStockID = QuoteDetails.CustomStockID " +
                    "LEFT OUTER JOIN Kit on Kit.KitID = QuoteDetails.KitID " +
                    "WHERE QuoteID = " + quoteid;
                SqlCommand com = new SqlCommand(strSql, con);
                SqlDataAdapter sda = new SqlDataAdapter(com);
                DataSet dsQuotes = new DataSet();
                sda.Fill(dsQuotes);
                con.Close();
                sda.Dispose();
                dsQuotes.Tables[0].TableName = "QuoteHistory";
                return Utilities.GetJSONString(dsQuotes.Tables[0]);
            }
            else
                return "login";
        }

        #endregion

        #region -- Custom Stock unit --

        //http://localhost:44409/RepStockUnits.svc/json/AddCustomStockToCart?winding=Aluminum&phase=Three&kva=15.00&temperature=150.00&kfactor=Standard&primaryVoltage=10&primaryVoltageDW=10&secondaryVoltage=10&secondaryVoltageDW=10&taps=10&soundlevel=10&quantity=5&quoteid=1&updateQuantity=true
        public int AddQuoteDetailToCart(int quoteid, int customid, int stockid, int kitid, string primaryVoltage, string primaryVoltageDW, 
            string secondaryVoltage, string secondaryVoltageDW, string taps, string soundlevel, int quantity, bool updateQuantity)
        {
            //string winding; string phase; string kva; string temperature; string kfactor;
            string[] user = UserValidation.GetLoggedInUser();
            string _repId = user[0];
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
            con.Open();
            SqlCommand com = null;
            if (customid != 0)
            {
                /*com = new SqlCommand("SELECT     " +
                            "CustomStock.CustomStockID, CustomStock.Price FROM " +
                          "CustomStock " +
                          "WHERE (CustomStock.Winding = '" + winding + "') " +
                        "AND (CustomStock.KVA = " + decimal.Parse(kva) + ") " +
                        "AND (CustomStock.temperature = " + decimal.Parse(temperature) + ") " +
                        "AND (CustomStock.kFactor = '" + kfactor + "') ", con);*/
                com = new SqlCommand("SELECT CustomStock.CustomStockID, CustomStock.Price FROM CustomStock " +
                          "WHERE CustomStock.CustomStockID =" + customid, con);
            }
            else if (stockid != 0)
            {
                com = new SqlCommand("SELECT     " +
                        "Stock.StockID, StockRepXRef.Price " +
                        "FROM         Rep INNER JOIN " +
                      "StockRepXRef ON Rep.RepID = StockRepXRef.RepID INNER JOIN " +
                      "Stock ON StockRepXRef.StockID = Stock.StockID " +
                      "WHERE (Rep.RepID = " + _repId + ") " +
                    "AND (Stock.StockID = " + stockid + ") ", con);
            }
            else
            {
                com = new SqlCommand("SELECT Price FROM Kit WHERE (kitid = " + kitid + ")", con);
            }
            SqlDataReader sdr = com.ExecuteReader();
            SqlDataReader sdrInsert;
            if (sdr.Read())
            {
                //string CustomStockId = (sdr["CustomStockID"] != null ? sdr["CustomStockID"].ToString() : null);
                string originalPrice = sdr["Price"].ToString();

                //string StockId = sdr["CustomStockID"].ToString();
                sdr.Close();

                if (quoteid == 0)
                {
                    com = new SqlCommand("INSERT INTO Quote(RepID, Created_on) " +
                            "VALUES(" + _repId + ", getdate()); SELECT @@IDENTITY as 'quoteid'; ", con);
                    sdrInsert = com.ExecuteReader();
                    if (sdrInsert.Read())
                    {
                        quoteid = int.Parse(sdrInsert["quoteid"].ToString());
                        sdrInsert.Close();
                        //Insert into QuoteDetails
                        com = new SqlCommand("INSERT INTO QuoteDetails(QuoteID, StockUnitID, CustomStockID, Price, CustomPrimaryVoltage, " +
                                    "CustomPrimaryVoltageDW, CustomSecondaryVoltage, CustomSecondaryVoltageDW, Taps, SoundLevel, " +
                                    "Quantity, KitID, Created_on) " +
                                    "VALUES(" + quoteid + ", " + stockid + ", " + customid + ", " + originalPrice + ", " +
                                    primaryVoltage + ", '" + primaryVoltageDW + "', " + secondaryVoltage + ", '" + secondaryVoltageDW + "', " + taps + ", " +
                                    soundlevel + ", " + quantity + ", " + kitid + ", getdate()) ", con);
                        com.ExecuteNonQuery();
                    }
                }
                else
                {
                    //Module to keep adding to the quote or update
                    int quoteDetailID = 0;
                    if (customid != 0)
                        com = new SqlCommand("SELECT * FROM QuoteDetails WHERE QuoteId = " + quoteid + " and CustomStockID = " + customid, con);
                    else if (stockid != 0)
                        com = new SqlCommand("SELECT * FROM QuoteDetails WHERE QuoteId = " + quoteid + " and StockUnitID = " + stockid, con);
                    else
                        com = new SqlCommand("SELECT * FROM QuoteDetails WHERE QuoteId = " + quoteid + " and kitID = " + kitid, con);
                    sdr = com.ExecuteReader();
                    if (sdr.Read())
                    {
                        int qty = 0;
                        if (!updateQuantity)
                            qty = int.Parse(sdr["Quantity"].ToString()) + quantity;
                        else
                            qty = quantity;
                        quoteDetailID = int.Parse(sdr["QuoteDetailsID"].ToString());
                        com = new SqlCommand("UPDATE QuoteDetails SET Quantity = " + qty + " Where QuoteDetailsID = " + quoteDetailID, con);
                    }
                    else
                    {
                        com = new SqlCommand("INSERT INTO QuoteDetails(QuoteID, StockUnitID, CustomStockID, Price, CustomPrimaryVoltage, " +
                                    "CustomPrimaryVoltageDW, CustomSecondaryVoltage, CustomSecondaryVoltageDW, Taps, SoundLevel, " +
                                    "Quantity, KitID, Created_on) " +
                                    "VALUES(" + quoteid + ", " + stockid + ", " + customid + ", " + originalPrice + ", " +
                                    primaryVoltage + ", '" + primaryVoltageDW + "', " + secondaryVoltage + ", '" + secondaryVoltageDW + "', " + taps + ", " +
                                    soundlevel + ", " + quantity + ", " + kitid + ", getdate()) ", con);
                    }
                    sdr.Close();
                    com.ExecuteNonQuery();
                }
            }
            con.Close();
            con.Dispose();
            return quoteid;
        }

        #endregion

        #region -- Stock Units Prices --

        #region -----  Ajax New Stock Quote ----

        /* ajax call to retrieve phase */
        public string GetStockPhase(string winding)
        {
            string[] user = UserValidation.GetLoggedInUser();
            string _repId = user[0];
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
            con.Open();
            SqlCommand com = new SqlCommand("SELECT DISTINCT Stock.Phase FROM Stock WHERE (Stock.Windings = '" + winding + "') ", con);

            SqlDataReader sdr = com.ExecuteReader();
            string strReturn = "[";
            bool flag= false;
            while (sdr.Read())
            {
                if (flag)
                    strReturn += ',';
                else
                    flag = true;

                strReturn += "{\"phase\":\"" + sdr["Phase"].ToString() + "\"}";
            }
            strReturn += "]";

            con.Close();
            con.Dispose();
            return strReturn;
        }


        public string GetStockConfig(string winding, string phase)
        {
            string[] user = UserValidation.GetLoggedInUser();
            string _repId = user[0];
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
            con.Open();
            SqlCommand com = new SqlCommand("SELECT DISTINCT Stock.Configuration FROM Stock WHERE (Stock.Windings = '" + winding + "') AND (Stock.Phase = '" + phase + "') ", con);

            SqlDataReader sdr = com.ExecuteReader();
            string strReturn = "[";
            bool flag = false;
            while (sdr.Read())
            {
                if (flag)
                    strReturn += ',';
                else
                    flag = true;

                strReturn += "{\"configuration\":\"" + sdr["Configuration"].ToString() + "\"}";
            }
            strReturn += "]";
            con.Close();
            con.Dispose();
            return strReturn;
        }

        public string GetStockKVA(string winding, string phase, string config)
        {
            string[] user = UserValidation.GetLoggedInUser();
            string _repId = user[0];
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
            con.Open();
            SqlCommand com = new SqlCommand("SELECT DISTINCT Stock.KVA FROM Stock WHERE (Stock.Windings = '" + winding + "') AND (Stock.Phase = '" + phase + "') " +
                " AND (Stock.Configuration = '" + config + "') "
                , con);

            SqlDataReader sdr = com.ExecuteReader();
            string strReturn = "[";
            bool flag = false;
            while (sdr.Read())
            {
                if (flag)
                    strReturn += ',';
                else
                    flag = true;

                strReturn += "{\"kva\":\"" + sdr["KVA"].ToString() + "\"}";
            }
            strReturn += "]";
            con.Close();
            con.Dispose();
            return strReturn;
        }

        /* ajax call to get the catalog number */
        //http://localhost:44409/RepStockUnits.svc/json/GetCatalogNumber?winding=Copper&phase=Three&config=480V%20DELTA%20PRIMARY%20-%20240D%2F120CT&kva=112.5
        public string GetCatalogNumber(string winding, string phase, string config, string KVA)
        {
            string[] user = UserValidation.GetLoggedInUser();
            string _repId = user[0];
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
            con.Open();
            SqlCommand com = new SqlCommand("SELECT     " +
                        "Stock.StockID, Stock.CatalogNumber, StockRepXRef.Price, Kit.KitID, Kit.KitNumber, Kit.Price as KitPrice " +
                        "FROM         Rep INNER JOIN " +
                      "StockRepXRef ON Rep.RepID = StockRepXRef.RepID INNER JOIN " +
                      "Stock ON StockRepXRef.StockID = Stock.StockID LEFT OUTER JOIN " +
                      "Kit ON Kit.KitID = Stock.KitID " +
                      "WHERE (Rep.RepID = " + _repId + ") " +
                      "AND (Stock.Phase = '" + phase + "') " +
                    "AND (Stock.Windings = '" + winding + "') " +
                    "AND (Stock.Configuration = '" + config + "') " +
                    "AND (Stock.KVA = '" + KVA + "') ", con);

            SqlDataReader sdr = com.ExecuteReader();
            string strReturn = "";
            if (sdr.Read())
            {
                strReturn = "[{\"stockID\":" + sdr["StockID"].ToString() + ",\"catalogNumber\":\"" + sdr["CatalogNumber"].ToString() + "\",\"price\":\"" + sdr["Price"].ToString() + "\",\"kit\":\"" + sdr["KitNumber"].ToString() + "\", \"kitPrice\":\"" + sdr["KitPrice"].ToString() + "\", \"kitId\":\"" + sdr["KitID"].ToString() + "\"}]";
            }
            con.Close();
            con.Dispose();
            return strReturn;
        }

        ///http://localhost:44409/RepStockUnits.svc/json/AddStockUnitToCart?stockid=2&quantity=3&quoteid=1
        public int AddStockUnitToCart(int stockid, int quantity, int quoteid = 0, bool updateQuantity = false)
        {
            string[] user = UserValidation.GetLoggedInUser();
            string _repId = user[0];
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
            con.Open();
            SqlCommand com = new SqlCommand("SELECT     " +
                        "Stock.StockID, StockRepXRef.Price " +
                        "FROM         Rep INNER JOIN " +
                      "StockRepXRef ON Rep.RepID = StockRepXRef.RepID INNER JOIN " +
                      "Stock ON StockRepXRef.StockID = Stock.StockID " +
                      "WHERE (Rep.RepID = " + _repId + ") " +
                    "AND (Stock.StockID = " + stockid + ") ", con);

            SqlDataReader sdr = com.ExecuteReader();
            SqlDataReader sdrInsert;
            //quoteid
            if (sdr.Read())
            {
                string StockId = sdr["StockID"].ToString();
                string originalPrice = sdr["Price"].ToString();
                sdr.Close();
                //Insert into the quote table
                if (quoteid == 0)
                {
                    com = new SqlCommand("INSERT INTO Quote(RepID, Created_on) " +
                            "VALUES(" + _repId + ", getdate()); SELECT @@IDENTITY as 'quoteid'; ", con);
                    sdrInsert = com.ExecuteReader();
                    if (sdrInsert.Read())
                    {
                        quoteid = int.Parse(sdrInsert["quoteid"].ToString());
                        sdrInsert.Close();
                        //Insert into QuoteDetails
                        com = new SqlCommand("INSERT INTO QuoteDetails(QuoteID, StockUnitID, Price, Quantity, Created_on) " +
                            "VALUES(" + quoteid + ", " + StockId + ", " + originalPrice + ", " + quantity + ", getdate()) ", con);
                        com.ExecuteNonQuery();
                    }
                }
                else
                {
                    //Module to keep adding to the quote or update
                    int quoteDetailID = 0;
                    com = new SqlCommand("SELECT * FROM QuoteDetails WHERE QuoteId = " + quoteid + " and stockUnitID = " + StockId, con);
                    sdr = com.ExecuteReader();
                    if (sdr.Read())
                    {
                        int qty = 0;
                        if (!updateQuantity)
                            qty = int.Parse(sdr["Quantity"].ToString()) + quantity;
                        else
                            qty = quantity;
                        quoteDetailID = int.Parse(sdr["QuoteDetailsID"].ToString());
                        com = new SqlCommand("UPDATE QuoteDetails SET Quantity = " + qty + " Where QuoteDetailsID = " + quoteDetailID, con);
                    }
                    else
                    {
                        com = new SqlCommand("INSERT INTO QuoteDetails(QuoteID, StockUnitID, Price, Quantity, Created_on) " +
                                "VALUES(" + quoteid + ", " + StockId + ", " + originalPrice + ", " + quantity + ", getdate()) ", con);
                    }
                    sdr.Close();
                    com.ExecuteNonQuery();
                }
            }
            con.Close();
            con.Dispose();
            return quoteid;
        }

        public bool RemoveQuoteDetail(int quoteDetailId)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
            con.Open();
            SqlCommand com = new SqlCommand("DELETE From QuoteDetails where QuoteDetailsId = " + quoteDetailId, con);
            com.ExecuteNonQuery();
            return true;
        }
        
        #endregion

        public List<StockUnit> GetRepStockUnits(string phase, string winding, string config)
        {
            string[] user = UserValidation.GetLoggedInUser();
            string _repId = user[0];
            return GetStockUnit(_repId, phase, winding, config);
        }

        private List<StockUnit> GetStockUnit(string repid, string phase, string winding, string config)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString);
            con.Open();
            SqlCommand com = new SqlCommand("SELECT     "+
                    "StockRepXRef.Price, Stock.StockID, Stock.Windings, Stock.Phase, "+
                        "Stock.Configuration, Stock.KVA, Stock.CatalogNumber, Stock.AltNumber, Kit.KITNumber, Kit.Price as KitPrice, "+
                      "Stock.Height, Stock.Width, Stock.Depth, Stock.Weight, Stock.UnitCase, Stock.Created_on " +
                        "FROM         Rep INNER JOIN "+
                      "StockRepXRef ON Rep.RepID = StockRepXRef.RepID INNER JOIN "+
                      "Stock ON StockRepXRef.StockID = Stock.StockID " +
                      "INNER JOIN Kit on Kit.KitID = Stock.KitID "+
                      "WHERE (Rep.RepID = " + repid + ") " +
                      "AND (Stock.Phase = '" + phase + "') " +
                    "AND (Stock.Windings = '" + winding + "') " +
                    "AND (Stock.Configuration = '" + config + "') ", con);

            SqlDataAdapter sda = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            con.Close();
            con.Dispose();
            List<StockUnit> stockUnits = new List<StockUnit>();
            StockUnit stockUnit;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                stockUnit = new StockUnit();
                stockUnit.phase = dr["Phase"].ToString();
                stockUnit.configuration = dr["Configuration"].ToString();
                stockUnit.kva = dr["KVA"].ToString();
                stockUnit.catalog = dr["CatalogNumber"].ToString();
                stockUnit.alt = dr["AltNumber"].ToString();
                stockUnit.price = dr["Price"].ToString();
                stockUnit.kit = dr["KITNumber"].ToString();
                stockUnit.kitprice = dr["KitPrice"].ToString();
                stockUnit.height = dr["Height"].ToString();
                stockUnit.width = dr["Width"].ToString();
                stockUnit.depth = dr["Depth"].ToString();
                stockUnit.weight = dr["Weight"].ToString();
                stockUnit.unitCase = dr["UnitCase"].ToString();
                stockUnits.Add(stockUnit);
            }
            return stockUnits;
        }
        #endregion
    }
}
