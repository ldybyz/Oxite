//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Linq;
using Oxite.Model;

namespace Oxite.Repositories
{
    public interface IPageRepository
    {
        Page GetPage(string name, Guid parentID);
        IQueryable<Page> GetPages();
        IQueryable<Page> GetChildren(Page parent);
        void Save(Page page);
        void Remove(Page page);
    }
}
