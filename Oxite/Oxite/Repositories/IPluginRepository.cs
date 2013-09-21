//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Oxite.Model;

namespace Oxite.Repositories
{
    public interface IPluginRepository
    {
        IList<IPlugin> GetPlugins();
        IPlugin GetPlugin(Guid pluginID);
        bool GetPluginExists(Guid pluginID);
        void Save(IPlugin plugin);
        void Save(IPlugin plugin, NameValueCollection settings);
        NameValueCollection GetPluginSettings(Guid pluginID);
        void SaveSetting(Guid pluginID, string name, string value);
    }
}
