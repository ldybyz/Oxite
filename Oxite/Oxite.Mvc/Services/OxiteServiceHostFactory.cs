//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.ServiceModel.Samples.XmlRpc;
using Oxite.Infrastructure;
using Oxite.Model;
using Oxite.Services;

namespace Oxite.Mvc.Services
{
    public class OxiteServiceHostFactory : ServiceHostFactory
    {
        private readonly IUnityContainer container;

        public OxiteServiceHostFactory()
        {
            // This service runs parallel to the main Oxite application so it needs to set up and configure it's own Unity container
            container = new ContainerFactory().GetOxiteContainer();
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            AppSettingsHelper appSettings = container.Resolve<AppSettingsHelper>();
            Site currentSite = container.Resolve<ISiteService>().GetSite(appSettings.GetString("SiteName", "Oxite"));

            container.RegisterInstance(currentSite);

            Uri cannonicalServiceUrl = new UriBuilder(currentSite.Host) { Path = baseAddresses[0].AbsolutePath }.Uri;

            return new XmlRpcServiceHost(serviceType, container, cannonicalServiceUrl);
        }
    }
}
