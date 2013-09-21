<%@ Import Namespace="System.Globalization"%>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelItem<Post>>" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
    <form method="post" id="comment" action="<%=Url.AddCommentToPost(Model.Item) %>#comment"><%
        if (Request.QueryString.Get("pending") == bool.TrueString)
        {
            %><div class="message info"><%=Model.Localize("PendingComment", "Thanks for the comment. It'll show up here pending admin approval.") %></div><%
        }
        %>
        <fieldset class="info">
            <legend><%=Model.Localize("Your Information") %></legend>
            <div id="comment_grav"><%= Html.Gravatar("48") %></div>
            <p class="gravatarhelp"><%= string.Format(Model.Localize("&lt;-- It's a {0}"), Html.Link(Model.Localize("gravatar"), Model.Localize("http://en.gravatar.com/site/signup"))) %></p>
            <div class="name">
                <label for="comment_name"><%=Model.Localize("Name") %></label>
                <%=Html.TextBox("name", (Request.Form["name"] ?? (Model.User != null ? Model.User.Name : "")), new { id = "comment_name", @class = "text", tabindex = "1", title = Model.Localize("comment_name", "Your name...") })%><%= Html.ValidationMessage("UserBase.Name", "You must provide a name.") %>
            </div>
            <div class="email">
                <label for="comment_email"><%=Model.Localize("Email") %></label>
                <%=Html.TextBox("email", (Request.Form["email"] ?? (Model.User != null ? Model.User.Email : "")), new { id = "comment_email", @class = "text", tabindex = "2", title = Model.Localize("comment_email","Your email...") }) %><%= Html.ValidationMessage("PostSubscription.Email", "Valid email required to subscribe.") %>
                <span><%=Model.Localize("email saved for notifications but never distributed") %></span>
            </div>
            <div class="url">
                <label for="comment_url"><%=Model.Localize("URL") %></label>
                <%=Html.TextBox("url", (Request.Form["url"] ?? (Model.User != null ? Model.User.Url : "")), new { id = "comment_url", @class = "text", tabindex = "3", title = Model.Localize("comment_url", "Your home on the interwebs (URL)...") })%><%= Html.ValidationMessage("UserBase.Url", "URL looks a little off. URL encoding stuff like quotes and angle brackets might help.") %>
            </div>
            <div class="remember">
                <%=Html.CheckBox("remember", Request.Form.IsTrue("remember") || (Model.User != null && !string.IsNullOrEmpty(Model.User.Email)), new { id = "comment_remember", tabindex = "5" })%>
                <label for="comment_remember"><%=Model.Localize("Remember your info?") %></label>
            </div>
            <div class="subscribe">
                <%=Html.CheckBox("subscribe", Request.Form.IsTrue("subscribe"), new { id = "comment_subscribe", tabindex = "6" })%>
                <label for="comment_subscribe"><%=Model.Localize("Subscribe?") %></label>
            </div>
            <div class="submit">
                <input type="submit" value="<%=Model.Localize("Submit Comment") %>" id="comment_submit" class="submit button" tabindex="7" />
            </div>
        </fieldset>
        <fieldset class="comment">
            <legend><%=Model.Localize("your comment") %></legend>
            <label for="comment_body"><%= Model.Localize("Leave a comment...") %></label><%=Html.ValidationMessage("Comment.Body") %>
            <%=Html.TextArea("body", Request.Form["body"] ?? "", 12, 60, new { id = "comment_body", tabindex = "4", title = Model.Localize("comment_body", "Leave a comment...") })%>
        </fieldset>
    </form>
