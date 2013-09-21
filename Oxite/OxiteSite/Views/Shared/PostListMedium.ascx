<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelList<Post>>" %>
<%@ Import Namespace="Oxite.Extensions" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" 
%><%
if (((IPageOfList<Post>)Model.List).TotalItemCount > 0)
{ %><ul class="posts medium"><%
    int counter = 0;
    foreach (Post post in Model.List)
    {
        StringBuilder className = new StringBuilder("post", 15);
        
        if (post.Equals(Model.List.First())) { className.Append(" first"); }
        if (post.Equals(Model.List.Last())) { className.Append(" last"); }

        if (counter % 2 != 0) { className.Append(" odd"); }
        %>
    <li class="<%=className.ToString() %>">
        <h2 class="title"><%=Html.Link(post.Title.CleanText(), Url.Post(post)) %></h2>
        <div class="posted"><%=post.Published.HasValue ? Html.ConvertToLocalTime(post.Published.Value).ToLongDateString() : "Draft" %></div>
        <div class="content"><%=post.GetBodyShort() %></div>                            
        <div class="more"><%
        if (Model.Site.HasMultipleAreas)
            Response.Write(string.Format(
                Model.Localize("From the {0} Blog. | "),
                Html.Link(post.Area.Name.CleanText(), Url.Posts(post.Area))
                ));
        
        if (post.Tags.Count > 0)
        {
            Response.Write(
                string.Format(
                    "{0} {1} | ", 
                    Model.Localize("Filed under"),
                    Html.UnorderedList(
                        post.Tags,
                        (t, i) => Html.Link(t.Name.CleanText(), Url.Posts(t), new { rel = "tag" }),
                        "tags"
                    )
                )
            );
        }
        %><%=Html.Link(string.Format("{0} comment{1}", post.Comments.Count, post.Comments.Count == 1 ? "" : "s"), string.Format("{0}#comments", Url.Post(post))) 
        %> <%=Html.Link("&raquo;", Url.Post(post), new { @class = "arrow" }) %></div>
    </li><%
        counter++;
    } %>
</ul><% 
} 
else
{ //todo: (nheskew) need an Html.Message html helper extension method that takes a message %>
<div class="message info"><%=Model.Localize("NoneFound", "None found.")%></div><%        
} %>