<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<Post>>" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
            <div class="sections">
                <div class="primary">
                    <h2 class="title"><% Html.RenderPartial("ArchiveBreadcrumb"); %></h2>
                    <%=Html.PageState((IPageOfList<Post>)Model.List, (k, v) => Model.Localize(k, v)) %><% 
                    Html.RenderPartial("PostListMedium");
                    %><%=Html.PostArchiveListPager((IPageOfList<Post>)Model.List, (k, v) => Model.Localize(k, v)) %>
                </div>
                <div class="secondary"><% 
                    Html.RenderPartial("SideBar"); %>
                </div>
            </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server"><%
    ArchiveData archiveData = ((ArchiveContainer)Model.Container).ArchiveData; %>
    <%=Html.PageTitle(
        Model.Localize("ArchiveTitle","Archives"),
        archiveData.Year.ToString(),
        archiveData.Month > 0 ? new DateTime(archiveData.Year, archiveData.Month, 1).ToString("MMMM") : null,
        archiveData.Day > 0 ? archiveData.Day.ToString() : null
        ) %>
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("site.js"); %>
</asp:Content>
