//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Oxite.Extensions
{
    public static class IUnityContainerExtensions
    {
        public static void LoadContainerConfigByName(this IUnityContainer container, string containerName)
        {
            UnityConfigurationSection config = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");

            config.Containers[containerName].Configure(container);
        }
    }
}
