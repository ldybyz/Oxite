<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Admin.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelItem<User>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title"><%=Model.Localize("ChangePassword", "Change Password") %></h2><%=
    Html.ValidationSummary() %>
    <form action="" method="post">
        <div><%=Html.Password("userPassword", null, Model.Localize("NewPassword", "New Password"), new { size = 42, @class = "text" })%></div>
        <div><%=Html.Password("userPasswordConfirm", null, Model.Localize("NewPasswordConfirm", "New Password (Confirm)"), new { size = 42, @class = "text" })%></div>
        <div><input type="submit" name="submit" class="submit button" value="<%=Model.Localize("ChangePassword", "Change Password") %>" /><%=
            Html.OxiteAntiForgeryToken(m => m.AntiForgeryToken) %></div>
    </form>
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("site.js"); %>
</asp:Content>