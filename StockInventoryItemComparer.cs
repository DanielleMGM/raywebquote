using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGM_Transformer
{
    public class StockInventoryItemComparer : IEqualityComparer<StockInventoryItem>
    {
        public bool Equals(StockInventoryItem x, StockInventoryItem y)
        {
            return x.KVA == y.KVA;
        }

        public int GetHashCode(StockInventoryItem obj)
        {
            return obj.KVA.GetHashCode();
        }
    }
}