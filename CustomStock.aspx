<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcelImport.aspx.cs" Inherits="MGM_Transformer.ExcelImport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Import Prices from Excel</title>
    <meta http-equiv="X-UA-Compatible"  content="IE-Edge"/>
    <!-- <link rel="stylesheet" type="text/css" href="./Scripts/mgmtransformer/production.css" /> -->
</head>
<body>
    <table align="center">
        <tr align="center">
            <td><h1>Import Prices from Excel</h1>
            </td>
        </tr>
        <tr>
            <td>You should click the &quot;Export&quot; button on whichever price spreadsheets you want to upload before clicking 
            &quot;Import Prices&quot; below.
            </td>
        </tr>
        <tr>
            <td>You don't have to do all prices at once.  It will upload whatever prices are available.
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td>Note:&nbsp;Clicking the &quot;Export&quot; button in the Excel spreadsheets places files on L:\data\WebQuote\  (macola server):
            </td>
        </tr>
        <tr>
            <td>
                <ul>
                    <li>CustomStockAluminum.xls</li>
                    <li>CustomStockCopper.xls</li>
                    <li>StockRepPrices.xls</li>
                </ul>
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:Button ID="btnImport" runat="server" Text="Import Prices " 
                        onclick="btnImport_Click" />
            </td>
        </tr>
        <tr align="center">
            <td><a href="home.aspx">Home</a>
            </td>
        </tr>
     </table>
</body>
</html>
