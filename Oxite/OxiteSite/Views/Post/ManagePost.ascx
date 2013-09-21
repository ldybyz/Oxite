<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelItem<Post>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.ViewModels.Extensions" %><% 
                                                             
bool urlIsLocked = Model.Item.State == EntityState.Normal
    && Model.Item.Published.HasValue
    && Model.Item.Published.Value.AddHours(Model.Site.PostEditTimeout) < DateTime.Now.ToUniversalTime();

if (Model.User.GetCanAccessAdmin())
{ %>
<div class="admin manage buttons"><%
    if (Model.Item.State != EntityState.Removed)
    { %>
    <a href="<%=Url.EditPost(Model.Item) %>" title="<%=Model.Localize("Edit") %>" class="ibutton edit"><%=Html.SkinImage("/images/page_edit.png", Model.Localize("Edit"), new { width = 16, height = 16 })%></a><%
        if (!urlIsLocked)
        { %>
    <form class="remove post" method="post" action="<%=Url.RemovePost(Model.Item) %>">
        <fieldset>
            <input type="image" src="<%=Url.CssPath("/images/page_delete.png", Model) %>" alt="<%=Model.Localize("Remove") %>" title="<%=Model.Localize("Remove") %>" class="ibutton image remove" />
            <%=Html.Hidden("returnUri", Request.Url.AbsoluteUri)%>
            <%=Html.OxiteAntiForgeryToken(m => m.AntiForgeryToken) %>
        </fieldset>
    </form><%
        }
    } %>
</div><%
} %>