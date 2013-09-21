<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelPartial<ArchiveViewModel>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %><%
if (Model.PartialModel != null)
{
    var months = Model.PartialModel.Archives; %>
        <div class="sub archives">
            <h3><%=Model.RootModel.Localize("Archives") %></h3><%
    if (Model.PartialModel.Archives != null && Model.PartialModel.Archives.Count > 0)
    {
        if (months.Count > 20)
        { %>
            <% Html.RenderPartial("ArchivesByYear"); %><%
        }
        else
        {
            Response.Write(
                Html.UnorderedList(
                months,
                t => Html.Link(
                    string.Format("{0:MMMM yyyy} ({1})", t.Key.ToDateTime(), t.Value),
                    Url.Posts(t.Key.Year, t.Key.Month)),
                "archiveMonthList"
                )
            );
        } 
    }
    else
    {
        %><div class="message info"><%=Model.RootModel.Localize("NoneFound", "None found.")%></div><%
    } %>
        </div><%
} %>
