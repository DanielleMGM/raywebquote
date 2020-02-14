<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MGM.Master" AutoEventWireup="true" CodeBehind="Submittal.aspx.cs" Inherits="MGM_Transformer.Submittal" %>
<%@ Register TagPrefix="uc" TagName="Navigation" Src="~/Navigation.ascx" %>
<%@ Register TagPrefix="uc" TagName="Submittal" Src="~/SubmittalBody.ascx" %>
<%@ Register TagPrefix="uc" TagName="Footer" Src="~/Footer.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNavigation" runat="server">
   <uc:Navigation id="ucNavigation" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <uc:Submittal id="ucSubmittal" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphFooter" runat="server">
    <uc:Footer id="ucFooter" runat="server" />
</asp:Content>
