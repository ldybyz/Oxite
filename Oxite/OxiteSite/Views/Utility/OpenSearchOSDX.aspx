<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<OxiteModel>" ContentType="application/opensearchdescription+xml" %>
<%@ Import Namespace="Oxite.Mvc.Extensions"
%><OpenSearchDescription xmlns="http://a9.com/-/spec/opensearch/1.1/">
    <ShortName><%=string.Format(Model.Localize("Search.ShortName", "{0} Search"), Model.Site.DisplayName)%></ShortName>
    <Description><%=string.Format(Model.Localize("Search.Description", "Search the content on {0}"), Model.Site.DisplayName)%></Description>
    <Image height="16" width="16" type="image/x-icon"><%=Url.AbsolutePath(Url.AppPath(Model.Site.FavIconUrl)) %></Image>
    <Image height="64" width="64" type="image/png"><%=Url.AbsolutePath(Url.AppPath(Model.Site.FavIconUrl.Replace(".ico", ".png"))) %></Image>
    <Url type="text/html" template="<%=Server.UrlDecode(Url.AbsolutePath(Url.Search("{searchTerms}"))) %>" />
    <Url type="application/rss+xml" template="<%=Server.UrlDecode(Url.AbsolutePath(Url.Search("RSS", "{searchTerms}"))) %>" />
</OpenSearchDescription>
