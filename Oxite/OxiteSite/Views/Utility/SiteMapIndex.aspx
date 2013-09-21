<%@ Page Language="C#" AutoEventWireup="true" ContentType="text/xml" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<DateTime>>" %>
<%@ OutputCache Duration="21600" VaryByParam="none" %>
<%@ Import Namespace="Oxite.Mvc.Extensions"
%><?xml version="1.0" encoding="UTF-8"?>
<sitemapindex xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"><%
    foreach (DateTime dt in Model.List)
    { %>
   <sitemap>
      <loc><%=Url.AbsolutePath(Url.SiteMap(dt.Year, dt.Month))%></loc><%--
      Add last modified date for all but the current month's sitemap --%>
   </sitemap><%
    } %>
</sitemapindex>
