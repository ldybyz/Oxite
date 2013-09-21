<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Admin.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<Post>>" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
            <div class="sections">
                <div class="primary">
                    <%=Html.PageState((IPageOfList<Post>)Model.List, (k, v) => Model.Localize(k, v)) %><% 
                    Html.RenderPartial("PostListMedium");
                    %><%=Html.PostListPager((IPageOfList<Post>)Model.List, (k,v) => Model.Localize(k,v)) %>
                </div>
                <div class="secondary"><% 
                    Html.RenderPartial("SideBar"); %>
                </div>
            </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("site.js");
    Html.RenderScriptTag("admin.js"); %>
</asp:Content>