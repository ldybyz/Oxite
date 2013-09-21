<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<Post>>" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="Oxite.Mvc.Extensions"
%><?xml version="1.0" encoding="utf-8"?>
<feed xmlns="http://www.w3.org/2005/Atom">
  <title type="html"><%=Html.PageTitle(Model.Container.Name) %></title>
  <icon><%=Url.AbsolutePath(Url.AppPath(Model.Site.FavIconUrl)) %></icon>
  <logo><%=Url.AbsolutePath(Url.AppPath(Model.Site.FavIconUrl.Replace(".ico", ".png"))) %></logo>
  <updated><%=XmlConvert.ToString(Model.List.First().Published.Value, XmlDateTimeSerializationMode.RoundtripKind) %></updated><%
    if (!string.IsNullOrEmpty(Model.Site.Description))
    { %>
  <subtitle type="html"><%=Html.Encode(Model.Site.Description)%></subtitle><%
    } %>
  <id><%=Context.Request.Url.ToString().ToLower() %></id>
  <link rel="alternate" type="text/html" hreflang="<%=Model.Site.LanguageDefault %>" href="<%=Url.Container(Model.Container).ToLower() %>"/>
  <link rel="self" type="application/atom+xml" href="<%=Context.Request.Url.ToString() %>"/>
  <generator uri="<%=Url.Oxite() %>" version="1.0">Oxite</generator><%
    foreach (Post post in Model.List)
    {
        string postUrl = Url.AbsolutePath(Url.Post(post)); %>
  <entry>
    <title type="html"><%=Html.Encode(post.Title)%></title>
    <link rel="alternate" type="text/html" href="<%=postUrl %>"/>
    <id><%=postUrl %></id>
    <updated><%=XmlConvert.ToString(post.Modified.Value, XmlDateTimeSerializationMode.RoundtripKind)%></updated>
    <published><%=XmlConvert.ToString(post.Published.Value, XmlDateTimeSerializationMode.RoundtripKind)%></published>
    <author>
      <name><%=Html.Encode(post.Creator.Name)%></name>
    </author><%
        if (Model.Site.HasMultipleAreas)
        { %>
    <category term="<%=post.Area.Name %>" /><%
        }
        foreach (Tag tag in post.Tags)
        { %>
    <category term="<%=tag.Name %>" /><%
        } %>
    <content type="html" xml:lang="<%=Model.Site.LanguageDefault %>">
      <%=Html.Encode(post.Body)%>
    </content>
  </entry><%
    } %>
</feed>
