<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelItem<Oxite.Model.Page>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.ViewModels.Extensions" %><% 

bool urlIsLocked = Model.Item.HasChildren;

if (Model.User.GetCanAccessAdmin())
{ %>
<div class="admin manage buttons"><% 
    if (Model.Item.State != EntityState.Removed)
    { %>
<a href="<%=Url.AddPage(Model.Item) %>" title="<%=Model.Localize("Add") %>" class="ibutton add"><%=Html.SkinImage("/images/page_add.png", Model.Localize("Add"), new { width = 16, height = 16 })%></a>
<a href="<%=Url.EditPage(Model.Item) %>" title="<%=Model.Localize("Edit") %>" class="ibutton edit"><%=Html.SkinImage("/images/page_edit.png", Model.Localize("Edit"), new { width = 16, height = 16 })%></a><%
        if (!urlIsLocked)
        { %>
<form class="remove post" method="post" action="<%=Url.RemovePage(Model.Item) %>">
    <fieldset>
        <input type="image" src="<%=Url.CssPath("/images/page_delete.png", Model) %>" alt="<%=Model.Localize("Remove") %>" title="<%=Model.Localize("Remove") %>" class="ibutton image remove" />
        <%=Html.Hidden("returnUri", Url.Page(Model.Item.Parent))%>
        <%=Html.OxiteAntiForgeryToken(m => m.AntiForgeryToken) %>
    </fieldset>
</form><%
        } 
    } else
    {
        %><%=Model.Localize("Removed") %><%
    } %>
</div><%
} %>