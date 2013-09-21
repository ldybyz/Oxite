<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModel>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %><%
    if (Model.User != null && Model.User.IsAuthenticated)
    { %>
<%=string.Format(Model.Localize("WelcomeUserMessageFormat", "Welcome, {0}!"), string.Format("<span class=\"username\">{0}</span>", Html.Encode(Model.User.DisplayName))) %>
<span class="logout">&laquo; <%=Html.Link(Model.Localize("Logout"), Url.SignOut())%> &raquo;</span><%
    }
    else
    { %>
<span class="login">&laquo; <%=Html.Link(Model.Localize("Login"), Url.SignIn(Request.Url.PathAndQuery))%> &raquo;</span><%
    } %>