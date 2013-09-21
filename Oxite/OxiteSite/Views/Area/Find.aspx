<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Admin.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<Area>>" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"><%
    AreaSearchCriteria searchCriteria = Model.GetModelItem<AreaSearchCriteria>(); %>
            <div class="sections">
                <div class="primary">
                    <h2 class="title"><%=Model.Localize("Areas.Find", "Find Area")%></h2>
                    <form action="" id="area" method="post">
                        <div class="add"><%=Html.Link(Model.Localize("Areas.Create", "Add an Area"), Url.AreaAdd())%></div>
                        <div class="find"><%=Html.TextBox("areaNameSearch", m => searchCriteria != null ? searchCriteria.Name : "", "Find an Area", new { size = 40, @class = "text" })%></div>
                        <div class="buttons"><input type="submit" name="findArea" class="button submit" value="<%=Model.Localize("Find") %>" /><%=
                            Html.OxiteAntiForgeryToken(m => m.AntiForgeryToken) %></div>
                    </form><%
    if (Model.List != null && Model.List.Count > 0)
    {
        Response.Write(
            Html.UnorderedList(
                Model.List,
                a => string.Format("<a href=\"{1}\">{0}</a>",
                    a.Name + (!string.IsNullOrEmpty(a.DisplayName) ? string.Format(" ({0})", a.DisplayName) : ""),
                    Url.AreaEdit(a)
                ),
                "areas"
            )
        );
    }
    else if (searchCriteria != null)
    { %>
                    <p><%=Model.Localize("Areas.NoneFound", "No areas found.") %></p><%
    } %>
                </div>
                <div class="secondary"><% 
                    Html.RenderPartial("SideBar"); %>
                </div>
            </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="Title" runat="server">
    <%=Html.PageTitle(Model.Localize("Admin"), Model.Localize("Areas.Find", "Find Area")) %>
</asp:Content>
<asp:Content ContentPlaceHolderID="Scripts" runat="server"><%
    Html.RenderScriptTag("site.js"); %>
</asp:Content>