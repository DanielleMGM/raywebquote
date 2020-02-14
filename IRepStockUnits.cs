using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MGM_Transformer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRepStockUnits" in both code and config file together.
    [ServiceContract]
    public interface IRepStockUnits
    {
        [OperationContract]
        [WebGet]
        string GetStockPhase(string winding);

        [OperationContract]
        [WebGet]
        string GetStockConfig(string winding, string phase);

        [OperationContract]
        [WebGet]
        string GetStockKVA(string winding, string phase, string config);

        [OperationContract]
        [WebGet]
        List<StockUnit> GetRepStockUnits(string phase, string winding, string config);

        [OperationContract]
        [WebGet]
        string GetCatalogNumber(string winding, string phase, string config, string KVA);

        [OperationContract]
        [WebGet]
        int AddStockUnitToCart(int stockid, int quantity, int quoteid, bool updateQuantity);

        [OperationContract]
        [WebGet]
        bool RemoveQuoteDetail(int quoteDetailId);

        [OperationContract]
        [WebGet]
        string GetQuoteHistory();

        [OperationContract]
        [WebGet]
        string GetQuoteDetails(int quoteid);

        /*[OperationContract]
        [WebGet]
        int AddCustomStockToCart(string winding, string phase, string kva, string temperature, string kfactor,
            string primaryVoltage, string primaryVoltageDW, string secondaryVoltage, string secondaryVoltageDW,
            string taps, string soundlevel, int quantity, int quoteid = 0, bool updateQuantity = false);*/

        [OperationContract]
        [WebGet]
        int AddQuoteDetailToCart(int quoteid, int customid, int stockid, int kitid, string primaryVoltage, string primaryVoltageDW, string secondaryVoltage, string secondaryVoltageDW,
            string taps, string soundlevel, int quantity, bool updateQuantity);
    }
}
