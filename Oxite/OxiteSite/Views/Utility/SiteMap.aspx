<%@ Page Language="C#" AutoEventWireup="true" ContentType="text/xml" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<Post>>" %>
<%@ OutputCache Duration="21600" VaryByParam="none" %>
<%@ Import Namespace="Oxite.Mvc.Extensions"
%><?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"><%
    foreach (Post post in Model.List)
    { %>
   <url>
        <loc><%=Url.AbsolutePath(Url.Post(post)) %></loc><%--
        Add last modified date for all but the current month's sitemap --%>
    </url><%
    } %>
</urlset>