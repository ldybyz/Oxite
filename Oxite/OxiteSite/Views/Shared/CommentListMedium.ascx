<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelList<ParentAndChild<PostBase, Comment>>>" %>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.ViewModels.Extensions" %><%
if (Model.List.Count > 0)
{ %><ul class="comments medium"><%
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
             <% Html.RenderPartial(
                   "Comment",
                   new OxiteModelPartial<ParentAndChild<Post, Comment>>(
                       Model, 
                       new ParentAndChild<Post, Comment>
                           {
                               Parent = postBaseAndComment.Parent as Post,
                               Child = postBaseAndComment.Child
                           })); %>
    </li><%
        counter++;
    } %>
</ul><% 
} %>