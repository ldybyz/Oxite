<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Admin.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelItem<Area>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title"><%=Model.Item == null ? Model.Localize("Areas.Add", "Add Area") : Model.Localize("Areas.Edit", "Edit Area") %></h2><%
    if (Model.Item != null && !Model.Site.HasMultipleAreas)
    { %>
    <div class="message info"><%=string.Format(Model.Localize("Site.HasSingleArea", "This site only has one area.  The below settings can be changed on the <a href=\"{0}\">site settings page</a>"), Url.Site()) %></div><%
    } %>
    <%=Html.ValidationSummary() %>
    <form action="" method="post"><%
    if (Model.Item != null)
    { %>
        <div><input type="hidden" name="areaID" value="<%=Model.Item.ID.ToString() %>" /></div><%
    } %>
        <div><%=Html.TextBox("areaName", m => m.Item != null ? m.Item.Name : "", "Name", Model.Item == null || Model.Site.HasMultipleAreas, new { size = 20, @class = "text" })%></div>
        <div><%=Html.TextBox("areaDisplayName", m => m.Item != null ? m.Item.DisplayName : "", "Display Name", Model.Item == null || Model.Site.HasMultipleAreas, new { size = 40, @class = "text" })%></div>
        <div><%=Html.TextArea("areaDescription", m => m.Item != null ? m.Item.Description : "", 6, 80, "Description", Model.Item == null || Model.Site.HasMultipleAreas, new { @class = "text" })%></div>
        <div><%=Html.CheckBox("areaCommentingDisabled", m => m.Item != null ? m.Item.CommentingDisabled : false, "", Model.Item == null || Model.Site.HasMultipleAreas)%> <label for="areaCommentingDisabled" class="checkbox"><%=Model.Localize("CommentingDisabled", "Commenting Disabled") %></label></div><%
    if (Model.Item != null)
    { %>
        <div><%=Html.Link(Model.Localize("BlogML.LinkTitle", "BlogML Import/Export"), Url.BlogML(Model.Item)) %></div><%
    } %>
        <div class="buttons">
            <input type="submit" name="addArea" class="button submit" value="<%=Model.Item == null ? Model.Localize("Areas.Add", "Add Area") : Model.Localize("Areas.Edit", "Edit Area") %>" <%=(Model.Item != null && !Model.Site.HasMultipleAreas ? "disabled=\"disabled\" " : "") %>/>
            <%=Html.Button(
                "cancel",
                Model.Localize("Cancel"),
                new { @class = "cancel", onclick = string.Format("if (window.confirm('{0}')){{window.document.location='{1}';}}return false;", Model.Localize("really?"), Url.ManageAreas()) }
                )%>
            <%=Html.Link(
                Model.Localize("Cancel"),
                Url.ManageAreas(),
                new { @class = "cancel" })%>
            <%=Html.OxiteAntiForgeryToken(m => m.AntiForgeryToken) %>
        </div>
    </form>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Localize("Admin"), Model.Localize("Areas"), Model.Item == null ? Model.Localize("Add") : Model.Localize("Edit")) %>
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("site.js"); %>
</asp:Content>