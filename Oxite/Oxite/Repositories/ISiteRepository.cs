//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using Oxite.Model;

namespace Oxite.Repositories
{
    public interface ISiteRepository
    {
        Site GetSite(string name);
        void Save(Site site);
    }
}
