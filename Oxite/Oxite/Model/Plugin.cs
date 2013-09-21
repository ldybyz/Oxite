//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Specialized;

namespace Oxite.Model
{
    public class Plugin : IPlugin
    {
        #region IPlugin Members

        public Guid ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Category
        {
            get;
            set;
        }

        public NameValueCollection Settings
        {
            get;
            set;
        }

        public bool Enabled
        {
            get;
            set;
        }

        public void RefreshSettings()
        {
        }

        #endregion
    }
}
