<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelItem<Post>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
    <form method="post" id="comment" action="<%=Url.Post(Model.Item) %>#comment" class="user">
        <div class="avatar"><%=Html.Gravatar("48") %></div>
        <fieldset class="comment">
            <legend><%=Model.Localize("your comment") %></legend>
            <div>
                <label for="comment_body"><%=Model.Localize("Leave a comment...") %></label><%=Html.ValidationMessage("Comment.Body") %>
                <%=Html.TextArea("body", Request.Form["body"] ?? "", 12, 60, new { id = "comment_body", @class = "authed", title = Model.Localize("comment_body", "Leave a comment...") })%>
            </div>
            <div class="subscribe">
                <%=Html.CheckBox("subscribe", Request.Form.IsTrue("subscribe"), new { id = "comment_subscribe" }) %>
                <label for="comment_subscribe">Subscribe?</label>
            </div>
            <div class="submit">
                <input type="submit" value="<%=Model.Localize("Submit Comment") %>" id="comment_submit" class="submit button" />
                <%=Html.OxiteAntiForgeryToken(m => m.AntiForgeryToken) %>
            </div>
        </fieldset>
    </form>