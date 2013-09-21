//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using Oxite.Extensions;
using Oxite.Model;

namespace Oxite.Mvc.ModelBinders
{
    public class PluginModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection form = controllerContext.HttpContext.Request.Form;

            Plugin plugin = new Plugin();

            string pluginIDValue = form["pluginID"];
            if (!string.IsNullOrEmpty(pluginIDValue))
            {
                Guid pluginID;

                if (pluginIDValue.GuidTryParse(out pluginID))
                {
                    plugin.ID = pluginID;
                }
            }

            bool enabled;
            if (form.GetValues("enabled") != null && bool.TryParse(form.GetValues("enabled")[0], out enabled))
            {
                plugin.Enabled = enabled;
            }

            string[] settingNames = form.GetValues("settingName");
            string[] settingValues = form.GetValues("settingValue");

            if (settingNames != null && settingValues != null && settingNames.Length > 0 && settingValues.Length > 0)
            {
                plugin.Settings = new NameValueCollection();

                for (int i = 0; i < settingNames.Length; i++)
                {
                    plugin.Settings[settingNames[i]] = settingValues[i];
                }
            }

            return plugin;
        }
    }
}
