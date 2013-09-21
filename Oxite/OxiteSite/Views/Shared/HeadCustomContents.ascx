<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<OxiteModel>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions" %><%
    Html.RenderCssFile("yui.reset.2.6.0.css");
    Html.RenderCssFile("site.css");
    Html.RenderScriptTag("jquery-1.3.1.js", "jquery-1.3.1.min.js"); %>