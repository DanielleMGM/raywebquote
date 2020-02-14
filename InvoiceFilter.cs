using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGM_Transformer
{
    public class InvoiceFilter
    {
        private int _iInvoiceNo;
        private decimal _dSalesAmount;

        public InvoiceFilter(int iInvoiceNo, decimal dSalesAmount)
        {
            _iInvoiceNo = iInvoiceNo;
            _dSalesAmount = dSalesAmount;

        }
            
        public int InvoiceNo
        {
            get { return _iInvoiceNo; }
        }

        public decimal SalesAmount
        {
            get { return _dSalesAmount; }
            set { _dSalesAmount = value; }
        }
            

        public static decimal SalesAmountTotal(List<InvoiceFilter> lstInvoices)
        {
            decimal dSales = 0;
            List<int> lstDupInvoice = new List<int>();

            for (int i = 0; i < lstInvoices.Count; i++)
            {
                if (!lstDupInvoice.Contains(lstInvoices[i].InvoiceNo))
                {
                    lstDupInvoice.Add(lstInvoices[i].InvoiceNo);
                }
                else
                {
                    lstInvoices[i].SalesAmount = 0;
                }
            }

            dSales = lstInvoices.Select(v => v.SalesAmount).Sum();

            return dSales;


        }


    }
}