<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModel>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.ViewModels.Extensions" %>
            <div id="title">
                <h1><a href="<%=Url.Posts() %>"><%=Model.Site.DisplayName %></a></h1>
            </div>
            <div id="logindisplay"><% Html.RenderPartial("LoginUserControl"); %></div>
            <div id="menucontainer">
                <ul class="menu">
                    <li class="home<%=Url.IsHome() ? " selected" : "" %>"><%=Html.Link(Model.Localize("HomeMenuItem", "Home"), Url.Posts()) %></li>
                    <li class="about<%=Url.IsPagePath("About") ? " selected" : "" %>"><%=Html.Link(Model.Localize("AboutMenuItem", "About"), Url.Page("About")) %></li>
                </ul><%
    if (Model.User.GetCanAccessAdmin())
    { %>
                <ul class="menu admin">
                    <li><%=Html.Link(Model.Localize("Admin", "Admin"), Url.Admin(), new { @class = "admin" })%></li>
                    <li><%=Html.Link(Model.Localize("AddPostLinkText", "Add Post"), Url.AddPost(Model.Container as Area))%></li>
                    <li><%=Html.Link(Model.Localize("AddPageLinkText", "Add Page"), Url.AddPage())%></li>
                    <li><%=Html.Link(Model.Localize("ManageSiteLinkText", "Manage Site"), Url.ManageSite())%></li>
                </ul><%
    } %>
            </div>