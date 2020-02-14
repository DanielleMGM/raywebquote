<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MGM.Master" AutoEventWireup="true" CodeBehind="Quote.aspx.cs" Inherits="MGM_Transformer.Quote" EnableEventValidation="false" %>
<%@ Register TagPrefix="uc" TagName="Navigation" Src="~/Navigation.ascx" %>
<%@ Register TagPrefix="uc" TagName="Quote" Src="~/QuoteBody.ascx" %>
<%@ Register TagPrefix="uc" TagName="Footer" Src="~/Footer.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNavigation" runat="server">
   <uc:Navigation id="ucNavigation" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <uc:Quote id="ucQuote" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphFooter" runat="server">
    <uc:Footer id="ucFooter" runat="server" />
</asp:Content>
