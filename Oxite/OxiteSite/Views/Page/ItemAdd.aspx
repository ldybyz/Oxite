<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Admin.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelItem<Oxite.Model.Page>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="post addPage" id="post">
    <% Html.RenderPartial("ItemEditForm"); %>
</div>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="HeadCssFiles"><%
    Html.RenderCssFile("jquery.css"); %>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptVariablesPre">
    <script type="text/javascript">
        window.cssPath = "<%=Url.CssPath(Model) %>";
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Scripts"><%
    Html.RenderScriptTag("site.js");
    Html.RenderScriptTag("admin.js");
    Html.RenderScriptTag("jquery-ui-20081126-1.5.2.js", "jquery-ui-20081126-1.5.2.min.js"); %>
</asp:Content>