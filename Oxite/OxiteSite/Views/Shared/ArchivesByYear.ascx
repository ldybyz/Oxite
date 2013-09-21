<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModelPartial<ArchiveViewModel>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %><%
var months = Model.PartialModel.Archives;
                                              
if (months != null && months.Count() > 0)
{ %>
            <ul class="yearList"><%
    int lastYear = months.First().Key.Year;
    int firstYear = months.Last().Key.Year;

    for (int year = lastYear; year >= firstYear; year--)
    {
        var yearMonths = months.Where(m => m.Key.Year == year);

        if (year == lastYear)
        { %>
                <li><h4><%=year %></h4><%
        } else { %>
                <li class="previous"><h4><%=year %> <span>(<%=yearMonths.Sum( ym => ym.Value) %>)</span></h4><%
        }
        Response.Write(
            Html.UnorderedList(
                yearMonths,
                t => Html.Link(
                    string.Format("{0:MMMM} ({1})", t.Key.ToDateTime(), t.Value),
                    Url.Posts(t.Key.Year, t.Key.Month)),
                "archiveMonthList"
                )
            ); %></li><% 
    } %>
            </ul><%
} %>