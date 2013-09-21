<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModel>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %>
            <div class="powered"><span><%=Model.Localize("PoweredBy", "Powered by") %> </span><%=Html.Link("Oxite", Url.Oxite()) %></div>
            <div class="credits"><%=Html.Link("famfamfam", "http://www.famfamfam.com", new { id = "famfamfam", title = Model.Localize("IconsByFamFamFam", "Icons by famfamfam") })%></div>