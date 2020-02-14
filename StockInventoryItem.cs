using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MGM_Transformer
{

    public enum InventoryDescType { CatalogNumber = 0, AltNumber = 1 };

    public class StockInventoryItem
    {
        private string _sSQL;
        private DataSet _ds;
        static private string _ssSQL;
        static private DataSet _sds;
        private string _sCatalogNumber;
        private string _sAltNumber;
        private decimal _dHeight;
        private decimal _dWidth;
        private decimal _dDepth;
        private string _sPrimaryVoltageDW;
        private string _sSecondaryVoltageDW;
        private int _iHertz;
        private int _iTemperature;
        private string _sEnclosure;
        private string _sPrimaryVoltage;
        private string _sSecondaryVoltage;
        private string _sTP1;
        private string _sSoundLevel;
        private double _dKVA;
        private int _iWeight;
        private string _sWindings;
        private string _sConfiguration;
        private string _sStockVoltageDisplay;
        private bool _bUseSelectString = false;
        private string _sVoltageCat;

        public StockInventoryItem(string sDesc, InventoryDescType descType)
        {

            if (descType == InventoryDescType.AltNumber)
                _sSQL = "select s.CatalogNumber,s.AltNumber,s.Height,s.Width,s.Depth,s.PrimaryVoltageDW,s.SecondaryVoltageDW,s.Hertz,s.Temperature,s.Enclosure," + 
                    "s.PrimaryVoltage,s.SecondaryVoltage,s.TP1,s.SoundLevel,s.KVA,s.Weight,s.Windings,s.Configuration,sv.StockVoltageDisplay,sv.VoltageCategory " + 
                    "from Stock s join StockVoltages sv on " + 
                    "s.Configuration = sv.StockVoltage and s.Windings = sv.Windings where AltNumber = '" + sDesc + "'";
            if (descType == InventoryDescType.CatalogNumber)
                _sSQL = "select s.CatalogNumber,s.AltNumber,s.Height,s.Width,s.Depth,s.PrimaryVoltageDW,s.SecondaryVoltageDW,s.Hertz,s.Temperature,s.Enclosure," +
                   "s.PrimaryVoltage,s.SecondaryVoltage,s.TP1,s.SoundLevel,s.KVA,s.Weight,s.Windings,s.Configuration,sv.StockVoltageDisplay,sv.VoltageCategory " + 
                   "from Stock s join StockVoltages sv on " + 
                   "s.Configuration = sv.StockVoltage and s.Windings = sv.Windings where CatalogNumber = '" + sDesc + "'";

            _ds = DataLink.Select(_sSQL,DataLinkCon.mgmuser);
            _sCatalogNumber = _ds.Tables[0].AsEnumerable().Select(r => r["CatalogNumber"].ToString()).First();
            _sAltNumber = _ds.Tables[0].AsEnumerable().Select(r => r["AltNumber"].ToString()).First();
            _dHeight = _ds.Tables[0].AsEnumerable().Select(r => Convert.ToDecimal(r["Height"])).First();
            _dWidth = _ds.Tables[0].AsEnumerable().Select(r => Convert.ToDecimal(r["Width"])).First();
            _dDepth = _ds.Tables[0].AsEnumerable().Select(r => Convert.ToDecimal(r["Depth"])).First();
            _sPrimaryVoltageDW = _ds.Tables[0].AsEnumerable().Select(r => r["PrimaryVoltageDW"].ToString()).First();
            _sSecondaryVoltageDW = _ds.Tables[0].AsEnumerable().Select(r => r["SecondaryVoltageDW"].ToString()).First();
            _iHertz = _ds.Tables[0].AsEnumerable().Select(r => Convert.ToInt32(r["Hertz"])).First();
            _iTemperature = _ds.Tables[0].AsEnumerable().Select(r => Convert.ToInt32(r["Temperature"])).First();
            _sEnclosure = _ds.Tables[0].AsEnumerable().Select(r => r["Enclosure"].ToString()).First();
            _sPrimaryVoltage = _ds.Tables[0].AsEnumerable().Select(r => r["PrimaryVoltage"].ToString()).First();
            _sSecondaryVoltage = _ds.Tables[0].AsEnumerable().Select(r => r["SecondaryVoltage"].ToString()).First();
            _sTP1 = _ds.Tables[0].AsEnumerable().Select(r => r["TP1"].ToString()).First();
            _sSoundLevel = _ds.Tables[0].AsEnumerable().Select(r => r["SoundLevel"].ToString()).First();
            _dKVA = _ds.Tables[0].AsEnumerable().Select(r => Convert.ToDouble(r["KVA"])).First();
            _iWeight = _ds.Tables[0].AsEnumerable().Select(r => Convert.ToInt32(r["Weight"])).First();
            _sWindings = _ds.Tables[0].AsEnumerable().Select(r => r["Windings"].ToString()).First();
            _sConfiguration = _ds.Tables[0].AsEnumerable().Select(r => r["Configuration"].ToString()).First();
            _sStockVoltageDisplay = _ds.Tables[0].AsEnumerable().Select(r => r["StockVoltageDisplay"].ToString()).First();
            _sVoltageCat = _ds.Tables[0].AsEnumerable().Select(r => r["VoltageCategory"].ToString()).First();
            _bUseSelectString = false;
        }


        public StockInventoryItem()
        {
            _bUseSelectString = true;
            _dKVA = 0;

            _sCatalogNumber = "";
            _sAltNumber = "";
            _dHeight = 0;
            _dWidth = 0;
            _dDepth = 0;
            _sPrimaryVoltageDW = "";
            _sSecondaryVoltageDW = "";
            _iHertz = 0;
            _iTemperature = 0;
            _sEnclosure = "";
            _sPrimaryVoltage = "";
            _sSecondaryVoltage = "";
            _sTP1 = "";
            _sSoundLevel = "";
            _iWeight = 0;
            _sWindings = "";
            _sConfiguration = "";
            _sStockVoltageDisplay = "";

        }
              


        static public List<StockInventoryItem> GetAllItems()
        {
            _ssSQL = "select CatalogNumber from Stock";
            _sds = DataLink.Select(_ssSQL,DataLinkCon.mgmuser);

            return _sds.Tables[0].AsEnumerable().Select(r => new StockInventoryItem(r["CatalogNumber"].ToString(),
                         InventoryDescType.CatalogNumber)).Cast<StockInventoryItem>().ToList();

        }

        
       static public List<string> GetAltNumber(string sWindings,string sStockVoltageDisplay,string sKVA)
        {

            _ssSQL = "select s.AltNumber from Stock s join StockVoltages sv on s.Configuration = sv.StockVoltage where s.Windings = '" + 
                sWindings + "' and s.KVA = " + sKVA + " and sv.StockVoltageDisplay = '" + sStockVoltageDisplay + "'";
                     
            _sds = DataLink.Select(_ssSQL,DataLinkCon.mgmuser);

            return _sds.Tables[0].AsEnumerable().Select(r => r["AltNumber"].ToString()).ToList();
        }


        public string VoltageCategory
        {
            get { return _sVoltageCat; }
        }



        public string StockVoltageDisplay
        {
            get { return _sStockVoltageDisplay; }
        }
        
        public string CatalogNumber
        {
            get { return _sCatalogNumber; }
        }

        public string AltNumber
        {
            get { return _sAltNumber; }
        }

        public decimal Height
        {
            get { return _dHeight; }

        }

        public decimal Width
        {
            get { return _dWidth; }
        }

        public decimal Depth
        {
            get { return _dDepth; }
        }

        public string PrimaryVoltageDW
        {
            get { return _sPrimaryVoltageDW; }
        }

        public string SecondaryVoltageDW
        {
            get { return _sSecondaryVoltageDW; }
        }

        public int Hertz
        {
            get { return _iHertz; }
        }

        public int Temperature
        {
            get { return _iTemperature; }
        }

        public string Enclosure
        {
            get { return _sEnclosure; }
        }

        public string PrimaryVoltage
        {
            get { return _sPrimaryVoltage; }
        }

        public string SecondaryVoltage
        {
            get { return _sSecondaryVoltage; }
        }

        public string TP1
        {
            get { return _sTP1; }
        }

        public string SoundLevel
        {
            get { return _sSoundLevel; }
        }

        public double KVA
        {
            get { return _dKVA; }
        }


        public string sKVA
        {
            get {
                    string sRet = ""; ;
                    if (_bUseSelectString)
                        sRet = "ALL";
                    else
                        sRet = _dKVA.ToString();

                    return sRet;
                }
        }
        public int Weight
        {
            get { return _iWeight; }
        }

        public string Windings
        {
            get { return _sWindings; }
        }


        public string Configuration
        {
            get { return _sConfiguration; }
        }
    }
}