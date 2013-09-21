<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Admin.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<ParentAndChild<PostBase, Comment>>>" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="sections">
        <div class="primary"> 
            <h2 class="title">Recent Comments</h2>
            <%=Html.PageState((IPageOfList<ParentAndChild<PostBase, Comment>>)Model.List, (k, v) => Model.Localize(k, v)) %><%
            Html.RenderPartial("CommentListMedium");
            %><%=Html.CommentListPager((IPageOfList<ParentAndChild<PostBase, Comment>>)Model.List, (k, v) => Model.Localize(k, v)) %>
        </div>
        <div class="secondary"><% 
            Html.RenderPartial("SideBar"); %>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("site.js"); %>
</asp:Content>