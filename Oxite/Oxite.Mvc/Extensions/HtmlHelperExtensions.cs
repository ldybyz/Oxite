//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Oxite.Extensions;
using Oxite.Model;
using Oxite.Mvc.Infrastructure;
using Oxite.Mvc.ViewModels;

namespace Oxite.Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        #region OxiteAntiForgeryToken

        public static string OxiteAntiForgeryToken<TModel>(this HtmlHelper<TModel> htmlHelper, Func<TModel, AntiForgeryToken> getToken) where TModel : class
        {
            AntiForgeryToken token = getToken(htmlHelper.ViewData.Model);

            if (token == null) return "";

            htmlHelper.ViewContext.HttpContext.Response.Cookies.Add(new System.Web.HttpCookie(AntiForgeryToken.TicksName, token.Ticks.ToString()));

            return string.Format("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", AntiForgeryToken.TokenName, token.Value);
        }

        #endregion

        #region CheckBox

        public static string CheckBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<TModel, bool> getIsChecked, string labelInnerHtml) where TModel : OxiteModel
        {
            return CheckBox(htmlHelper, name, getIsChecked, labelInnerHtml, true);
        }

        public static string CheckBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<TModel, bool> getIsChecked, string labelInnerHtml, bool enabled) where TModel : OxiteModel
        {
            return CheckBox(htmlHelper, name, getIsChecked, null, enabled, null);
        }

        public static string CheckBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<TModel, bool> getIsChecked, string labelInnerHtml, object htmlAttributes) where TModel : OxiteModel
        {
            return CheckBox(htmlHelper, name, getIsChecked, labelInnerHtml, true, htmlAttributes);
        }

        public static string CheckBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<TModel, bool> getIsChecked, string labelInnerHtml, bool enabled, object htmlAttributes) where TModel : OxiteModel
        {
            bool value = htmlHelper.ViewContext.HttpContext.Request.Form.GetValues(name) != null
                ? htmlHelper.ViewContext.HttpContext.Request.Form.IsTrue(name)
                : getIsChecked(htmlHelper.ViewData.Model);

            RouteValueDictionary attributes = new RouteValueDictionary(htmlAttributes);

            if (!enabled)
                attributes.Add("disabled", "disabled");

            return fieldWithLabel(htmlHelper, name, () => htmlHelper.CheckBox(name, value, attributes), labelInnerHtml, enabled);
        }

        #endregion

        #region ConvertToLocalTime

        public static DateTime ConvertToLocalTime<TModel>(this HtmlHelper<TModel> htmlHelper, DateTime dateTime) where TModel : OxiteModel
        {
            return ConvertToLocalTime(htmlHelper, dateTime, htmlHelper.ViewData.Model);
        }

        public static DateTime ConvertToLocalTime(this HtmlHelper htmlHelper, DateTime dateTime, OxiteModel model)
        {
            if (model.User == null)
            {
                if (model.Site.TimeZoneOffset != 0)
                    return dateTime.Add(TimeSpan.FromHours(model.Site.TimeZoneOffset));

                return dateTime;
            }

            return dateTime; //TODO: (erikpo) Get the timezone offset from the current user and apply it
        }

        #endregion

        #region Gravatar

        public static string Gravatar<TModel>(this HtmlHelper<TModel> htmlHelper, string size) where TModel : OxiteModel
        {
            OxiteModel model = htmlHelper.ViewData.Model;

            return htmlHelper.Gravatar(
                model.User != null ? (model.User.HashedEmail ?? model.User.Email.ComputeHash()).CleanAttribute() : null,
                model.User != null ? (model.User.DisplayName ?? model.User.Name).CleanAttribute() : null, 
                size, 
                model.Site.GravatarDefault
                );
        }

        public static string Gravatar<TModel>(this HtmlHelper<TModel> htmlHelper, UserBase user, string size) where TModel : OxiteModel
        {
            OxiteModel model = htmlHelper.ViewData.Model;

            return htmlHelper.Gravatar(
                user != null ? user.HashedEmail.CleanAttribute() : null,
                user != null ? (user.DisplayName ?? user.Name).CleanAttribute() : null,
                size,
                model.Site.GravatarDefault
                );
        }

        public static string Gravatar(this HtmlHelper htmlHelper, OxiteModel model, UserBase user, string size)
        {
            return htmlHelper.Gravatar(
                user != null ? user.HashedEmail.CleanAttribute() : null,
                user != null ? (user.DisplayName ?? user.Name).CleanAttribute() : null,
                size,
                model.Site.GravatarDefault
                );
        }

        public static string Gravatar(this HtmlHelper htmlHelper, string id, string name, string size, string defaultImage)
        {
            return htmlHelper.Image(
                string.Format("http://www.gravatar.com/avatar/{0}?s={1}&d={2}", id ?? "@", size, defaultImage),
                string.Format("{0} (gravatar)", name ?? "Mrs. Gravatar"),
                new RouteValueDictionary(new {width = size, height = size, @class = "gravatar"})
                );
        }

        #endregion

        #region HeadLink

        public static string HeadLink(this HtmlHelper htmlHelper, string rel, string href, string type, string title)
        {
            return htmlHelper.HeadLink(rel, href, type, title, null);
        }

        public static string HeadLink(this HtmlHelper htmlHelper, string rel, string href, string type, string title, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("link");

            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (!string.IsNullOrEmpty(rel))
            {
                tagBuilder.MergeAttribute("rel", rel);
            }
            if (!string.IsNullOrEmpty(href))
            {
                tagBuilder.MergeAttribute("href", href);
            }
            if (!string.IsNullOrEmpty(type))
            {
                tagBuilder.MergeAttribute("type", type);
            }
            if (!string.IsNullOrEmpty(title))
            {
                tagBuilder.MergeAttribute("title", title);
            }

            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        #endregion

        #region Image

        public static string Image(this HtmlHelper helper, string src, string alt, IDictionary<string, object> htmlAttributes)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            string imageUrl = url.Content(src);
            TagBuilder imageTag = new TagBuilder("img");

            if (!string.IsNullOrEmpty(imageUrl))
            {
                imageTag.MergeAttribute("src", imageUrl);
            }

            if (!string.IsNullOrEmpty(alt))
            {
                imageTag.MergeAttribute("alt", alt);
            }

            imageTag.MergeAttributes(htmlAttributes, true);

            if (imageTag.Attributes.ContainsKey("alt") && !imageTag.Attributes.ContainsKey("title"))
            {
                imageTag.MergeAttribute("title", imageTag.Attributes["alt"] ?? "");
            }

            return imageTag.ToString(TagRenderMode.SelfClosing);
        }

        #endregion

        #region Input

        public static string DropDownList(this HtmlHelper htmlHelper, string name, SelectList selectList, object htmlAttributes, bool isEnabled)
        {
            RouteValueDictionary htmlAttributeDictionary = new RouteValueDictionary(htmlAttributes);

            if (!isEnabled)
            {
                htmlAttributeDictionary["disabled"] = "disabled";

                StringBuilder inputItemBuilder = new StringBuilder();
                inputItemBuilder.AppendLine(htmlHelper.DropDownList(string.Format("{0}_view", name), selectList,
                                                                    htmlAttributeDictionary));
                inputItemBuilder.AppendLine(htmlHelper.Hidden(name, selectList.SelectedValue));
                return inputItemBuilder.ToString();
            }

            return htmlHelper.DropDownList(name, selectList, htmlAttributeDictionary);
        }

        public static string TextBox(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes, bool isEnabled)
        {
            RouteValueDictionary htmlAttributeDictionary = new RouteValueDictionary(htmlAttributes);

            if (!isEnabled)
            {
                htmlAttributeDictionary["disabled"] = "disabled";

                StringBuilder inputItemBuilder = new StringBuilder();

                inputItemBuilder.Append(htmlHelper.TextBox(string.Format("{0}_view", name), value, htmlAttributeDictionary));
                inputItemBuilder.Append(htmlHelper.Hidden(name, value));

                return inputItemBuilder.ToString();
            }

            return htmlHelper.TextBox(name, value, htmlAttributeDictionary);
        }

        public static string RadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked, object htmlAttributes, bool isEnabled)
        {
            RouteValueDictionary htmlAttributeDictionary = new RouteValueDictionary(htmlAttributes);

            if (!isEnabled)
            {
                htmlAttributeDictionary["disabled"] = "disabled";

                StringBuilder inputItemBuilder = new StringBuilder();

                inputItemBuilder.AppendLine(htmlHelper.RadioButton(string.Format("{0}_view", name), value, isChecked, htmlAttributeDictionary));
                
                if (isChecked)
                {
                    inputItemBuilder.AppendLine(htmlHelper.Hidden(name, value));
                }
                
                return inputItemBuilder.ToString();
            }

            return htmlHelper.RadioButton(name, value, isChecked, htmlAttributeDictionary);
        }

        public static string Button(this HtmlHelper htmlHelper, string name, string buttonContent, object htmlAttributes)
        {
            return htmlHelper.Button(name, buttonContent, new RouteValueDictionary(htmlAttributes));
        }

        public static string Button(this HtmlHelper htmlHelper, string name, string buttonContent, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("button") { InnerHtml = buttonContent};

            tagBuilder.MergeAttributes(htmlAttributes);

            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region Link

        public static string Link(this HtmlHelper htmlHelper, string linkText, string href)
        {
            return htmlHelper.Link(linkText, href, null);
        }

        public static string Link(this HtmlHelper htmlHelper, string linkText, string href, object htmlAttributes)
        {
            return htmlHelper.Link(linkText, href, new RouteValueDictionary(htmlAttributes));
        }

        public static string Link(this HtmlHelper htmlHelper, string linkText, string href, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = linkText
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", href);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region LinkOrDefault

        public static string LinkOrDefault(this HtmlHelper htmlHelper, string linkContents, string href)
        {
            return htmlHelper.LinkOrDefault(linkContents, href, null);
        }

        public static string LinkOrDefault(this HtmlHelper htmlHelper, string linkContents, string href, object htmlAttributes)
        {
            return htmlHelper.LinkOrDefault(linkContents, href, new RouteValueDictionary(htmlAttributes));
        }

        public static string LinkOrDefault(this HtmlHelper htmlHelper, string linkContents, string href, IDictionary<string, object> htmlAttributes)
        {
            if (!string.IsNullOrEmpty(href))
            {
                TagBuilder tagBuilder = new TagBuilder("a")
                {
                    InnerHtml = linkContents
                };
                tagBuilder.MergeAttributes(htmlAttributes);
                tagBuilder.MergeAttribute("href", href);
                linkContents = tagBuilder.ToString(TagRenderMode.Normal);
            }

            return linkContents;
        }

        #endregion

        #region OpenSearchOSDXLink

        public static string OpenSearchOSDXLink<TModel>(this HtmlHelper<TModel> htmlHelper) where TModel : OxiteModel
        {
            OxiteModel model = htmlHelper.ViewData.Model;

            if (model.Site.IncludeOpenSearch && htmlHelper.ViewContext.HttpContext.Request.UserAgent.Contains("Windows NT 6.1"))
            {
                UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

                return htmlHelper.Link(string.Format(model.Localize("Search.WindowsSearch", "Search {0} in Windows"), model.Site.DisplayName), urlHelper.AppPath(urlHelper.OpenSearchOSDX()), new { @class = "windowsSearch" });
            }

            return "";
        }

        #endregion

        #region PageDescription

        public static string PageDescription<TModel>(this HtmlHelper<TModel> htmlHelper) where TModel : OxiteModel
        {
            return htmlHelper.PageDescription(null);
        }

        public static string PageDescription<TModel>(this HtmlHelper<TModel> htmlHelper, string description) where TModel : OxiteModel
        {
            if (description == null)
                description = htmlHelper.Encode(htmlHelper.ViewData.Model.Site.Description);

            return string.Format("<meta name=\"description\" content=\"{0}\" />", description.CleanHtmlTags().CleanHref());
        }

        #endregion

        #region PageTitle

        public static string PageTitle<TModel>(this HtmlHelper<TModel> htmlHelper, params string[] items) where TModel : OxiteModel
        {
            OxiteModel model = htmlHelper.ViewData.Model;

            if (items == null || items.Length == 0)
                return model.Site.DisplayName;
            
            StringBuilder sb = new StringBuilder(50);
            List<string> itemList = new List<string>(items);

            itemList.RemoveAll(s => s == null);

            itemList.Insert(0, model.Site.DisplayName);

            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                sb.Append(itemList[i]);

                if (i > 0)
                    sb.Append(model.Site.PageTitleSeparator);
            }

            return htmlHelper.Encode(sb.ToString());
        }

        #endregion

        #region PageState

        public static string PageState<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize)
        {
            if (pageOfAList.TotalPageCount > 1)
                return string.Format(
                    "<div class=\"pageState\">{0}</div>", 
                    string.Format(
                        localize("PageStateFormat", "{0}-{1} of {2}"), 
                        (pageOfAList.PageIndex * pageOfAList.PageSize) + 1, 
                        (pageOfAList.PageIndex * pageOfAList.PageSize) + pageOfAList.Count, 
                        pageOfAList.TotalItemCount
                        )
                    );

            return "";
        }

        #endregion

        #region Pager

        public static string PostListPager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize)
        {
            return PostListPager(htmlHelper, pageOfAList, localize, null, localize("NewerPager", "&laquo; Newer"), localize("OlderPager", "Older &raquo;"), false);
        }

        public static string PostListPager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize, object values, string previousText, string nextText, bool alwaysShowPreviousAndNext)
        {
            return SimplePager(htmlHelper, pageOfAList, "PageOfPosts", values, previousText, nextText, alwaysShowPreviousAndNext);
        }

        public static string PostListByAreaPager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize, string areaName)
        {
            return PostListByAreaPager(htmlHelper, pageOfAList, localize, new { areaName }, localize("NewerPager", "&laquo; Newer"), localize("OlderPager", "Older &raquo;"), false);
        }

        public static string PostListByAreaPager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize, object values, string previousText, string nextText, bool alwaysShowPreviousAndNext)
        {
            return SimplePager(htmlHelper, pageOfAList, "PageOfPostsByArea", values, previousText, nextText, alwaysShowPreviousAndNext);
        }

        public static string PostListByTagPager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize, string tagName)
        {
            return PostListByTagPager(htmlHelper, pageOfAList, localize, new { tagName }, localize("NewerPager", "&laquo; Newer"), localize("OlderPager", "Older &raquo;"), false);
        }

        public static string PostListByTagPager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize, object values, string previousText, string nextText, bool alwaysShowPreviousAndNext)
        {
            return SimplePager(htmlHelper, pageOfAList, "PageOfPostsByTag", values, previousText, nextText, alwaysShowPreviousAndNext);
        }

        public static string PostListBySearchPager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize)
        {
            return PostListBySearchPager(htmlHelper, pageOfAList, localize, null, localize("NewerPager", "&laquo; Newer"), localize("OlderPager", "Older &raquo;"), false);
        }

        public static string PostListBySearchPager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize, object values, string previousText, string nextText, bool alwaysShowPreviousAndNext)
        {
            return SimplePager(htmlHelper, pageOfAList, "PageOfPostsBySearch", values, previousText, nextText, alwaysShowPreviousAndNext);
        }

        public static string CommentListPager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize)
        {
            return CommentListPager(htmlHelper, pageOfAList, localize, null, localize("NewerPager", "&laquo; Newer"), localize("OlderPager", "Older &raquo;"), false);
        }

        public static string CommentListPager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize, object values, string previousText, string nextText, bool alwaysShowPreviousAndNext)
        {
            return SimplePager(htmlHelper, pageOfAList, "PageOfAllComments", values, previousText, nextText, alwaysShowPreviousAndNext);
        }

        public static string SimplePager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, string routeName, object values, string previousText, string nextText, bool alwaysShowPreviousAndNext)
        {
            if (pageOfAList.TotalPageCount < 2) return "";

            StringBuilder sb = new StringBuilder(75);
            ViewContext viewContext = htmlHelper.ViewContext;
            RouteValueDictionary rvd = new RouteValueDictionary();

            foreach (KeyValuePair<string, object> item in viewContext.RouteData.Values)
            {
                rvd.Add(item.Key, item.Value);
            }

            UrlHelper urlHelper = new UrlHelper(viewContext.RequestContext);

            rvd.Remove("controller");
            rvd.Remove("action");
            rvd.Remove("id");
            rvd.Remove("dataFormat");

            if (values != null)
            {
                RouteValueDictionary rvd2 = new RouteValueDictionary(values);

                foreach (KeyValuePair<string, object> item in rvd2)
                {
                    rvd[item.Key] = item.Value;
                }
            }

            sb.Append("<div class=\"pager\">");

            if (pageOfAList.PageIndex < pageOfAList.TotalPageCount - 1 || alwaysShowPreviousAndNext)
            {
                rvd["pageNumber"] = pageOfAList.PageIndex + 2;

                sb.AppendFormat("<a href=\"{1}{2}\" class=\"next\">{0}</a>", nextText,
                                urlHelper.RouteUrl(routeName, rvd),
                                viewContext.HttpContext.Request.QueryString.ToQueryString());
            }

            if (pageOfAList.PageIndex > 0 || alwaysShowPreviousAndNext)
            {
                rvd["pageNumber"] = pageOfAList.PageIndex;

                sb.AppendFormat("<a href=\"{1}{2}\" class=\"previous\">{0}</a>", previousText,
                                urlHelper.RouteUrl(routeName, rvd),
                                viewContext.HttpContext.Request.QueryString.ToQueryString());
            }

            sb.Append("</div>");

            return sb.ToString();
        }

        public static string PostArchiveListPager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize)
        {
            return PostArchiveListPager(htmlHelper, pageOfAList, localize, null, localize("NewerPager", "&laquo; Newer"), localize("OlderPager","Older &raquo;"), false);
        }

        public static string PostArchiveListPager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, Func<string, string, string> localize, object values, string previousText, string nextText, bool alwaysShowPreviousAndNext)
        {
            return SimpleArchivePager(htmlHelper, pageOfAList, "PostsByArchive", values, previousText, nextText, alwaysShowPreviousAndNext);
        }

        public static string SimpleArchivePager<T>(this HtmlHelper htmlHelper, IPageOfList<T> pageOfAList, string routeName, object values, string previousText, string nextText, bool alwaysShowPreviousAndNext)
        {
            if (pageOfAList.TotalPageCount < 2) return "";

            StringBuilder sb = new StringBuilder(50);
            ViewContext viewContext = htmlHelper.ViewContext;
            RouteValueDictionary rvd = new RouteValueDictionary();

            foreach (KeyValuePair<string, object> item in viewContext.RouteData.Values)
            {
                rvd.Add(item.Key, item.Value);
            }

            UrlHelper urlHelper = new UrlHelper(viewContext.RequestContext);

            rvd.Remove("controller");
            rvd.Remove("action");
            rvd.Remove("id");

            if (values != null)
            {
                RouteValueDictionary rvd2 = new RouteValueDictionary(values);

                foreach (KeyValuePair<string, object> item in rvd2)
                {
                    rvd[item.Key] = item.Value;
                }
            }

            ArchiveData archiveData = new ArchiveData(rvd["archiveData"] as string);

            sb.Append("<div class=\"pager\">");

            if (pageOfAList.PageIndex < pageOfAList.TotalPageCount - 1 || alwaysShowPreviousAndNext)
            {
                archiveData.Page = pageOfAList.PageIndex + 2;
                rvd["archiveData"] = archiveData.ToString();

                sb.AppendFormat("<a href=\"{1}{2}\" class=\"next\">{0}</a>", nextText,
                                urlHelper.RouteUrl(routeName, rvd),
                                viewContext.HttpContext.Request.QueryString.ToQueryString());
            }

            if (pageOfAList.PageIndex > 0 || alwaysShowPreviousAndNext)
            {
                archiveData.Page = pageOfAList.PageIndex;
                rvd["archiveData"] = archiveData.ToString();

                sb.AppendFormat("<a href=\"{1}{2}\" class=\"previous\">{0}</a>", previousText,
                                urlHelper.RouteUrl(routeName, rvd),
                                viewContext.HttpContext.Request.QueryString.ToQueryString());
            }

            sb.Append("</div>");

            return sb.ToString();
        }

        #endregion

        #region Password

        public static string Password<TModel>(this HtmlHelper<TModel> htmlHelper, string name, object value, string labelInnerHtml) where TModel : OxiteModel
        {
            return Password(htmlHelper, name, value, labelInnerHtml, true);
        }

        public static string Password<TModel>(this HtmlHelper<TModel> htmlHelper, string name, object value, string labelInnerHtml, bool enabled) where TModel : OxiteModel
        {
            return Password(htmlHelper, name, value, labelInnerHtml, enabled, null);
        }

        public static string Password<TModel>(this HtmlHelper<TModel> htmlHelper, string name, object value, string labelInnerHtml, object htmlAttributes) where TModel : OxiteModel
        {
            return Password(htmlHelper, name, value, labelInnerHtml, true, htmlAttributes);
        }

        public static string Password<TModel>(this HtmlHelper<TModel> htmlHelper, string name, object value, string labelInnerHtml, bool enabled, object htmlAttributes) where TModel : OxiteModel
        {
            RouteValueDictionary attributes = new RouteValueDictionary(htmlAttributes);

            if (!enabled)
                attributes.Add("disabled", "disabled");

            return fieldWithLabel(htmlHelper, name, () => htmlHelper.Password(name, value, attributes), labelInnerHtml, enabled);
        }

        #endregion

        #region PingbackDiscovery

        public static string PingbackDiscovery(this HtmlHelper htmlHelper, Post post)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            return htmlHelper.HeadLink("pingback", urlHelper.AbsolutePath(urlHelper.Pingback(post)), "", "");
        }

        #endregion

        #region Published

        public static string Published<TModel>(this HtmlHelper<TModel> htmlHelper) where TModel : OxiteModelItem<Post>
        {
            return htmlHelper.Published(htmlHelper.ViewData.Model.Item);
        }

        public static string Published<TModel>(this HtmlHelper<TModel> htmlHelper, Post post) where TModel : OxiteModel
        {
            OxiteModel model = htmlHelper.ViewData.Model;

            if (post.State == EntityState.Removed)
                return model.Localize("Removed");

            if (post.Published.HasValue)
                return ConvertToLocalTime(htmlHelper, post.Published.Value, htmlHelper.ViewData.Model).ToLongDateString();

            return model.Localize("Draft");
        }

        #endregion

        #region RenderCssFile

        public static void RenderCssFile<TModel>(this HtmlHelper<TModel> htmlHelper, string path) where TModel : OxiteModel
        {
            htmlHelper.RenderCssFile(path, null);
        }

        public static void RenderCssFile<TModel>(this HtmlHelper<TModel> htmlHelper, string path, string releasePath) where TModel : OxiteModel
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

#if DEBUG
#else
            if (!string.IsNullOrEmpty(releasePath))
                path = releasePath;
#endif

            if (!(path.StartsWith("http://") || path.StartsWith("https://")))
            {
                UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

                path = !path.StartsWith("/")
                    ? urlHelper.CssPath(path, htmlHelper.ViewData.Model)
                    : urlHelper.AppPath(path);
            }

            htmlHelper.ViewContext.HttpContext.Response.Write(
                htmlHelper.HeadLink("stylesheet", path, "text/css", "")
                );
        }

        #endregion

        #region RenderFavIcon

        public static void RenderFavIcon<TModel>(this HtmlHelper<TModel> htmlHelper) where TModel : OxiteModel
        {
            OxiteModel model = htmlHelper.ViewData.Model;

            if (!string.IsNullOrEmpty(model.Site.FavIconUrl))
            {
                UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

                htmlHelper.ViewContext.HttpContext.Response.Write(htmlHelper.HeadLink("shortcut icon", urlHelper.AppPath(model.Site.FavIconUrl), null, null));
            }
        }

        #endregion

        #region RenderFeedDiscovery

        public static void RenderFeedDiscoveryRss(this HtmlHelper htmlHelper, string title, string url)
        {
            htmlHelper.RenderFeedDiscovery(title, url, "application/rss+xml");
        }

        public static void RenderFeedDiscoveryAtom(this HtmlHelper htmlHelper, string title, string url)
        {
            htmlHelper.RenderFeedDiscovery(title, url, "application/atom+xml");
        }

        public static void RenderFeedDiscovery(this HtmlHelper htmlHelper, string title, string url, string type)
        {
            htmlHelper.ViewContext.HttpContext.Response.Write(
                htmlHelper.HeadLink("alternate", url, type, title)
                );
        }

        #endregion

        #region RenderLiveWriterManifest

        public static void RenderLiveWriterManifest(this HtmlHelper htmlHelper)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            htmlHelper.RenderLiveWriterManifest(urlHelper.AppPath("~/LiveWriterManifest.xml"));
        }

        public static void RenderLiveWriterManifest(this HtmlHelper htmlHelper, string path)
        {
            htmlHelper.ViewContext.HttpContext.Response.Write(
                htmlHelper.HeadLink(
                    "wlwmanifest",
                    path,
                    "application/wlwmanifest+xml",
                    ""
                    )
                );
        }

        #endregion

        #region RenderOpenSearch

        public static void RenderOpenSearch<TModel>(this HtmlHelper<TModel> htmlHelper) where TModel : OxiteModel
        {
            OxiteModel model = htmlHelper.ViewData.Model;

            if (model.Site.IncludeOpenSearch)
            {
                UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

                htmlHelper.ViewContext.HttpContext.Response.Write(htmlHelper.HeadLink("search", urlHelper.AbsolutePath(urlHelper.OpenSearch()), "application/opensearchdescription+xml", string.Format(model.Localize("SearchFormat","{0} Search"), model.Site.DisplayName)));
            }
        }

        #endregion

        #region RenderRsd

        public static void RenderRsd(this HtmlHelper htmlHelper)
        {
            htmlHelper.RenderRsd((Area)null);
        }

        public static void RenderRsd(this HtmlHelper htmlHelper, Area area)
        {
            if (area != null)
            {
                htmlHelper.RenderRsd(area.Name);
            }
            else
            {
                htmlHelper.RenderRsd((string)null);
            }
        }

        public static void RenderRsd(this HtmlHelper htmlHelper, string areaName)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            htmlHelper.ViewContext.HttpContext.Response.Write(
                htmlHelper.HeadLink(
                    "EditURI",
                    urlHelper.AbsolutePath(urlHelper.Rsd(areaName)),
                    "application/rsd+xml",
                    "RSD"
                    )
                );
        }

        #endregion

        #region RenderScriptTag

        public static void RenderScriptTag<TModel>(this HtmlHelper<TModel> htmlHelper, string path) where TModel : OxiteModel
        {
            htmlHelper.RenderScriptTag(path, null);
        }

        public static void RenderScriptTag<TModel>(this HtmlHelper<TModel> htmlHelper, string path, string releasePath) where TModel : OxiteModel
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

#if DEBUG
#else
            if (!string.IsNullOrEmpty(releasePath))
                path = releasePath;
#endif

            if (!(path.StartsWith("http://") || path.StartsWith("https://")))
            {
                UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

                path = !path.StartsWith("/")
                    ? urlHelper.ScriptPath(path, htmlHelper.ViewData.Model)
                    : urlHelper.AppPath(path);
            }

            htmlHelper.ViewContext.HttpContext.Response.Write(htmlHelper.ScriptBlock("text/javascript", path));
        }

        #endregion

        #region RenderScriptVariable

        public static void RenderScriptVariable(this HtmlHelper htmlHelper, string name, object value)
        {
            const string scriptVariableFormat = "window.{0} = {1};";
            string script;

            if (value != null)
            {
                DataContractJsonSerializer dcjs = new DataContractJsonSerializer(value.GetType());

                using (MemoryStream ms = new MemoryStream())
                {
                    dcjs.WriteObject(ms, value);

                    script = string.Format(scriptVariableFormat, name, Encoding.Default.GetString(ms.ToArray()));

                    ms.Close();
                }
            }
            else
            {
                script = string.Format(scriptVariableFormat, name, "null");
            }

            htmlHelper.ViewContext.HttpContext.Response.Write(script);
        }

        #endregion

        #region ScriptBlock

        public static string ScriptBlock(this HtmlHelper htmlHelper, string type, string src)
        {
            return htmlHelper.ScriptBlock(type, src, null);
        }

        public static string ScriptBlock(this HtmlHelper htmlHelper, string type, string src, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("script");

            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (!string.IsNullOrEmpty(type))
            {
                tagBuilder.MergeAttribute("type", type);
            }
            if (!string.IsNullOrEmpty(src))
            {
                tagBuilder.MergeAttribute("src", src);
            }

            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region SkinImage

        public static string SkinImage<TModel>(this HtmlHelper<TModel> htmlHelper, string src, string alt, object htmlAttributes) where TModel : OxiteModel
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            src = urlHelper.CssPath(src, htmlHelper.ViewData.Model);

            return htmlHelper.Image(src, alt, new RouteValueDictionary(htmlAttributes));
        }

        #endregion

        #region TextArea

        public static string TextArea<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<TModel, string> getValue, int rows, int columns, string labelInnerHtml) where TModel : OxiteModel
        {
            return TextArea(htmlHelper, name, getValue, rows, columns, labelInnerHtml, true);
        }

        public static string TextArea<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<TModel, string> getValue, int rows, int columns, string labelInnerHtml, bool enabled) where TModel : OxiteModel
        {
            return TextArea(htmlHelper, name, getValue, rows, columns, labelInnerHtml, enabled, null);
        }

        public static string TextArea<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<TModel, string> getValue, int rows, int columns, string labelInnerHtml, object htmlAttributes) where TModel : OxiteModel
        {
            return TextArea(htmlHelper, name, getValue, rows, columns, labelInnerHtml, true, htmlAttributes);
        }

        public static string TextArea<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<TModel, string> getValue, int rows, int columns, string labelInnerHtml, bool enabled, object htmlAttributes) where TModel : OxiteModel
        {
            string value = htmlHelper.ViewContext.HttpContext.Request.Form[name] ?? getValue(htmlHelper.ViewData.Model) ?? "";

            RouteValueDictionary attributes = new RouteValueDictionary(htmlAttributes);

            if (!enabled)
                attributes.Add("disabled", "disabled");

            return fieldWithLabel(htmlHelper, name, () => htmlHelper.TextArea(name, value, rows, columns, attributes), labelInnerHtml, enabled);
        }

        #endregion

        #region TextBox

        public static string TextBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<TModel, object> getValue, string labelInnerHtml) where TModel : OxiteModel
        {
            return TextBox(htmlHelper, name, getValue, labelInnerHtml, true);
        }

        public static string TextBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<TModel, object> getValue, string labelInnerHtml, bool enabled) where TModel : OxiteModel
        {
            return TextBox(htmlHelper, name, getValue, labelInnerHtml, enabled, null);
        }

        public static string TextBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<TModel, object> getValue, string labelInnerHtml, object htmlAttributes) where TModel : OxiteModel
        {
            return TextBox(htmlHelper, name, getValue, labelInnerHtml, true, htmlAttributes);
        }

        public static string TextBox<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<TModel, object> getValue, string labelInnerHtml, bool enabled, object htmlAttributes) where TModel : OxiteModel
        {
            object value = htmlHelper.ViewContext.HttpContext.Request.Form[name] ?? getValue(htmlHelper.ViewData.Model);

            RouteValueDictionary attributes = new RouteValueDictionary(htmlAttributes);

            if (!enabled)
                attributes.Add("disabled", "disabled");

            return fieldWithLabel(htmlHelper, name, () => htmlHelper.TextBox(name, value, attributes), labelInnerHtml, enabled);
        }

        #endregion

        #region TextBoxWithValidation

        public static string TextBoxWithValidation<TModel>(this HtmlHelper<TModel> htmlHelper, string validationKey, string name) where TModel : OxiteModel
        {
            return TextBoxWithValidation(htmlHelper, validationKey, name, null);
        }

        public static string TextBoxWithValidation<TModel>(this HtmlHelper<TModel> htmlHelper, string validationKey, string name, object value) where TModel : OxiteModel
        {
            return TextBoxWithValidation(htmlHelper, validationKey, name, value, null);
        }

        public static string TextBoxWithValidation<TModel>(this HtmlHelper<TModel> htmlHelper, string validationKey, string name, object value, object htmlAttributes) where TModel : OxiteModel
        {
            OxiteModel model = htmlHelper.ViewData.Model;

            return htmlHelper.TextBox(name, value, htmlAttributes) + htmlHelper.ValidationMessage(validationKey, model.Localize(validationKey));
        }

        public static string TextBoxWithValidation<TModel>(this HtmlHelper<TModel> htmlHelper, string validationKey, string name, object value, object htmlAttributes, bool isEnabled) where TModel : OxiteModel
        {
            RouteValueDictionary htmlAttributeDictionary = new RouteValueDictionary(htmlAttributes);

            if (!isEnabled)
            {
                htmlAttributeDictionary["disabled"] = "disabled";

                StringBuilder inputItemBuilder = new StringBuilder();

                inputItemBuilder.Append(TextBoxWithValidation(htmlHelper, validationKey, string.Format("{0}_view", name), value, htmlAttributeDictionary));
                inputItemBuilder.Append(htmlHelper.Hidden(name, value));

                return inputItemBuilder.ToString();
            }

            return htmlHelper.TextBox(name, value, htmlAttributeDictionary);
        }

        #endregion

        #region TrackbackBlock

        public static string TrackbackBlock<TModel>(this HtmlHelper<TModel> htmlHelper) where TModel : OxiteModel
        {
            OxiteModelItem<Post> model = htmlHelper.ViewData.Model as OxiteModelItem<Post>;

            if (model != null)
            {
                Post post = ((OxiteModelItem<Post>)model).Item;

                if (post != null)
                {
                    UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

                    return string.Format("<!--<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:trackback=\"http://madskills.com/public/xml/rss/module/trackback/\"><rdf:Description rdf:about=\"{2}\" dc:identifier=\"{2}\" dc:title=\"{0}\" trackback:ping=\"{1}\" /></rdf:RDF>-->", htmlHelper.Encode(post.Title), urlHelper.AbsolutePath(urlHelper.Trackback(post)), urlHelper.AbsolutePath(urlHelper.Post(post)));
                }
            }

            return "";
        }

        #endregion

        #region UnorderedList

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items,
                                              Func<T, string> generateContent)
        {
            return UnorderedList(htmlHelper, items, (t, i) => generateContent(t));
        }

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items,
                                              Func<T, string> generateContent, string cssClass)
        {
            return UnorderedList(htmlHelper, items, (t, i) => generateContent(t), cssClass, null, null);
        }

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items,
                                              Func<T, int, string> generateContent)
        {
            return UnorderedList(htmlHelper, items, generateContent, null);
        }

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items,
                                              Func<T, int, string> generateContent, string cssClass)
        {
            return UnorderedList(htmlHelper, items, generateContent, cssClass, null, null);
        }

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items,
                                              Func<T, int, string> generateContent, string cssClass, string itemCssClass,
                                              string alternatingItemCssClass)
        {
            if (items == null || items.Count() == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder(100);
            int counter = 0;

            sb.Append("<ul");
            if (!string.IsNullOrEmpty(cssClass))
            {
                sb.AppendFormat(" class=\"{0}\"", cssClass);
            }
            sb.Append(">");
            foreach (T item in items)
            {
                StringBuilder sbClass = new StringBuilder(40);

                if (counter == 0)
                {
                    sbClass.Append("first ");
                }
                if (item.Equals(items.Last()))
                {
                    sbClass.Append("last ");
                }

                if (counter % 2 == 0 && !string.IsNullOrEmpty(itemCssClass))
                {
                    sbClass.AppendFormat("{0} ", itemCssClass);
                }
                else if (counter % 2 != 0 && !string.IsNullOrEmpty(alternatingItemCssClass))
                {
                    sbClass.AppendFormat("{0} ", alternatingItemCssClass);
                }

                sb.Append("<li");
                if (sbClass.Length > 0)
                {
                    sb.AppendFormat(" class=\"{0}\"", sbClass.Remove(sbClass.Length - 1, 1));
                }
                sb.AppendFormat(">{0}</li>", generateContent(item, counter));

                counter++;
            }
            sb.Append("</ul>");

            return sb.ToString();
        }

        #endregion

        #region Private Methods

        private static string fieldWithLabel<TModel>(this HtmlHelper<TModel> htmlHelper, string name, Func<string> renderText, string labelInnerHtml, bool enabled) where TModel : OxiteModel
        {
            StringBuilder output = new StringBuilder(200);

            if (!string.IsNullOrEmpty(labelInnerHtml))
            {
                TagBuilder builder = new TagBuilder("label");

                builder.Attributes["for"] = name;
                builder.InnerHtml = labelInnerHtml;

                output.Append(builder.ToString());
                output.Append(" ");
            }

            output.Append(renderText());

            return output.ToString();
        }

        #endregion
    }
}
