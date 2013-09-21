<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelItem<Post>>" %>
<%@ Import Namespace="Oxite.Extensions" %>
<div id="comments"><% 
    string statusClass = "status";
    if (Model.Item.Comments.Count < 1)
        statusClass = statusClass + " empty";
    %>
	<div class="<%=statusClass %>">
		<h3><%= string.Format(Model.Item.Comments.Count == 1 ? Model.Localize("SingleCommentStatus", "{0} Comment") : Model.Localize("MultiCommentStatus", "{0} Comments"), Model.Item.Comments.Count)%></h3>
		<div><a href="#comment"><%=Model.Localize("leave your own")%></a></div>
	</div>
	<%
	//todo: (nheskew) not this. it's _real_ ugly and even worse being in a view...   
	Html.RenderPartial("CommentListMedium", new OxiteModelList<ParentAndChild<PostBase, Comment>> { List = Model.Item.Comments.Select(c => new ParentAndChild<PostBase, Comment> { Parent = Model.Item, Child = c }).ToList(), Container = Model.Container, Site = Model.Site, SkinName = Model.SkinName, User = Model.User, AntiForgeryToken = Model.AntiForgeryToken }); %><%
    
    if (!Model.CommentingDisabled)
    {
        if (Model.User != null && Model.User.IsAuthenticated)
        {
            Html.RenderPartial("CommentFormAuthenticated");
        }
        else
        {
            Html.RenderPartial("CommentFormAnonymous");
        }
    } %>
</div>