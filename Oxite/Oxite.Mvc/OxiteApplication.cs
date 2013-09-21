//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Oxite.BackgroundServices;
using Oxite.Infrastructure;
using Oxite.Model;
using Oxite.Mvc.ActionFilters;
using Oxite.Mvc.Controllers;
using Oxite.Mvc.Extensions;
using Oxite.Mvc.Infrastructure;
using Oxite.Mvc.ModelBinders;
using Oxite.Routing;
using Oxite.Services;

namespace Oxite.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class OxiteApplication : HttpApplication
    {
        public OxiteApplication()
        {
            BeginRequest += OxiteApplication_BeginRequest;
        }

        protected virtual void OnStart()
        {
            setupContiner();

            setupSite();

            registerRoutes();

            registerActionFilters();

            registerModelBinders();

            registerViewEngines();

            registerControllerFactory();

            launchBackgroundServices();
        }

        protected virtual void OnEnd()
        {
            BackgroundServicesExecutor backgroundServicesExecutor = (BackgroundServicesExecutor)Application["backgroundServicesExecutor"];

            if (backgroundServicesExecutor != null)
                backgroundServicesExecutor.Stop();
        }

        private void OxiteApplication_BeginRequest(object sender, EventArgs e)
        {
            Site site = getSite();

            if (site.ID == Guid.Empty)
            {
                string setupUrl = new UrlHelper(new RequestContext(new HttpContextWrapper(Context), new RouteData())).Site();

                if (!Request.RawUrl.EndsWith(setupUrl, StringComparison.OrdinalIgnoreCase) && Request.RawUrl.IndexOf("/skins", StringComparison.OrdinalIgnoreCase) == -1 && !Request.RawUrl.StartsWith("/Content", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect(setupUrl, true);
                }
            }
            
            if (site.ID != Guid.Empty && !hasSameHostAsRequest(site.Host))
            {
                Uri hostAlias = site.HostRedirects.Where(hasSameHostAsRequest).FirstOrDefault();

                if (hostAlias != null)
                {
                    Response.RedirectLocation = makeHostMatchRequest(site.Host).ToString();
                    Response.StatusCode = 301;
                    Response.End();
                }
            }
        }

        private void setupContiner()
        {
            Application.Add("container", new ContainerFactory().GetOxiteContainer());
        }

        private void setupSite()
        {
            IUnityContainer container = getContainer();
            AppSettingsHelper appSettings = container.Resolve<AppSettingsHelper>();
            Site site = container.Resolve<ISiteService>().GetSite(appSettings.GetString("SiteName", "Oxite"));

            if (site != null && site.ID != Guid.Empty)
            {
                container.RegisterInstance(site);
            }
        }

        private void registerRoutes()
        {
            getContainer().Resolve<IRegisterRoutes>().RegisterRoutes();
        }

        private void registerActionFilters()
        {
            IUnityContainer container = getContainer();
            IActionFilterRegistry registry = container.Resolve<ActionFilterRegistry>();

            registry.Clear();

            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(AntiForgeryActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(SiteInfoActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(UserActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(LocalizationActionFilter));
            registry.Add(Enumerable.Empty<IActionFilterCriteria>(), typeof(AntiForgeryAuthorizationFilter));

            registry.Add(new[] { new DataFormatCriteria("RSS") }, typeof(RssResultActionFilter));
            registry.Add(new[] { new DataFormatCriteria("ATOM") }, typeof(AtomResultActionFilter));

            ControllerActionCriteria listActionsCriteria = new ControllerActionCriteria();
            listActionsCriteria.AddMethod<AreaController>(a => a.Find());
            listActionsCriteria.AddMethod<AreaController>(a => a.FindQuery(null));
            listActionsCriteria.AddMethod<AreaController>(a => a.BlogML(null));
            listActionsCriteria.AddMethod<AreaController>(a => a.BlogMLSave(null, null, null));
            listActionsCriteria.AddMethod<CommentController>(c => c.List(0, 0));
            listActionsCriteria.AddMethod<PostController>(p => p.List(null, 0, null));
            listActionsCriteria.AddMethod<PostController>(p => p.ListByArchive(0, null));
            listActionsCriteria.AddMethod<PostController>(p => p.ListByArea(null, 0, null, null));
            listActionsCriteria.AddMethod<PostController>(p => p.ListBySearch(null, 0, null, null));
            listActionsCriteria.AddMethod<PostController>(p => p.ListByTag(null, 0, null, null));
            listActionsCriteria.AddMethod<PostController>(p => p.ListWithDrafts(null, 0));
            registry.Add(new[] { listActionsCriteria }, typeof(ArchiveListActionFilter));
            registry.Add(new[] { listActionsCriteria }, typeof(PageSizeActionFilter));

            ControllerActionCriteria itemActionCriteria = new ControllerActionCriteria();
            itemActionCriteria.AddMethod<PostController>(p => p.Item(null, null));
            itemActionCriteria.AddMethod<PostController>(p => p.AddComment(null, null, null, null, null, null, null));
            registry.Add(new[] { itemActionCriteria }, typeof(CommentingDisabledActionFilter));

            ControllerActionCriteria tagCloudActionCriteria = new ControllerActionCriteria();
            tagCloudActionCriteria.AddMethod<TagController>(t => t.Cloud());
            registry.Add(new[] { tagCloudActionCriteria }, typeof(ArchiveListActionFilter));

            ControllerActionCriteria areaListActionCriteria = new ControllerActionCriteria();
            areaListActionCriteria.AddMethod<PostController>(p => p.Add(null, null));
            areaListActionCriteria.AddMethod<PostController>(p => p.SaveAdd(null, null, null));
            areaListActionCriteria.AddMethod<PostController>(p => p.Edit(null, null));
            areaListActionCriteria.AddMethod<PostController>(p => p.SaveEdit(null, null, null));
            registry.Add(new[] { areaListActionCriteria }, typeof(AreaListActionFilter));

            ControllerActionCriteria pageListActionCriteria = new ControllerActionCriteria();
            pageListActionCriteria.AddMethod<PageController>(p => p.Add(null));
            pageListActionCriteria.AddMethod<PageController>(p => p.SaveAdd(null, null, null));
            pageListActionCriteria.AddMethod<PageController>(p => p.Edit(null));
            pageListActionCriteria.AddMethod<PageController>(p => p.SaveEdit(null, null, null));
            registry.Add(new[] { pageListActionCriteria }, typeof(PageListActionFilter));

            ControllerActionCriteria adminActionsCriteria = new ControllerActionCriteria();
            adminActionsCriteria.AddMethod<AreaController>(a => a.Find());
            adminActionsCriteria.AddMethod<AreaController>(a => a.FindQuery(null));
            adminActionsCriteria.AddMethod<AreaController>(a => a.ItemEdit(null));
            adminActionsCriteria.AddMethod<AreaController>(a => a.ItemSave(null));
            adminActionsCriteria.AddMethod<AreaController>(a => a.BlogML(null));
            adminActionsCriteria.AddMethod<AreaController>(a => a.BlogMLSave(null, null, null));
            adminActionsCriteria.AddMethod<CommentController>(c => c.List(0, 0));
            adminActionsCriteria.AddMethod<CommentController>(c => c.Remove(null, null, null, null));
            adminActionsCriteria.AddMethod<CommentController>(c => c.Approve(null, null, null, null));
            adminActionsCriteria.AddMethod<PageController>(p => p.Add(null));
            adminActionsCriteria.AddMethod<PageController>(p => p.SaveAdd(null, null, null));
            adminActionsCriteria.AddMethod<PageController>(p => p.Edit(null));
            adminActionsCriteria.AddMethod<PageController>(p => p.SaveEdit(null, null, null));
            adminActionsCriteria.AddMethod<PageController>(p => p.Remove(null, null));
            adminActionsCriteria.AddMethod<PluginController>(p => p.List());
            adminActionsCriteria.AddMethod<PluginController>(p => p.Item(Guid.Empty));
            adminActionsCriteria.AddMethod<PostController>(p => p.Add(null, null));
            adminActionsCriteria.AddMethod<PostController>(p => p.SaveAdd(null, null, null));
            adminActionsCriteria.AddMethod<PostController>(p => p.Edit(null, null));
            adminActionsCriteria.AddMethod<PostController>(p => p.SaveEdit(null, null, null));
            adminActionsCriteria.AddMethod<PostController>(p => p.Remove(null, null, null));
            adminActionsCriteria.AddMethod<PostController>(p => p.ListWithDrafts(null, 0));
            adminActionsCriteria.AddMethod<SiteController>(s => s.Dashboard());
            if (container.Resolve<Site>().ID != Guid.Empty)
            {
                adminActionsCriteria.AddMethod<SiteController>(s => s.Item());
            }
            //TODO: (erikpo) Once we have roles other than "authenticated" this should move to not be part of the admin, but just part of authed users
            adminActionsCriteria.AddMethod<UserController>(u => u.ChangePassword(null));
            registry.Add(new[] { adminActionsCriteria }, typeof(AuthorizationFilter));

            ControllerActionCriteria dashboardDataActionCriteria = new ControllerActionCriteria();
            dashboardDataActionCriteria.AddMethod<SiteController>(s => s.Dashboard());
            registry.Add(new[] { dashboardDataActionCriteria }, typeof(DashboardDataActionFilter));

            //TODO: (erikpo) Once we have the plugin model completed, load up all available action filter criteria into the registry here instead of hardcoding them.

            container.RegisterInstance(registry);
        }

        private static void registerModelBinders()
        {
            ModelBinderDictionary binders = System.Web.Mvc.ModelBinders.Binders;

            binders[typeof(ArchiveData)] = new ArchiveDataModelBinder();
            binders[typeof(Area)] = new AreaModelBinder();
            binders[typeof(Comment)] = new CommentModelBinder();
            binders[typeof(PostBase)] = new PostBaseModelBinder();
            binders[typeof(Post)] = new PostModelBinder();
            binders[typeof(Page)] = new PageModelBinder();
            binders[typeof(SearchCriteria)] = new SearchCriteriaModelBinder();
            binders[typeof(Tag)] = new TagModelBinder();
            binders[typeof(UserBase)] = new UserBaseModelBinder();
            binders[typeof(Site)] = new SiteModelBinder();
            binders[typeof(Plugin)] = new PluginModelBinder();
            binders[typeof(AreaSearchCriteria)] = new AreaSearchCriteriaModelBinder();
            binders[typeof(User)] = new UserModelBinder();

            //TODO: (erikpo) Once we have the plugin model completed, load up all available model binders here instead of hardcoding them.
        }

        private void registerViewEngines()
        {
            //TODO: (erikpo) Once we have the plugin model completed, load up all available view engines here instead of hardcoding the single registered IViewEngine from the container
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(getContainer().Resolve<IViewEngine>());
        }

        private void registerControllerFactory()
        {
            ControllerBuilder.Current.SetControllerFactory(getContainer().Resolve<OxiteControllerFactory>());
        }

        private void launchBackgroundServices()
        {
            IUnityContainer container = getContainer();
            Site site = getSite(container);

            if (site.ID != Guid.Empty)
            {
                BackgroundServicesExecutor backgroundServicesExecutor = (BackgroundServicesExecutor)Application["backgroundServicesExecutor"];

                if (backgroundServicesExecutor == null)
                {
                    backgroundServicesExecutor = new BackgroundServicesExecutor(container);

                    Application.Add("backgroundServicesExecutor", backgroundServicesExecutor);

                    backgroundServicesExecutor.Start();
                }
            }
        }

        public void ReloadSite()
        {
            setupSite();
            registerRoutes();
            registerActionFilters();
            launchBackgroundServices();
        }

        private IUnityContainer getContainer()
        {
            return (IUnityContainer)Application["container"];
        }

        private Site getSite()
        {
            return getSite(getContainer());
        }

        private Site getSite(IUnityContainer container)
        {
            if (container == null)
                container = getContainer();

            if (container != null)
                return container.Resolve<Site>();

            return new Site();
        }

        private bool hasSameHostAsRequest(Uri url)
        {
            if (!string.Equals(url.Scheme, Request.Url.Scheme, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!string.Equals(url.Host, Request.Url.Host, StringComparison.OrdinalIgnoreCase))
                return false;

            if (url.Port != Request.Url.Port)
                return false;

            return true;
        }

        private Uri makeHostMatchRequest(Uri url)
        {
            if (url == null) return null;

            UriBuilder builder = new UriBuilder(Request.Url);
            UriBuilder builder2 = new UriBuilder(url);

            builder.Scheme = builder2.Scheme;
            builder.Host = builder2.Host;
            builder.Port = builder2.Port;

            return builder.Uri;
        }

        protected void Application_Start()
        {
            OnStart();
        }

        protected void Application_End()
        {
            OnEnd();
        }
    }
}
