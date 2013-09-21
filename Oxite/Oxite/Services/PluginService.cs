//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Oxite.Model;
using Oxite.Repositories;

namespace Oxite.Services
{
    public class PluginService : IPluginService
    {
        private readonly IPluginRepository repository;

        public PluginService(IPluginRepository repository)
        {
            this.repository = repository;
        }

        #region IPluginService Members

        public IList<IPlugin> GetPlugins()
        {
            return repository.GetPlugins();
        }

        public IPlugin GetPlugin(Guid pluginID)
        {
            return repository.GetPlugin(pluginID);
        }

        public NameValueCollection LoadSettings(IPlugin plugin)
        {
            if (!repository.GetPluginExists(plugin.ID))
            {
                repository.Save(plugin);
            }

            return repository.GetPluginSettings(plugin.ID);
        }

        public void Save(IPlugin plugin)
        {
            repository.Save(plugin, plugin.Settings);
        }

        public void SaveSetting(IPlugin plugin, string name, string value)
        {
            if (!repository.GetPluginExists(plugin.ID))
            {
                repository.Save(plugin);
            }

            repository.SaveSetting(plugin.ID, name, value);
        }

        #endregion
    }
}
