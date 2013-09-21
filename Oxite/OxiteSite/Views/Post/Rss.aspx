<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<OxiteModelList<Post>>" %>
<%@ Import Namespace="Oxite.Mvc.Extensions"
%><rss version="2.0" xmlns:dc="http://purl.org/dc/elements/1.1/">
    <channel>
        <title><%=Html.PageTitle(Model.Container.Name) %></title>
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
    foreach (Post post in Model.List)
    {
        string postUrl = Url.AbsolutePath(Url.Post(post)); %>
        <item>
            <dc:creator><%=Html.Encode(post.Creator.Name)%></dc:creator>
            <title><%=Html.Encode(post.Title)%></title>
            <description><%=Html.Encode(post.Body)%></description>
            <link><%=postUrl %></link>
            <guid isPermaLink="true"><%=postUrl %></guid>
            <pubDate><%=post.Published.Value.ToStringForFeed()%></pubDate><%
        if (Model.Site.HasMultipleAreas)
        { %>
            <category><%=post.Area.Name %></category><%
        }
        foreach (Tag tag in post.Tags)
        { %>
            <category><%=tag.Name %></category><%
        } %>
        </item><%
    } %>
    </channel>
</rss>
