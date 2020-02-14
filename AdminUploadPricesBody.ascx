<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminUploadPricesBody.ascx.cs" Inherits="MGM_Transformer.AdminUploadPricesBody" %>
<table width="100%" border="0" cellpadding="0" cellspacing="5">
    <tr align="center">
        <td>
            <h1>Upload Prices</h1>
        </td>
    </tr>
    <tr>
        <td>
            <hr />
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
                <li>StockRepDistributors.xls</li>
            </ul>
        </td>
    </tr>
    <tr align="center">
        <td>
            <asp:Button ID="btnImport" Text="Upload Prices" runat="server" Width="120" Height="60" onclick="btnImport_Click" />
        </td>
    </tr>
</table>

