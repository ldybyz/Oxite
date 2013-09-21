<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<Post>>" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
            <div class="sections">
                <div class="primary">
                    <h2 class="title"><%=Model.Container.Name %> Posts</h2>
                    <%=Html.PageState((IPageOfList<Post>)Model.List, (k, v) => Model.Localize(k, v)) %><%
                    Html.RenderPartial("PostListMedium");
                    %><%=Html.PostListByTagPager((IPageOfList<Post>)Model.List,(k,v) => Model.Localize(k,v), Model.Container.Name) %>
                </div>
                <div class="secondary"><% 
                    Html.RenderPartial("SideBar"); %>
                </div>
            </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Localize("TagsTitle","Tags"), Model.Container.GetDisplayName()) %>
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("site.js"); %>
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadCustom" runat="server"><%
    Html.RenderFeedDiscoveryRss(string.Format("{0} Posts (RSS)", Model.Container.Name), Url.Container(Model.Container, "RSS"));
    Html.RenderFeedDiscoveryAtom(string.Format("{0} Posts (ATOM)", Model.Container.Name), Url.Container(Model.Container, "ATOM"));
    Html.RenderFeedDiscoveryRss(string.Format("All {0} Comments (RSS)", Model.Container.Name), Url.ContainerComments(Model.Container, "RSS"));
    Html.RenderFeedDiscoveryAtom(string.Format("All {0} Comments (ATOM)", Model.Container.Name), Url.ContainerComments(Model.Container, "ATOM"));
    Html.RenderFeedDiscoveryRss(string.Format("{0} (RSS)", Model.Site.DisplayName), Url.Posts("RSS"));
    Html.RenderFeedDiscoveryAtom(string.Format("{0} (ATOM)", Model.Site.DisplayName), Url.Posts("ATOM")); %>
</asp:Content>
