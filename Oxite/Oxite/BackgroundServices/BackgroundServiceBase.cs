//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Specialized;
using Oxite.Model;
using Oxite.Services;

namespace Oxite.BackgroundServices
{
    public class BackgroundServiceBase : IPlugin
    {
        private NameValueCollection settings;
        private readonly IPluginService pluginService;

        public BackgroundServiceBase(IPluginService pluginService)
        {
            this.pluginService = pluginService;
        }

        #region IPlugin Members

        public Guid ID
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public string Category
        {
            get;
            protected set;
        }

        public NameValueCollection Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = pluginService.LoadSettings(this);

                    if (settings.Count == 0)
                    {
                        OnInitializeSettings();

                        pluginService.Save(this);
                    }
                }

                return settings;
            }
        }

        public bool Enabled
        {
            get;
            set;
        }

        public void RefreshSettings()
        {
            IPlugin plugin = pluginService.GetPlugin(this.ID);

            if (plugin != null)
                Enabled = plugin.Enabled;

            settings = null;
        }

        #endregion

        protected virtual void OnInitializeSettings()
        {
        }

        protected string GetSetting(string name)
        {
            if (Array.IndexOf(Settings.AllKeys, name) == -1)
            {
                throw new ArgumentException(string.Format("No setting with name '{0}' could be found", name), "name");
            }

            return Settings[name];
        }

        protected void SaveSetting(string name, string value)
        {
            pluginService.SaveSetting(this, name, value);

            Settings[Name] = value;
        }
    }
}
