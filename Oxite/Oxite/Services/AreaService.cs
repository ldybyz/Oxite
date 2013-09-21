//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Model;
using Oxite.Repositories;
using Oxite.Validation;

namespace Oxite.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository repository;
        private readonly IValidationService validator;

        public AreaService(IAreaRepository repository, IValidationService validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        #region IAreaService Members

        public Area GetArea(Guid id)
        {
            return repository.GetArea(id);
        }

        public Area GetArea(string areaName)
        {
            return repository.GetArea(areaName);
        }

        public IList<Area> GetAreas()
        {
            return repository.GetAreas().ToList();
        }

        public IList<Area> FindAreas(AreaSearchCriteria criteria)
        {
            return
                repository.GetAreas().ToList()
                .Where(a => 
                    a.Name.IndexOf(criteria.Name, StringComparison.OrdinalIgnoreCase) != -1 ||
                    a.DisplayName.IndexOf(criteria.Name, StringComparison.OrdinalIgnoreCase) != -1
                    ).ToList();
        }

        public void AddArea(Area area, out ValidationStateDictionary validationState, out Area newArea)
        {
            AddArea(area, null, out validationState, out newArea);
        }

        public void AddArea(Area area, Site site, out ValidationStateDictionary validationState, out Area newArea)
        {
            validationState = new ValidationStateDictionary();

            validationState.Add(typeof(Area), validator.Validate(area));

            if (!validationState.IsValid)
            {
                newArea = null;

                return;
            }

            repository.Save(area, site);

            newArea = GetArea(area.ID);
        }

        public void EditArea(Area area, out ValidationStateDictionary validationState)
        {
            validationState = new ValidationStateDictionary();

            validationState.Add(typeof(Area), validator.Validate(area));

            if (!validationState.IsValid)
            {
                return;
            }

            repository.Save(area, null);
        }

        #endregion
    }
}
