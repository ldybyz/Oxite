//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Oxite.Model;

namespace Oxite.Services
{
    public interface IPluginService
    {
        IList<IPlugin> GetPlugins();
        IPlugin GetPlugin(Guid pluginID);
        NameValueCollection LoadSettings(IPlugin plugin);
        void Save(IPlugin plugin);
        void SaveSetting(IPlugin plugin, string name, string value);
    }
}
