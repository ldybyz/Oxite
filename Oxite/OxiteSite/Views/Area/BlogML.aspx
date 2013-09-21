<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Admin.master" Inherits="System.Web.Mvc.ViewPage<OxiteModel>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
                    <h2>BlogML</h2>
                    <h3><%=Model.Localize("Import") %></h3>
                    <%=Html.ValidationSummary() %><%
    if (ViewData.ModelState.IsValid)
    {
        string message = "";
        
        if (Request.HttpMethod == "GET")
        {
            message = string.Format(Model.Localize("BlogML.ImportWarning", "Warning: Importing BlogML will wipe out all data for '{0}'!"), Model.Container.DisplayName);
        }
        else if (Request.HttpMethod == "POST")
        {
            message = Model.Localize("BlogML.ImportSuccess", "BlogML successfully imported!");
        } %>
                    <div class="message info"><%=message %></div><%
    } %>
                    <form action="" method="post" enctype="multipart/form-data">
                        <div><label for="blogMLFile">BlogML File</label><input type="file" id="blogMLFile" name="blogMLFile" size="60" class="text" /></div>
                        <div><%=Html.TextBox("slugPattern", m => "/([^/]+)\\.aspx", "Slug Pattern", new { size = 40, @class = "text" }) %> <span class="hint"><%=Model.Localize("BlogML.ImportSlugPatternDescription", "Leave blank if you want post-url attribute to be used as is") %></span></div>
                        <div><input type="submit" class="button submit" value="<%=Model.Localize("Upload") %>" /><%=
                            Html.OxiteAntiForgeryToken(m => m.AntiForgeryToken) %></div>
                    </form>
                    <h3><%=Model.Localize("Export") %></h3>
                    <p><span class="hint"><%=Model.Localize("Coming soon") %></span></p>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Localize("Admin"), "BlogML") %>
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("site.js"); %>
</asp:Content>