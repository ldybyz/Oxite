<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Admin.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelItem<IPlugin>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title"><%=Model.Item.Name %> Plugin</h2>
    <form action="" method="post" class="plugin">
        <div><%=Html.CheckBox("enabled", Model.Item.Enabled, new { id = "enabled" }) %> <label for="enabled" class="checkbox">Enabled?</label></div><%
    int settingCount = 0;
    foreach (string name in Model.Item.Settings.AllKeys)
    {
        string settingId = "s" + settingCount++; 
    %>
        <div><label for="<%=settingId %>"><%=name %></label> <%=Html.TextBox("settingValue", Model.Item.Settings[name], new { id = settingId, @class = "text" })%><%=Html.Hidden("settingName", name, new { id = name })%></div><%
    } %>
        <div class="buttons">
            <input type="submit" name="submit" value="Save" class="button submit" />
            <%=Html.Button(
                "cancel",
                Model.Localize("Cancel"),
                new { @class = "cancel", onclick = string.Format("if (window.confirm('{0}')){{window.document.location='{1}';}}return false;", Model.Localize("really?"), Url.Plugins()) }
                )%>
            <%=Html.Link(
                Model.Localize("Cancel"),
                Url.Plugins(),
                new { @class = "cancel" })%>
            <%=Html.Hidden("pluginID", Model.Item.ID.ToString("N")) %>
            <%=Html.OxiteAntiForgeryToken(m => m.AntiForgeryToken) %>
        </div>
    </form>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Localize("Admin"), Model.Localize("Plugins"), Model.Item.Name) %>
</asp:Content>
