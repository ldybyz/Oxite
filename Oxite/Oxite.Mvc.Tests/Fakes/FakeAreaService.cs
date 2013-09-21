//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Model;
using Oxite.Services;
using Oxite.Validation;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeAreaService : IAreaService
    {
        public Dictionary<string, Area> StoredAreas { get; set; }

        public FakeAreaService()
        {
            this.StoredAreas = new Dictionary<string, Area>();
        }

        #region IAreaService Members

        public Area GetArea(Guid id)
        {
            return this.StoredAreas.Where(kvp => kvp.Value.ID == id).FirstOrDefault().Value;
        }

        public Area GetArea(string areaName)
        {
            if (this.StoredAreas.ContainsKey(areaName))
                return this.StoredAreas[areaName];
            else
                return null;
        }

        public IList<Area> GetAreas()
        {
            return this.StoredAreas.Select(kvp => kvp.Value).ToList();
        }

        public IList<Area> FindAreas(AreaSearchCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public void AddArea(Area area, out ValidationStateDictionary validationState, out Area newArea)
        {
            throw new NotImplementedException();
        }

        public void AddArea(Area area, Site site, out ValidationStateDictionary validationState, out Area newArea)
        {
            throw new NotImplementedException();
        }

        public void EditArea(Area area, out ValidationStateDictionary validationState)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
