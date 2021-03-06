﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockListPrices.aspx.cs" Inherits="MGM_Transformer.StockListPrices" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Confidential: MGM Transformers Stock List Prices</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
<CR:CrystalReportSource ID="cryRS" runat="server">
    <Report FileName="StockPriceList.rpt">
    </Report>
</CR:CrystalReportSource>
<CR:CrystalReportViewer ID="cryRV" runat="server" 
    AutoDataBind="true" ReportSourceID="cryRS" />
<asp:SqlDataSource ID="dsStockListPrices" runat="server" 
        ConnectionString="<%$ ConnectionStrings:mgmdb %>" 
        SelectCommand="usp_Rpt_StockPrices" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
    </form>
</body>
</html>
