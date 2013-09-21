<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<ParentAndChild<PostBase, Comment>>>" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="Oxite.Mvc.Extensions"
%><?xml version="1.0" encoding="utf-8"?>
<feed xmlns="http://www.w3.org/2005/Atom">
  <title type="html"><%=Html.PageTitle(Model.Container.Name, "Comments") %></title>
  <icon><%=Url.AbsolutePath(Url.AppPath(Model.Site.FavIconUrl)) %></icon>
  <logo><%=Url.AbsolutePath(Url.AppPath(Model.Site.FavIconUrl.Replace(".ico", ".png"))) %></logo>
  <updated><%=XmlConvert.ToString(Model.List.First().Child.Created.Value, XmlDateTimeSerializationMode.RoundtripKind) %></updated><%
    if (!string.IsNullOrEmpty(Model.Site.Description))
    { %>
  <subtitle type="html"><%=Html.Encode(Model.Site.Description)%></subtitle><%
    } %>
  <id><%=Context.Request.Url.ToString().ToLower() %></id>
  <link rel="alternate" type="text/html" hreflang="<%=Model.Site.LanguageDefault %>" href="<%=Url.Container(Model.Container).ToLower()%>"/>
  <link rel="self" type="application/atom+xml" href="<%=Context.Request.Url.ToString() %>"/>
  <generator uri="<%=Url.Oxite() %>" version="1.0">Oxite</generator><%
    foreach (ParentAndChild<PostBase, Comment> pac in Model.List)
    {
        string itemUrl = Url.AbsolutePath(Url.Comment(pac.Parent as Post, pac.Child)).Replace("%23", "#"); %>
  <entry>
    <title type="html"><%=Html.Encode(pac.Parent.Title)%></title>
    <link rel="alternate" type="text/html" href="<%=itemUrl%>"/>
    <id><%=itemUrl%></id>
    <updated><%=XmlConvert.ToString(pac.Child.Modified.Value, XmlDateTimeSerializationMode.RoundtripKind)%></updated>
    <published><%=XmlConvert.ToString(pac.Child.Created.Value, XmlDateTimeSerializationMode.RoundtripKind)%></published>
    <author>
      <name><%=Html.Encode(pac.Child.Creator.Name)%></name>
    </author>
    <content type="html" xml:lang="<%=Model.Site.LanguageDefault %>">
      <%=Html.Encode(pac.Child.Body)%>
    </content>
  </entry><%
    } %>
</feed>
