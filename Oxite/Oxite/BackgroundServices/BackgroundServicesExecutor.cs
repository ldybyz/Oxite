//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Oxite.BackgroundServices
{
    public class BackgroundServicesExecutor
    {
        private IUnityContainer container;
        private readonly List<BackgroundServiceExecutor> executors;

        public BackgroundServicesExecutor(IUnityContainer container)
        {
            this.container = container;

            //TODO: (erikpo) Once we have a plugin framework in place to load types from different assemblies, get rid of the below hardcoded values and load up all background services dynamically
            executors = new List<BackgroundServiceExecutor>(2);
            executors.Add(new BackgroundServiceExecutor(container, typeof(SendTrackbacks)));
            executors.Add(new BackgroundServiceExecutor(container, typeof(SendMessages)));
        }

        public void Start()
        {
            foreach (BackgroundServiceExecutor executor in executors)
            {
                executor.Start();
            }
        }

        public void Stop()
        {
            foreach (BackgroundServiceExecutor executor in executors)
            {
                executor.Stop();
            }
        }
    }
}
