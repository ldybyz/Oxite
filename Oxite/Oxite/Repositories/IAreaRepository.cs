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
    public interface IAreaRepository
    {
        Area GetArea(Guid id);
        Area GetArea(string areaName);
        IQueryable<Area> GetAreas();
        void Save(Area area, Site site);
    }
}
