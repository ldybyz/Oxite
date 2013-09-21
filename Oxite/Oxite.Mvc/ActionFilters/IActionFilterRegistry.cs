//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Oxite.Mvc.ActionFilters
{
    public interface IActionFilterRegistry
    {
        void Clear();
        void Add(IEnumerable<IActionFilterCriteria> criteria, Type filterType);
        FilterInfo GetFilters(ActionFilterRegistryContext context);
    }
}
