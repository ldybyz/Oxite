<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Admin.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelItem<Site>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title"><%=(Model.Item.ID == Guid.Empty ? Model.Localize("Site.Add", "Initial Site Setup") : Model.Localize("Site.Edit", "Edit Site")) %></h2>
    <%=Html.ValidationSummary() %>
    <form action="" method="post" id="siteSettings"><%
    if (Model.Item.ID != Guid.Empty)
    { %>
        <div><%=Html.Hidden("siteID", Model.Item.ID) %></div><%
    } %>
        <div><%=Html.Hidden("hasMultipleAreas", Model.Item.ID == Guid.Empty ? false : Model.Item.HasMultipleAreas) %></div>
        <h3><%=Model.Localize("Site.Section.About", "What the site is about") %></h3>
        <div><%=Html.TextBox("siteDisplayName", m => m.Item.DisplayName, "Display Name", new { size = 60, @class = "text" })%></div>
        <div><%=Html.TextArea("siteDescription", m => m.Item.Description, 4, 80, "Description")%></div>
        <h3><%=Model.Localize("Site.Section.Appearance", "How it should look") %></h3>
        <div><%=Html.TextBox("skinDefault", m => m.Item.SkinDefault, "Skin Name", new { size = 20, @class = "text" })%></div>
        <div><%=Html.TextBox("scriptsPath", m => m.Item.ScriptsPath, "Scripts Path Format", new { size = 60, @class = "text" })%> <span class="hint">{0} will be replaced by Skin Name.</span></div>
        <div><%=Html.TextBox("cssPath", m => m.Item.CssPath, "CSS Path Format", new { size = 60, @class = "text" })%> <span class="hint">{0} will be replaced by Skin Name.</span></div>
        <div><%=Html.TextBox("favIconUrl", m => m.Item.FavIconUrl, "Fav Icon Path", new { size = 60, @class = "text" })%> <span class="hint icons"><span>Included icons:</span> <img src="<%=Url.AppPath("~/Content/images/flame.png") %>" title="~/Content/icons/flame.ico"<%=(Request.Form["favIconUrl"] ?? Model.Item.FavIconUrl) == "~/Content/icons/flame.ico" ? " class=\"selected\"" : "" %> alt="Flame" /><span> flame.ico,</span> <img src="<%=Url.AppPath("~/Content/images/mushroom.png") %>" title="~/Content/icons/mushroom.ico"<%=(Request.Form["favIconUrl"] ?? Model.Item.FavIconUrl) == "~/Content/icons/mushroom.ico" ? " class=\"selected\"" : "" %> alt="Mushroom" /><span> mushroom.ico,</span> <img src="<%=Url.AppPath("~/Content/images/frog.png") %>" title="~/Content/icons/frog.ico"<%=(Request.Form["favIconUrl"] ?? Model.Item.FavIconUrl) == "~/Content/icons/frog.ico" ? " class=\"selected\"" : "" %> alt="Frog" /><span> frog.ico</span></span></div>
        <div><%=Html.TextBox("gravatarDefault", m => m.Item.GravatarDefault, "Gravatar Image URL", new { size = 60, @class = "text" })%></div>
        <div><%=Html.TextBox("pageTitleSeparator", m => m.Item.PageTitleSeparator, "Page Title Separator", new { size = 4, @class = "text" })%></div>
        <h3><%=Model.Localize("Site.Section.Where", "Where it should appear to be") %></h3>
        <div><%=Html.TextBox("timezoneOffset", m => m.Item.TimeZoneOffset, "Timezone Offset", new { size = 4, @class = "text" })%></div>
        <div><%=Html.TextBox("languageDefault", m => m.Item.LanguageDefault, "Language", new { size = 6, @class = "text" })%></div>
        <h3><%=Model.Localize("Site.Section.Behavior", "How should it behave") %></h3>
        <div><%=Html.CheckBox("includeOpenSearch", m => m.Item.IncludeOpenSearch, "")%> <label for="includeOpenSearch" class="checkbox">Include OpenSearch</label></div>
        <div><%=Html.TextBox("postEditTimeout", m => m.Item.PostEditTimeout, "Post Editable Timeperiod (in hours)", new { size = 6, @class = "text" })%> <span class="hint">Leave blank to always allow editing of post urls.</span></div>
        <div><%=Html.CheckBox("commentingDisabled", m => m.Item.CommentingDisabled, "")%> <label for="commentingDisabled" class="checkbox">Commenting Disabled</label></div>
        <div><%=Html.CheckBox("commentStatePendingByDefault", m => !string.IsNullOrEmpty(m.Item.CommentStateDefault) ? m.Item.CommentStateDefault == EntityState.PendingApproval.ToString() : true, "")%> <label for="commentStatePendingByDefault" class="checkbox">Post Comments Pending By Default</label></div>
        <div><%=Html.CheckBox("authorAutoSubscribe", m => m.Item.AuthorAutoSubscribe, "")%> <label for="authorAutoSubscribe" class="checkbox">Post Author Is Auto Subscribed</label></div>
        <div><%=Html.TextBox("routeUrlPrefix", m => m.Item.RouteUrlPrefix, "Route Url Prefix", new { size = 10, @class = "text" })%> <span class="hint">If you are running IIS 6 and can not setup a wildcard, try setting this field to "oxite.aspx"</span></div><%
    if (Model.Item.ID == Guid.Empty)
    { %>
        <h3><%=Model.Localize("Site.Section.AdminUser", "Who runs this site") %></h3>
        <div><%=Html.TextBox("userName", m => "Admin", "Admin Username", new { size = 40, @class = "text" })%></div>
        <div><%=Html.TextBox("userDisplayName", m => "Oxite Administrator", "Admin Display Name", new { size = 40, @class = "text" })%></div>
        <div><%=Html.TextBox("userEmail", m => "", "Admin E-mail", new { size = 40, @class = "text" })%></div>
        <div><%=Html.Password("userPassword", null, "Admin Password", new { size = 40, @class = "text" })%></div>
        <div><%=Html.Password("userPasswordConfirm", null, "Admin Password (Confirm)", new { size = 40, @class = "text" })%></div><%
    } %>
        <h3><%=Model.Localize("Site.Section.Plumbing", "Mess with the plumbing <span class=\"hint\">at your own risk</span>") %></h3>
        <div><%=Html.TextBox("siteNameDisplay", m => m.Item.Name, "Instance Name", new { size = 60, @class = "text", disabled = "disabled" })%><%=Html.Hidden("siteName", Model.Item.Name) %> <span class="hint">This doesn't need to be changed unless hosting multiple sites in the same database. If you are running multiple sites in the same database this value must be unique. The value shown is retrieved from the web.config and must be changed there if needed.</span></div>
        <div><%=Html.TextBox("siteHostDisplay", m => m.Item.Host != null ? m.Item.Host.ToString() : Request.GenerateSiteHost(), "Host", new { size = 60, @class = "text" })%><%=Html.Hidden("siteHost", Request.GenerateSiteHost()) %></div>
        <div><span class="hint">Put each redirect on a new line</span> <%=Html.TextArea("siteHostRedirects", m => m.Item.HostRedirects != null ? string.Join("\n", m.Item.HostRedirects.Select(u => u.ToString()).ToArray()) : "", 4, 50, "Host Redirects")%></div>
        <div class="buttons">
            <input type="submit" name="submit" class="button submit" value="<%=Model.Item.ID == Guid.Empty ? Model.Localize("Site.Create", "Create Site") : Model.Localize("Site.Edit", "Edit Site") %>" /><%
    if (Model.Item.ID != Guid.Empty)
    { %>
            <%=Html.Button(
                "cancel",
                Model.Localize("Cancel"),
                new { @class = "cancel", onclick = string.Format("if (window.confirm('{0}')){{window.document.location='{1}';}}return false;", Model.Localize("really?"), Url.ManageSite()) }
                )%>
            <%=Html.Link(
                Model.Localize("Cancel"),
                Url.ManageSite(),
                new { @class = "cancel" })%><%
    } %><%=
            Html.OxiteAntiForgeryToken(m => m.AntiForgeryToken) %>
        </div>
    </form>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Localize("Admin"), Model.Localize("Site"), Model.Item.ID == Guid.Empty ? Model.Localize("Setup") : Model.Localize("Edit")) %>
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("site.js");
    Html.RenderScriptTag("admin.js"); %>
</asp:Content>
