<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Admin.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<IPlugin>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="title">Plugins</h2><%
    if (Model.List.Count > 0)
    { %>
    <ul id="pluginCategories"><%
        string lastCategory = "";
        int categoryCount = 0;
        
        for (int i = 0; i < Model.List.Count; i++)
        {
            if (Model.List[i].Category != lastCategory)
            {
                if (i > 0)
                { %>
            </ul>
        </li><%
                } %>
        <li class="category m3<%=categoryCount++ % 3 %>">
            <h3><%=Model.List[i].Category %></h3>
            <ul class="plugins"><%
            } %>
                <li class="<%=(Model.List[i].Enabled ? "enabled" : "disabled") %>"><%=Html.Link(Model.List[i].Name, Url.Plugin(Model.List[i])) %></li><%
            lastCategory = Model.List[i].Category;
        } %>
            </ul>
        </li>
    </ul><%
    }
    else
    { %>
    <div class="message info"><%=Model.Localize("NoPlugins", "No plugins found") %></div><%
    } %>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Localize("Admin"), Model.Localize("Plugins")) %>
</asp:Content>
