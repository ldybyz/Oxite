<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelList<ParentAndChild<PostBase, Comment>>>" %>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.ViewModels.Extensions" %><%
if (((IPageOfList<ParentAndChild<PostBase, Comment>>)Model.List).TotalItemCount > 0)
{ %><ul class="comments small"><%
    int counter = 0;
    foreach (ParentAndChild<PostBase, Comment> postBaseAndComment in Model.List)
    {
        StringBuilder className = new StringBuilder("comment", 40);

        if (postBaseAndComment.Equals(Model.List.First())) { className.Append(" first"); }
        if (postBaseAndComment.Equals(Model.List.Last())) { className.Append(" last"); }

        if (Model.User.GetCanAccessAdmin()) { className.AppendFormat(" {0}", postBaseAndComment.Child.State.ToString().ToLower()); }

        if (counter % 2 != 0) { className.Append(" odd"); }

        className.Append(!(postBaseAndComment.Child.Creator is User) 
            ? " anon" 
            : string.Format(
                " {0} {1}",
                postBaseAndComment.Child.Creator.ID == postBaseAndComment.Child.Creator.ID ? "author" : "user",
                postBaseAndComment.Child.Creator.Name.CleanAttribute()
                )
            ); 
        %>
    <li class="<%=className.ToString() %>">
            <div class="meta" id="<%=postBaseAndComment.Child.GetSlug() %>"><%
                if (Model.User.GetCanAccessAdmin() && postBaseAndComment.Child.State == EntityState.PendingApproval)
                {
                %><span class="state" title="<%=Model.Localize("PendingApproval", "Pending Approval") %>"><%=Model.Localize("PendingApproval", "Pending Approval") %></span><%
                } %>
                <span class="name"><%=Html.LinkOrDefault(postBaseAndComment.Child.Creator.Name.CleanText(), postBaseAndComment.Child.Creator.Url.CleanHref()) %></span>
                <span class="when"> - <%= 
                    Html.Link(
                        Html.ConvertToLocalTime(postBaseAndComment.Child.Created.Value, Model).ToString("MMMM dd, yyyy - h:mm tt"), //todo: (nheskew) localize date format
                        Model.User.GetCanAccessAdmin() && postBaseAndComment.Child.State == EntityState.PendingApproval
                            ? Url.ManageComment(postBaseAndComment.Child) //todo: (nheskew)need the route to work with a page of comments
                            : Url.Comment(postBaseAndComment.Parent as Post, postBaseAndComment.Child)
                        )%></span>
            </div>
            <div class="post"><%=postBaseAndComment.Parent.Title.CleanText() %></div>
            <div class="text"><%=postBaseAndComment.Child.Body.CleanText() %></div>
    </li><%
        counter++;
    } %>
</ul><% 
} 
else
{ //todo: (nheskew) need an Html.Message html helper extension method that takes a message %>
<div class="message info"><%=Model.Localize("NoneFound", "None found.") %></div><%        
} %>