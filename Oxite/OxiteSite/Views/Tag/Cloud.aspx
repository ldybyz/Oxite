<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<KeyValuePair<Tag, int>>>" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="sections">
        <div class="primary">
            <h2 class="title">Tags</h2><%
        double? averagePostCount = null;
            double? standardDeviationPostCount = null;
            
            Response.Write(
                Html.UnorderedList(
                    Model.List.OrderBy(kvp => kvp.Key.Name),
                    t => string.Format(
                        "<a href=\"{2}\" rel=\"tag\" class=\"t{3}\">{0} ({1})</a> ",
                        t.Key.Name,
                        t.Value,
                        Url.Posts(t.Key),
                        t.Key.GetTagWeight(Model.List, ref averagePostCount, ref standardDeviationPostCount)
                    ),
                    "tagCloud"
                )
            ); %>
        </div>
        <div class="secondary"><% 
            Html.RenderPartial("SideBar"); %>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Scripts"><%
    Html.RenderScriptTag("site.js"); %>
</asp:Content>