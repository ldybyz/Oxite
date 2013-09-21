//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using Oxite.Model;
using Oxite.Repositories;
using Oxite.Validation;

namespace Oxite.Services
{
    public class SiteService : ISiteService
    {
        private readonly ISiteRepository repository;
        private readonly IValidationService validator;

        public SiteService(ISiteRepository repository, IValidationService validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        #region ISiteService Members

        public Site GetSite(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return repository.GetSite(name);
        }

        public void AddSite(Site site, out ValidationStateDictionary validationState, out Site newSite)
        {
            validationState = new ValidationStateDictionary();

            validationState.Add(typeof(Site), validator.Validate(site));

            if (!validationState.IsValid)
            {
                newSite = null;

                return;
            }

            repository.Save(site);

            newSite = repository.GetSite(site.Name);
        }

        public void EditSite(Site site, out ValidationStateDictionary validationState)
        {
            validationState = new ValidationStateDictionary();

            validationState.Add(typeof(Site), validator.Validate(site));

            if (!validationState.IsValid)
            {
                return;
            }

            repository.Save(site);
        }

        #endregion
    }
}
