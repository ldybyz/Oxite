//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Oxite.Model;
using Oxite.Repositories;

namespace Oxite.LinqToSqlDataProvider
{
    public class PluginRepository : IPluginRepository
    {
        private OxiteLinqToSqlDataContext context;
        private Guid siteID;

        public PluginRepository(OxiteLinqToSqlDataContext context, Site site)
        {
            this.context = context;
            this.siteID = site.ID;
        }

        #region IPluginRepository Members

        public IList<IPlugin> GetPlugins()
        {
            var query =
                from p in context.oxite_Plugins
                where p.SiteID == siteID
                orderby p.PluginCategory, p.PluginName
                select p;

            return query.Select(
                p =>
                    new Plugin()
                    {
                        ID = p.PluginID,
                        Name = p.PluginName,
                        Category = p.PluginCategory,
                        Enabled = p.Enabled
                    }
                ).Cast<IPlugin>().ToList();
        }

        public IPlugin GetPlugin(Guid pluginID)
        {
            return (
                from p in context.oxite_Plugins
                let pluginSettings = getPluginSettings(p)
                where p.SiteID == siteID && p.PluginID == pluginID
                select new Plugin()
                {
                    ID = p.PluginID,
                    Name = p.PluginName,
                    Category = p.PluginCategory,
                    Enabled = p.Enabled,
                    Settings = projectPluginSettings(pluginSettings)
                }
                ).FirstOrDefault();
        }

        private IQueryable<oxite_PluginSetting> getPluginSettings(oxite_Plugin plugin)
        {
            return
                from ps in context.oxite_PluginSettings
                where ps.SiteID == siteID && ps.PluginID == plugin.PluginID
                select ps;
        }

        private NameValueCollection projectPluginSettings(IQueryable<oxite_PluginSetting> pluginSettings)
        {
            NameValueCollection settings = new NameValueCollection();

            foreach (oxite_PluginSetting setting in pluginSettings)
            {
                settings.Add(setting.PluginSettingName, setting.PluginSettingValue);
            }

            return settings;
        }

        public bool GetPluginExists(Guid pluginID)
        {
            return (
                from p in context.oxite_Plugins
                where p.SiteID == siteID && p.PluginID == pluginID
                select p
                ).FirstOrDefault() != null;
        }

        public void Save(IPlugin plugin)
        {
            Save(plugin, null);
        }

        public void Save(IPlugin plugin, NameValueCollection settings)
        {
            oxite_Plugin foundPlugin = (
                from p in context.oxite_Plugins
                where p.SiteID == siteID && p.PluginID == plugin.ID
                select p
                ).FirstOrDefault();

            if (foundPlugin != null)
            {
                foundPlugin.PluginName = plugin.Name;
                foundPlugin.PluginCategory = plugin.Category;
                foundPlugin.Enabled = plugin.Enabled;
            }
            else
            {
                context.oxite_Plugins.InsertOnSubmit(
                    new oxite_Plugin()
                    {
                        SiteID = siteID,
                        PluginID = plugin.ID,
                        PluginName = plugin.Name,
                        PluginCategory = plugin.Category,
                        Enabled = plugin.Enabled
                    }
                    );
            }

            if (settings != null)
            {
                foreach (string name in settings.AllKeys)
                {
                    oxite_PluginSetting setting = getSetting(plugin.ID, name);

                    if (setting != null)
                    {
                        setting.PluginSettingValue = settings[name];
                    }
                    else
                    {
                        context.oxite_PluginSettings.InsertOnSubmit(
                            new oxite_PluginSetting()
                            {
                                SiteID = siteID,
                                PluginID = plugin.ID,
                                PluginSettingName = name,
                                PluginSettingValue = settings[name]
                            }
                            );
                    }
                }

                //TODO: (erikpo) Cleanup settings that aren't needed anymore
            }

            context.SubmitChanges();
        }

        public NameValueCollection GetPluginSettings(Guid pluginID)
        {
            var query =
                from ps in context.oxite_PluginSettings
                where ps.SiteID == siteID && ps.PluginID == pluginID
                select new { Name = ps.PluginSettingName, Value = ps.PluginSettingValue };

            NameValueCollection settings = new NameValueCollection(query.Count());

            foreach (var item in query)
            {
                settings.Add(item.Name, item.Value);
            }

            return settings;
        }

        public void SaveSetting(Guid pluginID, string name, string value)
        {
            if (pluginID == Guid.Empty) throw new ArgumentException("Guid.Empty is not a valid value", "pluginID");
            if (value == null) throw new ArgumentNullException("value");

            oxite_PluginSetting setting = getSetting(pluginID, name);

            if (setting != null)
            {
                setting.PluginSettingValue = value;
            }
            else
            {
                context.oxite_PluginSettings.InsertOnSubmit(
                    new oxite_PluginSetting()
                    {
                        SiteID = siteID,
                        PluginID = pluginID,
                        PluginSettingName = name,
                        PluginSettingValue = value
                    }
                    );
            }

            context.SubmitChanges();
        }

        private oxite_PluginSetting getSetting(Guid pluginID, string name)
        {
            return (
                from ps in context.oxite_PluginSettings
                where ps.SiteID == siteID && ps.PluginID == pluginID && ps.PluginSettingName == name
                select ps
                ).FirstOrDefault();
        }

        #endregion
    }
}
