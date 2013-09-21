<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<ParentAndChild<PostBase, Comment>>>" %>
<%@ Import Namespace="Oxite.Model.Extensions" %>
<%@ Import Namespace="Oxite.Mvc.Extensions"
%><rss version="2.0" xmlns:dc="http://purl.org/dc/elements/1.1/">
    <channel>
        <title><%=Html.PageTitle(Model.Container.Name, "Comments")%></title>
        <description><%=Model.Site.Description %></description>
        <link><%=Url.Container(Model.Container) %></link>
        <language><%=Model.Site.LanguageDefault %></language>
        <image>
            <url><%=Url.AbsolutePath(Url.AppPath(Model.Site.FavIconUrl.Replace(".ico", ".png"))) %></url>
            <title><%=Model.Site.DisplayName %></title>
            <link><%=Url.Container(Model.Container)%></link>
            <width>64</width>
            <height>64</height>
        </image><%
    foreach (ParentAndChild<PostBase, Comment> pac in Model.List)
    {
        string postUrl = Url.AbsolutePath(Url.Comment(pac.Parent as Post, pac.Child)).Replace("%23", "#"); %>
        <item>
            <dc:creator><%=Html.Encode(pac.Child.Creator.Name) %></dc:creator>
            <title>RE: <%=Html.Encode(pac.Parent.Title) %></title>
            <description><%=Html.Encode(pac.Child.Body) %></description>
            <link><%=postUrl %></link>
            <guid isPermaLink="true"><%=postUrl %></guid>
            <pubDate><%=pac.Child.Created.Value.ToStringForFeed()%></pubDate>
        </item><%
    } %>
    </channel>
</rss>