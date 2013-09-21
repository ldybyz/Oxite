//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Linq;
using System.Web.Routing;
using Oxite.Infrastructure;
using Oxite.Model;
using Oxite.Services;

namespace Oxite.Routing
{
    public class OxiteRoutes : IRegisterRoutes
    {
        private readonly RouteCollection routes;
        private readonly AppSettingsHelper appSettings;
        private readonly Site site;
        private readonly IAreaService areaService;
        private readonly Type routeHandlerType;

        public OxiteRoutes(RouteCollection routes, AppSettingsHelper appSettings, Site site, IAreaService areaService, Type handlerType)
        {
            this.routes = routes;
            this.appSettings = appSettings;
            this.site = site;
            this.areaService = areaService;
            routeHandlerType = handlerType;
        }

        public virtual void RegisterRoutes()
        {
            string[] areas = areaService.GetAreas().Select(a => a.Name).ToArray();
            string areasConstraint =
                areas != null && areas.Length > 0
                    ? areas.Length > 1
                        ? string.Format("({0})", string.Join("|", areas)) : areas[0]
                    : "";

            routes.Clear();

            routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));

            RegisterRoutes(routes, areasConstraint, appSettings.GetStringArray("ControllerNamespaces", ",", null));
        }

        protected virtual void RegisterRoutes(RouteCollection existingRoutes, string areasConstraint, string[] controllerNamespaces)
        {
            RegisterAdminRoutes(areasConstraint, controllerNamespaces);

            RegisterHomeRoutes(controllerNamespaces);

            RegisterSearchRoutes(controllerNamespaces);

            RegisterAreaRoutes(areasConstraint, controllerNamespaces);

            RegisterUserRoutes(controllerNamespaces);

            RegisterTagRoutes(controllerNamespaces);

            RegisterArchiveRoutes(controllerNamespaces);

            RegisterTrackbackRoutes(controllerNamespaces);

            RegisterSEORoutes(controllerNamespaces);

            //INFO: (erikpo) This route must remain last
            RegisterPageRoutes(controllerNamespaces);
        }

        protected virtual void RegisterPageRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Page",
                "{*pagePath}",
                new { controller = "Page", action = "Item" },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageAdd",
                "add",
                new { controller = "Page", action = "Index", pagePath = string.Empty },
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterArchiveRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "PostsByArchive",
                "Archive/{*archiveData}",
                new { controller = "Post", action = "ListByArchive" },
                new { archiveData = new IsArchiveData() },
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterHomeRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Posts",
                "{dataFormat}",
                new { controller = "Post", action = "List", dataFormat = "" },
                new { dataFormat = "(|RSS|ATOM)" },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfPosts",
                "page{pageNumber}",
                new { controller = "Post", action = "List" },
                new { pageNumber = new IsInt() },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "Comments",
                "Comments/{dataFormat}",
                new { controller = "Comment", action = "List" },
                new { dataFormat = "(RSS|ATOM)" },
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterAreaRoutes(string areasConstraint, string[] controllerNamespaces)
        {
            AddRoute(
                "PostsByArea",
                "{areaName}/{dataFormat}",
                new { controller = "Post", action = "ListByArea", dataFormat = "" },
                new { areaName = areasConstraint, dataFormat = "(|RSS|ATOM)" },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfPostsByArea",
                "{areaName}/page{pageNumber}",
                new { controller = "Post", action = "ListByArea" },
                new { areaName = areasConstraint, pageNumber = new IsInt() },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "CommentsByArea",
                "{areaName}/Comments/{dataFormat}",
                new { controller = "Comment", action = "ListByArea" },
                new { areaName = areasConstraint, dataFormat = "(RSS|ATOM)" },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PostsByAreaArchive",
                "{areaName}/Archive/{*archiveData}",
                new { controller = "Post", action = "ListByArchive", archiveData = ArchiveData.DefaultString },
                new { areaName = areasConstraint, archiveData = new IsArchiveData() },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "Rsd",
                "RSD",
                new { controller = "Area", action = "Rsd" },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AreaRsd",
                "{areaName}/RSD",
                new { controller = "Area", action = "Rsd" },
                new { areaName = areasConstraint },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PostCommentPermalink",
                "{areaName}/{slug}#{comment}",
                null,
                new { routeDirection = new RouteDirectionConstraint(RouteDirection.UrlGeneration) },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PostCommentForm",
                "{areaName}/{slug}#comment",
                null,
                new { routeDirection = new RouteDirectionConstraint(RouteDirection.UrlGeneration) },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AddCommentToPostAsUser",
                "{areaName}/{slug}",
                new { controller = "Post", action = "Item", validateAntiForgeryToken = true },
                new { areaName = areasConstraint, httpMethod = new HttpMethodConstraint("POST"), authenticated = new IsAuthenticated() },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AddCommentToPost",
                "{areaName}/{slug}",
                new { controller = "Post", action = "Item" },
                new { areaName = areasConstraint, httpMethod = new HttpMethodConstraint("POST") },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "Post",
                "{areaName}/{slug}/{dataFormat}",
                new { controller = "Post", action = "Item", dataFormat = "" },
                new { areaName = areasConstraint, dataformat = "(|RSS|ATOM)" },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "CommentsByPost",
                "{areaName}/{slug}/Comments/{dataFormat}",
                new { controller = "Comment", action = "ListByPost" },
                new { areaName = areasConstraint, dataformat = "(RSS|ATOM)" },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "ComputeHash",
                "ComputeHash",
                new { controller = "Utility", action = "ComputeHash" },
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterSearchRoutes(string[] controllerNamespaces)
        {
            if (site.IncludeOpenSearch)
            {
                AddRoute(
                    "OpenSearch",
                    "OpenSearch.xml",
                    new { controller = "Utility", action = "OpenSearch" },
                    null,
                    new { Namespaces = controllerNamespaces }
                    );

                AddRoute(
                    "OpenSearchOSDX",
                    "OpenSearch.osdx",
                    new { controller = "Utility", action = "OpenSearchOSDX" },
                    null,
                    new { Namespaces = controllerNamespaces }
                    );
            }

            AddRoute(
                "PostsBySearch",
                "Search/{dataFormat}",
                new { controller = "Post", action = "ListBySearch", dataFormat = "" },
                new { dataformat = "(|RSS|ATOM)" },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfPostsBySearch",
                "Search/page{pageNumber}",
                new { controller = "Post", action = "ListBySearch" },
                new { pageNumber = new IsInt() },
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterUserRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "SignIn",
                "SignIn",
                new { controller = "User", action = "SignIn" },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SignOut",
                "SignOut",
                new { controller = "User", action = "SignOut" },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "UserChangePassword",
                "Admin/ChangePassword",
                new { controller = "User", action = "ChangePassword", validateAntiForgeryToken = true },
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterTrackbackRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Trackback",
                "{postID}/Trackback",
                new { controller = "Trackback", action = "Add" },
                new { postID = new IsGuid() },
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterTagRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Tags",
                "Tags",
                new { controller = "Tag", action = "Cloud" },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PostsByTag",
                "Tags/{tagName}/{dataFormat}",
                new { controller = "Post", action = "ListByTag", dataFormat = "" },
                new { dataFormat = "(|RSS|ATOM)" },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfPostsByTag",
                "Tags/{tagName}/page{pageNumber}",
                new { controller = "Post", action = "ListByTag" },
                new { pageNumber = new IsInt() },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "CommentsByTag",
                "Tags/{tagName}/Comments/{dataFormat}",
                new { controller = "Comment", action = "ListByTag" },
                new { dataFormat = "(RSS|ATOM)" },
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterSEORoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "RobotsTxt",
                "robots.txt",
                new { controller = "Utility", action = "RobotsTxt" },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SiteMapIndex",
                "SiteMap",
                new { controller = "Utility", action = "SiteMapIndex" },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SiteMap",
                "SiteMap/{year}/{month}",
                new { controller = "Utility", action = "SiteMap" },
                new
                {
                    year = new IsInt(DateTime.MinValue.Year, DateTime.MaxValue.Year),
                    month = new IsInt(DateTime.MinValue.Month, DateTime.MaxValue.Month)
                },
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterAdminRoutes(string areasConstraint, string[] controllerNamespaces)
        {
            AddRoute(
                "RemoveComment",
                "Admin/{areaName}/{slug}/RemoveComment",
                new { controller = "Comment", action = "Remove", validateAntiForgeryToken = true },
                new { areaName = areasConstraint, httpMethod = new HttpMethodConstraint("POST") },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "ApproveComment",
                "Admin/{areaName}/{slug}/Approve",
                new { controller = "Comment", action = "Approve", validateAntiForgeryToken = true },
                new { areaName = areasConstraint, httpMethod = new HttpMethodConstraint("POST") },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AddPostToSite",
                "Admin/AddPost",
                new { controller = "Post", action = "ItemAdd", validateAntiForgeryToken = true },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AddPostToArea",
                "Admin/{areaName}/Add",
                new { controller = "Post", action = "ItemAdd", validateAntiForgeryToken = true },
                new { areaName = areasConstraint },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "EditPost",
                "Admin/{areaName}/{slug}/Edit",
                new { controller = "Post", action = "ItemEdit", validateAntiForgeryToken = true },
                new { areaName = areasConstraint },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "RemovePost",
                "Admin/{areaName}/{slug}/Remove",
                new { controller = "Post", action = "Remove", validateAntiForgeryToken = true },
                new { areaName = areasConstraint, httpMethod = new HttpMethodConstraint("POST") },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "ManageSite",
                "Admin#manageSite",
                new { controller = "Site", action = "Dashboard" },
                new { routeDirection = new RouteDirectionConstraint(RouteDirection.UrlGeneration) },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "ManageAreas",
                "Admin#manageAreas",
                new { controller = "Site", action = "Dashboard" },
                new { routeDirection = new RouteDirectionConstraint(RouteDirection.UrlGeneration) },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "ManageUsers",
                "Admin#manageUsers",
                new { controller = "Site", action = "Dashboard" },
                new { routeDirection = new RouteDirectionConstraint(RouteDirection.UrlGeneration) },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "Site",
                "Admin/Setup",
                new { controller = "Site", action = "Item", validateAntiForgeryToken = true },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "Plugins",
                "Admin/Plugins",
                new { controller = "Plugin", action = "List" },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "Plugin",
                "Admin/Plugins/{pluginID}",
                new { controller = "Plugin", action = "Item" },
                new { pluginID = new IsGuid() },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "Admin",
                "Admin",
                new { controller = "Site", action = "Dashboard" },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PostsWithDrafts",
                "Admin/Posts",
                new { controller = "Post", action = "ListWithDrafts" },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfPostsWithDrafts",
                "Admin/Posts/page{pageNumber}",
                new { controller = "Post", action = "ListWithDrafts" },
                new { pageNumber = new IsInt() },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AllComments",
                "Admin/Comments",
                new { controller = "Comment", action = "ListForAdmin" },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfAllComments",
                "Admin/Comments/page{pageNumber}",
                new { controller = "Comment", action = "ListForAdmin" },
                new { pageNumber = new IsInt() },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AllCommentsPermalink",
                "Admin/Comments#{comment}",
                null,
                new { routeDirection = new RouteDirectionConstraint(RouteDirection.UrlGeneration) },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AreaFind",
                "Admin/Areas",
                new { controller = "Area", action = "Find", validateAntiForgeryToken = true },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AreaAdd",
                "Admin/Areas/Add",
                new { controller = "Area", action = "ItemEdit", validateAntiForgeryToken = true },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AreaEdit",
                "Admin/Areas/{areaID}/Edit",
                new { controller = "Area", action = "ItemEdit", validateAntiForgeryToken = true },
                new { areaID = new IsGuid() },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogML",
                "Admin/{areaName}/BlogML",
                new { controller = "Area", action = "BlogML", validateAntiForgeryToken = true },
                new { areaName = areasConstraint },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AddPageToSite",
                "Admin/AddPage",
                new { controller = "Page", action = "ItemAdd", validateAntiForgeryToken = true },
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AddPageToPage",
                "Admin/{*pagePath}",
                new { controller = "Page", action = "ItemAdd", validateAntiForgeryToken = true },
                new { pagePath = new IsPageMode(PageMode.Add) },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "EditPage",
                "Admin/{*pagePath}",
                new { controller = "Page", action = "ItemEdit", validateAntiForgeryToken = true },
                new { pagePath = new IsPageMode(PageMode.Edit) },
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "RemovePage",
                "Admin/{*pagePath}",
                new { controller = "Page", action = "Remove", validateAntiForgeryToken = true },
                new { pagePath = new IsPageMode(PageMode.Remove), httpMethod = new HttpMethodConstraint("POST") },
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void AddRoute(string name, string url, object defaults, object constraints, object dataTokens)
        {
            if (site.RouteUrlPrefix == null)
            {
                url = "oxite.aspx/" + url;
            }
            else if (site.RouteUrlPrefix != "")
            {
                url = site.RouteUrlPrefix + "/" + url;
            }

            routes.Add(
                name,
                new Route(url, CreateRouteHandler())
                {
                    Defaults = new RouteValueDictionary(defaults),
                    Constraints = new RouteValueDictionary(constraints),
                    DataTokens = new RouteValueDictionary(dataTokens)
                }
            );
        }

        protected virtual IRouteHandler CreateRouteHandler()
        {
            if (routeHandlerType != null)
                return (IRouteHandler)Activator.CreateInstance(routeHandlerType);

            return null;
        }
    }
}
