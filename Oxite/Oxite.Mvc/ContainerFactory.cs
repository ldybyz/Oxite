//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Oxite.Infrastructure;
using Oxite.Model;
using Oxite.Mvc.Infrastructure;
using Oxite.Mvc.Skinning;
using Oxite.Routing;
using Oxite.Services;
using Oxite.Validation;
using System.Web.Hosting;

namespace Oxite.Mvc
{
    public class ContainerFactory
    {
        public IUnityContainer GetOxiteContainer()
        {
            IUnityContainer parentContainer = new UnityContainer();

            parentContainer
                .RegisterInstance(new AppSettingsHelper(ConfigurationManager.AppSettings))
                .RegisterInstance(RouteTable.Routes)
                .RegisterInstance(HostingEnvironment.VirtualPathProvider)
                .RegisterInstance("RegisterRoutesHandler", typeof(MvcRouteHandler));

            foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
            {
                parentContainer.RegisterInstance(connectionString.Name, connectionString.ConnectionString);
            }

            parentContainer
                .RegisterType<ISiteService, SiteService>()
                .RegisterType<IPluginService, PluginService>()
                .RegisterType<IUserService, UserService>()
                .RegisterType<ITagService, TagService>()
                .RegisterType<IPostService, PostService>()
                .RegisterType<ITrackbackOutboundService, TrackbackOutboundService>()
                .RegisterType<IMessageService, MessageService>()
                .RegisterType<IPageService, PageService>()
                .RegisterType<IAreaService, AreaService>()
                .RegisterType<ILocalizationService, LocalizationService>()
                .RegisterType<ILanguageService, LanguageService>()
                .RegisterType<IActionInvoker, OxiteControllerActionInvoker>()
                .RegisterType<IFormsAuthentication, FormsAuthenticationWrapper>()
                .RegisterType<ISkinEngine, VirtualPathProviderSkinEngine>()
                .RegisterType<IViewEngine, OxiteViewEngine>()
                .RegisterType<IValidationService, ValidationService>()
                .RegisterType<IRegularExpressions, RegularExpressions>()
                .RegisterType<IValidator<Comment>, CommentValidator>()
                .RegisterType<IValidator<UserBase>, UserBaseValidator>()
                .RegisterType<IValidator<PostSubscription>, PostSubscriptionValidator>()
                .RegisterType<IValidator<User>, UserValidator>()
                .RegisterType<IValidator<Site>, SiteValidator>()
                .RegisterType<IValidator<Area>, AreaValidator>()
                .RegisterType<IValidator<Post>, PostValidator>()
                .RegisterType<IValidator<Page>, PageValidator>()
                .RegisterType<IRegisterRoutes, OxiteRoutes>(
                    new InjectionConstructor(
                        new ResolvedParameter<RouteCollection>(),
                        new ResolvedParameter<AppSettingsHelper>(),
                        new ResolvedParameter<Site>(),
                        new ResolvedParameter<IAreaService>(),
                        new ResolvedParameter<Type>("RegisterRoutesHandler")
                        ));

            IUnityContainer oxiteContainer = parentContainer.CreateChildContainer();

            UnityConfigurationSection config = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            config.Containers.Default.Configure(oxiteContainer);

            oxiteContainer.RegisterInstance(oxiteContainer);

            return oxiteContainer;
        }
    }
}
