//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using Oxite.Model;
using Oxite.Validation;

namespace Oxite.Services
{
    public interface ISiteService
    {
        Site GetSite(string name);
        void AddSite(Site site, out ValidationStateDictionary validationState, out Site newSite);
        void EditSite(Site site, out ValidationStateDictionary validationState);
    }
}
