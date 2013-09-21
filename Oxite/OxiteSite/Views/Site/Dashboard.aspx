<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<OxiteModel>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server"><%
    AdminDataViewModel adminData = Model.GetModelItem<AdminDataViewModel>(); %>
    <h2 class="title"><%=Model.Localize("AdminDashboardTitle", "Admin Dashboard")%></h2>
    <div id="dashboard">
        <ul id="data">
            <li id="recentPosts">
                <h3><%=Model.Localize("RecentPosts", "Recent Posts") %></h3>
                <% Html.RenderPartial("PostListSmall", new OxiteModelList<Post> { List = adminData.Posts, Site = Model.Site }); %>
                <% if (adminData.Posts.Count > 0) { %><div class="more"><%=Html.Link(Model.Localize("AllPosts", "More &raquo;"), Url.PostsWithDrafts()) %></div><% } %>
            </li>
            <li id="recentComments">
                <h3><%=Model.Localize("RecentComments", "Recent Comments") %></h3>
                <% Html.RenderPartial("CommentListSmall", new OxiteModelList<ParentAndChild<PostBase, Comment>> { List = adminData.Comments, Site = Model.Site, User = Model.User }); %>
                <% if (adminData.Comments.Count > 0) { %><div class="more"><%=Html.Link(Model.Localize("AllComments", "More/Manage &raquo;"), Url.ManageComments())%></div><% } %>
            </li>
        </ul>
        <ul id="settings">
            <li id="manageSite">
                <h3><%=Model.Localize("ManageSite", "Manage Site") %></h3>
                <ul>
                    <li><%=Html.Link(Model.Localize("Settings"), Url.Site()) %></li>
                    <li><%=Html.Link(Model.Localize("Plugins"), Url.Plugins()) %></li>
                </ul>
            </li>
            <li id="manageAreas">
                <h3><%=Model.Localize("ManageAreas", "Manage Areas") %></h3>
                <ul>
                    <li><%=Html.Link(Model.Localize("Area.Manage", "Edit Area"), adminData.Areas.Count > 1 ? Url.AreaFind() : Url.AreaEdit(adminData.Areas[0]))%></li>
                    <li><%=Html.Link(Model.Localize("Area.Add", "Add New area"), Url.AreaAdd()) %></li>
                    <li><%=Html.Link("BlogML", adminData.Areas.Count > 1 ? Url.AreaFind() : Url.BlogML(adminData.Areas[0])) %></li>
                </ul>
            </li>
            <li id="manageUsers">
                <h3><%=Model.Localize("ManageUsers", "Manage Users") %></h3>
                <ul>
                    <li><%=Html.Link(Model.Localize("ChangePassword", "Change Password"), Url.UserChangePassword()) %></li>
                </ul>
            </li>
        </ul>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Localize("Admin"), Model.Localize("Dashboard")) %>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Scripts"><%
    Html.RenderScriptTag("site.js");
 %>
</asp:Content>