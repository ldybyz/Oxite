//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using Oxite.Model;
using Oxite.Validation;

namespace Oxite.Services
{
    public interface IAreaService
    {
        Area GetArea(string areaName);
        Area GetArea(Guid id);
        IList<Area> GetAreas();
        IList<Area> FindAreas(AreaSearchCriteria criteria);
        void AddArea(Area area, out ValidationStateDictionary validationState, out Area newArea);
        void AddArea(Area area, Site site, out ValidationStateDictionary validationState, out Area newArea);
        void EditArea(Area area, out ValidationStateDictionary validationState);
    }
}
