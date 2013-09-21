<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelItem<Post>>" %>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="post">
    <% Html.RenderPartial("ManagePost"); %>
    <div class="avatar"><%=Html.Gravatar(Model.Item.Creator, "48") %></div>
    <h2 class="title"><%=Model.Item.Title %></h2>
    <div class="metadata">
        <div class="posted"><%=Html.Published() %></div><%
        if (Model.Item.Tags.Count > 0)
        {
            %> <%=Model.Localize("TagsLabel", "Filed under") 
            %> <%=Html.UnorderedList(Model.Item.Tags, (t, i) => Html.Link(t.Name.CleanText(), Url.Posts(t), new { rel = "tag" }), "tags") %><%
        } %>
    </div>
    <div class="content"><%=Model.Item.Body %></div>
    <%
        if(!(Model.CommentingDisabled && Model.Item.Comments.Count < 1))
        {
            Html.RenderPartial("Comments");
        }

        if (Model.CommentingDisabled)
        {
            %><div class="message"><%=Model.Localize("CommentingDisabled", "Commenting is disabled for this post.")%></div><%
        } %>
</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Site.HasMultipleAreas ? Model.Container.GetDisplayName() : null, Model.Item.GetDisplayName()) %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MetaDescription" runat="server">
    <%=Html.PageDescription(Model.Item.GetBodyShort()) %>
</asp:Content>
<asp:Content ContentPlaceHolderID="ScriptVariablesPre" runat="server">
    <script type="text/javascript">
        <% Html.RenderScriptVariable("computeHashPath", Url.ComputeHash()); %>
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("site.js"); %>
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadCustom" runat="server"><%
    Html.RenderFeedDiscoveryRss("Post Comments (RSS)", Url.Comments(Model.Item, "RSS"));
    Html.RenderFeedDiscoveryAtom("Post Comments (ATOM)", Url.Comments(Model.Item, "ATOM"));
    if (Model.Site.HasMultipleAreas)
    {
        Html.RenderFeedDiscoveryRss(string.Format("{0} Posts (RSS)", Model.Container.GetDisplayName()), Url.Container(Model.Container, "RSS"));
        Html.RenderFeedDiscoveryAtom(string.Format("{0} Posts (ATOM)", Model.Container.GetDisplayName()), Url.Container(Model.Container, "ATOM"));
        Html.RenderFeedDiscoveryRss(string.Format("All {0} Comments (RSS)", Model.Container.GetDisplayName()), Url.ContainerComments(Model.Container, "RSS"));
        Html.RenderFeedDiscoveryAtom(string.Format("All {0} Comments (ATOM)", Model.Container.GetDisplayName()), Url.ContainerComments(Model.Container, "ATOM"));
    }
    Html.RenderFeedDiscoveryRss(string.Format("{0} (RSS)", Model.Site.DisplayName), Url.Posts("RSS"));
    Html.RenderFeedDiscoveryAtom(string.Format("{0} (ATOM)", Model.Site.DisplayName), Url.Posts("ATOM"));
    Response.Write(Html.PingbackDiscovery(Model.Item)); %>
</asp:Content>
