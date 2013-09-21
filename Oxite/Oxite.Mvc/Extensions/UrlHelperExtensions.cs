//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Mvc;
using Oxite.Model;
using Oxite.Model.Extensions;
using Oxite.Mvc.Infrastructure;
using Oxite.Routing;
using System.Web;

namespace Oxite.Mvc.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string Oxite(this UrlHelper urlHelper)
        {
            return "http://oxite.net";
        }

        public static string AbsolutePath(this UrlHelper urlHelper, string relativeUrl)
        {
            Uri url = urlHelper.RequestContext.HttpContext.Request.Url;
            UriBuilder uriBuilder = new UriBuilder(url.Scheme, url.Host, url.Port) { Path = relativeUrl };
            //string appPath = urlHelper.RequestContext.HttpContext.Request.ApplicationPath;

            //if (!uriBuilder.Path.StartsWith(appPath))
            //    uriBuilder.Path = appPath + uriBuilder.Path;

            return uriBuilder.Uri.ToString();
        }

        public static string AppPath(this UrlHelper urlHelper, string relativePath)
        {
            if (relativePath == null) return null;

            return VirtualPathUtility.ToAbsolute(relativePath, urlHelper.RequestContext.HttpContext.Request.ApplicationPath);
        }

        public static string CssPath(this UrlHelper urlHelper, ViewModels.OxiteModel model)
        {
            return CssPath(urlHelper, "", model);
        }

        public static string CssPath(this UrlHelper urlHelper, string relativePath, ViewModels.OxiteModel model)
        {
            if (!string.IsNullOrEmpty(relativePath) && !relativePath.StartsWith("/"))
            {
                relativePath = "/" + relativePath;
            }

            return string.Format(urlHelper.AppPath(model.Site.CssPath), model.SkinName) + relativePath;
        }

        public static string ScriptPath(this UrlHelper urlHelper, ViewModels.OxiteModel model)
        {
            return ScriptPath(urlHelper, "", model);
        }

        public static string ScriptPath(this UrlHelper urlHelper, string relativePath, ViewModels.OxiteModel model)
        {
            if (!string.IsNullOrEmpty(relativePath) && !relativePath.StartsWith("/"))
            {
                relativePath = "/" + relativePath;
            }

            return string.Format(urlHelper.AppPath(model.Site.ScriptsPath), model.SkinName) + relativePath;
        }

        public static string Container(this UrlHelper urlHelper, INamedEntity entity)
        {
            PostVisitor vistor = new PostVisitor(urlHelper);

            return vistor.Visit<string>(entity);
        }

        public static string Container(this UrlHelper urlHelper, INamedEntity entity, string dataFormat)
        {
            PostVisitor vistor = new PostVisitor(urlHelper);

            return vistor.Visit<string>(entity, dataFormat);
        }

        public static string ContainerComments(this UrlHelper urlHelper, INamedEntity entity, string dataFormat)
        {
            CommentVisitor vistor = new CommentVisitor(urlHelper);

            return vistor.Visit<string>(entity, dataFormat);
        }

        public static string Home(this UrlHelper urlHelper)
        {
            return urlHelper.Posts();
        }

        public static string Posts(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Posts");
        }

        public static string PostsWithDrafts(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("PostsWithDrafts");
        }

        public static string Posts(this UrlHelper urlHelper, string dataFormat)
        {
            return urlHelper.RouteUrl("Posts", new { dataFormat });
        }

        public static string Posts(this UrlHelper urlHelper, Area area)
        {
            return area.GetUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static string Posts(this UrlHelper urlHelper, Area area, string dataFormat)
        {
            return urlHelper.RouteUrl("PostsByArea", new { areaName = area.Name, dataFormat });
        }

        public static string Tags(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Tags");
        }

        public static string Posts(this UrlHelper urlHelper, Tag tag)
        {
            return tag.GetUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static string Posts(this UrlHelper urlHelper, Tag tag, string dataFormat)
        {
            return urlHelper.RouteUrl("PostsByTag", new { tagName = tag.Name, dataFormat });
        }

        public static string Posts(this UrlHelper urlHelper, int year)
        {
            return urlHelper.RouteUrl("PostsByArchive", new { archiveData = string.Format("{0}", year) });
        }

        public static string Posts(this UrlHelper urlHelper, int year, int month, int day)
        {
            return urlHelper.RouteUrl("PostsByArchive", new { archiveData = string.Format("{0}/{1}/{2}", year, month, day) });
        }

        public static string Posts(this UrlHelper urlHelper, int year, int month)
        {
            return urlHelper.RouteUrl("PostsByArchive", new { archiveData = string.Format("{0}/{1}", year, month) });
        }

        public static string Post(this UrlHelper urlHelper, Post post)
        {
            return post.GetUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static string AddPost(this UrlHelper urlHelper, Area area)
        {
            return area != null
                ? urlHelper.RouteUrl("AddPostToArea", new { areaName = area.Name })
                : urlHelper.RouteUrl("AddPostToSite");
        }

        public static string EditPost(this UrlHelper urlHelper, Post post)
        {
            return urlHelper.RouteUrl("EditPost", new { areaName = post.Area.Name, slug = post.Slug });
        }

        public static string RemovePost(this UrlHelper urlHelper, Post post)
        {
            return urlHelper.RouteUrl("RemovePost", new { areaName = post.Area.Name, slug = post.Slug });
        }

        public static string Page(this UrlHelper urlHelper, Page page)
        {
            if (page != null)
                return urlHelper.Page(page.Path.Substring(1));

            return urlHelper.Home();
        }

        public static string Page(this UrlHelper urlHelper, string pagePath)
        {
            return urlHelper.RouteUrl("Page", new { pagePath });
        }

        public static string AddPage(this UrlHelper urlHelper, Page page)
        {
            return urlHelper.AddPage(page.Path.Substring(1));
        }

        public static string AddPage(this UrlHelper urlHelper)
        {
            return urlHelper.AddPage(urlHelper.RequestContext.RouteData.Values["pagePath"] as string);
        }

        public static string AddPage(this UrlHelper urlHelper, string pagePath)
        {
            if (string.IsNullOrEmpty(pagePath))
                pagePath = urlHelper.RequestContext.RouteData.Values["pagePath"] as string;

            if (!string.IsNullOrEmpty(pagePath))
            {
               if (pagePath.EndsWith("/" + PageMode.Add))
                   pagePath = pagePath.Substring(0, pagePath.Length - ("/" + PageMode.Add).Length);
               else if (pagePath.EndsWith("/" + PageMode.Edit))
                   pagePath = pagePath.Substring(0, pagePath.Length - ("/" + PageMode.Edit).Length);
               else if (pagePath.EndsWith("/" + PageMode.Remove))
                   pagePath = pagePath.Substring(0, pagePath.Length - ("/" + PageMode.Remove).Length);
            }

            return !string.IsNullOrEmpty(pagePath)
                ? urlHelper.RouteUrl("AddPageToPage", new { pagePath = pagePath + "/Add" })
                : urlHelper.RouteUrl("AddPageToSite");
        }

        public static string EditPage(this UrlHelper urlHelper, Page page)
        {
            return urlHelper.EditPage(page.Path.Substring(1));
        }

        public static string EditPage(this UrlHelper urlHelper, string pagePath)
        {
            return urlHelper.RouteUrl("EditPage", new { pagePath = pagePath + "/Edit" });
        }

        public static string RemovePage(this UrlHelper urlHelper, Page page)
        {
            return urlHelper.RemovePage(page.Path.Substring(1));
        }

        public static string RemovePage(this UrlHelper urlHelper, string pagePath)
        {
            return urlHelper.RouteUrl("RemovePage", new { pagePath = pagePath + "/Remove" });
        }

        public static string Comments(this UrlHelper urlHelper, string dataFormat)
        {
            return urlHelper.RouteUrl("Comments", new { dataFormat });
        }

        public static string Comments(this UrlHelper urlHelper, Post post, string dataFormat)
        {
            return urlHelper.RouteUrl("CommentsByPost", new { areaName = post.Area.Name, slug = post.Slug, dataFormat });
        }

        public static string Comments(this UrlHelper urlHelper, Area area, string dataFormat)
        {
            return urlHelper.RouteUrl("CommentsByArea", new { areaName = area.Name, dataFormat });
        }

        public static string Comments(this UrlHelper urlHelper, Tag tag, string dataFormat)
        {
            return urlHelper.RouteUrl("CommentsByTag", new { tagName = tag.Name, dataFormat });
        }

        public static string Comment(this UrlHelper urlHelper, Post post, Comment comment)
        {
            return comment.GetUrl(post, urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static string CommentPending(this UrlHelper urlHelper, Post post, Comment comment)
        {
            return comment.GetPendingUrl(post, urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static string ManageComments(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("AllComments");
        }

        public static string ManageComment(this UrlHelper urlHelper, Comment comment)
        {
            return urlHelper.RouteUrl("AllCommentsPermalink", new { comment = comment.GetSlug() });
        }

        public static string AddCommentToPost(this UrlHelper urlHelper, Post post)
        {
            return urlHelper.RouteUrl("AddCommentToPost", new { areaName = post.Area.Name, slug = post.Slug });
        }

        public static string RemoveComment(this UrlHelper urlHelper, Post post)
        {
            return urlHelper.RouteUrl("RemoveComment", new { areaName = post.Area.Name, slug = post.Slug });
        }

        public static string ApproveComment(this UrlHelper urlHelper, Post post)
        {
            return urlHelper.RouteUrl("ApproveComment", new { areaName = post.Area.Name, slug = post.Slug });
        }

        public static string Search(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("PostsBySearch");
        }

        public static string Search(this UrlHelper urlHelper, string term)
        {
            return urlHelper.RouteUrl("PostsBySearch", new { term });
        }

        public static string Search(this UrlHelper urlHelper, string dataFormat, string term)
        {
            return urlHelper.RouteUrl("PostsBySearch", new { dataFormat = dataFormat, term = term });
        }

        public static string OpenSearch(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("OpenSearch");
        }

        public static string OpenSearchOSDX(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("OpenSearchOSDX");
        }

        public static string Rsd(this UrlHelper urlHelper)
        {
            return urlHelper.Rsd((Area)null);
        }

        public static string Rsd(this UrlHelper urlHelper, Area area)
        {
            if (area != null)
                return urlHelper.Rsd(area.Name);

            return urlHelper.Rsd((string)null);
        }

        public static string Rsd(this UrlHelper urlHelper, string areaName)
        {
            if (!string.IsNullOrEmpty(areaName))
                return urlHelper.RouteUrl("AreaRsd", new {areaName });

            return urlHelper.RouteUrl("Rsd");
        }

        public static string Pingback(this UrlHelper urlHelper, Post post)
        {
            return urlHelper.AppPath("~/Pingback.svc");
        }

        public static string Trackback(this UrlHelper urlHelper, Post post)
        {
            return urlHelper.RouteUrl("Trackback", new { postID = post.ID.ToString("N") });
        }

        public static string SiteMapIndex(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("SiteMapIndex");
        }

        public static string SiteMap(this UrlHelper urlHelper, int year, int month)
        {
            return urlHelper.RouteUrl("SiteMap", new { year, month });
        }

        public static string MetaWeblogApi(this UrlHelper urlHelper)
        {
            return urlHelper.AppPath("~/MetaWeblog.svc");
        }

        public static string SignIn(this UrlHelper urlHelper, string returnUrl)
        {
            return urlHelper.RouteUrl("SignIn", new { ReturnUrl = returnUrl });
        }

        public static string SignOut(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("SignOut");
        }

        public static string ComputeHash(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("ComputeHash");
        }

        public static string Site(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Site");
        }

        public static string ManageSite(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("ManageSite");
        }

        public static string ManageAreas(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("ManageAreas");
        }

        public static string ManageUsers(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("ManageUsers");
        }

        public static string UserChangePassword(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("UserChangePassword");
        }

        public static string Plugins(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Plugins");
        }

        public static string Plugin(this UrlHelper urlHelper, IPlugin plugin)
        {
            return urlHelper.RouteUrl("Plugin", new { pluginID = plugin.ID.ToString("N") });
        }

        public static string BlogML(this UrlHelper urlHelper, Area area)
        {
            return urlHelper.RouteUrl("BlogML", new { areaName = area.Name });
        }

        public static string AreaFind(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("AreaFind");
        }

        public static string AreaAdd(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("AreaAdd");
        }

        public static string AreaEdit(this UrlHelper urlHelper, Area area)
        {
            return urlHelper.RouteUrl("AreaEdit", new { areaID = area.ID });
        }

        public static string Admin(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Admin");
        }

        public static string AdminUser(this UrlHelper urlHelper, User user)
        {
            return user.GetAdminUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static string AdminUserChangePassword(this UrlHelper urlHelper, User user)
        {
            return user.GetAdminChangePasswordUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static bool IsHome(this UrlHelper urlHelper)
        {
            string controller = urlHelper.RequestContext.RouteData.Values["controller"] as string;
            string action = urlHelper.RequestContext.RouteData.Values["action"] as string;

            return
                !string.IsNullOrEmpty(controller) &&
                string.Compare(controller, "Post", true) == 0 &&
                !string.IsNullOrEmpty(action) &&
                string.Compare(action, "List", true) == 0;
        }

        public static bool IsPagePath(this UrlHelper urlHelper, string pagePath)
        {
            string controller = urlHelper.RequestContext.RouteData.Values["controller"] as string;
            string pagePathValue = urlHelper.RequestContext.RouteData.Values["pagePath"] as string;

            return
                !string.IsNullOrEmpty(controller) &&
                string.Compare(controller, "Page", true) == 0 &&
                !string.IsNullOrEmpty(pagePath) &&
                string.Compare(pagePathValue, pagePath, true) == 0;
        }

        public static bool IsAdmin(this UrlHelper urlHelper)
        {
            string controller = urlHelper.RequestContext.RouteData.Values["controller"] as string;

            //TODO: (erikpo) Once the admin is refactored, change this to look at the url to determine if we're in the admin or not
            return
                !string.IsNullOrEmpty(controller) &&
                string.Compare(controller, "Admin", true) == 0;
        }

        //public static string User(this UrlHelper urlHelper, IUser user)
        //{
        //    return user.GetUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        //}

        //public static string Area(this UrlHelper urlHelper, IArea area)
        //{
        //    return area.GetUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        //}

        //public static string Tag(this UrlHelper urlHelper, ITag tag)
        //{
        //    return tag.GetUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        //}

        //public static string Post(this UrlHelper urlHelper, IPost post)
        //{
        //    return post.GetAreaPostUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        //}

        //public static string Page(this UrlHelper urlHelper, IPost post)
        //{
        //    return post.GetPageUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        //}

        //public static string Comment(this UrlHelper urlHelper, IComment comment)
        //{
        //    return comment.GetUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        //}
    }
}
