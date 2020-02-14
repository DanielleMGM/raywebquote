using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MGM_Transformer
{
    public enum StockVoltagesType { StockVoltageDisplay = 0};

    public class StockVoltages
    {
        private static string _ssSQL;
        private static DataSet _sds;
        private string _sSQL;
        private DataSet _ds;
        private string _sStockVoltage;
        private string _sStockVoltageDisplay;
        private string _sVoltageCategory;
        private string _sWindings;


        public StockVoltages(string sDesc, StockVoltagesType svt)
        {
            if(svt == StockVoltagesType.StockVoltageDisplay)
            {
                _sSQL = "select * from StockVoltages where StockVoltageDisplay = '" + sDesc + "'";
            }
                     

            _ds = DataLink.Select(_sSQL,DataLinkCon.mgmuser);

            _sStockVoltage = _ds.Tables[0].AsEnumerable().Select(r => r["StockVoltage"].ToString()).First();
            _sStockVoltageDisplay = _ds.Tables[0].AsEnumerable().Select(r => r["StockVoltageDisplay"].ToString()).First();
            _sVoltageCategory = _ds.Tables[0].AsEnumerable().Select(r => r["VoltageCategory"].ToString()).First();
            _sWindings = _ds.Tables[0].AsEnumerable().Select(r => r["Windings"].ToString()).First();

        }

        public StockVoltages()
        {
            _sStockVoltageDisplay = "ALL";
            _sStockVoltage = "";
            _sVoltageCategory = "";
            _sWindings = "";
        }


        static public string GetCategory(string sStockVoltageDisplay, string sWindings)
        {
            if (sStockVoltageDisplay == "ALL")
                return "ALL";

            _sds = DataLink.Select("select VoltageCategory from StockVoltages where StockVoltageDisplay = '" + sStockVoltageDisplay + "' and Windings = '" + sWindings + "'",
                                    DataLinkCon.mgmuser);
            return _sds.Tables[0].AsEnumerable().Select(r => r["VoltageCategory"].ToString()).First();

        }



        static public List<StockVoltages> GetAllItems()
        {
            List<StockVoltages> retLst;

            _ssSQL = "select * from StockVoltages";
            _sds = DataLink.Select(_ssSQL,DataLinkCon.mgmuser);

            retLst = _sds.Tables[0].AsEnumerable().Select(r => new StockVoltages(r["StockVoltageDisplay"].ToString(),
                         StockVoltagesType.StockVoltageDisplay)).Cast<StockVoltages>().ToList();

            return retLst;

        }

        public string StockVoltage
        {
            get { return _sStockVoltage; }
        }


        public string StockVoltageDisplay
        {
            get { return _sStockVoltageDisplay; }
        }


        public string VoltageCategory
        {
            get { return _sVoltageCategory; }
        }


        public string Windings
        {
            get { return _sWindings; }
        }


    }
}