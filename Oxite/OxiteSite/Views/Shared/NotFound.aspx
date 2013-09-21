<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<OxiteModel>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <p><%=Model.Localize("NotFoundTitle", "The url you requested could not be found.")%></p><%
    if (!string.IsNullOrEmpty((string)ViewData["Description"]))
    { %>
    <p><%=ViewData["Description"] %></p><%
    } %>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Localize("NotFoundMessage", "Not Found")) %>
</asp:Content>
