//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Mvc;
using Oxite.Model;
using Oxite.Mvc.Extensions;
using Oxite.Mvc.ViewModels;
using Oxite.Services;

namespace Oxite.Mvc.Controllers
{
    public class PluginController : Controller
    {
        private readonly IPluginService pluginService;

        public PluginController(IPluginService pluginService)
        {
            this.pluginService = pluginService;
        }

        public virtual OxiteModelList<IPlugin> List()
        {
            return new OxiteModelList<IPlugin> { List = pluginService.GetPlugins() };
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult Item(Guid pluginID)
        {
            return View(new OxiteModelItem<IPlugin> { Item = pluginService.GetPlugin(pluginID) });
        }

        [ActionName("Item"), AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult SaveItem(Plugin pluginInput)
        {
            //TODO: (erikpo) Validation instead of trycatch

            try
            {
                IPlugin plugin = pluginService.GetPlugin(pluginInput.ID);

                plugin.Enabled = pluginInput.Enabled;

                foreach (string name in pluginInput.Settings)
                {
                    plugin.Settings[name] = pluginInput.Settings[name];
                }

                pluginService.Save(plugin);

                return Redirect(Url.Plugins());
            }
            catch
            {
                return Item(pluginInput.ID);
            }
        }
    }
}
